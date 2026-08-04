using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using Il2CppSystem.Collections;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Globalization;
using Il2CppSystem.Reflection;
using Il2CppSystem.Text.RegularExpressions;
using UnityEngine;

namespace I2.Loc;

public static class LocalizationManager : Il2CppSystem.Object
{
	public sealed class FnCustomApplyLocalizationParams : Il2CppSystem.MulticastDelegate
	{
		private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_Invoke_Public_Virtual_New_Boolean_byref_String__GetParam_Boolean_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_byref_String__GetParam_Boolean_AsyncCallback_Object_0;

		private static readonly System.IntPtr NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Boolean_byref_String_IAsyncResult_0;

		static FnCustomApplyLocalizationParams()
		{
			Il2CppClassPointerStore<FnCustomApplyLocalizationParams>.NativeClassPtr = IL2CPP.GetIl2CppNestedType(Il2CppClassPointerStore<LocalizationManager>.NativeClassPtr, "FnCustomApplyLocalizationParams");
			NativeMethodInfoPtr__ctor_Public_Void_Object_IntPtr_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FnCustomApplyLocalizationParams>.NativeClassPtr, 100663661);
			NativeMethodInfoPtr_Invoke_Public_Virtual_New_Boolean_byref_String__GetParam_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FnCustomApplyLocalizationParams>.NativeClassPtr, 100663662);
			NativeMethodInfoPtr_BeginInvoke_Public_Virtual_New_IAsyncResult_byref_String__GetParam_Boolean_AsyncCallback_Object_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FnCustomApplyLocalizationParams>.NativeClassPtr, 100663663);
			NativeMethodInfoPtr_EndInvoke_Public_Virtual_New_Boolean_byref_String_IAsyncResult_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<FnCustomApplyLocalizationParams>.NativeClassPtr, 100663664);
		}

		[CallerCount(0)]
		public unsafe FnCustomApplyLocalizationParams(Il2CppSystem.Object @object, System.IntPtr method)
