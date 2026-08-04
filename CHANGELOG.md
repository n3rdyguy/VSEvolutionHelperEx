# Changelog

All notable changes to this BepInEx port are listed here.

## [1.7.2] — 2026-08-04

### Fixed
- **Level Up tooltips (harder):** require mouse **move** after the screen opens, then ~0.45s hover on the **icon** (not the full card). No dwell-from-auto-select. Stops tooltips when cards spawn under a stationary cursor.

## [1.7.1] — 2026-08-04

### Fixed
- **Level Up:** first pass — clear popups on open; don't treat auto-select as controller dwell.

## [1.7.0] — 2026-08-04

### Added
- Stage Selection: **Music \| Guide** tab reusing the song panel area (tips, relics, hyper status, curated extra notes).
- Stage Selection: tooltips on **Relics in stage** icons.
- Pause **map** tooltips for relics / pickups / tokens.
- Grimoire: tooltips on **all** formula icons (not only the middle slot).
- Multi-row evolution UI and typed arcana / item `GameData` layers.
- Player-facing docs: README features, smoke-test checklist, roadmap.
- `.clocignore` for authored-code line counts.

### Changed
- `Debug.VerboseLogging` defaults to **false** (quieter player logs).
- Stage Guide: scrollable content, taller panel, word-wrapped body and relic names.
- Stage Guide: omit empty relics/tips sections; no duplicate stage flavor blurb.

### Fixed
- Evolution rows with 2+ recipes no longer collapse into the wrong passive path.
- Grimoire parent-row InstanceID overwrite (only one icon type per formula).
- Moongolow extra notes incorrectly labeled as Dairy Plant.

### Credits
- Original MelonLoader mod: **NihilXD**
- Unofficial 1.14 update: **ashimpure**

## [1.6.x] — 2026-08

Internal iteration: Stage Guide layout, word wrap, relic row wrap, height fitting (see git history).

## [1.5.x] — 2026-08

Map tooltips, grimoire patches, arcana multi-evo plan implementation.

## [1.4.x] and earlier

BepInEx bootstrap and typed weapon/powerup GameData after MelonLoader failure on Unity 6.
