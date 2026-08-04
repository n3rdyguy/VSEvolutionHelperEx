# Vampire Survivors game API notes (1.15.113 / Unity 6000.0.62f1)

Decompiled from BepInEx Il2Cpp interop (`VampireSurvivors.Runtime.dll`) via `ilspycmd`.

## Key types

| Type | Role |
|------|------|
| `VampireSurvivors.Data.DataManager` | Central data (not a MonoBehaviour) |
| `VampireSurvivors.Data.Weapons.WeaponData` | Weapon/passive stats + evolution fields |
| `VampireSurvivors.Data.WeaponType` | Enum for weapons **and** passives (e.g. GARLIC) |
| `VampireSurvivors.Data.PowerUpType` | Permanent power-up shop (POWER, REGEN, …) |
| `VampireSurvivors.Data.PowerUp.PowerUpData` | Power-up rows |
| `VampireSurvivors.Data.ItemType` | Stage pickups (COIN, GEM, …) — **not** passives |
| `VampireSurvivors.UI.BaseUIPage.Data` | How UI reaches `DataManager` |
| `VampireSurvivors.UI.EquipmentIconPaused` | Pause equipment icons |

## DataManager API (typed)

```csharp
Dictionary<WeaponType, List<WeaponData>> GetConvertedWeapons();
Dictionary<PowerUpType, List<PowerUpData>> GetConvertedPowerUpData();
Dictionary<WeaponType, List<WeaponData>> GetConvertedDlcWeaponData(DlcType dlc);
Dictionary<PowerUpType, List<PowerUpData>> GetConvertedDlcPowerUpData(DlcType dlc);
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

## Localization

`I2.Loc.LocalizationManager.GetTranslation(term, ...)` in assembly `l2localization`.

## Regeneration

```powershell
ilspycmd "…\BepInEx\interop\VampireSurvivors.Runtime.dll" -t "VampireSurvivors.Data.DataManager"
```
