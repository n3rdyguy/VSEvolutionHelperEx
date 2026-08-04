using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VampireSurvivors.Data;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VSItemTooltips;

public static class GenericIconPatches
{
	public static void SetWeapon_Postfix(object __instance, WeaponType __0, MethodBase __originalMethod)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (__originalMethod?.DeclaringType?.Name == "ArcanaInfoPanel")
			{
				ItemTooltipsMod.CaptureArcanaAffectedWeapon(__instance, __0);
			}
			GameObject gameObject = GetGameObject(__instance);
			if ((Object)(object)gameObject != (Object)null)
			{
				bool isAddMethod = (object)__originalMethod != null && __originalMethod.Name?.Contains("Add") == true;
				ItemTooltipsMod.RegisterWeaponUI(((Object)gameObject).GetInstanceID(), gameObject, __0, isAddMethod);
			}
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("Error in generic weapon patch: " + ex.Message);
		}
	}

	public static void SetItem_Postfix(object __instance, ItemType __0, MethodBase __originalMethod)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (__originalMethod?.DeclaringType?.Name == "ArcanaInfoPanel")
			{
				ItemTooltipsMod.CaptureArcanaAffectedItem(__instance, __0);
			}
			GameObject gameObject = GetGameObject(__instance);
			if ((Object)(object)gameObject != (Object)null)
			{
				bool isAddMethod = (object)__originalMethod != null && __originalMethod.Name?.Contains("Add") == true;
				ItemTooltipsMod.RegisterItemUI(((Object)gameObject).GetInstanceID(), gameObject, __0, isAddMethod);
			}
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("Error in generic item patch: " + ex.Message);
		}
	}

	public static void SetWeapon_Postfix_Arg1(object __instance, object __0, WeaponType __1, object[] __args, MethodBase __originalMethod)
	{
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (__args != null)
			{
				foreach (object obj in __args)
				{
					if (obj != null && !(obj is WeaponType))
					{
						TryCacheSessionFromArg(obj);
					}
				}
			}
			GameObject gameObject = GetGameObject(__instance);
			if ((Object)(object)gameObject != (Object)null)
			{
				bool isAddMethod = (object)__originalMethod != null && __originalMethod.Name?.Contains("Add") == true;
				ItemTooltipsMod.RegisterWeaponUI(((Object)gameObject).GetInstanceID(), gameObject, __1, isAddMethod);
			}
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("Error in generic weapon patch (arg1): " + ex.Message);
		}
	}

	public static void SetItem_Postfix_Arg1(object __instance, object __0, ItemType __1, object[] __args, MethodBase __originalMethod)
	{
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (__args != null)
			{
				foreach (object obj in __args)
				{
					if (obj != null && !(obj is ItemType))
					{
						TryCacheSessionFromArg(obj);
					}
				}
			}
			GameObject gameObject = GetGameObject(__instance);
			if ((Object)(object)gameObject != (Object)null)
			{
				bool isAddMethod = (object)__originalMethod != null && __originalMethod.Name?.Contains("Add") == true;
				ItemTooltipsMod.RegisterItemUI(((Object)gameObject).GetInstanceID(), gameObject, __1, isAddMethod);
			}
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("Error in generic item patch (arg1): " + ex.Message);
		}
	}

	public static void SetWeapon_Postfix_ArgN(object __instance, object[] __args, MethodBase __originalMethod)
	{
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GameObject gameObject = GetGameObject(__instance);
			if ((Object)(object)gameObject == (Object)null)
			{
				return;
			}
			bool isAddMethod = (object)__originalMethod != null && __originalMethod.Name?.Contains("Add") == true;
			WeaponType? val = null;
			foreach (object obj in __args)
			{
				if (obj != null)
				{
					if (obj is WeaponType value)
					{
						val = value;
					}
					else
					{
						TryCacheSessionFromArg(obj);
					}
				}
			}
			if (val.HasValue)
			{
				ItemTooltipsMod.RegisterWeaponUI(((Object)gameObject).GetInstanceID(), gameObject, val.Value, isAddMethod);
			}
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("Error in generic weapon patch (argN): " + ex.Message);
		}
	}

	public static void SetItem_Postfix_ArgN(object __instance, object[] __args, MethodBase __originalMethod)
	{
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GameObject gameObject = GetGameObject(__instance);
			if ((Object)(object)gameObject == (Object)null)
			{
				return;
			}
			bool isAddMethod = (object)__originalMethod != null && __originalMethod.Name?.Contains("Add") == true;
			ItemType? val = null;
			foreach (object obj in __args)
			{
				if (obj != null)
				{
					if (obj is ItemType value)
					{
						val = value;
					}
					else
					{
						TryCacheSessionFromArg(obj);
					}
				}
			}
			if (val.HasValue)
			{
				ItemTooltipsMod.RegisterItemUI(((Object)gameObject).GetInstanceID(), gameObject, val.Value, isAddMethod);
			}
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("Error in generic item patch (argN): " + ex.Message);
		}
	}

	public static void SetArcana_Postfix_ArgN(object __instance, object[] __args, MethodBase __originalMethod)
	{
		try
		{
			Type cachedArcanaTypeEnum = ItemTooltipsMod.GetCachedArcanaTypeEnum();
			if (cachedArcanaTypeEnum == null)
			{
				return;
			}
			GameObject gameObject = GetGameObject(__instance);
			if ((Object)(object)gameObject == (Object)null)
			{
				return;
			}
			foreach (object obj in __args)
			{
				if (obj != null && obj.GetType() == cachedArcanaTypeEnum)
				{
					ItemTooltipsMod.RegisterArcanaUI(((Object)gameObject).GetInstanceID(), gameObject, obj);
					break;
				}
			}
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("Error in generic arcana patch (argN): " + ex.Message);
		}
	}

	private static void TryCacheSessionFromArg(object arg)
	{
		if (arg == null)
		{
			return;
		}
		Type type = arg.GetType();
		string text = type.Name.ToLower();
		if (!ItemTooltipsMod.HasCachedDataManager() && type.GetMethod("GetConvertedWeapons") != null)
		{
			ItemTooltipsMod.CacheDataManager(arg);
		}
		else if (text.Contains("character") || text.Contains("controller"))
		{
			TryCacheSessionFromCharacter(arg);
		}
		else
		{
			if (!text.Contains("page") && !text.Contains("view") && !text.Contains("window"))
			{
				return;
			}
			string[] array = new string[4] { "_gameSession", "GameSession", "gameSession", "_session" };
			string[] array2 = array;
			string[] array3 = array2;
			foreach (string name in array3)
			{
				PropertyInfo property = type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (property != null)
				{
					try
					{
						object value = property.GetValue(arg);
						if (value != null)
						{
							PropertyInfo property2 = value.GetType().GetProperty("ActiveCharacter", BindingFlags.Instance | BindingFlags.Public);
							if (property2 != null)
							{
								ItemTooltipsMod.CacheGameSession(value);
								return;
							}
						}
					}
					catch
					{
					}
				}
				FieldInfo field = type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (!(field != null))
				{
					continue;
				}
				try
				{
					object value2 = field.GetValue(arg);
					if (value2 != null)
					{
						PropertyInfo property3 = value2.GetType().GetProperty("ActiveCharacter", BindingFlags.Instance | BindingFlags.Public);
						if (property3 != null)
						{
							ItemTooltipsMod.CacheGameSession(value2);
							return;
						}
					}
				}
				catch
				{
				}
			}
			if (ItemTooltipsMod.HasCachedDataManager())
			{
				return;
			}
			PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			PropertyInfo[] array4 = properties;
			PropertyInfo[] array5 = array4;
			foreach (PropertyInfo propertyInfo in array5)
			{
				if (!propertyInfo.PropertyType.Name.Contains("DataManager"))
				{
					continue;
				}
				try
				{
					object value3 = propertyInfo.GetValue(arg);
					if (value3 != null)
					{
						ItemTooltipsMod.CacheDataManager(value3);
						return;
					}
				}
				catch
				{
				}
			}
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			FieldInfo[] array6 = fields;
			FieldInfo[] array7 = array6;
			foreach (FieldInfo fieldInfo in array7)
			{
				if (!fieldInfo.FieldType.Name.Contains("DataManager"))
				{
					continue;
				}
				try
				{
					object value4 = fieldInfo.GetValue(arg);
					if (value4 != null)
					{
						ItemTooltipsMod.CacheDataManager(value4);
						break;
					}
				}
				catch
				{
				}
			}
		}
	}

	private static void TryCacheSessionFromCharacter(object character)
	{
		if (character == null)
		{
			return;
		}
		Type type = character.GetType();
		try
		{
			PropertyInfo property = type.GetProperty("_gameManager", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (property == null)
			{
				return;
			}
			object value = property.GetValue(character);
			if (value == null || TryGetSessionFromObject(value, "GameManager"))
			{
				return;
			}
			PropertyInfo property2 = value.GetType().GetProperty("_stage", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (property2 != null)
			{
				object value2 = property2.GetValue(value);
				if (value2 != null)
				{
					if (TryGetSessionFromObject(value2, "Stage"))
					{
						return;
					}
					List<string> list = (from p in value2.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
						select p.Name).Take(20).ToList();
				}
			}
			PropertyInfo property3 = value.GetType().GetProperty("_adventureManager", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (property3 != null)
			{
				object value3 = property3.GetValue(value);
				if (value3 != null && TryGetSessionFromObject(value3, "AdventureManager"))
				{
				}
			}
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[TryCacheSessionFromCharacter] Error: " + ex.Message);
		}
	}

	private static bool TryGetSessionFromObject(object obj, string objName)
	{
		if (obj == null)
		{
			return false;
		}
		TryCacheDataManagerFromObject(obj, objName);
		string[] array = new string[8] { "GameSessionData", "_gameSession", "GameSession", "gameSession", "_session", "Session", "CurrentSession", "_currentSession" };
		string[] array2 = array;
		string[] array3 = array2;
		foreach (string name in array3)
		{
			try
			{
				PropertyInfo property = obj.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (!(property != null))
				{
					continue;
				}
				object value = property.GetValue(obj);
				if (value != null)
				{
					PropertyInfo property2 = value.GetType().GetProperty("ActiveCharacter", BindingFlags.Instance | BindingFlags.Public);
					if (property2 != null)
					{
						ItemTooltipsMod.CacheGameSession(value);
						return true;
					}
				}
			}
			catch
			{
			}
		}
		return false;
	}

	public static void TryCacheDataManagerFromGameManager(object gameManager)
	{
		if (gameManager == null || ItemTooltipsMod.HasCachedDataManager())
		{
			return;
		}
		Type type = gameManager.GetType();
		string[] array = new string[4] { "Data", "_data", "DataManager", "_dataManager" };
		string[] array2 = array;
		string[] array3 = array2;
		foreach (string text in array3)
		{
			try
			{
				PropertyInfo property = type.GetProperty(text, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (property != null)
				{
					object value = property.GetValue(gameManager);
					if (value != null && value.GetType().Name.Contains("DataManager"))
					{
						ItemTooltipsMod.CacheDataManager(value);
						return;
					}
				}
				MethodInfo method = type.GetMethod("get_" + text, BindingFlags.Instance | BindingFlags.Public);
				if (method != null)
				{
					object obj = method.Invoke(gameManager, null);
					if (obj != null && obj.GetType().Name.Contains("DataManager"))
					{
						ItemTooltipsMod.CacheDataManager(obj);
						return;
					}
				}
			}
			catch (Exception)
			{
			}
		}
		List<string> list = (from p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
			select p.Name).Take(25).ToList();
	}

	private static void TryCacheDataManagerFromObject(object obj, string objName)
	{
		if (obj == null)
		{
			return;
		}
		string[] array = new string[5] { "Data", "_data", "DataManager", "_dataManager", "data" };
		string[] array2 = array;
		string[] array3 = array2;
		foreach (string name in array3)
		{
			try
			{
				PropertyInfo property = obj.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (property != null)
				{
					object value = property.GetValue(obj);
					if (value != null && value.GetType().Name.Contains("DataManager"))
					{
						ItemTooltipsMod.CacheDataManager(value);
						break;
					}
				}
			}
			catch
			{
			}
		}
	}

	private static GameObject GetGameObject(object instance)
	{
		object obj = instance.GetType().GetProperty("gameObject", BindingFlags.Instance | BindingFlags.Public)?.GetValue(instance);
		return (GameObject)((obj is GameObject) ? obj : null);
	}
}
