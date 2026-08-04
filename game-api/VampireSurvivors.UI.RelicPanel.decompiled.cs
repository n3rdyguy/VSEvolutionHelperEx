using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem.Collections.Generic;
using UnityEngine;
using VampireSurvivors.Data;
using VampireSurvivors.Data.Stage;
using VampireSurvivors.Objects;

namespace VampireSurvivors.UI;

public class RelicPanel : MonoBehaviour
{
	private static readonly IntPtr NativeFieldInfoPtr__Prefab;

	private static readonly IntPtr NativeFieldInfoPtr__Container;

	private static readonly IntPtr NativeFieldInfoPtr__spawned;

	private static readonly IntPtr NativeFieldInfoPtr__spawnedType;

	private static readonly IntPtr NativeFieldInfoPtr__data;

	private static readonly IntPtr NativeFieldInfoPtr__playerOptions;

	private static readonly IntPtr NativeFieldInfoPtr__hasYellowRelic;

	private static readonly IntPtr NativeMethodInfoPtr_Construct_Private_Void_DataManager_PlayerOptions_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetRelics_Public_Void_StageData_StageType_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe GameObject _Prefab
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Prefab);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<GameObject>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Prefab)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gameObject));
		}
	}

	public unsafe RectTransform _Container
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Container);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<RectTransform>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Container)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rectTransform));
		}
	}

	public unsafe List<GameObject> _spawned
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__spawned);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<GameObject>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__spawned)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe List<ItemType> _spawnedType
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__spawnedType);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<List<ItemType>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__spawnedType)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe DataManager _data
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__data);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<DataManager>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__data)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)dataManager));
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

	public unsafe bool _hasYellowRelic
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hasYellowRelic);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hasYellowRelic)) = flag;
		}
	}

	static RelicPanel()
	{
		Il2CppClassPointerStore<RelicPanel>.NativeClassPtr = IL2CPP.GetIl2CppClass("VampireSurvivors.Runtime.dll", "VampireSurvivors.UI", "RelicPanel");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<RelicPanel>.NativeClassPtr);
		NativeFieldInfoPtr__Prefab = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RelicPanel>.NativeClassPtr, "_Prefab");
		NativeFieldInfoPtr__Container = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RelicPanel>.NativeClassPtr, "_Container");
		NativeFieldInfoPtr__spawned = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RelicPanel>.NativeClassPtr, "_spawned");
		NativeFieldInfoPtr__spawnedType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RelicPanel>.NativeClassPtr, "_spawnedType");
		NativeFieldInfoPtr__data = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RelicPanel>.NativeClassPtr, "_data");
		NativeFieldInfoPtr__playerOptions = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RelicPanel>.NativeClassPtr, "_playerOptions");
		NativeFieldInfoPtr__hasYellowRelic = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<RelicPanel>.NativeClassPtr, "_hasYellowRelic");
		NativeMethodInfoPtr_Construct_Private_Void_DataManager_PlayerOptions_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RelicPanel>.NativeClassPtr, 100670326);
		NativeMethodInfoPtr_SetRelics_Public_Void_StageData_StageType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RelicPanel>.NativeClassPtr, 100670327);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<RelicPanel>.NativeClassPtr, 100670328);
	}

	[CallerCount(0)]
	public unsafe void Construct(DataManager data, PlayerOptions player)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)data);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)player);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Construct_Private_Void_DataManager_PlayerOptions_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 986471, RefRangeEnd = 986472, XrefRangeStart = 986330, XrefRangeEnd = 986471, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetRelics(StageData stage, StageType stageType)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stage);
		*(StageType**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &stageType;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetRelics_Public_Void_StageData_StageType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 986472, XrefRangeEnd = 986487, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe RelicPanel()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<RelicPanel>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public RelicPanel(IntPtr pointer)
		: base(pointer)
	{
	}
}
