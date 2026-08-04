# Smoke test checklist (pre-release)

Run before tagging or uploading a release zip. Game: **VS 1.15.x**, BepInEx 6 IL2CPP only. Plugin version under test: **1.10.10** (or current `PluginVersion`).

## Setup

- [ ] MelonLoader proxy disabled (no Melon `version.dll`)
- [ ] BepInEx present (`winhttp.dll` / doorstop)
- [ ] Fresh DLL copy with game **closed**
- [ ] `BepInEx/LogOutput.log` shows `VS Evolution Helper {version}` and `Patches applied successfully`
- [ ] `[GameData] Ready:` appears with weapon/item/arcana counts

## In-run

- [ ] Pause: hover a base weapon (e.g. Whip) → evo row(s) + icons with sprites
- [ ] Multi-path passive/weapon shows **multiple** evo rows when applicable
- [ ] Nested click on evo icon opens nested tooltip
- [ ] Arcana section shows real names (not raw keys); click opens popup with Affects
- [ ] Pause **map**: hover a relic/pickup → name/description
- [ ] **Level Up:** no tooltip until you hover a choice; moving away hides it
- [ ] Tooltip spacing looks readable (title, description, evo, arcana gaps)

## Collection / Grimoire

- [ ] Grimoire formula icons show weapon tooltips (at least one reliable icon per row is OK)
- [ ] Collection grid: weapon / item / arcana cells tooltip when hovered
- [ ] No continuous error spam when scrolling grimoire

## Stage selection

- [ ] Left **Relics in stage**: hover → tooltip; long names don’t crush layout
- [ ] **Music \| Guide** tabs: switch without breaking music list
- [ ] Tabs aligned to song panel; small gap; labels readable
- [ ] **Guide:** stage name; Tips when present; Features quirks when present; Relics list when present
- [ ] Guide does **not** need to repeat bottom-panel Hyper/length/mods
- [ ] Curated **Guide** notes appear on known stages (e.g. Mad Forest) when defined
- [ ] Stages with **no** relics: no empty clutter
- [ ] Long Guide content scrolls
- [ ] Closing stage select restores music panel
- [ ] **Controller:** LB/RB (or Q/E) switch tabs; Guide scrolls; relic dwell tooltip works

## Character & adventure select

- [ ] **Character selection:** hover grid card → flavor, starter, evo icons, stats; click still selects
- [ ] No raw `itemLang/…` / `powerupLang/…` in title or body (1.10.7+)
- [ ] Odd characters/skins (e.g. Merchant line): name human-readable even if flavor is sparse
- [ ] **Adventure selection:** hover adventure → summary (if `AdventureTooltips` on)

## Config / regression

- [ ] With `VerboseLogging = false`, no continuous `[DBG]` flood
- [ ] Feature kill switches work (`MapTooltips`, `StageGuide`, `CharacterTooltips`, etc.)
- [ ] No spam of exceptions in log during stage select + one short run

## Release package

- [ ] Zip layout: `BepInEx/plugins/VSEvolutionHelper/VSEvolutionHelper.dll`
- [ ] Zip includes README.md + CHANGELOG.md
- [ ] Version in log matches `PluginVersion`, CHANGELOG header, and git tag
- [ ] GitHub release notes list highlights since last public tag
