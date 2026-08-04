# Plan: Arcana polish + multi-row evolutions

**Status:** **Done** (shipped; current plugin **1.10.24**). Kept for historical design notes.

See [USER-GUIDE.md](USER-GUIDE.md) for player-facing behavior and [ROADMAP.md](ROADMAP.md) for what’s next.

## Goals

1. **Multi-row evolution UI** — show every recipe (e.g. Hollow Heart → Bloody Tear *and* Mazzo Familiar), each as its own `base + passives → evolved` line with icons and nested click tooltips.
2. **Typed arcana tooltips** — load `DataManager.AllArcanas` (`Dictionary<ArcanaType,ArcanaData>`), resolve names/descriptions via I2 + sprites via `SpriteManager`, clickable cards with full popup and “Affects” list.
3. **Debug** — keep `[DBG]` lines for evo rows and arcana matches.

## Game API (decompiled)

| API | Use |
|-----|-----|
| `DataManager.AllArcanas` | `Dictionary<ArcanaType, ArcanaData>` |
| `ArcanaData.name/description/frameName/texture` | Display |
| `ArcanaData.GetLocalizedNameTerm(ArcanaType)` | I2 term |
| `ArcanaData.weapons` / `.items` | `List<Object>` — parse as WeaponType / ItemType |
| `ArcanaData.major`, `enabled`, `hidden` | Filtering |

## Implementation

### A. `GameData` extensions — done
- Cache `AllArcanas` + reverse indexes `WeaponToArcanas` / `ItemToArcanas`
- `GetArcanaName/Description/Sprite`, `GetArcanasAffectingWeapon/Item`
- `GetWeaponsAffectedByArcana` / `GetItemsAffectedByArcana`
- Parse `weapons`/`items` Object lists via ToString + enum/int

### B. Multi-row evo UI — done
- `AddWeaponEvolutionSection` loops **all** `BuildEvoRowsFor` rows
- Each row: base + passives → evolved with click tooltips; multi-row shows evolved name
- Removed the old “Count ≥ 2 → collapse to AddPassiveEvolutionSection” path

### C. Arcana section — done
- `GetActiveArcanasForWeapon/Item` → `GameData.GetArcanasAffecting*`
- Arcana icon: `PointerClick` → `ShowArcanaPopup(ArcanaType)`
- `CreateArcanaPopup`: typed name/desc/sprite + Affects icons

### D. Verify
- [ ] Build Release, install DLL
- [ ] Log: no Harmony param errors; DBG multi rows + arcana matches
- [ ] Manual: Whip, Hollow Heart, pause Lightning; click arcana card

## Out of scope
- MelonLoader
- Full arcana selection UI rewrite
- Controller dwell polish
