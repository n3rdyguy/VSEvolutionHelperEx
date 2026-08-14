# Smoke test checklist (pre-release)

Run before tagging or uploading a release zip. Game: **VS 1.15.x**, BepInEx 6 IL2CPP only. Plugin version under test: current `PluginVersion`.

## Setup

- [ ] MelonLoader proxy disabled (no Melon `version.dll`)
- [ ] BepInEx present (`winhttp.dll` / doorstop)
- [ ] Fresh DLL copy with game **closed**, and **md5 matches the build output**
- [ ] `BepInEx/LogOutput.log` shows `VS Evolution Helper {version}` and `Patches applied successfully`
- [ ] `[GameData] Ready:` appears with weapon/item/arcana counts
- [ ] No `Patched … (0 args)` / `SetData not found` warnings - that means a game update renamed a method

## In-run

- [ ] Pause: hover a base weapon (e.g. Whip) → evo row(s) + icons with sprites
- [ ] Multi-path passive/weapon shows **multiple** evo rows when applicable
- [ ] Nested click on evo icon opens nested tooltip
- [ ] Arcana section shows real names (not raw keys); click opens popup with Affects
- [ ] Pause **map**: hover a relic/pickup → name/description
- [ ] **Level Up:** no tooltip until you hover a choice; moving away hides it
- [ ] Tooltip spacing looks readable (title, description, evo, arcana gaps)

## Weapon selectors

- [ ] **Penshin Fatcha** (Para Kooleo start): hover a tuna form → tooltip; clicking still selects
- [ ] **Arma Dio**: same — this is a **different view** (`View - WeaponSelection`, not `TP_`) and a different bind method, so it must be checked separately
- [ ] Open a selector twice in one run: tooltips work the second time too

> **Two views, one cell type.** Both selector views live under `Safe Area` at once and only
> one is active. A tooltip that builds but never draws means the mod latched onto the inactive
> one — with `VerboseLogging`, `ShowItemPopup: no modal UI active` is the tell.

## Collection / Grimoire

- [ ] Grimoire formula icons show weapon tooltips (at least one reliable icon per row is OK)
- [ ] Collection grid: weapon / item / arcana cells tooltip when hovered
- [ ] No continuous error spam when scrolling grimoire

## Stage selection

- [ ] Left **Relics in stage**: hover → tooltip; long names don’t crush layout
- [ ] **Music \| Guide** tabs: switch without breaking music list
- [ ] Tabs aligned to song panel; small gap; labels readable
- [ ] **Guide:** stage name; Tips when present; Features quirks when present; Relics list when present
- [ ] Guide does **not** need to repeat bottom-panel Hyper/length/mods
- [ ] Curated **Guide** notes appear on known stages (e.g. Mad Forest) when defined
- [ ] Stages with **no** relics: no empty clutter
- [ ] Long Guide content scrolls
- [ ] Closing stage select restores music panel
- [ ] **Controller:** LB/RB (or Q/E) switch tabs; Guide scrolls; relic dwell tooltip works
- [ ] **On a save where music is NOT unlocked:** Guide is still reachable (known defect - tracked in the private roadmap)

> **Unlock-state coverage.** Most of this list passes trivially on a fully-unlocked save.
> Anything that reuses a game panel can silently vanish when that panel does not exist yet —
> the Stage Guide is coupled to the song panel exactly this way. When a feature depends on
> another UI element, test it on a **partially-unlocked** save too, not just the dev save.

## List pages (Secrets, Bestiary, Unlocks, Power Up, Music)

Same shape on all five: hover a row, a docked panel appears in the free space beside the list.

- [ ] Each page: hover a row → panel appears with content, not an empty box
- [ ] **Hover the selected row too** — the selection highlight swallows the pointer, and this is
      the case that failed first on Secrets
- [ ] Panel is **not clipped** by the list's scroll mask, and does not follow the cursor
- [ ] Panel is **not dimmed** by the page's own overlay
- [ ] With `VerboseLogging`, `[Tooltip] sorting: … order=` lands on the **same** number every
      hover. A number climbing +10 per hover is the deactivate-before-destroy regression
- [ ] Scroll the list, then hover again — recycled rows still respond
- [ ] **Power Up:** buy a level; the open panel updates in place without moving the mouse
- [ ] **Power Up:** where a projection cannot be trusted, no projection is shown (never a wrong total)
- [ ] **Bestiary:** a DLC enemy shows its icon from the **second** hover (async atlas — expected)
- [ ] **Bestiary:** a variant row names the row, not the family (e.g. "Calamity", not "Spirit")
- [ ] **Music:** a locked track is named from the record, not `-----`
- [ ] **Music:** locked tracks show an icon
- [ ] Spoiler switches off (`SecretSpoilers`, `BestiarySpoilers`, `MusicSpoilers`) → masking restored

## Arcana cards

- [ ] **Collections → Arcana:** hover a card → description + **Affects:** list
- [ ] **Mid-run pick:** start a run with Arcanas enabled, reach the first pick, hover a dealt card
      → tooltip appears (this uses the **Game UI** canvas, not the menu one — the case that draws
      nothing if parenting regresses)
- [ ] A **face-down** card shows nothing
- [ ] Clicking a card still selects it
- [ ] **Heart of Fire** (49 affected): the list continues in a **second panel** on the opposite
      margin, with art and title, and no "+N more"
- [ ] An **unreleased Darkana** reads "Not in this version of the game yet." rather than a bare name
- [ ] **Affected-weapon icons:** hover an icon along the bottom of the info panel → its evolutions
      and unions. Try an arcana affecting **three or fewer** as well as a long one - the panel keeps
      two containers and draws from either
- [ ] The arcana screen still looks **dimmed as before** behind the panel (the fade overlay is left
      drawn; only its raycast blocking is cleared)

## Character & adventure select

- [ ] **Character selection:** hover grid card → flavor, starter, evo icons, stats; click still selects
- [ ] No raw `itemLang/…` / `powerupLang/…` in title or body (1.10.7+)
- [ ] Odd characters/skins (e.g. Merchant line): name human-readable even if flavor is sparse
- [ ] **Adventure selection:** hover adventure → summary (if `AdventureTooltips` on)
- [ ] **Ascension Points:** open it from Adventure select and from an active Adventure; hover all
      four controls → effect, per-point amount, current allocation and unspent points; +/- still work
      and the values refresh after the panel updates

## Config / regression

- [ ] With `VerboseLogging = false`, no continuous `[DBG]` flood
- [ ] Feature kill switches work (`MapTooltips`, `StageGuide`, `CharacterTooltips`, etc.)
- [ ] No spam of exceptions in log during stage select + one short run

## Release package

- [ ] Zip layout: `BepInEx/plugins/VSEvolutionHelper/VSEvolutionHelper.dll`
- [ ] Zip includes README.md + CHANGELOG.md
- [ ] Version in log matches `PluginVersion`, CHANGELOG header, and git tag
- [ ] GitHub release notes list highlights since last public tag
