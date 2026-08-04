# VS Evolution Helper (BepInEx port)

**Version 1.8.1** — Interactive evolution, arcana, grimoire, map, and stage-select tooltips for **Vampire Survivors** on Unity 6 / BepInEx IL2CPP.

Ported because **MelonLoader crashes** on current Unity 6 builds (`0x80131506` / CoreCLR). **Use BepInEx only.**

## Credits / original creators

This is a **BepInEx + Unity 6 port** of community MelonLoader work. Credit for the original design goes to:

| Role | Author | Links |
|------|--------|--------|
| **Original mod author** | **[NihilXD](https://www.nexusmods.com/vampiresurvivors/users/5661694)** | [Nexus: VS Evolution Helper](https://www.nexusmods.com/vampiresurvivors/mods/96) · [GitHub: NihilXD/VSEvolutionHelper](https://github.com/NihilXD/VSEvolutionHelper) |
| Unofficial 1.14 Melon update | [ashimpure](https://www.nexusmods.com/vampiresurvivors/users/80031423) | [Nexus: Unofficial Update for 1.14](https://www.nexusmods.com/vampiresurvivors/mods/101) |

Please support the original authors. This port reuses their design with typed Il2Cpp APIs for VS 1.15.

## Features

| Area | What you get |
|------|----------------|
| **Pause / level-up / merchant** | Hover weapons & passives → multi-row evolution recipes, nested click tooltips |
| **Arcana** | Related arcanas with names/sprites; click for description + affected items |
| **Collection / Grimoire** | Tooltips on formula icons (all slots in a row, not only the middle) |
| **Pause map** | Hover relics, pickups, and other map icons |
| **Stage selection** | Hover **Relics in stage** icons; **Music \| Guide** tab shares the music panel area for tips, unlocks, hyper status, and curated notes |

## Requirements

- Vampire Survivors (Steam) — tested on **1.15.113**, Unity **6000.0.62f1**
- **BepInEx 6 IL2CPP** bleeding-edge (e.g. BE 785)
- **Do not** run MelonLoader and BepInEx at the same time

## Install (Nexus / zip)

1. Install BepInEx 6 IL2CPP for the game (if needed).
2. Extract this mod so you have:

   ```
   <Vampire Survivors>/
     BepInEx/
       plugins/
         VSEvolutionHelper/
           VSEvolutionHelper.dll
   ```

3. Launch the game. **Close the game before replacing the DLL.**

Confirm in `BepInEx/LogOutput.log`:

```
Loading [VS Evolution Helper 1.8.1]
Patches applied successfully
[GameData] Ready: …
Chainloader startup complete
```

## Install (this machine / from source)

```powershell
cd VSEvolutionHelper.BepInEx
dotnet build -c Release
Copy-Item bin\Release\VSEvolutionHelper.dll `
  "D:\SteamLibrary\steamapps\common\Vampire Survivors\BepInEx\plugins\VSEvolutionHelper\"
```

`GamePath` in the `.csproj` points at your Steam install for interop references.

## Config

File (created on first run):

`BepInEx/config/com.nihil.vsevolutionhelper.cfg`

| Section | Key | Default | Meaning |
|---------|-----|---------|---------|
| Debug | `VerboseLogging` | **false** | Extra `[DBG]` lines in the console |
| Tooltips | `HoverDelay` | **0.4** | Seconds before collection / map / stage-relic tooltips |
| Tooltips | `LevelUpHoverDelay` | **0.15** | Hold time on Level Up icons (after you move the mouse) |
| Tooltips | `ControllerDwellDelay` | **0.5** | Controller focus dwell before tooltip |
| Features | `MapTooltips` | **true** | Pause-map hover tooltips |
| Features | `StageGuide` | **true** | Music \| Guide tabs on stage select |
| Features | `StageGuideDefaultToGuide` | **false** | Open Guide tab first instead of Music |
| Features | `LevelUpTooltips` | **true** | Evolution tooltips on Level Up choices |

Edit the cfg and restart the game (or re-enter menus) for changes to apply on next load.

## In-game use

1. **In a run:** pause or open level-up / merchant — hover equipment icons.
2. **Grimoire / collection:** hover evolution formula icons.
3. **Map (pause):** hover pickups and relics.
4. **Stage selection:** hover left **Relics in stage**; use **Music \| Guide** above the music list for stage tips and unlocks. **Controller:** LB/RB (or Q/E) switch tabs; stick scrolls Guide; dwell on Guide relic icons for tooltips.

## Loader notes

| Loader | Status on VS 1.15 + Unity 6 |
|--------|------------------------------|
| MelonLoader 0.7.3 | Crashes after support module (even with 0 mods) |
| BepInEx 6 BE 785 | Works |

- MelonLoader proxy (if present): keep **disabled** while using BepInEx  
- BepInEx proxy: `winhttp.dll` + `doorstop_config.ini`

## Docs

| Doc | Purpose |
|-----|---------|
| [CHANGELOG.md](CHANGELOG.md) | Release history |
| [docs/SMOKE-TEST.md](docs/SMOKE-TEST.md) | Pre-release checklist |
| [docs/ROADMAP.md](docs/ROADMAP.md) | Longer-term plan |
| [game-api/README.md](game-api/README.md) | Decompiled API notes (dev) |

## License / attribution

- **Original work:** [NihilXD](https://github.com/NihilXD/VSEvolutionHelper) — VS Evolution Helper / VS Item Tooltips (MelonLoader)
- **1.14 community update:** [ashimpure on Nexus](https://www.nexusmods.com/vampiresurvivors/mods/101)
- **This repo:** BepInEx IL2CPP port and VS 1.15 / Unity 6 fixes

If you redistribute this port, keep the credits and links to NihilXD’s original mod.
