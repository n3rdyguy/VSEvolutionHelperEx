# Plan: Full parity with original VS Evolution Helper

**Goal:** Match the original Melon mod’s promise — tooltips on every screen where weapon/item icons appear, with the full formula feature set and solid mouse/keyboard/controller support.

**Baseline:** BepInEx port **1.10.25** already has ~90% of formula UX (multi-row evo, arcana, owned/banned/MAX, nested popups, evolved-from, I2) and several **extras** (Stage Guide, character/adventure tooltips, map). This plan closes **gaps**, not rewrites extras.

**Non-goals (stay out of “parity” scope):** Stage Guide polish, character/adventure, map tokens, StageExtraTips content mill.

**Exit criteria (definition of done):**

1. Every **original screen** below has verified tooltips on representative icons (mouse + pad).
2. Every **original formula feature** works on those screens (owned gold, banned red, MAX, multi-path, nested, evolved-from, localized).
3. Documented smoke matrix checked green; version **1.11.0** tagged as “original parity”.
4. Extras remain optional via existing config kill switches.

---

## Gap matrix (current)

| Area | Status | Gap |
|------|--------|-----|
| Level-up | Strong | Re-verify pad navigate + hover delay |
| Pause inventory (weapons) | Strong | Confirm accessories/passives same path |
| Collection / Grimoire | **Done** | Collections tab hover/placement done (1.10.11–1.10.24). One active icon per formula row is **intended behaviour**, not a gap (owner call, 2026-08-05). Pad dwell on grid still unverified |
| Merchant | **Mouse confirmed** (1.10.24) | Pad/nested unverified; typed SetData patch still worth doing |
| Weapon selectors — Penshin Fatcha + Arma Dio | **Mouse confirmed** (1.10.25) | Both views verified separately. Pad/nested unverified; `EME_SELECTOR` variant still unseen |
| Owned gold circle | Implemented | Verify `PlayerOwnsWeapon` still correct in 1.15 |
| Banned red | Implemented | Visual is red bars not “X”; optional polish |
| MAX labels | Implemented | — |
| Nested recursive | Implemented | Pad enter/exit interactive mode edge cases |
| Evolved-from | Implemented | — |
| Localization | Strong (1.10.7–9) | Spot-check non-English if possible |
| Full input | Partial | Equipment nav + collection interactive + level-up pad matrix |

---

## Phase 0 — Measurement (½–1 day)

**Purpose:** Stop guessing; log what actually fires in-game.

### 0.1 Parity smoke matrix (fill while playing)

Use `docs/SMOKE-TEST.md` plus this table.

**Last session:** 2026-08-05 · plugin **1.10.25** · VS **1.15.113** · owner playtest, **mouse only**.
(Rows below dated 2026-08-04 were confirmed on 1.10.24 and not re-run.)

| Screen | Mouse hover | Nested click | Pad focus dwell | Nested pad submit | Notes |
|--------|-------------|--------------|-----------------|-------------------|-------|
| Level-up weapon | ✅ | — | — | — | |
| Level-up passive | ✅ | — | — | — | |
| Pause weapon slot | ✅ | — | — | — | |
| Pause accessory | ✅ | — | — | — | |
| Merchant offer | ✅ | — | — | — | |
| Collection weapon | ✅ | — | — | — | Docked-panel rework (1.10.11–1.10.24) |
| Grimoire formula L/M/R | ✅ | — | — | — | One active icon per row is by design |
| **Arma Dio list item** | ✅ | ⬜ | ⬜ | ⬜ | Confirmed 2026-08-05 (1.10.25). Base `View - WeaponSelection` + `SetData` — verified separately from Penshin |
| **Penshin Fatcha list item** | ✅ | ⬜ | ⬜ | ⬜ | Fixed and confirmed 2026-08-05 (1.10.25). Binds via `SetPenshinData` under `View - TP_WeaponSelection` |

**Legend:** ✅ confirmed · ⬜ not tested · — not separately reported this session

**Caveat:** only **mouse hover** was reported. Nested click and all controller columns are
unverified, not passing — do not read ✅ rows as input-complete.

### Weapon selectors: one cell type, but NOT one screen (corrected 2026-08-05)

An earlier revision of this plan asserted the selectors were "ONE gap, not two" — that fixing
either fixed both, and that testing either exercised the same code. **Fixing Penshin Fatcha
disproved that.** The shared part is real but shallower than claimed:

| Shared | Not shared |
|--------|-----------|
| Cell type `WeaponSelectionItemUI` | **View root**: base `View - WeaponSelection` vs `View - TP_WeaponSelection` |
| `RegisterWeaponUI` hover path | **Bind method**: `SetData` vs `SetPenshinData` |

Both views exist in `Safe Area` simultaneously; only one is active at a time. So testing
Penshin Fatcha exercises the `TP_` view and the Penshin bind, and says nothing about Arma
Dio's. `WeaponType` also has `EME_SELECTOR = 404` — assume a third variant until seen.

**Three stacked defects caused the total failure**, each masked by the one before it:

| # | Defect | Why it was invisible |
|---|--------|----------------------|
| 1 | Cell type resolved by scanning for an assembly whose name contains `"Il2Cpp"` — a MelonLoader convention; BepInEx interop is unprefixed (`VampireSurvivors.Runtime`) | Logged a warning, but the scan was never reached at all (defect 3) |
| 2 | Only `SetData` was considered; Penshin Fatcha binds via `SetPenshinData` | Would have looked like "some selectors work" |
| 3 | `weaponSelectionView` cached the **base** view while it was briefly active, then went inactive. `IsGamePaused()` derives "a modal is open" from that field, and `ShowItemPopup` returns early when it is false — **with no log line** | Hover fired, `ShowItemPopup` logged, popup was built and silently discarded. Log looked identical to success |

**The transferable lesson:** a cached scene object is only meaningful *while it is active*, and
an early return in a display path must log. Defect 3 cost three build/test cycles purely
because that branch was silent; it now logs, and views are matched by scanning `Safe Area` for
an **active** child whose name contains `WeaponSelection` rather than by a fixed path.

**Penshin is also the best stress test for multi-row evolutions.** `WeaponType` carries
`EX_PENSHIN`, `EX_PENSHIN_EVO1…EVO7`, `EX_PENSHIN_SELECTOR_EVO`, `EX_PENSHIN_UNION`, and
`EX_PENSHIN_STATIC1…STATIC7` — the wiki's "Miracle of Multiplication (6+ evolutions)".
`BuildEvoRowsFor` has no row cap (rows are data-driven), so this is the natural case to
confirm Phase 2's "multi-row recipes" against something harder than Hollow Heart.

### 0.2 Runtime diagnostics (temporary or behind `VerboseLogging`)

| Log when | Message should include |
|----------|------------------------|
| Merchant page show | Page type name, whether icons registered |
| Each merchant icon bind | Weapon/Item type + GO name |
| WeaponSelection scan | Count registered / total cells |
| Grimoire `AddWeaponIcon` | Type + root id + child count |
| EquipmentIconPaused SetData | Weapon + banished flag |

### 0.3 Decompile / interop pass

| Type to capture (if missing) | Why |
|------------------------------|-----|
| Merchant page + offer/item UI | Typed SetData instead of name heuristics |
| `WeaponSelectionItemUI` | Replace Activator/reflection scan |
| Accessory path on pause if separate from `EquipmentIconPaused` | Ensure passives register |

**Deliverable:** Filled matrix + short “broken list” ordered by player impact.

---

## Phase 1 — Screen registration parity (core)

**Theme:** Every icon that shows a weapon/item gets a stable `RegisterWeaponUI` / `RegisterItemUI` / hover target.

### 1.1 Merchant (high priority)

| Step | Action |
|------|--------|
| 1 | Confirm `View - Merchant` active path in 1.15 hierarchy |
| 2 | Find real type: `MerchantPage` / shop item UI / `SetData` methods via interop |
| 3 | Patch **typed** SetData (weapon + item) like LevelUp/Equipment — not only `Show` → cache Data |
| 4 | On Show: delayed rescan of icons (1–3 frames) for pool recycle |
| 5 | Mouse + pad verify buy row tooltips before purchase |

**Files likely:** `ItemTooltipsMod.cs` (`TryPatchMerchantPage`), new `MerchantPatches.cs`, `GenericIconPatches.cs`.

### 1.2 Weapon selection — **done for Penshin Fatcha** (1.10.25)

| Step | Action | State |
|------|--------|-------|
| 1 | Typed `WeaponSelectionItemUI` instead of reflection + `Activator` | Done |
| 2 | Harmony postfix on **both** `SetData` and `SetPenshinData`, instance-only | Done |
| 3 | Resolve the view by scanning `Safe Area` for an active `*WeaponSelection*` child | Done |
| 4 | Re-arm `scannedWeaponSelection` when the view goes inactive or changes | Done |
| 5 | **Open Arma Dio** and confirm the base view path | Done |
| 6 | Controller dwell + nested click on selector cells | **Outstanding** |

Hit target is `_WeaponFrame` → `_Icon`, deliberately *not* the cell root: registering a hover
destroys any existing `EventTrigger` on the target, and the root carries the game's own button
wiring — the same shape as the `CharacterItemUI.SetData` patch that broke character select in
1.9.1. The root is used only when it has no `EventTrigger` to clobber.

**Files:** `WeaponSelectionPatches.cs`, `ItemTooltipsMod.AdoptWeaponSelectionView` /
`FindActiveWeaponSelectionView` / `ScanWeaponSelectionView`.

### 1.3 Pause inventory completeness (medium)

| Step | Action |
|------|--------|
| 1 | Verify all weapon slots + passive/accessory slots register |
| 2 | If accessories use a different component, patch it |
| 3 | Confirm banished equipment still tooltips (`isBanished` already on SetData signature) |
| 4 | Controller equipment selection: dwell → tooltip; Submit → interactive nested |

**Files:** `EquipmentIconPatches.cs`, equipment nav block in `ItemTooltipsMod.cs`.

### 1.4 Collection / Grimoire (medium — finish multi-icon)

| Step | Action |
|------|--------|
| 1 | Per-icon registration already via `AddWeaponIcon`; fix remaining **hit** issues |
| 2 | Prefer world-corner rects of **icon root**; avoid registering only nested sprite |
| 3 | When multiple hits, score by **distance to icon center** among mid-sized rects (already started in 1.10.10) |
| 4 | Optional: exclude registering whole `EvolutionItemUI` row as a single weapon (generic SetWeapon on parent) so one type doesn’t “win” the whole row |
| 5 | Pad dwell on collection grid + grimoire list |

**Files:** `GrimoirePatches.cs`, `UpdateCollectionHover`, filter generic patches when under `EvolutionItemUI`.

### 1.5 Level-up (low — regression)

| Step | Action |
|------|--------|
| 1 | Re-run no-tooltip-on-open + hover works |
| 2 | Pad: navigate choices, dwell, no false first-item popup |
| 3 | Keep `LevelUpTooltips` config kill switch |

---

## Phase 2 — Formula feature parity (polish)

Already implemented; make them **reliable and visible** on every screen from Phase 1.

| Feature | Work |
|---------|------|
| Multi-row recipes | Spot-check Hollow Heart / multi-path; fix data parse if a path missing |
| Arcana list + click | Click + pad submit; Affects list non-empty for known arcanas |
| Owned gold circle | Audit `PlayerOwnsWeapon` / session accessors on 1.15; fix if always false |
| Banned overlay | Keep red bars **or** add simple X graphic for closer original look |
| MAX labels | Ensure `RequiresMax` still set from evo data |
| Nested popups | Mouse click stack; pad: Submit enter interactive, Cancel/B exit; no stuck mode |
| Evolved-from | Hover evolved weapon in collection shows base path |
| Localization | EN required; optional second language smoke |

**Files:** `CreateFormulaIcon`, `PlayerOwnsWeapon`, `IsWeaponBanned`, interactive mode handlers, `GameData.BuildEvoRowsFor`.

---

## Phase 3 — Input parity

| Input | Target behavior |
|-------|-----------------|
| **Mouse** | Hover delay; exit hide; click nested icons |
| **Keyboard** | Focus via EventSystem; same dwell as controller where selection moves |
| **Controller** | Dwell on focused icon; LB/RB not stolen incorrectly; interactive nested with highlight ring |
| **Level-up** | No auto-tooltip from default selection without nav input (already fixed — keep tests) |

### 3.1 Concrete tasks

1. Document bind map (A/Submit, B/Cancel, stick, shoulder) in USER-GUIDE.
2. Single shared “resolve focused weapon/item under EventSystem.currentSelectedGameObject” helper used by level-up, collection, equipment, weapon selection.
3. Equipment nav mode: verify still enables on pause pad; fix if 1.15 renamed pause hierarchy (`View - Pause` etc.).
4. Interactive mode: always clear on page hide / scene change.

---

## Phase 4 — Docs, config, release

| Task | Detail |
|------|--------|
| USER-GUIDE | “Original mod parity” section + screen matrix |
| README features | Align with original wording where true; note extras separately |
| SMOKE-TEST | Expand with parity matrix (Phase 0 table) |
| ROADMAP | Mark Phase 3 extras done; parity plan linked |
| Version | **1.11.0** when matrix green |
| Tag/release | Only after Phase 0–3 exit criteria |

Optional config (only if needed):

- `Features.WeaponSelectionTooltips` (default true)
- `Features.MerchantTooltips` (default true)  
  Prefer one global in-run toggle later rather than config sprawl.

---

## Suggested delivery order

```
Week-ish sequencing (flexible):

  [Phase 0]  Playtest matrix + verbose logs + decompile missing types
       │
       ▼
  [Phase 1.1] Merchant typed registration ────────┐
  [Phase 1.2] Arma Dio typed bind + rescan ───────┼─► biggest original gaps
  [Phase 1.4] Grimoire multi-icon finish ─────────┘
       │
       ▼
  [Phase 1.3] Pause accessories / pad equipment nav
       │
       ▼
  [Phase 2] Owned/banned/nested reliability polish
       │
       ▼
  [Phase 3] Input matrix green
       │
       ▼
  [Phase 4] Docs + 1.11.0 release
```

Extras (Stage Guide, character, adventure, map) keep shipping as-is; **no** requirement to freeze them during parity work.

---

## Effort estimate

| Phase | Effort | Risk |
|-------|--------|------|
| 0 Measurement | S | Low |
| 1.1 Merchant | M | Medium (UI types) |
| 1.2 Arma Dio | M | Medium (hierarchy) |
| 1.3 Pause completeness | S–M | Low–medium |
| 1.4 Grimoire multi-icon | M | Medium (hit tests) |
| 2 Formula polish | S | Low |
| 3 Input | M | Medium |
| 4 Docs/release | S | Low |

**Total:** roughly **1–2 focused weeks** of playtest + coding, depending on how broken Merchant/Arma Dio are in practice.

---

## Risk register

| Risk | Mitigation |
|------|------------|
| 1.15 renamed views | Log Find failures; soft hierarchy search by type not path |
| Il2Cpp type name changes | Typed interop from game-api decompile |
| Generic patches double-register wrong parent | Prefer specific patches; skip EvolutionItemUI parent weapon set |
| Pad input fights game UI | Don’t eat buttons globally; only when our popup interactive |
| Scope creep into extras | Explicit non-goals above |

---

## Immediate next action

1. **Phase 0 play session** with `VerboseLogging = true`: Merchant, Arma Dio, Grimoire L/M/R, pause accessories, level-up pad.  
2. Paste log snippets + filled matrix.  
3. Implement **1.1 Merchant** or **1.2 Arma Dio** first, whichever failed harder in the matrix.

---

## Success metrics

- Original marketing bullets can be claimed honestly in README without asterisks (or with one documented caveat max).  
- SMOKE-TEST parity section all checked.  
- No regression on Level Up “no tooltip on open”.  
- 1.11.0 release notes: “Original-mod screen + formula parity”.
