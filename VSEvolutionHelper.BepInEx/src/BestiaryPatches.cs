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
/// EnemyData carries HP, damage, speed, XP and - the reason this exists - per-effect
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
	/// Placement in Safe Area reference units (1920x1200, so +/-960 x +/-600). Measured from the
	/// screen box 2169,346 - 2557,778 at 2560x1600: right edge 0.999 -> (0.999 - 0.5) * 1920 =
	/// 958, top edge 0.216 -> (0.5 - 0.216) * 1200 = 341.
	///
	/// Pinned by its top right corner, so the panel grows left as the title and stat lines get
	/// wider and down as the trait and stage lists get longer. Both edges it grows towards are
	/// open screen; the two it cannot cross are the ones it is nailed to.
	/// </summary>
	private const float BestiaryPopupRightX = 958f;
	private const float BestiaryPopupTopY = 341f;

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

			// The page owns the info panel - EnemyItemUI.SetInfoPanel() never fired, because
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
			string rowName = ResolveName(__instance);

			Rows.Register(root, null, new RowTooltipRegistry.Entry
			{
				Title = rowName,
				// Nothing resolved here. Every row on the page runs this, and a miss costs a
				// chain of atlas lookups that ends in a scan of every sprite in memory - 217
				// times over, for icons all but one of which will never be looked at. The
				// hovered row is the only one that needs an answer.
				SpriteProvider = () => LateSprite(data, enemyId),
				Rows = info.Rows,
				SectionHeader = "Stats:",
				// Right of the list, hanging from a fixed top right corner.
				Offset = new Vector2(BestiaryPopupRightX, BestiaryPopupTopY),
				Pivot = new Vector2(1f, 1f),
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
	/// own Found In label. Capture that rather than deriving a stage list - a derived one
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
			CapturePanelSprite(__instance);
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
	/// rebuilt from the enum id - otherwise revealing the stats but not the name would be a
	/// half-spoiler that helps nobody.
	/// </summary>
	private static string ResolveName(EnemyItemUI item)
	{
		// The row's own label first. A record's bName covers a whole Bestiary family, so on a
		// variant row it names the family and not the row - "Spirit" where the game itself
		// prints "Calamity" - and contradicting the label the reader is looking at is worse
		// than whatever is gained by reading the data directly.
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
		// Undiscovered rows draw dashes, and bName is present even for those.
		try
		{
			string b = GameData.GetEnemyName(item._type.ToString());
			if (!string.IsNullOrWhiteSpace(b) && !IsPlaceholder(b)) return b;
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
	/// The enemy's own sprite, from the first animation frame, then from the JSON record and its
	/// Bestiary siblings.
	///
	/// Both of those resolve a frame name against an atlas, which fails outright when the atlas
	/// holding it is not loaded on this page - no name is going to find a sprite that is not
	/// there. The info panel's portrait covers that case, but it is only available once the
	/// enemy has been selected, so it is looked up on hover (<see cref="PanelSprite"/>).
	/// </summary>
	private static Sprite ResolveSprite(VampireSurvivors.Data.Enemies.EnemyData data, string enemyId)
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

		// The typed record's frame list is empty for a whole block of enemies; the JSON has
		// them, and falls back to the entry's other variants.
		Sprite fromJson = GameData.GetEnemySprite(enemyId);
		if ((Object)(object)fromJson != (Object)null) return fromJson;

		// The info panel draws portraits named "<base>_i01" rather than the animation frame the
		// records name, and an atlas holding no "kappa_0.png" may still hold "kappa_i01".
		try
		{
			Sprite icon = GameData.GetEnemyIconSprite(enemyId);
			if ((Object)(object)icon != (Object)null)
			{
				if (Plugin.DebugVerbose)
					Plugin.Dbg($"[Bestiary] {enemyId}: icon by _i01 convention = {((Object)icon).name}");
				return icon;
			}
		}
		catch { }
		if (Plugin.DebugVerbose)
			Plugin.Dbg($"[Bestiary] {enemyId}: no icon from data, waiting on an atlas");
		return null;
	}

	private static readonly System.Collections.Generic.Dictionary<string, Sprite> IconCache =
		new System.Collections.Generic.Dictionary<string, Sprite>(StringComparer.OrdinalIgnoreCase);

	/// <summary>
	/// Enemies known to have no icon, against the sprite generation that was current when we
	/// looked. Re-running the full chain on every re-hover of the same unresolvable row is what
	/// made the page feel slow; the generation is what stops that becoming permanent, since it
	/// moves whenever an atlas loads or a portrait is captured.
	/// </summary>
	private static readonly System.Collections.Generic.Dictionary<string, int> IconMisses =
		new System.Collections.Generic.Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

	/// <summary>
	/// The hovered row's icon: cache, then a captured portrait, then the data paths, and failing
	/// all of it a request for this enemy's atlas. That request is asynchronous, so this hover
	/// shows no icon and the next one does - the price of not preloading art nobody looked at.
	/// </summary>
	private static Sprite LateSprite(VampireSurvivors.Data.Enemies.EnemyData data, string enemyId)
	{
		if (string.IsNullOrEmpty(enemyId)) return null;

		if (IconCache.TryGetValue(enemyId, out Sprite hit) && (Object)(object)hit != (Object)null)
			return hit;
		if (IconMisses.TryGetValue(enemyId, out int gen) && gen == GameData.SpriteGeneration)
			return null;

		Sprite s = PanelSprite(enemyId);
		if ((Object)(object)s == (Object)null) s = ResolveSprite(data, enemyId);

		if ((Object)(object)s != (Object)null)
		{
			IconCache[enemyId] = s;
			IconMisses.Remove(enemyId);
			return s;
		}

		IconMisses[enemyId] = GameData.SpriteGeneration;
		GameData.RequestEnemyTexture(enemyId);
		return null;
	}

	/// <summary>
	/// Portraits seen on the page, keyed by enemy id.
	///
	/// The list row is text only - a name and a kill count, no picture - so the info panel is
	/// the one place the game draws an enemy here. It names each portrait object after the
	/// enemy it shows ("BAT1", "EX_SEAPIG"), which is what makes this keyable at all: the page
	/// title is the header, "BESTIARY: 211/217", and identifies nothing.
	///
	/// It fills as enemies are selected, so a row never clicked has no portrait to offer.
	/// </summary>
	private static readonly System.Collections.Generic.Dictionary<string, Sprite> PanelSprites =
		new System.Collections.Generic.Dictionary<string, Sprite>(StringComparer.OrdinalIgnoreCase);

	private static Sprite PanelSprite(string enemyId)
	{
		if (string.IsNullOrWhiteSpace(enemyId)) return null;
		if (!PanelSprites.TryGetValue(enemyId.Trim(), out Sprite s)) return null;
		return (Object)(object)s != (Object)null ? s : null;
	}

	/// <summary>
	/// Cache every portrait the page is currently drawing, by the enemy id naming its object.
	///
	/// Selecting one enemy draws its whole family, so a single selection can fill several
	/// entries. Object names are the filter: an enemy id is upper case with underscores, which
	/// excludes the panel's own furniture ("Icon", "Viewport", "Selector") without needing to
	/// guess at the hierarchy.
	/// </summary>
	private static void CapturePanelSprite(VampireSurvivors.UI.BestiaryPage page)
	{
		try
		{
			Transform root = ((Component)page).transform;
			var images = root.GetComponentsInChildren<UnityEngine.UI.Image>(true);
			if (images == null) return;

			int added = 0;
			for (int i = 0; i < images.Count; i++)
			{
				var img = images[i];
				if ((Object)(object)img == (Object)null || (Object)(object)img.sprite == (Object)null) continue;
				string nm = ((Object)img).name;
				if (!LooksLikeEnemyId(nm)) continue;
				if (PanelSprite(nm) != null) continue;
				PanelSprites[nm.Trim()] = img.sprite;
				added++;
				if (Plugin.DebugVerbose)
					Plugin.Dbg($"[Bestiary] portrait cached: {nm} = {((Object)img.sprite).name}");
			}

			// A new portrait can satisfy a row that has already been written off as iconless.
			if (added > 0) GameData.BumpSpriteGeneration();
			else if (Plugin.DebugVerbose)
				Plugin.Dbg($"[Bestiary] no new portraits among {images.Count} images");
		}
		catch (Exception ex)
		{
			Plugin.Dbg("[Bestiary] capture panel sprite: " + ex.Message);
		}
	}

	/// <summary>
	/// Enemy ids are upper case with digits and underscores (EME_BOAR, BAT1). "QuestionMark" is
	/// the panel's placeholder for an enemy the player has not met, and is not a portrait.
	/// </summary>
	private static bool LooksLikeEnemyId(string name)
	{
		if (string.IsNullOrEmpty(name) || name.Length < 3) return false;
		bool hasLetter = false;
		foreach (char c in name)
		{
			if (c >= 'A' && c <= 'Z') { hasLetter = true; continue; }
			if ((c >= '0' && c <= '9') || c == '_') continue;
			return false;
		}
		return hasLetter;
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
