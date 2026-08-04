using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using VampireSurvivors.Data;
using VampireSurvivors.UI;
using Object = UnityEngine.Object;

namespace VSItemTooltips;

/// <summary>
/// Pause-map tooltips for relics, floor pickups, chests, tokens, etc.
/// MapManager.SpawnItemOnMap(Sprite, Vector2, float) creates each icon;
/// we resolve type from sprite (and reflectively from world pickups after populate).
/// Avoids referencing Pickup's base ArcadeSprite/PauseSystem chain.
/// </summary>
public static class MapPatches
{
	public static void Apply(Harmony harmony)
	{
		try
		{
			var spawn = typeof(MapManager).GetMethod(
				"SpawnItemOnMap",
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
				null,
				new Type[] { typeof(Sprite), typeof(Vector2), typeof(float) },
				null);
			if (spawn != null)
			{
				harmony.Patch(spawn, postfix: new HarmonyMethod(typeof(MapPatches), nameof(SpawnItemOnMap_Postfix)));
				Plugin.Log.LogInfo("[Map] Patched MapManager.SpawnItemOnMap");
			}
			else
				Plugin.Log.LogWarning("[Map] SpawnItemOnMap not found");
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Map] SpawnItemOnMap patch: " + ex.Message);
		}

		try
		{
			var clear = typeof(MapManager).GetMethod(
				"ClearIcons",
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (clear != null)
			{
				harmony.Patch(clear, postfix: new HarmonyMethod(typeof(MapPatches), nameof(ClearIcons_Postfix)));
				Plugin.Log.LogInfo("[Map] Patched MapManager.ClearIcons");
			}
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Map] ClearIcons patch: " + ex.Message);
		}

		foreach (string name in new[] { "SetPickups", "Populate" })
		{
			try
			{
				var m = typeof(MapManager).GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (m != null)
				{
					harmony.Patch(m, postfix: new HarmonyMethod(typeof(MapPatches), nameof(AfterPopulate_Postfix)));
					Plugin.Log.LogInfo($"[Map] Patched MapManager.{name}");
				}
			}
			catch (Exception ex)
			{
				Plugin.Log.LogWarning($"[Map] {name} patch: " + ex.Message);
			}
		}
	}

	public static void SpawnItemOnMap_Postfix(MapManager __instance, Sprite s, Vector2 tPos, float scale, GameObject __result)
	{
		try
		{
			if (!Plugin.MapTooltipsEnabled)
				return;
			if ((Object)(object)__result == (Object)null)
				return;

			ItemType? item = null;
			WeaponType? weapon = null;
			string label = null;
			string desc = null;
			Sprite icon = s;

			if ((Object)(object)s != (Object)null)
			{
				string sn = ((Object)s).name;
				if (!GameData.TryResolveSprite(sn, out item, out weapon))
					label = GameData.HumanizeEnum(sn);
			}

			if (item.HasValue)
			{
				label = GameData.GetItemName(item.Value);
				desc = GameData.GetItemDescription(item.Value);
				icon = GameData.GetItemSprite(item.Value) ?? s;
			}
			else if (weapon.HasValue)
			{
				label = GameData.GetWeaponName(weapon.Value);
				desc = GameData.GetWeaponDescription(weapon.Value);
				icon = GameData.GetSprite(weapon.Value) ?? s;
			}

			if (string.IsNullOrEmpty(label) && (Object)(object)s != (Object)null)
				label = GameData.HumanizeEnum(((Object)s).name);

			ItemTooltipsMod.RegisterMapIcon(__result, item, weapon, label, desc, icon);

			if (Plugin.DebugVerbose)
				Plugin.Dbg($"Map icon: item={item} weapon={weapon} label={label} sprite={((Object)s)?.name}");
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Map] SpawnItemOnMap postfix: " + ex.Message);
		}
	}

	public static void ClearIcons_Postfix(MapManager __instance)
	{
		ItemTooltipsMod.ClearMapIcons();
	}

	public static void AfterPopulate_Postfix(MapManager __instance)
	{
		try
		{
			EnrichFromWorldPickupsReflect(__instance);
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Map] Enrich pickups: " + ex.Message);
		}
	}

	/// <summary>
	/// Reflectively read GetAllWorldItems() pickups (PickupType / SpriteName / frames)
	/// without compiling against Pickup's ArcadeSprite base type.
	/// </summary>
	private static void EnrichFromWorldPickupsReflect(MapManager map)
	{
		if ((Object)(object)map == (Object)null) return;
		GameData.EnsureLoaded();

		object pickupsObj;
		try
		{
			var m = typeof(MapManager).GetMethod("GetAllWorldItems", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (m == null) return;
			pickupsObj = m.Invoke(map, null);
		}
		catch
		{
			return;
		}
		if (pickupsObj == null) return;

		var byFrame = new Dictionary<string, ItemType>(StringComparer.OrdinalIgnoreCase);
		int count = 0;
		try
		{
			// Il2Cpp List: Count + Item[i]
			var listType = pickupsObj.GetType();
			var countProp = listType.GetProperty("Count");
			var itemProp = listType.GetProperty("Item");
			if (countProp == null || itemProp == null) return;
			int n = (int)countProp.GetValue(pickupsObj);
			count = n;
			for (int i = 0; i < n; i++)
			{
				object p = itemProp.GetValue(pickupsObj, new object[] { i });
				if (p == null) continue;
				ItemType type = default;
				bool haveType = false;
				try
				{
					var pt = p.GetType().GetProperty("PickupType", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (pt != null)
					{
						object v = pt.GetValue(p);
						if (v is ItemType it) { type = it; haveType = true; }
						else if (v != null && Enum.TryParse(v.ToString(), true, out ItemType it2)) { type = it2; haveType = true; }
					}
				}
				catch { }
				if (!haveType) continue;

				void add(string key)
				{
					if (string.IsNullOrEmpty(key)) return;
					if (!byFrame.ContainsKey(key)) byFrame[key] = type;
					string bare = key.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
						? key.Substring(0, key.Length - 4) : key;
					if (!byFrame.ContainsKey(bare)) byFrame[bare] = type;
				}

				try
				{
					var sn = p.GetType().GetProperty("SpriteName")?.GetValue(p)?.ToString();
					add(sn);
				}
				catch { }
				try
				{
					var f = p.GetType().GetField("_frameName", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.GetValue(p)?.ToString()
						?? p.GetType().GetProperty("_frameName")?.GetValue(p)?.ToString();
					add(f);
				}
				catch { }
				try
				{
					var mf = p.GetType().GetField("MapTokenFrameName", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.GetValue(p)?.ToString()
						?? p.GetType().GetProperty("MapTokenFrameName")?.GetValue(p)?.ToString();
					add(mf);
				}
				catch { }
			}
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Map] Reflect pickups: " + ex.Message);
			return;
		}

		ItemTooltipsMod.EnrichMapIconsFromItemTypes(byFrame);
		if (Plugin.DebugVerbose)
			Plugin.Dbg($"Map enrich: {count} world pickups, {byFrame.Count} frame keys, icons={ItemTooltipsMod.MapIconCount}");
	}
}
