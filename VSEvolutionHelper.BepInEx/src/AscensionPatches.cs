using System;
using System.Collections.Generic;
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
	private const float ScanCooldown = 0.5f;
	private static float _lastScan;

	public static void Apply(Harmony harmony)
	{
		if (!Plugin.AdventureTooltipsEnabled)
		{
			Plugin.Log.LogInfo("[Ascension] Disabled by AdventureTooltips config");
			return;
		}

		// These UI methods are generated IL2CPP bindings with native implementations. Harmony
		// reports them as patchable, but a postfix on any of the Ascension bindings crashes
		// CoreCLR before the main menu. The existing Update host is safe after the scene has
		// constructed its UI, so scan it there instead of intercepting the native calls.
		Plugin.Log.LogInfo("[Ascension] Using scene scan; native UI methods are not Harmony-patched");
	}

	public static void Tick()
	{
		if (!Plugin.AdventureTooltipsEnabled) return;
		if (Time.unscaledTime - _lastScan < ScanCooldown) return;
		_lastScan = Time.unscaledTime;

		try
		{
			AscensionButton[] buttons = null;
			AscensionPanel[] panels = null;
			try { buttons = Object.FindObjectsOfType<AscensionButton>(true); } catch { }
			try { panels = Object.FindObjectsOfType<AscensionPanel>(true); } catch { }

			int registered = 0;
			if (buttons != null)
				foreach (var button in buttons)
					if ((Object)(object)button != (Object)null && ((Component)button).gameObject.activeInHierarchy)
					{
						RegisterButton(button);
						registered++;
					}
			if (panels != null)
				foreach (var panel in panels)
					if ((Object)(object)panel != (Object)null && ((Component)panel).gameObject.activeInHierarchy)
					{
						RegisterPanel(panel);
						registered += 4;
					}
			if (registered > 0) Plugin.Dbg("Ascension: registered " + registered + " controls");
		}
		catch (Exception ex) { Plugin.Dbg("[Ascension] scan: " + ex.Message); }
	}

	private static void RegisterButton(AscensionButton button)
	{
		Rows.Register(((Component)button).gameObject, null, new RowTooltipRegistry.Entry
		{
			Title = "Ascension Points",
			Description = "Complete an Adventure to earn Ascension Points. Spend them here to boost Luck, Growth, Greed and Curse for that Adventure.",
			Offset = new Vector2(ItemTooltipsMod.AscensionPopupLeftX, ItemTooltipsMod.AscensionPopupTopY),
			Pivot = ItemTooltipsMod.AscensionPopupPivot,
		});
	}

	private static void RegisterPanel(AscensionPanel panel)
	{
		Register(panel._LuckPanel, PowerUpType.LUCK, panel);
		Register(panel._GrowthPanel, PowerUpType.GROWTH, panel);
		Register(panel._GreedPanel, PowerUpType.GREED, panel);
		Register(panel._CursePanel, PowerUpType.CURSE, panel);
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
			Offset = new Vector2(ItemTooltipsMod.AscensionPopupLeftX, ItemTooltipsMod.AscensionPopupTopY),
			Pivot = ItemTooltipsMod.AscensionPopupPivot,
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
