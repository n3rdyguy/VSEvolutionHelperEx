using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using VampireSurvivors.App.Data;
using VampireSurvivors.App.Objects;

namespace VampireSurvivors.Data.Stage;

[System.Serializable]
public class StageData : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr__order_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__tilesetStageType_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__stageName_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__description_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__uiTexture_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__uiFrame_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__texture_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__bestiaryBG_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__stageNumber_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__frameName_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__frameNameUnlock_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__unlocked_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__BGM_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__sideBBGM_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__legacyBGM_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__tips_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__hyperTips_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__validForCharcaterData_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__hidden_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__alwaysHidden_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__mods_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__hyper_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__inverse_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__tileset_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__background_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__poolsMapping_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__spawnType_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__startingSpawns_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__minute_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__randomMinutes_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__destructibleType_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__destructibleFreq_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__destructibleChance_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__destructibleChanceMax_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__maxDestructibles_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__BGTextureName_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__Extra_Texture_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__Extra_Audio_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__isMerchantBanned_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__isSpeedupBanned_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__isSuvarotsBlocked_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__hasLights_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__disableGlobalLight_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__hasCharacterSpotlight_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__dayNight_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__DayColor_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__NightColor_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__InverseDayColor_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__InverseNightColor_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__tilemapTiledJSON_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__tilemapTiledIMG_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__tilemapPos_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__minimum_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__frequency_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__zoom_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__enemies_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__bosses_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__treasure_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__arcanaHolder_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__arcanaTreasure_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__events_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__pizzaEvents_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__cff_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__LootTable_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__relics_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__relics2_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__yellowRelics_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__preload_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__adventureMerchants_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__defaultFollowers_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__adventurePriceMarkup_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__isRacingStage_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__skipVisualInversion_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__allowVisualInversion_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__biome_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__biomes_k__BackingField;

	private static readonly System.IntPtr NativeFieldInfoPtr__batDragonType_k__BackingField;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_order_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_order_Public_set_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_tilesetStageType_Public_get_Nullable_1_StageType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_tilesetStageType_Public_set_Void_Nullable_1_StageType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_stageName_Public_get_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_stageName_Public_set_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_description_Public_get_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_description_Public_set_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_uiTexture_Public_get_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_uiTexture_Public_set_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_uiFrame_Public_get_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_uiFrame_Public_set_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_texture_Public_get_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_texture_Public_set_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_bestiaryBG_Public_get_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_bestiaryBG_Public_set_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_stageNumber_Public_get_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_stageNumber_Public_set_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_frameName_Public_get_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_frameName_Public_set_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_frameNameUnlock_Public_get_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_frameNameUnlock_Public_set_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_unlocked_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_unlocked_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_BGM_Public_get_BgmType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_BGM_Public_set_Void_BgmType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_sideBBGM_Public_get_Nullable_1_BgmType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_sideBBGM_Public_set_Void_Nullable_1_BgmType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_legacyBGM_Public_get_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_legacyBGM_Public_set_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_tips_Public_get_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_tips_Public_set_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_hyperTips_Public_get_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_hyperTips_Public_set_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_validForCharcaterData_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_validForCharcaterData_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_hidden_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_hidden_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_alwaysHidden_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_alwaysHidden_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_mods_Public_get_StageModifiers_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_mods_Public_set_Void_StageModifiers_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_hyper_Public_get_StageModifiers_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_hyper_Public_set_Void_StageModifiers_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_inverse_Public_get_StageModifiers_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_inverse_Public_set_Void_StageModifiers_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_tileset_Public_get_Tileset_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_tileset_Public_set_Void_Tileset_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_background_Public_get_Background_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_background_Public_set_Void_Background_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_poolsMapping_Public_get_List_1_PoolsMapping_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_poolsMapping_Public_set_Void_List_1_PoolsMapping_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_spawnType_Public_get_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_spawnType_Public_set_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_startingSpawns_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_startingSpawns_Public_set_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_minute_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_minute_Public_set_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_randomMinutes_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_randomMinutes_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_destructibleType_Public_get_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_destructibleType_Public_set_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_destructibleFreq_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_destructibleFreq_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_destructibleChance_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_destructibleChance_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_destructibleChanceMax_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_destructibleChanceMax_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_maxDestructibles_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_maxDestructibles_Public_set_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_BGTextureName_Public_get_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_BGTextureName_Public_set_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Extra_Texture_Public_get_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Extra_Texture_Public_set_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Extra_Audio_Public_get_BgmType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Extra_Audio_Public_set_Void_BgmType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_isMerchantBanned_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_isMerchantBanned_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_isSpeedupBanned_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_isSpeedupBanned_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_isSuvarotsBlocked_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_isSuvarotsBlocked_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_hasLights_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_hasLights_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_disableGlobalLight_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_disableGlobalLight_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_hasCharacterSpotlight_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_hasCharacterSpotlight_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_dayNight_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_dayNight_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_DayColor_Public_get_UInt32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_DayColor_Public_set_Void_UInt32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_NightColor_Public_get_UInt32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_NightColor_Public_set_Void_UInt32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_InverseDayColor_Public_get_UInt32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_InverseDayColor_Public_set_Void_UInt32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_InverseNightColor_Public_get_UInt32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_InverseNightColor_Public_set_Void_UInt32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_tilemapTiledJSON_Public_get_TilemapTiledJSON_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_tilemapTiledJSON_Public_set_Void_TilemapTiledJSON_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_tilemapTiledIMG_Public_get_TilemapTiledIMG_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_tilemapTiledIMG_Public_set_Void_TilemapTiledIMG_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_tilemapPos_Public_get_TilemapPos_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_tilemapPos_Public_set_Void_TilemapPos_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_minimum_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_minimum_Public_set_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_frequency_Public_get_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_frequency_Public_set_Void_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_zoom_Public_get_Nullable_1_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_zoom_Public_set_Void_Nullable_1_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_enemies_Public_get_List_1_Nullable_1_EnemyType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_enemies_Public_set_Void_List_1_Nullable_1_EnemyType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_bosses_Public_get_List_1_Nullable_1_EnemyType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_bosses_Public_set_Void_List_1_Nullable_1_EnemyType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_treasure_Public_get_Treasure_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_treasure_Public_set_Void_Treasure_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_arcanaHolder_Public_get_Nullable_1_EnemyType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_arcanaHolder_Public_set_Void_Nullable_1_EnemyType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_arcanaTreasure_Public_get_Treasure_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_arcanaTreasure_Public_set_Void_Treasure_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_events_Public_get_List_1_Event_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_events_Public_set_Void_List_1_Event_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_pizzaEvents_Public_get_List_1_Event_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_pizzaEvents_Public_set_Void_List_1_Event_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_cff_Public_get_Nullable_1_CharacterType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_cff_Public_set_Void_Nullable_1_CharacterType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_LootTable_Public_get_List_1_ItemType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_LootTable_Public_set_Void_List_1_ItemType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_relics_Public_get_List_1_ItemType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_relics_Public_set_Void_List_1_ItemType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_relics2_Public_get_List_1_ItemType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_relics2_Public_set_Void_List_1_ItemType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_yellowRelics_Public_get_List_1_ItemType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_yellowRelics_Public_set_Void_List_1_ItemType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_preload_Public_get_PreloadData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_preload_Public_set_Void_PreloadData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_adventureMerchants_Public_get_List_1_CustomMerchantData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_adventureMerchants_Public_set_Void_List_1_CustomMerchantData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_defaultFollowers_Public_get_List_1_FollowerData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_defaultFollowers_Public_set_Void_List_1_FollowerData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_adventurePriceMarkup_Public_get_Nullable_1_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_adventurePriceMarkup_Public_set_Void_Nullable_1_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_isRacingStage_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_isRacingStage_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_skipVisualInversion_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_skipVisualInversion_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_allowVisualInversion_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_allowVisualInversion_Public_set_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_biome_Public_get_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_biome_Public_set_Void_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_biomes_Public_get_List_1_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_biomes_Public_set_Void_List_1_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_batDragonType_Public_get_Nullable_1_EnemyType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_batDragonType_Public_set_Void_Nullable_1_EnemyType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetLocalizedName_Public_String_StageType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetLocalizedTips_Public_String_StageType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetLocalizedHyperTips_Public_String_StageType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetLocalizedDescription_Public_String_StageType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetPrefix_Private_String_StageType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe int _order_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__order_k__BackingField);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__order_k__BackingField)) = num;
		}
	}

	public unsafe Il2CppSystem.Nullable<StageType> _tilesetStageType_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__tilesetStageType_k__BackingField);
			return new Il2CppSystem.Nullable<StageType>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<Il2CppSystem.Nullable<StageType>>.NativeClassPtr, (System.IntPtr)num));
		}
		set
		{
			// IL cpblk instruction
			Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__tilesetStageType_k__BackingField), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)nullable)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<Il2CppSystem.Nullable<StageType>>.NativeClassPtr, ref *(uint*)null));
		}
	}

	public unsafe string _stageName_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__stageName_k__BackingField);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__stageName_k__BackingField)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string _description_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__description_k__BackingField);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__description_k__BackingField)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string _uiTexture_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__uiTexture_k__BackingField);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__uiTexture_k__BackingField)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string _uiFrame_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__uiFrame_k__BackingField);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__uiFrame_k__BackingField)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string _texture_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__texture_k__BackingField);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__texture_k__BackingField)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string _bestiaryBG_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__bestiaryBG_k__BackingField);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__bestiaryBG_k__BackingField)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string _stageNumber_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__stageNumber_k__BackingField);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__stageNumber_k__BackingField)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string _frameName_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__frameName_k__BackingField);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__frameName_k__BackingField)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string _frameNameUnlock_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__frameNameUnlock_k__BackingField);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__frameNameUnlock_k__BackingField)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe bool _unlocked_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__unlocked_k__BackingField);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__unlocked_k__BackingField)) = flag;
		}
	}

	public unsafe BgmType _BGM_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__BGM_k__BackingField);
			return *(BgmType*)num;
		}
		set
		{
			*(BgmType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__BGM_k__BackingField)) = bgmType;
		}
	}

	public unsafe Il2CppSystem.Nullable<BgmType> _sideBBGM_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__sideBBGM_k__BackingField);
			return new Il2CppSystem.Nullable<BgmType>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<Il2CppSystem.Nullable<BgmType>>.NativeClassPtr, (System.IntPtr)num));
		}
		set
		{
			// IL cpblk instruction
			Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__sideBBGM_k__BackingField), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)nullable)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<Il2CppSystem.Nullable<BgmType>>.NativeClassPtr, ref *(uint*)null));
		}
	}

	public unsafe string _legacyBGM_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__legacyBGM_k__BackingField);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__legacyBGM_k__BackingField)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string _tips_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__tips_k__BackingField);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__tips_k__BackingField)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string _hyperTips_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hyperTips_k__BackingField);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hyperTips_k__BackingField)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe bool _validForCharcaterData_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__validForCharcaterData_k__BackingField);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__validForCharcaterData_k__BackingField)) = flag;
		}
	}

	public unsafe bool _hidden_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hidden_k__BackingField);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hidden_k__BackingField)) = flag;
		}
	}

	public unsafe bool _alwaysHidden_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__alwaysHidden_k__BackingField);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__alwaysHidden_k__BackingField)) = flag;
		}
	}

	public unsafe StageModifiers _mods_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mods_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<StageModifiers>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mods_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stageModifiers));
		}
	}

	public unsafe StageModifiers _hyper_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hyper_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<StageModifiers>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hyper_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stageModifiers));
		}
	}

	public unsafe StageModifiers _inverse_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__inverse_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<StageModifiers>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__inverse_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stageModifiers));
		}
	}

	public unsafe Tileset _tileset_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__tileset_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Tileset>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__tileset_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)tileset));
		}
	}

	public unsafe Background _background_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__background_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Background>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__background_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)background));
		}
	}

	public unsafe List<PoolsMapping> _poolsMapping_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__poolsMapping_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<PoolsMapping>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__poolsMapping_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe string _spawnType_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__spawnType_k__BackingField);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__spawnType_k__BackingField)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe int _startingSpawns_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__startingSpawns_k__BackingField);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__startingSpawns_k__BackingField)) = num;
		}
	}

	public unsafe int _minute_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__minute_k__BackingField);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__minute_k__BackingField)) = num;
		}
	}

	public unsafe bool _randomMinutes_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__randomMinutes_k__BackingField);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__randomMinutes_k__BackingField)) = flag;
		}
	}

	public unsafe string _destructibleType_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__destructibleType_k__BackingField);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__destructibleType_k__BackingField)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe float _destructibleFreq_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__destructibleFreq_k__BackingField);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__destructibleFreq_k__BackingField)) = num;
		}
	}

	public unsafe float _destructibleChance_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__destructibleChance_k__BackingField);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__destructibleChance_k__BackingField)) = num;
		}
	}

	public unsafe float _destructibleChanceMax_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__destructibleChanceMax_k__BackingField);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__destructibleChanceMax_k__BackingField)) = num;
		}
	}

	public unsafe int _maxDestructibles_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__maxDestructibles_k__BackingField);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__maxDestructibles_k__BackingField)) = num;
		}
	}

	public unsafe string _BGTextureName_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__BGTextureName_k__BackingField);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__BGTextureName_k__BackingField)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string _Extra_Texture_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Extra_Texture_k__BackingField);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Extra_Texture_k__BackingField)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe BgmType _Extra_Audio_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Extra_Audio_k__BackingField);
			return *(BgmType*)num;
		}
		set
		{
			*(BgmType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Extra_Audio_k__BackingField)) = bgmType;
		}
	}

	public unsafe bool _isMerchantBanned_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__isMerchantBanned_k__BackingField);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__isMerchantBanned_k__BackingField)) = flag;
		}
	}

	public unsafe bool _isSpeedupBanned_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__isSpeedupBanned_k__BackingField);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__isSpeedupBanned_k__BackingField)) = flag;
		}
	}

	public unsafe bool _isSuvarotsBlocked_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__isSuvarotsBlocked_k__BackingField);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__isSuvarotsBlocked_k__BackingField)) = flag;
		}
	}

	public unsafe bool _hasLights_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hasLights_k__BackingField);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hasLights_k__BackingField)) = flag;
		}
	}

	public unsafe bool _disableGlobalLight_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__disableGlobalLight_k__BackingField);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__disableGlobalLight_k__BackingField)) = flag;
		}
	}

	public unsafe bool _hasCharacterSpotlight_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hasCharacterSpotlight_k__BackingField);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hasCharacterSpotlight_k__BackingField)) = flag;
		}
	}

	public unsafe bool _dayNight_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__dayNight_k__BackingField);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__dayNight_k__BackingField)) = flag;
		}
	}

	public unsafe uint _DayColor_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__DayColor_k__BackingField);
			return *(uint*)num;
		}
		set
		{
			*(uint*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__DayColor_k__BackingField)) = num;
		}
	}

	public unsafe uint _NightColor_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__NightColor_k__BackingField);
			return *(uint*)num;
		}
		set
		{
			*(uint*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__NightColor_k__BackingField)) = num;
		}
	}

	public unsafe uint _InverseDayColor_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__InverseDayColor_k__BackingField);
			return *(uint*)num;
		}
		set
		{
			*(uint*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__InverseDayColor_k__BackingField)) = num;
		}
	}

	public unsafe uint _InverseNightColor_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__InverseNightColor_k__BackingField);
			return *(uint*)num;
		}
		set
		{
			*(uint*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__InverseNightColor_k__BackingField)) = num;
		}
	}

	public unsafe TilemapTiledJSON _tilemapTiledJSON_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__tilemapTiledJSON_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TilemapTiledJSON>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__tilemapTiledJSON_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)tilemapTiledJSON));
		}
	}

	public unsafe TilemapTiledIMG _tilemapTiledIMG_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__tilemapTiledIMG_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TilemapTiledIMG>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__tilemapTiledIMG_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)tilemapTiledIMG));
		}
	}

	public unsafe TilemapPos _tilemapPos_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__tilemapPos_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TilemapPos>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__tilemapPos_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)tilemapPos));
		}
	}

	public unsafe int _minimum_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__minimum_k__BackingField);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__minimum_k__BackingField)) = num;
		}
	}

	public unsafe float _frequency_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__frequency_k__BackingField);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__frequency_k__BackingField)) = num;
		}
	}

	public unsafe Il2CppSystem.Nullable<float> _zoom_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__zoom_k__BackingField);
			return new Il2CppSystem.Nullable<float>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<Il2CppSystem.Nullable<float>>.NativeClassPtr, (System.IntPtr)num));
		}
		set
		{
			// IL cpblk instruction
			Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__zoom_k__BackingField), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)nullable)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<Il2CppSystem.Nullable<float>>.NativeClassPtr, ref *(uint*)null));
		}
	}

	public unsafe List<Il2CppSystem.Nullable<EnemyType>> _enemies_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__enemies_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Il2CppSystem.Nullable<EnemyType>>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__enemies_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe List<Il2CppSystem.Nullable<EnemyType>> _bosses_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__bosses_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Il2CppSystem.Nullable<EnemyType>>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__bosses_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe Treasure _treasure_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__treasure_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Treasure>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__treasure_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)treasure));
		}
	}

	public unsafe Il2CppSystem.Nullable<EnemyType> _arcanaHolder_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__arcanaHolder_k__BackingField);
			return new Il2CppSystem.Nullable<EnemyType>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<Il2CppSystem.Nullable<EnemyType>>.NativeClassPtr, (System.IntPtr)num));
		}
		set
		{
			// IL cpblk instruction
			Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__arcanaHolder_k__BackingField), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)nullable)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<Il2CppSystem.Nullable<EnemyType>>.NativeClassPtr, ref *(uint*)null));
		}
	}

	public unsafe Treasure _arcanaTreasure_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__arcanaTreasure_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Treasure>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__arcanaTreasure_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)treasure));
		}
	}

	public unsafe List<Event> _events_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__events_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Event>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__events_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe List<Event> _pizzaEvents_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__pizzaEvents_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Event>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__pizzaEvents_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe Il2CppSystem.Nullable<CharacterType> _cff_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__cff_k__BackingField);
			return new Il2CppSystem.Nullable<CharacterType>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<Il2CppSystem.Nullable<CharacterType>>.NativeClassPtr, (System.IntPtr)num));
		}
		set
		{
			// IL cpblk instruction
			Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__cff_k__BackingField), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)nullable)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<Il2CppSystem.Nullable<CharacterType>>.NativeClassPtr, ref *(uint*)null));
		}
	}

	public unsafe List<ItemType> _LootTable_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__LootTable_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<ItemType>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__LootTable_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe List<ItemType> _relics_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__relics_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<ItemType>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__relics_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe List<ItemType> _relics2_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__relics2_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<ItemType>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__relics2_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe List<ItemType> _yellowRelics_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__yellowRelics_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<ItemType>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__yellowRelics_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe PreloadData _preload_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__preload_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PreloadData>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__preload_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)preloadData));
		}
	}

	public unsafe List<CustomMerchantData> _adventureMerchants_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__adventureMerchants_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<CustomMerchantData>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__adventureMerchants_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe List<FollowerData> _defaultFollowers_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__defaultFollowers_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<FollowerData>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__defaultFollowers_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe Il2CppSystem.Nullable<float> _adventurePriceMarkup_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__adventurePriceMarkup_k__BackingField);
			return new Il2CppSystem.Nullable<float>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<Il2CppSystem.Nullable<float>>.NativeClassPtr, (System.IntPtr)num));
		}
		set
		{
			// IL cpblk instruction
			Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__adventurePriceMarkup_k__BackingField), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)nullable)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<Il2CppSystem.Nullable<float>>.NativeClassPtr, ref *(uint*)null));
		}
	}

	public unsafe bool _isRacingStage_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__isRacingStage_k__BackingField);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__isRacingStage_k__BackingField)) = flag;
		}
	}

	public unsafe bool _skipVisualInversion_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__skipVisualInversion_k__BackingField);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__skipVisualInversion_k__BackingField)) = flag;
		}
	}

	public unsafe bool _allowVisualInversion_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allowVisualInversion_k__BackingField);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__allowVisualInversion_k__BackingField)) = flag;
		}
	}

	public unsafe string _biome_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__biome_k__BackingField);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__biome_k__BackingField)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe List<string> _biomes_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__biomes_k__BackingField);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<string>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__biomes_k__BackingField)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe Il2CppSystem.Nullable<EnemyType> _batDragonType_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__batDragonType_k__BackingField);
			return new Il2CppSystem.Nullable<EnemyType>(IL2CPP.il2cpp_value_box(Il2CppClassPointerStore<Il2CppSystem.Nullable<EnemyType>>.NativeClassPtr, (System.IntPtr)num));
		}
		set
		{
			// IL cpblk instruction
			Unsafe.CopyBlock((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__batDragonType_k__BackingField), IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)nullable)), IL2CPP.il2cpp_class_value_size(Il2CppClassPointerStore<Il2CppSystem.Nullable<EnemyType>>.NativeClassPtr, ref *(uint*)null));
		}
	}

	public unsafe int order
	{
		[CallerCount(6)]
		[CachedScanResults(RefRangeStart = 495343, RefRangeEnd = 495349, XrefRangeStart = 495343, XrefRangeEnd = 495349, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_order_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 40, RefRangeEnd = 41, XrefRangeStart = 40, XrefRangeEnd = 41, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_order_Public_set_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Il2CppSystem.Nullable<StageType> tilesetStageType
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr);
			System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_tilesetStageType_Public_get_Nullable_1_StageType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
			Il2CppException.RaiseExceptionIfNecessary(intPtr);
			return new Il2CppSystem.Nullable<StageType>(pointer);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)value));
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_tilesetStageType_Public_set_Void_Nullable_1_StageType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe string stageName
	{
		[CallerCount(739)]
		[CachedScanResults(RefRangeStart = 870190, RefRangeEnd = 870929, XrefRangeStart = 870190, XrefRangeEnd = 870929, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_stageName_Public_get_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		[CallerCount(60)]
		[CachedScanResults(RefRangeStart = 883857, RefRangeEnd = 883917, XrefRangeStart = 883857, XrefRangeEnd = 883917, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_stageName_Public_set_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe string description
	{
		[CallerCount(29)]
		[CachedScanResults(RefRangeStart = 877012, RefRangeEnd = 877041, XrefRangeStart = 877012, XrefRangeEnd = 877041, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_description_Public_get_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		[CallerCount(95)]
		[CachedScanResults(RefRangeStart = 883917, RefRangeEnd = 884012, XrefRangeStart = 883917, XrefRangeEnd = 884012, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_description_Public_set_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe string uiTexture
	{
		[CallerCount(12)]
		[CachedScanResults(RefRangeStart = 876177, RefRangeEnd = 876189, XrefRangeStart = 876177, XrefRangeEnd = 876189, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_uiTexture_Public_get_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		[CallerCount(52)]
		[CachedScanResults(RefRangeStart = 884012, RefRangeEnd = 884064, XrefRangeStart = 884012, XrefRangeEnd = 884064, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_uiTexture_Public_set_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe string uiFrame
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 887705, RefRangeEnd = 887706, XrefRangeStart = 887705, XrefRangeEnd = 887706, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_uiFrame_Public_get_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		[CallerCount(72)]
		[CachedScanResults(RefRangeStart = 883281, RefRangeEnd = 883353, XrefRangeStart = 883281, XrefRangeEnd = 883353, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_uiFrame_Public_set_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe string texture
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 899615, RefRangeEnd = 899616, XrefRangeStart = 899615, XrefRangeEnd = 899616, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_texture_Public_get_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		[CallerCount(43)]
		[CachedScanResults(RefRangeStart = 958550, RefRangeEnd = 958593, XrefRangeStart = 958550, XrefRangeEnd = 958593, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_texture_Public_set_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe string bestiaryBG
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_bestiaryBG_Public_get_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		[CallerCount(63)]
		[CachedScanResults(RefRangeStart = 873168, RefRangeEnd = 873231, XrefRangeStart = 873168, XrefRangeEnd = 873231, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_bestiaryBG_Public_set_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe string stageNumber
	{
		[CallerCount(5)]
		[CachedScanResults(RefRangeStart = 902043, RefRangeEnd = 902048, XrefRangeStart = 902043, XrefRangeEnd = 902048, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_stageNumber_Public_get_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		[CallerCount(34)]
		[CachedScanResults(RefRangeStart = 899616, RefRangeEnd = 899650, XrefRangeStart = 899616, XrefRangeEnd = 899650, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_stageNumber_Public_set_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe string frameName
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_frameName_Public_get_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		[CallerCount(15)]
		[CachedScanResults(RefRangeStart = 918342, RefRangeEnd = 918357, XrefRangeStart = 918342, XrefRangeEnd = 918357, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_frameName_Public_set_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe string frameNameUnlock
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_frameNameUnlock_Public_get_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		[CallerCount(48)]
		[CachedScanResults(RefRangeStart = 891305, RefRangeEnd = 891353, XrefRangeStart = 891305, XrefRangeEnd = 891353, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_frameNameUnlock_Public_set_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool unlocked
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_unlocked_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_unlocked_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe BgmType BGM
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_BGM_Public_get_BgmType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(BgmType*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_BGM_Public_set_Void_BgmType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Il2CppSystem.Nullable<BgmType> sideBBGM
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr);
			System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_sideBBGM_Public_get_Nullable_1_BgmType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
			Il2CppException.RaiseExceptionIfNecessary(intPtr);
			return new Il2CppSystem.Nullable<BgmType>(pointer);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)value));
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_sideBBGM_Public_set_Void_Nullable_1_BgmType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe string legacyBGM
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_legacyBGM_Public_get_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 494892, RefRangeEnd = 494896, XrefRangeStart = 494892, XrefRangeEnd = 494896, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_legacyBGM_Public_set_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe string tips
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 885995, RefRangeEnd = 885996, XrefRangeStart = 885995, XrefRangeEnd = 885996, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_tips_Public_get_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		[CallerCount(20)]
		[CachedScanResults(RefRangeStart = 1011323, RefRangeEnd = 1011343, XrefRangeStart = 1011323, XrefRangeEnd = 1011343, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_tips_Public_set_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe string hyperTips
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_hyperTips_Public_get_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		[CallerCount(11)]
		[CachedScanResults(RefRangeStart = 1004562, RefRangeEnd = 1004573, XrefRangeStart = 1004562, XrefRangeEnd = 1004573, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_hyperTips_Public_set_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool validForCharcaterData
	{
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 891353, RefRangeEnd = 891355, XrefRangeStart = 891353, XrefRangeEnd = 891355, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_validForCharcaterData_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_validForCharcaterData_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool hidden
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_hidden_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_hidden_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool alwaysHidden
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_alwaysHidden_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_alwaysHidden_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe StageModifiers mods
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_mods_Public_get_StageModifiers_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<StageModifiers>(intPtr) : null;
		}
		[CallerCount(17)]
		[CachedScanResults(RefRangeStart = 888369, RefRangeEnd = 888386, XrefRangeStart = 888369, XrefRangeEnd = 888386, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_mods_Public_set_Void_StageModifiers_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe StageModifiers hyper
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_hyper_Public_get_StageModifiers_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<StageModifiers>(intPtr) : null;
		}
		[CallerCount(15)]
		[CachedScanResults(RefRangeStart = 495087, RefRangeEnd = 495102, XrefRangeStart = 495087, XrefRangeEnd = 495102, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_hyper_Public_set_Void_StageModifiers_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe StageModifiers inverse
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_inverse_Public_get_StageModifiers_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<StageModifiers>(intPtr) : null;
		}
		[CallerCount(10)]
		[CachedScanResults(RefRangeStart = 1171708, RefRangeEnd = 1171718, XrefRangeStart = 1171708, XrefRangeEnd = 1171718, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_inverse_Public_set_Void_StageModifiers_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Tileset tileset
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_tileset_Public_get_Tileset_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Tileset>(intPtr) : null;
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_tileset_Public_set_Void_Tileset_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Background background
	{
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 886002, RefRangeEnd = 886005, XrefRangeStart = 886002, XrefRangeEnd = 886005, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_background_Public_get_Background_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Background>(intPtr) : null;
		}
		[CallerCount(5)]
		[CachedScanResults(RefRangeStart = 887764, RefRangeEnd = 887769, XrefRangeStart = 887764, XrefRangeEnd = 887769, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_background_Public_set_Void_Background_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe List<PoolsMapping> poolsMapping
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_poolsMapping_Public_get_List_1_PoolsMapping_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<PoolsMapping>>(intPtr) : null;
		}
		[CallerCount(5)]
		[CachedScanResults(RefRangeStart = 1565047, RefRangeEnd = 1565052, XrefRangeStart = 1565047, XrefRangeEnd = 1565052, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_poolsMapping_Public_set_Void_List_1_PoolsMapping_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe string spawnType
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_spawnType_Public_get_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_spawnType_Public_set_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe int startingSpawns
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_startingSpawns_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_startingSpawns_Public_set_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe int minute
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_minute_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_minute_Public_set_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool randomMinutes
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_randomMinutes_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_randomMinutes_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe string destructibleType
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_destructibleType_Public_get_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		[CallerCount(11)]
		[CachedScanResults(RefRangeStart = 1083014, RefRangeEnd = 1083025, XrefRangeStart = 1083014, XrefRangeEnd = 1083025, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_destructibleType_Public_set_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float destructibleFreq
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_destructibleFreq_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_destructibleFreq_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float destructibleChance
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_destructibleChance_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_destructibleChance_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float destructibleChanceMax
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_destructibleChanceMax_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_destructibleChanceMax_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe int maxDestructibles
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_maxDestructibles_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_maxDestructibles_Public_set_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe string BGTextureName
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_BGTextureName_Public_get_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		[CallerCount(6)]
		[CachedScanResults(RefRangeStart = 1134500, RefRangeEnd = 1134506, XrefRangeStart = 1134500, XrefRangeEnd = 1134506, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_BGTextureName_Public_set_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe string Extra_Texture
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 904910, RefRangeEnd = 904911, XrefRangeStart = 904910, XrefRangeEnd = 904911, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Extra_Texture_Public_get_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Extra_Texture_Public_set_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe BgmType Extra_Audio
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Extra_Audio_Public_get_BgmType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(BgmType*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Extra_Audio_Public_set_Void_BgmType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool isMerchantBanned
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_isMerchantBanned_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_isMerchantBanned_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool isSpeedupBanned
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_isSpeedupBanned_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_isSpeedupBanned_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool isSuvarotsBlocked
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_isSuvarotsBlocked_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_isSuvarotsBlocked_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool hasLights
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_hasLights_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_hasLights_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool disableGlobalLight
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_disableGlobalLight_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_disableGlobalLight_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool hasCharacterSpotlight
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_hasCharacterSpotlight_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_hasCharacterSpotlight_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool dayNight
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_dayNight_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_dayNight_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe uint DayColor
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_DayColor_Public_get_UInt32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(uint*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_DayColor_Public_set_Void_UInt32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe uint NightColor
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_NightColor_Public_get_UInt32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(uint*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_NightColor_Public_set_Void_UInt32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe uint InverseDayColor
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_InverseDayColor_Public_get_UInt32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(uint*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_InverseDayColor_Public_set_Void_UInt32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe uint InverseNightColor
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_InverseNightColor_Public_get_UInt32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(uint*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_InverseNightColor_Public_set_Void_UInt32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe TilemapTiledJSON tilemapTiledJSON
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_tilemapTiledJSON_Public_get_TilemapTiledJSON_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TilemapTiledJSON>(intPtr) : null;
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 1071834, RefRangeEnd = 1071835, XrefRangeStart = 1071834, XrefRangeEnd = 1071835, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_tilemapTiledJSON_Public_set_Void_TilemapTiledJSON_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe TilemapTiledIMG tilemapTiledIMG
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_tilemapTiledIMG_Public_get_TilemapTiledIMG_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TilemapTiledIMG>(intPtr) : null;
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 949826, RefRangeEnd = 949827, XrefRangeStart = 949826, XrefRangeEnd = 949827, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_tilemapTiledIMG_Public_set_Void_TilemapTiledIMG_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe TilemapPos tilemapPos
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_tilemapPos_Public_get_TilemapPos_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TilemapPos>(intPtr) : null;
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_tilemapPos_Public_set_Void_TilemapPos_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe int minimum
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_minimum_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_minimum_Public_set_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe float frequency
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_frequency_Public_get_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_frequency_Public_set_Void_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Il2CppSystem.Nullable<float> zoom
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr);
			System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_zoom_Public_get_Nullable_1_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
			Il2CppException.RaiseExceptionIfNecessary(intPtr);
			return new Il2CppSystem.Nullable<float>(pointer);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)value));
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_zoom_Public_set_Void_Nullable_1_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe List<Il2CppSystem.Nullable<EnemyType>> enemies
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_enemies_Public_get_List_1_Nullable_1_EnemyType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Il2CppSystem.Nullable<EnemyType>>>(intPtr) : null;
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 1562866, RefRangeEnd = 1562867, XrefRangeStart = 1562866, XrefRangeEnd = 1562867, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_enemies_Public_set_Void_List_1_Nullable_1_EnemyType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe List<Il2CppSystem.Nullable<EnemyType>> bosses
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_bosses_Public_get_List_1_Nullable_1_EnemyType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Il2CppSystem.Nullable<EnemyType>>>(intPtr) : null;
		}
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 1123920, RefRangeEnd = 1123924, XrefRangeStart = 1123920, XrefRangeEnd = 1123924, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_bosses_Public_set_Void_List_1_Nullable_1_EnemyType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Treasure treasure
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_treasure_Public_get_Treasure_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Treasure>(intPtr) : null;
		}
		[CallerCount(3)]
		[CachedScanResults(RefRangeStart = 1172820, RefRangeEnd = 1172823, XrefRangeStart = 1172820, XrefRangeEnd = 1172823, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_treasure_Public_set_Void_Treasure_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Il2CppSystem.Nullable<EnemyType> arcanaHolder
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr);
			System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_arcanaHolder_Public_get_Nullable_1_EnemyType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
			Il2CppException.RaiseExceptionIfNecessary(intPtr);
			return new Il2CppSystem.Nullable<EnemyType>(pointer);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)value));
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_arcanaHolder_Public_set_Void_Nullable_1_EnemyType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Treasure arcanaTreasure
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_arcanaTreasure_Public_get_Treasure_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Treasure>(intPtr) : null;
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 1221051, RefRangeEnd = 1221052, XrefRangeStart = 1221051, XrefRangeEnd = 1221052, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_arcanaTreasure_Public_set_Void_Treasure_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe List<Event> events
	{
		[CallerCount(6)]
		[CachedScanResults(RefRangeStart = 1185953, RefRangeEnd = 1185959, XrefRangeStart = 1185953, XrefRangeEnd = 1185959, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_events_Public_get_List_1_Event_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Event>>(intPtr) : null;
		}
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 1205870, RefRangeEnd = 1205872, XrefRangeStart = 1205870, XrefRangeEnd = 1205872, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_events_Public_set_Void_List_1_Event_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe List<Event> pizzaEvents
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_pizzaEvents_Public_get_List_1_Event_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Event>>(intPtr) : null;
		}
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 1205872, RefRangeEnd = 1205874, XrefRangeStart = 1205872, XrefRangeEnd = 1205874, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_pizzaEvents_Public_set_Void_List_1_Event_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Il2CppSystem.Nullable<CharacterType> cff
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr);
			System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_cff_Public_get_Nullable_1_CharacterType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
			Il2CppException.RaiseExceptionIfNecessary(intPtr);
			return new Il2CppSystem.Nullable<CharacterType>(pointer);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)value));
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_cff_Public_set_Void_Nullable_1_CharacterType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe List<ItemType> LootTable
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_LootTable_Public_get_List_1_ItemType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<ItemType>>(intPtr) : null;
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 1038917, RefRangeEnd = 1038918, XrefRangeStart = 1038917, XrefRangeEnd = 1038918, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_LootTable_Public_set_Void_List_1_ItemType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe List<ItemType> relics
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_relics_Public_get_List_1_ItemType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<ItemType>>(intPtr) : null;
		}
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 1176201, RefRangeEnd = 1176202, XrefRangeStart = 1176201, XrefRangeEnd = 1176202, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_relics_Public_set_Void_List_1_ItemType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe List<ItemType> relics2
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_relics2_Public_get_List_1_ItemType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<ItemType>>(intPtr) : null;
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_relics2_Public_set_Void_List_1_ItemType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe List<ItemType> yellowRelics
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_yellowRelics_Public_get_List_1_ItemType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<ItemType>>(intPtr) : null;
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_yellowRelics_Public_set_Void_List_1_ItemType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe PreloadData preload
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_preload_Public_get_PreloadData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<PreloadData>(intPtr) : null;
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_preload_Public_set_Void_PreloadData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe List<CustomMerchantData> adventureMerchants
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_adventureMerchants_Public_get_List_1_CustomMerchantData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<CustomMerchantData>>(intPtr) : null;
		}
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 1201393, RefRangeEnd = 1201395, XrefRangeStart = 1201393, XrefRangeEnd = 1201395, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_adventureMerchants_Public_set_Void_List_1_CustomMerchantData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe List<FollowerData> defaultFollowers
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_defaultFollowers_Public_get_List_1_FollowerData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<FollowerData>>(intPtr) : null;
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_defaultFollowers_Public_set_Void_List_1_FollowerData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Il2CppSystem.Nullable<float> adventurePriceMarkup
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr);
			System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_adventurePriceMarkup_Public_get_Nullable_1_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
			Il2CppException.RaiseExceptionIfNecessary(intPtr);
			return new Il2CppSystem.Nullable<float>(pointer);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)value));
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_adventurePriceMarkup_Public_set_Void_Nullable_1_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool isRacingStage
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_isRacingStage_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_isRacingStage_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool skipVisualInversion
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_skipVisualInversion_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_skipVisualInversion_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe bool allowVisualInversion
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_allowVisualInversion_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_allowVisualInversion_Public_set_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe string biome
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_biome_Public_get_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.ManagedStringToIl2Cpp(value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_biome_Public_set_Void_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe List<string> biomes
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_biomes_Public_get_List_1_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<string>>(intPtr) : null;
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_biomes_Public_set_Void_List_1_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe Il2CppSystem.Nullable<EnemyType> batDragonType
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr);
			System.IntPtr pointer = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_batDragonType_Public_get_Nullable_1_EnemyType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr);
			Il2CppException.RaiseExceptionIfNecessary(intPtr);
			return new Il2CppSystem.Nullable<EnemyType>(pointer);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)value));
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_batDragonType_Public_set_Void_Nullable_1_EnemyType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	static StageData()
	{
		Il2CppClassPointerStore<StageData>.NativeClassPtr = IL2CPP.GetIl2CppClass("VampireSurvivors.Runtime.dll", "VampireSurvivors.Data.Stage", "StageData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<StageData>.NativeClassPtr);
		NativeFieldInfoPtr__order_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<order>k__BackingField");
		NativeFieldInfoPtr__tilesetStageType_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<tilesetStageType>k__BackingField");
		NativeFieldInfoPtr__stageName_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<stageName>k__BackingField");
		NativeFieldInfoPtr__description_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<description>k__BackingField");
		NativeFieldInfoPtr__uiTexture_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<uiTexture>k__BackingField");
		NativeFieldInfoPtr__uiFrame_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<uiFrame>k__BackingField");
		NativeFieldInfoPtr__texture_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<texture>k__BackingField");
		NativeFieldInfoPtr__bestiaryBG_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<bestiaryBG>k__BackingField");
		NativeFieldInfoPtr__stageNumber_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<stageNumber>k__BackingField");
		NativeFieldInfoPtr__frameName_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<frameName>k__BackingField");
		NativeFieldInfoPtr__frameNameUnlock_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<frameNameUnlock>k__BackingField");
		NativeFieldInfoPtr__unlocked_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<unlocked>k__BackingField");
		NativeFieldInfoPtr__BGM_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<BGM>k__BackingField");
		NativeFieldInfoPtr__sideBBGM_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<sideBBGM>k__BackingField");
		NativeFieldInfoPtr__legacyBGM_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<legacyBGM>k__BackingField");
		NativeFieldInfoPtr__tips_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<tips>k__BackingField");
		NativeFieldInfoPtr__hyperTips_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<hyperTips>k__BackingField");
		NativeFieldInfoPtr__validForCharcaterData_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<validForCharcaterData>k__BackingField");
		NativeFieldInfoPtr__hidden_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<hidden>k__BackingField");
		NativeFieldInfoPtr__alwaysHidden_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<alwaysHidden>k__BackingField");
		NativeFieldInfoPtr__mods_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<mods>k__BackingField");
		NativeFieldInfoPtr__hyper_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<hyper>k__BackingField");
		NativeFieldInfoPtr__inverse_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<inverse>k__BackingField");
		NativeFieldInfoPtr__tileset_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<tileset>k__BackingField");
		NativeFieldInfoPtr__background_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<background>k__BackingField");
		NativeFieldInfoPtr__poolsMapping_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<poolsMapping>k__BackingField");
		NativeFieldInfoPtr__spawnType_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<spawnType>k__BackingField");
		NativeFieldInfoPtr__startingSpawns_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<startingSpawns>k__BackingField");
		NativeFieldInfoPtr__minute_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<minute>k__BackingField");
		NativeFieldInfoPtr__randomMinutes_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<randomMinutes>k__BackingField");
		NativeFieldInfoPtr__destructibleType_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<destructibleType>k__BackingField");
		NativeFieldInfoPtr__destructibleFreq_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<destructibleFreq>k__BackingField");
		NativeFieldInfoPtr__destructibleChance_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<destructibleChance>k__BackingField");
		NativeFieldInfoPtr__destructibleChanceMax_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<destructibleChanceMax>k__BackingField");
		NativeFieldInfoPtr__maxDestructibles_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<maxDestructibles>k__BackingField");
		NativeFieldInfoPtr__BGTextureName_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<BGTextureName>k__BackingField");
		NativeFieldInfoPtr__Extra_Texture_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<Extra_Texture>k__BackingField");
		NativeFieldInfoPtr__Extra_Audio_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<Extra_Audio>k__BackingField");
		NativeFieldInfoPtr__isMerchantBanned_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<isMerchantBanned>k__BackingField");
		NativeFieldInfoPtr__isSpeedupBanned_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<isSpeedupBanned>k__BackingField");
		NativeFieldInfoPtr__isSuvarotsBlocked_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<isSuvarotsBlocked>k__BackingField");
		NativeFieldInfoPtr__hasLights_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<hasLights>k__BackingField");
		NativeFieldInfoPtr__disableGlobalLight_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<disableGlobalLight>k__BackingField");
		NativeFieldInfoPtr__hasCharacterSpotlight_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<hasCharacterSpotlight>k__BackingField");
		NativeFieldInfoPtr__dayNight_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<dayNight>k__BackingField");
		NativeFieldInfoPtr__DayColor_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<DayColor>k__BackingField");
		NativeFieldInfoPtr__NightColor_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<NightColor>k__BackingField");
		NativeFieldInfoPtr__InverseDayColor_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<InverseDayColor>k__BackingField");
		NativeFieldInfoPtr__InverseNightColor_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<InverseNightColor>k__BackingField");
		NativeFieldInfoPtr__tilemapTiledJSON_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<tilemapTiledJSON>k__BackingField");
		NativeFieldInfoPtr__tilemapTiledIMG_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<tilemapTiledIMG>k__BackingField");
		NativeFieldInfoPtr__tilemapPos_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<tilemapPos>k__BackingField");
		NativeFieldInfoPtr__minimum_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<minimum>k__BackingField");
		NativeFieldInfoPtr__frequency_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<frequency>k__BackingField");
		NativeFieldInfoPtr__zoom_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<zoom>k__BackingField");
		NativeFieldInfoPtr__enemies_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<enemies>k__BackingField");
		NativeFieldInfoPtr__bosses_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<bosses>k__BackingField");
		NativeFieldInfoPtr__treasure_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<treasure>k__BackingField");
		NativeFieldInfoPtr__arcanaHolder_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<arcanaHolder>k__BackingField");
		NativeFieldInfoPtr__arcanaTreasure_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<arcanaTreasure>k__BackingField");
		NativeFieldInfoPtr__events_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<events>k__BackingField");
		NativeFieldInfoPtr__pizzaEvents_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<pizzaEvents>k__BackingField");
		NativeFieldInfoPtr__cff_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<cff>k__BackingField");
		NativeFieldInfoPtr__LootTable_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<LootTable>k__BackingField");
		NativeFieldInfoPtr__relics_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<relics>k__BackingField");
		NativeFieldInfoPtr__relics2_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<relics2>k__BackingField");
		NativeFieldInfoPtr__yellowRelics_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<yellowRelics>k__BackingField");
		NativeFieldInfoPtr__preload_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<preload>k__BackingField");
		NativeFieldInfoPtr__adventureMerchants_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<adventureMerchants>k__BackingField");
		NativeFieldInfoPtr__defaultFollowers_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<defaultFollowers>k__BackingField");
		NativeFieldInfoPtr__adventurePriceMarkup_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<adventurePriceMarkup>k__BackingField");
		NativeFieldInfoPtr__isRacingStage_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<isRacingStage>k__BackingField");
		NativeFieldInfoPtr__skipVisualInversion_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<skipVisualInversion>k__BackingField");
		NativeFieldInfoPtr__allowVisualInversion_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<allowVisualInversion>k__BackingField");
		NativeFieldInfoPtr__biome_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<biome>k__BackingField");
		NativeFieldInfoPtr__biomes_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<biomes>k__BackingField");
		NativeFieldInfoPtr__batDragonType_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageData>.NativeClassPtr, "<batDragonType>k__BackingField");
		NativeMethodInfoPtr_get_order_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728127);
		NativeMethodInfoPtr_set_order_Public_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728128);
		NativeMethodInfoPtr_get_tilesetStageType_Public_get_Nullable_1_StageType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728129);
		NativeMethodInfoPtr_set_tilesetStageType_Public_set_Void_Nullable_1_StageType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728130);
		NativeMethodInfoPtr_get_stageName_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728131);
		NativeMethodInfoPtr_set_stageName_Public_set_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728132);
		NativeMethodInfoPtr_get_description_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728133);
		NativeMethodInfoPtr_set_description_Public_set_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728134);
		NativeMethodInfoPtr_get_uiTexture_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728135);
		NativeMethodInfoPtr_set_uiTexture_Public_set_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728136);
		NativeMethodInfoPtr_get_uiFrame_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728137);
		NativeMethodInfoPtr_set_uiFrame_Public_set_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728138);
		NativeMethodInfoPtr_get_texture_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728139);
		NativeMethodInfoPtr_set_texture_Public_set_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728140);
		NativeMethodInfoPtr_get_bestiaryBG_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728141);
		NativeMethodInfoPtr_set_bestiaryBG_Public_set_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728142);
		NativeMethodInfoPtr_get_stageNumber_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728143);
		NativeMethodInfoPtr_set_stageNumber_Public_set_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728144);
		NativeMethodInfoPtr_get_frameName_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728145);
		NativeMethodInfoPtr_set_frameName_Public_set_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728146);
		NativeMethodInfoPtr_get_frameNameUnlock_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728147);
		NativeMethodInfoPtr_set_frameNameUnlock_Public_set_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728148);
		NativeMethodInfoPtr_get_unlocked_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728149);
		NativeMethodInfoPtr_set_unlocked_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728150);
		NativeMethodInfoPtr_get_BGM_Public_get_BgmType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728151);
		NativeMethodInfoPtr_set_BGM_Public_set_Void_BgmType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728152);
		NativeMethodInfoPtr_get_sideBBGM_Public_get_Nullable_1_BgmType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728153);
		NativeMethodInfoPtr_set_sideBBGM_Public_set_Void_Nullable_1_BgmType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728154);
		NativeMethodInfoPtr_get_legacyBGM_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728155);
		NativeMethodInfoPtr_set_legacyBGM_Public_set_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728156);
		NativeMethodInfoPtr_get_tips_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728157);
		NativeMethodInfoPtr_set_tips_Public_set_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728158);
		NativeMethodInfoPtr_get_hyperTips_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728159);
		NativeMethodInfoPtr_set_hyperTips_Public_set_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728160);
		NativeMethodInfoPtr_get_validForCharcaterData_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728161);
		NativeMethodInfoPtr_set_validForCharcaterData_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728162);
		NativeMethodInfoPtr_get_hidden_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728163);
		NativeMethodInfoPtr_set_hidden_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728164);
		NativeMethodInfoPtr_get_alwaysHidden_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728165);
		NativeMethodInfoPtr_set_alwaysHidden_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728166);
		NativeMethodInfoPtr_get_mods_Public_get_StageModifiers_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728167);
		NativeMethodInfoPtr_set_mods_Public_set_Void_StageModifiers_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728168);
		NativeMethodInfoPtr_get_hyper_Public_get_StageModifiers_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728169);
		NativeMethodInfoPtr_set_hyper_Public_set_Void_StageModifiers_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728170);
		NativeMethodInfoPtr_get_inverse_Public_get_StageModifiers_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728171);
		NativeMethodInfoPtr_set_inverse_Public_set_Void_StageModifiers_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728172);
		NativeMethodInfoPtr_get_tileset_Public_get_Tileset_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728173);
		NativeMethodInfoPtr_set_tileset_Public_set_Void_Tileset_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728174);
		NativeMethodInfoPtr_get_background_Public_get_Background_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728175);
		NativeMethodInfoPtr_set_background_Public_set_Void_Background_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728176);
		NativeMethodInfoPtr_get_poolsMapping_Public_get_List_1_PoolsMapping_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728177);
		NativeMethodInfoPtr_set_poolsMapping_Public_set_Void_List_1_PoolsMapping_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728178);
		NativeMethodInfoPtr_get_spawnType_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728179);
		NativeMethodInfoPtr_set_spawnType_Public_set_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728180);
		NativeMethodInfoPtr_get_startingSpawns_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728181);
		NativeMethodInfoPtr_set_startingSpawns_Public_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728182);
		NativeMethodInfoPtr_get_minute_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728183);
		NativeMethodInfoPtr_set_minute_Public_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728184);
		NativeMethodInfoPtr_get_randomMinutes_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728185);
		NativeMethodInfoPtr_set_randomMinutes_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728186);
		NativeMethodInfoPtr_get_destructibleType_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728187);
		NativeMethodInfoPtr_set_destructibleType_Public_set_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728188);
		NativeMethodInfoPtr_get_destructibleFreq_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728189);
		NativeMethodInfoPtr_set_destructibleFreq_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728190);
		NativeMethodInfoPtr_get_destructibleChance_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728191);
		NativeMethodInfoPtr_set_destructibleChance_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728192);
		NativeMethodInfoPtr_get_destructibleChanceMax_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728193);
		NativeMethodInfoPtr_set_destructibleChanceMax_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728194);
		NativeMethodInfoPtr_get_maxDestructibles_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728195);
		NativeMethodInfoPtr_set_maxDestructibles_Public_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728196);
		NativeMethodInfoPtr_get_BGTextureName_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728197);
		NativeMethodInfoPtr_set_BGTextureName_Public_set_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728198);
		NativeMethodInfoPtr_get_Extra_Texture_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728199);
		NativeMethodInfoPtr_set_Extra_Texture_Public_set_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728200);
		NativeMethodInfoPtr_get_Extra_Audio_Public_get_BgmType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728201);
		NativeMethodInfoPtr_set_Extra_Audio_Public_set_Void_BgmType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728202);
		NativeMethodInfoPtr_get_isMerchantBanned_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728203);
		NativeMethodInfoPtr_set_isMerchantBanned_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728204);
		NativeMethodInfoPtr_get_isSpeedupBanned_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728205);
		NativeMethodInfoPtr_set_isSpeedupBanned_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728206);
		NativeMethodInfoPtr_get_isSuvarotsBlocked_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728207);
		NativeMethodInfoPtr_set_isSuvarotsBlocked_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728208);
		NativeMethodInfoPtr_get_hasLights_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728209);
		NativeMethodInfoPtr_set_hasLights_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728210);
		NativeMethodInfoPtr_get_disableGlobalLight_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728211);
		NativeMethodInfoPtr_set_disableGlobalLight_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728212);
		NativeMethodInfoPtr_get_hasCharacterSpotlight_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728213);
		NativeMethodInfoPtr_set_hasCharacterSpotlight_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728214);
		NativeMethodInfoPtr_get_dayNight_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728215);
		NativeMethodInfoPtr_set_dayNight_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728216);
		NativeMethodInfoPtr_get_DayColor_Public_get_UInt32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728217);
		NativeMethodInfoPtr_set_DayColor_Public_set_Void_UInt32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728218);
		NativeMethodInfoPtr_get_NightColor_Public_get_UInt32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728219);
		NativeMethodInfoPtr_set_NightColor_Public_set_Void_UInt32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728220);
		NativeMethodInfoPtr_get_InverseDayColor_Public_get_UInt32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728221);
		NativeMethodInfoPtr_set_InverseDayColor_Public_set_Void_UInt32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728222);
		NativeMethodInfoPtr_get_InverseNightColor_Public_get_UInt32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728223);
		NativeMethodInfoPtr_set_InverseNightColor_Public_set_Void_UInt32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728224);
		NativeMethodInfoPtr_get_tilemapTiledJSON_Public_get_TilemapTiledJSON_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728225);
		NativeMethodInfoPtr_set_tilemapTiledJSON_Public_set_Void_TilemapTiledJSON_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728226);
		NativeMethodInfoPtr_get_tilemapTiledIMG_Public_get_TilemapTiledIMG_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728227);
		NativeMethodInfoPtr_set_tilemapTiledIMG_Public_set_Void_TilemapTiledIMG_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728228);
		NativeMethodInfoPtr_get_tilemapPos_Public_get_TilemapPos_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728229);
		NativeMethodInfoPtr_set_tilemapPos_Public_set_Void_TilemapPos_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728230);
		NativeMethodInfoPtr_get_minimum_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728231);
		NativeMethodInfoPtr_set_minimum_Public_set_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728232);
		NativeMethodInfoPtr_get_frequency_Public_get_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728233);
		NativeMethodInfoPtr_set_frequency_Public_set_Void_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728234);
		NativeMethodInfoPtr_get_zoom_Public_get_Nullable_1_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728235);
		NativeMethodInfoPtr_set_zoom_Public_set_Void_Nullable_1_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728236);
		NativeMethodInfoPtr_get_enemies_Public_get_List_1_Nullable_1_EnemyType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728237);
		NativeMethodInfoPtr_set_enemies_Public_set_Void_List_1_Nullable_1_EnemyType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728238);
		NativeMethodInfoPtr_get_bosses_Public_get_List_1_Nullable_1_EnemyType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728239);
		NativeMethodInfoPtr_set_bosses_Public_set_Void_List_1_Nullable_1_EnemyType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728240);
		NativeMethodInfoPtr_get_treasure_Public_get_Treasure_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728241);
		NativeMethodInfoPtr_set_treasure_Public_set_Void_Treasure_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728242);
		NativeMethodInfoPtr_get_arcanaHolder_Public_get_Nullable_1_EnemyType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728243);
		NativeMethodInfoPtr_set_arcanaHolder_Public_set_Void_Nullable_1_EnemyType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728244);
		NativeMethodInfoPtr_get_arcanaTreasure_Public_get_Treasure_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728245);
		NativeMethodInfoPtr_set_arcanaTreasure_Public_set_Void_Treasure_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728246);
		NativeMethodInfoPtr_get_events_Public_get_List_1_Event_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728247);
		NativeMethodInfoPtr_set_events_Public_set_Void_List_1_Event_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728248);
		NativeMethodInfoPtr_get_pizzaEvents_Public_get_List_1_Event_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728249);
		NativeMethodInfoPtr_set_pizzaEvents_Public_set_Void_List_1_Event_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728250);
		NativeMethodInfoPtr_get_cff_Public_get_Nullable_1_CharacterType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728251);
		NativeMethodInfoPtr_set_cff_Public_set_Void_Nullable_1_CharacterType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728252);
		NativeMethodInfoPtr_get_LootTable_Public_get_List_1_ItemType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728253);
		NativeMethodInfoPtr_set_LootTable_Public_set_Void_List_1_ItemType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728254);
		NativeMethodInfoPtr_get_relics_Public_get_List_1_ItemType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728255);
		NativeMethodInfoPtr_set_relics_Public_set_Void_List_1_ItemType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728256);
		NativeMethodInfoPtr_get_relics2_Public_get_List_1_ItemType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728257);
		NativeMethodInfoPtr_set_relics2_Public_set_Void_List_1_ItemType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728258);
		NativeMethodInfoPtr_get_yellowRelics_Public_get_List_1_ItemType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728259);
		NativeMethodInfoPtr_set_yellowRelics_Public_set_Void_List_1_ItemType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728260);
		NativeMethodInfoPtr_get_preload_Public_get_PreloadData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728261);
		NativeMethodInfoPtr_set_preload_Public_set_Void_PreloadData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728262);
		NativeMethodInfoPtr_get_adventureMerchants_Public_get_List_1_CustomMerchantData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728263);
		NativeMethodInfoPtr_set_adventureMerchants_Public_set_Void_List_1_CustomMerchantData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728264);
		NativeMethodInfoPtr_get_defaultFollowers_Public_get_List_1_FollowerData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728265);
		NativeMethodInfoPtr_set_defaultFollowers_Public_set_Void_List_1_FollowerData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728266);
		NativeMethodInfoPtr_get_adventurePriceMarkup_Public_get_Nullable_1_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728267);
		NativeMethodInfoPtr_set_adventurePriceMarkup_Public_set_Void_Nullable_1_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728268);
		NativeMethodInfoPtr_get_isRacingStage_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728269);
		NativeMethodInfoPtr_set_isRacingStage_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728270);
		NativeMethodInfoPtr_get_skipVisualInversion_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728271);
		NativeMethodInfoPtr_set_skipVisualInversion_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728272);
		NativeMethodInfoPtr_get_allowVisualInversion_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728273);
		NativeMethodInfoPtr_set_allowVisualInversion_Public_set_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728274);
		NativeMethodInfoPtr_get_biome_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728275);
		NativeMethodInfoPtr_set_biome_Public_set_Void_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728276);
		NativeMethodInfoPtr_get_biomes_Public_get_List_1_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728277);
		NativeMethodInfoPtr_set_biomes_Public_set_Void_List_1_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728278);
		NativeMethodInfoPtr_get_batDragonType_Public_get_Nullable_1_EnemyType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728279);
		NativeMethodInfoPtr_set_batDragonType_Public_set_Void_Nullable_1_EnemyType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728280);
		NativeMethodInfoPtr_GetLocalizedName_Public_String_StageType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728281);
		NativeMethodInfoPtr_GetLocalizedTips_Public_String_StageType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728282);
		NativeMethodInfoPtr_GetLocalizedHyperTips_Public_String_StageType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728283);
		NativeMethodInfoPtr_GetLocalizedDescription_Public_String_StageType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728284);
		NativeMethodInfoPtr_GetPrefix_Private_String_StageType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728285);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageData>.NativeClassPtr, 100728286);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 1566495, RefRangeEnd = 1566502, XrefRangeStart = 1566491, XrefRangeEnd = 1566495, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe string GetLocalizedName(StageType sType)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&sType);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetLocalizedName_Public_String_StageType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 1566506, RefRangeEnd = 1566508, XrefRangeStart = 1566502, XrefRangeEnd = 1566506, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe string GetLocalizedTips(StageType sType)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&sType);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetLocalizedTips_Public_String_StageType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1566508, XrefRangeEnd = 1566512, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe string GetLocalizedHyperTips(StageType sType)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&sType);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetLocalizedHyperTips_Public_String_StageType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 1566512, XrefRangeEnd = 1566516, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe string GetLocalizedDescription(StageType sType)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&sType);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetLocalizedDescription_Public_String_StageType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(9)]
	[CachedScanResults(RefRangeStart = 1566524, RefRangeEnd = 1566533, XrefRangeStart = 1566516, XrefRangeEnd = 1566524, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe string GetPrefix(StageType sType)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&sType);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetPrefix_Private_String_StageType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 1566594, RefRangeEnd = 1566597, XrefRangeStart = 1566533, XrefRangeEnd = 1566594, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe StageData()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<StageData>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public StageData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
