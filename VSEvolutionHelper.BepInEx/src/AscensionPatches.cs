using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using VampireSurvivors;
using VampireSurvivors.Data;
using VampireSurvivors.UI;
using Object = UnityEngine.Object;

namespace VSItemTooltips;

/// <summary>
/// Adventure Ascension: explain the shared Ascension Points controls.
///
/// The same panel is opened from the Adventures screen and from an active adventure's upper-left
/// menu. Its four icons only show a number and +/- buttons, leaving both the per-point increment
/// and the already committed allocation hidden. Read those live from AdjustValuePanel so a point
/// added while the pointer stays over the control is reflected without leaving the panel.
/// </summary>
public static class AscensionPatches
{
	private static readonly RowTooltipRegistry Rows = new RowTooltipRegistry("Ascension");

	public static void Apply(Harmony harmony)
	{
		if (!Plugin.AdventureTooltipsEnabled)
		{
			Plugin.Log.LogInfo("[Ascension] Disabled by AdventureTooltips config");
			return;
		}

		try
		{
			Patch(harmony, typeof(AscensionButton), "SetAdventure", nameof(AscensionButton_Postfix));
			Patch(harmony, typeof(AscensionPanel), "SetData", nameof(SetData_Postfix));
			Patch(harmony, typeof(AscensionPanel), "RefreshData", nameof(Refresh_Postfix));

			// The pointer does not leave a +/- button after it is clicked. Refreshing the open
			// panel here makes its allocation and bonus move with the game control instead of
			// making the player hover away and back to see the new values.
			Patch(harmony, typeof(AdjustValuePanel), "IncrementUp", nameof(Refresh_Postfix));
			Patch(harmony, typeof(AdjustValuePanel), "IncrementDown", nameof(Refresh_Postfix));
			Patch(harmony, typeof(AdjustValuePanel), "SetValue", nameof(Refresh_Postfix));
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Ascension] patch: " + ex.Message);
		}
	}

	private static void Patch(Harmony harmony, Type type, string methodName, string postfixName)
	{
		MethodInfo method = type.GetMethod(methodName,
			BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		if (method == null)
		{
			Plugin.Log.LogWarning($"[Ascension] {type.Name}.{methodName} not found");
			return;
		}
		harmony.Patch(method, postfix: new HarmonyMethod(typeof(AscensionPatches), postfixName));
		Plugin.Log.LogInfo($"[Ascension] Patched {type.Name}.{methodName}({method.GetParameters().Length} args)");
	}

	public static void AscensionButton_Postfix(AscensionButton __instance)
	{
		try
		{
			if ((Object)(object)__instance == (Object)null) return;
			Rows.Register(((Component)__instance).gameObject, null, new RowTooltipRegistry.Entry
			{
				Title = "Ascension Points",
				Description = "Complete an Adventure to earn Ascension Points. Spend them here to boost Luck, Growth, Greed and Curse for that Adventure.",
			});
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Ascension] button postfix: " + ex.Message);
		}
	}

	public static void SetData_Postfix(AscensionPanel __instance)
	{
		try
		{
			if ((Object)(object)__instance == (Object)null) return;
			Register(__instance._LuckPanel, PowerUpType.LUCK, __instance);
			Register(__instance._GrowthPanel, PowerUpType.GROWTH, __instance);
			Register(__instance._GreedPanel, PowerUpType.GREED, __instance);
			Register(__instance._CursePanel, PowerUpType.CURSE, __instance);
			Plugin.Dbg("Ascension: registered 4 point controls");
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Ascension] SetData postfix: " + ex.Message);
		}
	}

	public static void Refresh_Postfix()
	{
		try { Rows.Refresh(); }
		catch (Exception ex) { Plugin.Dbg("[Ascension] refresh: " + ex.Message); }
	}

	private static void Register(AdjustValuePanel control, PowerUpType type, AscensionPanel panel)
	{
		if ((Object)(object)control == (Object)null) return;

		GameObject root = ((Component)control).gameObject;
		Rows.Register(root, IconObject(control), new RowTooltipRegistry.Entry
		{
			Title = GameData.GetPowerUpName(type),
			Description = Description(type),
			SpriteProvider = () => Sprite(control, type),
			RowsProvider = () => BuildRows(control, panel),
		});
	}

	private static List<GameData.IconRow> BuildRows(AdjustValuePanel control, AscensionPanel panel)
	{
		var rows = new List<GameData.IconRow>();
		int assigned = 0;
		int available = -1;
		float increment = 0f;
		string suffix = null;
		string displayed = null;

		try { assigned = control._pointsAssigned; } catch { }
		try { increment = control._IncrementAmount; } catch { }
		try { suffix = control._Suffix; } catch { }
		try { displayed = control._ValueText.text; } catch { }
		try { available = panel._completionCount - panel._currentSpend; } catch { }

		rows.Add(GameData.IconRow.Header("Ascension:"));
		rows.Add(new GameData.IconRow(null, $"Assigned: {assigned}"));
		if (!string.IsNullOrWhiteSpace(displayed))
			rows.Add(new GameData.IconRow(null, "Current bonus: " + displayed.Trim()));
		if (increment != 0f)
			rows.Add(new GameData.IconRow(null, $"Each point: {Signed(increment)}{suffix}"));
		if (available >= 0)
			rows.Add(new GameData.IconRow(null, $"Unspent points: {available}"));
		return rows;
	}

	private static string Description(PowerUpType type)
	{
		switch (type)
		{
			case PowerUpType.LUCK: return "Raises Luck, improving favorable rolls and the chance for fortunate outcomes.";
			case PowerUpType.GROWTH: return "Raises Growth, increasing experience gained.";
			case PowerUpType.GREED: return "Raises Greed, increasing gold earned.";
			case PowerUpType.CURSE: return "Raises Curse, increasing enemy quantity, health, speed and frequency.";
			default: return "Spend Ascension Points to improve this Adventure-only bonus.";
		}
	}

	private static string Signed(float value)
	{
		return value > 0f ? "+" + value.ToString("0.##") : value.ToString("0.##");
	}

	private static Sprite Sprite(AdjustValuePanel control, PowerUpType type)
	{
		try
		{
			var image = control._Icon;
			if ((Object)(object)image != (Object)null && (Object)(object)image.sprite != (Object)null)
				return image.sprite;
		}
		catch { }
		try { return GameData.GetSprite(type); }
		catch { return null; }
	}

	private static GameObject IconObject(AdjustValuePanel control)
	{
		try
		{
			var image = control._Icon;
			if ((Object)(object)image != (Object)null) return ((Component)image).gameObject;
		}
		catch { }
		return null;
	}
}
