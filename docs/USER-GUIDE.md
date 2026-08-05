# VS Evolution Helper — User guide

**Plugin:** VS Evolution Helper (BepInEx)  
**Current version:** 1.10.25  
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
| [ROADMAP.md](ROADMAP.md) | Done vs future work |
