using System;
using System.Runtime.CompilerServices;
using DG.Tweening;
using I2.Loc;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VampireSurvivors.Data;
using VampireSurvivors.Data.Stage;
using VampireSurvivors.Objects;

namespace VampireSurvivors.UI;

public class StageItemUI : SelectableUI
{
	private static readonly IntPtr NativeFieldInfoPtr__Background;

	private static readonly IntPtr NativeFieldInfoPtr__Icon;

	private static readonly IntPtr NativeFieldInfoPtr__NameText;

	private static readonly IntPtr NativeFieldInfoPtr__DescriptionText;

	private static readonly IntPtr NativeFieldInfoPtr__StageNumber;

	private static readonly IntPtr NativeFieldInfoPtr__Overlay;

	private static readonly IntPtr NativeFieldInfoPtr__FrameCorners;

	private static readonly IntPtr NativeFieldInfoPtr__Exclamation;

	private static readonly IntPtr NativeFieldInfoPtr__BatSignal;

	private static readonly IntPtr NativeFieldInfoPtr__unlocked;

	private static readonly IntPtr NativeFieldInfoPtr__specialTween;

	private static readonly IntPtr NativeFieldInfoPtr__page;

	private static readonly IntPtr NativeFieldInfoPtr__stage;

	private static readonly IntPtr NativeFieldInfoPtr__playerOptions;

	private static readonly IntPtr NativeFieldInfoPtr__frameNames;

	private static readonly IntPtr NativeFieldInfoPtr__Type_k__BackingField;

	private static readonly IntPtr NativeMethodInfoPtr_get_Type_Public_get_StageType_0;

	private static readonly IntPtr NativeMethodInfoPtr_set_Type_Public_set_Void_StageType_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_DescriptionText_Public_get_TextMeshProUGUI_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnDestroy_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_DoHighlight_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_DoUnhighlight_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetData_Public_Void_PlayerOptions_StageSelectPage_StageData_Sprite_StageType_Int32_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetInfoPanel_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetData_Public_StageData_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetStageType_Public_StageType_0;

	private static readonly IntPtr NativeMethodInfoPtr_HasHyperUnlocked_Public_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_MakeDisabled_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_MakeEnabled_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnSelected_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr__SetData_b__24_0_Private_Void_0;

	public unsafe Image _Background
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Background);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Image>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Background)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)image));
		}
	}

	public unsafe Image _Icon
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Icon);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Image>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Icon)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)image));
		}
	}

	public unsafe Localize _NameText
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__NameText);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Localize>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__NameText)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)localize));
		}
	}

	public unsafe Localize _DescriptionText
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__DescriptionText);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Localize>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__DescriptionText)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)localize));
		}
	}

	public unsafe Text _StageNumber
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__StageNumber);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Text>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__StageNumber)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)text));
		}
	}

	public unsafe Image _Overlay
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Overlay);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Image>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Overlay)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)image));
		}
	}

	public unsafe Image _FrameCorners
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__FrameCorners);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Image>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__FrameCorners)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)image));
		}
	}

	public unsafe GameObject _Exclamation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Exclamation);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<GameObject>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Exclamation)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gameObject));
		}
	}

	public unsafe Image _BatSignal
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__BatSignal);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Image>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__BatSignal)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)image));
		}
	}

	public unsafe bool _unlocked
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__unlocked);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__unlocked)) = flag;
		}
	}

	public unsafe Sequence _specialTween
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__specialTween);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Sequence>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__specialTween)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)sequence));
		}
	}

	public unsafe StageSelectPage _page
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__page);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<StageSelectPage>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__page)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stageSelectPage));
		}
	}

	public unsafe StageData _stage
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__stage);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<StageData>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__stage)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stageData));
		}
	}

	public unsafe PlayerOptions _playerOptions
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__playerOptions);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<PlayerOptions>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__playerOptions)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)playerOptions));
		}
	}

	public unsafe Il2CppStringArray _frameNames
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__frameNames);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStringArray>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__frameNames)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe StageType _Type_k__BackingField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Type_k__BackingField);
			return *(StageType*)num;
		}
		set
		{
			*(StageType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Type_k__BackingField)) = stageType;
		}
	}

	public unsafe StageType Type
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Type_Public_get_StageType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(StageType*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
		[CallerCount(0)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = stackalloc IntPtr[1];
			*ptr = (nint)(&value);
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Type_Public_set_Void_StageType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe TextMeshProUGUI DescriptionText
	{
		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 994728, XrefRangeEnd = 994748, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_DescriptionText_Public_get_TextMeshProUGUI_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<TextMeshProUGUI>(intPtr) : null;
		}
	}

	static StageItemUI()
	{
		Il2CppClassPointerStore<StageItemUI>.NativeClassPtr = IL2CPP.GetIl2CppClass("VampireSurvivors.Runtime.dll", "VampireSurvivors.UI", "StageItemUI");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<StageItemUI>.NativeClassPtr);
		NativeFieldInfoPtr__Background = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageItemUI>.NativeClassPtr, "_Background");
		NativeFieldInfoPtr__Icon = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageItemUI>.NativeClassPtr, "_Icon");
		NativeFieldInfoPtr__NameText = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageItemUI>.NativeClassPtr, "_NameText");
		NativeFieldInfoPtr__DescriptionText = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageItemUI>.NativeClassPtr, "_DescriptionText");
		NativeFieldInfoPtr__StageNumber = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageItemUI>.NativeClassPtr, "_StageNumber");
		NativeFieldInfoPtr__Overlay = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageItemUI>.NativeClassPtr, "_Overlay");
		NativeFieldInfoPtr__FrameCorners = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageItemUI>.NativeClassPtr, "_FrameCorners");
		NativeFieldInfoPtr__Exclamation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageItemUI>.NativeClassPtr, "_Exclamation");
		NativeFieldInfoPtr__BatSignal = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageItemUI>.NativeClassPtr, "_BatSignal");
		NativeFieldInfoPtr__unlocked = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageItemUI>.NativeClassPtr, "_unlocked");
		NativeFieldInfoPtr__specialTween = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageItemUI>.NativeClassPtr, "_specialTween");
		NativeFieldInfoPtr__page = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageItemUI>.NativeClassPtr, "_page");
		NativeFieldInfoPtr__stage = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageItemUI>.NativeClassPtr, "_stage");
		NativeFieldInfoPtr__playerOptions = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageItemUI>.NativeClassPtr, "_playerOptions");
		NativeFieldInfoPtr__frameNames = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageItemUI>.NativeClassPtr, "_frameNames");
		NativeFieldInfoPtr__Type_k__BackingField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageItemUI>.NativeClassPtr, "<Type>k__BackingField");
		NativeMethodInfoPtr_get_Type_Public_get_StageType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageItemUI>.NativeClassPtr, 100670837);
		NativeMethodInfoPtr_set_Type_Public_set_Void_StageType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageItemUI>.NativeClassPtr, 100670838);
		NativeMethodInfoPtr_get_DescriptionText_Public_get_TextMeshProUGUI_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageItemUI>.NativeClassPtr, 100670839);
		NativeMethodInfoPtr_OnDestroy_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageItemUI>.NativeClassPtr, 100670840);
		NativeMethodInfoPtr_DoHighlight_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageItemUI>.NativeClassPtr, 100670841);
		NativeMethodInfoPtr_DoUnhighlight_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageItemUI>.NativeClassPtr, 100670842);
		NativeMethodInfoPtr_SetData_Public_Void_PlayerOptions_StageSelectPage_StageData_Sprite_StageType_Int32_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageItemUI>.NativeClassPtr, 100670843);
		NativeMethodInfoPtr_SetInfoPanel_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageItemUI>.NativeClassPtr, 100670844);
		NativeMethodInfoPtr_GetData_Public_StageData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageItemUI>.NativeClassPtr, 100670845);
		NativeMethodInfoPtr_GetStageType_Public_StageType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageItemUI>.NativeClassPtr, 100670846);
		NativeMethodInfoPtr_HasHyperUnlocked_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageItemUI>.NativeClassPtr, 100670847);
		NativeMethodInfoPtr_MakeDisabled_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageItemUI>.NativeClassPtr, 100670848);
		NativeMethodInfoPtr_MakeEnabled_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageItemUI>.NativeClassPtr, 100670849);
		NativeMethodInfoPtr_OnSelected_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageItemUI>.NativeClassPtr, 100670850);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageItemUI>.NativeClassPtr, 100670851);
		NativeMethodInfoPtr__SetData_b__24_0_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageItemUI>.NativeClassPtr, 100670852);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 994748, XrefRangeEnd = 994759, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnDestroy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnDestroy_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 994759, XrefRangeEnd = 994761, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void DoHighlight()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DoHighlight_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 994761, XrefRangeEnd = 994763, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void DoUnhighlight()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DoUnhighlight_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 994938, RefRangeEnd = 994940, XrefRangeStart = 994763, XrefRangeEnd = 994938, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetData(PlayerOptions player, StageSelectPage page, StageData stage, Sprite mapSprite, StageType stageType, int index, bool hideDescriptionText)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[7];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)player);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)page);
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stage);
		*(IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)mapSprite);
		*(StageType**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = &stageType;
		*(int**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(IntPtr)))) = &index;
		*(bool**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(IntPtr)))) = &hideDescriptionText;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetData_Public_Void_PlayerOptions_StageSelectPage_StageData_Sprite_StageType_Int32_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 994961, RefRangeEnd = 994962, XrefRangeStart = 994940, XrefRangeEnd = 994961, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetInfoPanel()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetInfoPanel_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe StageData GetData()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetData_Public_StageData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<StageData>(intPtr) : null;
	}

	[CallerCount(0)]
	public unsafe StageType GetStageType()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetStageType_Public_StageType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(StageType*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe bool HasHyperUnlocked()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HasHyperUnlocked_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 994967, RefRangeEnd = 994968, XrefRangeStart = 994962, XrefRangeEnd = 994967, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void MakeDisabled()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MakeDisabled_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 994976, RefRangeEnd = 994978, XrefRangeStart = 994968, XrefRangeEnd = 994976, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void MakeEnabled()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MakeEnabled_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 994978, XrefRangeEnd = 994979, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnSelected()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnSelected_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 994979, XrefRangeEnd = 994997, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe StageItemUI()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<StageItemUI>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 994997, XrefRangeEnd = 995014, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void _SetData_b__24_0()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__SetData_b__24_0_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public StageItemUI(IntPtr pointer)
		: base(pointer)
	{
	}
}
