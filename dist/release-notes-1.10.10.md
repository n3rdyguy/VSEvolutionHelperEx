## VS Evolution Helper (BepInEx) 1.10.10

Vampire Survivors **1.15** / Unity 6 · BepInEx 6 IL2CPP

### Highlights (since 1.9.7)

**Stage Guide (Music | Guide)**
- Tab strip above the song panel with gold framing and proper alignment
- Guide leads with curated notes + game tips; **Features** (merchant ban, coffin unlock, day/night, boss/event counts) instead of duplicating bottom-panel Hyper/mods
- Controller-friendly tabs; roomier layout

**Character select**
- Rich tooltips: flavor, starter + evo icons, other outfits, notable stats
- Click-through tooltips near the card; TMP-based sizing
- Localization scrub — no raw `itemLang/…` / `powerupLang/…` keys

**Adventure select**
- Hover adventures for cast / weapon summary (config `Features.AdventureTooltips`)

**Grimoire / collections**
- Evolution formula icons: hover the **whole icon**, not only the + in the middle

**In-game / UI polish**
- Weapon tooltip spacing (title, evo rows, arcana)
- Stage relic tooltips with dynamic TMP width/height
- More StageExtraTips (DLC / adventure legs)

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
