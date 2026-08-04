using System;
using System.Reflection;
using MelonLoader;

namespace VSItemTooltips;

public static class GenericPagePatches
{
	public static void Show_Postfix(object __instance)
	{
		try
		{
			object dataManager = GetDataManager(__instance);
			if (dataManager != null)
			{
				ItemTooltipsMod.CacheDataManager(dataManager);
			}
		}
		catch (Exception ex)
		{
			MelonLogger.Warning("Error in page Show patch: " + ex.Message);
		}
	}

	private static object GetDataManager(object page)
	{
		Type type = page.GetType();
		PropertyInfo property = type.GetProperty("Data", BindingFlags.Instance | BindingFlags.Public);
		if (property != null)
		{
			object value = property.GetValue(page);
			if (value != null)
			{
				return value;
			}
		}
		FieldInfo field = type.GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic);
		if (field != null)
		{
			object value2 = field.GetValue(page);
			if (value2 != null)
			{
				return value2;
			}
		}
		Type baseType = type.BaseType;
		while (baseType != null)
		{
			property = baseType.GetProperty("Data", BindingFlags.Instance | BindingFlags.Public);
			if (property != null)
			{
				object value3 = property.GetValue(page);
				if (value3 != null)
				{
					return value3;
				}
			}
			baseType = baseType.BaseType;
		}
		return null;
	}
}
