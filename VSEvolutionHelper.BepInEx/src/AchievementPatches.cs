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

	/// <summary>
	/// Placement in Safe Area reference units. Bottom-centre pivot, so the panel grows upward
	/// and stays put as the reward list changes length.
	/// </summary>
	private const float PopupX = 560f;
	private const float PopupY = -568f;

	public static void Apply(Harmony harmony)
	{
		if (!Plugin.AchievementTooltipsEnabled)
		{
			Plugin.Log.LogInfo("[Achievements] Disabled by config");
			return;
		}
		try
		{
			int patched = 0;
			foreach (var m in typeof(AchievementDataUI).GetMethods(
				System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
			{
				if (m.Name != "SetData") continue;
				harmony.Patch(m, postfix: new HarmonyMethod(typeof(AchievementPatches), nameof(SetData_Postfix)));
				patched++;
			}
			if (patched > 0) Plugin.Log.LogInfo($"[Achievements] Patched AchievementDataUI.SetData x{patched}");
			else Plugin.Log.LogWarning("[Achievements] AchievementDataUI.SetData not found");
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Achievements] patch: " + ex.Message);
		}
	}

	/// <summary>
	/// Instance-only postfix — the achievement is read back off the row rather than taken from
	/// the patched call's arguments, matching the other IL2CPP postfixes here. There are two
	/// SetData overloads (normal and adventure) and this serves both.
	/// </summary>
	public static void SetData_Postfix(AchievementDataUI __instance)
	{
		try
		{
			if ((Object)(object)__instance == (Object)null) return;

			string id = null;
			try { id = __instance._type.ToString(); } catch { }
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
				Offset = new Vector2(PopupX, PopupY),
				Pivot = new Vector2(0.5f, 0f),
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
