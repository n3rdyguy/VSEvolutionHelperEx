using System;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using VampireSurvivors.Data;
using VampireSurvivors.Data.Items;
using VampireSurvivors.Data.Weapons;
using VampireSurvivors.UI;
using Object = UnityEngine.Object;

namespace VSItemTooltips;

/// <summary>
/// Grimoire / Collections evolution formula rows use EvolutionItemUI.AddWeaponIcon(WeaponType)
/// which creates a child icon GameObject per ingredient/result. Generic patches only saw the
/// parent EvolutionItemUI instance, so every icon in a row shared one InstanceID and only
/// the last WeaponType stuck — hence "only the middle (or one) icon has a tooltip".
///
/// Hit targets: register the full icon root (not a tiny nested sprite), enable raycasts, and
/// map all graphic children to the same weapon so the whole icon is hoverable.
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

	/// <summary>
	/// Register the full icon root from AddWeaponIcon (not a tiny nested sprite).
	/// Nested Image-only targets caused hover only near the formula center / + sign.
	/// </summary>
	public static void AddWeaponIcon_Postfix(EvolutionItemUI __instance, WeaponType __0, GameObject __result)
	{
		try
		{
			if ((Object)(object)__result == (Object)null)
				return;

			// Full icon cell — hit area matches the layout slot, not just the pixel sprite
			PrepareIconHitArea(__result);
			int id = ((Object)__result).GetInstanceID();
			ItemTooltipsMod.RegisterWeaponUI(id, __result, __0, isAddMethod: false);

			// Also map every graphic child under this icon so raycasts on frames/sprites resolve
			RegisterChildGraphicsAsWeapon(__result, __0);

			if (Plugin.DebugVerbose)
			{
				Plugin.Dbg($"Grimoire AddWeaponIcon -> {__0} on {((Object)__result).name} id={id}");
			}
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Grimoire] AddWeaponIcon postfix: " + ex.Message);
		}
	}

	/// <summary>
	/// Instance-only postfix — read types from the UI to avoid IL2CPP Harmony arg marshaling crashes.
	/// </summary>
	public static void CollectionItem_SetData_Postfix(CollectionItemUI __instance)
	{
		try
		{
			if ((Object)(object)__instance == (Object)null) return;
			GameObject go = ((Component)__instance).gameObject;
			WeaponType wt = __instance.GetWeaponType();
			if (!GameData.IsRealWeaponType(wt)) return;
			PrepareIconHitArea(go);
			ItemTooltipsMod.RegisterWeaponUI(((Object)go).GetInstanceID(), go, wt, isAddMethod: false);
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Grimoire] CollectionItem SetData: " + ex.Message);
		}
	}

	public static void CollectionItem_SetItem_Postfix(CollectionItemUI __instance)
	{
		try
		{
			if ((Object)(object)__instance == (Object)null) return;
			GameObject go = ((Component)__instance).gameObject;
			ItemType it = __instance.GetItemType();
			PrepareIconHitArea(go);
			ItemTooltipsMod.RegisterItemUI(((Object)go).GetInstanceID(), go, it, isAddMethod: false);
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Grimoire] CollectionItem SetItem: " + ex.Message);
		}
	}

	public static void CollectionItem_SetArcana_Postfix(CollectionItemUI __instance)
	{
		try
		{
			if ((Object)(object)__instance == (Object)null) return;
			GameObject go = ((Component)__instance).gameObject;
			ArcanaType at = __instance.GetArcanaType();
			PrepareIconHitArea(go);
			ItemTooltipsMod.RegisterArcanaUI(((Object)go).GetInstanceID(), go, at);
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Grimoire] CollectionItem SetArcana: " + ex.Message);
		}
	}

	/// <summary>
	/// Ensure the icon root can receive hover: raycastable graphics + transparent full-rect
	/// Image if the root itself has no Graphic (common for layout cells).
	/// </summary>
	private static void PrepareIconHitArea(GameObject root)
	{
		if ((Object)(object)root == (Object)null) return;
		try
		{
			// Enable raycasts on icon images (skip huge backgrounds)
			var graphics = root.GetComponentsInChildren<Graphic>(true);
			if (graphics != null)
			{
				for (int i = 0; i < graphics.Count; i++)
				{
					var g = graphics[i];
					if ((Object)(object)g == (Object)null) continue;
					string nm = ((Object)g).name.ToLowerInvariant();
					if (nm.Contains("background") || nm.Contains("panel") || nm.Contains("scroll"))
						continue;
					try { g.raycastTarget = true; } catch { }
				}
			}

			// If root has no Graphic, add a nearly-invisible hit plate so the full cell is hoverable
			var rootGraphic = root.GetComponent<Graphic>();
			if ((Object)(object)rootGraphic == (Object)null)
			{
				var img = root.AddComponent<Image>();
				img.color = new Color(1f, 1f, 1f, 0.01f);
				img.raycastTarget = true;
			}
			else
			{
				try { rootGraphic.raycastTarget = true; } catch { }
			}
		}
		catch { }
	}

	private static void RegisterChildGraphicsAsWeapon(GameObject root, WeaponType type)
	{
		if ((Object)(object)root == (Object)null) return;
		try
		{
			var graphics = root.GetComponentsInChildren<Graphic>(true);
			if (graphics == null) return;
			int rootId = ((Object)root).GetInstanceID();
			for (int i = 0; i < graphics.Count; i++)
			{
				var g = graphics[i];
				if ((Object)(object)g == (Object)null) continue;
				GameObject child = ((Component)g).gameObject;
				int id = ((Object)child).GetInstanceID();
				if (id == rootId) continue;
				// Only map direct visual children of this icon, not the whole grimoire tree
				if (!IsUnder(child.transform, root.transform)) continue;
				ItemTooltipsMod.RegisterWeaponUI(id, child, type, isAddMethod: false);
			}
		}
		catch { }
	}

	private static void RegisterChildGraphicsAsItem(GameObject root, ItemType type)
	{
		if ((Object)(object)root == (Object)null) return;
		try
		{
			var graphics = root.GetComponentsInChildren<Graphic>(true);
			if (graphics == null) return;
			int rootId = ((Object)root).GetInstanceID();
			for (int i = 0; i < graphics.Count; i++)
			{
				var g = graphics[i];
				if ((Object)(object)g == (Object)null) continue;
				GameObject child = ((Component)g).gameObject;
				int id = ((Object)child).GetInstanceID();
				if (id == rootId) continue;
				if (!IsUnder(child.transform, root.transform)) continue;
				ItemTooltipsMod.RegisterItemUI(id, child, type, isAddMethod: false);
			}
		}
		catch { }
	}

	private static bool IsUnder(Transform t, Transform ancestor)
	{
		while ((Object)(object)t != (Object)null)
		{
			if ((Object)(object)t == (Object)(object)ancestor) return true;
			t = t.parent;
		}
		return false;
	}
}
