using System;
using System.Reflection;
using Il2CppVampireSurvivors.Data;
using MelonLoader;
using UnityEngine;

namespace VSItemTooltips;

public static class EquipmentIconPatches
{
	public static void SetData_Postfix(object __instance)
	{
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Type type = __instance.GetType();
			PropertyInfo property = type.GetProperty("gameObject", BindingFlags.Instance | BindingFlags.Public);
			if (property == null)
			{
				return;
			}
			object? value = property.GetValue(__instance);
			GameObject val = (GameObject)((value is GameObject) ? value : null);
			if ((Object)(object)val == (Object)null)
			{
				return;
			}
			PropertyInfo property2 = type.GetProperty("Type", BindingFlags.Instance | BindingFlags.Public);
			if (property2 != null)
			{
				object value2 = property2.GetValue(__instance);
				if (value2 is WeaponType type2)
				{
					ItemTooltipsMod.RegisterWeaponUI(((Object)val).GetInstanceID(), val, type2);
				}
				else if (value2 is ItemType type3)
				{
					ItemTooltipsMod.RegisterItemUI(((Object)val).GetInstanceID(), val, type3);
				}
			}
			FieldInfo field = type.GetField("_weaponType", BindingFlags.Instance | BindingFlags.NonPublic);
			if (field != null && field.GetValue(__instance) is WeaponType type4)
			{
				ItemTooltipsMod.RegisterWeaponUI(((Object)val).GetInstanceID(), val, type4);
			}
			FieldInfo field2 = type.GetField("_itemType", BindingFlags.Instance | BindingFlags.NonPublic);
			if (field2 != null && field2.GetValue(__instance) is ItemType type5)
			{
				ItemTooltipsMod.RegisterItemUI(((Object)val).GetInstanceID(), val, type5);
			}
		}
		catch (Exception ex)
		{
			MelonLogger.Warning("Error in EquipmentIcon patch: " + ex.Message);
		}
	}
}
