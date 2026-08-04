## VS Evolution Helper (BepInEx) 1.10.24

Vampire Survivors **1.15** (tested 1.15.113) / Unity 6 · BepInEx 6 IL2CPP

### Highlights (since 1.10.10)

**Collections tab — now actually works**
- Hover tooltips fire on the Collections grid in the main menu (previously registered but never triggered)
- **Locked cells** show an **Unlock:** hint pulled from the game's achievement text
- Fixed a **crash** on the Collections tab: no more per-frame full-scene `Transform` scan; instance-only postfixes and throttled rescans

**Collections tooltip placement**
- Tooltips are **docked to the right margin**, outside the grid — always fully visible, never clipped by the scroll mask or buried behind the panel
- The docked panel stays **clickable**, so nested formula icons still work
- Hiding is delayed while the cursor travels from a grid cell to the panel

**Polish**
- Collection names/descriptions resolve properly instead of showing raw `itemLang/…` paths
- **Arcana** headers and names use a darker purple for readability; tighter section spacing
- Fixed a relic description that was, on reflection, insufficiently ominous

### Install
1. **BepInEx 6 IL2CPP** — [builds.bepinex.dev](https://builds.bepinex.dev/projects/bepinex_be) (Unity.IL2CPP Windows x64)
2. Extract this zip so you get:  
   `BepInEx/plugins/VSEvolutionHelper/VSEvolutionHelper.dll`
3. Close the game before overwriting the DLL

### Config
`BepInEx/config/com.nihil.vsevolutionhelper.cfg` after first launch — feature toggles for Stage Guide, character/adventure tooltips, Level Up, map, etc.

### Credits
- Original: [NihilXD](https://github.com/NihilXD/VSEvolutionHelper) / [Nexus #96](https://www.nexusmods.com/vampiresurvivors/mods/96)
- 1.14 update: [ashimpure](https://www.nexusmods.com/vampiresurvivors/mods/101)
- BepInEx 1.15 port + continuation: this repo
