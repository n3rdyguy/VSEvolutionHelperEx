using System;
using HarmonyLib;
using UnityEngine;
using VampireSurvivors.Data;
using VampireSurvivors.UI;
using Object = UnityEngine.Object;

namespace VSItemTooltips;

/// <summary>
/// Arcana cards, wherever they are dealt: the mid-run pick, the Collections grid and the
/// pre-run loadout all draw the same <see cref="ArcanaCardUI"/>.
///
/// The mid-run pick is the one that matters. It is the only place in the game an arcana has to
/// be chosen against a timer, from a card that shows a name and a paragraph and never names the
/// weapons it actually changes - so the choice is made on memory or not at all. The Collections
/// page answers that question, but only with the run stopped.
///
/// Three <c>SetData</c> overloads exist and all three are patched: the selection page, the
/// info-panel form and the plain owned/locked form. Which one a card is bound through depends
/// on where it was dealt, and a card bound through an unpatched overload is silently inert.
/// </summary>
public static class ArcanaCardPatches
{
	private static readonly RowTooltipRegistry Rows = new RowTooltipRegistry("ArcanaCards");

	public static void Apply(Harmony harmony)
	{
		if (!Plugin.ArcanaCardTooltipsEnabled)
		{
			Plugin.Log.LogInfo("[ArcanaCards] Disabled by config");
			return;
		}
		try
		{
			int patched = 0;
			foreach (var m in typeof(ArcanaCardUI).GetMethods(
				System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
			{
				if (m.Name != "SetData") continue;
				harmony.Patch(m, postfix: new HarmonyMethod(typeof(ArcanaCardPatches), nameof(SetData_Postfix)));
				Plugin.Log.LogInfo($"[ArcanaCards] Patched ArcanaCardUI.SetData({m.GetParameters().Length} args)");
				patched++;
			}
			if (patched == 0) Plugin.Log.LogWarning("[ArcanaCards] ArcanaCardUI.SetData not found");
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[ArcanaCards] patch: " + ex.Message);
		}
	}

	public static void SetData_Postfix(ArcanaCardUI __instance)
	{
		try
		{
			if ((Object)(object)__instance == (Object)null) return;

			ArcanaType type;
			try { type = __instance.GetArcanaType(); }
			catch
			{
				try { type = __instance._type; }
				catch { return; }
			}

			// A face-down card in a spread that has not been flipped yet reads back as VOID.
			// Registering it would hand the player the pick before the game has revealed it.
			string id = type.ToString();
			if (string.IsNullOrEmpty(id) || id == "VOID" || id == "0") return;
			if (!Enum.IsDefined(typeof(ArcanaType), type)) return;

			var rows = GameData.GetArcanaAffectRows(type);
			string description = GameData.GetArcanaDescription(type);

			// The A group (ids 201+, e.g. A011_CRACKEDMIRROR) are adventure arcanas. They change a
			// global rule rather than naming weapons, so an empty Affects list is correct for them
			// and is not a lookup failure - the description is the whole tooltip, and the section
			// header is already suppressed when there are no rows.
			if ((rows == null || rows.Count == 0) && string.IsNullOrWhiteSpace(description))
			{
				if (Plugin.DebugVerbose)
					Plugin.Dbg($"ArcanaCards: nothing to show for {type} "
						+ $"(name='{GameData.GetArcanaName(type)}' desc=empty)");
				return;
			}

			GameObject root = ((Component)__instance).gameObject;

			Rows.Register(root, IconObject(__instance), new RowTooltipRegistry.Entry
			{
				Title = ResolveTitle(type),
				Description = description,
				Sprite = ResolveSprite(__instance, type),
				Rows = rows,
				SectionHeader = (rows != null && rows.Count > 0) ? "Affects:" : null,
				// Arcana cards are picked, not just read - and the card's own click handling sits
				// above the card, so our hover trigger ended the event walk before it got there.
				// Calling the card's own OnClick puts the selection back.
				OnClick = () =>
				{
					try
					{
						if ((Object)(object)__instance != (Object)null) __instance.OnClick();
					}
					catch (Exception ex) { Plugin.Dbg("[ArcanaCards] click: " + ex.Message); }
				},
				Offset = new Vector2(ItemTooltipsMod.SidePanelX, ItemTooltipsMod.SidePanelTopY),
				Pivot = ItemTooltipsMod.SidePanelPivot,
			});

			if (Plugin.DebugVerbose)
				Plugin.Dbg($"ArcanaCards: registered {(rows == null ? 0 : rows.Count)} rows for {type} "
					+ $"desc={(string.IsNullOrWhiteSpace(description) ? 0 : description.Length)}ch "
					+ $"under '{RootPath(root)}'");
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[ArcanaCards] SetData postfix: " + ex.Message);
		}
	}

	/// <summary>
	/// Which screen a card was dealt on, as a short ancestor path.
	///
	/// Cards from the mid-run pick, the pre-run loadout and the Collections grid are the same
	/// component, so a log line naming only the arcana cannot tell them apart - and "works in
	/// Collections, not in the run" is exactly the distinction that matters when one of them
	/// shows nothing.
	/// </summary>
	private static string RootPath(GameObject go)
	{
		try
		{
			var parts = new System.Collections.Generic.List<string>();
			Transform t = go.transform;
			for (int i = 0; i < 4 && (Object)(object)t != (Object)null; i++)
			{
				parts.Insert(0, ((Object)t).name);
				t = t.parent;
			}
			return string.Join("/", parts.ToArray());
		}
		catch { return "?"; }
	}

	private static string ResolveTitle(ArcanaType type)
	{
		try
		{
			string n = GameData.GetArcanaName(type);
			if (!string.IsNullOrWhiteSpace(n) && !GameData.LooksLikeLocKey(n)) return n.Trim();
		}
		catch { }
		return GameData.HumanizeEnum(type.ToString());
	}

	/// <summary>The card's drawn art, falling back to the sprite resolved from data.</summary>
	private static Sprite ResolveSprite(ArcanaCardUI ui, ArcanaType type)
	{
		try
		{
			var img = ui._Icon;
			if ((Object)(object)img != (Object)null && (Object)(object)img.sprite != (Object)null)
				return img.sprite;
		}
		catch { }
		try { return GameData.GetArcanaSprite(type); }
		catch { return null; }
	}

	/// <summary>
	/// The art child is registered alongside the root: a selected card draws a highlight over its
	/// contents that swallows the pointer before it reaches the card itself.
	/// </summary>
	private static GameObject IconObject(ArcanaCardUI ui)
	{
		try
		{
			var img = ui._Icon;
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
