using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using UnityEngine.UI;
using VampireSurvivors.Data;
using VampireSurvivors.Data.Items;
using VampireSurvivors.Data.Weapons;

namespace VampireSurvivors.UI;

public class CollectionItemUI : SelectableUI
{
	public enum CollectionTypes
	{
		WEAPON,
		ITEM,
		ARCANA
	}

	private static readonly IntPtr NativeFieldInfoPtr_LockedIcon;

	private static readonly IntPtr NativeFieldInfoPtr_UnlockedIcon;

	private static readonly IntPtr NativeFieldInfoPtr_Frame;

	private static readonly IntPtr NativeFieldInfoPtr__SealIcon;

	private static readonly IntPtr NativeFieldInfoPtr__weaponData;

	private static readonly IntPtr NativeFieldInfoPtr__weaponType;

	private static readonly IntPtr NativeFieldInfoPtr__page;

	private static readonly IntPtr NativeFieldInfoPtr__itemData;

	private static readonly IntPtr NativeFieldInfoPtr__itemType;

	private static readonly IntPtr NativeFieldInfoPtr__arcanaData;

	private static readonly IntPtr NativeFieldInfoPtr__arcanaType;

	private static readonly IntPtr NativeFieldInfoPtr__button;

	private static readonly IntPtr NativeFieldInfoPtr__seen;

	private static readonly IntPtr NativeFieldInfoPtr_CollectionType;

	private static readonly IntPtr NativeMethodInfoPtr_SetData_Public_Void_WeaponData_CollectionsPage_WeaponType_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetItemData_Public_ItemData_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetIconSizes_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetItem_Public_Void_ItemData_CollectionsPage_ItemType_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetArcana_Public_Void_ArcanaData_CollectionsPage_ArcanaType_0;

	private static readonly IntPtr NativeMethodInfoPtr_Seal_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_UnSeal_Public_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_IsWeapon_Public_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_IsPassive_Public_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_IsItem_Public_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_IsRelic_Public_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_IsArcana_Public_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_IsDefaultContent_Public_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_IsExtra_Public_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetLocked_Private_Void_Boolean_0;

	private static readonly IntPtr NativeMethodInfoPtr_OnSelected_Protected_Virtual_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetupClickRegister_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_RegisterClick_Private_Void_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetWeaponType_Public_WeaponType_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetWeaponData_Public_WeaponData_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetItemType_Public_ItemType_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetContentGroup_Public_ContentGroupType_0;

	private static readonly IntPtr NativeMethodInfoPtr_GetArcanaType_Public_ArcanaType_0;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe Image LockedIcon
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LockedIcon);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Image>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_LockedIcon)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)image));
		}
	}

	public unsafe Image UnlockedIcon
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UnlockedIcon);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Image>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_UnlockedIcon)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)image));
		}
	}

	public unsafe Image Frame
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Frame);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Image>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_Frame)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)image));
		}
	}

	public unsafe Image _SealIcon
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__SealIcon);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Image>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__SealIcon)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)image));
		}
	}

	public unsafe WeaponData _weaponData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__weaponData);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<WeaponData>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__weaponData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)weaponData));
		}
	}

	public unsafe WeaponType _weaponType
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__weaponType);
			return *(WeaponType*)num;
		}
		set
		{
			*(WeaponType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__weaponType)) = weaponType;
		}
	}

	public unsafe CollectionsPage _page
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__page);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<CollectionsPage>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__page)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)collectionsPage));
		}
	}

	public unsafe ItemData _itemData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__itemData);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ItemData>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__itemData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)itemData));
		}
	}

	public unsafe ItemType _itemType
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__itemType);
			return *(ItemType*)num;
		}
		set
		{
			*(ItemType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__itemType)) = itemType;
		}
	}

	public unsafe ArcanaData _arcanaData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__arcanaData);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ArcanaData>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__arcanaData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)arcanaData));
		}
	}

	public unsafe ArcanaType _arcanaType
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__arcanaType);
			return *(ArcanaType*)num;
		}
		set
		{
			*(ArcanaType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__arcanaType)) = arcanaType;
		}
	}

	public unsafe Button _button
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__button);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Button>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__button)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)button));
		}
	}

	public unsafe bool _seen
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__seen);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__seen)) = flag;
		}
	}

	public unsafe CollectionTypes CollectionType
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CollectionType);
			return *(CollectionTypes*)num;
		}
		set
		{
			*(CollectionTypes*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_CollectionType)) = collectionTypes;
		}
	}

	static CollectionItemUI()
	{
		Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr = IL2CPP.GetIl2CppClass("VampireSurvivors.Runtime.dll", "VampireSurvivors.UI", "CollectionItemUI");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr);
		NativeFieldInfoPtr_LockedIcon = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, "LockedIcon");
		NativeFieldInfoPtr_UnlockedIcon = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, "UnlockedIcon");
		NativeFieldInfoPtr_Frame = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, "Frame");
		NativeFieldInfoPtr__SealIcon = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, "_SealIcon");
		NativeFieldInfoPtr__weaponData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, "_weaponData");
		NativeFieldInfoPtr__weaponType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, "_weaponType");
		NativeFieldInfoPtr__page = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, "_page");
		NativeFieldInfoPtr__itemData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, "_itemData");
		NativeFieldInfoPtr__itemType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, "_itemType");
		NativeFieldInfoPtr__arcanaData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, "_arcanaData");
		NativeFieldInfoPtr__arcanaType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, "_arcanaType");
		NativeFieldInfoPtr__button = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, "_button");
		NativeFieldInfoPtr__seen = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, "_seen");
		NativeFieldInfoPtr_CollectionType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, "CollectionType");
		NativeMethodInfoPtr_SetData_Public_Void_WeaponData_CollectionsPage_WeaponType_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, 100668565);
		NativeMethodInfoPtr_GetItemData_Public_ItemData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, 100668566);
		NativeMethodInfoPtr_SetIconSizes_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, 100668567);
		NativeMethodInfoPtr_SetItem_Public_Void_ItemData_CollectionsPage_ItemType_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, 100668568);
		NativeMethodInfoPtr_SetArcana_Public_Void_ArcanaData_CollectionsPage_ArcanaType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, 100668569);
		NativeMethodInfoPtr_Seal_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, 100668570);
		NativeMethodInfoPtr_UnSeal_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, 100668571);
		NativeMethodInfoPtr_IsWeapon_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, 100668572);
		NativeMethodInfoPtr_IsPassive_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, 100668573);
		NativeMethodInfoPtr_IsItem_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, 100668574);
		NativeMethodInfoPtr_IsRelic_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, 100668575);
		NativeMethodInfoPtr_IsArcana_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, 100668576);
		NativeMethodInfoPtr_IsDefaultContent_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, 100668577);
		NativeMethodInfoPtr_IsExtra_Public_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, 100668578);
		NativeMethodInfoPtr_SetLocked_Private_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, 100668579);
		NativeMethodInfoPtr_OnSelected_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, 100668580);
		NativeMethodInfoPtr_SetupClickRegister_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, 100668581);
		NativeMethodInfoPtr_RegisterClick_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, 100668582);
		NativeMethodInfoPtr_GetWeaponType_Public_WeaponType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, 100668583);
		NativeMethodInfoPtr_GetWeaponData_Public_WeaponData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, 100668584);
		NativeMethodInfoPtr_GetItemType_Public_ItemType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, 100668585);
		NativeMethodInfoPtr_GetContentGroup_Public_ContentGroupType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, 100668586);
		NativeMethodInfoPtr_GetArcanaType_Public_ArcanaType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, 100668587);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr, 100668588);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 955673, RefRangeEnd = 955674, XrefRangeStart = 955646, XrefRangeEnd = 955673, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetData(WeaponData w, CollectionsPage page, WeaponType _wType, bool isSealed)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)w);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)page);
		*(WeaponType**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &_wType;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &isSealed;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetData_Public_Void_WeaponData_CollectionsPage_WeaponType_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 886002, RefRangeEnd = 886005, XrefRangeStart = 886002, XrefRangeEnd = 886005, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe ItemData GetItemData()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetItemData_Public_ItemData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<ItemData>(intPtr) : null;
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 955721, RefRangeEnd = 955724, XrefRangeStart = 955674, XrefRangeEnd = 955721, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetIconSizes()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetIconSizes_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 955757, RefRangeEnd = 955758, XrefRangeStart = 955724, XrefRangeEnd = 955757, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetItem(ItemData w, CollectionsPage page, ItemType _item, bool isSealed)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[4];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)w);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)page);
		*(ItemType**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &_item;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &isSealed;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetItem_Public_Void_ItemData_CollectionsPage_ItemType_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 955786, RefRangeEnd = 955787, XrefRangeStart = 955758, XrefRangeEnd = 955786, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetArcana(ArcanaData w, CollectionsPage page, ArcanaType type)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)w);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)page);
		*(ArcanaType**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &type;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetArcana_Public_Void_ArcanaData_CollectionsPage_ArcanaType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 955790, RefRangeEnd = 955797, XrefRangeStart = 955787, XrefRangeEnd = 955790, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Seal()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Seal_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 955800, RefRangeEnd = 955807, XrefRangeStart = 955797, XrefRangeEnd = 955800, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void UnSeal()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_UnSeal_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe bool IsWeapon()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsWeapon_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe bool IsPassive()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsPassive_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 955807, XrefRangeEnd = 955808, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsItem()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsItem_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(7)]
	[CachedScanResults(RefRangeStart = 955814, RefRangeEnd = 955821, XrefRangeStart = 955808, XrefRangeEnd = 955814, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe bool IsRelic()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsRelic_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe bool IsArcana()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsArcana_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe bool IsDefaultContent()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsDefaultContent_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe bool IsExtra()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_IsExtra_Public_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 955840, RefRangeEnd = 955843, XrefRangeStart = 955821, XrefRangeEnd = 955840, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetLocked(bool isUnlocked)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&isUnlocked);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetLocked_Private_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 955843, XrefRangeEnd = 955844, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnSelected()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnSelected_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 955855, RefRangeEnd = 955858, XrefRangeStart = 955844, XrefRangeEnd = 955855, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetupClickRegister()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetupClickRegister_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 955858, XrefRangeEnd = 955863, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RegisterClick()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RegisterClick_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe WeaponType GetWeaponType()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetWeaponType_Public_WeaponType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(WeaponType*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe WeaponData GetWeaponData()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetWeaponData_Public_WeaponData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<WeaponData>(intPtr) : null;
	}

	[CallerCount(0)]
	public unsafe ItemType GetItemType()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetItemType_Public_ItemType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(ItemType*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe ContentGroupType GetContentGroup()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetContentGroup_Public_ContentGroupType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(ContentGroupType*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	public unsafe ArcanaType GetArcanaType()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetArcanaType_Public_ArcanaType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(ArcanaType*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 955863, XrefRangeEnd = 955866, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe CollectionItemUI()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CollectionItemUI>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public CollectionItemUI(IntPtr pointer)
		: base(pointer)
	{
	}
}
