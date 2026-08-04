# VS Evolution Helper (BepInEx port)

Interactive evolution tooltips for **Vampire Survivors** (weapon/item hover tooltips on pause, level-up, merchant, collection, grimoire, and map).

Ported to BepInEx because **MelonLoader crashes** on current Unity 6 builds of the game (`0x80131506` / CoreCLR). **BepInEx works.**

## Credits / original creators

This project is a **BepInEx + Unity 6 port** of community MelonLoader work. All credit for the original mod idea and implementation goes to:

| Role | Author | Links |
|------|--------|--------|
| **Original mod author** | **[NihilXD](https://www.nexusmods.com/vampiresurvivors/users/5661694)** | [Nexus: VS Evolution Helper](https://www.nexusmods.com/vampiresurvivors/mods/96) · [GitHub: NihilXD/VSEvolutionHelper](https://github.com/NihilXD/VSEvolutionHelper) |
| Unofficial 1.14 Melon update | [ashimpure](https://www.nexusmods.com/vampiresurvivors/users/80031423) | [Nexus: Unofficial Update for 1.14](https://www.nexusmods.com/vampiresurvivors/mods/101) |

Please support the original authors on Nexus / GitHub. This port reuses their design (recursive evo tooltips, arcana info, collection/grimoire helpers) with typed Il2Cpp APIs for VS 1.15.

## Requirements

- Vampire Survivors (Steam) — tested on **1.15.113**, Unity **6000.0.62f1**
- **BepInEx 6 IL2CPP** bleeding-edge (e.g. `be.785`) — already installed on your game if you followed setup
- **Do not** run MelonLoader and BepInEx at the same time

## Install (this machine)

Plugin is installed at:

```
D:\SteamLibrary\steamapps\common\Vampire Survivors\BepInEx\plugins\VSEvolutionHelper\VSEvolutionHelper.dll
```

Launch the game via Steam (or `VampireSurvivors.exe`). Confirm in `BepInEx\LogOutput.log`:

```
Loading [VS Evolution Helper 1.5.x]
Patches applied successfully
[GameData] Ready: … weapon names, … items, … arcanas (typed Il2Cpp API)
Chainloader startup complete
```

Game API notes (decompiled types): see `game-api/README.md`.

## In-game use

1. Start a run (or open menus that show weapons/items).
2. Hover weapon/item icons on **pause**, **level-up**, **merchant**, **collection**, **grimoire**, or the **map**.
3. Tooltips show evolution formulas / related passives, arcana where supported, and map relics/pickups.

## Rebuild from source

```powershell
cd C:\Users\Martin\projects\mods\vampire\VSEvolutionHelper.BepInEx
dotnet build -c Release
Copy-Item bin\Release\VSEvolutionHelper.dll `
  "D:\SteamLibrary\steamapps\common\Vampire Survivors\BepInEx\plugins\VSEvolutionHelper\"
```

`GamePath` in the `.csproj` points at your Steam install for interop references.

## Loader notes

| Loader | Status on VS 1.15 + Unity 6 |
|--------|------------------------------|
| MelonLoader 0.7.3 | Crashes after support module (even with 0 mods) |
| BepInEx 6 BE 785 | Works |

- MelonLoader proxy (if present): `version.dll` — keep **disabled** (`version.dll.melon.off`) while using BepInEx  
- BepInEx proxy: `winhttp.dll` + `doorstop_config.ini`

## License / attribution

- **Original work:** [NihilXD](https://github.com/NihilXD/VSEvolutionHelper) — VS Evolution Helper / VS Item Tooltips (MelonLoader)
- **1.14 community update:** [ashimpure on Nexus](https://www.nexusmods.com/vampiresurvivors/mods/101)
- **This repo:** BepInEx IL2CPP port and VS 1.15 / Unity 6 fixes (typed `GameData`, multi-row evolutions, grimoire/map tooltips)

If you redistribute this port, please keep the credits and links to NihilXD’s original mod above.
