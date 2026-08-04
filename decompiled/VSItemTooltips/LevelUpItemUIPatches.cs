using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Il2CppVampireSurvivors.Data;
using MelonLoader;
using UnityEngine;

namespace VSItemTooltips;

public static class LevelUpItemUIPatches
{
	public static void SetWeaponData_Postfix(object __instance, object __0, WeaponType type)
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			TryCacheGameSessionFromPage(__0);
			Type type2 = __instance.GetType();
			PropertyInfo property = type2.GetProperty("gameObject", BindingFlags.Instance | BindingFlags.Public);
			MethodInfo method = type2.GetMethod("GetInstanceID", BindingFlags.Instance | BindingFlags.Public);
			if (property != null && method != null)
			{
				object? value = property.GetValue(__instance);
				GameObject val = (GameObject)((value is GameObject) ? value : null);
				int instanceId = (int)method.Invoke(__instance, null);
				if ((Object)(object)val != (Object)null)
				{
					ItemTooltipsMod.RegisterWeaponUI(instanceId, val, type);
				}
			}
		}
		catch (Exception ex)
		{
			MelonLogger.Warning("Error in SetWeaponData patch: " + ex.Message);
		}
	}

	public static void SetItemData_Postfix(object __instance, object __2, ItemType type)
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			TryCacheGameSessionFromPage(__2);
			Type type2 = __instance.GetType();
			PropertyInfo property = type2.GetProperty("gameObject", BindingFlags.Instance | BindingFlags.Public);
			MethodInfo method = type2.GetMethod("GetInstanceID", BindingFlags.Instance | BindingFlags.Public);
			if (property != null && method != null)
			{
				object? value = property.GetValue(__instance);
				GameObject val = (GameObject)((value is GameObject) ? value : null);
				int instanceId = (int)method.Invoke(__instance, null);
				if ((Object)(object)val != (Object)null)
				{
					ItemTooltipsMod.RegisterItemUI(instanceId, val, type);
				}
			}
		}
		catch (Exception ex)
		{
			MelonLogger.Warning("Error in SetItemData patch: " + ex.Message);
		}
	}

	private static void TryCacheGameSessionFromPage(object page)
	{
		if (page == null)
		{
			return;
		}
		try
		{
			Type type = page.GetType();
			string[] array = new string[5] { "_gameSession", "GameSession", "gameSession", "_session", "Session" };
			string[] array2 = array;
			string[] array3 = array2;
			foreach (string name in array3)
			{
				PropertyInfo property = type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (property != null)
				{
					object value = property.GetValue(page);
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
				FieldInfo field = type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (!(field != null))
				{
					continue;
				}
				object value2 = field.GetValue(page);
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
			PropertyInfo property4 = type.GetProperty("ActiveCharacter", BindingFlags.Instance | BindingFlags.Public);
			if (property4 != null)
			{
				ItemTooltipsMod.CacheGameSession(page);
				return;
			}
			List<string> list = (from p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
				select p.Name).Take(10).ToList();
		}
		catch (Exception ex)
		{
			MelonLogger.Warning("Error caching session from page: " + ex.Message);
		}
	}
}
