# VS Evolution Helper (BepInEx port)

**Version 1.14.3** - Evolution, arcana, grimoire, collection, map, stage, character, adventure, secrets, bestiary, power-up and music tooltips for **Vampire Survivors** on Unity 6 / BepInEx IL2CPP.

Ported because **MelonLoader crashes** on current Unity 6 builds (`0x80131506` / CoreCLR). **Use BepInEx only.**

| | |
|--|--|
| **Latest release** | [v1.14.3](https://github.com/n3rdyguy/VSEvolutionHelperEx/releases/tag/v1.14.3) |
| **Nexus Mods** | [VS Evolution Helper (BepInEx port)](https://www.nexusmods.com/vampiresurvivors/mods/105) |
| **Game** | Vampire Survivors **1.15.x** (tested **1.15.114**) and the **1.16 public beta**, Unity **6000.0.62f1** |
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
| **Adventure selection** | Hover an adventure → cast / weapon summary; Ascension Points explains each bonus (config `AdventureTooltips`) |
| **Weapon selectors** | Hover a weapon on a selector screen (Penshin Fatcha tuna forms, Arma Dio) → tooltip (config `WeaponSelectionTooltips`) |
| **Collection / Grimoire** | Tooltips on evolution formula icons and collection grid items/arcanas. Collections tooltips **dock to the right margin** (always visible, never clipped by the grid) and stay **clickable** for nested formula detail; locked cells show an **Unlock:** hint |
| **Stage selection - relics** | Hover **Relics in stage** icons for name + description (dynamic sizing for long names) |
| **Stage selection - Music \| Guide** | Tabs above the song panel: **Music** keeps the track list; **Guide** reuses that space for stage help |
| **Secrets** | Hover a secret → what it unlocks (character portrait, weapons, relic, arcana, power-up, skins, stage, gold). Renamed characters show both names. Turn off `SecretSpoilers` to reveal only secrets you have already found |
| **Unlocks (achievements)** | Hover a row on the Unlocks page for what it grants (character portrait, weapons, relic, arcana, power-up, skins, stage, gold) and what it requires |
| **Power Up** | Hover an upgrade → level owned, next price, and what the rest costs. Prices climb with every level bought anywhere on the page, so the game's single "next" figure says little about finishing an upgrade; levels beyond the next are projected and labelled as such |
| **Bestiary** | Hover an enemy → HP, damage, speed, XP, knockback, plus **resistances**, skills and the stages it appears in - none of which the page itself shows. Stats read as ranges where one entry covers a family of enemies. Turn off `BestiarySpoilers` for killed enemies only |
| **Music** | Hover a track → the composer, the game or DLC it came from, and how it is unlocked. The page itself shows only a title, and credits the music nowhere. Turn off `MusicSpoilers` to keep the dashes the game draws over locked track names |
| **Arcana cards** | Hover any arcana card → its description and an **Affects** list of every weapon and passive it changes. Works on the **mid-run pick**, which is the only place an arcana has to be chosen against a timer. Face-down cards show nothing. A list too long for one panel continues on the opposite margin instead of truncating |
| **Affected weapons** | Hover an individual weapon icon along the bottom of the arcana info panel → what that weapon **evolves or unions into**. The panel already names the weapons an arcana touches; the evolution is the part it never says |

### Stage Guide (right column)

Shown when **Guide** is selected on stage select:

| Section | Content |
|---------|---------|
| **Title** | Localized stage name |
| **Guide** | Curated short notes when available (`StageExtraTips` - main path + many DLC/adventure stages) |
| **Tips / Hyper tips** | Game-localized stage tips when present |
| **Features** | Things the bottom stats bar usually does **not** repeat: merchant banned, clock speed-up banned, Survarots blocked, racing, day/night, **coffin unlock** character, arcana/timed treasure, boss & event counts |
| **Relics** | Interactive relic rows with hover tooltips |

**Not duplicated** in Guide: Hyper unlocked, stage length multipliers, HP/Gold mods - those stay on the bottom stage stats panel.

**Controller / keyboard (stage select):** LB/RB (or Q/E) switch Music|Guide; vertical axis scrolls Guide content; dwell on Guide relic icons for tooltips.

### Data & quality

| Area | Behavior |
|------|----------|
| **Typed `GameData`** | Weapons, powerups, items, arcanas via Il2Cpp interop + I2 + SpriteManager |
| **Multi-row evolutions** | Every known recipe for a weapon (e.g. multi-path passives) as its own line |
| **Localization scrub** | Never shows raw I2 terms like `itemLang/…` or `powerupLang/MERCHANT name`; falls back to humanized names or omits missing text |
| **Layout** | TMP preferred size for wrapping titles/descriptions; tooltip spacing for evo/arcana sections |

---

## Install the easy way

Download **`vsevolutionhelper-installer-win-x64.exe`** from the
[latest release](https://github.com/n3rdyguy/VSEvolutionHelperEx/releases/latest) and run it.
It finds your Steam copy of the game, downloads BepInEx and this mod, and installs both - then
offers install / update / uninstall from a menu.

Prefer a script you can read first? `vsevolutionhelper-installer-scripts.zip` has `install.ps1`
and `install.sh`. Details in [installer/README.md](installer/README.md).

Everything below is the manual route, and is also what to read when something goes wrong.

## Requirements

- Vampire Survivors (Steam) - **1.15.x** or the **1.16 public beta**, Unity **6000.x**
- **[BepInEx 6 bleeding-edge (BE)](https://builds.bepinex.dev/projects/bepinex_be)**, **Unity.IL2CPP**, **win-x64**
- **Do not** run MelonLoader and BepInEx at the same time

### Which BepInEx build, exactly

This is the single most common thing to get wrong, so be precise - three different
distinctions all matter, and picking the wrong one on any of them means the game launches with
no mods loaded (or does not launch at all).

| Must be | Must **not** be | Why |
|---------|-----------------|-----|
| **6.x bleeding-edge (BE)** | 5.x "stable" | 5.x has no IL2CPP support for Unity 6 |
| **Unity.IL2CPP** | Unity.Mono | Vampire Survivors is an IL2CPP build; the Mono package silently does nothing |
| **win-x64** | win-x86 | The game is 64-bit |

BepInEx 6 has **no stable release** - bleeding-edge *is* the correct channel here, not a
risky choice. Download from the official CI:
**[builds.bepinex.dev/projects/bepinex_be](https://builds.bepinex.dev/projects/bepinex_be)**

The file you want looks like:

```
BepInEx-Unity.IL2CPP-win-x64-6.0.0-be.785+<commit>.zip
```

**Verified working combination** (what this release was built and tested against):

| Component | Version |
|-----------|---------|
| BepInEx | **6.0.0-be.785** (Unity.IL2CPP, win-x64) |
| Vampire Survivors | **1.15.114** |
| Unity | **6000.0.62f1** |
| .NET runtime (bundled by BepInEx) | 6.0.7 |

Newer BE builds generally work too. If a very new BE build misbehaves, dropping back to
**be.785** is the known-good fallback.

## Install BepInEx (first time)

1. **Find the game folder.** In Steam: right-click Vampire Survivors →
   *Manage* → *Browse local files*. It contains `VampireSurvivors.exe`.
2. **Extract the BepInEx zip directly into that folder** - not into a subfolder. Afterwards
   the game folder must look like this, with `winhttp.dll` sitting *next to* the `.exe`:

   ```
   <Vampire Survivors>/
     VampireSurvivors.exe
     GameAssembly.dll
     winhttp.dll            ← the loader; if this is missing, nothing loads
     doorstop_config.ini    ← tells winhttp.dll to start BepInEx
     .doorstop_version
     BepInEx/
       core/
   ```

   If you end up with `<Vampire Survivors>/BepInEx-Unity.IL2CPP-win-x64…/`, you extracted one
   level too deep - move the contents up.

3. **Launch the game once, wait for the main menu, then quit.** The first run generates the
   IL2CPP interop assemblies, which takes noticeably longer than a normal start (a minute or
   more is normal). This creates:

   ```
   BepInEx/
     config/          ← config files appear here after plugins run
     plugins/         ← your mods go here
     interop/         ← generated game API assemblies
     LogOutput.log    ← check this when something is wrong
   ```

4. **Confirm it actually loaded.** Open `BepInEx/LogOutput.log`; the first lines should read:

   ```
   [Message: Preloader] BepInEx 6.0.0-be.785 - VampireSurvivors
   [Info   :   BepInEx] Process bitness: 64-bit (x64)
   [Info   :   BepInEx] Running under Unity 6000.0.62f1
   ```

   No `LogOutput.log` at all means the loader never started - re-check step 2.

5. **If you used MelonLoader before**, it must not load alongside BepInEx. Rename its loader so
   Windows ignores it (renaming is reversible; deleting is not):

   ```
   version.dll  →  version.dll.melon.off
   ```

   Leaving both installed causes the crash this port exists to avoid.

## Install this mod

1. Install BepInEx as above, and confirm it loaded (step 4).
2. **Close the game.** Windows keeps the DLL locked while it runs, so replacing it mid-session
   either fails or leaves you testing the old build.
3. Extract the [release zip](https://github.com/n3rdyguy/VSEvolutionHelperEx/releases) into the
   game folder. The zip already contains the `BepInEx/plugins/…` path, so it merges straight in
   and you end up with:

   ```
   <Vampire Survivors>/
     BepInEx/
       plugins/
         VSEvolutionHelper/
           VSEvolutionHelper.dll
   ```

4. **Upgrading?** Replace the existing `VSEvolutionHelper.dll`. Leave
   `BepInEx/config/com.nihil.vsevolutionhelper.cfg` alone - your settings carry over, and new
   options are added on next launch.

Confirm in `BepInEx/LogOutput.log`:

```
Loading [VS Evolution Helper 1.14.3]
Patches applied successfully
[GameData] Ready: …
Chainloader startup complete
```

### Troubleshooting

| Symptom | Cause | Fix |
|---------|-------|-----|
| No `BepInEx/LogOutput.log` at all | Loader never started | `winhttp.dll` must sit next to `VampireSurvivors.exe`, not in a subfolder |
| Log exists, but no `Loading [VS Evolution Helper …]` | Plugin not found | The DLL must be under `BepInEx/plugins/`, not `BepInEx/` |
| Log shows a **Mono** BepInEx | Wrong package | Re-download the **Unity.IL2CPP** build |
| Game crashes on startup | MelonLoader still active | Rename Melon's `version.dll` (see step 5) |
| First launch seems frozen | Interop generation | Normal on first run only - wait for it to finish |
| Tooltips missing after an update | Stale DLL | Close the game *before* replacing the DLL, then re-check the version in the log |

When reporting a problem, the first ~10 lines of `LogOutput.log` (BepInEx version, bitness,
Unity version) plus the plugin's own lines are what actually identify the setup.

## Install from source (dev)

From the `VSEvolutionHelper` repository root:

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
| Features | `SecretTooltips` | **true** | Secrets page unlock tooltips |
| Features | `SecretSpoilers` | **true** | Also reveal secrets you have **not** discovered yet |
| Features | `BestiaryTooltips` | **true** | Bestiary enemy stat tooltips |
| Features | `BestiarySpoilers` | **true** | Also show stats for enemies you have **not** killed yet |
| Features | `AchievementTooltips` | **true** | Unlocks page tooltips |
| Features | `PowerUpTooltips` | **true** | Power Up cost-to-max tooltips |
| Features | `ArcanaCardTooltips` | **true** | Arcana card tooltips, including the mid-run pick |
| Features | `MusicTooltips` | **true** | Music page credit / unlock tooltips |
| Features | `MusicSpoilers` | **true** | Also name tracks you have **not** unlocked yet |

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

Tooltips do not need a mouse. Move the selection and hold it - the tooltip appears after
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

**Not yet keyboard-reachable:** pause **map** icons - they are hit-tested against the mouse
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
| [docs/USER-GUIDE.md](docs/USER-GUIDE.md) | Every feature and every config key, in detail |
| [docs/UI-SPEC.md](docs/UI-SPEC.md) | How the tooltips are built - canvases, sorting, positioning, sprites (dev) |
| [docs/SMOKE-TEST.md](docs/SMOKE-TEST.md) | Pre-release checklist |
| [game-api/README.md](game-api/README.md) | Decompiled game API notes (dev) |

Planning notes, the roadmap and save tooling are kept outside this repo.

## License / attribution

- **Original work:** [NihilXD](https://github.com/NihilXD/VSEvolutionHelper) - VS Evolution Helper / VS Item Tooltips (MelonLoader)
- **1.14 community update:** [ashimpure on Nexus](https://www.nexusmods.com/vampiresurvivors/mods/101)
- **This repo:** BepInEx IL2CPP port and VS 1.15 / Unity 6 fixes - also on
  [Nexus](https://www.nexusmods.com/vampiresurvivors/mods/105)

If you redistribute this port, keep the credits and links to NihilXD’s original mod.
