using System;
using HarmonyLib;
using UnityEngine;
using VampireSurvivors.Data;
using VampireSurvivors.UI;
using Object = UnityEngine.Object;

namespace VSItemTooltips;

/// <summary>
/// Music page: credit the track and say how it is unlocked, on hover.
///
/// The page lists titles. <c>MusicData</c> also carries an author and a source - the composer
/// and the game or DLC a track came from - which the UI credits nowhere, and three unlock
/// fields that answer what a greyed-out row is waiting on.
///
/// Instance-only postfix, matching the other patches here: the record is read back off the row
/// rather than taken from the patched call's arguments.
/// </summary>
public static class MusicPatches
{
	private static readonly RowTooltipRegistry Rows = new RowTooltipRegistry("Music");

	public static void Apply(Harmony harmony)
	{
		if (!Plugin.MusicTooltipsEnabled)
		{
			Plugin.Log.LogInfo("[Music] Disabled by config");
			return;
		}
		try
		{
			int patched = 0;
			foreach (var m in typeof(TrackItemUI).GetMethods(
				System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
			{
				if (m.Name != "SetData") continue;
				harmony.Patch(m, postfix: new HarmonyMethod(typeof(MusicPatches), nameof(SetData_Postfix)));
				Plugin.Log.LogInfo($"[Music] Patched TrackItemUI.SetData({m.GetParameters().Length} args)");
				patched++;
			}
			if (patched == 0) Plugin.Log.LogWarning("[Music] TrackItemUI.SetData not found");
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Music] patch: " + ex.Message);
		}
	}

	public static void SetData_Postfix(TrackItemUI __instance)
	{
		try
		{
			if ((Object)(object)__instance == (Object)null) return;

			MusicData data;
			try { data = __instance.GetMusicData(); }
			catch
			{
				try { data = __instance._data; }
				catch { return; }
			}
			if (data == null) return;

			var rows = GameData.GetMusicRows(data, out string description);
			if ((rows == null || rows.Count == 0) && string.IsNullOrEmpty(description))
			{
				if (Plugin.DebugVerbose) Plugin.Dbg("Music: nothing to show for " + ResolveTitle(__instance, data));
				return;
			}

			GameObject root = ((Component)__instance).gameObject;

			Rows.Register(root, IconObject(__instance), new RowTooltipRegistry.Entry
			{
				Title = ResolveTitle(__instance, data),
				Description = description,
				Sprite = ResolveSprite(__instance),
				// A locked row draws no art at all, so there is nothing to scrape off it. The
				// record names its own frame, and that lookup is deferred to hover: the atlas
				// holding it may not be in memory when the page is built, and asking early
				// caches a miss for a sprite that arrives moments later.
				SpriteProvider = () => LateSprite(data),
				Rows = rows,
				SectionHeader = (rows != null && rows.Count > 0) ? "Unlocked by:" : null,
				Offset = new Vector2(ItemTooltipsMod.MusicPanelX, ItemTooltipsMod.MusicPanelTopY),
				Pivot = ItemTooltipsMod.MusicPanelPivot,
			});

			if (Plugin.DebugVerbose)
				Plugin.Dbg($"Music: registered {(rows == null ? 0 : rows.Count)} rows for {ResolveTitle(__instance, data)}");
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Music] SetData postfix: " + ex.Message);
		}
	}

	/// <summary>
	/// What to head the tooltip with.
	///
	/// A locked row draws its title as a run of dashes, so preferring the row's own label - the
	/// rule everywhere else, since the game has already localized it - named every locked track
	/// "---------------------------". <c>MusicData.title</c> is the real name and is populated
	/// whether or not the track is unlocked, so it comes first; the row label is the fallback for
	/// anything the record does not name.
	///
	/// With <c>MusicSpoilers</c> off the masked label wins instead, matching the Secrets and
	/// Bestiary pages.
	/// </summary>
	private static string ResolveTitle(TrackItemUI ui, MusicData data)
	{
		string label = null;
		try
		{
			var tmp = ui._Title;
			if ((Object)(object)tmp != (Object)null)
			{
				string t = ((TMPro.TMP_Text)tmp).text;
				if (!string.IsNullOrWhiteSpace(t) && !GameData.LooksLikeLocKey(t)) label = t.Trim();
			}
		}
		catch { }

		if (!Plugin.MusicSpoilers && IsMasked(label)) return label ?? "Locked track";

		try
		{
			string t = data.title;
			if (!string.IsNullOrWhiteSpace(t) && !GameData.LooksLikeLocKey(t)) return t.Trim();
		}
		catch { }
		return label ?? "Track";
	}

	/// <summary>
	/// Is this the page's placeholder rather than a name? The mask is a run of dashes, so a label
	/// carrying no letter or digit at all is one.
	/// </summary>
	private static bool IsMasked(string label)
	{
		if (string.IsNullOrWhiteSpace(label)) return true;
		foreach (char c in label)
			if (char.IsLetterOrDigit(c)) return false;
		return true;
	}

	private static Sprite ResolveSprite(TrackItemUI ui)
	{
		try
		{
			var img = ui._Icon;
			if ((Object)(object)img != (Object)null && (Object)(object)img.sprite != (Object)null)
				return img.sprite;
		}
		catch { }
		return null;
	}

	/// <summary>
	/// The track's own art, resolved from <c>MusicData.icon</c> on hover.
	///
	/// Misses are remembered against <see cref="GameData.SpriteGeneration"/> rather than
	/// permanently: a miss only means the atlas was not in memory yet, and the generation moves
	/// when one arrives, so the next hover asks again instead of showing a blank forever.
	/// </summary>
	private static Sprite LateSprite(MusicData data)
	{
		string frame = null;
		try { frame = data.icon; } catch { }
		if (string.IsNullOrWhiteSpace(frame)) return null;
		frame = frame.Trim();

		if (IconCache.TryGetValue(frame, out Sprite hit) && (Object)(object)hit != (Object)null) return hit;
		if (IconMisses.TryGetValue(frame, out int gen) && gen == GameData.SpriteGeneration) return null;

		Sprite s = GameData.LoadSprite(frame, null);
		if ((Object)(object)s != (Object)null)
		{
			IconCache[frame] = s;
			IconMisses.Remove(frame);
			return s;
		}
		IconMisses[frame] = GameData.SpriteGeneration;
		if (Plugin.DebugVerbose) Plugin.Dbg("Music: no sprite for frame '" + frame + "'");
		return null;
	}

	private static readonly System.Collections.Generic.Dictionary<string, Sprite> IconCache =
		new System.Collections.Generic.Dictionary<string, Sprite>(StringComparer.OrdinalIgnoreCase);

	private static readonly System.Collections.Generic.Dictionary<string, int> IconMisses =
		new System.Collections.Generic.Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

	/// <summary>
	/// The icon child is registered alongside the root: a selected row draws a highlight over its
	/// contents that swallows the pointer before it reaches the row itself.
	/// </summary>
	private static GameObject IconObject(TrackItemUI ui)
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
