# VS Evolution Helper (BepInEx port)

**Version 1.10.26** — Evolution, arcana, grimoire, collection, map, stage, character, and adventure tooltips for **Vampire Survivors** on Unity 6 / BepInEx IL2CPP.

Ported because **MelonLoader crashes** on current Unity 6 builds (`0x80131506` / CoreCLR). **Use BepInEx only.**

| | |
|--|--|
| **Latest release** | [v1.10.25](https://github.com/n3rdyguy/VSEvolutionHelperEx/releases/tag/v1.10.25) |
| **Game** | Vampire Survivors **1.15.x** (tested **1.15.113**), Unity **6000.0.62f1** |
| **Loader** | [BepInEx 6 IL2CPP](https://builds.bepinex.dev/projects/bepinex_be) (BE / bleeding-edge) |

## Credits / original creators

This is a **BepInEx + Unity 6 port** of community MelonLoader work. Credit for the original design goes to:

| Role | Author | Links |
|------|--------|--------|
| **Original mod author** | **[NihilXD](https://www.nexusmods.com/vampiresurvivors/users/5661694)** | [Nexus: VS Evolution Helper](https://www.nexusmods.com/vampiresurvivors/mods/96) · [GitHub: NihilXD/VSEvolutionHelper](https://github.com/NihilXD/VSEvolutionHelper) |
| Unofficial 1.14 Melon update | [ashimpure](https://www.nexusmods.com/vampiresurvivors/users/80031423) | [Nexus: Unofficial Update for 1.14](https://www.nexusmods.com/vampiresurvivors/mods/101) |

Please support the original authors. This port reuses their design with typed Il2Cpp APIs for VS 1.15.

---

## Features (full)

### In a run

| Feature | What you get |
|---------|----------------|
| **Equipment tooltips** | Hover weapons & passives (pause inventory, HUD icons where registered) → multi-row evolution recipes with icons, `+` / `→`, nested click tooltips |
| **Arcana on weapons/items** | Related arcanas with names/sprites; click for description + **Affects** list |
| **Level Up** | Hover a level-up choice for the same evolution/arcana info; **no** tooltip until you actually hover (no popup on open) |
| **Pause map** | Hover map relics, pickups, and other registered map icons → name + description |
| **Merchant / shops** | Same weapon/item tooltip pattern where icons are registered |

### Pre-run menus

| Feature | What you get |
|---------|----------------|
| **Character selection** | Hover a character → portrait, flavor (localized), **starting weapon**, evolution path icons, **other outfits**’ starters, **notable stats**. Click-through so you can still select the card. |
| **Adventure selection** | Hover an adventure → cast / weapon summary (config `AdventureTooltips`) |
| **Weapon selectors** | Hover a weapon on a selector screen (Penshin Fatcha tuna forms, Arma Dio) → tooltip (config `WeaponSelectionTooltips`) |
| **Collection / Grimoire** | Tooltips on evolution formula icons and collection grid items/arcanas. Collections tooltips **dock to the right margin** (always visible, never clipped by the grid) and stay **clickable** for nested formula detail; locked cells show an **Unlock:** hint |
| **Stage selection — relics** | Hover **Relics in stage** icons for name + description (dynamic sizing for long names) |
| **Stage selection — Music \| Guide** | Tabs above the song panel: **Music** keeps the track list; **Guide** reuses that space for stage help |

### Stage Guide (right column)

Shown when **Guide** is selected on stage select:

| Section | Content |
|---------|---------|
| **Title** | Localized stage name |
| **Guide** | Curated short notes when available (`StageExtraTips` — main path + many DLC/adventure stages) |
| **Tips / Hyper tips** | Game-localized stage tips when present |
| **Features** | Things the bottom stats bar usually does **not** repeat: merchant banned, clock speed-up banned, Survarots blocked, racing, day/night, **coffin unlock** character, arcana/timed treasure, boss & event counts |
| **Relics** | Interactive relic rows with hover tooltips |

**Not duplicated** in Guide: Hyper unlocked, stage length multipliers, HP/Gold mods — those stay on the bottom stage stats panel.

**Controller / keyboard (stage select):** LB/RB (or Q/E) switch Music|Guide; vertical axis scrolls Guide content; dwell on Guide relic icons for tooltips.

### Data & quality

| Area | Behavior |
|------|----------|
| **Typed `GameData`** | Weapons, powerups, items, arcanas via Il2Cpp interop + I2 + SpriteManager |
| **Multi-row evolutions** | Every known recipe for a weapon (e.g. multi-path passives) as its own line |
| **Localization scrub** | Never shows raw I2 terms like `itemLang/…` or `powerupLang/MERCHANT name`; falls back to humanized names or omits missing text |
| **Layout** | TMP preferred size for wrapping titles/descriptions; tooltip spacing for evo/arcana sections |

---

## Requirements

- Vampire Survivors (Steam) — **1.15.x**, Unity **6000.x**
- **[BepInEx 6](https://builds.bepinex.dev/projects/bepinex_be)** — **Unity.IL2CPP** bleeding-edge (e.g. **BE 785+**). Stable BepInEx 5 / MelonLoader will not work on this Unity 6 build.
- **Do not** run MelonLoader and BepInEx at the same time

## Install BepInEx (first time)

Official bleeding-edge builds: **[https://builds.bepinex.dev/projects/bepinex_be](https://builds.bepinex.dev/projects/bepinex_be)**

1. Download a recent **Windows** package for **Unity.IL2CPP** (e.g. `BepInEx-Unity.IL2CPP-win-x64-6.0.0-be.xxx+….zip` — **not** Mono).
2. Extract into the Vampire Survivors game folder (next to `VampireSurvivors.exe`):

   ```
   <Vampire Survivors>/
     VampireSurvivors.exe
     winhttp.dll
     doorstop_config.ini
     BepInEx/
       core/
       …
   ```

3. Launch the game **once** so BepInEx creates `config/`, `plugins/`, and `LogOutput.log`. Quit.
4. If you used MelonLoader before: remove/rename Melon’s `version.dll` / MelonLoader folder so only BepInEx’s `winhttp.dll` loads.

## Install this mod

1. Install BepInEx as above.
2. Extract the [release zip](https://github.com/n3rdyguy/VSEvolutionHelperEx/releases) so you have:

   ```
   <Vampire Survivors>/
     BepInEx/
       plugins/
         VSEvolutionHelper/
           VSEvolutionHelper.dll
   ```

3. **Close the game before replacing the DLL.**

Confirm in `BepInEx/LogOutput.log`:

```
Loading [VS Evolution Helper 1.10.26]
Patches applied successfully
[GameData] Ready: …
Chainloader startup complete
```

## Install from source (dev)

```powershell
cd VSEvolutionHelper.BepInEx
dotnet build -c Release
Copy-Item bin\Release\VSEvolutionHelper.dll `
  "<YourSteam>\Vampire Survivors\BepInEx\plugins\VSEvolutionHelper\"
```

`GamePath` in the `.csproj` points at your Steam install for interop references.

---

## Config

Created on first run:

`BepInEx/config/com.nihil.vsevolutionhelper.cfg`

| Section | Key | Default | Meaning |
|---------|-----|---------|---------|
| Debug | `VerboseLogging` | **false** | Extra `[DBG]` lines in the console |
| Tooltips | `HoverDelay` | **0.4** | Seconds before collection / map / stage-relic / character / adventure tooltips |
| Tooltips | `LevelUpHoverDelay` | **0.15** | Hold time on Level Up icons after mouse move |
| Tooltips | `ControllerDwellDelay` | **0.5** | Controller focus dwell before tooltip |
| Features | `MapTooltips` | **true** | Pause-map hover tooltips |
| Features | `StageGuide` | **true** | Music \| Guide tabs on stage select |
| Features | `StageGuideDefaultToGuide` | **false** | Open Guide first instead of Music |
| Features | `LevelUpTooltips` | **true** | Evolution tooltips on Level Up choices |
| Features | `CharacterTooltips` | **true** | Character Selection tooltips |
| Features | `AdventureTooltips` | **true** | Adventures select tooltips |
| Features | `WeaponSelectionTooltips` | **true** | Weapon selector screen tooltips |

Restart the game (or re-enter menus) after edits so values reload.

---

## Quick start (in-game)

1. **Run:** pause or level-up → hover equipment icons; click evo/arcana icons for nested detail.
2. **Map:** pause map → hover pickups/relics.
3. **Grimoire:** hover formula icons.
4. **Stage select:** hover left relics; open **Guide** for tips/features/relics; **Music** for tracks.
5. **Characters / Adventures:** hover cards for summaries.

Every feature and every config key is described below.

## Keyboard / controller

Tooltips do not need a mouse. Move the selection and hold it — the tooltip appears after
`Tooltips.ControllerDwellDelay` (default 0.5s).

| Key | Pad | Does |
|-----|-----|------|
| Arrow keys / WASD | Stick / d-pad | Move selection; switches out of mouse mode |
| **Tab** | Y | Enter **interactive mode** on a shown tooltip (focus its formula icons); with no tooltip while paused, enter **equipment nav**; press again to back out |
| **Space** / **Enter** | A | In equipment nav with a tooltip up: enter interactive mode |
| **Backspace** | B | Close the top popup / step back one nested level |
| **Q** / **E** | LB / RB | Stage select: switch **Music ↔ Guide** |

Works on pause equipment, level-up, Collections / Grimoire (including arcana), stage select,
and weapon selector screens.

**Moving the mouse returns to mouse mode immediately**, closing any nav mode.

**Not yet keyboard-reachable:** pause **map** icons — they are hit-tested against the mouse
pointer and have no selection to move. Use the mouse for those.

---

## Loader notes

| Loader | Status on VS 1.15 + Unity 6 |
|--------|------------------------------|
| MelonLoader 0.7.x | Crashes after support module (even with 0 mods) |
| [BepInEx 6 BE](https://builds.bepinex.dev/projects/bepinex_be) (IL2CPP) | Works (tested BE 785+) |

## Docs

| Doc | Purpose |
|-----|---------|
| [CHANGELOG.md](CHANGELOG.md) | Release history |
| [game-api/README.md](game-api/README.md) | Decompiled API notes (dev) |

Planning notes, the smoke-test checklist and save tooling are kept outside this repo.

## License / attribution

- **Original work:** [NihilXD](https://github.com/NihilXD/VSEvolutionHelper) — VS Evolution Helper / VS Item Tooltips (MelonLoader)
- **1.14 community update:** [ashimpure on Nexus](https://www.nexusmods.com/vampiresurvivors/mods/101)
- **This repo:** BepInEx IL2CPP port and VS 1.15 / Unity 6 fixes

If you redistribute this port, keep the credits and links to NihilXD’s original mod.
