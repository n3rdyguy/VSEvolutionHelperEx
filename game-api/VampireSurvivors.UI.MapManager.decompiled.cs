using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using VampireSurvivors.Data;
using VampireSurvivors.Data.Items;
using VampireSurvivors.Data.Stage;
using VampireSurvivors.Data.Weapons;
using VampireSurvivors.Framework;
using VampireSurvivors.Framework.PhaserTweens;
using VampireSurvivors.Objects;
using VampireSurvivors.Objects.Pickups;

namespace VampireSurvivors.UI;

public class MapManager : MonoBehaviour
{
	[ObfuscatedName("VampireSurvivors.UI.MapManager+<>c__DisplayClass61_0")]
	public sealed class __c__DisplayClass61_0 : Il2CppSystem.ValueType
	{
		private static readonly System.IntPtr NativeFieldInfoPtr_frameName;

		private static readonly System.IntPtr NativeFieldInfoPtr_textureName;

		public unsafe string frameName
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_frameName);
				return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_frameName)), IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		public unsafe string textureName
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_textureName);
				return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_textureName)), IL2CPP.ManagedStringToIl2Cpp(text));
			}
		}

		static __c__DisplayClass61_0()
		{
			Il2CppClassPointerStore<__c__DisplayClass61_0>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "<>c__DisplayClass61_0");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c__DisplayClass61_0>.NativeClassPtr);
			NativeFieldInfoPtr_frameName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass61_0>.NativeClassPtr, "frameName");
			NativeFieldInfoPtr_textureName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c__DisplayClass61_0>.NativeClassPtr, "textureName");
		}

		public __c__DisplayClass61_0(System.IntPtr pointer)
			: base(pointer)
		{
		}

		public __c__DisplayClass61_0()
			: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<__c__DisplayClass61_0>.NativeClassPtr))
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr__Grid;

	private static readonly System.IntPtr NativeFieldInfoPtr__MapIcon;

	private static readonly System.IntPtr NativeFieldInfoPtr__Player;

	private static readonly System.IntPtr NativeFieldInfoPtr__Foreground;

	private static readonly System.IntPtr NativeFieldInfoPtr__ForegroundSupports;

	private static readonly System.IntPtr NativeFieldInfoPtr__MapStaticBackgroundImage;

	private static readonly System.IntPtr NativeFieldInfoPtr__DetailedMapContainer;

	private static readonly System.IntPtr NativeFieldInfoPtr__CanvasGroup;

	private static readonly System.IntPtr NativeFieldInfoPtr__AlphaWhileArcanaInfoShown;

	private static readonly System.IntPtr NativeFieldInfoPtr__session;

	private static readonly System.IntPtr NativeFieldInfoPtr__data;

	private static readonly System.IntPtr NativeFieldInfoPtr__gameManager;

	private static readonly System.IntPtr NativeFieldInfoPtr__playerOptions;

	private static readonly System.IntPtr NativeFieldInfoPtr__smallSprites;

	private static readonly System.IntPtr NativeFieldInfoPtr__spawned;

	private static readonly System.IntPtr NativeFieldInfoPtr__gridLines;

	private static readonly System.IntPtr NativeFieldInfoPtr__pickupFadeTweens;

	private static readonly System.IntPtr NativeFieldInfoPtr__width;

	private static readonly System.IntPtr NativeFieldInfoPtr__height;

	private static readonly System.IntPtr NativeFieldInfoPtr__zoom;

	private static readonly System.IntPtr NativeFieldInfoPtr__manualZoomFactor;

	private static readonly System.IntPtr NativeFieldInfoPtr__manualZoomStep;

	private static readonly System.IntPtr NativeFieldInfoPtr__manualZoomOutCap;

	private static readonly System.IntPtr NativeFieldInfoPtr__manualZoomInCap;

	private static readonly System.IntPtr NativeFieldInfoPtr__mapRatioX;

	private static readonly System.IntPtr NativeFieldInfoPtr__mapRatioY;

	private static readonly System.IntPtr NativeFieldInfoPtr__mapSpriteWidth;

	private static readonly System.IntPtr NativeFieldInfoPtr__mapSpriteHeight;

	private static readonly System.IntPtr NativeFieldInfoPtr__detailedMapSprite;

	private static readonly System.IntPtr NativeFieldInfoPtr__positionBuckets;

	private static readonly System.IntPtr NativeFieldInfoPtr_DefaultMapRatio;

	private static readonly System.IntPtr NativeFieldInfoPtr_DefaultMapSize;

	private static readonly System.IntPtr NativeFieldInfoPtr_DefaultPhaserZoom;

	private static readonly System.IntPtr NativeFieldInfoPtr_BaselineMapScale;

	private static readonly System.IntPtr NativeFieldInfoPtr_PhaserToUnityAdjustment;

	private static readonly System.IntPtr NativeFieldInfoPtr_DefaultPixelsPerTile;

	private static readonly System.IntPtr NativeFieldInfoPtr__minorItemTypes;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AllItemData_Private_get_Dictionary_2_ItemType_ItemData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_AllWeaponData_Private_get_Dictionary_2_WeaponType_List_1_WeaponData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Construct_Private_Void_GameSessionData_DataManager_GameManager_PlayerOptions_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Awake_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnDisable_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnDestroy_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetPickups_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ReduceAlphaOnArcanaInfoShown_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ResetToDefaultAlpha_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_IsMinorItem_Private_Boolean_Pickup_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetAllWorldItems_Private_List_1_Pickup_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ZoomIn_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ZoomOut_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Populate_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddPickupFadingTweens_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DrawMapBackground_Private_Void_StageData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MakeGrid_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_MakeGridLine_Private_GameObject_Boolean_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddMinorItems_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddPlayers_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ShouldSkipDrawingPickup_Private_Boolean_Pickup_Dictionary_2_int2_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddTreasureChests_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddStagePickups_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddMapTokens_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AddEventTargets_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SpawnItemOnMap_Private_GameObject_Sprite_Vector2_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClearIcons_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClearGrid_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InitMultiMaps_Private_Void_StageData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_InitDetailedMap_Private_Void_StageData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ShowDetailedMap_Private_Void_StageData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Method_Private_Void_byref___c__DisplayClass61_0_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Method_Private_Void_byref___c__DisplayClass61_0_1;

	public unsafe GameObject _Grid
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Grid);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<GameObject>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Grid)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gameObject));
		}
	}

	public unsafe GameObject _MapIcon
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__MapIcon);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<GameObject>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__MapIcon)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gameObject));
		}
	}

	public unsafe GameObject _Player
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Player);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<GameObject>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Player)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gameObject));
		}
	}

	public unsafe Image _Foreground
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Foreground);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Image>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Foreground)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)image));
		}
	}

	public unsafe List<Image> _ForegroundSupports
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__ForegroundSupports);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Image>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__ForegroundSupports)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe Image _MapStaticBackgroundImage
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__MapStaticBackgroundImage);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Image>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__MapStaticBackgroundImage)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)image));
		}
	}

	public unsafe RectTransform _DetailedMapContainer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__DetailedMapContainer);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<RectTransform>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__DetailedMapContainer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rectTransform));
		}
	}

	public unsafe CanvasGroup _CanvasGroup
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__CanvasGroup);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<CanvasGroup>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__CanvasGroup)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)canvasGroup));
		}
	}

	public unsafe float _AlphaWhileArcanaInfoShown
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AlphaWhileArcanaInfoShown);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AlphaWhileArcanaInfoShown)) = num;
		}
	}

	public unsafe GameSessionData _session
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__session);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<GameSessionData>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__session)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gameSessionData));
		}
	}

	public unsafe DataManager _data
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__data);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<DataManager>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__data)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dataManager));
		}
	}

	public unsafe GameManager _gameManager
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__gameManager);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<GameManager>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__gameManager)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gameManager));
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

	public unsafe List<GameObject> _smallSprites
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__smallSprites);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<GameObject>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__smallSprites)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe List<GameObject> _spawned
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__spawned);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<GameObject>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__spawned)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe List<GameObject> _gridLines
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__gridLines);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<GameObject>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__gridLines)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe MultiTargetTween _pickupFadeTweens
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__pickupFadeTweens);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<MultiTargetTween>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__pickupFadeTweens)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)multiTargetTween));
		}
	}

	public unsafe float _width
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__width);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__width)) = num;
		}
	}

	public unsafe float _height
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__height);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__height)) = num;
		}
	}

	public unsafe float _zoom
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__zoom);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__zoom)) = num;
		}
	}

	public unsafe float _manualZoomFactor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__manualZoomFactor);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__manualZoomFactor)) = num;
		}
	}

	public unsafe float _manualZoomStep
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__manualZoomStep);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__manualZoomStep)) = num;
		}
	}

	public unsafe float _manualZoomOutCap
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__manualZoomOutCap);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__manualZoomOutCap)) = num;
		}
	}

	public unsafe float _manualZoomInCap
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__manualZoomInCap);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__manualZoomInCap)) = num;
		}
	}

	public unsafe float _mapRatioX
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mapRatioX);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mapRatioX)) = num;
		}
	}

	public unsafe float _mapRatioY
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mapRatioY);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mapRatioY)) = num;
		}
	}

	public unsafe float _mapSpriteWidth
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mapSpriteWidth);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mapSpriteWidth)) = num;
		}
	}

	public unsafe float _mapSpriteHeight
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mapSpriteHeight);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mapSpriteHeight)) = num;
		}
	}

	public unsafe Sprite _detailedMapSprite
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__detailedMapSprite);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Sprite>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__detailedMapSprite)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sprite));
		}
	}

	public unsafe Dictionary<int2, int> _positionBuckets
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__positionBuckets);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<int2, int>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__positionBuckets)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dictionary));
		}
	}

	public unsafe static float DefaultMapRatio
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DefaultMapRatio, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DefaultMapRatio, (void*)(&num));
		}
	}

	public unsafe static float DefaultMapSize
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DefaultMapSize, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DefaultMapSize, (void*)(&num));
		}
	}

	public unsafe static float DefaultPhaserZoom
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DefaultPhaserZoom, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DefaultPhaserZoom, (void*)(&num));
		}
	}

	public unsafe static float BaselineMapScale
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_BaselineMapScale, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_BaselineMapScale, (void*)(&num));
		}
	}

	public unsafe static float PhaserToUnityAdjustment
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_PhaserToUnityAdjustment, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_PhaserToUnityAdjustment, (void*)(&num));
		}
	}

	public unsafe static float DefaultPixelsPerTile
	{
		get
		{
			Unsafe.SkipInit(out float result);
			IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr_DefaultPixelsPerTile, (void*)(&result));
			return result;
		}
		set
		{
			IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr_DefaultPixelsPerTile, (void*)(&num));
		}
	}

	public unsafe Il2CppStructArray<ItemType> _minorItemTypes
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__minorItemTypes);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<ItemType>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__minorItemTypes)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Dictionary<ItemType, ItemData> AllItemData
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AllItemData_Private_get_Dictionary_2_ItemType_ItemData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<ItemType, ItemData>>(intPtr) : null;
		}
	}

	public unsafe Dictionary<WeaponType, List<WeaponData>> AllWeaponData
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 971853, XrefRangeEnd = 971855, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_AllWeaponData_Private_get_Dictionary_2_WeaponType_List_1_WeaponData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<WeaponType, List<WeaponData>>>(intPtr) : null;
		}
	}

	static MapManager()
	{
		Il2CppClassPointerStore<MapManager>.NativeClassPtr = IL2CPP.GetIl2CppClass("VampireSurvivors.Runtime.dll", "VampireSurvivors.UI", "MapManager");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<MapManager>.NativeClassPtr);
		NativeFieldInfoPtr__Grid = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "_Grid");
		NativeFieldInfoPtr__MapIcon = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "_MapIcon");
		NativeFieldInfoPtr__Player = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "_Player");
		NativeFieldInfoPtr__Foreground = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "_Foreground");
		NativeFieldInfoPtr__ForegroundSupports = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "_ForegroundSupports");
		NativeFieldInfoPtr__MapStaticBackgroundImage = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "_MapStaticBackgroundImage");
		NativeFieldInfoPtr__DetailedMapContainer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "_DetailedMapContainer");
		NativeFieldInfoPtr__CanvasGroup = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "_CanvasGroup");
		NativeFieldInfoPtr__AlphaWhileArcanaInfoShown = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "_AlphaWhileArcanaInfoShown");
		NativeFieldInfoPtr__session = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "_session");
		NativeFieldInfoPtr__data = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "_data");
		NativeFieldInfoPtr__gameManager = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "_gameManager");
		NativeFieldInfoPtr__playerOptions = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "_playerOptions");
		NativeFieldInfoPtr__smallSprites = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "_smallSprites");
		NativeFieldInfoPtr__spawned = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "_spawned");
		NativeFieldInfoPtr__gridLines = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "_gridLines");
		NativeFieldInfoPtr__pickupFadeTweens = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "_pickupFadeTweens");
		NativeFieldInfoPtr__width = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "_width");
		NativeFieldInfoPtr__height = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "_height");
		NativeFieldInfoPtr__zoom = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "_zoom");
		NativeFieldInfoPtr__manualZoomFactor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "_manualZoomFactor");
		NativeFieldInfoPtr__manualZoomStep = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "_manualZoomStep");
		NativeFieldInfoPtr__manualZoomOutCap = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "_manualZoomOutCap");
		NativeFieldInfoPtr__manualZoomInCap = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "_manualZoomInCap");
		NativeFieldInfoPtr__mapRatioX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "_mapRatioX");
		NativeFieldInfoPtr__mapRatioY = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "_mapRatioY");
		NativeFieldInfoPtr__mapSpriteWidth = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "_mapSpriteWidth");
		NativeFieldInfoPtr__mapSpriteHeight = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "_mapSpriteHeight");
		NativeFieldInfoPtr__detailedMapSprite = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "_detailedMapSprite");
		NativeFieldInfoPtr__positionBuckets = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "_positionBuckets");
		NativeFieldInfoPtr_DefaultMapRatio = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "DefaultMapRatio");
		NativeFieldInfoPtr_DefaultMapSize = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "DefaultMapSize");
		NativeFieldInfoPtr_DefaultPhaserZoom = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "DefaultPhaserZoom");
		NativeFieldInfoPtr_BaselineMapScale = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "BaselineMapScale");
		NativeFieldInfoPtr_PhaserToUnityAdjustment = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "PhaserToUnityAdjustment");
		NativeFieldInfoPtr_DefaultPixelsPerTile = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "DefaultPixelsPerTile");
		NativeFieldInfoPtr__minorItemTypes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<MapManager>.NativeClassPtr, "_minorItemTypes");
		NativeMethodInfoPtr_get_AllItemData_Private_get_Dictionary_2_ItemType_ItemData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MapManager>.NativeClassPtr, 100669501);
		NativeMethodInfoPtr_get_AllWeaponData_Private_get_Dictionary_2_WeaponType_List_1_WeaponData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MapManager>.NativeClassPtr, 100669502);
		NativeMethodInfoPtr_Construct_Private_Void_GameSessionData_DataManager_GameManager_PlayerOptions_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MapManager>.NativeClassPtr, 100669503);
		NativeMethodInfoPtr_Awake_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MapManager>.NativeClassPtr, 100669504);
		NativeMethodInfoPtr_OnDisable_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MapManager>.NativeClassPtr, 100669505);
		NativeMethodInfoPtr_OnDestroy_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MapManager>.NativeClassPtr, 100669506);
		NativeMethodInfoPtr_SetPickups_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MapManager>.NativeClassPtr, 100669507);
		NativeMethodInfoPtr_ReduceAlphaOnArcanaInfoShown_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MapManager>.NativeClassPtr, 100669508);
		NativeMethodInfoPtr_ResetToDefaultAlpha_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MapManager>.NativeClassPtr, 100669509);
		NativeMethodInfoPtr_IsMinorItem_Private_Boolean_Pickup_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MapManager>.NativeClassPtr, 100669510);
		NativeMethodInfoPtr_GetAllWorldItems_Private_List_1_Pickup_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MapManager>.NativeClassPtr, 100669511);
		NativeMethodInfoPtr_ZoomIn_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MapManager>.NativeClassPtr, 100669512);
		NativeMethodInfoPtr_ZoomOut_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MapManager>.NativeClassPtr, 100669513);
		NativeMethodInfoPtr_Populate_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MapManager>.NativeClassPtr, 100669514);
		NativeMethodInfoPtr_AddPickupFadingTweens_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MapManager>.NativeClassPtr, 100669515);
		NativeMethodInfoPtr_DrawMapBackground_Private_Void_StageData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MapManager>.NativeClassPtr, 100669516);
		NativeMethodInfoPtr_MakeGrid_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MapManager>.NativeClassPtr, 100669517);
		NativeMethodInfoPtr_MakeGridLine_Private_GameObject_Boolean_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MapManager>.NativeClassPtr, 100669518);
		NativeMethodInfoPtr_AddMinorItems_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MapManager>.NativeClassPtr, 100669519);
		NativeMethodInfoPtr_AddPlayers_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MapManager>.NativeClassPtr, 100669520);
		NativeMethodInfoPtr_ShouldSkipDrawingPickup_Private_Boolean_Pickup_Dictionary_2_int2_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MapManager>.NativeClassPtr, 100669521);
		NativeMethodInfoPtr_AddTreasureChests_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MapManager>.NativeClassPtr, 100669522);
		NativeMethodInfoPtr_AddStagePickups_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MapManager>.NativeClassPtr, 100669523);
		NativeMethodInfoPtr_AddMapTokens_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MapManager>.NativeClassPtr, 100669524);
		NativeMethodInfoPtr_AddEventTargets_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MapManager>.NativeClassPtr, 100669525);
		NativeMethodInfoPtr_SpawnItemOnMap_Private_GameObject_Sprite_Vector2_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MapManager>.NativeClassPtr, 100669526);
		NativeMethodInfoPtr_ClearIcons_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MapManager>.NativeClassPtr, 100669527);
		NativeMethodInfoPtr_ClearGrid_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MapManager>.NativeClassPtr, 100669528);
		NativeMethodInfoPtr_InitMultiMaps_Private_Void_StageData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MapManager>.NativeClassPtr, 100669529);
		NativeMethodInfoPtr_InitDetailedMap_Private_Void_StageData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MapManager>.NativeClassPtr, 100669530);
		NativeMethodInfoPtr_ShowDetailedMap_Private_Void_StageData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MapManager>.NativeClassPtr, 100669531);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MapManager>.NativeClassPtr, 100669532);
		NativeMethodInfoPtr_Method_Private_Void_byref___c__DisplayClass61_0_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MapManager>.NativeClassPtr, 100669533);
		NativeMethodInfoPtr_Method_Private_Void_byref___c__DisplayClass61_0_1 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<MapManager>.NativeClassPtr, 100669534);
	}

	[CallerCount(0)]
	public unsafe void Construct(GameSessionData session, DataManager data, GameManager gameManager, PlayerOptions playerOptions)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)session);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)data);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gameManager);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)playerOptions);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Construct_Private_Void_GameSessionData_DataManager_GameManager_PlayerOptions_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 971855, XrefRangeEnd = 971862, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Awake()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Awake_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 971862, XrefRangeEnd = 971864, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnDisable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDisable_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 0, XrefRangeEnd = 0, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnDestroy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDestroy_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 971878, RefRangeEnd = 971882, XrefRangeStart = 971864, XrefRangeEnd = 971878, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetPickups()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetPickups_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 971882, XrefRangeEnd = 971884, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ReduceAlphaOnArcanaInfoShown()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReduceAlphaOnArcanaInfoShown_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 971884, XrefRangeEnd = 971886, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ResetToDefaultAlpha()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ResetToDefaultAlpha_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 971898, RefRangeEnd = 971899, XrefRangeStart = 971886, XrefRangeEnd = 971898, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsMinorItem(Pickup pickupItem)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)pickupItem);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsMinorItem_Private_Boolean_Pickup_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 971899, XrefRangeEnd = 971903, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe List<Pickup> GetAllWorldItems()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetAllWorldItems_Private_List_1_Pickup_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Pickup>>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 971903, XrefRangeEnd = 971904, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ZoomIn()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ZoomIn_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 971904, XrefRangeEnd = 971905, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ZoomOut()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ZoomOut_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(8)]
	[CachedScanResults(RefRangeStart = 971958, RefRangeEnd = 971966, XrefRangeStart = 971905, XrefRangeEnd = 971958, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Populate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Populate_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 972011, RefRangeEnd = 972012, XrefRangeStart = 971966, XrefRangeEnd = 972011, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AddPickupFadingTweens()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddPickupFadingTweens_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 972043, RefRangeEnd = 972044, XrefRangeStart = 972012, XrefRangeEnd = 972043, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void DrawMapBackground(StageData stageData)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stageData);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DrawMapBackground_Private_Void_StageData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 972108, RefRangeEnd = 972109, XrefRangeStart = 972044, XrefRangeEnd = 972108, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void MakeGrid()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MakeGrid_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 972131, RefRangeEnd = 972133, XrefRangeStart = 972109, XrefRangeEnd = 972131, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe GameObject MakeGridLine(bool vertical, float pos)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&vertical);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &pos;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MakeGridLine_Private_GameObject_Boolean_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<GameObject>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 972170, RefRangeEnd = 972171, XrefRangeStart = 972133, XrefRangeEnd = 972170, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AddMinorItems()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddMinorItems_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 972234, RefRangeEnd = 972235, XrefRangeStart = 972171, XrefRangeEnd = 972234, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AddPlayers()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddPlayers_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 972246, RefRangeEnd = 972248, XrefRangeStart = 972235, XrefRangeEnd = 972246, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool ShouldSkipDrawingPickup(Pickup pickupItem, Dictionary<int2, int> positionBuckets)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)pickupItem);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)positionBuckets);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ShouldSkipDrawingPickup_Private_Boolean_Pickup_Dictionary_2_int2_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 972284, RefRangeEnd = 972285, XrefRangeStart = 972248, XrefRangeEnd = 972284, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AddTreasureChests()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddTreasureChests_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 972362, RefRangeEnd = 972363, XrefRangeStart = 972285, XrefRangeEnd = 972362, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AddStagePickups()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddStagePickups_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 972378, RefRangeEnd = 972379, XrefRangeStart = 972363, XrefRangeEnd = 972378, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AddMapTokens()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddMapTokens_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 972423, RefRangeEnd = 972424, XrefRangeStart = 972379, XrefRangeEnd = 972423, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AddEventTargets()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AddEventTargets_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(6)]
	[CachedScanResults(RefRangeStart = 972440, RefRangeEnd = 972446, XrefRangeStart = 972424, XrefRangeEnd = 972440, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe GameObject SpawnItemOnMap(Sprite s, Vector2 tPos, float scale = 1f)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)s);
		*(Vector2**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &tPos;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &scale;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SpawnItemOnMap_Private_GameObject_Sprite_Vector2_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<GameObject>(intPtr) : null;
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 972464, RefRangeEnd = 972467, XrefRangeStart = 972446, XrefRangeEnd = 972464, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ClearIcons()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClearIcons_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 972484, RefRangeEnd = 972487, XrefRangeStart = 972467, XrefRangeEnd = 972484, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ClearGrid()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClearGrid_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 972487, XrefRangeEnd = 972529, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void InitMultiMaps(StageData stageData)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stageData);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InitMultiMaps_Private_Void_StageData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 972561, RefRangeEnd = 972562, XrefRangeStart = 972529, XrefRangeEnd = 972561, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void InitDetailedMap(StageData stageData)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stageData);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_InitDetailedMap_Private_Void_StageData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 972682, RefRangeEnd = 972683, XrefRangeStart = 972562, XrefRangeEnd = 972682, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ShowDetailedMap(StageData stageData)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stageData);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ShowDetailedMap_Private_Void_StageData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 972683, XrefRangeEnd = 972712, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe MapManager()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<MapManager>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 972721, RefRangeEnd = 972722, XrefRangeStart = 972712, XrefRangeEnd = 972721, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Method_Private_Void_byref___c__DisplayClass61_0_0(ref __c__DisplayClass61_0 P_0)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)P_0));
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Method_Private_Void_byref___c__DisplayClass61_0_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 972725, RefRangeEnd = 972726, XrefRangeStart = 972722, XrefRangeEnd = 972725, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Method_Private_Void_byref___c__DisplayClass61_0_1(ref __c__DisplayClass61_0 P_0)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)P_0));
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Method_Private_Void_byref___c__DisplayClass61_0_1, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public MapManager(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
