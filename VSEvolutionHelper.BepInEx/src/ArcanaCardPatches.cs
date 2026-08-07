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

			// The info panel renders description text for arcanas whose data holds none, so it is
			// scraped as the player browses rather than looked up.
			var setInfo = typeof(ArcanaInfoPanel).GetMethod("SetInfo",
				System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
			if (setInfo != null)
			{
				harmony.Patch(setInfo, postfix: new HarmonyMethod(typeof(ArcanaCardPatches), nameof(SetInfo_Postfix)));
				Plugin.Log.LogInfo("[ArcanaCards] Patched ArcanaInfoPanel.SetInfo");
			}
			else Plugin.Log.LogWarning("[ArcanaCards] ArcanaInfoPanel.SetInfo not found");
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[ArcanaCards] patch: " + ex.Message);
		}
	}

	/// <summary>
	/// Keep whatever the info panel just rendered for this arcana.
	///
	/// <c>_InfoDescription</c> is an I2 Localize component, not the label - the rendered string
	/// lives on the TMP sharing its GameObject, and only that has been through localization. The
	/// same trap as PowerUpItemUI.Title.
	/// </summary>
	public static void SetInfo_Postfix(ArcanaInfoPanel __instance, ArcanaType arcanaType)
	{
		try
		{
			if ((Object)(object)__instance == (Object)null) return;
			var loc = __instance._InfoDescription;
			if ((Object)(object)loc == (Object)null) return;

			var tmp = ((Component)loc).gameObject.GetComponent<TMPro.TextMeshProUGUI>();
			if ((Object)(object)tmp == (Object)null) return;

			GameData.CaptureArcanaDescription(arcanaType, ((TMPro.TMP_Text)tmp).text);
			// The tooltip on screen was built before this text existed.
			Rows.Refresh();
		}
		catch (Exception ex)
		{
			Plugin.Dbg("[ArcanaCards] capture description: " + ex.Message);
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

			// Deliberately no early return for a card with nothing to say yet.
			//
			// The A group (ids 201+, e.g. A011_CRACKEDMIRROR) are adventure arcanas that change a
			// global rule rather than naming weapons, so an empty Affects list is correct for them
			// and not a lookup failure. Their text arrives later, from the info panel, and a card
			// skipped here would never be registered to receive it - it is bound once and the
			// panel draws afterwards.
			//
			// Registering an empty card costs nothing: ShowDockedPopup refuses to draw a panel
			// with no description and no rows, so nothing appears until there is something to say.
			if (Plugin.DebugVerbose && (rows == null || rows.Count == 0)
				&& string.IsNullOrWhiteSpace(description))
			{
				Plugin.Dbg($"ArcanaCards: {type} has nothing yet "
					+ $"(name='{GameData.GetArcanaName(type)}') - awaiting panel text");
			}

			GameObject root = ((Component)__instance).gameObject;

			Rows.Register(root, IconObject(__instance), new RowTooltipRegistry.Entry
			{
				Title = ResolveTitle(type),
				Description = description,
				// Re-read on hover: the info panel supplies text for arcanas whose data has none,
				// and it has not drawn them yet when the card is first bound.
				DescriptionProvider = () => DescribeOrExplainSilence(type),
				// Likewise for the Affects list, and for the same reason sprites are re-asked:
				// an answer worked out from a table that was not finished loading is a miss
				// cached as though it were an answer. Rebuilds only if the table actually grew.
				RowsProvider = () =>
				{
					GameData.RefreshArcanasIfGrown();
					return GameData.GetArcanaAffectRows(type);
				},
				// Every card answers the pointer, even the ones the game gives no text for.
				AllowTitleOnly = true,
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
				Offset = new Vector2(ItemTooltipsMod.ArcanaPanelX, ItemTooltipsMod.ArcanaPanelY),
				Pivot = ItemTooltipsMod.ArcanaPanelPivot,
				// Arcanas are the one surface with lists long enough to need it - Heart of Fire
				// touches 49 weapons - and the one with a free margin on both sides to use.
				SpillOffset = new Vector2(ItemTooltipsMod.ArcanaSpillX, ItemTooltipsMod.ArcanaSpillY),
				SpillPivot = ItemTooltipsMod.ArcanaSpillPivot,
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

	/// <summary>
	/// The arcana's description, or why there is not one.
	///
	/// Ten Darkanas are declared by the enum and absent from the data table - a later version's
	/// content, one still called <c>D07_tbd_bouncy</c>. A panel holding a name and nothing else
	/// reads as a broken tooltip, which is the wrong thing to tell the player: the tooltip is
	/// working and the game has nothing to say. Better to say that.
	/// </summary>
	private static string DescribeOrExplainSilence(ArcanaType type)
	{
		try
		{
			string desc = GameData.GetArcanaDescription(type);
			if (!string.IsNullOrWhiteSpace(desc)) return desc;
			if (!GameData.HasArcanaRecord(type)) return "Not in this version of the game yet.";
		}
		catch { }
		return null;
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

	/// <summary>
	/// The arcana's own art, resolved from its data rather than scraped off the card.
	///
	/// The card's <c>_Icon</c> was preferred first and gave the wrong picture: what that Image
	/// holds depends on the card's state, so it can be the card back, a frame or a placeholder,
	/// and on a face-down or locked card it is never the arcana at all. The data lookup answers
	/// with the art for the type regardless of how the card is currently drawn.
	///
	/// The card is still the fallback, for a type whose atlas has not resolved.
	/// </summary>
	private static Sprite ResolveSprite(ArcanaCardUI ui, ArcanaType type)
	{
		try
		{
			Sprite fromData = GameData.GetArcanaSprite(type);
			if ((Object)(object)fromData != (Object)null) return fromData;
		}
		catch { }
		try
		{
			var img = ui._Icon;
			if ((Object)(object)img != (Object)null && (Object)(object)img.sprite != (Object)null)
			{
				Plugin.Dbg($"[ArcanaCards] {type}: no sprite in data, using the card's own art");
				return img.sprite;
			}
		}
		catch { }
		return null;
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
