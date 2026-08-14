using System;
using HarmonyLib;
using UnityEngine;
using VampireSurvivors.UI;
using Object = UnityEngine.Object;

namespace VSItemTooltips;

/// <summary>
/// Achievements page: show what each achievement unlocks.
///
/// The page lists a condition and a tick, and reveals the reward only in a side panel for the
/// selected row. Rewards are read from the raw achievements JSON so every row is covered on
/// hover, without needing it selected first.
/// </summary>
public static class AchievementPatches
{
	private static readonly RowTooltipRegistry Rows = new RowTooltipRegistry("Achievements");

	// Same panel slot Collections uses, so the two pages read the same way.

	public static void Apply(Harmony harmony)
	{
		if (!Plugin.AchievementTooltipsEnabled)
		{
			Plugin.Log.LogInfo("[Achievements] Disabled by config");
			return;
		}
		try
		{
			// SetData bound but never fired: the page binds its rows through Init. Both are
			// patched rather than swapping one for the other, since either may be the entry
			// point depending on how a row is created.
			int patched = 0;
			foreach (var m in typeof(AchievementDataUI).GetMethods(
				System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
			{
				if (m.Name != "SetData" && m.Name != "Init") continue;
				harmony.Patch(m, postfix: new HarmonyMethod(typeof(AchievementPatches), nameof(SetData_Postfix)));
				Plugin.Log.LogInfo($"[Achievements] Patched AchievementDataUI.{m.Name}({m.GetParameters().Length} args)");
				patched++;
			}
			if (patched == 0) Plugin.Log.LogWarning("[Achievements] No AchievementDataUI bind method found");
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Achievements] patch: " + ex.Message);
		}
	}

	/// <summary>
	/// Instance-only postfix - the achievement is read back off the row rather than taken from
	/// the patched call's arguments, matching the other IL2CPP postfixes here. There are two
	/// SetData overloads (normal and adventure) and this serves both.
	/// </summary>
	public static void SetData_Postfix(AchievementDataUI __instance)
	{
		try
		{
			if ((Object)(object)__instance == (Object)null) return;

			// Rows are recycled. Init carries no AchievementType and can run while _type still
			// holds the previous row's value - which made every hovered row inherit Wings' reward.
			// The freshly bound record carries its own type, so it must win; _type is only a
			// fallback for the adventure overloads whose record does not expose a normal id.
			string id = null;
			try { id = __instance._data.Type.ToString(); } catch { }
			if (string.IsNullOrEmpty(id) || id == "0" || id == "VOID")
			{
				try { id = __instance._type.ToString(); } catch { }
			}
			if (string.IsNullOrEmpty(id)) return;

			GameData.DumpAchievementJsonOnce(id);

			var rows = GameData.GetAchievementRows(id, out string description);
			if ((rows == null || rows.Count == 0) && string.IsNullOrEmpty(description))
			{
				if (Plugin.DebugVerbose) Plugin.Dbg("Achievements: nothing to show for " + id);
				return;
			}

			GameObject root = ((Component)__instance).gameObject;

			Rows.Register(root, IconObject(__instance), new RowTooltipRegistry.Entry
			{
				Title = ResolveTitle(__instance, id),
				Description = description,
				Sprite = IconSprite(__instance),
				Rows = rows,
				Offset = new Vector2(ItemTooltipsMod.SidePanelX, ItemTooltipsMod.SidePanelTopY),
				Pivot = ItemTooltipsMod.SidePanelPivot,
			});

			if (Plugin.DebugVerbose)
				Plugin.Dbg($"Achievements: registered {rows.Count} rows for {id}");
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Achievements] SetData postfix: " + ex.Message);
		}
	}

	/// <summary>The row's own label, which the game has already localized.</summary>
	private static string ResolveTitle(AchievementDataUI item, string id)
	{
		try
		{
			var label = item.Label;
			if ((Object)(object)label != (Object)null)
			{
				string t = ((TMPro.TMP_Text)label).text;
				if (!string.IsNullOrWhiteSpace(t) && !GameData.LooksLikeLocKey(t)) return t.Trim();
			}
		}
		catch { }
		return GameData.HumanizeId(id);
	}

	private static GameObject IconObject(AchievementDataUI item)
	{
		try
		{
			var img = item.Icon;
			if ((Object)(object)img != (Object)null) return ((Component)img).gameObject;
		}
		catch { }
		return null;
	}

	private static Sprite IconSprite(AchievementDataUI item)
	{
		try
		{
			var img = item.Icon;
			if ((Object)(object)img != (Object)null) return img.sprite;
		}
		catch { }
		return null;
	}

	public static void Clear()
	{
		Rows.Clear();
	}
}
