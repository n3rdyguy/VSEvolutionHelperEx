using System;
using System.Runtime.CompilerServices;
using I2.Loc;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections;
using Il2CppSystem.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VampireSurvivors.App.Scripts.Framework.Adventures;
using VampireSurvivors.App.Scripts.UI;
using VampireSurvivors.Data;
using VampireSurvivors.Data.Stage;
using VampireSurvivors.Framework.Cheats;
using VampireSurvivors.Objects;
using Zenject;

namespace VampireSurvivors.UI;

public class StageSelectPage : BaseUIPage
{
	public enum SelectionPhase
	{
		PHASE1,
		PHASE2
	}

	[System.Serializable]
	[ObfuscatedName("VampireSurvivors.UI.StageSelectPage+<>c")]
	public sealed class __c : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr___9;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__63_0;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__63_1;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__63_2;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__63_3;

		private static readonly System.IntPtr NativeFieldInfoPtr___9__84_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__AutoSizeStageDescriptions_b__63_0_Internal_StageItemUI_GameObject_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__AutoSizeStageDescriptions_b__63_1_Internal_Boolean_StageItemUI_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__AutoSizeStageDescriptions_b__63_2_Internal_TextMeshProUGUI_StageItemUI_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__AutoSizeStageDescriptions_b__63_3_Internal_Boolean_TextMeshProUGUI_0;

		private static readonly System.IntPtr NativeMethodInfoPtr__Populate_b__84_0_Internal_Int32_StageItemUI_0;

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

		public unsafe static Il2CppSystem.Func<GameObject, StageItemUI> __9__63_0
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__63_0, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<GameObject, StageItemUI>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__63_0, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<StageItemUI, bool> __9__63_1
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__63_1, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<StageItemUI, bool>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__63_1, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<StageItemUI, TextMeshProUGUI> __9__63_2
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__63_2, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<StageItemUI, TextMeshProUGUI>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__63_2, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<TextMeshProUGUI, bool> __9__63_3
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__63_3, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<TextMeshProUGUI, bool>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__63_3, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		public unsafe static Il2CppSystem.Func<StageItemUI, int> __9__84_0
		{
			get
			{
				Unsafe.SkipInit(out System.IntPtr intPtr);
				IL2CPP.il2cpp_field_static_get_value(NativeFieldInfoPtr___9__84_0, (void*)(&intPtr));
				System.IntPtr intPtr2 = intPtr;
				return (intPtr2 != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Func<StageItemUI, int>>(intPtr2) : null;
			}
			set
			{
				IL2CPP.il2cpp_field_static_set_value(NativeFieldInfoPtr___9__84_0, (void*)IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)func));
			}
		}

		static __c()
		{
			Il2CppClassPointerStore<__c>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "<>c");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<__c>.NativeClassPtr);
			NativeFieldInfoPtr___9 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9");
			NativeFieldInfoPtr___9__63_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__63_0");
			NativeFieldInfoPtr___9__63_1 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__63_1");
			NativeFieldInfoPtr___9__63_2 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__63_2");
			NativeFieldInfoPtr___9__63_3 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__63_3");
			NativeFieldInfoPtr___9__84_0 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<__c>.NativeClassPtr, "<>9__84_0");
			NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100670897);
			NativeMethodInfoPtr__AutoSizeStageDescriptions_b__63_0_Internal_StageItemUI_GameObject_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100670898);
			NativeMethodInfoPtr__AutoSizeStageDescriptions_b__63_1_Internal_Boolean_StageItemUI_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100670899);
			NativeMethodInfoPtr__AutoSizeStageDescriptions_b__63_2_Internal_TextMeshProUGUI_StageItemUI_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100670900);
			NativeMethodInfoPtr__AutoSizeStageDescriptions_b__63_3_Internal_Boolean_TextMeshProUGUI_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100670901);
			NativeMethodInfoPtr__Populate_b__84_0_Internal_Int32_StageItemUI_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<__c>.NativeClassPtr, 100670902);
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
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 995014, XrefRangeEnd = 995018, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe StageItemUI _AutoSizeStageDescriptions_b__63_0(GameObject go)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)go);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__AutoSizeStageDescriptions_b__63_0_Internal_StageItemUI_GameObject_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<StageItemUI>(intPtr) : null;
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 995018, XrefRangeEnd = 995027, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe bool _AutoSizeStageDescriptions_b__63_1(StageItemUI comp)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)comp);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__AutoSizeStageDescriptions_b__63_1_Internal_Boolean_StageItemUI_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 995027, XrefRangeEnd = 995047, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe TextMeshProUGUI _AutoSizeStageDescriptions_b__63_2(StageItemUI stageItemUI)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stageItemUI);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__AutoSizeStageDescriptions_b__63_2_Internal_TextMeshProUGUI_StageItemUI_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TextMeshProUGUI>(intPtr) : null;
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 995047, XrefRangeEnd = 995056, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe bool _AutoSizeStageDescriptions_b__63_3(TextMeshProUGUI descText)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)descText);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__AutoSizeStageDescriptions_b__63_3_Internal_Boolean_TextMeshProUGUI_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		public unsafe int _Populate_b__84_0(StageItemUI x)
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)x);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__Populate_b__84_0_Internal_Int32_StageItemUI_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		public __c(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[ObfuscatedName("VampireSurvivors.UI.StageSelectPage+<WaitAndSelect>d__62")]
	public sealed class _WaitAndSelect_d__62 : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr___1__state;

		private static readonly System.IntPtr NativeFieldInfoPtr___2__current;

		private static readonly System.IntPtr NativeFieldInfoPtr___4__this;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_IDisposable_Dispose_Private_Virtual_Final_New_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_MoveNext_Private_Virtual_Final_New_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_Generic_IEnumerator_System_Object__get_Current_Private_Virtual_Final_New_get_Object_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_IEnumerator_Reset_Private_Virtual_Final_New_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_IEnumerator_get_Current_Private_Virtual_Final_New_get_Object_0;

		public unsafe int __1__state
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___1__state);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___1__state)) = num;
			}
		}

		public unsafe Il2CppSystem.Object __2__current
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___2__current);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___2__current)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)obj));
			}
		}

		public unsafe StageSelectPage __4__this
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___4__this);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<StageSelectPage>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___4__this)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stageSelectPage));
			}
		}

		public unsafe virtual Il2CppSystem.Object System.Collections.Generic.IEnumerator<System.Object>.Current
		{
			[CallerCount(4)]
			[CachedScanResults(RefRangeStart = 844, RefRangeEnd = 848, XrefRangeStart = 844, XrefRangeEnd = 848, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_Generic_IEnumerator_System_Object__get_Current_Private_Virtual_Final_New_get_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr) : null;
			}
		}

		public unsafe virtual Il2CppSystem.Object System.Collections.IEnumerator.Current
		{
			[CallerCount(4)]
			[CachedScanResults(RefRangeStart = 844, RefRangeEnd = 848, XrefRangeStart = 844, XrefRangeEnd = 848, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_IEnumerator_get_Current_Private_Virtual_Final_New_get_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr) : null;
			}
		}

		static _WaitAndSelect_d__62()
		{
			Il2CppClassPointerStore<_WaitAndSelect_d__62>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "<WaitAndSelect>d__62");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<_WaitAndSelect_d__62>.NativeClassPtr);
			NativeFieldInfoPtr___1__state = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_WaitAndSelect_d__62>.NativeClassPtr, "<>1__state");
			NativeFieldInfoPtr___2__current = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_WaitAndSelect_d__62>.NativeClassPtr, "<>2__current");
			NativeFieldInfoPtr___4__this = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_WaitAndSelect_d__62>.NativeClassPtr, "<>4__this");
			NativeMethodInfoPtr__ctor_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_WaitAndSelect_d__62>.NativeClassPtr, 100670903);
			NativeMethodInfoPtr_System_IDisposable_Dispose_Private_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_WaitAndSelect_d__62>.NativeClassPtr, 100670904);
			NativeMethodInfoPtr_MoveNext_Private_Virtual_Final_New_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_WaitAndSelect_d__62>.NativeClassPtr, 100670905);
			NativeMethodInfoPtr_System_Collections_Generic_IEnumerator_System_Object__get_Current_Private_Virtual_Final_New_get_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_WaitAndSelect_d__62>.NativeClassPtr, 100670906);
			NativeMethodInfoPtr_System_Collections_IEnumerator_Reset_Private_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_WaitAndSelect_d__62>.NativeClassPtr, 100670907);
			NativeMethodInfoPtr_System_Collections_IEnumerator_get_Current_Private_Virtual_Final_New_get_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_WaitAndSelect_d__62>.NativeClassPtr, 100670908);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 40, RefRangeEnd = 41, XrefRangeStart = 40, XrefRangeEnd = 41, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe _WaitAndSelect_d__62(int <>1__state)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<_WaitAndSelect_d__62>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&<>1__state);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(754)]
		[CachedScanResults(RefRangeStart = 41, RefRangeEnd = 795, XrefRangeStart = 41, XrefRangeEnd = 795, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void System_IDisposable_Dispose()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_IDisposable_Dispose_Private_Virtual_Final_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 995056, XrefRangeEnd = 995060, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual bool MoveNext()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MoveNext_Private_Virtual_Final_New_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 995060, XrefRangeEnd = 995065, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void System_Collections_IEnumerator_Reset()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_IEnumerator_Reset_Private_Virtual_Final_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public _WaitAndSelect_d__62(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	[ObfuscatedName("VampireSurvivors.UI.StageSelectPage+<WaitRoutine>d__89")]
	public sealed class _WaitRoutine_d__89 : Il2CppSystem.Object
	{
		private static readonly System.IntPtr NativeFieldInfoPtr___1__state;

		private static readonly System.IntPtr NativeFieldInfoPtr___2__current;

		private static readonly System.IntPtr NativeFieldInfoPtr_cb;

		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_IDisposable_Dispose_Private_Virtual_Final_New_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_MoveNext_Private_Virtual_Final_New_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_Generic_IEnumerator_System_Object__get_Current_Private_Virtual_Final_New_get_Object_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_IEnumerator_Reset_Private_Virtual_Final_New_Void_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_System_Collections_IEnumerator_get_Current_Private_Virtual_Final_New_get_Object_0;

		public unsafe int __1__state
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___1__state);
				return *(int*)num;
			}
			set
			{
				*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___1__state)) = num;
			}
		}

		public unsafe Il2CppSystem.Object __2__current
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___2__current);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___2__current)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)obj));
			}
		}

		public unsafe Il2CppSystem.Action cb
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cb);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Action>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_cb)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)action));
			}
		}

		public unsafe virtual Il2CppSystem.Object System.Collections.Generic.IEnumerator<System.Object>.Current
		{
			[CallerCount(4)]
			[CachedScanResults(RefRangeStart = 844, RefRangeEnd = 848, XrefRangeStart = 844, XrefRangeEnd = 848, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_Generic_IEnumerator_System_Object__get_Current_Private_Virtual_Final_New_get_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr) : null;
			}
		}

		public unsafe virtual Il2CppSystem.Object System.Collections.IEnumerator.Current
		{
			[CallerCount(4)]
			[CachedScanResults(RefRangeStart = 844, RefRangeEnd = 848, XrefRangeStart = 844, XrefRangeEnd = 848, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
			get
			{
				IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				System.IntPtr* ptr = null;
				Unsafe.SkipInit(out System.IntPtr intPtr2);
				System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_IEnumerator_get_Current_Private_Virtual_Final_New_get_Object_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
				Il2CppException.RaiseExceptionIfNecessary(intPtr2);
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppSystem.Object>(intPtr) : null;
			}
		}

		static _WaitRoutine_d__89()
		{
			Il2CppClassPointerStore<_WaitRoutine_d__89>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "<WaitRoutine>d__89");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<_WaitRoutine_d__89>.NativeClassPtr);
			NativeFieldInfoPtr___1__state = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_WaitRoutine_d__89>.NativeClassPtr, "<>1__state");
			NativeFieldInfoPtr___2__current = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_WaitRoutine_d__89>.NativeClassPtr, "<>2__current");
			NativeFieldInfoPtr_cb = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_WaitRoutine_d__89>.NativeClassPtr, "cb");
			NativeMethodInfoPtr__ctor_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_WaitRoutine_d__89>.NativeClassPtr, 100670909);
			NativeMethodInfoPtr_System_IDisposable_Dispose_Private_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_WaitRoutine_d__89>.NativeClassPtr, 100670910);
			NativeMethodInfoPtr_MoveNext_Private_Virtual_Final_New_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_WaitRoutine_d__89>.NativeClassPtr, 100670911);
			NativeMethodInfoPtr_System_Collections_Generic_IEnumerator_System_Object__get_Current_Private_Virtual_Final_New_get_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_WaitRoutine_d__89>.NativeClassPtr, 100670912);
			NativeMethodInfoPtr_System_Collections_IEnumerator_Reset_Private_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_WaitRoutine_d__89>.NativeClassPtr, 100670913);
			NativeMethodInfoPtr_System_Collections_IEnumerator_get_Current_Private_Virtual_Final_New_get_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_WaitRoutine_d__89>.NativeClassPtr, 100670914);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 40, RefRangeEnd = 41, XrefRangeStart = 40, XrefRangeEnd = 41, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe _WaitRoutine_d__89(int <>1__state)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<_WaitRoutine_d__89>.NativeClassPtr))
		{
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = (nint)(&<>1__state);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(754)]
		[CachedScanResults(RefRangeStart = 41, RefRangeEnd = 795, XrefRangeStart = 41, XrefRangeEnd = 795, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void System_IDisposable_Dispose()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_IDisposable_Dispose_Private_Virtual_Final_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		[CallerCount(0)]
		public unsafe virtual bool MoveNext()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_MoveNext_Private_Virtual_Final_New_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}

		[CallerCount(0)]
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 995065, XrefRangeEnd = 995070, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void System_Collections_IEnumerator_Reset()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_IEnumerator_Reset_Private_Virtual_Final_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public _WaitRoutine_d__89(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr_stagePrefab;

	private static readonly System.IntPtr NativeFieldInfoPtr_container;

	private static readonly System.IntPtr NativeFieldInfoPtr__BackgroundPanel;

	private static readonly System.IntPtr NativeFieldInfoPtr_nameText;

	private static readonly System.IntPtr NativeFieldInfoPtr_tips;

	private static readonly System.IntPtr NativeFieldInfoPtr_hyperTips;

	private static readonly System.IntPtr NativeFieldInfoPtr__PageTitle;

	private static readonly System.IntPtr NativeFieldInfoPtr__HyperModeTickBox;

	private static readonly System.IntPtr NativeFieldInfoPtr__HurryModeTickBox;

	private static readonly System.IntPtr NativeFieldInfoPtr__MazzoModeTickBox;

	private static readonly System.IntPtr NativeFieldInfoPtr__LimitBreakTickBox;

	private static readonly System.IntPtr NativeFieldInfoPtr__InverseModeTickBox;

	private static readonly System.IntPtr NativeFieldInfoPtr__EndlessModeTickBox;

	private static readonly System.IntPtr NativeFieldInfoPtr__LockSelectedTickBox;

	private static readonly System.IntPtr NativeFieldInfoPtr__ConfirmButton;

	private static readonly System.IntPtr NativeFieldInfoPtr__SelectButton;

	private static readonly System.IntPtr NativeFieldInfoPtr__Stats;

	private static readonly System.IntPtr NativeFieldInfoPtr__SongPanel;

	private static readonly System.IntPtr NativeFieldInfoPtr__SelectableSongButton;

	private static readonly System.IntPtr NativeFieldInfoPtr__SelectableSpeedButton;

	private static readonly System.IntPtr NativeFieldInfoPtr__AdvancedSettingsButton;

	private static readonly System.IntPtr NativeFieldInfoPtr__RelicPanel;

	private static readonly System.IntPtr NativeFieldInfoPtr__InfoPanel;

	private static readonly System.IntPtr NativeFieldInfoPtr__SliderRect;

	private static readonly System.IntPtr NativeFieldInfoPtr__StageRandomPanel;

	private static readonly System.IntPtr NativeFieldInfoPtr__SharePassivesPanel;

	private static readonly System.IntPtr NativeFieldInfoPtr__SharePassivesBox;

	private static readonly System.IntPtr NativeFieldInfoPtr__DescriptionPanelTextPage;

	private static readonly System.IntPtr NativeFieldInfoPtr__selectionPhase;

	private static readonly System.IntPtr NativeFieldInfoPtr__signalBus;

	private static readonly System.IntPtr NativeFieldInfoPtr__spawned;

	private static readonly System.IntPtr NativeFieldInfoPtr__selectedStage;

	private static readonly System.IntPtr NativeFieldInfoPtr__highlightedStage;

	private static readonly System.IntPtr NativeFieldInfoPtr__selectedData;

	private static readonly System.IntPtr NativeFieldInfoPtr__data;

	private static readonly System.IntPtr NativeFieldInfoPtr__playerOptions;

	private static readonly System.IntPtr NativeFieldInfoPtr__diContainer;

	private static readonly System.IntPtr NativeFieldInfoPtr__cheats;

	private static readonly System.IntPtr NativeFieldInfoPtr__adventureManager;

	private static readonly System.IntPtr NativeFieldInfoPtr__advancedMusicPanel;

	private static readonly System.IntPtr NativeFieldInfoPtr__hurryModeAvailable;

	private static readonly System.IntPtr NativeFieldInfoPtr__hyperModeAvailable;

	private static readonly System.IntPtr NativeFieldInfoPtr__mazzoModeAvailable;

	private static readonly System.IntPtr NativeFieldInfoPtr__limitBreakAvailable;

	private static readonly System.IntPtr NativeFieldInfoPtr__inverseModeAvailable;

	private static readonly System.IntPtr NativeFieldInfoPtr__endlessModeAvailable;

	private static readonly System.IntPtr NativeFieldInfoPtr__hasConfirmed;

	private static readonly System.IntPtr NativeFieldInfoPtr__phase1Disabled;

	private static readonly System.IntPtr NativeFieldInfoPtr__phase2Disabled;

	private static readonly System.IntPtr NativeFieldInfoPtr__availableOptions;

	private static readonly System.IntPtr NativeFieldInfoPtr__hasBanger;

	private static readonly System.IntPtr NativeFieldInfoPtr__hasRandomOptions;

	private static readonly System.IntPtr NativeFieldInfoPtr__hasRandomEvents;

	private static readonly System.IntPtr NativeFieldInfoPtr__hasRandomLevelUps;

	private static readonly System.IntPtr NativeFieldInfoPtr__hasToggles;

	private static readonly System.IntPtr NativeMethodInfoPtr_Construct_Private_Void_SignalBus_DataManager_PlayerOptions_DiContainer_AdventureManager_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Start_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Update_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnDestroy_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_BackPressed_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnShowStart_Protected_Virtual_Void_GameObject_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_WaitAndSelect_Private_IEnumerator_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_AutoSizeStageDescriptions_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnEnterPressed_Protected_Virtual_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnDisable_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnHideStart_Protected_Virtual_Void_GameObject_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OnHideFinish_Protected_Virtual_Void_GameObject_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_OpenAdvancedMusicSelectionPanel_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ReEnable_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_RefreshSongPanel_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ConfirmStage_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SelectStage_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_HighlightStage_Public_Void_StageItemUI_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetInfoPanel_Public_Void_StageItemUI_StageData_StageType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetHyper_Public_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetHurry_Public_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetArcanas_Public_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetLimitBreak_Public_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetInverse_Public_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetEndless_Public_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToggleHyper_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetSharePassives_Public_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Stage6Checks_Private_Static_Dictionary_2_StageType_List_1_StageData_Dictionary_2_StageType_List_1_StageData_PlayerOptions_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Populate_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetAvailableStages_Public_Static_Dictionary_2_StageType_List_1_StageData_DataManager_PlayerOptions_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GenerateNavigation_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_CreateStageItem_Private_GameObject_StageData_StageType_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_WaitAndDo_Private_Void_Action_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_WaitRoutine_Private_Static_IEnumerator_Action_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_EnableInfoPanelNavigation_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetNavigationPhase1_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetNavigationPhase2_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DisableFirstPhaseGroup_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_EnableFirstPhaseGroup_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_DisableSecondPhaseGroup_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_EnableSecondPhaseGroup_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SwitchInput_Public_Void_ActiveInputType_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe GameObject stagePrefab
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_stagePrefab);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<GameObject>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_stagePrefab)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gameObject));
		}
	}

	public unsafe RectTransform container
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_container);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<RectTransform>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_container)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rectTransform));
		}
	}

	public unsafe Image _BackgroundPanel
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__BackgroundPanel);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Image>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__BackgroundPanel)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)image));
		}
	}

	public unsafe Localize nameText
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_nameText);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Localize>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_nameText)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)localize));
		}
	}

	public unsafe Localize tips
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_tips);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Localize>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_tips)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)localize));
		}
	}

	public unsafe Localize hyperTips
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_hyperTips);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Localize>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_hyperTips)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)localize));
		}
	}

	public unsafe TextMeshProUGUI _PageTitle
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__PageTitle);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TextMeshProUGUI>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__PageTitle)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)textMeshProUGUI));
		}
	}

	public unsafe TickBoxUI _HyperModeTickBox
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__HyperModeTickBox);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TickBoxUI>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__HyperModeTickBox)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)tickBoxUI));
		}
	}

	public unsafe TickBoxUI _HurryModeTickBox
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__HurryModeTickBox);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TickBoxUI>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__HurryModeTickBox)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)tickBoxUI));
		}
	}

	public unsafe TickBoxUI _MazzoModeTickBox
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__MazzoModeTickBox);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TickBoxUI>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__MazzoModeTickBox)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)tickBoxUI));
		}
	}

	public unsafe TickBoxUI _LimitBreakTickBox
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__LimitBreakTickBox);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TickBoxUI>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__LimitBreakTickBox)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)tickBoxUI));
		}
	}

	public unsafe TickBoxUI _InverseModeTickBox
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__InverseModeTickBox);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TickBoxUI>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__InverseModeTickBox)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)tickBoxUI));
		}
	}

	public unsafe TickBoxUI _EndlessModeTickBox
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__EndlessModeTickBox);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TickBoxUI>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__EndlessModeTickBox)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)tickBoxUI));
		}
	}

	public unsafe TickBoxUI _LockSelectedTickBox
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__LockSelectedTickBox);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TickBoxUI>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__LockSelectedTickBox)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)tickBoxUI));
		}
	}

	public unsafe Button _ConfirmButton
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__ConfirmButton);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Button>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__ConfirmButton)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)button));
		}
	}

	public unsafe Button _SelectButton
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__SelectButton);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Button>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__SelectButton)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)button));
		}
	}

	public unsafe StageStatsPanel _Stats
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Stats);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<StageStatsPanel>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Stats)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stageStatsPanel));
		}
	}

	public unsafe SongSelectionPanel _SongPanel
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__SongPanel);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SongSelectionPanel>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__SongPanel)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)songSelectionPanel));
		}
	}

	public unsafe Button _SelectableSongButton
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__SelectableSongButton);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Button>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__SelectableSongButton)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)button));
		}
	}

	public unsafe Button _SelectableSpeedButton
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__SelectableSpeedButton);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Button>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__SelectableSpeedButton)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)button));
		}
	}

	public unsafe Button _AdvancedSettingsButton
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AdvancedSettingsButton);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Button>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__AdvancedSettingsButton)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)button));
		}
	}

	public unsafe RelicPanel _RelicPanel
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__RelicPanel);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<RelicPanel>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__RelicPanel)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)relicPanel));
		}
	}

	public unsafe RectTransform _InfoPanel
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__InfoPanel);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<RectTransform>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__InfoPanel)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rectTransform));
		}
	}

	public unsafe RectTransform _SliderRect
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__SliderRect);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<RectTransform>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__SliderRect)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)rectTransform));
		}
	}

	public unsafe StageRandomPanel _StageRandomPanel
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__StageRandomPanel);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<StageRandomPanel>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__StageRandomPanel)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stageRandomPanel));
		}
	}

	public unsafe GameObject _SharePassivesPanel
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__SharePassivesPanel);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<GameObject>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__SharePassivesPanel)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gameObject));
		}
	}

	public unsafe TickBoxUI _SharePassivesBox
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__SharePassivesBox);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<TickBoxUI>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__SharePassivesBox)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)tickBoxUI));
		}
	}

	public unsafe GameObject _DescriptionPanelTextPage
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__DescriptionPanelTextPage);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<GameObject>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__DescriptionPanelTextPage)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gameObject));
		}
	}

	public unsafe SelectionPhase _selectionPhase
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__selectionPhase);
			return *(SelectionPhase*)num;
		}
		set
		{
			*(SelectionPhase*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__selectionPhase)) = selectionPhase;
		}
	}

	public unsafe SignalBus _signalBus
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__signalBus);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SignalBus>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__signalBus)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)signalBus));
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

	public unsafe StageItemUI _selectedStage
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__selectedStage);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<StageItemUI>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__selectedStage)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stageItemUI));
		}
	}

	public unsafe StageItemUI _highlightedStage
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__highlightedStage);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<StageItemUI>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__highlightedStage)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stageItemUI));
		}
	}

	public unsafe StageData _selectedData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__selectedData);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<StageData>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__selectedData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stageData));
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

	public unsafe DiContainer _diContainer
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__diContainer);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<DiContainer>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__diContainer)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)diContainer));
		}
	}

	public unsafe StageSelectionCheatCodeManager _cheats
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__cheats);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<StageSelectionCheatCodeManager>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__cheats)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stageSelectionCheatCodeManager));
		}
	}

	public unsafe AdventureManager _adventureManager
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__adventureManager);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AdventureManager>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__adventureManager)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)adventureManager));
		}
	}

	public unsafe AdvancedMusicSelection _advancedMusicPanel
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__advancedMusicPanel);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<AdvancedMusicSelection>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__advancedMusicPanel)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)advancedMusicSelection));
		}
	}

	public unsafe bool _hurryModeAvailable
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hurryModeAvailable);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hurryModeAvailable)) = flag;
		}
	}

	public unsafe bool _hyperModeAvailable
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hyperModeAvailable);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hyperModeAvailable)) = flag;
		}
	}

	public unsafe bool _mazzoModeAvailable
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mazzoModeAvailable);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__mazzoModeAvailable)) = flag;
		}
	}

	public unsafe bool _limitBreakAvailable
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__limitBreakAvailable);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__limitBreakAvailable)) = flag;
		}
	}

	public unsafe bool _inverseModeAvailable
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__inverseModeAvailable);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__inverseModeAvailable)) = flag;
		}
	}

	public unsafe bool _endlessModeAvailable
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__endlessModeAvailable);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__endlessModeAvailable)) = flag;
		}
	}

	public unsafe bool _hasConfirmed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hasConfirmed);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hasConfirmed)) = flag;
		}
	}

	public unsafe bool _phase1Disabled
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__phase1Disabled);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__phase1Disabled)) = flag;
		}
	}

	public unsafe bool _phase2Disabled
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__phase2Disabled);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__phase2Disabled)) = flag;
		}
	}

	public unsafe List<Selectable> _availableOptions
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__availableOptions);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<List<Selectable>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__availableOptions)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)list));
		}
	}

	public unsafe bool _hasBanger
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hasBanger);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hasBanger)) = flag;
		}
	}

	public unsafe bool _hasRandomOptions
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hasRandomOptions);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hasRandomOptions)) = flag;
		}
	}

	public unsafe bool _hasRandomEvents
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hasRandomEvents);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hasRandomEvents)) = flag;
		}
	}

	public unsafe bool _hasRandomLevelUps
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hasRandomLevelUps);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hasRandomLevelUps)) = flag;
		}
	}

	public unsafe bool _hasToggles
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hasToggles);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hasToggles)) = flag;
		}
	}

	static StageSelectPage()
	{
		Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr = IL2CPP.GetIl2CppClass("VampireSurvivors.Runtime.dll", "VampireSurvivors.UI", "StageSelectPage");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr);
		NativeFieldInfoPtr_stagePrefab = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "stagePrefab");
		NativeFieldInfoPtr_container = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "container");
		NativeFieldInfoPtr__BackgroundPanel = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_BackgroundPanel");
		NativeFieldInfoPtr_nameText = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "nameText");
		NativeFieldInfoPtr_tips = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "tips");
		NativeFieldInfoPtr_hyperTips = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "hyperTips");
		NativeFieldInfoPtr__PageTitle = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_PageTitle");
		NativeFieldInfoPtr__HyperModeTickBox = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_HyperModeTickBox");
		NativeFieldInfoPtr__HurryModeTickBox = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_HurryModeTickBox");
		NativeFieldInfoPtr__MazzoModeTickBox = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_MazzoModeTickBox");
		NativeFieldInfoPtr__LimitBreakTickBox = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_LimitBreakTickBox");
		NativeFieldInfoPtr__InverseModeTickBox = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_InverseModeTickBox");
		NativeFieldInfoPtr__EndlessModeTickBox = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_EndlessModeTickBox");
		NativeFieldInfoPtr__LockSelectedTickBox = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_LockSelectedTickBox");
		NativeFieldInfoPtr__ConfirmButton = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_ConfirmButton");
		NativeFieldInfoPtr__SelectButton = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_SelectButton");
		NativeFieldInfoPtr__Stats = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_Stats");
		NativeFieldInfoPtr__SongPanel = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_SongPanel");
		NativeFieldInfoPtr__SelectableSongButton = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_SelectableSongButton");
		NativeFieldInfoPtr__SelectableSpeedButton = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_SelectableSpeedButton");
		NativeFieldInfoPtr__AdvancedSettingsButton = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_AdvancedSettingsButton");
		NativeFieldInfoPtr__RelicPanel = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_RelicPanel");
		NativeFieldInfoPtr__InfoPanel = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_InfoPanel");
		NativeFieldInfoPtr__SliderRect = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_SliderRect");
		NativeFieldInfoPtr__StageRandomPanel = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_StageRandomPanel");
		NativeFieldInfoPtr__SharePassivesPanel = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_SharePassivesPanel");
		NativeFieldInfoPtr__SharePassivesBox = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_SharePassivesBox");
		NativeFieldInfoPtr__DescriptionPanelTextPage = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_DescriptionPanelTextPage");
		NativeFieldInfoPtr__selectionPhase = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_selectionPhase");
		NativeFieldInfoPtr__signalBus = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_signalBus");
		NativeFieldInfoPtr__spawned = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_spawned");
		NativeFieldInfoPtr__selectedStage = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_selectedStage");
		NativeFieldInfoPtr__highlightedStage = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_highlightedStage");
		NativeFieldInfoPtr__selectedData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_selectedData");
		NativeFieldInfoPtr__data = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_data");
		NativeFieldInfoPtr__playerOptions = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_playerOptions");
		NativeFieldInfoPtr__diContainer = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_diContainer");
		NativeFieldInfoPtr__cheats = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_cheats");
		NativeFieldInfoPtr__adventureManager = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_adventureManager");
		NativeFieldInfoPtr__advancedMusicPanel = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_advancedMusicPanel");
		NativeFieldInfoPtr__hurryModeAvailable = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_hurryModeAvailable");
		NativeFieldInfoPtr__hyperModeAvailable = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_hyperModeAvailable");
		NativeFieldInfoPtr__mazzoModeAvailable = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_mazzoModeAvailable");
		NativeFieldInfoPtr__limitBreakAvailable = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_limitBreakAvailable");
		NativeFieldInfoPtr__inverseModeAvailable = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_inverseModeAvailable");
		NativeFieldInfoPtr__endlessModeAvailable = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_endlessModeAvailable");
		NativeFieldInfoPtr__hasConfirmed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_hasConfirmed");
		NativeFieldInfoPtr__phase1Disabled = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_phase1Disabled");
		NativeFieldInfoPtr__phase2Disabled = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_phase2Disabled");
		NativeFieldInfoPtr__availableOptions = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_availableOptions");
		NativeFieldInfoPtr__hasBanger = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_hasBanger");
		NativeFieldInfoPtr__hasRandomOptions = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_hasRandomOptions");
		NativeFieldInfoPtr__hasRandomEvents = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_hasRandomEvents");
		NativeFieldInfoPtr__hasRandomLevelUps = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_hasRandomLevelUps");
		NativeFieldInfoPtr__hasToggles = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, "_hasToggles");
		NativeMethodInfoPtr_Construct_Private_Void_SignalBus_DataManager_PlayerOptions_DiContainer_AdventureManager_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670853);
		NativeMethodInfoPtr_Start_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670854);
		NativeMethodInfoPtr_Update_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670855);
		NativeMethodInfoPtr_OnDestroy_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670856);
		NativeMethodInfoPtr_BackPressed_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670857);
		NativeMethodInfoPtr_OnShowStart_Protected_Virtual_Void_GameObject_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670858);
		NativeMethodInfoPtr_WaitAndSelect_Private_IEnumerator_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670859);
		NativeMethodInfoPtr_AutoSizeStageDescriptions_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670860);
		NativeMethodInfoPtr_OnEnterPressed_Protected_Virtual_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670861);
		NativeMethodInfoPtr_OnDisable_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670862);
		NativeMethodInfoPtr_OnHideStart_Protected_Virtual_Void_GameObject_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670863);
		NativeMethodInfoPtr_OnHideFinish_Protected_Virtual_Void_GameObject_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670864);
		NativeMethodInfoPtr_OpenAdvancedMusicSelectionPanel_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670865);
		NativeMethodInfoPtr_ReEnable_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670866);
		NativeMethodInfoPtr_RefreshSongPanel_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670867);
		NativeMethodInfoPtr_ConfirmStage_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670868);
		NativeMethodInfoPtr_SelectStage_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670869);
		NativeMethodInfoPtr_HighlightStage_Public_Void_StageItemUI_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670870);
		NativeMethodInfoPtr_SetInfoPanel_Public_Void_StageItemUI_StageData_StageType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670871);
		NativeMethodInfoPtr_SetHyper_Public_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670872);
		NativeMethodInfoPtr_SetHurry_Public_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670873);
		NativeMethodInfoPtr_SetArcanas_Public_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670874);
		NativeMethodInfoPtr_SetLimitBreak_Public_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670875);
		NativeMethodInfoPtr_SetInverse_Public_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670876);
		NativeMethodInfoPtr_SetEndless_Public_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670877);
		NativeMethodInfoPtr_ToggleHyper_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670878);
		NativeMethodInfoPtr_SetSharePassives_Public_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670879);
		NativeMethodInfoPtr_Stage6Checks_Private_Static_Dictionary_2_StageType_List_1_StageData_Dictionary_2_StageType_List_1_StageData_PlayerOptions_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670880);
		NativeMethodInfoPtr_Populate_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670881);
		NativeMethodInfoPtr_GetAvailableStages_Public_Static_Dictionary_2_StageType_List_1_StageData_DataManager_PlayerOptions_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670882);
		NativeMethodInfoPtr_GenerateNavigation_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670883);
		NativeMethodInfoPtr_CreateStageItem_Private_GameObject_StageData_StageType_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670884);
		NativeMethodInfoPtr_WaitAndDo_Private_Void_Action_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670885);
		NativeMethodInfoPtr_WaitRoutine_Private_Static_IEnumerator_Action_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670886);
		NativeMethodInfoPtr_EnableInfoPanelNavigation_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670887);
		NativeMethodInfoPtr_SetNavigationPhase1_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670888);
		NativeMethodInfoPtr_SetNavigationPhase2_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670889);
		NativeMethodInfoPtr_DisableFirstPhaseGroup_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670890);
		NativeMethodInfoPtr_EnableFirstPhaseGroup_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670891);
		NativeMethodInfoPtr_DisableSecondPhaseGroup_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670892);
		NativeMethodInfoPtr_EnableSecondPhaseGroup_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670893);
		NativeMethodInfoPtr_SwitchInput_Public_Void_ActiveInputType_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670894);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr, 100670895);
	}

	[CallerCount(0)]
	public unsafe void Construct(SignalBus signal, DataManager data, PlayerOptions player, DiContainer diContainer, AdventureManager adventureManager)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)signal);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)data);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)player);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)diContainer);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)adventureManager);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Construct_Private_Void_SignalBus_DataManager_PlayerOptions_DiContainer_AdventureManager_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 995070, XrefRangeEnd = 995075, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Start()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Start_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 995075, XrefRangeEnd = 995081, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void Update()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Update_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 995081, XrefRangeEnd = 995083, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnDestroy()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDestroy_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 995089, RefRangeEnd = 995090, XrefRangeStart = 995083, XrefRangeEnd = 995089, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void BackPressed()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_BackPressed_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 995090, XrefRangeEnd = 995140, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnShowStart(GameObject g)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)g);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnShowStart_Protected_Virtual_Void_GameObject_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 995140, XrefRangeEnd = 995143, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe IEnumerator WaitAndSelect()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_WaitAndSelect_Private_IEnumerator_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerator>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 995228, RefRangeEnd = 995229, XrefRangeStart = 995143, XrefRangeEnd = 995228, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void AutoSizeStageDescriptions()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_AutoSizeStageDescriptions_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 995229, XrefRangeEnd = 995246, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnEnterPressed()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnEnterPressed_Protected_Virtual_Void_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 995246, XrefRangeEnd = 995256, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OnDisable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OnDisable_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 995256, XrefRangeEnd = 995269, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnHideStart(GameObject g)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)g);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnHideStart_Protected_Virtual_Void_GameObject_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 995269, XrefRangeEnd = 995291, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override void OnHideFinish(GameObject g)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)g);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_OnHideFinish_Protected_Virtual_Void_GameObject_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 995291, XrefRangeEnd = 995326, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void OpenAdvancedMusicSelectionPanel()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_OpenAdvancedMusicSelectionPanel_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 995326, XrefRangeEnd = 995373, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ReEnable()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ReEnable_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 995373, XrefRangeEnd = 995390, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void RefreshSongPanel()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_RefreshSongPanel_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 995390, XrefRangeEnd = 995429, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ConfirmStage()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ConfirmStage_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 995449, RefRangeEnd = 995450, XrefRangeStart = 995429, XrefRangeEnd = 995449, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SelectStage()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SelectStage_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 995450, XrefRangeEnd = 995462, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void HighlightStage(StageItemUI item)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)item);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_HighlightStage_Public_Void_StageItemUI_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 995517, RefRangeEnd = 995518, XrefRangeStart = 995462, XrefRangeEnd = 995517, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetInfoPanel(StageItemUI stageItemUI, StageData stage, StageType stageType)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stageItemUI);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stage);
		*(StageType**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &stageType;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetInfoPanel_Public_Void_StageItemUI_StageData_StageType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 995518, XrefRangeEnd = 995521, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetHyper(bool b)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&b);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetHyper_Public_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 995521, XrefRangeEnd = 995524, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetHurry(bool b)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&b);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetHurry_Public_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 995524, XrefRangeEnd = 995525, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetArcanas(bool b)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&b);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetArcanas_Public_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 995525, XrefRangeEnd = 995526, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetLimitBreak(bool b)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&b);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetLimitBreak_Public_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 995526, XrefRangeEnd = 995530, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetInverse(bool b)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&b);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetInverse_Public_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 995530, XrefRangeEnd = 995531, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetEndless(bool b)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&b);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetEndless_Public_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 995531, XrefRangeEnd = 995533, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void ToggleHyper()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToggleHyper_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 995533, XrefRangeEnd = 995536, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetSharePassives(bool b)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&b);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetSharePassives_Public_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 995562, RefRangeEnd = 995563, XrefRangeStart = 995536, XrefRangeEnd = 995562, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Dictionary<StageType, List<StageData>> Stage6Checks(Dictionary<StageType, List<StageData>> STAGE_DATA, PlayerOptions playerOptions)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)STAGE_DATA);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)playerOptions);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Stage6Checks_Private_Static_Dictionary_2_StageType_List_1_StageData_Dictionary_2_StageType_List_1_StageData_PlayerOptions_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<StageType, List<StageData>>>(intPtr) : null;
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 995852, RefRangeEnd = 995853, XrefRangeStart = 995563, XrefRangeEnd = 995852, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Populate()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Populate_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 995961, RefRangeEnd = 995963, XrefRangeStart = 995853, XrefRangeEnd = 995961, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static Dictionary<StageType, List<StageData>> GetAvailableStages(DataManager data, PlayerOptions playerOptions)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)data);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)playerOptions);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetAvailableStages_Public_Static_Dictionary_2_StageType_List_1_StageData_DataManager_PlayerOptions_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Dictionary<StageType, List<StageData>>>(intPtr) : null;
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 996057, RefRangeEnd = 996060, XrefRangeStart = 995963, XrefRangeEnd = 996057, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void GenerateNavigation()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GenerateNavigation_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 996095, RefRangeEnd = 996096, XrefRangeStart = 996060, XrefRangeEnd = 996095, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe GameObject CreateStageItem(StageData stage, StageType type, int index)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stage);
		*(StageType**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &type;
		*(int**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &index;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_CreateStageItem_Private_GameObject_StageData_StageType_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<GameObject>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 996096, XrefRangeEnd = 996100, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void WaitAndDo(Il2CppSystem.Action cb)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cb);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_WaitAndDo_Private_Void_Action_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 996100, XrefRangeEnd = 996103, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe static IEnumerator WaitRoutine(Il2CppSystem.Action cb)
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)cb);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_WaitRoutine_Private_Static_IEnumerator_Action_0, (System.IntPtr)0, (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerator>(intPtr) : null;
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 996103, XrefRangeEnd = 996104, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void EnableInfoPanelNavigation()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EnableInfoPanelNavigation_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 996104, XrefRangeEnd = 996106, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetNavigationPhase1()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetNavigationPhase1_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 996106, XrefRangeEnd = 996108, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetNavigationPhase2()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetNavigationPhase2_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 996130, RefRangeEnd = 996133, XrefRangeStart = 996108, XrefRangeEnd = 996130, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void DisableFirstPhaseGroup()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DisableFirstPhaseGroup_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 996164, RefRangeEnd = 996168, XrefRangeStart = 996133, XrefRangeEnd = 996164, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void EnableFirstPhaseGroup()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EnableFirstPhaseGroup_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(3)]
	[CachedScanResults(RefRangeStart = 996191, RefRangeEnd = 996194, XrefRangeStart = 996168, XrefRangeEnd = 996191, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void DisableSecondPhaseGroup()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_DisableSecondPhaseGroup_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(4)]
	[CachedScanResults(RefRangeStart = 996217, RefRangeEnd = 996221, XrefRangeStart = 996194, XrefRangeEnd = 996217, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void EnableSecondPhaseGroup()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_EnableSecondPhaseGroup_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 996231, RefRangeEnd = 996232, XrefRangeStart = 996221, XrefRangeEnd = 996231, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SwitchInput(UIHelper.ActiveInputType input)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&input);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SwitchInput_Public_Void_ActiveInputType_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 996232, XrefRangeEnd = 996253, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe StageSelectPage()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<StageSelectPage>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public StageSelectPage(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
