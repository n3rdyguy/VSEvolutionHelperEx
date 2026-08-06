using System;
using HarmonyLib;
using UnityEngine;
using VampireSurvivors;
using Object = UnityEngine.Object;

namespace VSItemTooltips;

/// <summary>
/// Bestiary: show an enemy's real stats on hover.
///
/// The page lists enemies and a kill count, and shows nothing about what they actually do.
/// EnemyData carries HP, damage, speed, XP and — the reason this exists — per-effect
/// resistances (freeze, Rosary, debuffs, knockback, corridor, defang) plus a fire weakness.
/// None of that is surfaced anywhere in game, and "why won't this thing freeze" has no answer
/// without it.
///
/// Instance-only postfix, matching the other IL2CPP patches here: the data is read back off
/// the row rather than taken from the patched call's arguments.
/// </summary>
public static class BestiaryPatches
{
	private static readonly RowTooltipRegistry Rows = new RowTooltipRegistry("Bestiary");

	/// <summary>
	/// Placement in Safe Area reference units (1920x1200, so ±960 x ±600). With a bottom-centre
	/// pivot the Y value is the popup's bottom edge, which keeps it pinned near the screen
	/// bottom no matter how tall the trait list grows.
	/// </summary>
	private const float BestiaryPopupX = 560f;
	private const float BestiaryPopupY = -568f;

	public static void Apply(Harmony harmony)
	{
		if (!Plugin.BestiaryTooltipsEnabled)
		{
			Plugin.Log.LogInfo("[Bestiary] Disabled by config");
			return;
		}
		try
		{
			bool patchedData = false, patchedInfo = false;
			foreach (var m in typeof(EnemyItemUI).GetMethods(
				System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
			{
				if (!patchedData && m.Name == "SetData")
				{
					harmony.Patch(m, postfix: new HarmonyMethod(typeof(BestiaryPatches), nameof(SetData_Postfix)));
					Plugin.Log.LogInfo("[Bestiary] Patched EnemyItemUI.SetData");
					patchedData = true;
				}
			}
			if (!patchedData) Plugin.Log.LogWarning("[Bestiary] EnemyItemUI.SetData not found");

			// The page owns the info panel — EnemyItemUI.SetInfoPanel() never fired, because
			// the page calls its own overload.
			foreach (var m in typeof(VampireSurvivors.UI.BestiaryPage).GetMethods(
				System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
			{
				if (m.Name != "SetInfoPanel") continue;
				harmony.Patch(m, postfix: new HarmonyMethod(typeof(BestiaryPatches), nameof(PageInfoPanel_Postfix)));
				Plugin.Log.LogInfo("[Bestiary] Patched BestiaryPage.SetInfoPanel");
				patchedInfo = true;
				break;
			}
			if (!patchedInfo) Plugin.Log.LogWarning("[Bestiary] BestiaryPage.SetInfoPanel not found");
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Bestiary] patch: " + ex.Message);
		}
	}

	public static void SetData_Postfix(EnemyItemUI __instance)
	{
		try
		{
			if ((Object)(object)__instance == (Object)null) return;

			bool killed = false;
			try { killed = __instance._hasKilled; } catch { }
			if (!killed && !Plugin.BestiarySpoilers) return;

			VampireSurvivors.Data.Enemies.EnemyData data = null;
			try { data = __instance._data; } catch { }
			if (data == null) return;

			string enemyId = null;
			try { enemyId = __instance._type.ToString(); } catch { }

			VampireSurvivors.Data.EnemyType? type = null;
			try { type = __instance._type; } catch { }

			GameData.DumpEnemyJsonOnce(enemyId);

			GameData.EnemyInfo info = GameData.GetEnemyInfo(data, enemyId, type, ResolveName(__instance));
			if (info == null || info.Rows == null || info.Rows.Count == 0)
			{
				if (Plugin.DebugVerbose) Plugin.Dbg("Bestiary: no readable stats for " + Describe(__instance));
				return;
			}

			GameObject root = ((Component)__instance).gameObject;

			Rows.Register(root, null, new RowTooltipRegistry.Entry
			{
				Title = ResolveName(__instance),
				Sprite = ResolveSprite(data),
				Rows = info.Rows,
				SectionHeader = "Stats:",
				// Bottom of the screen, right of centre: the Bestiary list runs the full height
				// on the left, so the default upper dock sat across it.
				Offset = new Vector2(BestiaryPopupX, BestiaryPopupY),
				Pivot = new Vector2(0.5f, 0f),
			});

			if (Plugin.DebugVerbose)
				Plugin.Dbg($"Bestiary: registered {info.Rows.Count} rows for {Describe(__instance)} killed={killed}");
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Bestiary] SetData postfix: " + ex.Message);
		}
	}

	/// <summary>
	/// The game works out which stages an enemy appears in and writes it into the Bestiary's
	/// own Found In label. Capture that rather than deriving a stage list — a derived one
	/// disagreed with the game and there is no way for a reader to tell which is right.
	///
	/// The panel is populated for the selected enemy, so the cache fills as rows are selected;
	/// a row that has never been selected simply omits the section.
	/// </summary>
	public static void PageInfoPanel_Postfix(VampireSurvivors.UI.BestiaryPage __instance)
	{
		try
		{
			if ((Object)(object)__instance == (Object)null) return;

			string title = Text(__instance._Title);
			if (string.IsNullOrWhiteSpace(title)) return;

			var panel = new GameData.EnemyPanel
			{
				Hp = Text(__instance._HP),
				Power = Text(__instance._Power),
				Speed = Text(__instance._Speed),
				Resistances = Text(__instance._Resistances),
				Skills = Text(__instance._Skills),
				FoundIn = Text(__instance._FoundIn),
			};
			GameData.SetEnemyPanel(title, panel);
			Plugin.Dbg($"Bestiary: panel '{title}' hp='{panel.Hp}' pow='{panel.Power}' spd='{panel.Speed}' "
				+ $"res='{panel.Resistances}' found='{panel.FoundIn}'");
		}
		catch (Exception ex)
		{
			Plugin.Dbg("[Bestiary] page info postfix: " + ex.Message);
		}
	}

	private static string Text(TMPro.TextMeshProUGUI label)
	{
		try
		{
			if ((Object)(object)label == (Object)null) return null;
			string t = ((TMPro.TMP_Text)label).text;
			return string.IsNullOrWhiteSpace(t) ? null : t.Trim();
		}
		catch { return null; }
	}

	/// <summary>
	/// Prefer the row's own label, which the game has already localized. Undiscovered enemies
	/// render as a row of dashes, though, so that placeholder is rejected and the name is
	/// rebuilt from the enum id — otherwise revealing the stats but not the name would be a
	/// half-spoiler that helps nobody.
	/// </summary>
	private static string ResolveName(EnemyItemUI item)
	{
		// The record's own bName is what the Bestiary prints, and it is present even for rows
		// the list still renders as dashes.
		try
		{
			string b = GameData.GetEnemyName(item._type.ToString());
			if (!string.IsNullOrWhiteSpace(b) && !IsPlaceholder(b)) return b;
		}
		catch { }
		try
		{
			var label = item._Name;
			if ((Object)(object)label != (Object)null)
			{
				string t = ((TMPro.TMP_Text)label).text;
				if (!string.IsNullOrWhiteSpace(t) && !GameData.LooksLikeLocKey(t) && !IsPlaceholder(t))
					return t.Trim();
			}
		}
		catch { }
		try
		{
			string id = item._type.ToString();
			string loc = GameData.LocalizeTypedDescription(id, "name");
			if (!string.IsNullOrWhiteSpace(loc) && !GameData.LooksLikeLocKey(loc) && !IsPlaceholder(loc))
				return loc.Trim();
			return GameData.HumanizeId(id);
		}
		catch { }
		return "Enemy";
	}

	/// <summary>A locked row's name is drawn as dashes rather than left blank.</summary>
	private static bool IsPlaceholder(string s)
	{
		string t = s.Trim();
		if (t.Length == 0) return true;
		foreach (char c in t)
		{
			if (c != '-' && c != '?' && c != '_' && c != '.' && c != ' ') return false;
		}
		return true;
	}

	/// <summary>
	/// The enemy's own sprite, from the first animation frame. Locked rows draw a placeholder,
	/// so this is taken from the data rather than from whatever the row happens to show.
	/// </summary>
	private static Sprite ResolveSprite(VampireSurvivors.Data.Enemies.EnemyData data)
	{
		try
		{
			var frames = data.frameNames;
			if (frames != null && frames.Count > 0)
			{
				for (int i = 0; i < frames.Count; i++)
				{
					if (string.IsNullOrEmpty(frames[i])) continue;
					Sprite s = GameData.LoadSprite(frames[i], data.textureName);
					if ((Object)(object)s != (Object)null) return s;
				}
			}
		}
		catch { }
		return null;
	}

	private static string Describe(EnemyItemUI item)
	{
		try { return item._type.ToString(); } catch { return "?"; }
	}

	public static void Clear()
	{
		Rows.Clear();
	}
}
