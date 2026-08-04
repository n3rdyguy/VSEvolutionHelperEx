using System;
using HarmonyLib;
using UnityEngine;
using VampireSurvivors.Data;
using VampireSurvivors.UI;
using Object = UnityEngine.Object;

namespace VSItemTooltips;

/// <summary>
/// Grimoire / Collections evolution formula rows use EvolutionItemUI.AddWeaponIcon(WeaponType)
/// which creates a child icon GameObject per ingredient/result. Generic patches only saw the
/// parent EvolutionItemUI instance, so every icon in a row shared one InstanceID and only
/// the last WeaponType stuck — hence "only the middle (or one) icon has a tooltip".
///
/// Also patches CollectionItemUI.SetData/SetItem/SetArcana for reliable collection-grid hover.
/// </summary>
public static class GrimoirePatches
{
	public static void Apply(Harmony harmony)
	{
		try
		{
			var addIcon = typeof(EvolutionItemUI).GetMethod(
				"AddWeaponIcon",
				System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic,
				null,
				new Type[] { typeof(WeaponType) },
				null);
			if (addIcon != null)
			{
				harmony.Patch(addIcon, postfix: new HarmonyMethod(typeof(GrimoirePatches), nameof(AddWeaponIcon_Postfix)));
				Plugin.Log.LogInfo("[Grimoire] Patched EvolutionItemUI.AddWeaponIcon");
			}
			else
			{
				Plugin.Log.LogWarning("[Grimoire] EvolutionItemUI.AddWeaponIcon not found");
			}
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Grimoire] AddWeaponIcon patch: " + ex.Message);
		}

		try
		{
			var setData = typeof(CollectionItemUI).GetMethod(
				"SetData",
				System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public,
				null,
				new Type[] { typeof(VampireSurvivors.Data.Weapons.WeaponData), typeof(CollectionsPage), typeof(WeaponType), typeof(bool) },
				null);
			if (setData != null)
			{
				harmony.Patch(setData, postfix: new HarmonyMethod(typeof(GrimoirePatches), nameof(CollectionItem_SetData_Postfix)));
				Plugin.Log.LogInfo("[Grimoire] Patched CollectionItemUI.SetData");
			}
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Grimoire] CollectionItemUI.SetData patch: " + ex.Message);
		}

		try
		{
			// SetItem(ItemData, CollectionsPage, ItemType, bool)
			foreach (var m in typeof(CollectionItemUI).GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
			{
				if (m.Name != "SetItem") continue;
				var ps = m.GetParameters();
				if (ps.Length >= 3 && ps[2].ParameterType == typeof(ItemType))
				{
					harmony.Patch(m, postfix: new HarmonyMethod(typeof(GrimoirePatches), nameof(CollectionItem_SetItem_Postfix)));
					Plugin.Log.LogInfo("[Grimoire] Patched CollectionItemUI.SetItem");
					break;
				}
			}
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Grimoire] CollectionItemUI.SetItem patch: " + ex.Message);
		}

		try
		{
			foreach (var m in typeof(CollectionItemUI).GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
			{
				if (m.Name != "SetArcana") continue;
				var ps = m.GetParameters();
				if (ps.Length >= 3 && ps[2].ParameterType == typeof(ArcanaType))
				{
					harmony.Patch(m, postfix: new HarmonyMethod(typeof(GrimoirePatches), nameof(CollectionItem_SetArcana_Postfix)));
					Plugin.Log.LogInfo("[Grimoire] Patched CollectionItemUI.SetArcana");
					break;
				}
			}
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Grimoire] CollectionItemUI.SetArcana patch: " + ex.Message);
		}
	}

	/// <summary>Register the *child icon* returned by AddWeaponIcon, not the EvolutionItemUI row.</summary>
	public static void AddWeaponIcon_Postfix(EvolutionItemUI __instance, WeaponType __0, GameObject __result)
	{
		try
		{
			if ((Object)(object)__result == (Object)null)
			{
				return;
			}
			// Prefer the Image child for tighter hitboxes
			GameObject hit = FindBestHitTarget(__result);
			int id = ((Object)hit).GetInstanceID();
			ItemTooltipsMod.RegisterWeaponUI(id, hit, __0, isAddMethod: false);
			if (Plugin.DebugVerbose)
			{
				Plugin.Dbg($"Grimoire AddWeaponIcon -> {__0} on {((Object)hit).name} id={id} parent={((Object)__instance)?.name}");
			}
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Grimoire] AddWeaponIcon postfix: " + ex.Message);
		}
	}

	public static void CollectionItem_SetData_Postfix(CollectionItemUI __instance, WeaponType _wType)
	{
		try
		{
			if ((Object)(object)__instance == (Object)null) return;
			GameObject go = ((Component)__instance).gameObject;
			GameObject hit = FindBestHitTarget(go);
			ItemTooltipsMod.RegisterWeaponUI(((Object)hit).GetInstanceID(), hit, _wType, isAddMethod: false);
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Grimoire] CollectionItem SetData: " + ex.Message);
		}
	}

	public static void CollectionItem_SetItem_Postfix(CollectionItemUI __instance, ItemType _item)
	{
		try
		{
			if ((Object)(object)__instance == (Object)null) return;
			GameObject go = ((Component)__instance).gameObject;
			GameObject hit = FindBestHitTarget(go);
			ItemTooltipsMod.RegisterItemUI(((Object)hit).GetInstanceID(), hit, _item, isAddMethod: false);
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Grimoire] CollectionItem SetItem: " + ex.Message);
		}
	}

	public static void CollectionItem_SetArcana_Postfix(CollectionItemUI __instance, ArcanaType type)
	{
		try
		{
			if ((Object)(object)__instance == (Object)null) return;
			GameObject go = ((Component)__instance).gameObject;
			GameObject hit = FindBestHitTarget(go);
			ItemTooltipsMod.RegisterArcanaUI(((Object)hit).GetInstanceID(), hit, type);
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Grimoire] CollectionItem SetArcana: " + ex.Message);
		}
	}

	/// <summary>Use an Image child when present so hit-tests match the visible sprite, not a wide panel.</summary>
	private static GameObject FindBestHitTarget(GameObject root)
	{
		if ((Object)(object)root == (Object)null) return root;
		try
		{
			// Prefer named icon images
			string[] names = { "UnlockedIcon", "Icon", "icon", "WeaponIcon", "ItemIcon", "Image" };
			foreach (string n in names)
			{
				Transform t = root.transform.Find(n);
				if ((Object)(object)t != (Object)null)
				{
					var img = t.GetComponent<UnityEngine.UI.Image>();
					if ((Object)(object)img != (Object)null && (Object)(object)img.sprite != (Object)null)
						return t.gameObject;
				}
			}
			// Any child Image with a sprite (skip backgrounds)
			var images = root.GetComponentsInChildren<UnityEngine.UI.Image>(true);
			if (images != null)
			{
				GameObject best = null;
				float bestArea = float.MaxValue;
				for (int i = 0; i < images.Count; i++)
				{
					var img = images[i];
					if ((Object)(object)img == (Object)null || (Object)(object)img.sprite == (Object)null) continue;
					string nm = ((Object)img).name.ToLowerInvariant();
					if (nm.Contains("background") || nm.Contains("frame") || nm.Contains("panel") || nm.Contains("highlight") || nm.Contains("seal"))
						continue;
					var rt = img.rectTransform;
					if ((Object)(object)rt == (Object)null) continue;
					float area = Mathf.Abs(rt.rect.width * rt.rect.height);
					if (area < 4f) continue;
					if (area < bestArea)
					{
						bestArea = area;
						best = ((Component)img).gameObject;
					}
				}
				if ((Object)(object)best != (Object)null)
					return best;
			}
		}
		catch
		{
		}
		return root;
	}
}
