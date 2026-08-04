# Smoke test checklist (pre-release)

Run before tagging or uploading a Nexus zip. Game build under test: **VS 1.15.x**, BepInEx 6 IL2CPP only.

## Setup

- [ ] MelonLoader proxy disabled (no `version.dll` Melon hook)
- [ ] BepInEx present (`winhttp.dll` / doorstop)
- [ ] Fresh DLL copy with game **closed**
- [ ] `BepInEx/LogOutput.log` shows `VS Evolution Helper {version}` and `Patches applied successfully`

## In-run

- [ ] Pause: hover a base weapon (e.g. Whip) → evo row + icons with sprites
- [ ] Multi-path passive/weapon (if known) shows **multiple** evo rows
- [ ] Nested click on evo icon opens nested tooltip
- [ ] Arcana section (if any) shows real names; click opens popup with Affects
- [ ] Pause **map**: hover a relic/pickup → name/description

## Menus

- [ ] Grimoire / collection evolution formulas: **left, middle, and right** icons all tooltip
- [ ] Stage selection: left **Relics in stage** icon tooltips
- [ ] Stage selection: **Music \| Guide** tabs switch without breaking music
- [ ] Guide: tips wrap; long relic names wrap; long content scrolls if needed
- [ ] Stages with **no** relics: no empty “Unlocks” clutter (or quiet empty state)

## Regression

- [ ] No spam of errors in log during stage select + one short run
- [ ] Closing stage select restores music panel
- [ ] Config: with `VerboseLogging = false`, no continuous `[DBG]` flood

## Release package

- [ ] Zip layout: `BepInEx/plugins/VSEvolutionHelper/VSEvolutionHelper.dll`
- [ ] Zip includes README.md + CHANGELOG.md (or root readme pointing at install)
- [ ] Version in log matches `PluginVersion` and CHANGELOG header
