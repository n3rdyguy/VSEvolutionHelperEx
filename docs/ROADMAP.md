# VS Evolution Helper — Full Roadmap

**Mod:** BepInEx IL2CPP port of NihilXD’s Evolution Helper for Vampire Survivors 1.15 / Unity 6  
**Current version:** 1.10.1 (Phase 3 mostly done; polish ongoing)  
**Repo state:** Character + adventure tooltips solid; more StageExtraTips / UX polish  

### Working practices (owner notes)
- **Do not tag/GitHub-release on every polish change.** Iterate: build → install local DLL → commit/push when useful. Ship `vX.Y.Z` only when the owner asks or when a real milestone is ready.
- Keep `CHANGELOG.md` / version bumps light during polish; batch them into the next intentional release.
- `storage/` is private (gitignored); save-fix tooling lives under `scripts/`.


**Credits (keep in all releases):**
- Original: [NihilXD](https://github.com/NihilXD/VSEvolutionHelper) · [Nexus #96](https://www.nexusmods.com/vampiresurvivors/mods/96)
- 1.14 update: [ashimpure](https://www.nexusmods.com/vampiresurvivors/mods/101)

---

## 1. Already shipped (baseline)

| Area | What users get | Notes |
|------|----------------|--------|
| BepInEx load | Works where MelonLoader CTDs on Unity 6 | `winhttp` / BepInEx 6 BE |
| Typed `GameData` | Weapons, powerups, items, arcanas | Il2Cpp interop, I2, SpriteManager |
| Evolution tooltips | Multi-row recipes, nested clicks | Pause / level-up / merchant / collection |
| Arcana section | Names, sprites, Affects, click popups | Data-driven from `AllArcanas` |
| Grimoire | All formula icons hover (not only middle) | `EvolutionItemUI.AddWeaponIcon` |
| Pause map | Relics / pickups / tokens tooltips | `MapManager.SpawnItemOnMap` |
| Stage select A | “Relics in stage” tooltips | `RelicPanel.SetRelics` |
| Stage select B+C | Music \| Guide tab, tips, extra notes, wrap | Shares song-panel real estate |

**Out of scope historically (still optional later):** MelonLoader revival, full arcana UI rewrite, inventing unofficial “wiki guides” for every adventure stage without data.

---

## 2. Near-term polish (quality / UX)

### 2.1 Stage Guide polish
| Task | Why | Effort |
|------|-----|--------|
| ScrollRect (or clip + scroll) when content > ~screen height | Long stages still overflow on small resolutions | M |
| Stabilize tab bar layout across resolutions | Stretch vs fixed anchors differ | S–M |
| Hide empty sections cleanly | e.g. LABORRATORY with 0 relics logs noise; “No relics” is fine but can be quieter | S |
| Re-select stage while Guide open keeps content | Already mostly works; verify tab state | S |
| Optional: default tab = Guide once per session | Power users; keep Music default for first-timers | S |
| Expand / curate `StageExtraTips` | Wrong mappings (e.g. SINKING/Moongolow) already partially fixed; cover main + popular DLC | S–M |
| Adventure / DLC stage tips (sparse) | Only where we have good short notes | S |

### 2.2 Map tooltips polish
| Task | Why | Effort |
|------|-----|--------|
| Better map-token / unknown sprite labels | Some icons only humanize frame names | M |
| Faster hover delay option | Map icons are tiny | S |
| Don’t clear/register races on map zoom | Edge cases if icons rebuild | S |

### 2.3 Evolution / arcana polish
| Task | Why | Effort |
|------|-----|--------|
| Verify multi-evo on all famous multi-path passives | Hollow Heart, etc. | S |
| Arcana “active run only” filter option | Currently data-driven (all affecting) | S |
| Nested popup stack edge cases | Click relic → weapon → arcana depth | S |

### 2.5 Known bugs (fix for next phase)
| Bug | Severity | Repro | Likely area | Fix ideas |
|-----|----------|-------|-------------|-----------|
| **Level Up tooltip shows without hovering a choice** | High / annoying | Open level-up with cursor off cards → tooltip appears; goes away after hover-on/off an item | False `usingController` from EventSystem auto-select of first card → dwell popup; also possible enter-on-spawn | **Fixed in 1.7.1:** `OnLevelUpOpened` clears popups; selection no longer enables controller mode; grace + pointer check on `ShowItemPopup`; skip HUD hovers while Level Up open |

**Reporter note (2026-08):** Confirmed — unsolicited tooltip on Level Up open; clears after hover-on then hover-off an item. Fix shipped **1.7.1**.

### 2.4 README & in-repo docs
| Task | Why | Effort |
|------|-----|--------|
| Update README features list (map, grimoire, stage Guide) | Installers need current truth | S |
| Version badge / changelog section | Matches releases | S |
| Config keys (`VerboseLogging`, future options) | Support | S |
| Keep credits + links prominent | Attribution | S |
| Short `docs/USER-GUIDE.md` (optional) | Hover delays, Music/Guide tab | S |

---

## 3. Input / accessibility

### 3.1 Controller support (stage select + menus)
| Task | Why | Effort |
|------|-----|--------|
| Stage Guide Music/Guide tabs selectable with gamepad | Parity with mouse | M |
| D-pad/stick between relic icons in Guide + left RelicPanel | Full pad use on stage select | M |
| Reuse existing dwell / submit patterns from collection mode | Code already exists for collection | M |
| Configurable dwell time | Accessibility | S |
| Keyboard focus outline on tabs | Desktop without mouse | S |

### 3.2 Config surface
| Config | Default | Purpose |
|--------|---------|---------|
| `Debug.VerboseLogging` | true (dev) / false (release) | Console noise |
| `Tooltips.HoverDelay` | ~0.4–1.0s | Collection / map / stage |
| `StageGuide.DefaultTab` | Music | Music vs Guide |
| `StageGuide.Enabled` | true | Kill switch |
| `MapTooltips.Enabled` | true | Kill switch |
| `Arcana.ShowOnlyActiveInRun` | false | Optional filter |

---

## 4. Feature expansions (“anything else”)

Prioritize by player value vs maintenance cost.

### 4.1 High value / medium cost
| Feature | Description | Effort |
|---------|-------------|--------|
| **Character select tooltips** | Hover character → starter weapon, evo path, arcana synergies | M–L |
| **Power-up / passive collection tooltips** | Consistent with weapons where still thin | M |
| **Adventure stage Guide** | Reuse Stage Guide hooks on adventure stage pickers if separate UI | M |
| **Hyper / hurry / inverse summary** in Guide | Pull from `StageModifiers` + tick boxes state | M |

### 4.2 Medium value
| Feature | Description | Effort |
|---------|-------------|--------|
| **Unlock requirement hints** | “How is this stage unlocked?” if data/player options expose it | L (data may be incomplete) |
| **Bestiary / secrets page tooltips** | Same item/weapon pattern | M |
| **Merchant special offers** | Already partial; deepen | M |
| **Locale pass** | Extra tips stay English or key into I2 later | S–M |

### 4.3 Lower priority / high cost
| Feature | Description | Effort |
|---------|-------------|--------|
| Full unofficial wiki per stage | Content mill; drifts with patches | XL |
| Arcana selection screen overhaul | Large UI rewrite | XL |
| MelonLoader dual target | Not needed if BepInEx is standard | L |
| Online / multiplayer edge cases | Coop stage `COOP` already special-cased lightly | M |

---

## 5. Nexus-style packaging & release

### 5.1 Artifacts
```
VSEvolutionHelper-BepInEx-v{VERSION}.zip
├── README.md                 (install + credits)
├── CHANGELOG.md
├── BepInEx/
│   └── plugins/
│       └── VSEvolutionHelper/
│           └── VSEvolutionHelper.dll
└── (optional) config defaults note — do not ship machine-specific cfg
```

### 5.2 Versioning
- SemVer: **MAJOR.MINOR.PATCH** (current **1.7.0**)
- Bump `Plugin.PluginVersion` with every player-facing change
- Tag git: `v1.7.0`, etc.

### 5.3 Changelog (keep forever)
- `CHANGELOG.md` with Added / Fixed / Changed per release
- Nexus “Changelogs” tab mirrors the same text

### 5.4 Nexus page checklist
| Item | Content |
|------|---------|
| Title | VS Evolution Helper (BepInEx / 1.15+) |
| Requirements | BepInEx 6 IL2CPP BE; **not** MelonLoader at the same time |
| Description | Features list + screenshots (evo tooltip, grimoire, map, stage Guide) |
| Credits | NihilXD + ashimpure + this port |
| Permissions | Follow original author’s / Nexus rules; credit clearly |
| Soft requirements | VS version tested (e.g. 1.15.113) |
| Sticky post | Install order, common CTDs (wrong loader, game open when updating DLL) |

### 5.5 Release process (repeatable)
1. Update version + CHANGELOG + README  
2. `dotnet build -c Release`  
3. Smoke test: stage select Guide, pause evo, map, grimoire  
4. Zip layout as in 5.1  
5. `git tag vX.Y.Z` + push tag (if remote exists)  
6. Upload to Nexus; paste changelog  
7. Optional: GitHub Release with same zip  

### 5.6 Build / CI (optional later)
| Task | Effort |
|------|--------|
| GitHub Action: build on tag, attach zip | M |
| Path to game interop still local — document `GamePath` in csproj | S |
| Ship without embedding game DLLs | S (already) |

---

## 6. Engineering hygiene

| Task | Why | Effort |
|------|-----|--------|
| Split mega `ItemTooltipsMod.cs` over time | Maintainability | L |
| Reduce reflection where typed interop exists | Stability | M |
| Default `VerboseLogging` = false for release builds | Cleaner player logs | S |
| `.gitignore` already excludes `.tools` / bins | Keep | — |
| Smoke checklist in `docs/SMOKE-TEST.md` | Every release | S |

### Smoke checklist (minimum)
- [ ] Game starts; plugin loads in log  
- [ ] Pause: weapon multi-evo + arcana  
- [ ] Grimoire: left/middle/right formula icons  
- [ ] Map: hover pickup/relic  
- [ ] Stage select: relic hover  
- [ ] Stage select: Music ↔ Guide; wrap + height  
- [ ] No MelonLoader / only BepInEx  

---

## 7. Suggested delivery phases

### Phase 0 — Done
BepInEx core + evo/arcana + grimoire + map + stage A/B/C baseline.

### Phase 1 — “Release candidate polish” — Done (1.7.0)
1. README + CHANGELOG + `VerboseLogging` default false  
2. Stage Guide scroll + empty-section polish  
3. Smoke checklist doc  
4. Nexus-style zip under `dist/`  

**Exit:** someone can install from zip without this repo.

### Phase 2 — Bugs, input & config — Done (1.8.0–1.8.1)
1. **Fix Level Up global tooltip** — **Done (1.7.1–1.7.3 / 1.8.0)**  
2. Hover delay + feature toggles in cfg — **Done (1.8.0)**  
3. Controller: Guide tabs + relic focus — **Done (1.8.1)**  

**Exit:** level-up is usable without stray tooltips; pad-friendly stage select; configurable UX.

### Phase 3 — Content expansions — Mostly done (1.10.0)
1. Character select tooltips — **Done (1.9.x)**  
2. Hyper/modifier summary in Guide — **Done (1.9.0)**  
3. Adventure select tooltips — **Done (1.10.0)**  
4. Broader `StageExtraTips` — **Partial (1.10.0)**; more DLC/adventure notes anytime  

**Exit:** feels like a “full” helper across pre-run and in-run.

### Phase 4 — Packaging automation (later; not every PR)
1. Tag-driven zip — **only when releasing**  
2. Optional GH Action  
3. Nexus update cadence  

### Next polish (no release unless asked)
| Item | Notes |
|------|--------|
| StageExtraTips expansion | More DLC / adventure stage notes |
| Adventure tooltip richness | Weapon icons like character tooltips |
| Stage Guide edge cases | Layout on odd resolutions, empty stages |
| In-run / map / grimoire nits | Whatever shows up while playing unlocked save |
| Config / README polish | Batch into next ship |

---

## 8. Risk register

| Risk | Mitigation |
|------|------------|
| Game update breaks interop | Re-gen interop; re-decompile changed types; pin tested game version on Nexus |
| Song panel layout changes | Fall back Guide below list / full-screen overlay if `_SongPanel` missing |
| Wrong StageType ↔ name mapping | Prefer localized names; tips keyed carefully; log stage id in verbose only |
| DLL locked while game open | Document “close game before update” |
| Double loader | README: Melon off, BepInEx only |

---

## 9. Immediate next action

**Default mode: polish + local install.** No GitHub tag/release unless requested.

Suggested order: play with unlocked save → fix nits → expand tips → (optional) adventure tooltip icons.

---

## 10. Success metrics

- Installs without hand-holding beyond BepInEx  
- No CTDs on stage select / pause / map for tested version  
- Credits always visible on Nexus + README  
- Releases are intentional milestones, not every polish commit  

