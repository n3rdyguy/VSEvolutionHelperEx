# Vampire Survivors game API notes (1.15.113 / Unity 6000.0.62f1)
Re-checked against the **1.16 public beta**: every type, field and method below still resolves,
and all patches bind. Two behaviour notes for 1.16 are marked inline.

Decompiled from BepInEx Il2Cpp interop via `ilspycmd`.

Two assemblies matter, and which one holds a type is not obvious:

| Assembly | Holds |
|----------|-------|
| `Assembly-CSharp.dll` | Most gameplay and UI |
| `VampireSurvivors.Runtime.dll` | `MusicData`, `TrackItemUI`, and other later additions |

When a type cannot be found, search both rather than guessing — see [Regeneration](#regeneration).

## Key types

| Type | Role |
|------|------|
| `VampireSurvivors.Data.DataManager` | Central data (not a MonoBehaviour) |
| `VampireSurvivors.Data.Weapons.WeaponData` | Weapon/passive stats + evolution fields |
| `VampireSurvivors.Data.WeaponType` | Enum for weapons **and** passives (e.g. GARLIC) |
| `VampireSurvivors.Data.PowerUpType` | Permanent power-up shop (POWER, REGEN, …) |
| `VampireSurvivors.Data.PowerUp.PowerUpData` | Power-up rows, **one record per rank** |
| `VampireSurvivors.Data.ItemType` | Stage pickups (COIN, GEM, …) - **not** passives |
| `VampireSurvivors.Data.ArcanaData` / `ArcanaType` | Arcana record + enum |
| `VampireSurvivors.Data.SecretData` | Secret rewards - **sparse when handed to the UI**, see below |
| `VampireSurvivors.Achievements.AchievementData` | Unlock rewards as plain strings |
| `VampireSurvivors.Data.Enemies.EnemyData` | Enemy stats, resistances, behaviour |
| `VampireSurvivors.Data.MusicData` | Track credits + unlock source (`VampireSurvivors.Runtime`) |
| `VampireSurvivors.UI.BaseUIPage.Data` | How UI reaches `DataManager` |
| `VampireSurvivors.UI.EquipmentIconPaused` | Pause equipment icons |
| `VampireSurvivors.Data.Characters.CharacterData` | Character/skin stats, starter weapon, description terms |
| `VampireSurvivors.Data.Characters.CharacterItem` | Character roster row (skins/outfits) |
| `VampireSurvivors.UI.CharacterItemUI` | Character select grid card - do **not** patch `SetData` (see CHANGELOG 1.9.1) |

## DataManager API (typed)

```csharp
Dictionary<WeaponType, List<WeaponData>>   GetConvertedWeapons();
Dictionary<PowerUpType, List<PowerUpData>> GetConvertedPowerUpData();
Dictionary<WeaponType, List<WeaponData>>   GetConvertedDlcWeaponData(DlcType dlc);
Dictionary<PowerUpType, List<PowerUpData>> GetConvertedDlcPowerUpData(DlcType dlc);

Dictionary<SecretType, SecretData>  AllSecrets;
Dictionary<ArcanaType, ArcanaData>  AllArcanas;
Dictionary<PowerUpType, JArray>     AllPowerUps;   // one entry per rank
```

`List<>` here is **Il2Cpp** `Il2CppSystem.Collections.Generic.List<>`.

## WeaponData evolution fields

- `bool isEvolution`
- `string evoInto`
- `List<WeaponType> evolvesFrom`
- `List<WeaponType> requires`
- `List<WeaponType> requiresMax`
- `Il2CppStructArray<WeaponType> evoSynergy`
- `string name`, `frameName`, `texture`, `description`, `tips`
- `string GetLocalizedNameTerm(WeaponType)`
- `string GetLocalizedDescriptionTerm(WeaponType)`

## UI hooks used by this mod

| Type | Method | Instance fields read |
|------|--------|----------------------|
| `SecretItemUI` | `SetData` | `_data`, `_type` |
| `EnemyItemUI` | `SetData(EnemyType, int, EnemyData, BestiaryPage, bool hasKilled)` | `_data`, `_type`, `_hasKilled`, `_Name` |
| `AchievementDataUI` | `SetData(AchievementType, AchievementData, DataManager, bool, ContentGroupType)` + `AdventureAchievementType` overload; `Init(AchievementData, DataManager, bool)` | `Init` updates `_data` but leaves both type fields at their default (`ReachLV5`); use current `_data` reward fields, then `Label` |
| `PowerUpItemUI` | `SetData(PowerUpData, PowerUpType, PowerUpsPage, int, int)`, `UpdateAfterPurchase` | `_data`, `_type`, `_maxRank`, `_page`, `Title`, `Icon` |
| `PowerUpsPage` | `Purchase`, `RefundPowerUps`, `ResetAll` | `_playerStats` |
| `AscensionPanel` | `SetData(PlayerOptionsData, AdventureType)`, `RefreshData` | four `AdjustValuePanel`s, `_completionCount`, `_currentSpend` |
| `ArcanaCardUI` | `SetData` **x3 overloads** | `_data`, `_type`, `_Icon`; `GetArcanaType()` |
| `TrackItemUI` | `SetData(string, Sprite, BgmType, MusicData, AdvancedMusicSelection)` | `_data`, `_bgmType`, `_Title`, `_Icon`; `GetMusicData()` |

`ArcanaCardUI` overloads, all of which must be patched because which one binds a card depends on
where it was dealt:

```csharp
void SetData(ArcanaData, ArcanaType, ArcanaMainSelectionPage);   // mid-run pick
void SetData(ArcanaData, ArcanaType, ISetArcanaInfo, bool);      // info panel
void SetData(ArcanaData, ArcanaType, bool, bool);                // owned / locked
```

## EnemyData

| Group | Fields |
|-------|--------|
| Core | `maxHp`, `power`, `speed` / `maxSpeed`, `xp`, `knockback`, `lives` |
| Resistances | `res_Freeze`, `res_Rosary`, `res_Debuffs`, `res_Knockback`, `res_Corridor`, `res_Defang` |
| Weakness | `weak_Fire` |
| Behaviour | `skills`, `passThroughWalls`, `shieldDuration`, `minimum`/`maximumHpScalingLevel` |

Most resistance fields are `Nullable` and absent on ordinary enemies. There is **no display-name
field** — read `EnemyItemUI._Name.text`, which the game has already localized. `bName` is a
*family* name and is wrong on variant rows.

## MusicData

```csharp
string title, author, source, icon;
bool   isUnlocked;
Il2CppSystem.Nullable<StageType>     unlockedByStage;
Il2CppSystem.Nullable<CharacterType> unlockedByCharacter;
Il2CppSystem.Nullable<ItemType>      unlockedByItem;
HyperMod hyperMod;  ForsakenMod forsakenMod;
```

`title` is populated whether or not the track is unlocked; the **page** masks locked rows with
dashes, the record does not.

## EnemyController (for boss HP work)

`IsBoss` / `IsBossEnemy()`, `NormalizedHp`, `Hp`, `_maxHp`, `IsDead`, `EnemyType`, and a spawn
hook in `InitialiseLocalData(EnemyType)`.

**The game's own health bar cannot be reused.** `EnemyController` derives from
`BasePoolableSpriteBehaviour`, *not* `CharacterController`, while both `HealthBar._character` and
`HealthBarUi.Initialize(...)` are typed to `CharacterController`. The `ShowHealthBar` flag is the
player's.

**Enemy instances are pooled** — the same `EnemyController` comes back later as a different
enemy. Key any tracking off the re-init hook and drop it on `IsDead`, never on object identity.

## Sparse records and sentinels

- `SecretData` handed to `SecretItemUI` has every reward field null or `VOID`. The populated
  record is in `DataManager.AllSecrets` keyed by type, with raw JSON behind that.
- `AchievementData` reward fields are **plain strings**, not nullable enums, so they need no
  fallback. The Progress page calls `AchievementDataUI.Init`, which does not set an achievement
  type; looking that id up in JSON makes every row read `ReachLV5` and claim it unlocks Wings.
  Read the record's reward fields directly instead. DLC/custom records can use I2 terms in those
  string fields, so resolve each displayed reward through I2 before falling back to a humanized id.
- An unset typed id reads back as `"VOID"`, sometimes `"0"`, sometimes empty — check all three.
- `PowerUpItemUI._currentLevel` reads `0` for every upgrade on 1.15; use
  `PlayerStats.GetOwnedPowerUps()[type]._Level`. **1.16 appears to have fixed it** - the two now
  agree (`rowLevel=5 statsLevel=5`) - but keep reading PlayerStats, since pricing keys off the
  player's stats anyway and the workaround costs nothing.

## Localization

`I2.Loc.LocalizationManager.GetTranslation(term, ...)` in assembly `l2localization`.

An I2 `Localize` **component** is not the text — the rendered string is on the
`TextMeshProUGUI` sharing its GameObject, and only that has been through localization.

## Regeneration

```powershell
$interop = "<game>\BepInEx\interop"

# One type
ilspycmd -t "VampireSurvivors.Data.MusicData" "$interop\VampireSurvivors.Runtime.dll"

# Which assembly holds a type?
Select-String -Path "$interop\*.dll" -Pattern "TrackItemUI" -List

# All classes in an assembly
ilspycmd -l c "$interop\Assembly-CSharp.dll"
```

`ilspycmd` installs with `dotnet tool install -g ilspycmd`.
