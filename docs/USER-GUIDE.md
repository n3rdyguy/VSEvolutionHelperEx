# VS Evolution Helper — User guide

**Plugin:** VS Evolution Helper (BepInEx)  
**Current version:** 1.13.0 (plus Music and arcana-card tooltips, unreleased)  
**Game:** Vampire Survivors 1.15 / Unity 6  

This guide describes **every player-facing feature**. For install steps, see the root [README](../README.md). For release history, see [CHANGELOG](../CHANGELOG.md).

---

## 1. What this mod does

Adds **hover tooltips** and a **Stage Guide** so you can see:

- How weapons evolve (all known recipes, not just one)
- Which arcanas affect a weapon/item
- What map icons and stage relics are
- Character starters, evo paths, outfits, and stats before a run
- Adventure cast/weapon summaries
- Stage tips, quirks, and relic lists without leaving stage select

It does **not** change game balance, unlocks, or saves by itself (aside from optional local save tooling under `scripts/`, not part of the plugin DLL).

---

## 2. Feature map

```
┌─ In a run ─────────────────────────────────────────────┐
│  Pause equipment / HUD icons → evo + arcana tooltips   │
│  Level Up choices → same (after real hover)            │
│  Pause map → relic / pickup tooltips                    │
│  Merchant / similar UI → weapon & item tooltips        │
│  Weapon selector screens → weapon tooltips             │
└────────────────────────────────────────────────────────┘

┌─ Collection ───────────────────────────────────────────┐
│  Grimoire evolution formulas → icon tooltips           │
│  Collection grid weapons / items / arcanas             │
│  Secrets → what each one unlocks                       │
│  Bestiary → enemy stats and resistances                │
│  Unlocks → rewards and requirements                    │
└────────────────────────────────────────────────────────┘

┌─ Menus ────────────────────────────────────────────────┐
│  Power Up → what an upgrade costs to finish            │
│  Music → composer, source, how a track is unlocked     │
└────────────────────────────────────────────────────────┘

┌─ Pre-run ──────────────────────────────────────────────┐
│  Character select → rich character tooltip             │
│  Adventure select → cast / weapon summary              │
│  Stage select → Relics in stage tooltips               │
│  Stage select → Music | Guide (Stage Guide panel)      │
└────────────────────────────────────────────────────────┘
```

---

## 3. In-run tooltips

### 3.1 Weapons & passives

**Where:** Pause equipment icons, registered HUD icons, merchant icons, collection/grimoire.

**Shows:**

- Localized name + description  
- **Evolutions** section: one row per recipe  
  - Base icon + passive(s) + `→` evolved icon + name  
  - Multi-path weapons list every recipe (not a single combined line)  
- Click an icon in the formula for a **nested** detail tooltip (click stack when interactive)

**Not shown as a raw key:** localization falls back cleanly if I2 is missing a string.

### 3.2 Arcana

**Where:** On a weapon/item tooltip that has linked arcanas.

**Shows:**

- Header **Arcana: (click for details)**  
- Card sprite + name (e.g. `IX - Divine Bloodline`)  
- Click for full description and **Affects** (weapons/items that arcana modifies)

Data comes from `DataManager.AllArcanas` and reverse indexes (which arcanas list this weapon/item).

### 3.3 Level Up

**Where:** Level-up choice icons.

**Behavior:**

- Tooltip only after you **hover** a choice (with `LevelUpHoverDelay`)  
- Opening Level Up with the cursor elsewhere does **not** spam a tooltip  
- Same evolution/arcana content as pause tooltips  
- Kill switch: `Features.LevelUpTooltips`

### 3.4 Pause map

**Where:** Icons spawned on the pause map (relics, pickups, tokens when identified).

**Shows:** Name + description (simple popup; long names resize).

**Merchants** additionally list their **Wares** — what that merchant sells, with icons.
This is read from the game's own merchant data, so it stays correct across patches, and DLC
wares appear only when you have that DLC installed.

The **base Merchant** shows name and description only. Its stock is rolled per encounter
rather than stored anywhere, so there is nothing accurate to list before you open it.

Kill switch: `Features.MapTooltips`.

### 3.5 Weapon selectors (1.10.25+)

**Where:** the mid-run screen that asks you to pick a weapon — Penshin Fatcha's tuna forms
(Para Kooleo's starting weapon), and Arma Dio.

**Shows:** the same weapon tooltip you get elsewhere — name, description, and evolution rows.

Hover the weapon's frame or icon. Clicking still selects the weapon as normal.

Before 1.10.25 these screens had no tooltips at all.

Kill switch: `Features.WeaponSelectionTooltips`.

### 3.6 Arcana cards

**Where:** any arcana card - the mid-run pick, the Collections grid, and the pre-run loadout all
draw the same card.

**Shows:** the arcana's name and description, plus an **Affects:** list of every weapon and
passive it changes, with icons.

The mid-run pick is the point of this. It is the only place in the game an arcana has to be
chosen against a timer, from a card that shows a paragraph and never names the weapons it
actually modifies - so the choice is otherwise made from memory.

A **face-down** card shows nothing. The tooltip cannot reveal a pick the game has not flipped yet.

Kill switch: `Features.ArcanaCardTooltips`.

### 3.7 Keyboard / controller navigation

Every tooltip except the pause map can be reached without a mouse. Move the selection and
hold it; the tooltip appears after `Tooltips.ControllerDwellDelay` (default 0.5s).

| Key | Pad | Does |
|-----|-----|------|
| Arrow keys / WASD | Stick / d-pad | Move selection; leaves mouse mode |
| **Tab** | Y | Enter **interactive mode** on a shown tooltip (its formula icons become focusable); with no tooltip while paused, enter **equipment nav** along your weapon/accessory row; press again to back out |
| **Space** / **Enter** | A | In equipment nav with a tooltip up: enter interactive mode |
| **Backspace** | B | Close the top popup / step back one nested level |
| **Q** / **E** | LB / RB | Stage select: switch **Music ↔ Guide** |

**Covered:** pause equipment, level-up, Collections / Grimoire (including arcana cells),
stage select and the Stage Guide, and weapon selector screens (Arma Dio, Penshin Fatcha).

**Two behaviours worth knowing:**

- **Moving the mouse instantly returns to mouse mode** and closes any nav mode, so a stray
  nudge while navigating by keyboard will drop the tooltip.
- **Level Up is deliberately stricter** — it will not show a tooltip until you give real
  navigation input, because the game auto-selects the first card and that otherwise produced
  an unsolicited popup.

**Known gap — the pause map.** Map icons are hit-tested against the mouse pointer and are not
selectable, so there is no selection for the keyboard to move. Use the mouse there. Fixing it
needs either a synthetic cursor or selectable overlays on the icons.

---

## 4. Grimoire / collection

| UI | Tooltip content |
|----|-----------------|
| Evolution formula icons | Weapon tooltip (evo/arcana) for that icon’s type |
| Collection weapon cells | Same as weapon tooltips |
| Collection items | Item / power-up tooltips |
| Collection arcanas | Arcana name + description |
| **Locked** collection cells | **Unlock:** hint from the game’s achievement text |

### 4.1 Collections tab behavior (1.10.11+)

The Collections tab tooltip does **not** follow the mouse. It is **docked to the right margin**
of the screen, outside the collection grid:

| Behavior | Detail |
|----------|--------|
| Placement | Fixed right-edge dock, upper-middle — always visible, never clipped by the grid’s scroll mask |
| Interactive | The docked panel stays **clickable**: click formula icons for nested detail |
| Hide delay | Hiding is delayed while you move the cursor from a grid cell to the panel |
| Names | Resolved through game data + localization — no raw `itemLang/…` paths |
| Arcana | Headers and names in a darker purple for readability |

**Note:** Depending on layout/registration, some formula rows may only respond on one icon slot; treat that as acceptable UX unless you need full multi-icon hit testing later.

---

## 4.2 List pages: Secrets, Bestiary, Unlocks, Power Up, Music

Five pages whose backing data the game holds but never shows. All behave the same way: hover a
row, a panel appears in the free space beside the list. There is no hover delay on these - the
panel is docked rather than following the cursor, so an eager tooltip costs nothing.

| Page | Hover a row to see |
|------|--------------------|
| **Secrets** | Everything the secret unlocks: character portraits, weapons, relic, arcana, power-up, skins, stage, gold. Renamed characters show both names |
| **Bestiary** | HP, damage, speed, XP, knockback, plus **resistances**, skills, and the stages an enemy appears in. Stats read as ranges where one entry covers a family |
| **Unlocks** | What an achievement grants and what it requires |
| **Power Up** | The level you own, the price of the next one, and what the rest costs |
| **Music** | The composer, the game or DLC a track came from, and how it is unlocked |

**Spoilers are shown by default** on all of them. Three switches turn that off individually:
`SecretSpoilers`, `BestiarySpoilers`, `MusicSpoilers`. With a spoiler switch off, the page's own
masking is kept - undiscovered secrets stay hidden, unkilled enemies show no stats, locked tracks
keep the row of dashes the game draws instead of a name.

**Power Up pricing, in detail.** A level costs its own number times a base price, plus a surcharge
that grows with **every level bought anywhere on the page** - so the single "next" figure the game
shows says almost nothing about finishing an upgrade. The next price shown is the game's own. The
levels beyond it are projected from it by measuring that surcharge rather than assuming a formula,
and if the measurement does not hold up, **no projection is shown at all** rather than a wrong
total. Buying rebuilds the panel in place, since prices across the whole page just moved.

**DLC icons can take a second hover.** Art for content whose atlas is not in memory is requested
on demand, and that request is asynchronous, so the icon appears the next time you hover the row.

Kill switches: `SecretTooltips`, `BestiaryTooltips`, `AchievementTooltips`, `PowerUpTooltips`,
`MusicTooltips`.

---

## 5. Character selection

**Where:** Character grid cards (not the bottom detail strip).

**Hover shows:**

| Block | Content |
|-------|---------|
| Title | Character / skin full name (localized) |
| Flavor | Character description (localized; no raw `powerupLang/…` keys) |
| Starting weapon | Icon + name for the **current outfit** |
| Evolution | Formula icons for that starter |
| Other outfits | Other skins’ starters when they differ |
| Notable stats | Non-default HP, armor, speed, luck, etc. |

**Interaction:**

- Tooltip is **click-through** — you can select the character underneath  
- Placed near the card  
- Controller: dwell with `ControllerDwellDelay` when the card is focused  

Kill switch: `Features.CharacterTooltips`.

---

## 6. Adventure selection

**Where:** Adventure list / cards on the Adventures select screen.

**Shows:** Adventure title, description when available, cast characters, related weapons when data is present.

Kill switch: `Features.AdventureTooltips`.

## 7. Stage selection

### 7.1 Relics in stage (left panel)

Hover each relic icon → name + description. Long names (e.g. multi-line relic titles) use **dynamic TMP sizing** so text is not crushed.

### 7.2 Music | Guide tabs

Above the **song / music panel** on the right:

| Tab | Behavior |
|-----|----------|
| **Music** | Default. Normal game music track UI. |
| **Guide** | Hides the song list and shows the Stage Guide panel in the same footprint (scrolls when tall). |

Visuals: dark strip, gold outline, active tab gold, inactive muted; small gap above the song frame; centered labels.

**Config:**

- `Features.StageGuide` — master kill switch  
- `Features.StageGuideDefaultToGuide` — start on Guide instead of Music (once per stage-select session)

> **Known limitation — the Guide needs music unlocked.** On a save where the stage's music is
> not yet unlocked there is no song panel, and the Guide is built onto that panel — so the
> `Music | Guide` tabs and the whole Guide are missing rather than merely empty. This is a
> defect, not intended behaviour, and it is scheduled to be fixed by giving the Guide its own
> layout. It is invisible on a fully-unlocked save. Tracked in the private roadmap.

### 7.3 Stage Guide contents

| Section | When shown | Purpose |
|---------|------------|---------|
| Stage name | Always | Localized title |
| **Guide** | If curated tip exists | Short unofficial notes (`StageExtraTips`) |
| **Tips** | If game tips exist | Official localized tips |
| **Hyper tips** | If present | Official hyper tips |
| **Features** | If any quirk applies | Merchant ban, speed-up ban, Survarots, racing, day/night, coffin unlock, treasure flags, boss/event counts |
| **Relics (N)** | If stage has relics | List with hover tooltips |

**Intentionally omitted from Guide** (already on the bottom stats UI):

- Hyper unlocked yes/no  
- Stage length / random minutes as a “Progression” dump  
- Normal/Hyper/Inverse multiplier lines (HP×, Gold×, etc.)

### 7.4 Controller / keyboard (stage select)

| Input | Action |
|-------|--------|
| LB / RB or Q / E | Switch Music ↔ Guide |
| Vertical stick / D-pad | Scroll Guide content |
| Focus + dwell | Tooltip on Guide relic rows |
| Tab buttons | Mouse click / Submit |

---

## 8. Curated stage notes (`StageExtraTips`)

Extra English notes for many stages (Mad Forest, Library, Moongolow, DLC paths, adventure legs, test arenas, etc.). Shown under **Guide** only when a mapping exists for that `StageType`.

These are **community helper text**, not official localization. Wrong or outdated notes can be fixed in `StageExtraTips.cs` without changing game data.

---

## 9. Config reference

File: `BepInEx/config/com.nihil.vsevolutionhelper.cfg`

### Debug

| Key | Default | Description |
|-----|---------|-------------|
| `VerboseLogging` | `false` | Log evolution rows, sprite misses, hover IDs |

### Tooltips

| Key | Default | Description |
|-----|---------|-------------|
| `HoverDelay` | `0.4` | Menu/map/collection hover delay (seconds, 0–2) |
| `LevelUpHoverDelay` | `0.15` | Level Up icon hold after mouse move (0–1) |
| `ControllerDwellDelay` | `0.5` | Gamepad focus dwell (0–2) |

### Features

| Key | Default | Description |
|-----|---------|-------------|
| `MapTooltips` | `true` | Pause map tooltips |
| `StageGuide` | `true` | Music \| Guide UI |
| `StageGuideDefaultToGuide` | `false` | Prefer Guide tab on open |
| `LevelUpTooltips` | `true` | Level Up evolution tooltips |
| `CharacterTooltips` | `true` | Character select tooltips |
| `AdventureTooltips` | `true` | Adventure select tooltips |
| `WeaponSelectionTooltips` | `true` | Weapon selector screen tooltips |
| `SecretTooltips` | `true` | Secrets page unlock tooltips |
| `SecretSpoilers` | `true` | Also reveal secrets you have **not** found |
| `BestiaryTooltips` | `true` | Bestiary enemy stat tooltips |
| `BestiarySpoilers` | `true` | Also show stats for enemies you have **not** killed |
| `AchievementTooltips` | `true` | Unlocks page tooltips |
| `PowerUpTooltips` | `true` | Power Up cost-to-max tooltips |
| `ArcanaCardTooltips` | `true` | Arcana card tooltips, including the mid-run pick |
| `MusicTooltips` | `true` | Music page credit / unlock tooltips |
| `MusicSpoilers` | `true` | Also name tracks you have **not** unlocked |

There is **no** separate kill switch for pause evolution tooltips or grimoire (core behavior). Use delay values if tooltips feel too eager.

---

## 10. Localization behavior

| Situation | Result |
|-----------|--------|
| Normal I2 term | Translated string shown |
| Raw key like `powerupLang/MERCHANT name` | Parsed / cross-table lookup / humanized name |
| Description key missing | Line omitted (no gibberish key) |
| Multi-line body with one bad key | Bad line dropped; other lines kept |

---

## 11. Compatibility & limits

| Topic | Notes |
|-------|--------|
| **Loader** | BepInEx 6 IL2CPP only; MelonLoader CTDs on Unity 6 |
| **Game updates** | Interop may break on major VS patches; retest after updates |
| **Co-op / online** | Not specially tested; UI hooks are client-side |
| **Mod conflicts** | Other UI mods that replace Stage Select / Collections may fight hooks |
| **Official vs curated tips** | Guide notes are unofficial; Features are derived from stage data fields |

---

## 12. Troubleshooting

| Problem | What to try |
|---------|-------------|
| Mod not loading | Confirm BepInEx log; path `plugins/VSEvolutionHelper/VSEvolutionHelper.dll` |
| DLL won’t update | Close the game completely, then copy |
| CTD at boot with Melon leftovers | Remove Melon `version.dll` / MelonLoader folder |
| No Guide tabs | `Features.StageGuide = true`; stage select with a stage selected |
| No character tooltips | `Features.CharacterTooltips = true`; hover grid cards, not only the bottom panel |
| Raw `*Lang/` text | Update to **1.10.9+**; if one character still fails, report name + screenshot |
| Level Up tooltip on open | Should be fixed since 1.7.1; ensure current DLL |

Private save helpers (merge unlock dumps, checksum) live under `scripts/` / gitignored `storage/` — see comments in `scripts/Fix-VSSave.ps1`. Not required for normal play.

---

## 13. Related docs

| Doc | Use |
|-----|-----|
| [README.md](../README.md) | Install + feature summary |
| [CHANGELOG.md](../CHANGELOG.md) | Version-by-version changes |
| [SMOKE-TEST.md](SMOKE-TEST.md) | Pre-release checklist |
| [UI-SPEC.md](UI-SPEC.md) | How the tooltips are built (dev) |
| [game-api/README.md](../game-api/README.md) | Decompiled game API notes (dev) |
