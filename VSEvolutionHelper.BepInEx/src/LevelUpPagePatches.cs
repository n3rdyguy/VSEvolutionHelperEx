using System;
using System.Reflection;
using VampireSurvivors.Data;
using VampireSurvivors.UI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VSItemTooltips;

public static class LevelUpPagePatches
{
	public static void Show_Postfix(LevelUpPage __instance)
	{
		try
		{
			DataManager data = ((BaseUIPage)__instance).Data;
			if (data != null)
			{
				GameData.CacheFrom(data);
				ItemTooltipsMod.CacheDataManager(data);
			}
			TryCacheGameSessionFromLevelUpPage(__instance);
		}
		catch (Exception arg)
		{
			Plugin.Log.LogError($"Error in LevelUpPage.Show patch: {arg}");
		}
	}

	private static void TryCacheGameSessionFromLevelUpPage(LevelUpPage page)
	{
		if ((Object)(object)page == (Object)null)
		{
			return;
		}
		Type type = ((object)page).GetType();
		string[] array = new string[5] { "_gameSession", "GameSession", "gameSession", "_session", "Session" };
		string[] array2 = array;
		string[] array3 = array2;
		foreach (string text in array3)
		{
			PropertyInfo property = type.GetProperty(text, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (property != null)
			{
				try
				{
					object value = property.GetValue(page);
					if (value != null && ValidateGameSession(value))
					{
						ItemTooltipsMod.CacheGameSession(value);
						return;
					}
				}
				catch (Exception ex)
				{
					Plugin.Log.LogWarning("Error accessing " + text + " property: " + ex.Message);
				}
			}
			FieldInfo field = type.GetField(text, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (!(field != null))
			{
				continue;
			}
			try
			{
				object value2 = field.GetValue(page);
				if (value2 != null && ValidateGameSession(value2))
				{
					ItemTooltipsMod.CacheGameSession(value2);
					return;
				}
			}
			catch (Exception ex2)
			{
				Plugin.Log.LogWarning("Error accessing " + text + " field: " + ex2.Message);
			}
		}
		PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		PropertyInfo[] array4 = properties;
		foreach (PropertyInfo propertyInfo in array4)
		{
			if (!propertyInfo.Name.ToLower().Contains("session") && !propertyInfo.Name.ToLower().Contains("game"))
			{
				continue;
			}
			try
			{
				object value3 = propertyInfo.GetValue(page);
				if (value3 != null && ValidateGameSession(value3))
				{
					ItemTooltipsMod.CacheGameSession(value3);
					return;
				}
			}
			catch
			{
			}
		}
		Plugin.Log.LogWarning("Could not find GameSession in LevelUpPage!");
	}

	private static bool ValidateGameSession(object session)
	{
		if (session == null)
		{
			return false;
		}
		PropertyInfo property = session.GetType().GetProperty("ActiveCharacter", BindingFlags.Instance | BindingFlags.Public);
		return property != null;
	}
}
