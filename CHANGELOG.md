# Changelog

All notable changes to this BepInEx port are listed here.

## [1.11.0] — 2026-08-06

Tested on Vampire Survivors **1.15.114** (no patches needed re-targeting from 1.15.113).

### Added
- **Bestiary tooltips.** Hover an enemy to see HP, damage, speed, XP and knockback, plus its
  resistances, skills and the stages it appears in — none of which the page shows. Stats are
  ranges where the Bestiary groups several enemy ids into one entry. Config `BestiaryTooltips`;
  `BestiarySpoilers` (default on) also covers enemies you have not killed yet.
- **Character Selection tooltips are interactive.** The evolution, passive and starting-weapon
  icons now open the same nested formula popups the Grimoire has, stacking down-right from the
  tooltip they came from. Related arcanas are clickable too.

### Fixed
- **Character tooltips triggered outside the visible grid.** After scrolling, the hover
  fallback tested each card's rectangle without checking it was still inside the scroll
  viewport, so empty space above the list matched a card that had scrolled out of view.
- **Character tooltips could not be reached.** The tooltip was destroyed the moment the pointer
  touched a neighbouring card — before that card's own delay had elapsed — so moving onto it to
  click anything was impossible. It now survives until another card is genuinely dwelt on, and
  holds while the pointer is on the tooltip or any popup opened from it.
- **Clicks passed through the character tooltip.** Every graphic in it was made click-through so
  the card underneath stayed selectable, which also silenced the icons themselves. Only
  interactive icons opt back in; the background and text still pass clicks through.

### Internal
- Shared `RowTooltipRegistry` and a generalized docked popup, so list pages (Secrets, Bestiary)
  no longer each carry their own copy of the hover/registry code.

### Added
- **Secrets page tooltips.** Hover a secret to see what it unlocks — character (with portrait),
  weapons, relic, arcana, power-up, skins, stage, hyper, gold and custom unlock text.
  Config `SecretTooltips`; `SecretSpoilers` (default on) also reveals secrets you have not
  discovered yet, and can be turned off to show only the ones you have already found.

### Fixed
- **Secret rewards were reported as "Void".** Every reward field on the parsed record reads
  back as the enum's `VOID` member for mystery secrets — in the row's own copy *and* in the
  `AllSecrets` catalog. The raw secrets JSON those records are parsed from still carries them
  as plain strings, so rewards are now read from there. Ids belonging to DLC that is not
  installed still list, as a humanized label rather than vanishing.
- **Renamed characters showed their retired name.** Names are now composed from the character
  record's own name parts, the same way the game builds them, instead of from a single
  localization term that was never updated after a rename. Where the two disagree, both are
  shown — e.g. `Minnah Mannarah (Graziella)`.
- **The selected secret did not respond to hover.** The selection highlight covers the row's
  contents and swallowed the pointer before it reached the reward icon; the row root is now
  registered too, so the hover still resolves. Existing `EventTrigger` entries are appended to
  rather than cleared, leaving the row's own wiring intact.

## [1.10.26] — 2026-08-05

### Fixed
- **Weapon selector tooltips are reachable by keyboard and controller.** They were mouse-only:
  the tooltip was attached to the weapon's frame image, but keyboard/pad select the whole
  cell, and the lookup only searched upward from the selection — so it never found a tooltip
  registered on a child. The cell itself is now mapped for navigation, without touching the
  button wiring that makes selecting the weapon work.

### Added
- Keyboard / controller reference in README and the user guide, including the one place that
  still needs a mouse (pause map icons).

## [1.10.25] — 2026-08-05

### Fixed
- **Weapon selector tooltips now work.** Hovering a weapon on a selector screen — Arma Dio,
  and Penshin Fatcha's tuna forms — shows its tooltip. Previously no selector screen had
  tooltips at all. Three separate faults had to be cleared:
  - The selector cell type was looked up by searching for an assembly whose name contained
    `Il2Cpp` — a MelonLoader convention that never matches under BepInEx, so the screen's
    setup returned before touching a single cell.
  - Penshin Fatcha binds its cells through `SetPenshinData`, a different method from the
    `SetData` used elsewhere, so it needed its own hook.
  - The mod cached the wrong selector view. Two exist side by side and only one is live at a
    time; it held onto the inactive one, decided no menu was open, and threw each tooltip
    away after building it.
- Selector cells re-register when a selector is reopened mid-run, instead of only after
  unpausing.
- **Merchant map tooltips showed raw text** — "MERCHANT name" and "MERCHANT description"
  instead of real text. The game's localization returns those placeholder strings as a
  *successful* result, so they passed straight through to the tooltip. Lookup results are now
  checked for that shape, not just lookup inputs. Merchants read **Merchant** and **Xanthia**.
- Section headers in tooltips no longer overlap the row beneath them when the label wraps to
  two lines, and no longer run past the right edge of the panel.

### Added
- **Merchant wares.** Hovering a custom merchant on the pause map (Xanthia, adventure
  merchants) lists what they sell, with icons. Read from the game's own merchant data, so it
  stays right across patches, and DLC items appear only when that DLC is installed.
  The base Merchant shows name and description only — its stock is rolled per encounter
  rather than stored, so there is nothing accurate to list ahead of time.

- `Features.WeaponSelectionTooltips` config toggle (default `true`).

### Changed
- **Weapon selector and merchant** tooltips are offset down and to the right so they no longer
  sit on top of the icon you are hovering. These screens use much larger cells than the rest;
  every other screen keeps its existing placement.
- Weapon selector cells are resolved through the typed IL2CPP API rather than string-name
  reflection, and the screen is located by looking for whichever selector view is actually on
  screen rather than by a fixed scene path — so a renamed or re-parented view no longer
  silently disables the feature.

## [1.10.24] — 2026-08-04

Collections tab polish, batched from the 1.10.11 → 1.10.24 iteration into one release.

### Added
- **Collections tab tooltips** (main menu) now fire at all: cells get pointer enter/exit
  triggers when registered, and hover updates run on the main menu instead of only while
  the game is paused.
- **Locked cells** show an **Unlock:** hint sourced from the game's achievement text.

### Changed
- Collections tooltips are **docked to the right margin** of the App Safe Area, outside the
  collections grid. They no longer chase the mouse or the hovered cell, so they stay fully
  visible and clear of the center panel.
- The docked panel stays **interactive** — click formula icons for nested detail; hiding is
  delayed while the cursor travels from the grid to the panel.
- **Arcana** headers and names use a darker purple for readability; tighter section spacing.

### Fixed
- **Crash on the Collections tab:** removed the per-frame full-scene `FindObjectsOfType<Transform>`
  scan, dropped the `Sort*` patches, switched to instance-only `CollectionItemUI` postfixes,
  and throttled rescans.
- IL2CPP Harmony `SetData` parameter binding (bind by position) so registration works.
- Collection names/descriptions resolve through `GameData` + `LocalizeDisplayText` instead of
  aborting or painting raw `itemLang/…` paths.
- Tooltips no longer clipped under the `ScrollRect` mask, blanked by a nested `Canvas`, or
  buried behind the grid.
- Fixed a relic description that was, on reflection, insufficiently ominous.

### Notes
- Phase 0 playtest: level-up, pause, and merchant confirmed working on **1.15.113**.

<details>
<summary>Version trail (each bump is one commit; later entries supersede earlier attempts)</summary>

| Version | Change |
|---------|--------|
| 1.10.11 | Rescan `CollectionItemUI` on open/filter; IL2CPP SetData binding; locked **Unlock:** tips |
| 1.10.12 | Crash fix — no full-scene Transform scan; instance-only postfixes; throttled scans |
| 1.10.13 | EventTrigger hover on App UI cells; run collection hover on the main menu |
| 1.10.14 | Place tooltip next to the hovered cell instead of a fixed 1450,930 position |
| 1.10.15 | Pin to hovered cell; scrub I2 names via `GameData` + `LocalizeDisplayText` |
| 1.10.16 | Override canvas sorting so tooltips draw above masked UI |
| 1.10.17 | Fix invisible/hover-stealing popups: `overrideSorting` only, raycasts off, delayed exit |
| 1.10.18 | Reverse polarity — outside grid, no nested Canvas, place from cell corners |
| 1.10.19 | Simplify — Safe Area parent + world placement; end delayed-exit races |
| 1.10.20 | Dock to the Safe Area right margin; stop chasing mouse/cell placement |
| 1.10.21 | Docked panel clickable; delayed hide grid→panel; darker Arcana headers; spacing |
| 1.10.22 | Dark Arcana purple for arcana name text |
| 1.10.23 | Collections tooltip copy tweak; context flag guarded with `try/finally` |
| 1.10.24 | Prefer the official I2 blurb, with a little something after it |

</details>

## [1.10.10] — 2026-08-04

### Fixed
- Grimoire evolution icons: hover the **whole icon cell** (not only near the +). Full root hit plate, child graphic mapping, padded screen-space hit tests, smarter icon-vs-row scoring.

### Included since 1.9.7 (shipped as 1.10.x)
- Stage Guide Music|Guide polish, Features panel, adventure tooltips, character tooltip polish/loc scrub, weapon/relic spacing, StageExtraTips expansion (see entries below).

## [1.10.9] — 2026-08-04

### Fixed
- Character tooltips: detect **any** `*Lang/` I2 term (including `powerupLang/MERCHANT name` with spaces / line wraps). Cross-table lookup + humanize name fallback.

## [1.10.8] — 2026-08-04

### Fixed
- Broader I2 scrubbing: weapons, items, powerups, arcanas, and character titles/flavor never fall back to raw loc keys. Extra character key synthesis (`LocalizeTypedDescription`) for skins with missing description data.

## [1.10.7] — 2026-08-04

### Fixed
- Character tooltips: no more raw I2 keys like `itemLang/{MERCHANT}description`; localize via `LocalizeDisplayText` / line-by-line body scrub. Name-only popups when the description was a failed loc key are fixed.

## [1.10.6] — 2026-08-04

### Changed
- Stage Guide: drop redundant **Progression / Hyper / mods** (already on the bottom stats panel). Lead with **Guide** notes + tips, then **Features** (merchant ban, coffin unlock, day/night, boss/event counts, etc.) and relics.

## [1.10.5] — 2026-08-04

### Fixed
- Music|Guide tabs: small gap above the song panel; label text Midline-centered.

## [1.10.4] — 2026-08-04

### Fixed
- In-game weapon tooltips: tighter **section gaps**, centered +/→ on evo rows, TMP-sized title/description, arcana name wrap/alignment.
- Stage select **Music | Guide** tabs: gold-framed strip, better contrast, edges aligned to the song panel.

## [1.10.3] — 2026-08-04

### Fixed
- Stage relic / simple tooltips: **dynamic width + TMP height** so long names (e.g. “Roast Chicken with a Clock…”) and descriptions no longer look cramped or collide.

## [1.10.2] — 2026-08-04

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
