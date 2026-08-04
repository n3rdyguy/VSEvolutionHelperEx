using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Newtonsoft.Json.Linq;
using Unity.Profiling;
using UnityEngine;
using VampireSurvivors.Achievements;
using VampireSurvivors.App.Data;
using VampireSurvivors.App.Data.Adventures;
using VampireSurvivors.Data.Characters;
using VampireSurvivors.Data.Enemies;
using VampireSurvivors.Data.Items;
using VampireSurvivors.Data.PowerUp;
using VampireSurvivors.Data.Props;
using VampireSurvivors.Data.Stage;
using VampireSurvivors.Data.Weapons;
using VampireSurvivors.Objects;
using VampireSurvivors.Objects.Algorithm;

namespace VampireSurvivors.Data;

public class DataManager : Il2CppSystem.Object
{
	[System.Serializable]
	[ObfuscatedName("VampireSurvivors.Data.DataManager+<>c")]
	public sealed class __c : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr___9;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__168_0;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__168_1;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__168_2;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__GetBaseGameChars_b__168_0_Internal_Boolean_KeyValuePair_2_CharacterType_List_1_CharacterData_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__GetBaseGameChars_b__168_1_Internal_CharacterType_KeyValuePair_2_CharacterType_List_1_CharacterData_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__GetBaseGameChars_b__168_2_Internal_List_1_CharacterData_KeyValuePair_2_CharacterType_List_1_CharacterData_0;

		public unsafe static __c __9
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<__c>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)_c));
			}
		}

		public unsafe static Il2CppSystem.Func<KeyValuePair<CharacterType, List<CharacterData>>, bool> __9__168_0
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__168_0, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<KeyValuePair<CharacterType, List<CharacterData>>, bool>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__168_0, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<KeyValuePair<CharacterType, List<CharacterData>>, CharacterType> __9__168_1
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__168_1, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<KeyValuePair<CharacterType, List<CharacterData>>, CharacterType>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__168_1, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<KeyValuePair<CharacterType, List<CharacterData>>, List<CharacterData>> __9__168_2
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__168_2, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<KeyValuePair<CharacterType, List<CharacterData>>, List<CharacterData>>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__168_2, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		static __c()
		{
			Il2CppClassPointerStore<__c>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "<>c");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c>.NativeClassPtr);
			NativeFieldInfoPtr___9 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9");
			NativeFieldInfoPtr___9__168_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__168_0");
			NativeFieldInfoPtr___9__168_1 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__168_1");
			NativeFieldInfoPtr___9__168_2 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__168_2");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100727317);
			NativeMethodInfoPtr__GetBaseGameChars_b__168_0_Internal_Boolean_KeyValuePair_2_CharacterType_List_1_CharacterData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100727318);
			NativeMethodInfoPtr__GetBaseGameChars_b__168_1_Internal_CharacterType_KeyValuePair_2_CharacterType_List_1_CharacterData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100727319);
			NativeMethodInfoPtr__GetBaseGameChars_b__168_2_Internal_List_1_CharacterData_KeyValuePair_2_CharacterType_List_1_CharacterData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100727320);
		}

		[CallerCount(754)]
		[CachedScanResults(RefRangeStart = 41, RefRangeEnd = 795, XrefRangeStart = 41, XrefRangeEnd = 795, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe __c()
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<__c>.NativeClassPtr))
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1562860, XrefRangeEnd = 1562864, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe bool _GetBaseGameChars_b__168_0(KeyValuePair<CharacterType, List<CharacterData>> kvp)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)kvp));
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__GetBaseGameChars_b__168_0_Internal_Boolean_KeyValuePair_2_CharacterType_List_1_CharacterData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1562864, XrefRangeEnd = 1562865, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe CharacterType _GetBaseGameChars_b__168_1(KeyValuePair<CharacterType, List<CharacterData>> kvp)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)kvp));
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__GetBaseGameChars_b__168_1_Internal_CharacterType_KeyValuePair_2_CharacterType_List_1_CharacterData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(CharacterType*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1562865, XrefRangeEnd = 1562866, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe List<CharacterData> _GetBaseGameChars_b__168_2(KeyValuePair<CharacterType, List<CharacterData>> kvp)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)kvp));
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__GetBaseGameChars_b__168_2_Internal_List_1_CharacterData_KeyValuePair_2_CharacterType_List_1_CharacterData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<CharacterData>>(intPtr) : null;
		}

		public __c(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr__settings;

	private static readonly System.IntPtr NativeFieldInfoPtr__playerOptions;

	private static readonly System.IntPtr NativeFieldInfoPtr__characterData;

	private static readonly System.IntPtr NativeFieldInfoPtr__powerUpData;

	private static readonly System.IntPtr NativeFieldInfoPtr__stageData;

	private static readonly System.IntPtr NativeFieldInfoPtr__weaponData;

	private static readonly System.IntPtr NativeFieldInfoPtr__enemyData;

	private static readonly System.IntPtr NativeFieldInfoPtr__characterDataChangedForOnline;

	private static readonly System.IntPtr NativeFieldInfoPtr__powerUpDataChangedForOnline;

	private static readonly System.IntPtr NativeFieldInfoPtr__stageDataChangedForOnline;

	private static readonly System.IntPtr NativeFieldInfoPtr__weaponDataChangedForOnline;

	private static readonly System.IntPtr NativeFieldInfoPtr__enemyDataChangedForOnline;

	private static readonly System.IntPtr NativeFieldInfoPtr__dlcCharacterData;

	private static readonly System.IntPtr NativeFieldInfoPtr__dlcPowerUpData;

	private static readonly System.IntPtr NativeFieldInfoPtr__dlcStageData;

	private static readonly System.IntPtr NativeFieldInfoPtr__dlcWeaponData;

	private static readonly System.IntPtr NativeFieldInfoPtr__dlcEnemyData;

	private static readonly System.IntPtr NativeFieldInfoPtr__dlcMusicData;

	private static readonly System.IntPtr NativeFieldInfoPtr__dlcSfxData;

	private static readonly System.IntPtr NativeFieldInfoPtr__mergeSettings;

	private static readonly System.IntPtr NativeFieldInfoPtr__allWeaponDataJson;

	private static readonly System.IntPtr NativeFieldInfoPtr__allCharactersJson;

	private static readonly System.IntPtr NativeFieldInfoPtr__allEnemiesJson;

	private static readonly System.IntPtr NativeFieldInfoPtr__allItemsJson;

	private static readonly System.IntPtr NativeFieldInfoPtr__allPowerUpsJson;

	private static readonly System.IntPtr NativeFieldInfoPtr__allPropsJson;

	private static readonly System.IntPtr NativeFieldInfoPtr__allStagesJson;

	private static readonly System.IntPtr NativeFieldInfoPtr__allArcanasJson;

	private static readonly System.IntPtr NativeFieldInfoPtr__allHitVfxDataJson;

	private static readonly System.IntPtr NativeFieldInfoPtr__allMusicDataJson;

	private static readonly System.IntPtr NativeFieldInfoPtr__allLimitBreakDataJson;

	private static readonly System.IntPtr NativeFieldInfoPtr__allAchievementsJson;

	private static readonly System.IntPtr NativeFieldInfoPtr__allSecretsJson;

	private static readonly System.IntPtr NativeFieldInfoPtr__allAdventuresJson;

	private static readonly System.IntPtr NativeFieldInfoPtr__allStageSetJson;

	private static readonly System.IntPtr NativeFieldInfoPtr__allAdventureStagesJson;

	private static readonly System.IntPtr NativeFieldInfoPtr__allAdventureMerchantsJson;

	private static readonly System.IntPtr NativeFieldInfoPtr__allAlbumData;

	private static readonly System.IntPtr NativeFieldInfoPtr__allCustomMerchantsJson;

	private static readonly System.IntPtr NativeFieldInfoPtr__allCPUJson;

	private static readonly System.IntPtr NativeFieldInfoPtr__adventureCharacterData;

	private static readonly System.IntPtr NativeFieldInfoPtr__adventureStageData;

	private static readonly System.IntPtr NativeFieldInfoPtr__adventureBestiaryData;

	private static readonly System.IntPtr NativeFieldInfoPtr__adventureMerchantsData;

	private static readonly System.IntPtr NativeFieldInfoPtr_MarkerReloadAllData;

	private static readonly System.IntPtr NativeFieldInfoPtr_MarkerLoadDataFromJson;

	private static readonly System.IntPtr NativeFieldInfoPtr_MarkerBuildConvertedData;

	private static readonly System.IntPtr NativeFieldInfoPtr_MarkerLoadBaseJObjects;

	private static readonly System.IntPtr NativeFieldInfoPtr__AllWeaponData_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__AllCharacters_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__AllEnemies_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__AllItems_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__AllPowerUps_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__AllProps_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__AllStages_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__AllArcanas_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__AllHitVfxData_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__AllMusicData_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__AllLimitBreakData_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__AllAchievements_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__AllSecrets_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__AllAdventures_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__AllCPU_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__AllStageSetData_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__AllAdventureMerchantsData_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__AllCustomMerchantsData_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__AllAlbumData_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__AllLoadedAchievements_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__AllBaseGameAchievements_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__AllDlcAchievements_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr_JsonPartFileNameAchievement;

	private static readonly System.IntPtr NativeFieldInfoPtr_JsonPartFileNameArcana;

	private static readonly System.IntPtr NativeFieldInfoPtr_JsonPartFileNameCharacter;

	private static readonly System.IntPtr NativeFieldInfoPtr_JsonPartFileNameEnemy;

	private static readonly System.IntPtr NativeFieldInfoPtr_JsonPartFileNameHitVfx;

	private static readonly System.IntPtr NativeFieldInfoPtr_JsonPartFileNameItem;

	private static readonly System.IntPtr NativeFieldInfoPtr_JsonPartFileNameLimitBreak;

	private static readonly System.IntPtr NativeFieldInfoPtr_JsonPartFileNameMusic;

	private static readonly System.IntPtr NativeFieldInfoPtr_JsonPartFileNamePowerUp;

	private static readonly System.IntPtr NativeFieldInfoPtr_JsonPartFileNameProps;

	private static readonly System.IntPtr NativeFieldInfoPtr_JsonPartFileNameSecrets;

	private static readonly System.IntPtr NativeFieldInfoPtr_JsonPartFileNameStage;

	private static readonly System.IntPtr NativeFieldInfoPtr_JsonPartFileNameWeapon;

	private static readonly System.IntPtr NativeFieldInfoPtr_JsonPartFileNameAlbum;

	private static readonly System.IntPtr NativeFieldInfoPtr_JsonPartFileNameAdventure;

	private static readonly System.IntPtr NativeFieldInfoPtr_JsonPartFileNameAdventuresStageSet;

	private static readonly System.IntPtr NativeFieldInfoPtr_JsonPartFileNameAdventuresMerchants;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_DefaultData_Public_get_DataManagerSettings_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AllWeaponData_Public_get_Dictionary_2_WeaponType_JArray_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_AllWeaponData_Private_set_Void_Dictionary_2_WeaponType_JArray_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AllCharacters_Public_get_Dictionary_2_CharacterType_JArray_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_AllCharacters_Private_set_Void_Dictionary_2_CharacterType_JArray_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AllEnemies_Public_get_Dictionary_2_EnemyType_JArray_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_AllEnemies_Private_set_Void_Dictionary_2_EnemyType_JArray_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AllItems_Public_get_Dictionary_2_ItemType_ItemData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_AllItems_Private_set_Void_Dictionary_2_ItemType_ItemData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AllPowerUps_Public_get_Dictionary_2_PowerUpType_JArray_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_AllPowerUps_Private_set_Void_Dictionary_2_PowerUpType_JArray_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AllProps_Public_get_Dictionary_2_PropType_PropData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_AllProps_Private_set_Void_Dictionary_2_PropType_PropData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AllStages_Public_get_Dictionary_2_StageType_JArray_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_AllStages_Private_set_Void_Dictionary_2_StageType_JArray_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AllArcanas_Public_get_Dictionary_2_ArcanaType_ArcanaData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_AllArcanas_Private_set_Void_Dictionary_2_ArcanaType_ArcanaData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AllHitVfxData_Public_get_Dictionary_2_HitVfxType_HitVfxData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_AllHitVfxData_Private_set_Void_Dictionary_2_HitVfxType_HitVfxData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AllMusicData_Public_get_Dictionary_2_BgmType_MusicData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_AllMusicData_Private_set_Void_Dictionary_2_BgmType_MusicData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AllLimitBreakData_Public_get_Dictionary_2_WeaponType_JArray_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_AllLimitBreakData_Private_set_Void_Dictionary_2_WeaponType_JArray_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AllAchievements_Public_get_Dictionary_2_AchievementType_AchievementData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_AllAchievements_Private_set_Void_Dictionary_2_AchievementType_AchievementData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AllSecrets_Public_get_Dictionary_2_SecretType_SecretData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_AllSecrets_Private_set_Void_Dictionary_2_SecretType_SecretData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AllAdventures_Public_get_Dictionary_2_AdventureType_AdventureData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_AllAdventures_Private_set_Void_Dictionary_2_AdventureType_AdventureData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AllCPU_Public_get_Dictionary_2_AIType_AIData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_AllCPU_Private_set_Void_Dictionary_2_AIType_AIData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AllStageSetData_Public_get_Dictionary_2_StageSetType_JObject_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_AllStageSetData_Private_set_Void_Dictionary_2_StageSetType_JObject_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AllAdventureMerchantsData_Public_get_Dictionary_2_CharacterType_CustomMerchantData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_AllAdventureMerchantsData_Private_set_Void_Dictionary_2_CharacterType_CustomMerchantData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AllCustomMerchantsData_Public_get_Dictionary_2_CharacterType_CustomMerchantData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_AllCustomMerchantsData_Private_set_Void_Dictionary_2_CharacterType_CustomMerchantData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AllAlbumData_Public_get_Dictionary_2_AlbumType_AlbumData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_AllAlbumData_Private_set_Void_Dictionary_2_AlbumType_AlbumData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AllLoadedAchievements_Public_get_HashSet_1_AchievementType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_AllLoadedAchievements_Private_set_Void_HashSet_1_AchievementType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AllBaseGameAchievements_Public_get_HashSet_1_AchievementType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_AllBaseGameAchievements_Private_set_Void_HashSet_1_AchievementType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AllDlcAchievements_Public_get_Dictionary_2_DlcType_List_1_AchievementType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_AllDlcAchievements_Private_set_Void_Dictionary_2_DlcType_List_1_AchievementType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AllDlcCharacterData_Public_get_Dictionary_2_DlcType_Dictionary_2_CharacterType_List_1_CharacterData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AllDlcPowerUpData_Public_get_Dictionary_2_DlcType_Dictionary_2_PowerUpType_List_1_PowerUpData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AllDlcStageData_Public_get_Dictionary_2_DlcType_Dictionary_2_StageType_List_1_StageData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AllDlcWeaponData_Public_get_Dictionary_2_DlcType_Dictionary_2_WeaponType_List_1_WeaponData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AllDlcEnemyData_Public_get_Dictionary_2_DlcType_Dictionary_2_EnemyType_List_1_EnemyData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AllDlcMusicData_Public_get_Dictionary_2_DlcType_Dictionary_2_BgmType_MusicData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AllDlcSfxData_Public_get_Dictionary_2_DlcType_HashSet_1_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AdventureCharacterData_Public_get_Dictionary_2_CharacterType_List_1_CharacterData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AdventureStageData_Public_get_Dictionary_2_StageType_List_1_StageData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AdventureBestiaryData_Public_get_Dictionary_2_EnemyType_List_1_EnemyData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Initialize_Public_Virtual_Final_New_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Dispose_Public_Virtual_Final_New_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ReloadAllData_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetConvertedDlcCharacterData_Public_Dictionary_2_CharacterType_List_1_CharacterData_DlcType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetConvertedDlcStageData_Public_Dictionary_2_StageType_List_1_StageData_DlcType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetConvertedDlcWeaponData_Public_Dictionary_2_WeaponType_List_1_WeaponData_DlcType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetConvertedDlcEnemyData_Public_Dictionary_2_EnemyType_List_1_EnemyData_DlcType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetConvertedDlcPowerUpData_Public_Dictionary_2_PowerUpType_List_1_PowerUpData_DlcType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetConvertedDlcMusicData_Public_Dictionary_2_BgmType_MusicData_DlcType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetConvertedCharacterData_Public_Dictionary_2_CharacterType_List_1_CharacterData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetBaseGameChars_Public_Dictionary_2_CharacterType_List_1_CharacterData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetConvertedEnemyData_Public_Dictionary_2_EnemyType_List_1_EnemyData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetConvertedPowerUpData_Public_Dictionary_2_PowerUpType_List_1_PowerUpData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetConvertedStages_Public_Dictionary_2_StageType_List_1_StageData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetConvertedAdventureStages_Public_Dictionary_2_StageType_List_1_StageData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetConvertedWeapons_Public_Dictionary_2_WeaponType_List_1_WeaponData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetPropData_Public_PropData_PropType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddDefaultUnlocksToSaveData_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_UpdateAllCharacterHiddenPropertiesForAdventures_Public_Void_AdventureData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GenerateAdventureSpecificData_Public_Void_AdventureData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ExitAdventure_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsOnline_Private_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LoadBaseJObjects_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LoadDataFromJson_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MergeInJsonData_Public_Void_DataManagerSettings_DlcType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InternalMergeInJsonData_Private_Void_DataManagerSettings_DlcType_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CacheBaseGameLoadedAchievements_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MergeInDlcAchievements_Public_Void_DlcType_TextAsset_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MergeInSFXTypes_Public_Void_DlcType_Transform_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_LoadAndMergeIn_Private_Void_JObject_TextAsset_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_BuildConvertedDlcData_Private_Void_DataManagerSettings_DlcType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClearConvertedDlcData_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClearConvertedData_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_BuildConvertedData_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ConvertEnemyDataJsonToObjects_Private_Static_Dictionary_2_EnemyType_List_1_EnemyData_Dictionary_2_EnemyType_JArray_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ConvertWeaponDataJsonToObjects_Private_Static_Dictionary_2_WeaponType_List_1_WeaponData_Dictionary_2_WeaponType_JArray_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ConvertStageDataJsonToObjects_Private_Static_Dictionary_2_StageType_List_1_StageData_Dictionary_2_StageType_JArray_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ConvertCharacterJsonDataToObjects_Private_Static_Dictionary_2_CharacterType_List_1_CharacterData_Dictionary_2_CharacterType_JArray_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ConvertPowerUpJsonData_Private_Static_Dictionary_2_PowerUpType_List_1_PowerUpData_Dictionary_2_PowerUpType_JArray_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ConvertMusicJsonDataToObjects_Private_Static_Dictionary_2_BgmType_List_1_MusicData_Dictionary_2_BgmType_JArray_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CacheEnemyDataStrings_Private_Static_Void_EnemyData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AdjustAchievementDataWithTypes_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AdjustAdventureProgressDataWithTypes_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GenerateBestiaryDataForAdventure_Private_Void_AdventureData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_FindEnemyBaseVariant_Private_Nullable_1_EnemyType_EnemyType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AllJsonPartFileNames_Public_Static_get_List_1_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe DataManagerSettings _settings
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__settings);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<DataManagerSettings>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__settings)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dataManagerSettings));
		}
	}

	public unsafe PlayerOptions _playerOptions
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__playerOptions);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PlayerOptions>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__playerOptions)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)playerOptions));
		}
	}

	public unsafe Dictionary<CharacterType, List<CharacterData>> _characterData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__characterData);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<CharacterType, List<CharacterData>>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__characterData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<PowerUpType, List<PowerUpData>> _powerUpData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__powerUpData);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<PowerUpType, List<PowerUpData>>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__powerUpData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<StageType, List<StageData>> _stageData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__stageData);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<StageType, List<StageData>>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__stageData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<WeaponType, List<WeaponData>> _weaponData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__weaponData);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<WeaponType, List<WeaponData>>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__weaponData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<EnemyType, List<EnemyData>> _enemyData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__enemyData);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<EnemyType, List<EnemyData>>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__enemyData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe bool _characterDataChangedForOnline
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__characterDataChangedForOnline);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__characterDataChangedForOnline)) = flag;
		}
	}

	public unsafe bool _powerUpDataChangedForOnline
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__powerUpDataChangedForOnline);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__powerUpDataChangedForOnline)) = flag;
		}
	}

	public unsafe bool _stageDataChangedForOnline
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__stageDataChangedForOnline);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__stageDataChangedForOnline)) = flag;
		}
	}

	public unsafe bool _weaponDataChangedForOnline
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__weaponDataChangedForOnline);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__weaponDataChangedForOnline)) = flag;
		}
	}

	public unsafe bool _enemyDataChangedForOnline
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__enemyDataChangedForOnline);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__enemyDataChangedForOnline)) = flag;
		}
	}

	public unsafe Dictionary<DlcType, Dictionary<CharacterType, List<CharacterData>>> _dlcCharacterData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__dlcCharacterData);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<DlcType, Dictionary<CharacterType, List<CharacterData>>>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__dlcCharacterData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<DlcType, Dictionary<PowerUpType, List<PowerUpData>>> _dlcPowerUpData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__dlcPowerUpData);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<DlcType, Dictionary<PowerUpType, List<PowerUpData>>>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__dlcPowerUpData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<DlcType, Dictionary<StageType, List<StageData>>> _dlcStageData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__dlcStageData);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<DlcType, Dictionary<StageType, List<StageData>>>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__dlcStageData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<DlcType, Dictionary<WeaponType, List<WeaponData>>> _dlcWeaponData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__dlcWeaponData);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<DlcType, Dictionary<WeaponType, List<WeaponData>>>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__dlcWeaponData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<DlcType, Dictionary<EnemyType, List<EnemyData>>> _dlcEnemyData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__dlcEnemyData);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<DlcType, Dictionary<EnemyType, List<EnemyData>>>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__dlcEnemyData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<DlcType, Dictionary<BgmType, MusicData>> _dlcMusicData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__dlcMusicData);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<DlcType, Dictionary<BgmType, MusicData>>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__dlcMusicData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<DlcType, HashSet<string>> _dlcSfxData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__dlcSfxData);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<DlcType, HashSet<string>>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__dlcSfxData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe JsonMergeSettings _mergeSettings
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mergeSettings);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<JsonMergeSettings>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mergeSettings)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)jsonMergeSettings));
		}
	}

	public unsafe JObject _allWeaponDataJson
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allWeaponDataJson);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<JObject>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allWeaponDataJson)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)jObject));
		}
	}

	public unsafe JObject _allCharactersJson
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allCharactersJson);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<JObject>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allCharactersJson)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)jObject));
		}
	}

	public unsafe JObject _allEnemiesJson
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allEnemiesJson);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<JObject>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allEnemiesJson)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)jObject));
		}
	}

	public unsafe JObject _allItemsJson
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allItemsJson);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<JObject>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allItemsJson)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)jObject));
		}
	}

	public unsafe JObject _allPowerUpsJson
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allPowerUpsJson);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<JObject>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allPowerUpsJson)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)jObject));
		}
	}

	public unsafe JObject _allPropsJson
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allPropsJson);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<JObject>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allPropsJson)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)jObject));
		}
	}

	public unsafe JObject _allStagesJson
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allStagesJson);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<JObject>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allStagesJson)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)jObject));
		}
	}

	public unsafe JObject _allArcanasJson
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allArcanasJson);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<JObject>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allArcanasJson)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)jObject));
		}
	}

	public unsafe JObject _allHitVfxDataJson
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allHitVfxDataJson);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<JObject>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allHitVfxDataJson)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)jObject));
		}
	}

	public unsafe JObject _allMusicDataJson
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allMusicDataJson);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<JObject>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allMusicDataJson)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)jObject));
		}
	}

	public unsafe JObject _allLimitBreakDataJson
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allLimitBreakDataJson);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<JObject>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allLimitBreakDataJson)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)jObject));
		}
	}

	public unsafe JObject _allAchievementsJson
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allAchievementsJson);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<JObject>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allAchievementsJson)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)jObject));
		}
	}

	public unsafe JObject _allSecretsJson
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allSecretsJson);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<JObject>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allSecretsJson)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)jObject));
		}
	}

	public unsafe JObject _allAdventuresJson
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allAdventuresJson);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<JObject>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allAdventuresJson)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)jObject));
		}
	}

	public unsafe JObject _allStageSetJson
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allStageSetJson);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<JObject>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allStageSetJson)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)jObject));
		}
	}

	public unsafe JObject _allAdventureStagesJson
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allAdventureStagesJson);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<JObject>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allAdventureStagesJson)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)jObject));
		}
	}

	public unsafe JObject _allAdventureMerchantsJson
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allAdventureMerchantsJson);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<JObject>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allAdventureMerchantsJson)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)jObject));
		}
	}

	public unsafe JObject _allAlbumData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allAlbumData);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<JObject>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allAlbumData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)jObject));
		}
	}

	public unsafe JObject _allCustomMerchantsJson
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allCustomMerchantsJson);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<JObject>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allCustomMerchantsJson)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)jObject));
		}
	}

	public unsafe JObject _allCPUJson
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allCPUJson);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<JObject>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allCPUJson)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)jObject));
		}
	}

	public unsafe Dictionary<CharacterType, List<CharacterData>> _adventureCharacterData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__adventureCharacterData);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<CharacterType, List<CharacterData>>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__adventureCharacterData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<StageType, List<StageData>> _adventureStageData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__adventureStageData);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<StageType, List<StageData>>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__adventureStageData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<EnemyType, List<EnemyData>> _adventureBestiaryData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__adventureBestiaryData);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<EnemyType, List<EnemyData>>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__adventureBestiaryData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<CharacterType, CustomMerchantData> _adventureMerchantsData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__adventureMerchantsData);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<CharacterType, CustomMerchantData>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__adventureMerchantsData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe static ProfilerMarker MarkerReloadAllData
	{
		get
		{
			Unsafe.SkipInit(out ProfilerMarker result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MarkerReloadAllData, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MarkerReloadAllData, (void*)(&profilerMarker));
		}
	}

	public unsafe static ProfilerMarker MarkerLoadDataFromJson
	{
		get
		{
			Unsafe.SkipInit(out ProfilerMarker result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MarkerLoadDataFromJson, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MarkerLoadDataFromJson, (void*)(&profilerMarker));
		}
	}

	public unsafe static ProfilerMarker MarkerBuildConvertedData
	{
		get
		{
			Unsafe.SkipInit(out ProfilerMarker result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MarkerBuildConvertedData, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MarkerBuildConvertedData, (void*)(&profilerMarker));
		}
	}

	public unsafe static ProfilerMarker MarkerLoadBaseJObjects
	{
		get
		{
			Unsafe.SkipInit(out ProfilerMarker result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_MarkerLoadBaseJObjects, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_MarkerLoadBaseJObjects, (void*)(&profilerMarker));
		}
	}

	public unsafe Dictionary<WeaponType, JArray> _AllWeaponData_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllWeaponData_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<WeaponType, JArray>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllWeaponData_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<CharacterType, JArray> _AllCharacters_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllCharacters_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<CharacterType, JArray>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllCharacters_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<EnemyType, JArray> _AllEnemies_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllEnemies_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<EnemyType, JArray>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllEnemies_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<ItemType, ItemData> _AllItems_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllItems_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<ItemType, ItemData>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllItems_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<PowerUpType, JArray> _AllPowerUps_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllPowerUps_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<PowerUpType, JArray>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllPowerUps_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<PropType, PropData> _AllProps_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllProps_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<PropType, PropData>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllProps_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<StageType, JArray> _AllStages_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllStages_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<StageType, JArray>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllStages_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<ArcanaType, ArcanaData> _AllArcanas_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllArcanas_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<ArcanaType, ArcanaData>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllArcanas_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<HitVfxType, HitVfxData> _AllHitVfxData_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllHitVfxData_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<HitVfxType, HitVfxData>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllHitVfxData_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<BgmType, MusicData> _AllMusicData_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllMusicData_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<BgmType, MusicData>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllMusicData_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<WeaponType, JArray> _AllLimitBreakData_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllLimitBreakData_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<WeaponType, JArray>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllLimitBreakData_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<AchievementType, AchievementData> _AllAchievements_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllAchievements_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<AchievementType, AchievementData>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllAchievements_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<SecretType, SecretData> _AllSecrets_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllSecrets_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<SecretType, SecretData>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllSecrets_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<AdventureType, AdventureData> _AllAdventures_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllAdventures_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<AdventureType, AdventureData>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllAdventures_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<AIType, AIData> _AllCPU_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllCPU_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<AIType, AIData>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllCPU_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<StageSetType, JObject> _AllStageSetData_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllStageSetData_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<StageSetType, JObject>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllStageSetData_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<CharacterType, CustomMerchantData> _AllAdventureMerchantsData_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllAdventureMerchantsData_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<CharacterType, CustomMerchantData>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllAdventureMerchantsData_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<CharacterType, CustomMerchantData> _AllCustomMerchantsData_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllCustomMerchantsData_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<CharacterType, CustomMerchantData>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllCustomMerchantsData_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe Dictionary<AlbumType, AlbumData> _AllAlbumData_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllAlbumData_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<AlbumType, AlbumData>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllAlbumData_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe HashSet<AchievementType> _AllLoadedAchievements_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllLoadedAchievements_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<HashSet<AchievementType>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllLoadedAchievements_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)hashSet));
		}
	}

	public unsafe HashSet<AchievementType> _AllBaseGameAchievements_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllBaseGameAchievements_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<HashSet<AchievementType>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllBaseGameAchievements_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)hashSet));
		}
	}

	public unsafe Dictionary<DlcType, List<AchievementType>> _AllDlcAchievements_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllDlcAchievements_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<DlcType, List<AchievementType>>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AllDlcAchievements_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe static string JsonPartFileNameAchievement
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_JsonPartFileNameAchievement, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_JsonPartFileNameAchievement, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string JsonPartFileNameArcana
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_JsonPartFileNameArcana, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_JsonPartFileNameArcana, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string JsonPartFileNameCharacter
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_JsonPartFileNameCharacter, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_JsonPartFileNameCharacter, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string JsonPartFileNameEnemy
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_JsonPartFileNameEnemy, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_JsonPartFileNameEnemy, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string JsonPartFileNameHitVfx
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_JsonPartFileNameHitVfx, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_JsonPartFileNameHitVfx, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string JsonPartFileNameItem
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_JsonPartFileNameItem, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_JsonPartFileNameItem, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string JsonPartFileNameLimitBreak
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_JsonPartFileNameLimitBreak, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_JsonPartFileNameLimitBreak, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string JsonPartFileNameMusic
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_JsonPartFileNameMusic, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_JsonPartFileNameMusic, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string JsonPartFileNamePowerUp
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_JsonPartFileNamePowerUp, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_JsonPartFileNamePowerUp, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string JsonPartFileNameProps
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_JsonPartFileNameProps, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_JsonPartFileNameProps, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string JsonPartFileNameSecrets
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_JsonPartFileNameSecrets, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_JsonPartFileNameSecrets, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string JsonPartFileNameStage
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_JsonPartFileNameStage, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_JsonPartFileNameStage, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string JsonPartFileNameWeapon
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_JsonPartFileNameWeapon, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_JsonPartFileNameWeapon, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string JsonPartFileNameAlbum
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_JsonPartFileNameAlbum, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_JsonPartFileNameAlbum, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string JsonPartFileNameAdventure
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_JsonPartFileNameAdventure, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_JsonPartFileNameAdventure, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string JsonPartFileNameAdventuresStageSet
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_JsonPartFileNameAdventuresStageSet, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_JsonPartFileNameAdventuresStageSet, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe static string JsonPartFileNameAdventuresMerchants
	{
		get
		{
			Unsafe.SkipInit(out System.IntPtr intPtr);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_JsonPartFileNameAdventuresMerchants, (void*)(&intPtr));
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_JsonPartFileNameAdventuresMerchants, (void*)IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe DataManagerSettings DefaultData
	{
		[CallerCount(12)]
		[CachedScanResults(RefRangeStart = 870178, RefRangeEnd = 870190, XrefRangeStart = 870178, XrefRangeEnd = 870190, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_DefaultData_Public_get_DataManagerSettings_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<DataManagerSettings>(intPtr) : null;
		}
	}

	public unsafe Dictionary<WeaponType, JArray> AllWeaponData
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AllWeaponData_Public_get_Dictionary_2_WeaponType_JArray_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<WeaponType, JArray>>(intPtr) : null;
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 1562866, RefRangeEnd = 1562867, XrefRangeStart = 1562866, XrefRangeEnd = 1562866, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_AllWeaponData_Private_set_Void_Dictionary_2_WeaponType_JArray_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Dictionary<CharacterType, JArray> AllCharacters
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AllCharacters_Public_get_Dictionary_2_CharacterType_JArray_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<CharacterType, JArray>>(intPtr) : null;
		}
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 1123920, RefRangeEnd = 1123924, XrefRangeStart = 1123920, XrefRangeEnd = 1123924, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_AllCharacters_Private_set_Void_Dictionary_2_CharacterType_JArray_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Dictionary<EnemyType, JArray> AllEnemies
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AllEnemies_Public_get_Dictionary_2_EnemyType_JArray_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<EnemyType, JArray>>(intPtr) : null;
		}
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 1172820, RefRangeEnd = 1172823, XrefRangeStart = 1172820, XrefRangeEnd = 1172823, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_AllEnemies_Private_set_Void_Dictionary_2_EnemyType_JArray_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Dictionary<ItemType, ItemData> AllItems
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AllItems_Public_get_Dictionary_2_ItemType_ItemData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<ItemType, ItemData>>(intPtr) : null;
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_AllItems_Private_set_Void_Dictionary_2_ItemType_ItemData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Dictionary<PowerUpType, JArray> AllPowerUps
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AllPowerUps_Public_get_Dictionary_2_PowerUpType_JArray_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<PowerUpType, JArray>>(intPtr) : null;
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 1221051, RefRangeEnd = 1221052, XrefRangeStart = 1221051, XrefRangeEnd = 1221052, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_AllPowerUps_Private_set_Void_Dictionary_2_PowerUpType_JArray_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Dictionary<PropType, PropData> AllProps
	{
		[CallerCount(6)]
		[CachedScanResults(RefRangeStart = 1185953, RefRangeEnd = 1185959, XrefRangeStart = 1185953, XrefRangeEnd = 1185959, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AllProps_Public_get_Dictionary_2_PropType_PropData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<PropType, PropData>>(intPtr) : null;
		}
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 1205870, RefRangeEnd = 1205872, XrefRangeStart = 1205870, XrefRangeEnd = 1205872, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_AllProps_Private_set_Void_Dictionary_2_PropType_PropData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Dictionary<StageType, JArray> AllStages
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AllStages_Public_get_Dictionary_2_StageType_JArray_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<StageType, JArray>>(intPtr) : null;
		}
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 1205872, RefRangeEnd = 1205874, XrefRangeStart = 1205872, XrefRangeEnd = 1205874, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_AllStages_Private_set_Void_Dictionary_2_StageType_JArray_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Dictionary<ArcanaType, ArcanaData> AllArcanas
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AllArcanas_Public_get_Dictionary_2_ArcanaType_ArcanaData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<ArcanaType, ArcanaData>>(intPtr) : null;
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_AllArcanas_Private_set_Void_Dictionary_2_ArcanaType_ArcanaData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Dictionary<HitVfxType, HitVfxData> AllHitVfxData
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AllHitVfxData_Public_get_Dictionary_2_HitVfxType_HitVfxData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<HitVfxType, HitVfxData>>(intPtr) : null;
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 1038917, RefRangeEnd = 1038918, XrefRangeStart = 1038917, XrefRangeEnd = 1038918, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_AllHitVfxData_Private_set_Void_Dictionary_2_HitVfxType_HitVfxData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Dictionary<BgmType, MusicData> AllMusicData
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AllMusicData_Public_get_Dictionary_2_BgmType_MusicData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<BgmType, MusicData>>(intPtr) : null;
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 1176201, RefRangeEnd = 1176202, XrefRangeStart = 1176201, XrefRangeEnd = 1176202, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_AllMusicData_Private_set_Void_Dictionary_2_BgmType_MusicData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Dictionary<WeaponType, JArray> AllLimitBreakData
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AllLimitBreakData_Public_get_Dictionary_2_WeaponType_JArray_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<WeaponType, JArray>>(intPtr) : null;
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_AllLimitBreakData_Private_set_Void_Dictionary_2_WeaponType_JArray_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Dictionary<AchievementType, AchievementData> AllAchievements
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AllAchievements_Public_get_Dictionary_2_AchievementType_AchievementData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<AchievementType, AchievementData>>(intPtr) : null;
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_AllAchievements_Private_set_Void_Dictionary_2_AchievementType_AchievementData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Dictionary<SecretType, SecretData> AllSecrets
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AllSecrets_Public_get_Dictionary_2_SecretType_SecretData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<SecretType, SecretData>>(intPtr) : null;
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_AllSecrets_Private_set_Void_Dictionary_2_SecretType_SecretData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Dictionary<AdventureType, AdventureData> AllAdventures
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AllAdventures_Public_get_Dictionary_2_AdventureType_AdventureData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<AdventureType, AdventureData>>(intPtr) : null;
		}
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 1201393, RefRangeEnd = 1201395, XrefRangeStart = 1201393, XrefRangeEnd = 1201395, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_AllAdventures_Private_set_Void_Dictionary_2_AdventureType_AdventureData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Dictionary<AIType, AIData> AllCPU
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AllCPU_Public_get_Dictionary_2_AIType_AIData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<AIType, AIData>>(intPtr) : null;
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_AllCPU_Private_set_Void_Dictionary_2_AIType_AIData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Dictionary<StageSetType, JObject> AllStageSetData
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AllStageSetData_Public_get_Dictionary_2_StageSetType_JObject_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<StageSetType, JObject>>(intPtr) : null;
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 1276704, RefRangeEnd = 1276705, XrefRangeStart = 1276704, XrefRangeEnd = 1276705, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_AllStageSetData_Private_set_Void_Dictionary_2_StageSetType_JObject_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Dictionary<CharacterType, CustomMerchantData> AllAdventureMerchantsData
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AllAdventureMerchantsData_Public_get_Dictionary_2_CharacterType_CustomMerchantData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<CharacterType, CustomMerchantData>>(intPtr) : null;
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_AllAdventureMerchantsData_Private_set_Void_Dictionary_2_CharacterType_CustomMerchantData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Dictionary<CharacterType, CustomMerchantData> AllCustomMerchantsData
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AllCustomMerchantsData_Public_get_Dictionary_2_CharacterType_CustomMerchantData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<CharacterType, CustomMerchantData>>(intPtr) : null;
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_AllCustomMerchantsData_Private_set_Void_Dictionary_2_CharacterType_CustomMerchantData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Dictionary<AlbumType, AlbumData> AllAlbumData
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AllAlbumData_Public_get_Dictionary_2_AlbumType_AlbumData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<AlbumType, AlbumData>>(intPtr) : null;
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_AllAlbumData_Private_set_Void_Dictionary_2_AlbumType_AlbumData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe HashSet<AchievementType> AllLoadedAchievements
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AllLoadedAchievements_Public_get_HashSet_1_AchievementType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<HashSet<AchievementType>>(intPtr) : null;
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_AllLoadedAchievements_Private_set_Void_HashSet_1_AchievementType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe HashSet<AchievementType> AllBaseGameAchievements
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AllBaseGameAchievements_Public_get_HashSet_1_AchievementType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<HashSet<AchievementType>>(intPtr) : null;
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_AllBaseGameAchievements_Private_set_Void_HashSet_1_AchievementType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Dictionary<DlcType, List<AchievementType>> AllDlcAchievements
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AllDlcAchievements_Public_get_Dictionary_2_DlcType_List_1_AchievementType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<DlcType, List<AchievementType>>>(intPtr) : null;
		}
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 1177598, RefRangeEnd = 1177600, XrefRangeStart = 1177598, XrefRangeEnd = 1177600, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_AllDlcAchievements_Private_set_Void_Dictionary_2_DlcType_List_1_AchievementType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Dictionary<DlcType, Dictionary<CharacterType, List<CharacterData>>> AllDlcCharacterData
	{
		[CallerCount(5)]
		[CachedScanResults(RefRangeStart = 902043, RefRangeEnd = 902048, XrefRangeStart = 902043, XrefRangeEnd = 902048, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AllDlcCharacterData_Public_get_Dictionary_2_DlcType_Dictionary_2_CharacterType_List_1_CharacterData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<DlcType, Dictionary<CharacterType, List<CharacterData>>>>(intPtr) : null;
		}
	}

	public unsafe Dictionary<DlcType, Dictionary<PowerUpType, List<PowerUpData>>> AllDlcPowerUpData
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AllDlcPowerUpData_Public_get_Dictionary_2_DlcType_Dictionary_2_PowerUpType_List_1_PowerUpData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<DlcType, Dictionary<PowerUpType, List<PowerUpData>>>>(intPtr) : null;
		}
	}

	public unsafe Dictionary<DlcType, Dictionary<StageType, List<StageData>>> AllDlcStageData
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AllDlcStageData_Public_get_Dictionary_2_DlcType_Dictionary_2_StageType_List_1_StageData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<DlcType, Dictionary<StageType, List<StageData>>>>(intPtr) : null;
		}
	}

	public unsafe Dictionary<DlcType, Dictionary<WeaponType, List<WeaponData>>> AllDlcWeaponData
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AllDlcWeaponData_Public_get_Dictionary_2_DlcType_Dictionary_2_WeaponType_List_1_WeaponData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<DlcType, Dictionary<WeaponType, List<WeaponData>>>>(intPtr) : null;
		}
	}

	public unsafe Dictionary<DlcType, Dictionary<EnemyType, List<EnemyData>>> AllDlcEnemyData
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AllDlcEnemyData_Public_get_Dictionary_2_DlcType_Dictionary_2_EnemyType_List_1_EnemyData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<DlcType, Dictionary<EnemyType, List<EnemyData>>>>(intPtr) : null;
		}
	}

	public unsafe Dictionary<DlcType, Dictionary<BgmType, MusicData>> AllDlcMusicData
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AllDlcMusicData_Public_get_Dictionary_2_DlcType_Dictionary_2_BgmType_MusicData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<DlcType, Dictionary<BgmType, MusicData>>>(intPtr) : null;
		}
	}

	public unsafe Dictionary<DlcType, HashSet<string>> AllDlcSfxData
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 885995, RefRangeEnd = 885996, XrefRangeStart = 885995, XrefRangeEnd = 885996, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AllDlcSfxData_Public_get_Dictionary_2_DlcType_HashSet_1_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<DlcType, HashSet<string>>>(intPtr) : null;
		}
	}

	public unsafe Dictionary<CharacterType, List<CharacterData>> AdventureCharacterData
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AdventureCharacterData_Public_get_Dictionary_2_CharacterType_List_1_CharacterData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<CharacterType, List<CharacterData>>>(intPtr) : null;
		}
	}

	public unsafe Dictionary<StageType, List<StageData>> AdventureStageData
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AdventureStageData_Public_get_Dictionary_2_StageType_List_1_StageData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<StageType, List<StageData>>>(intPtr) : null;
		}
	}

	public unsafe Dictionary<EnemyType, List<EnemyData>> AdventureBestiaryData
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AdventureBestiaryData_Public_get_Dictionary_2_EnemyType_List_1_EnemyData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<EnemyType, List<EnemyData>>>(intPtr) : null;
		}
	}

	public unsafe static List<string> AllJsonPartFileNames
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1564664, XrefRangeEnd = 1564771, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AllJsonPartFileNames_Public_Static_get_List_1_String_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<string>>(intPtr) : null;
		}
	}

	static DataManager()
	{
		Il2CppClassPointerStore<DataManager>.NativeClassPtr = IL2CPP.GetIl2CppClass("VampireSurvivors.Runtime.dll", "VampireSurvivors.Data", "DataManager");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DataManager>.NativeClassPtr);
		NativeFieldInfoPtr__settings = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_settings");
		NativeFieldInfoPtr__playerOptions = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_playerOptions");
		NativeFieldInfoPtr__characterData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_characterData");
		NativeFieldInfoPtr__powerUpData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_powerUpData");
		NativeFieldInfoPtr__stageData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_stageData");
		NativeFieldInfoPtr__weaponData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_weaponData");
		NativeFieldInfoPtr__enemyData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_enemyData");
		NativeFieldInfoPtr__characterDataChangedForOnline = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_characterDataChangedForOnline");
		NativeFieldInfoPtr__powerUpDataChangedForOnline = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_powerUpDataChangedForOnline");
		NativeFieldInfoPtr__stageDataChangedForOnline = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_stageDataChangedForOnline");
		NativeFieldInfoPtr__weaponDataChangedForOnline = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_weaponDataChangedForOnline");
		NativeFieldInfoPtr__enemyDataChangedForOnline = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_enemyDataChangedForOnline");
		NativeFieldInfoPtr__dlcCharacterData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_dlcCharacterData");
		NativeFieldInfoPtr__dlcPowerUpData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_dlcPowerUpData");
		NativeFieldInfoPtr__dlcStageData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_dlcStageData");
		NativeFieldInfoPtr__dlcWeaponData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_dlcWeaponData");
		NativeFieldInfoPtr__dlcEnemyData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_dlcEnemyData");
		NativeFieldInfoPtr__dlcMusicData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_dlcMusicData");
		NativeFieldInfoPtr__dlcSfxData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_dlcSfxData");
		NativeFieldInfoPtr__mergeSettings = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_mergeSettings");
		NativeFieldInfoPtr__allWeaponDataJson = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_allWeaponDataJson");
		NativeFieldInfoPtr__allCharactersJson = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_allCharactersJson");
		NativeFieldInfoPtr__allEnemiesJson = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_allEnemiesJson");
		NativeFieldInfoPtr__allItemsJson = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_allItemsJson");
		NativeFieldInfoPtr__allPowerUpsJson = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_allPowerUpsJson");
		NativeFieldInfoPtr__allPropsJson = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_allPropsJson");
		NativeFieldInfoPtr__allStagesJson = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_allStagesJson");
		NativeFieldInfoPtr__allArcanasJson = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_allArcanasJson");
		NativeFieldInfoPtr__allHitVfxDataJson = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_allHitVfxDataJson");
		NativeFieldInfoPtr__allMusicDataJson = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_allMusicDataJson");
		NativeFieldInfoPtr__allLimitBreakDataJson = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_allLimitBreakDataJson");
		NativeFieldInfoPtr__allAchievementsJson = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_allAchievementsJson");
		NativeFieldInfoPtr__allSecretsJson = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_allSecretsJson");
		NativeFieldInfoPtr__allAdventuresJson = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_allAdventuresJson");
		NativeFieldInfoPtr__allStageSetJson = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_allStageSetJson");
		NativeFieldInfoPtr__allAdventureStagesJson = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_allAdventureStagesJson");
		NativeFieldInfoPtr__allAdventureMerchantsJson = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_allAdventureMerchantsJson");
		NativeFieldInfoPtr__allAlbumData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_allAlbumData");
		NativeFieldInfoPtr__allCustomMerchantsJson = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_allCustomMerchantsJson");
		NativeFieldInfoPtr__allCPUJson = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_allCPUJson");
		NativeFieldInfoPtr__adventureCharacterData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_adventureCharacterData");
		NativeFieldInfoPtr__adventureStageData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_adventureStageData");
		NativeFieldInfoPtr__adventureBestiaryData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_adventureBestiaryData");
		NativeFieldInfoPtr__adventureMerchantsData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "_adventureMerchantsData");
		NativeFieldInfoPtr_MarkerReloadAllData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "MarkerReloadAllData");
		NativeFieldInfoPtr_MarkerLoadDataFromJson = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "MarkerLoadDataFromJson");
		NativeFieldInfoPtr_MarkerBuildConvertedData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "MarkerBuildConvertedData");
		NativeFieldInfoPtr_MarkerLoadBaseJObjects = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "MarkerLoadBaseJObjects");
		NativeFieldInfoPtr__AllWeaponData_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "<AllWeaponData>k__BackingField");
		NativeFieldInfoPtr__AllCharacters_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "<AllCharacters>k__BackingField");
		NativeFieldInfoPtr__AllEnemies_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "<AllEnemies>k__BackingField");
		NativeFieldInfoPtr__AllItems_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "<AllItems>k__BackingField");
		NativeFieldInfoPtr__AllPowerUps_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "<AllPowerUps>k__BackingField");
		NativeFieldInfoPtr__AllProps_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "<AllProps>k__BackingField");
		NativeFieldInfoPtr__AllStages_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "<AllStages>k__BackingField");
		NativeFieldInfoPtr__AllArcanas_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "<AllArcanas>k__BackingField");
		NativeFieldInfoPtr__AllHitVfxData_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "<AllHitVfxData>k__BackingField");
		NativeFieldInfoPtr__AllMusicData_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "<AllMusicData>k__BackingField");
		NativeFieldInfoPtr__AllLimitBreakData_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "<AllLimitBreakData>k__BackingField");
		NativeFieldInfoPtr__AllAchievements_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "<AllAchievements>k__BackingField");
		NativeFieldInfoPtr__AllSecrets_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "<AllSecrets>k__BackingField");
		NativeFieldInfoPtr__AllAdventures_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "<AllAdventures>k__BackingField");
		NativeFieldInfoPtr__AllCPU_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "<AllCPU>k__BackingField");
		NativeFieldInfoPtr__AllStageSetData_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "<AllStageSetData>k__BackingField");
		NativeFieldInfoPtr__AllAdventureMerchantsData_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "<AllAdventureMerchantsData>k__BackingField");
		NativeFieldInfoPtr__AllCustomMerchantsData_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "<AllCustomMerchantsData>k__BackingField");
		NativeFieldInfoPtr__AllAlbumData_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "<AllAlbumData>k__BackingField");
		NativeFieldInfoPtr__AllLoadedAchievements_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "<AllLoadedAchievements>k__BackingField");
		NativeFieldInfoPtr__AllBaseGameAchievements_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "<AllBaseGameAchievements>k__BackingField");
		NativeFieldInfoPtr__AllDlcAchievements_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "<AllDlcAchievements>k__BackingField");
		NativeFieldInfoPtr_JsonPartFileNameAchievement = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "JsonPartFileNameAchievement");
		NativeFieldInfoPtr_JsonPartFileNameArcana = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "JsonPartFileNameArcana");
		NativeFieldInfoPtr_JsonPartFileNameCharacter = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "JsonPartFileNameCharacter");
		NativeFieldInfoPtr_JsonPartFileNameEnemy = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "JsonPartFileNameEnemy");
		NativeFieldInfoPtr_JsonPartFileNameHitVfx = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "JsonPartFileNameHitVfx");
		NativeFieldInfoPtr_JsonPartFileNameItem = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "JsonPartFileNameItem");
		NativeFieldInfoPtr_JsonPartFileNameLimitBreak = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "JsonPartFileNameLimitBreak");
		NativeFieldInfoPtr_JsonPartFileNameMusic = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "JsonPartFileNameMusic");
		NativeFieldInfoPtr_JsonPartFileNamePowerUp = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "JsonPartFileNamePowerUp");
		NativeFieldInfoPtr_JsonPartFileNameProps = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "JsonPartFileNameProps");
		NativeFieldInfoPtr_JsonPartFileNameSecrets = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "JsonPartFileNameSecrets");
		NativeFieldInfoPtr_JsonPartFileNameStage = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "JsonPartFileNameStage");
		NativeFieldInfoPtr_JsonPartFileNameWeapon = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "JsonPartFileNameWeapon");
		NativeFieldInfoPtr_JsonPartFileNameAlbum = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "JsonPartFileNameAlbum");
		NativeFieldInfoPtr_JsonPartFileNameAdventure = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "JsonPartFileNameAdventure");
		NativeFieldInfoPtr_JsonPartFileNameAdventuresStageSet = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "JsonPartFileNameAdventuresStageSet");
		NativeFieldInfoPtr_JsonPartFileNameAdventuresMerchants = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DataManager>.NativeClassPtr, "JsonPartFileNameAdventuresMerchants");
		NativeMethodInfoPtr_get_DefaultData_Public_get_DataManagerSettings_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727213);
		NativeMethodInfoPtr_get_AllWeaponData_Public_get_Dictionary_2_WeaponType_JArray_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727214);
		NativeMethodInfoPtr_set_AllWeaponData_Private_set_Void_Dictionary_2_WeaponType_JArray_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727215);
		NativeMethodInfoPtr_get_AllCharacters_Public_get_Dictionary_2_CharacterType_JArray_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727216);
		NativeMethodInfoPtr_set_AllCharacters_Private_set_Void_Dictionary_2_CharacterType_JArray_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727217);
		NativeMethodInfoPtr_get_AllEnemies_Public_get_Dictionary_2_EnemyType_JArray_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727218);
		NativeMethodInfoPtr_set_AllEnemies_Private_set_Void_Dictionary_2_EnemyType_JArray_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727219);
		NativeMethodInfoPtr_get_AllItems_Public_get_Dictionary_2_ItemType_ItemData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727220);
		NativeMethodInfoPtr_set_AllItems_Private_set_Void_Dictionary_2_ItemType_ItemData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727221);
		NativeMethodInfoPtr_get_AllPowerUps_Public_get_Dictionary_2_PowerUpType_JArray_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727222);
		NativeMethodInfoPtr_set_AllPowerUps_Private_set_Void_Dictionary_2_PowerUpType_JArray_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727223);
		NativeMethodInfoPtr_get_AllProps_Public_get_Dictionary_2_PropType_PropData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727224);
		NativeMethodInfoPtr_set_AllProps_Private_set_Void_Dictionary_2_PropType_PropData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727225);
		NativeMethodInfoPtr_get_AllStages_Public_get_Dictionary_2_StageType_JArray_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727226);
		NativeMethodInfoPtr_set_AllStages_Private_set_Void_Dictionary_2_StageType_JArray_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727227);
		NativeMethodInfoPtr_get_AllArcanas_Public_get_Dictionary_2_ArcanaType_ArcanaData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727228);
		NativeMethodInfoPtr_set_AllArcanas_Private_set_Void_Dictionary_2_ArcanaType_ArcanaData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727229);
		NativeMethodInfoPtr_get_AllHitVfxData_Public_get_Dictionary_2_HitVfxType_HitVfxData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727230);
		NativeMethodInfoPtr_set_AllHitVfxData_Private_set_Void_Dictionary_2_HitVfxType_HitVfxData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727231);
		NativeMethodInfoPtr_get_AllMusicData_Public_get_Dictionary_2_BgmType_MusicData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727232);
		NativeMethodInfoPtr_set_AllMusicData_Private_set_Void_Dictionary_2_BgmType_MusicData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727233);
		NativeMethodInfoPtr_get_AllLimitBreakData_Public_get_Dictionary_2_WeaponType_JArray_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727234);
		NativeMethodInfoPtr_set_AllLimitBreakData_Private_set_Void_Dictionary_2_WeaponType_JArray_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727235);
		NativeMethodInfoPtr_get_AllAchievements_Public_get_Dictionary_2_AchievementType_AchievementData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727236);
		NativeMethodInfoPtr_set_AllAchievements_Private_set_Void_Dictionary_2_AchievementType_AchievementData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727237);
		NativeMethodInfoPtr_get_AllSecrets_Public_get_Dictionary_2_SecretType_SecretData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727238);
		NativeMethodInfoPtr_set_AllSecrets_Private_set_Void_Dictionary_2_SecretType_SecretData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727239);
		NativeMethodInfoPtr_get_AllAdventures_Public_get_Dictionary_2_AdventureType_AdventureData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727240);
		NativeMethodInfoPtr_set_AllAdventures_Private_set_Void_Dictionary_2_AdventureType_AdventureData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727241);
		NativeMethodInfoPtr_get_AllCPU_Public_get_Dictionary_2_AIType_AIData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727242);
		NativeMethodInfoPtr_set_AllCPU_Private_set_Void_Dictionary_2_AIType_AIData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727243);
		NativeMethodInfoPtr_get_AllStageSetData_Public_get_Dictionary_2_StageSetType_JObject_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727244);
		NativeMethodInfoPtr_set_AllStageSetData_Private_set_Void_Dictionary_2_StageSetType_JObject_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727245);
		NativeMethodInfoPtr_get_AllAdventureMerchantsData_Public_get_Dictionary_2_CharacterType_CustomMerchantData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727246);
		NativeMethodInfoPtr_set_AllAdventureMerchantsData_Private_set_Void_Dictionary_2_CharacterType_CustomMerchantData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727247);
		NativeMethodInfoPtr_get_AllCustomMerchantsData_Public_get_Dictionary_2_CharacterType_CustomMerchantData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727248);
		NativeMethodInfoPtr_set_AllCustomMerchantsData_Private_set_Void_Dictionary_2_CharacterType_CustomMerchantData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727249);
		NativeMethodInfoPtr_get_AllAlbumData_Public_get_Dictionary_2_AlbumType_AlbumData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727250);
		NativeMethodInfoPtr_set_AllAlbumData_Private_set_Void_Dictionary_2_AlbumType_AlbumData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727251);
		NativeMethodInfoPtr_get_AllLoadedAchievements_Public_get_HashSet_1_AchievementType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727252);
		NativeMethodInfoPtr_set_AllLoadedAchievements_Private_set_Void_HashSet_1_AchievementType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727253);
		NativeMethodInfoPtr_get_AllBaseGameAchievements_Public_get_HashSet_1_AchievementType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727254);
		NativeMethodInfoPtr_set_AllBaseGameAchievements_Private_set_Void_HashSet_1_AchievementType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727255);
		NativeMethodInfoPtr_get_AllDlcAchievements_Public_get_Dictionary_2_DlcType_List_1_AchievementType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727256);
		NativeMethodInfoPtr_set_AllDlcAchievements_Private_set_Void_Dictionary_2_DlcType_List_1_AchievementType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727257);
		NativeMethodInfoPtr_get_AllDlcCharacterData_Public_get_Dictionary_2_DlcType_Dictionary_2_CharacterType_List_1_CharacterData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727258);
		NativeMethodInfoPtr_get_AllDlcPowerUpData_Public_get_Dictionary_2_DlcType_Dictionary_2_PowerUpType_List_1_PowerUpData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727259);
		NativeMethodInfoPtr_get_AllDlcStageData_Public_get_Dictionary_2_DlcType_Dictionary_2_StageType_List_1_StageData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727260);
		NativeMethodInfoPtr_get_AllDlcWeaponData_Public_get_Dictionary_2_DlcType_Dictionary_2_WeaponType_List_1_WeaponData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727261);
		NativeMethodInfoPtr_get_AllDlcEnemyData_Public_get_Dictionary_2_DlcType_Dictionary_2_EnemyType_List_1_EnemyData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727262);
		NativeMethodInfoPtr_get_AllDlcMusicData_Public_get_Dictionary_2_DlcType_Dictionary_2_BgmType_MusicData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727263);
		NativeMethodInfoPtr_get_AllDlcSfxData_Public_get_Dictionary_2_DlcType_HashSet_1_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727264);
		NativeMethodInfoPtr_get_AdventureCharacterData_Public_get_Dictionary_2_CharacterType_List_1_CharacterData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727265);
		NativeMethodInfoPtr_get_AdventureStageData_Public_get_Dictionary_2_StageType_List_1_StageData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727266);
		NativeMethodInfoPtr_get_AdventureBestiaryData_Public_get_Dictionary_2_EnemyType_List_1_EnemyData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727267);
		NativeMethodInfoPtr_Initialize_Public_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727268);
		NativeMethodInfoPtr_Dispose_Public_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727269);
		NativeMethodInfoPtr_ReloadAllData_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727270);
		NativeMethodInfoPtr_GetConvertedDlcCharacterData_Public_Dictionary_2_CharacterType_List_1_CharacterData_DlcType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727271);
		NativeMethodInfoPtr_GetConvertedDlcStageData_Public_Dictionary_2_StageType_List_1_StageData_DlcType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727272);
		NativeMethodInfoPtr_GetConvertedDlcWeaponData_Public_Dictionary_2_WeaponType_List_1_WeaponData_DlcType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727273);
		NativeMethodInfoPtr_GetConvertedDlcEnemyData_Public_Dictionary_2_EnemyType_List_1_EnemyData_DlcType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727274);
		NativeMethodInfoPtr_GetConvertedDlcPowerUpData_Public_Dictionary_2_PowerUpType_List_1_PowerUpData_DlcType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727275);
		NativeMethodInfoPtr_GetConvertedDlcMusicData_Public_Dictionary_2_BgmType_MusicData_DlcType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727276);
		NativeMethodInfoPtr_GetConvertedCharacterData_Public_Dictionary_2_CharacterType_List_1_CharacterData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727277);
		NativeMethodInfoPtr_GetBaseGameChars_Public_Dictionary_2_CharacterType_List_1_CharacterData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727278);
		NativeMethodInfoPtr_GetConvertedEnemyData_Public_Dictionary_2_EnemyType_List_1_EnemyData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727279);
		NativeMethodInfoPtr_GetConvertedPowerUpData_Public_Dictionary_2_PowerUpType_List_1_PowerUpData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727280);
		NativeMethodInfoPtr_GetConvertedStages_Public_Dictionary_2_StageType_List_1_StageData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727281);
		NativeMethodInfoPtr_GetConvertedAdventureStages_Public_Dictionary_2_StageType_List_1_StageData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727282);
		NativeMethodInfoPtr_GetConvertedWeapons_Public_Dictionary_2_WeaponType_List_1_WeaponData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727283);
		NativeMethodInfoPtr_GetPropData_Public_PropData_PropType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727284);
		NativeMethodInfoPtr_AddDefaultUnlocksToSaveData_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727285);
		NativeMethodInfoPtr_UpdateAllCharacterHiddenPropertiesForAdventures_Public_Void_AdventureData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727286);
		NativeMethodInfoPtr_GenerateAdventureSpecificData_Public_Void_AdventureData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727287);
		NativeMethodInfoPtr_ExitAdventure_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727288);
		NativeMethodInfoPtr_IsOnline_Private_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727289);
		NativeMethodInfoPtr_LoadBaseJObjects_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727290);
		NativeMethodInfoPtr_LoadDataFromJson_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727291);
		NativeMethodInfoPtr_MergeInJsonData_Public_Void_DataManagerSettings_DlcType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727292);
		NativeMethodInfoPtr_InternalMergeInJsonData_Private_Void_DataManagerSettings_DlcType_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727293);
		NativeMethodInfoPtr_CacheBaseGameLoadedAchievements_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727294);
		NativeMethodInfoPtr_MergeInDlcAchievements_Public_Void_DlcType_TextAsset_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727295);
		NativeMethodInfoPtr_MergeInSFXTypes_Public_Void_DlcType_Transform_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727296);
		NativeMethodInfoPtr_LoadAndMergeIn_Private_Void_JObject_TextAsset_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727297);
		NativeMethodInfoPtr_BuildConvertedDlcData_Private_Void_DataManagerSettings_DlcType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727298);
		NativeMethodInfoPtr_ClearConvertedDlcData_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727299);
		NativeMethodInfoPtr_ClearConvertedData_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727300);
		NativeMethodInfoPtr_BuildConvertedData_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727301);
		NativeMethodInfoPtr_ConvertEnemyDataJsonToObjects_Private_Static_Dictionary_2_EnemyType_List_1_EnemyData_Dictionary_2_EnemyType_JArray_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727302);
		NativeMethodInfoPtr_ConvertWeaponDataJsonToObjects_Private_Static_Dictionary_2_WeaponType_List_1_WeaponData_Dictionary_2_WeaponType_JArray_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727303);
		NativeMethodInfoPtr_ConvertStageDataJsonToObjects_Private_Static_Dictionary_2_StageType_List_1_StageData_Dictionary_2_StageType_JArray_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727304);
		NativeMethodInfoPtr_ConvertCharacterJsonDataToObjects_Private_Static_Dictionary_2_CharacterType_List_1_CharacterData_Dictionary_2_CharacterType_JArray_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727305);
		NativeMethodInfoPtr_ConvertPowerUpJsonData_Private_Static_Dictionary_2_PowerUpType_List_1_PowerUpData_Dictionary_2_PowerUpType_JArray_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727306);
		NativeMethodInfoPtr_ConvertMusicJsonDataToObjects_Private_Static_Dictionary_2_BgmType_List_1_MusicData_Dictionary_2_BgmType_JArray_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727307);
		NativeMethodInfoPtr_CacheEnemyDataStrings_Private_Static_Void_EnemyData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727308);
		NativeMethodInfoPtr_AdjustAchievementDataWithTypes_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727309);
		NativeMethodInfoPtr_AdjustAdventureProgressDataWithTypes_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727310);
		NativeMethodInfoPtr_GenerateBestiaryDataForAdventure_Private_Void_AdventureData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727311);
		NativeMethodInfoPtr_FindEnemyBaseVariant_Private_Nullable_1_EnemyType_EnemyType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727312);
		NativeMethodInfoPtr_get_AllJsonPartFileNames_Public_Static_get_List_1_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727313);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DataManager>.NativeClassPtr, 100727314);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1562867, XrefRangeEnd = 1562871, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void Initialize()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Initialize_Public_Virtual_Final_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(754)]
	[CachedScanResults(RefRangeStart = 41, RefRangeEnd = 795, XrefRangeStart = 41, XrefRangeEnd = 795, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe virtual void Dispose()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Dispose_Public_Virtual_Final_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 1562883, RefRangeEnd = 1562890, XrefRangeStart = 1562871, XrefRangeEnd = 1562883, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ReloadAllData()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReloadAllData_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 1562894, RefRangeEnd = 1562897, XrefRangeStart = 1562890, XrefRangeEnd = 1562894, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Dictionary<CharacterType, List<CharacterData>> GetConvertedDlcCharacterData(DlcType dlcType)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&dlcType);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetConvertedDlcCharacterData_Public_Dictionary_2_CharacterType_List_1_CharacterData_DlcType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<CharacterType, List<CharacterData>>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1562897, XrefRangeEnd = 1562901, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Dictionary<StageType, List<StageData>> GetConvertedDlcStageData(DlcType dlcType)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&dlcType);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetConvertedDlcStageData_Public_Dictionary_2_StageType_List_1_StageData_DlcType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<StageType, List<StageData>>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1562901, XrefRangeEnd = 1562905, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Dictionary<WeaponType, List<WeaponData>> GetConvertedDlcWeaponData(DlcType dlcType)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&dlcType);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetConvertedDlcWeaponData_Public_Dictionary_2_WeaponType_List_1_WeaponData_DlcType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<WeaponType, List<WeaponData>>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1562905, XrefRangeEnd = 1562909, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Dictionary<EnemyType, List<EnemyData>> GetConvertedDlcEnemyData(DlcType dlcType)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&dlcType);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetConvertedDlcEnemyData_Public_Dictionary_2_EnemyType_List_1_EnemyData_DlcType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<EnemyType, List<EnemyData>>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1562909, XrefRangeEnd = 1562913, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Dictionary<PowerUpType, List<PowerUpData>> GetConvertedDlcPowerUpData(DlcType dlcType)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&dlcType);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetConvertedDlcPowerUpData_Public_Dictionary_2_PowerUpType_List_1_PowerUpData_DlcType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<PowerUpType, List<PowerUpData>>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1562913, XrefRangeEnd = 1562917, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Dictionary<BgmType, MusicData> GetConvertedDlcMusicData(DlcType dlcType)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&dlcType);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetConvertedDlcMusicData_Public_Dictionary_2_BgmType_MusicData_DlcType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<BgmType, MusicData>>(intPtr) : null;
	}

	[CallerCount(105)]
	[CachedScanResults(RefRangeStart = 1562956, RefRangeEnd = 1563061, XrefRangeStart = 1562917, XrefRangeEnd = 1562956, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Dictionary<CharacterType, List<CharacterData>> GetConvertedCharacterData()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetConvertedCharacterData_Public_Dictionary_2_CharacterType_List_1_CharacterData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<CharacterType, List<CharacterData>>>(intPtr) : null;
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 1563160, RefRangeEnd = 1563163, XrefRangeStart = 1563061, XrefRangeEnd = 1563160, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Dictionary<CharacterType, List<CharacterData>> GetBaseGameChars()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetBaseGameChars_Public_Dictionary_2_CharacterType_List_1_CharacterData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<CharacterType, List<CharacterData>>>(intPtr) : null;
	}

	[CallerCount(25)]
	[CachedScanResults(RefRangeStart = 1563199, RefRangeEnd = 1563224, XrefRangeStart = 1563163, XrefRangeEnd = 1563199, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Dictionary<EnemyType, List<EnemyData>> GetConvertedEnemyData()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetConvertedEnemyData_Public_Dictionary_2_EnemyType_List_1_EnemyData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<EnemyType, List<EnemyData>>>(intPtr) : null;
	}

	[CallerCount(16)]
	[CachedScanResults(RefRangeStart = 1563260, RefRangeEnd = 1563276, XrefRangeStart = 1563224, XrefRangeEnd = 1563260, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Dictionary<PowerUpType, List<PowerUpData>> GetConvertedPowerUpData()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetConvertedPowerUpData_Public_Dictionary_2_PowerUpType_List_1_PowerUpData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<PowerUpType, List<PowerUpData>>>(intPtr) : null;
	}

	[CallerCount(34)]
	[CachedScanResults(RefRangeStart = 1563312, RefRangeEnd = 1563346, XrefRangeStart = 1563276, XrefRangeEnd = 1563312, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Dictionary<StageType, List<StageData>> GetConvertedStages()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetConvertedStages_Public_Dictionary_2_StageType_List_1_StageData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<StageType, List<StageData>>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1563346, XrefRangeEnd = 1563350, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Dictionary<StageType, List<StageData>> GetConvertedAdventureStages()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetConvertedAdventureStages_Public_Dictionary_2_StageType_List_1_StageData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<StageType, List<StageData>>>(intPtr) : null;
	}

	[CallerCount(112)]
	[CachedScanResults(RefRangeStart = 1563386, RefRangeEnd = 1563498, XrefRangeStart = 1563350, XrefRangeEnd = 1563386, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Dictionary<WeaponType, List<WeaponData>> GetConvertedWeapons()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetConvertedWeapons_Public_Dictionary_2_WeaponType_List_1_WeaponData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<WeaponType, List<WeaponData>>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1563498, XrefRangeEnd = 1563502, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe PropData GetPropData(PropType propType)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&propType);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetPropData_Public_PropData_PropType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PropData>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 1563522, RefRangeEnd = 1563523, XrefRangeStart = 1563502, XrefRangeEnd = 1563522, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AddDefaultUnlocksToSaveData()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddDefaultUnlocksToSaveData_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 1563549, RefRangeEnd = 1563550, XrefRangeStart = 1563523, XrefRangeEnd = 1563549, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UpdateAllCharacterHiddenPropertiesForAdventures(AdventureData adventureData)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)adventureData);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UpdateAllCharacterHiddenPropertiesForAdventures_Public_Void_AdventureData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 1563641, RefRangeEnd = 1563642, XrefRangeStart = 1563550, XrefRangeEnd = 1563641, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void GenerateAdventureSpecificData(AdventureData adventureData)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)adventureData);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GenerateAdventureSpecificData_Public_Void_AdventureData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1563642, XrefRangeEnd = 1563643, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ExitAdventure()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ExitAdventure_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(11)]
	[CachedScanResults(RefRangeStart = 1563666, RefRangeEnd = 1563677, XrefRangeStart = 1563643, XrefRangeEnd = 1563666, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsOnline()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsOnline_Private_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 1563744, RefRangeEnd = 1563746, XrefRangeStart = 1563677, XrefRangeEnd = 1563744, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void LoadBaseJObjects()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LoadBaseJObjects_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 1563887, RefRangeEnd = 1563890, XrefRangeStart = 1563746, XrefRangeEnd = 1563887, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void LoadDataFromJson()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LoadDataFromJson_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 1563909, RefRangeEnd = 1563911, XrefRangeStart = 1563890, XrefRangeEnd = 1563909, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void MergeInJsonData(DataManagerSettings settings, DlcType dlcType)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)settings);
		*(DlcType**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &dlcType;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MergeInJsonData_Public_Void_DataManagerSettings_DlcType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1563911, XrefRangeEnd = 1563929, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void InternalMergeInJsonData(DataManagerSettings settings, DlcType dlcType, bool reload = true)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)settings);
		*(DlcType**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &dlcType;
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &reload;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InternalMergeInJsonData_Private_Void_DataManagerSettings_DlcType_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 1563957, RefRangeEnd = 1563958, XrefRangeStart = 1563929, XrefRangeEnd = 1563957, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void CacheBaseGameLoadedAchievements()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CacheBaseGameLoadedAchievements_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 1564000, RefRangeEnd = 1564002, XrefRangeStart = 1563958, XrefRangeEnd = 1564000, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void MergeInDlcAchievements(DlcType dlcType, TextAsset achievements)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&dlcType);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)achievements);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MergeInDlcAchievements_Public_Void_DlcType_TextAsset_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 1564036, RefRangeEnd = 1564037, XrefRangeStart = 1564002, XrefRangeEnd = 1564036, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void MergeInSFXTypes(DlcType dlc, Transform instantiatedSoundGroup)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&dlc);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)instantiatedSoundGroup);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MergeInSFXTypes_Public_Void_DlcType_Transform_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(30)]
	[CachedScanResults(RefRangeStart = 1564049, RefRangeEnd = 1564079, XrefRangeStart = 1564037, XrefRangeEnd = 1564049, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void LoadAndMergeIn(JObject original, TextAsset newAsset)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)original);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)newAsset);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_LoadAndMergeIn_Private_Void_JObject_TextAsset_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 1564228, RefRangeEnd = 1564230, XrefRangeStart = 1564079, XrefRangeEnd = 1564228, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void BuildConvertedDlcData(DataManagerSettings settings, DlcType dlcType)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)settings);
		*(DlcType**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &dlcType;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BuildConvertedDlcData_Private_Void_DataManagerSettings_DlcType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 1564236, RefRangeEnd = 1564238, XrefRangeStart = 1564230, XrefRangeEnd = 1564236, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ClearConvertedDlcData()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClearConvertedDlcData_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 1564238, RefRangeEnd = 1564239, XrefRangeStart = 1564238, XrefRangeEnd = 1564238, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ClearConvertedData()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClearConvertedData_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 1564253, RefRangeEnd = 1564256, XrefRangeStart = 1564239, XrefRangeEnd = 1564253, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void BuildConvertedData()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BuildConvertedData_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 1564301, RefRangeEnd = 1564304, XrefRangeStart = 1564256, XrefRangeEnd = 1564301, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Dictionary<EnemyType, List<EnemyData>> ConvertEnemyDataJsonToObjects(Dictionary<EnemyType, JArray> enemyJson)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)enemyJson);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ConvertEnemyDataJsonToObjects_Private_Static_Dictionary_2_EnemyType_List_1_EnemyData_Dictionary_2_EnemyType_JArray_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<EnemyType, List<EnemyData>>>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 1564326, RefRangeEnd = 1564328, XrefRangeStart = 1564304, XrefRangeEnd = 1564326, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Dictionary<WeaponType, List<WeaponData>> ConvertWeaponDataJsonToObjects(Dictionary<WeaponType, JArray> weaponJson)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)weaponJson);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ConvertWeaponDataJsonToObjects_Private_Static_Dictionary_2_WeaponType_List_1_WeaponData_Dictionary_2_WeaponType_JArray_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<WeaponType, List<WeaponData>>>(intPtr) : null;
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 1564350, RefRangeEnd = 1564354, XrefRangeStart = 1564328, XrefRangeEnd = 1564350, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Dictionary<StageType, List<StageData>> ConvertStageDataJsonToObjects(Dictionary<StageType, JArray> jsonData)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)jsonData);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ConvertStageDataJsonToObjects_Private_Static_Dictionary_2_StageType_List_1_StageData_Dictionary_2_StageType_JArray_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<StageType, List<StageData>>>(intPtr) : null;
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 1564376, RefRangeEnd = 1564379, XrefRangeStart = 1564354, XrefRangeEnd = 1564376, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Dictionary<CharacterType, List<CharacterData>> ConvertCharacterJsonDataToObjects(Dictionary<CharacterType, JArray> jsonData)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)jsonData);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ConvertCharacterJsonDataToObjects_Private_Static_Dictionary_2_CharacterType_List_1_CharacterData_Dictionary_2_CharacterType_JArray_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<CharacterType, List<CharacterData>>>(intPtr) : null;
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 1564401, RefRangeEnd = 1564403, XrefRangeStart = 1564379, XrefRangeEnd = 1564401, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Dictionary<PowerUpType, List<PowerUpData>> ConvertPowerUpJsonData(Dictionary<PowerUpType, JArray> jsonData)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)jsonData);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ConvertPowerUpJsonData_Private_Static_Dictionary_2_PowerUpType_List_1_PowerUpData_Dictionary_2_PowerUpType_JArray_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<PowerUpType, List<PowerUpData>>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1564403, XrefRangeEnd = 1564425, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Dictionary<BgmType, List<MusicData>> ConvertMusicJsonDataToObjects(Dictionary<BgmType, JArray> jsonData)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)jsonData);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ConvertMusicJsonDataToObjects_Private_Static_Dictionary_2_BgmType_List_1_MusicData_Dictionary_2_BgmType_JArray_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<BgmType, List<MusicData>>>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 1564484, RefRangeEnd = 1564485, XrefRangeStart = 1564425, XrefRangeEnd = 1564484, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static void CacheEnemyDataStrings(EnemyData enemyData)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)enemyData);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CacheEnemyDataStrings_Private_Static_Void_EnemyData_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 1564495, RefRangeEnd = 1564496, XrefRangeStart = 1564485, XrefRangeEnd = 1564495, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AdjustAchievementDataWithTypes()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AdjustAchievementDataWithTypes_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 1564514, RefRangeEnd = 1564515, XrefRangeStart = 1564496, XrefRangeEnd = 1564514, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AdjustAdventureProgressDataWithTypes()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AdjustAdventureProgressDataWithTypes_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 1564643, RefRangeEnd = 1564644, XrefRangeStart = 1564515, XrefRangeEnd = 1564643, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void GenerateBestiaryDataForAdventure(AdventureData adventureData)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)adventureData);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GenerateBestiaryDataForAdventure_Private_Void_AdventureData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 1564663, RefRangeEnd = 1564664, XrefRangeStart = 1564644, XrefRangeEnd = 1564663, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe Il2CppSystem.Nullable<EnemyType> FindEnemyBaseVariant(EnemyType enemyType)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&enemyType);
		Unsafe.SkipInit(out System.IntPtr intPtr);
		System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_FindEnemyBaseVariant_Private_Nullable_1_EnemyType_EnemyType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
		Il2CppException.RaiseExceptionIfNecessary(intPtr);
		return new Il2CppSystem.Nullable<EnemyType>(pointer);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1564771, XrefRangeEnd = 1564804, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe DataManager()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DataManager>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public DataManager(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
