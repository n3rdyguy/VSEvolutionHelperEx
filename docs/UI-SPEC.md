# Tooltip UI specification

How this mod draws things into Vampire Survivors' UI, and what the game does that forced each
decision. Everything here was paid for by a bug; the rationale is kept because the rules look
arbitrary without it.

Game: **VS 1.15.x** (tested 1.15.114) and the **1.16 public beta**, Unity **6000.0.62f1**,
BepInEx 6 BE IL2CPP. Every surface here was re-confirmed on 1.16 with no source changes; the
only thing that moved was the game's own canvas sorting orders, which is exactly what
[§2](#2-sorting-and-the-dimmer) measures rather than assumes.

| Section | Covers |
|---------|--------|
| [1. Canvases](#1-canvases) | Where UI lives, menu vs in-run |
| [2. Sorting](#2-sorting-and-the-dimmer) | Drawing above the screen dimmer |
| [3. Positioning](#3-positioning) | The coordinate system and how to measure a dock |
| [4. Docked popups](#4-docked-popups) | The shared panel every list page uses |
| [5. Row registry](#5-row-registry) | Hover wiring for list rows |
| [6. Sprites](#6-sprites-and-async-atlases) | Frame lookups, DLC art, async atlas loads |
| [7. Patching](#7-patching-conventions) | Harmony conventions against IL2CPP |
| [8. Data quirks](#8-data-quirks) | Sparse records, VOID, nullable enums, masked labels |
| [9. Page inventory](#9-page-inventory) | Every surface, its hook and its dock |

---

## 1. Canvases

Two root canvases matter, and only one of them exists at a time.

| Path | When | Notes |
|------|------|-------|
| `UI/Canvas - App/Safe Area` | Menus | Canvas `Canvas - App`, sorting layer **UI**; order **60** on 1.15, ~**10000** on 1.16 |
| `GAME UI/Canvas - Game UI/Safe Area` | During a run | Modal views hang off here |

**A popup parented to a canvas that is not in the scene is never drawn.** It is created, it is
positioned, it logs correctly, and nothing appears. This is the failure mode for anything shown
mid-run: the menu canvas simply is not there.

### Mid-run, no game canvas can draw a popup at all

The Game UI Safe Area is **`activeInHierarchy` and scaled to zero** during a run, as is the
`AspectMask` canvas above it. A popup parented there inherits that zero scale and renders
nothing while its position, rect, alpha and sorting order all read perfectly correct. Setting
our own `localScale` to one does not help: lossy scale is parent x local, and nothing undoes a
zero above you.

So `FindDockParent()` offers exactly two answers - the menu Safe Area when it is in the scene,
and otherwise `OwnDock()`, a `DontDestroyOnLoad` Screen Space Overlay canvas we build once. Its
inner rect is fixed at exactly **1920x1200**, matching the reference space every dock constant
is written in, so placement is identical either way. It carries **no `GraphicRaycaster`**, by
omission rather than by removal, so it cannot swallow clicks on what it covers.

> ### `lossyScale` is stale until the canvas updates
>
> Checking a container's scale *before* building the popup reads whatever the last canvas pass
> left there. Building the popup triggers a canvas update, the scaler recomputes, and only then
> is the number real.
>
> This cost two build cycles and looked like a contradiction in the log: `IsUsableDock` approved
> the Game UI Safe Area, and the diagnostic three lines later reported `parentScale=(0,0,0)` for
> that same object. Call `Canvas.ForceUpdateCanvases()` before reading, and re-check once the
> popup is really in the hierarchy - that is the only honest moment to measure. A popup that
> lands somewhere unable to draw it should relocate itself rather than trust a prior verdict.

Diagnosing this class of bug: **`active` is not the same as `visible`.** Log `activeInHierarchy`,
`CanvasGroup.alpha`, ancestor masks, **and `lossyScale`** together, plus the parent's name and
scale. Any one of them alone reads fine while the tooltip is invisible.

### Hovering the game's own objects: check the canvas before anything else

A `GraphicRaycaster` only tests graphics registered to **its own canvas**, and a graphic registers
to the **nearest enabled `Canvas` above it**. So a nested canvas with no raycaster makes its whole
subtree unhittable - by us and by the game alike.

`ArcanaInfoGroup` is exactly that: `overrideSorting`, `sortingOrder 10`, **no `GraphicRaycaster`**.
Nothing in the arcana info panel had ever been hoverable. Eight build cycles went into the wiring
before the canvas was questioned, because every obvious check passes while this is wrong:

```
icon rect size=(45.50, 30.00)  containsProbePoint=True   cull=False
graphic canvas='.../View - ArcanaMainSelection/ArcanaInfoGroup'
raycasterCanvas='View - ArcanaMainSelection'             sameCanvas=False
```

`sameCanvas=False` is the whole diagnosis. Fix: `AddComponent<GraphicRaycaster>()` on the owning
canvas. It cannot steal clicks from what is behind, because that canvas already draws above it.

**So when a hover never fires on a game object, ask in this order** - each step is one log line and
rules out everything below it:

1. `Graphic.canvas` vs the raycaster's own canvas. If they differ, stop; nothing else matters.
2. `EventSystem.RaycastAll` at the object's own screen point, printing the full hit list. Whatever
   is at `hit[0]` and is not the object is a lid - on the arcana screen, a full-view `BlackFader`
   at `alpha 0.5` that is a raycast target with no handler of its own.
3. `RectangleContainsScreenPoint`, ancestor `Mask` / `RectMask2D` / `CanvasGroup.blocksRaycasts`,
   then `raycastTarget`, `enabled`, `color.a`, `lossyScale`.

> **Convert through the right camera.** `RectTransformUtility.WorldToScreenPoint(null, pos)` on a
> `ScreenSpaceCamera` canvas silently returns world units as pixels, aiming the probe at the corner
> of the screen; its "0 hits" measures nothing but that mistake. Pass `canvas.worldCamera` for any
> canvas that is not `ScreenSpaceOverlay`.
>
> And print scale to **four** decimals. `Canvas - Game UI` runs at `lossyScale 0.0038`, which
> `Vector3.ToString()` renders as `(0.00, 0.00, 0.00)` - indistinguishable from a hidden object,
> and read as one for two build cycles.

Known in-run modal views, all under `GAME UI/Canvas - Game UI/Safe Area`:

```
View - Level Up
View - Merchant
View - ItemFound
View - ArcanaMainSelection
View - WeaponSelection        ← and per-mode variants, e.g. View - TP_WeaponSelection
View - Paused / View - Pause / View - Map
```

> **Never cache a view object and trust it.** More than one weapon-selection view exists at
> once and only one is active; caching the first one found pinned an inactive decoy and made
> "is a modal open" report false for the live screen. Re-resolve whenever the cached object is
> not `activeInHierarchy`. This cost three build cycles.

---

## 2. Sorting and the dimmer

Several pages darken everything behind their panel with a full-screen overlay. Clearing it is
not a matter of sibling order.

**Rules of Unity UI sorting, in the order they are applied:**

1. **Sorting layer** - decided first, and it outranks everything below it.
2. **Sorting order** - only compared between canvases on the *same* layer.
3. **Sibling index** - only within a single canvas.

The dimmer is its own root canvas, so `SetAsLastSibling()` on our popup does nothing to it. The
popup needs its own `Canvas` with `overrideSorting = true` to enter the same contest.

**Two traps, both hit in 1.14 development:**

| Trap | Symptom | Rule |
|------|---------|------|
| A fresh `Canvas` starts on the **Default** sorting layer | Tooltip vanished completely - it was behind the entire UI layer, not merely dimmed | Copy `sortingLayerID` from the parent canvas |
| A fixed order (`parent + 100`) still lost | Tooltip drew but stayed dimmed | Scan active canvases on that layer, take the highest order, sit `+10` above it |
| `Object.Destroy` is deferred to end of frame | Order climbed +10 on every hover and never reset | **Deactivate the outgoing popup before destroying it** |

That third one is worth stating in full, because measuring the top order is what makes it
possible. The old popup is still alive - and still carrying its own sorting canvas - when the new
one measures, so each show stepped above the one it was replacing. Observed on 1.16: `10010` to
`10620` across one session, with nothing to reset it. `sortingOrder` clamps at **32767**, so a
long enough session walks the tooltip back under the dimmer it was raised above. `SetActive(false)`
before `Destroy` makes the existing `activeInHierarchy` filter exclude it in the same frame.

**Do not hardcode an order.** The game's own numbers move between versions - the menu canvas was
order **60** on 1.15 and roughly **10000** on 1.16, with the in-run canvas resolving against a top
of ~**16959**. Measuring is why the popups survived that jump untouched; a fixed `parent + 100`
would have gone straight back to invisible.

A child `Canvas` does **not** inherit the parent's `GraphicRaycaster`. Without one, the popup's
own graphics stop taking pointer events, so a tooltip the cursor moves onto would flicker off.
Add a raycaster alongside the canvas.

Implementation: `ItemTooltipsMod.LiftAboveDimmer()`, `TopOrderOnLayer()` and
`HideDockedPopup()`. With `VerboseLogging` it prints the resolved values:

```
[Tooltip] sorting: layer=UI order=10010 (parent 'Canvas - App' layer=UI order=10000)
```

If a popup is invisible, that line is the first thing to read. If the order **climbs across
hovers** rather than landing on the same number each time, the deactivate-before-destroy above
has regressed.

---

## 3. Positioning

### The coordinate system

The Safe Area is a **1920x1200 reference space**, so `anchoredPosition` runs +/-960 horizontally
and +/-600 vertically, with **+Y up**. It scales with the canvas, so a position expressed this
way is resolution-independent - never place by pixels.

Always anchor to the centre and offset explicitly:

```csharp
rt.anchorMin = new Vector2(0.5f, 0.5f);
rt.anchorMax = new Vector2(0.5f, 0.5f);
rt.pivot     = pivot;
rt.anchoredPosition = offset;
```

> **Do not use fractional anchors.** Changing `anchorMin`/`anchorMax` does not recompute the
> transform in the same frame, so a fractional anchor leaves the popup wherever it already was.

### Measuring a new dock from a screenshot

**Tool:** <https://wsmlby.github.io/webtools/bbox.html> - drop in a screenshot of the page, drag a
box over the empty space the panel should occupy, and read off both absolute and **relative**
coordinates. The relative numbers are the ones to use: they are resolution-independent, which is
exactly what the Safe Area space needs.

Given a box in **relative** screen coordinates (0-1, origin top-left):

```
x = (relX - 0.5) * 1920
y = (0.5 - relY) * 1200
```

Take `relX` from whichever edge is being pinned and `relY` from the top edge. Worked example -
the Music panel, measured at 2560x1600 as `1917,154 - 2437,394` = relative `0.749, 0.097`:

```
x = (0.749 - 0.5) * 1920 = 478
y = (0.5  - 0.097) * 1200 = 484
```

Round to a whole number; sub-pixel precision here is noise.

**Procedure, end to end:**

1. Screenshot the page with a tooltip-worthy row hovered, so the space actually in use is visible.
2. Draw the box over free space. Leave a margin - the panel grows downward as rows are added, and
   a box drawn tight to existing content will overlap once a long entry appears.
3. Decide which corner is fixed (see the pivot table below), and take `relX` from **that** edge:
   left edge for `(0, 1)`, right edge for `(1, 1)`, the box centre for `(0.5, 1)`.
4. Convert with the formulas above.
5. Add the pair as named constants in `ItemTooltipsMod`, next to the existing docks, with the
   measured screen box recorded in the comment. Every existing dock documents its own
   measurement this way, so a later resolution change can be re-derived rather than re-guessed.

### Pivot decides which corner is pinned

The pivot is the point that stays still as content grows, so it is chosen by which direction
there is room to expand.

| Pivot | Pinned | Grows | Use when |
|-------|--------|-------|----------|
| `(0.5, 1)` | Top centre | Down, both ways | Symmetric space, fixed-width panel |
| `(0, 1)` | Top **left** | Right and down | Open space to the right |
| `(1, 1)` | Top **right** | Left and down | Panel near the right screen edge |
| `(0.5, 0.5)` | Centre | All ways | Generic fallback only |

Pinning the top edge matters on its own: the panel then hangs from a fixed line and only its
bottom moves, instead of creeping upward as rows are added.

### The placement types

Five ways a tooltip can be placed. Picking the wrong one is the single most common source of
"it works but looks broken", so choose deliberately.

| Type | Placement | Right when | Wrong when |
|------|-----------|------------|-----------|
| **Docked** | Fixed offset + pivot in Safe Area units | The page is a `ScrollRect`, or rows are dense enough that a following panel would jitter | The page has no reliable free space |
| **Near-icon** | Follows the hovered object's screen position | Sparse icons with room around them - pause equipment, map, level-up | Rows are adjacent; the panel covers the next row |
| **Offset-from-icon** | Near-icon plus a fixed nudge | The panel would otherwise sit on top of the thing being described - weapon selectors | Space is tight on the offset side |
| **Click-through** | Near-icon, raycast disabled | The card underneath must stay selectable - character select | The tooltip's own contents need clicking |
| **Fixed box** | Docked, but sized to a measured rect | Content varies wildly and must not creep sideways - Adventures | Content is short and a full box looks empty |

**How to get each one right:**

- **Docked** - measure with the bbox tool, pick the pivot by which way there is room, name the
  constants. Never parent inside the scroll view; `ShowDockedPopup` handles the parent for you.
- **Near-icon** - clamp to the Safe Area bounds, or a row at the screen edge pushes the panel
  off-screen. Convert the object's position with `WorldToScreenPoint` only for world-space
  objects; UI objects already have a rect.
- **Offset-from-icon** - the offset is in Safe Area units like everything else, and it must be
  applied on the side with room, not a fixed direction.
- **Click-through** - set `raycastTarget = false` on **every** graphic in the panel, not just the
  root. One stray background image swallows the click.
- **Fixed box** - pin left, right and top; let only the bottom move. Anything else and the panel
  creeps sideways as content grows.

**When in doubt, dock it.** Docked is the default for any list page, it is the only type immune
to scroll-mask clipping, and it is the one with shared machinery behind it
([§4](#4-docked-popups), [§5](#5-row-registry)).

---

## 4. Docked popups

`ItemTooltipsMod.ShowDockedPopup(title, description, sprite, rows, sectionHeader, logTag,
offset, pivot)` is the shared panel. One is shown at a time; showing hides the previous.

**Why docked rather than placed next to the hovered row:** every list page is a `ScrollRect`, and
a popup parented inside one is clipped by the viewport mask. The Collections tooltip fell into
exactly this and had to be moved out.

Content model:

| Field | Renders as |
|-------|-----------|
| `Title` | Heading |
| `Description` | Body text, `\n` for line breaks |
| `Sprite` | Icon beside the heading |
| `Rows` | `IconRow` list - icon + label per line |
| `SectionHeader` | A heading above the rows, e.g. `Unlocks:`, `Affects:` |

`GameData.IconRow` is `{ Sprite, Label, IsHeader }`; `IconRow.Header(label)` makes a heading row
inside the list.

A popup with no rows **and** no description is not shown at all - an empty panel is worse than
none.

### Known dock constants

All in `ItemTooltipsMod`, all in Safe Area units.

| Constant | Value | Pivot | Used by |
|----------|-------|-------|---------|
| `SidePanelX` / `SidePanelTopY` | `667, 312` | `(0.5, 1)` | Collections, Unlocks, Bestiary, Power Up, arcana cards |
| `SecretPanelX` / `SecretPanelTopY` | `71, 344` | `(0, 1)` | Secrets |
| `MusicPanelX` / `MusicPanelTopY` | `478, 484` | `(0, 1)` | Music |
| `DockedPopupOffsetX/Y` | `480, 200` | `(0.5, 0.5)` | Fallback only - no page relies on it |

---

## 5. Row registry

`RowTooltipRegistry` (one instance per page) wires hover on list rows. Two behaviours in it were
paid for the hard way and must survive any refactor:

1. **Register the row root as well as the inner icon.** A selected row draws a highlight over its
   contents which swallows the pointer before it reaches the icon. A handler on the root still
   fires, because pointer events bubble to the first ancestor handling them.
2. **Append to `EventTrigger.triggers`, never clear them.** Clearing entries on a row that owns
   its own wiring is how a similar patch broke character select in 1.9.1.
3. **Add nothing to the row's graphics.** No transparent `Image`, and never force `raycastTarget`
   back on where the game switched it off. See below - both make rows unclickable.

Rows are recycled, so listeners are attached once per `GameObject` (tracked by instance ID) while
the entry behind that ID is replaced freely.

### A delayed hide must know which tooltip it belongs to

`PointerExit` cannot hide a tooltip on its own - the pointer legitimately leaves an icon on its way
to the popup - so the hide is deferred a few frames and re-checked. That defers it past the point
where the player has moved on:

1. `PointerExit` on the icon being left schedules a hide.
2. `PointerEnter` on the icon being entered opens **its** tooltip immediately.
3. The first hide runs and calls `HideAllPopups`, destroying the second icon's tooltip.

Moving slowly hides it; moving quickly from one icon to the next is the case that fails. It reads as
a flicker, and the log shows two neighbours alternating - each killing the other's tooltip, the
still-hovered one reopening its own.

**Every deferred hide captures `hoverSerial` and skips if it has changed.** `ShowItemPopup` bumps
it, so the guard holds no matter which path opened the tooltip.

That last part is the trap: an icon can be registered by the HUD scan (`AddHoverToIcon`) *or* by a
patch handing it over (`AddHoverToGameObject`), and **each installs its own delayed hide**. Guarding
one leaves the other free to destroy the same tooltip, and the bug survives a fix that looks
complete. Bump the serial where the tooltip is shown, not at the hover sites.

Two related answers that do **not** work, both tried here:

- Suppressing the hide while the pointer is still inside the icon that scheduled it. The pointer has
  genuinely left that icon; the answer is honestly "no".
- Relying on `mouseOverPopupIndex`. The popup's own hover tracking has not registered anything yet
  when the grace expires.

### `EventTrigger` eats clicks

`EventTrigger` implements **every** pointer interface, `IPointerClickHandler` included. There is
no way to attach only hover. `ExecuteEvents.ExecuteHierarchy` stops at the first object handling
a click, so the moment a raycast lands on an object carrying our trigger, the click is consumed
there - and a handler living on an **ancestor** never runs.

That is exactly the arcana screens, whose click handling sits above the card. It made every
arcana and Darkana unclickable, and took three wrong fixes before the cause was found. Adjusting
what the raycast hits does not help; the trigger only has to be somewhere on the chain.

Two rules follow:

- **Hover needs no raycast target of its own.** `PointerEnter`/`PointerExit` propagate up, so a
  trigger on the row root fires when any child is hovered. That is why the root is registered
  alongside the icon. It works off the row's existing art.
- **Clickable rows hand back a typed `Entry.OnClick`.** Re-dispatching through
  `ExecuteEvents.ExecuteHierarchy` is the general answer but its generic `EventFunction` does not
  survive the IL2CPP interop boundary (`CS1503`). A typed action cannot mis-target, and inert
  list pages simply leave it null.

The control test that isolated this: **turn the tooltip config off.** If the game's own
interaction returns, the tooltip is the cause; no amount of reading the log proves that as
quickly.

### Deferred content

Two hooks exist because registration time is the wrong time to answer:

| Field | Asked at | Why |
|-------|----------|-----|
| `RowsProvider` | Every hover | Numbers that move while the page is open. Power Up prices climb with **every** purchase anywhere on the page, so rows computed once are stale for every upgrade, not just the bought one |
| `SpriteProvider` | Hover, until it answers | Art not yet loaded or not yet drawn. Result is cached once non-null |

`Registry.Refresh()` rebuilds the tooltip already on screen. A purchase changes what the panel
should say while the pointer has not moved, so nothing else would re-trigger it.

---

## 6. Sprites and async atlases

### The lookup chain

`GameData.LoadSprite(frameName, textureName)` tries, in order:

1. `SpriteManager.GetSpriteFast(frame, texture)`
2. `SpriteManager.GetSprite(frame, texture, ignoreExtension: true)`
3. `SpriteManager.GetSprite(frame, ignoreExtension: true)` - frame only
4. The same again with any file extension stripped
5. A short list of common atlases (`weapons`, `items`, `ui`, `characters`, …)
6. **A scan of every sprite already in memory**, whatever atlas holds it

Step 6 exists because `SpriteManager` only answers for atlases whose names it knows, and **DLC
art is not among them**. The Bestiary drew `kappa_i01` on screen while every lookup for it
returned nothing.

### Asynchronous loads

An enemy or track whose atlas is not loaded at all has that atlas requested - **one at a time**,
rather than preloading art nobody has looked at. The request is asynchronous, so the icon appears
from the **second hover onward**. This is expected behaviour, not a bug.

`GameData.SpriteGeneration` increments when new art arrives.

### Caching rules

Cache hits *and* misses, but not the same way:

```csharp
if (Cache.TryGetValue(key, out Sprite hit) && hit != null) return hit;
if (Misses.TryGetValue(key, out int gen) && gen == GameData.SpriteGeneration) return null;
```

A miss is only ever "the atlas was not in memory **yet**". Keying misses to the generation means
the next hover after an atlas arrives asks again, instead of showing a blank forever.

### Resolve on hover, not on build

The Bestiary page resolved a sprite per row as it built: 217 rows, each miss costing the full
chain above ending in a scan of memory, for icons all but one of which are never looked at. The
page was visibly slow to open. **Only the hovered row resolves.**

Where the page draws art the mod cannot otherwise find, capture it as it is drawn, keyed by id -
that is how DLC Bestiary portraits are recovered.

---

## 7. Patching conventions

### Instance-only postfix

Read data back off `__instance` rather than taking it from the patched call's arguments. IL2CPP
argument marshalling across overloads is where this gets fragile; the instance fields are stable.

```csharp
public static void SetData_Postfix(SomeItemUI __instance)
{
    if ((Object)(object)__instance == (Object)null) return;
    ...
}
```

### Patch every overload, by name

Which `SetData` overload binds a row depends on where it was drawn. `ArcanaCardUI` has three; a
card bound through an unpatched one is silently inert. Loop over the methods instead of
resolving one signature:

```csharp
foreach (var m in typeof(ArcanaCardUI).GetMethods(BindingFlags.Instance | BindingFlags.Public))
{
    if (m.Name != "SetData") continue;
    harmony.Patch(m, postfix: new HarmonyMethod(typeof(X), nameof(SetData_Postfix)));
}
```

Log the count, and warn when it is zero - a renamed method otherwise fails silently.

### Patch what changes the data too

`SetData` binds a row; something else mutates it. The Power Up page needed `UpdateAfterPurchase`
on the row plus `Purchase` / `RefundPowerUps` / `ResetAll` on the page, the latter calling
`Registry.Refresh()`.

### Do not patch `CharacterItemUI.SetData`

It broke character select in 1.9.1. See CHANGELOG.

### Failure is silent by design

Every patch body is wrapped; a throw inside a postfix on a UI method can take the page down.
Warn once, show nothing, let the game carry on.

### `__instance` is only trustworthy in the postfix that was handed it

Do not stash an instance, and do not read fields off `__instance` in a per-frame patch such as
`LateUpdate`. `ArcanaInfoPanel.LateUpdate` runs on something that is not the live panel:
`_affectedWeapons.Count` read back as `-1595577652`, and `_AffectedWeaponGroup.transform` threw
`The component is not attached to any game object!` as a **native** exception. The `catch` logged
it and the process still died - a `try` around interop does not contain everything.

Where state is needed later, capture plain values in the good postfix and navigate the scene
afterwards (`GameObject.Find` on a landmark, then walk down).

### Find what is drawn, not what was built

A panel may build into one container and draw from another. `ArcanaInfoPanel` keeps a fixed row
and a dynamic grid and switches past `_MaxWeaponsBeforeGrid`, which is **3** - so the grid is the
normal case. The unused container stays `activeInHierarchy` at `scale=(0,0,0)` with its graphics
disabled, and a disabled `Graphic` is neither drawn nor raycast against.

Prune inactive and zero-scaled branches whole - scale is inherited - then group the enabled
`Image`s by parent and take the group whose count matches what was queued. That answers "which
container won" without naming either one, and survives the panel being rearranged.

---

## 8. Data quirks

### Sparse records

The `SecretData` handed to `SecretItemUI` arrives with every reward field null or `VOID`. The
populated record lives in the catalog keyed by type (`DataManager.AllSecrets`), with a raw-JSON
fallback behind that. Same shape for the custom-merchant catalog.

`AchievementData` is the opposite: its reward fields are **plain strings**
(`characterToUnlock`, `weaponToUnlock`, …), so they feed `GameData.BuildRewardRows` directly with
no fallback needed.

### The enum runs ahead of the data table

An enum value existing does **not** mean the game ships a record for it. On 1.15 `ArcanaType`
declares 22 Darkanas while `DataManager.AllArcanas` holds records for 12 - matching the 22 cards
in the deck against 12 officially released. The ten without records are a later version's
content, one of them still named `D07_tbd_bouncy`:

```
D02  D04  D07  D09  D11  D14  D15  D16  D17  D20
```

`Enum.IsDefined` therefore proves nothing about whether anything can be shown. Check
`GameData.HasArcanaRecord` (i.e. `GetArcanaData(type) != null`) and **say so in the panel** -
these render their name, their art and *"Not in this version of the game yet."* A panel holding
a bare name reads as a broken tooltip, which is the wrong thing to tell the player when the
tooltip is working and the game has nothing to say.

Distinguish three causes before concluding anything, because they are identical from the UI:

| Reading | Means |
|---------|-------|
| `no record in AllArcanas` | The game never ships it. Nothing to fix |
| `record present, weapons=0 items=0` | Blank record. Nothing to show |
| `record present, weapons=12 …` | We hold the data and misread it. **Our bug** |

Empty is not automatically a lookup failure, and a table that never grows is not an async
problem - check whether it grows before building a retry for it.

### `VOID` and other sentinels

An unset typed id reads back as the string `"VOID"`, sometimes `"0"`, sometimes empty. Check all
three (`GameData.IsVoidValue`) rather than testing for null.

This is also load-bearing for spoilers: a **face-down arcana card** reads `VOID`, so skipping
those is what stops a tooltip revealing an unflipped pick.

### Nullable enums

Fields like `MusicData.unlockedByStage` are `Il2CppSystem.Nullable<StageType>`. Use
`.HasValue` / `.Value`, then still check the value against the sentinels above.

### Labels: prefer the row, except when the page is hiding something

The general rule is to read the label the game already drew - it is localized, and it is correct
for variants. Two exceptions, both real:

| Page | Problem | Resolution |
|------|---------|-----------|
| Bestiary | `bName` covers a whole family, so a variant row read "Spirit" where the game printed "Calamity" | Prefer the **row label**; keep `bName` for undiscovered rows |
| Music | A locked row's label is a run of dashes, so every locked track was named `-----` | Prefer **`MusicData.title`**; row label is the fallback (config `MusicSpoilers`) |

Detect a mask by content, not by a literal: a label with no letter or digit in it is a
placeholder.

### Localize components are not the text

`PowerUpItemUI.Title` is an I2 `Localize` component, not the label. The rendered string lives on
the `TextMeshProUGUI` sharing its GameObject, and only that has been through localization.

### Fields that lie

| Field | Reads | Use instead |
|-------|-------|-------------|
| `PowerUpItemUI._currentLevel` | `0` for every upgrade regardless of what is owned | `PlayerStats.GetOwnedPowerUps()[type]._Level` |
| `EnemyData` display name | Does not exist - there is no `enemyLang/` prefix in use | `EnemyItemUI._Name.text` |

### Text measurement

TMP keeps drawing past a clamped height. Clamping a section's measured height while the text
overflows it makes the panel measure shorter than its own contents, so the body runs out of the
bottom and long rows overlap the row beneath. Clamp layout if needed, never the measurement.

---

## 9. Page inventory

Every surface, its hook, and where its tooltip lands.

| Page | Hook | Source file | Dock |
|------|------|-------------|------|
| Pause equipment | `EquipmentIconPaused.SetData` | `EquipmentIconPatches.cs` | Near icon |
| Level Up | `LevelUpItemUI` | `LevelUpItemUIPatches.cs` | Near icon |
| Pause map | `MapManager` icons | `MapPatches.cs` | Near icon |
| Grimoire | formula icons | `GrimoirePatches.cs` | Near icon |
| Collections | `CollectionItemUI` | `CollectionSelectPatches.cs` | Side panel |
| Character select | grid cards | `CharacterSelectPatches.cs` | Near card, click-through |
| Adventure select | adventure cards | `AdventureSelectPatches.cs` | Fixed box |
| Stage select | relic icons | `StageSelectPatches.cs` | Near icon |
| Weapon selectors | selector cells | `WeaponSelectionPatches.cs` | Offset from icon |
| Secrets | `SecretItemUI.SetData` | `SecretsPatches.cs` | `(71, 344)` pivot `(0,1)` |
| Bestiary | `EnemyItemUI.SetData` | `BestiaryPatches.cs` | Side panel, top-right pinned |
| Unlocks | `AchievementDataUI.SetData` | `AchievementPatches.cs` | Side panel |
| Power Up | `PowerUpItemUI.SetData` + `UpdateAfterPurchase` | `PowerUpPatches.cs` | Side panel |
| Arcana cards | `ArcanaCardUI.SetData` x3 | `ArcanaCardPatches.cs` | Side panel |
| Music | `TrackItemUI.SetData` | `MusicPatches.cs` | `(478, 484)` pivot `(0,1)` |

Registration order lives in `ItemTooltipsMod.Apply(harmonyInstance)`; each entry is individually
try-wrapped so one failing page cannot stop the rest.

---

## 10. Diagnosing a tooltip that does not appear

In order, because each step rules out the one below it.

1. **Is the patch applied?** `LogOutput.log` should have `[Page] Patched X.SetData(N args)`.
   Zero patched means the method was renamed by a game update.
2. **Is the deployed DLL the one you built?** Compare md5s on both sides, and check the log's
   own mtime against the deploy. Two rounds of this session were spent reading a log written
   before the build under test existed.
3. **Did the hover arrive?** `[Page] hover title='…' desc=…ch rows=…`. Nothing here means the
   pointer never reached the row - a different bug entirely from a panel that declined to draw.
4. **Is the row registered?** `Page: registered N rows for X`. `0 rows` and no description means
   the data lookup failed, not the UI - and see §8 on why that may be correct.
5. **Is the popup created?** `[Page] popup rect=… anchored=…`. If this prints and nothing is on
   screen, the problem is sorting, parenting or scale.
6. **Can the parent draw?** `visibility: active=… alpha=… maskedByAncestor=… scale=… parentScale=…`.
   A `scale=(0,0,0)` is the mid-run Safe Area; see §1. Active is not visible.
7. **Is it off screen?** Compare `anchored=` against +/-960 x +/-600.

**When three rounds have not fixed it, stop changing behaviour and ship a build that only
measures.** Every wrong turn in this session came from acting on the most plausible cause rather
than the one the log named; every fix came from a build whose only job was to separate two
causes that looked identical from outside.

---

## 11. Related docs

| Doc | Use |
|-----|-----|
| [game-api/README.md](../game-api/README.md) | Decompiled game types and how to regenerate them |
| [USER-GUIDE.md](USER-GUIDE.md) | Player-facing feature reference |
| [SMOKE-TEST.md](SMOKE-TEST.md) | Pre-release checklist |
