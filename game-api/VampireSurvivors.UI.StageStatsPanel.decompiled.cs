using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections;
using UnityEngine;
using UnityEngine.UI;
using VampireSurvivors.Data;
using VampireSurvivors.Data.Stage;
using VampireSurvivors.Objects;

namespace VampireSurvivors.UI;

public class StageStatsPanel : MonoBehaviour
{
	[ObfuscatedName("VampireSurvivors.UI.StageStatsPanel+<WaitAndCheckPages>d__24")]
	public sealed class _WaitAndCheckPages_d__24 : Il2CppSystem.Object
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

		public unsafe StageStatsPanel __4__this
		{
			get
			{
				nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___4__this);
				System.IntPtr intPtr = *(System.IntPtr*)num;
				return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<StageStatsPanel>(intPtr) : null;
			}
			set
			{
				System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
				IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr___4__this)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stageStatsPanel));
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

		static _WaitAndCheckPages_d__24()
		{
			Il2CppClassPointerStore<_WaitAndCheckPages_d__24>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, "<WaitAndCheckPages>d__24");
			IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<_WaitAndCheckPages_d__24>.NativeClassPtr);
			NativeFieldInfoPtr___1__state = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_WaitAndCheckPages_d__24>.NativeClassPtr, "<>1__state");
			NativeFieldInfoPtr___2__current = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_WaitAndCheckPages_d__24>.NativeClassPtr, "<>2__current");
			NativeFieldInfoPtr___4__this = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<_WaitAndCheckPages_d__24>.NativeClassPtr, "<>4__this");
			NativeMethodInfoPtr__ctor_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_WaitAndCheckPages_d__24>.NativeClassPtr, 100670933);
			NativeMethodInfoPtr_System_IDisposable_Dispose_Private_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_WaitAndCheckPages_d__24>.NativeClassPtr, 100670934);
			NativeMethodInfoPtr_MoveNext_Private_Virtual_Final_New_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_WaitAndCheckPages_d__24>.NativeClassPtr, 100670935);
			NativeMethodInfoPtr_System_Collections_Generic_IEnumerator_System_Object__get_Current_Private_Virtual_Final_New_get_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_WaitAndCheckPages_d__24>.NativeClassPtr, 100670936);
			NativeMethodInfoPtr_System_Collections_IEnumerator_Reset_Private_Virtual_Final_New_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_WaitAndCheckPages_d__24>.NativeClassPtr, 100670937);
			NativeMethodInfoPtr_System_Collections_IEnumerator_get_Current_Private_Virtual_Final_New_get_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<_WaitAndCheckPages_d__24>.NativeClassPtr, 100670938);
		}

		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 40, RefRangeEnd = 41, XrefRangeStart = 40, XrefRangeEnd = 41, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe _WaitAndCheckPages_d__24(int <>1__state)
			: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<_WaitAndCheckPages_d__24>.NativeClassPtr))
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
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 996253, XrefRangeEnd = 996255, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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
		[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 996255, XrefRangeEnd = 996260, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		public unsafe virtual void System_Collections_IEnumerator_Reset()
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_System_Collections_IEnumerator_Reset_Private_Virtual_Final_New_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}

		public _WaitAndCheckPages_d__24(System.IntPtr pointer)
			: base(pointer)
		{
		}
	}

	private static readonly System.IntPtr NativeFieldInfoPtr__TimeLimit;

	private static readonly System.IntPtr NativeFieldInfoPtr__ClockSpeed;

	private static readonly System.IntPtr NativeFieldInfoPtr__MoveSpeed;

	private static readonly System.IntPtr NativeFieldInfoPtr__GoldBonus;

	private static readonly System.IntPtr NativeFieldInfoPtr__LuckBonus;

	private static readonly System.IntPtr NativeFieldInfoPtr__EnemyHealth;

	private static readonly System.IntPtr NativeFieldInfoPtr__Description;

	private static readonly System.IntPtr NativeFieldInfoPtr__XPBonus;

	private static readonly System.IntPtr NativeFieldInfoPtr__DescriptionPage;

	private static readonly System.IntPtr NativeFieldInfoPtr__PreviousPage;

	private static readonly System.IntPtr NativeFieldInfoPtr__NextPage;

	private static readonly System.IntPtr NativeFieldInfoPtr__currentStage;

	private static readonly System.IntPtr NativeFieldInfoPtr__currentType;

	private static readonly System.IntPtr NativeFieldInfoPtr__hyperSelected;

	private static readonly System.IntPtr NativeFieldInfoPtr__hurrySelected;

	private static readonly System.IntPtr NativeFieldInfoPtr__inverseSelected;

	private static readonly System.IntPtr NativeFieldInfoPtr__darkRed;

	private static readonly System.IntPtr NativeFieldInfoPtr__playerOptions;

	private static readonly System.IntPtr NativeFieldInfoPtr__pageCount;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetHyper_Public_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetHurry_Public_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetInverse_Public_Void_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetPlayerOptions_Public_Void_PlayerOptions_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Refresh_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_WaitAndCheckPages_Private_IEnumerator_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ShowHyperInfo_Private_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetStage_Public_Void_StageData_StageType_PlayerOptions_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetTimeLimit_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetClockSpeed_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetMoveSpeed_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetGoldBonus_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetLuckBonus_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetEnemyHealth_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetDescription_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetXPBonus_Private_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_0;

	public unsafe StageStatUI _TimeLimit
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__TimeLimit);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<StageStatUI>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__TimeLimit)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stageStatUI));
		}
	}

	public unsafe StageStatUI _ClockSpeed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__ClockSpeed);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<StageStatUI>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__ClockSpeed)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stageStatUI));
		}
	}

	public unsafe StageStatUI _MoveSpeed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__MoveSpeed);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<StageStatUI>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__MoveSpeed)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stageStatUI));
		}
	}

	public unsafe StageStatUI _GoldBonus
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__GoldBonus);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<StageStatUI>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__GoldBonus)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stageStatUI));
		}
	}

	public unsafe StageStatUI _LuckBonus
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__LuckBonus);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<StageStatUI>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__LuckBonus)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stageStatUI));
		}
	}

	public unsafe StageStatUI _EnemyHealth
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__EnemyHealth);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<StageStatUI>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__EnemyHealth)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stageStatUI));
		}
	}

	public unsafe StageStatUI _Description
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Description);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<StageStatUI>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__Description)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stageStatUI));
		}
	}

	public unsafe StageStatUI _XPBonus
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__XPBonus);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<StageStatUI>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__XPBonus)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stageStatUI));
		}
	}

	public unsafe GameObject _DescriptionPage
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__DescriptionPage);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<GameObject>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__DescriptionPage)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)gameObject));
		}
	}

	public unsafe Button _PreviousPage
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__PreviousPage);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Button>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__PreviousPage)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)button));
		}
	}

	public unsafe Button _NextPage
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__NextPage);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Button>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__NextPage)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)button));
		}
	}

	public unsafe StageData _currentStage
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__currentStage);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<StageData>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__currentStage)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stageData));
		}
	}

	public unsafe StageType _currentType
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__currentType);
			return *(StageType*)num;
		}
		set
		{
			*(StageType*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__currentType)) = stageType;
		}
	}

	public unsafe bool _hyperSelected
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hyperSelected);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hyperSelected)) = flag;
		}
	}

	public unsafe bool _hurrySelected
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hurrySelected);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__hurrySelected)) = flag;
		}
	}

	public unsafe bool _inverseSelected
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__inverseSelected);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__inverseSelected)) = flag;
		}
	}

	public unsafe Color _darkRed
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__darkRed);
			return *(Color*)num;
		}
		set
		{
			*(Color*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__darkRed)) = color;
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

	public unsafe int _pageCount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__pageCount);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr__pageCount)) = num;
		}
	}

	static StageStatsPanel()
	{
		Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr = IL2CPP.GetIl2CppClass("VampireSurvivors.Runtime.dll", "VampireSurvivors.UI", "StageStatsPanel");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr);
		NativeFieldInfoPtr__TimeLimit = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, "_TimeLimit");
		NativeFieldInfoPtr__ClockSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, "_ClockSpeed");
		NativeFieldInfoPtr__MoveSpeed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, "_MoveSpeed");
		NativeFieldInfoPtr__GoldBonus = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, "_GoldBonus");
		NativeFieldInfoPtr__LuckBonus = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, "_LuckBonus");
		NativeFieldInfoPtr__EnemyHealth = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, "_EnemyHealth");
		NativeFieldInfoPtr__Description = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, "_Description");
		NativeFieldInfoPtr__XPBonus = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, "_XPBonus");
		NativeFieldInfoPtr__DescriptionPage = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, "_DescriptionPage");
		NativeFieldInfoPtr__PreviousPage = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, "_PreviousPage");
		NativeFieldInfoPtr__NextPage = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, "_NextPage");
		NativeFieldInfoPtr__currentStage = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, "_currentStage");
		NativeFieldInfoPtr__currentType = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, "_currentType");
		NativeFieldInfoPtr__hyperSelected = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, "_hyperSelected");
		NativeFieldInfoPtr__hurrySelected = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, "_hurrySelected");
		NativeFieldInfoPtr__inverseSelected = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, "_inverseSelected");
		NativeFieldInfoPtr__darkRed = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, "_darkRed");
		NativeFieldInfoPtr__playerOptions = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, "_playerOptions");
		NativeFieldInfoPtr__pageCount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, "_pageCount");
		NativeMethodInfoPtr_SetHyper_Public_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, 100670916);
		NativeMethodInfoPtr_SetHurry_Public_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, 100670917);
		NativeMethodInfoPtr_SetInverse_Public_Void_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, 100670918);
		NativeMethodInfoPtr_SetPlayerOptions_Public_Void_PlayerOptions_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, 100670919);
		NativeMethodInfoPtr_Refresh_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, 100670920);
		NativeMethodInfoPtr_WaitAndCheckPages_Private_IEnumerator_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, 100670921);
		NativeMethodInfoPtr_ShowHyperInfo_Private_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, 100670922);
		NativeMethodInfoPtr_SetStage_Public_Void_StageData_StageType_PlayerOptions_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, 100670923);
		NativeMethodInfoPtr_SetTimeLimit_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, 100670924);
		NativeMethodInfoPtr_SetClockSpeed_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, 100670925);
		NativeMethodInfoPtr_SetMoveSpeed_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, 100670926);
		NativeMethodInfoPtr_SetGoldBonus_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, 100670927);
		NativeMethodInfoPtr_SetLuckBonus_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, 100670928);
		NativeMethodInfoPtr_SetEnemyHealth_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, 100670929);
		NativeMethodInfoPtr_SetDescription_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, 100670930);
		NativeMethodInfoPtr_SetXPBonus_Private_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, 100670931);
		NativeMethodInfoPtr__ctor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr, 100670932);
	}

	[CallerCount(0)]
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
	public unsafe void SetInverse(bool b)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&b);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetInverse_Public_Void_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(17)]
	[CachedScanResults(RefRangeStart = 888369, RefRangeEnd = 888386, XrefRangeStart = 888369, XrefRangeEnd = 888386, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetPlayerOptions(PlayerOptions playerOptions)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)playerOptions);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetPlayerOptions_Public_Void_PlayerOptions_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(5)]
	[CachedScanResults(RefRangeStart = 996291, RefRangeEnd = 996296, XrefRangeStart = 996260, XrefRangeEnd = 996291, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void Refresh()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Refresh_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 996296, XrefRangeEnd = 996299, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe IEnumerator WaitAndCheckPages()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_WaitAndCheckPages_Private_IEnumerator_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<IEnumerator>(intPtr) : null;
	}

	[CallerCount(0)]
	public unsafe bool ShowHyperInfo()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ShowHyperInfo_Private_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 996302, RefRangeEnd = 996304, XrefRangeStart = 996299, XrefRangeEnd = 996302, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetStage(StageData stage, StageType t, PlayerOptions playerOptions)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)stage);
		*(StageType**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &t;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)playerOptions);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetStage_Public_Void_StageData_StageType_PlayerOptions_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 996340, RefRangeEnd = 996341, XrefRangeStart = 996304, XrefRangeEnd = 996340, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetTimeLimit()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetTimeLimit_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 996358, RefRangeEnd = 996359, XrefRangeStart = 996341, XrefRangeEnd = 996358, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetClockSpeed()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetClockSpeed_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 996375, RefRangeEnd = 996376, XrefRangeStart = 996359, XrefRangeEnd = 996375, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetMoveSpeed()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetMoveSpeed_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 996402, RefRangeEnd = 996403, XrefRangeStart = 996376, XrefRangeEnd = 996402, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetGoldBonus()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetGoldBonus_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 996422, RefRangeEnd = 996423, XrefRangeStart = 996403, XrefRangeEnd = 996422, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetLuckBonus()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetLuckBonus_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 996442, RefRangeEnd = 996443, XrefRangeStart = 996423, XrefRangeEnd = 996442, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetEnemyHealth()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetEnemyHealth_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 996471, RefRangeEnd = 996472, XrefRangeStart = 996443, XrefRangeEnd = 996471, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetDescription()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetDescription_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 996504, RefRangeEnd = 996505, XrefRangeStart = 996472, XrefRangeEnd = 996504, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetXPBonus()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetXPBonus_Private_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 996505, XrefRangeEnd = 996508, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe StageStatsPanel()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<StageStatsPanel>.NativeClassPtr))
	{
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public StageStatsPanel(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
