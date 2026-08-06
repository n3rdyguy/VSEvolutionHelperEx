using System;
using HarmonyLib;
using UnityEngine;
using VampireSurvivors.Data;
using VampireSurvivors.UI;
using Object = UnityEngine.Object;

namespace VSItemTooltips;

/// <summary>
/// Power Up page: show what an upgrade costs to finish, on hover.
///
/// The page shows a name, a description and one price - the next level - and only for the
/// upgrade currently selected. What it never shows is the rest of the ladder: every level is
/// its own record with its own price, so "what does maxing this cost" is answerable from data
/// the game already has, and is otherwise arithmetic the player does by hand while comparing
/// two upgrades.
///
/// Instance-only postfix, matching the other IL2CPP patches here: the data is read back off the
/// row rather than taken from the patched call's arguments.
/// </summary>
public static class PowerUpPatches
{
	private static readonly RowTooltipRegistry Rows = new RowTooltipRegistry("PowerUps");

	public static void Apply(Harmony harmony)
	{
		if (!Plugin.PowerUpTooltipsEnabled)
		{
			Plugin.Log.LogInfo("[PowerUps] Disabled by config");
			return;
		}
		try
		{
			int patched = 0;
			foreach (var m in typeof(PowerUpItemUI).GetMethods(
				System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
			{
				// SetData binds the row; UpdateAfterPurchase is what moves the level on, and
				// without it a bought level leaves the tooltip quoting the old remaining cost.
				if (m.Name != "SetData" && m.Name != "UpdateAfterPurchase") continue;
				harmony.Patch(m, postfix: new HarmonyMethod(typeof(PowerUpPatches), nameof(SetData_Postfix)));
				Plugin.Log.LogInfo($"[PowerUps] Patched PowerUpItemUI.{m.Name}({m.GetParameters().Length} args)");
				patched++;
			}
			if (patched == 0) Plugin.Log.LogWarning("[PowerUps] PowerUpItemUI.SetData not found");

			// Buying does not move the pointer, so nothing re-triggers the tooltip that is
			// already open - and its numbers changed the moment the purchase went through.
			foreach (var m in typeof(PowerUpsPage).GetMethods(
				System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
			{
				if (m.Name != "Purchase" && m.Name != "RefundPowerUps" && m.Name != "ResetAll") continue;
				harmony.Patch(m, postfix: new HarmonyMethod(typeof(PowerUpPatches), nameof(Purchase_Postfix)));
				Plugin.Log.LogInfo($"[PowerUps] Patched PowerUpsPage.{m.Name} for tooltip refresh");
			}
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[PowerUps] patch: " + ex.Message);
		}
	}

	public static void Purchase_Postfix()
	{
		try { Rows.Refresh(); }
		catch (Exception ex) { Plugin.Dbg("[PowerUps] refresh: " + ex.Message); }
	}

	public static void SetData_Postfix(PowerUpItemUI __instance)
	{
		try
		{
			if ((Object)(object)__instance == (Object)null) return;

			PowerUpType type;
			try { type = __instance._type; }
			catch { return; }

			int maxLevel = 0;
			try { maxLevel = __instance._maxRank; }
			catch { }

			var page = __instance._page;
			var rows = BuildRows(page, type, maxLevel, out string description);
			if (Plugin.DebugVerbose) LogPricing(__instance, type, maxLevel);
			if ((rows == null || rows.Count == 0) && string.IsNullOrEmpty(description))
			{
				if (Plugin.DebugVerbose) Plugin.Dbg("PowerUps: nothing to show for " + type);
				return;
			}

			GameObject root = ((Component)__instance).gameObject;

			Rows.Register(root, IconObject(__instance), new RowTooltipRegistry.Entry
			{
				Title = ResolveTitle(__instance, type),
				Description = description,
				Sprite = ResolveSprite(__instance, type),
				Rows = rows,
				// Recomputed on every hover. A purchase raises the surcharge on the whole page,
				// so rows worked out once here are stale for every upgrade, not just the one
				// that was bought.
				RowsProvider = () => BuildRows(page, type, maxLevel, out _),
				// The same side panel Collections and Unlocks use, so the pages read alike.
				Offset = new Vector2(ItemTooltipsMod.SidePanelX, ItemTooltipsMod.SidePanelTopY),
				Pivot = ItemTooltipsMod.SidePanelPivot,
			});

			if (Plugin.DebugVerbose)
				Plugin.Dbg($"PowerUps: registered {rows.Count} rows for {type}");
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[PowerUps] SetData postfix: " + ex.Message);
		}
	}

	/// <summary>
	/// The rows for one upgrade, from live player stats.
	///
	/// Both the owned level and the next price come from PlayerStats rather than from the row:
	/// PowerUpItemUI._currentLevel read 0 for every upgrade regardless of what was owned, and
	/// pricing keys off the player's actual stats anyway.
	/// </summary>
	private static System.Collections.Generic.List<GameData.IconRow> BuildRows(
		PowerUpsPage page, PowerUpType type, int maxLevel, out string description)
	{
		description = null;
		int level = 0;
		float nextPrice = 0f;
		try
		{
			var stats = page._playerStats;
			if (stats != null)
			{
				nextPrice = stats.GetPrice(type);
				var owned = stats.GetOwnedPowerUps();
				if (owned != null && owned.ContainsKey(type))
				{
					var stat = owned[type];
					if (stat != null) level = stat._Level;
				}
			}
		}
		catch (Exception ex)
		{
			if (Plugin.DebugVerbose) Plugin.Dbg("[PowerUps] player stats: " + ex.Message);
		}
		return GameData.GetPowerUpRows(type, level, maxLevel, nextPrice, out description);
	}

	/// <summary>
	/// Everything that feeds a price, in one line. The level the row reports and the level the
	/// player's own stats report are logged side by side, because a projection built on the
	/// wrong one is wrong everywhere except the next purchase.
	/// </summary>
	private static void LogPricing(PowerUpItemUI ui, PowerUpType type, int maxLevel)
	{
		try
		{
			int level = -1;
			float next = -1f;
			try { level = ui._currentLevel; } catch { }
			double markup = -1d;
			double baseMarkup = -1d;
			int owned = -1, ownedLevel = -1;
			try
			{
				var stats = ui._page._playerStats;
				if (stats != null)
				{
					try { markup = stats.PowerUpMarkUp; } catch { }
					try { next = stats.GetPrice(type); } catch { }
					var dict = stats.GetOwnedPowerUps();
					if (dict != null)
					{
						owned = dict.Count;
						try
						{
							if (dict.ContainsKey(type))
							{
								var stat = dict[type];
								if (stat != null)
								{
									ownedLevel = stat._Level;
									baseMarkup = VampireSurvivors.PlayerStat.BASE_MARKUP;
								}
							}
						}
						catch { }
					}
				}
			}
			catch { }

			int basePrice = -1;
			try
			{
				var data = GameData.GetPowerUpData(type);
				if (data != null) basePrice = data.price;
			}
			catch { }

			Plugin.Dbg($"[PowerUps] {type}: rowLevel={level} statsLevel={ownedLevel} max={maxLevel} "
				+ $"next={next} base={basePrice} markup={markup} baseMarkup={baseMarkup} ownedKinds={owned}");
		}
		catch { }
	}

	/// <summary>
	/// The row's own label first, since the game has already localized it; the typed name is
	/// the fallback, and a humanized id the last resort so no raw term reaches the tooltip.
	/// </summary>
	private static string ResolveTitle(PowerUpItemUI ui, PowerUpType type)
	{
		// Title is an I2 Localize component, not the label itself - the rendered text lives on
		// the TMP component sharing its object, and only that has been through localization.
		try
		{
			var loc = ui.Title;
			if ((Object)(object)loc != (Object)null)
			{
				var tmp = ((Component)loc).gameObject.GetComponent<TMPro.TextMeshProUGUI>();
				if ((Object)(object)tmp != (Object)null)
				{
					string t = ((TMPro.TMP_Text)tmp).text;
					if (!string.IsNullOrWhiteSpace(t) && !GameData.LooksLikeLocKey(t)) return t.Trim();
				}
			}
		}
		catch { }
		try
		{
			string n = GameData.GetPowerUpName(type);
			if (!string.IsNullOrWhiteSpace(n) && !GameData.LooksLikeLocKey(n)) return n.Trim();
		}
		catch { }
		return GameData.HumanizeId(type.ToString());
	}

	/// <summary>The row's drawn icon, falling back to the one resolved from data.</summary>
	private static Sprite ResolveSprite(PowerUpItemUI ui, PowerUpType type)
	{
		try
		{
			var img = ui.Icon;
			if ((Object)(object)img != (Object)null && (Object)(object)img.sprite != (Object)null)
				return img.sprite;
		}
		catch { }
		try { return GameData.GetSprite(type); }
		catch { return null; }
	}

	/// <summary>
	/// The icon child is registered alongside the root: a selected row draws a highlight over
	/// its contents that swallows the pointer before it reaches the row itself.
	/// </summary>
	private static GameObject IconObject(PowerUpItemUI ui)
	{
		try
		{
			var img = ui.Icon;
			if ((Object)(object)img != (Object)null) return ((Component)img).gameObject;
		}
		catch { }
		return null;
	}

	public static void Clear()
	{
		Rows.Clear();
	}
}
