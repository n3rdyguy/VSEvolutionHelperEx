using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VampireSurvivors.Data;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VSItemTooltips;

public static class LevelUpItemUIPatches
{
	public static void SetWeaponData_Postfix(object __instance, object __0, WeaponType type)
	{
		try
		{
			TryCacheGameSessionFromPage(__0);
			if (!TryGetGameObject(__instance, out GameObject val, out int instanceId))
				return;
			// Prefer small icon child so full-card hitboxes don't fire tooltips
			GameObject target = PreferIconChild(val);
			ItemTooltipsMod.RegisterWeaponUI(((Object)target).GetInstanceID(), target, type);
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("Error in SetWeaponData patch: " + ex.Message);
		}
	}

	public static void SetItemData_Postfix(object __instance, object __2, ItemType type)
	{
		try
		{
			TryCacheGameSessionFromPage(__2);
			if (!TryGetGameObject(__instance, out GameObject val, out int instanceId))
				return;
			GameObject target = PreferIconChild(val);
			ItemTooltipsMod.RegisterItemUI(((Object)target).GetInstanceID(), target, type);
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("Error in SetItemData patch: " + ex.Message);
		}
	}

	private static bool TryGetGameObject(object __instance, out GameObject go, out int instanceId)
	{
		go = null;
		instanceId = 0;
		Type type2 = __instance.GetType();
		PropertyInfo property = type2.GetProperty("gameObject", BindingFlags.Instance | BindingFlags.Public);
		MethodInfo method = type2.GetMethod("GetInstanceID", BindingFlags.Instance | BindingFlags.Public);
		if (property == null || method == null) return false;
		object value = property.GetValue(__instance);
		go = (GameObject)((value is GameObject) ? value : null);
		instanceId = (int)method.Invoke(__instance, null);
		return (Object)(object)go != (Object)null;
	}

	/// <summary>Use Icon / weapon image child when present (avoids full-card PointerEnter).</summary>
	private static GameObject PreferIconChild(GameObject root)
	{
		if ((Object)(object)root == (Object)null) return root;
		string[] names = { "Icon", "icon", "WeaponIcon", "ItemIcon", "Image", "Sprite" };
		foreach (string n in names)
		{
			Transform t = root.transform.Find(n);
			if ((Object)(object)t != (Object)null)
				return t.gameObject;
		}
		// Smallest Image with a sprite (likely the weapon icon, not the frame)
		var images = root.GetComponentsInChildren<UnityEngine.UI.Image>(true);
		if (images == null || images.Count == 0) return root;
		GameObject best = root;
		float bestArea = float.MaxValue;
		for (int i = 0; i < images.Count; i++)
		{
			var img = images[i];
			if ((Object)(object)img == (Object)null || (Object)(object)img.sprite == (Object)null) continue;
			string nm = ((Object)img).name.ToLowerInvariant();
			if (nm.Contains("background") || nm.Contains("frame") || nm.Contains("panel") || nm.Contains("fill"))
				continue;
			var rt = img.rectTransform;
			if ((Object)(object)rt == (Object)null) continue;
			float area = Mathf.Abs(rt.rect.width * rt.rect.height);
			if (area < 4f || area > 120f * 120f) continue; // skip tiny noise and huge panels
			if (area < bestArea)
			{
				bestArea = area;
				best = ((Component)img).gameObject;
			}
		}
		return best;
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
			Plugin.Log.LogWarning("Error caching session from page: " + ex.Message);
		}
	}
}
