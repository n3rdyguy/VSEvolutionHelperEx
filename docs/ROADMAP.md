# VS Evolution Helper — Roadmap

**Mod:** BepInEx IL2CPP port of NihilXD’s Evolution Helper for Vampire Survivors 1.15 / Unity 6  
**Current version:** **1.10.24** (Phase 3 complete; Collections polish — released)  
**Docs:** [USER-GUIDE](USER-GUIDE.md) · [README](../README.md) · [CHANGELOG](../CHANGELOG.md)

### Working practices
- **Do not tag/GitHub-release on every polish change.** Iterate: build → local install → commit. Ship `vX.Y.Z` when asked or at a real milestone.
- Batch changelog/version bumps into intentional releases.
- `storage/` is private (gitignored); save tooling under `scripts/`.

**Credits:** [NihilXD](https://github.com/NihilXD/VSEvolutionHelper) · [Nexus #96](https://www.nexusmods.com/vampiresurvivors/mods/96) · [ashimpure 1.14](https://www.nexusmods.com/vampiresurvivors/mods/101)

---

## 1. Shipped features (1.10.24)

| Area | Status | What players get |
|------|--------|------------------|
| BepInEx load on Unity 6 | Done | Works where MelonLoader CTDs |
| Typed `GameData` | Done | Weapons, powerups, items, arcanas, I2, sprites |
| Multi-row evolution tooltips | Done | All recipes; nested clicks |
| Arcana on tooltips | Done | Names, sprites, Affects popups |
| Level Up tooltips | Done | No popup on open; hover only |
| Grimoire / collection | Done | Formula + grid tooltips (multi-icon hit may be partial) |
| Collections tab | Done | Right-docked clickable tooltip, locked **Unlock:** hints, loc scrub (1.10.11–1.10.24) |
| Pause map tooltips | Done | Relics / pickups |
| Stage relics panel | Done | Hover + dynamic sizing |
| Stage Music \| Guide | Done | Polished tabs; Guide content; Features; curated tips |
| Character select tooltips | Done | Flavor, starter, evo, outfits, stats; click-through; loc scrub |
| Adventure select tooltips | Done | Cast / weapon summary |
| Config kill switches + delays | Done | See USER-GUIDE / README |
| Controller stage Guide | Done | LB/RB tabs, scroll, dwell |
| Release packaging | Done | `dist/` zip + GitHub releases on demand |

Full player-facing detail: **[USER-GUIDE.md](USER-GUIDE.md)**.

---

## 2. Known acceptable limitations

| Item | Notes |
|------|--------|
| Grimoire: sometimes one active icon per evo row | Accepted for now; full multi-icon hit is optional later |
| Character flavor missing for rare I2 gaps | Name humanized; description line omitted rather than raw keys |
| StageExtraTips coverage | Main + many DLC/adventure stages; not every StageType |
| Features vs bottom stats | Hyper/length/mods intentionally **not** duplicated in Guide |
| **Stage Guide needs music unlocked** | **Known defect**, not acceptable — Guide is entirely unavailable when the song panel is absent. See §4.1 |

---

## 3. Active plan: original-mod parity → **1.11.0**

Full plan: **[PLAN-original-parity.md](PLAN-original-parity.md)**

| Phase | Focus | Exit |
|-------|--------|------|
| 0 | Playtest matrix + verbose logs | Known broken list |
| 1 | Merchant, Arma Dio, pause accessories, grimoire multi-icon | All original **screens** work |
| 2 | Owned / banned / MAX / nested reliability | All original **formula** features solid |
| 3 | Mouse + keyboard + controller matrix | Input parity |
| 4 | Docs + tag **1.11.0** | Claim original parity honestly |

**Non-goals for that milestone:** Stage Guide / character / adventure / map extras (already beyond original).

---

## 4. Optional future work (after or parallel to parity)

### 4.1 Polish

#### Stage Guide must not depend on the song panel (defect — do this first)

**Symptom:** on a save/character where **music is not unlocked**, the whole Stage Guide
disappears — no `Music | Guide` tabs, no Guide content.

**Cause:** the Guide was designed and built on a save that *had* music unlocked, so the
song panel was assumed to always exist. It is coupled to that panel in two ways:

| Coupling | Where | Effect when song panel is missing |
|----------|-------|-----------------------------------|
| Gate | `StageGuideUI.EnsureChrome` → `page._SongPanel` null ⇒ `return false` (`:209`) | Tabs and Guide root are never created |
| Geometry | `CopyRect(gr, _songPanelRt)` (`:284`, `:515`); `PlaceTabBarAboveSong` (`:237`) | Guide has no rect of its own to fall back on |

So it is not just a hidden tab — the Guide has no independent layout at all.

**Fix direction:**
1. Decouple geometry: derive the Guide rect from the **stage select right column / parent**
   rather than from `_songPanelRt`, and keep the song-panel rect only as a preferred source
   when present.
2. Decouple the gate: let `EnsureChrome` succeed without a song panel.
3. When there is no music to show, drop the tab strip and show the Guide directly in that
   space (a `Music` tab with nothing behind it is worse than no tabs).
4. Keep `Features.StageGuide` as the kill switch.

**Note:** the existing risk-register line *"Song panel layout changes → fall back if
`_SongPanel` missing"* anticipated exactly this. The mitigation was written down but never
implemented.

**Test:** needs a save where music is **not** unlocked — this class of bug is invisible on a
fully-unlocked save. See `SMOKE-TEST.md`.

| Task | Effort |
|------|--------|
| **Stage Guide without song panel** (above) | **M** |
| Expand `StageExtraTips` for remaining DLC / adventures | S–M |
| Map token labels for unknown sprites | M |
| Adventure tooltips as rich as character (icon strip) | M |
| Stage Guide layout on unusual resolutions | S–M |
| Split mega `ItemTooltipsMod.cs` | L |

### 4.2 Features (not scheduled)
| Feature | Effort |
|---------|--------|
| Unlock-requirement hints for stages | L (data-dependent) |
| Bestiary / secrets page tooltips | M |
| Arcana “active run only” filter | S |
| Locale for curated tips | S–M |
| MelonLoader dual target | L (low value) |

### 4.3 Packaging
| Task | Effort |
|------|--------|
| GH Action: build on tag | M |
| Nexus upload cadence | Process |

---

## 5. Delivery phases (history)

| Phase | Versions | Outcome |
|-------|----------|---------|
| 0 — Core | …–1.6.x | Evo/arcana, grimoire, map, stage A/B/C baseline |
| 1 — RC polish | 1.7.0 | README, changelog, zip layout, Guide scroll |
| 2 — Bugs & input | 1.7.1–1.8.1 | Level Up fix, config, controller Guide |
| 3 — Content | 1.9.x–1.10.x | Character + adventure tooltips, StageExtraTips, Guide Features, loc scrub, UI polish |
| 4 — Automation | Later | Tag CI; not every PR |

---

## 6. Release process

1. Update `PluginVersion` + CHANGELOG + USER-GUIDE/README if features changed  
2. `dotnet build -c Release`  
3. Smoke test ([SMOKE-TEST.md](SMOKE-TEST.md))  
4. Zip: `BepInEx/plugins/VSEvolutionHelper/VSEvolutionHelper.dll` + README + CHANGELOG  
5. Tag `vX.Y.Z`, push, GitHub Release (owner account)  
6. Optional Nexus upload  

**Artifact layout:**

```
VSEvolutionHelper-BepInEx-v{VERSION}.zip
├── README.md
├── CHANGELOG.md
└── BepInEx/plugins/VSEvolutionHelper/VSEvolutionHelper.dll
```

---

## 7. Risk register

| Risk | Mitigation |
|------|------------|
| Game update breaks interop | Re-gen interop; pin tested version on release notes |
| Song panel layout changes | Fall back if `_SongPanel` missing |
| DLL locked while game open | Close game before update |
| Double loader | README: BepInEx only |

---

## 8. Success metrics

- Installable from zip with only BepInEx as a dependency  
- No CTDs on stage select / pause / map / character select for tested build  
- Credits always in README + release notes  
- Releases are intentional milestones  

---

## 9. Immediate mode

**Default:** execute **[PLAN-original-parity.md](PLAN-original-parity.md)**. Phase 0 matrix is partly filled (2026-08-04, mouse only): level-up, pause, merchant, collection, grimoire confirmed; **weapon selectors** (Arma Dio, Penshin Fatcha) still untested, and controller columns are unverified across the board.

Next: verify the **weapon selector** view — Arma Dio and Penshin Fatcha share one `View - WeaponSelection` path, so one fix covers both (check for the `WeaponSelectionItemUI type not found` warning first). Then grimoire multi-icon, then the controller pass.

Tag **1.11.0** only when parity exit criteria are met (or owner requests an interim release).
