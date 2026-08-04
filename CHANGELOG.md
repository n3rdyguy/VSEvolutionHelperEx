# Changelog

All notable changes to this BepInEx port are listed here.

## [1.10.9] — polish (unreleased tag)

### Fixed
- Character tooltips: detect **any** `*Lang/` I2 term (including `powerupLang/MERCHANT name` with spaces / line wraps). Cross-table lookup + humanize name fallback.

## [1.10.8] — polish (unreleased tag)

### Fixed
- Broader I2 scrubbing: weapons, items, powerups, arcanas, and character titles/flavor never fall back to raw loc keys. Extra character key synthesis (`LocalizeTypedDescription`) for skins with missing description data.

## [1.10.7] — polish (unreleased tag)

### Fixed
- Character tooltips: no more raw I2 keys like `itemLang/{MERCHANT}description`; localize via `LocalizeDisplayText` / line-by-line body scrub. Name-only popups when the description was a failed loc key are fixed.

## [1.10.6] — polish (unreleased tag)

### Changed
- Stage Guide: drop redundant **Progression / Hyper / mods** (already on the bottom stats panel). Lead with **Guide** notes + tips, then **Features** (merchant ban, coffin unlock, day/night, boss/event counts, etc.) and relics.

## [1.10.5] — polish (unreleased tag)

### Fixed
- Music|Guide tabs: small gap above the song panel; label text Midline-centered.

## [1.10.4] — polish (unreleased tag)

### Fixed
- In-game weapon tooltips: tighter **section gaps**, centered +/→ on evo rows, TMP-sized title/description, arcana name wrap/alignment.
- Stage select **Music | Guide** tabs: gold-framed strip, better contrast, edges aligned to the song panel.

## [1.10.3] — polish (unreleased tag)

### Fixed
- Stage relic / simple tooltips: **dynamic width + TMP height** so long names (e.g. “Roast Chicken with a Clock…”) and descriptions no longer look cramped or collide.

## [1.10.2] — polish (unreleased tag)

### Added / improved
- Adventure tooltips: **weapon icon strip**, character list, TMP-sized panel
- Many more **StageExtraTips** (Moonspell/Poe/Imelda/OTC/FB adventure legs + bazaars)

## [1.10.1] — 2026-08-04

### Fixed
- Character select tooltips **resize from real TMP preferred height/width** so flavor, stats, and multi-line names are no longer clipped or oversized.

## [1.10.0] — 2026-08-04

### Added
- **Adventure select tooltips:** hover adventures for cast / weapon summary (config `Features.AdventureTooltips`).
- More **StageExtraTips** (machine/space/test arenas + sample adventure stages).

## [1.9.7] — 2026-08-04

### Changed
- Character tooltip placement restored to the **original near-card position**; still **click-through** so you can select the character under it. Keeps weapon/evo icons from 1.9.6.

## [1.9.6] — 2026-08-04

### Fixed
- Character tooltip **positioning** uses screen-space conversion (no more stuck mid-left); still prefers right of card, flips left if needed; non-blocking raycasts.

### Added
- Character tooltip **weapon + evolution icons** (starter, passives, evolved) with labels.

## [1.9.5] — 2026-08-04

### Fixed
- Character tooltip sits **to the right** of the card (flips left if no room) and **does not capture mouse**, so you can still click to select.

## [1.9.4] — 2026-08-04

### Fixed
- Character tooltips only name / few cards: register **whole grid cards** (not tiny weapon icons); relax grid filter; pre-bake full body + live rebuild; rect hover fallback so most characters work.

## [1.9.3] — 2026-08-04

### Fixed
- Starting weapon no longer always **Void**: resolve from the card’s **weapon icon sprite** first (skin/outfit-correct), then data fields. `GetWeaponName` refuses VOID; never print “Void” as a starter.

## [1.9.2] — 2026-08-04

### Fixed
- Character tooltips only on **grid cards** (UI raycast) — no longer pop when hovering the bottom info panel.
- Starting weapon: ignore `WeaponType.VOID` (false HasValue); prefer **current outfit/skin** starter, then character, then weapon-icon sprite.
- Outfits with different starters (e.g. Para Kooleo) listed under **Other outfits**; tooltip rebuilds live on hover/skin change.

## [1.9.1] — 2026-08-04

### Fixed
- **Character Selection broken** (every card “Pasqualina” / blank art): removed Harmony patch on `CharacterItemUI.SetData` (IL2CPP detour corrupted population). Tooltips now register by scanning cards after Populate/show only.

## [1.9.0] — 2026-08-04

### Added
- **Character Selection tooltips:** hover a character for flavor text, starting weapon, evolution path(s), and notable stats. Config: `Features.CharacterTooltips` (default true).
- **Stage Guide progression:** stage length minutes; Normal / Hyper / Inverse modifier summary (HP, gold, speeds, etc. when present in data).

## [1.8.1] — 2026-08-04

### Added
- **Controller / keyboard Stage Guide:** LB/RB (or Q/E) switch Music|Guide; tabs are UI Buttons (focus + Submit); vertical stick scrolls Guide; Guide relic icons are selectable with controller dwell tooltips.

## [1.8.0] — 2026-08-04

### Added
- **Config surface** (`com.nihil.vsevolutionhelper.cfg`): hover delays, map/stage-guide/level-up feature toggles, default Guide tab option.

### Fixed (also 1.7.1–1.7.3)
- Level Up no longer shows an unsolicited tooltip on open; requires mouse move + short icon hover.

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
