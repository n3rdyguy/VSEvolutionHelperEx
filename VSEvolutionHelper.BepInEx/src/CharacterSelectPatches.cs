using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using HarmonyLib;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VampireSurvivors.Data;
using VampireSurvivors.Data.Characters;
using VampireSurvivors.UI;
using Object = UnityEngine.Object;

namespace VSItemTooltips;

/// <summary>
/// Character Selection tooltips (starter weapon / evo / outfits).
/// Never Harmony-patch CharacterItemUI.SetData (IL2CPP detour breaks population).
/// Scan grid cards after Populate; rebuild tooltip content live on hover so outfit
/// switches (e.g. Para Kooleo skins with different starters) stay correct.
/// </summary>
public static class CharacterSelectPatches
{
	private static float _lastScanTime = -999f;
	private const float ScanCooldown = 0.6f;
	/// <summary>GameObject instance id → owning CharacterItemUI (for live rebuild).</summary>
	private static readonly Dictionary<int, CharacterItemUI> HitToUi = new Dictionary<int, CharacterItemUI>();

	public static void Apply(Harmony harmony)
	{
		TryPatch(harmony, typeof(CharacterSelectionPage), "Populate", nameof(Populate_Postfix));
		TryPatch(harmony, typeof(CharacterSelectionPage), "RefreshCharacters", nameof(Populate_Postfix));
		TryPatch(harmony, typeof(CharacterSelectionPage), "OnShowFinish", nameof(Show_Postfix));
		TryPatch(harmony, typeof(CharacterSelectionPage), "OnShowStart", nameof(Show_Postfix));

		// Outfit / selection refresh on individual cards (not SetData)
		TryPatch(harmony, typeof(CharacterItemUI), "Refresh", nameof(CardRefresh_Postfix));
		TryPatch(harmony, typeof(CharacterItemUI), "RefreshForSkin", nameof(CardRefresh_Postfix));
		TryPatch(harmony, typeof(CharacterItemUI), "SetSelected", nameof(CardRefresh_Postfix));

		foreach (string name in new[] { "OnHideStart", "OnHideFinish", "OnHide", "Hide", "Close" })
		{
			if (TryPatch(harmony, typeof(CharacterSelectionPage), name, nameof(Hide_Postfix)))
				break;
		}
	}

	private static bool TryPatch(Harmony harmony, Type type, string methodName, string postfix)
	{
		try
		{
			MethodInfo best = null;
			foreach (var m in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (m.Name != methodName) continue;
				if (best == null || m.GetParameters().Length < best.GetParameters().Length)
					best = m;
			}
			if (best == null)
				return false;
			harmony.Patch(best, postfix: new HarmonyMethod(typeof(CharacterSelectPatches), postfix));
			Plugin.Log.LogInfo($"[CharacterSelect] Patched {type.Name}.{best.Name}");
			return true;
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning($"[CharacterSelect] {type.Name}.{methodName}: {ex.Message}");
			return false;
		}
	}

	public static void Populate_Postfix() => ScheduleScan(2);
	public static void Show_Postfix() => ScheduleScan(3);

	public static void CardRefresh_Postfix(CharacterItemUI __instance)
	{
		if (!Plugin.CharacterTooltipsEnabled) return;
		if ((Object)(object)__instance == (Object)null) return;
		try { RegisterOne(__instance); } catch { }
	}

	public static void Hide_Postfix()
	{
		HitToUi.Clear();
		ItemTooltipsMod.ClearCharacterIcons();
	}

	private static void ScheduleScan(int frames)
	{
		if (!Plugin.CharacterTooltipsEnabled) return;
		ItemTooltipsMod.DelayFrames(frames, ScanAndRegister);
	}

	public static void Tick()
	{
		if (!Plugin.CharacterTooltipsEnabled) return;
		if (ItemTooltipsMod.CharacterIconCount > 0) return;
		if (Time.unscaledTime - _lastScanTime < ScanCooldown) return;
		try
		{
			var any = Object.FindObjectOfType<CharacterItemUI>();
			if ((Object)(object)any == (Object)null || !((Component)any).gameObject.activeInHierarchy)
				return;
		}
		catch { return; }
		ScanAndRegister();
	}

	public static void ScanAndRegister()
	{
		if (!Plugin.CharacterTooltipsEnabled) return;
		_lastScanTime = Time.unscaledTime;
		try
		{
			GameData.EnsureLoaded();
			HitToUi.Clear();
			ItemTooltipsMod.ClearCharacterIcons();

			CharacterItemUI[] uis = null;
			try { uis = Object.FindObjectsOfType<CharacterItemUI>(true); }
			catch { try { uis = Object.FindObjectsOfType<CharacterItemUI>(); } catch { } }
			if (uis == null || uis.Length == 0) return;

			int n = 0;
			foreach (CharacterItemUI ui in uis)
			{
				if ((Object)(object)ui == (Object)null) continue;
				if (!((Component)ui).gameObject.activeInHierarchy) continue;
				if (!IsGridCard(ui)) continue;
				if (RegisterOne(ui)) n++;
			}
			if (n > 0)
				Plugin.Log.LogInfo($"[CharacterSelect] Registered {n} grid cards for tooltips");
			else
				Plugin.Dbg("[CharacterSelect] scan: 0 grid cards registered");
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[CharacterSelect] ScanAndRegister: " + ex.Message);
		}
	}

	/// <summary>
	/// Skip large info/portrait panels (bottom character details). Only grid-sized cards.
	/// </summary>
	private static bool IsGridCard(CharacterItemUI ui)
	{
		try
		{
			var rt = ((Component)ui).GetComponent<RectTransform>();
			if ((Object)(object)rt == (Object)null) return true;
			Rect r = rt.rect;
			// Bottom info portrait / selected panel is much larger than a grid cell
			if (r.height > 160f || r.width > 220f)
				return false;
			// Also reject if deep under a named info panel
			Transform t = ((Component)ui).transform;
			while ((Object)(object)t != (Object)null)
			{
				string n = t.name ?? "";
				if (n.IndexOf("InfoPanel", StringComparison.OrdinalIgnoreCase) >= 0
					|| n.IndexOf("CharacterInfo", StringComparison.OrdinalIgnoreCase) >= 0
					|| n.IndexOf("SelectedCharacter", StringComparison.OrdinalIgnoreCase) >= 0
					|| n.IndexOf("Detail", StringComparison.OrdinalIgnoreCase) >= 0)
					return false;
				t = t.parent;
			}
		}
		catch { }
		return true;
	}

	private static bool RegisterOne(CharacterItemUI ui)
	{
		CharacterItem item = null;
		try { item = ui.CharacterItem; } catch { }
		if (item == null) return false;

		// Hit targets: small icons only (not the whole card / bottom panel)
		GameObject hit = ResolveHitTarget(ui);
		if ((Object)(object)hit == (Object)null) return false;

		// Placeholder label — live rebuild on show fills real content
		string name = SafeDisplayName(ui, item);
		if (string.IsNullOrWhiteSpace(name)) return false;

		int id = ((Object)hit).GetInstanceID();
		HitToUi[id] = ui;
		ItemTooltipsMod.RegisterCharacterIcon(hit, SafeType(item), name, "", ResolveCharacterSprite(ui));
		// Also map root so parent-walk resolves
		int rootId = ((Object)((Component)ui).gameObject).GetInstanceID();
		HitToUi[rootId] = ui;
		return true;
	}

	private static GameObject ResolveHitTarget(CharacterItemUI ui)
	{
		// Prefer weapon icon (player looks for starter) then character portrait — both small
		try
		{
			Image w = ui._WeaponIcon;
			if ((Object)(object)w != (Object)null && ((Component)w).gameObject.activeInHierarchy)
			{
				((Graphic)w).raycastTarget = true;
				return ((Component)w).gameObject;
			}
		}
		catch { }
		try
		{
			Image c = ui._CharacterIcon;
			if ((Object)(object)c != (Object)null && ((Component)c).gameObject.activeInHierarchy)
			{
				((Graphic)c).raycastTarget = true;
				return ((Component)c).gameObject;
			}
		}
		catch { }
		// Fallback: card root only if it is grid-sized
		return ((Component)ui).gameObject;
	}

	/// <summary>Live tooltip body for current character + current outfit/skin.</summary>
	public static bool TryBuildLiveTooltip(GameObject hitGo, out string title, out string body, out Sprite sprite)
	{
		title = null;
		body = null;
		sprite = null;
		if ((Object)(object)hitGo == (Object)null) return false;

		CharacterItemUI ui = null;
		int id = ((Object)hitGo).GetInstanceID();
		if (!HitToUi.TryGetValue(id, out ui) || (Object)(object)ui == (Object)null)
		{
			try { ui = hitGo.GetComponentInParent<CharacterItemUI>(); } catch { }
		}
		if ((Object)(object)ui == (Object)null) return false;
		if (!IsGridCard(ui)) return false;

		CharacterItem item = null;
		try { item = ui.CharacterItem; } catch { }
		if (item == null) return false;

		CharacterData cdata = null;
		CharacterType ctype = SafeType(item);
		try { cdata = item.CharacterData; } catch { }
		if (cdata == null) { try { cdata = item._characterData; } catch { } }

		Skin skin = ResolveCurrentSkin(item, cdata, ui);
		title = BuildTitle(ui, cdata, ctype, skin);
		body = BuildBody(ui, cdata, ctype, skin);
		sprite = ResolveCharacterSprite(ui);
		return !string.IsNullOrEmpty(title);
	}

	private static Skin ResolveCurrentSkin(CharacterItem item, CharacterData cdata, CharacterItemUI ui)
	{
		// 1) CharacterItem current skin item
		try
		{
			SkinItem si = item.GetCurrentSkinItem();
			if (si != null)
			{
				Skin s = null;
				try { s = si.SkinData; } catch { try { s = si._skinData; } catch { } }
				if (s != null) return s;
			}
		}
		catch { }

		// 2) CharacterData helpers
		if (cdata != null)
		{
			try
			{
				Skin s = cdata.GetCurrentSkinData();
				if (s != null) return s;
			}
			catch { }
			try
			{
				SkinType st = cdata.currentSkin;
				Skin s = cdata.GetSkinData(st);
				if (s != null) return s;
			}
			catch { }
		}
		return null;
	}

	private static string BuildTitle(CharacterItemUI ui, CharacterData cdata, CharacterType ctype, Skin skin)
	{
		string baseName = SafeDisplayName(ui, null) ?? ctype.ToString();
		string skinName = null;
		try
		{
			if (skin != null)
			{
				skinName = skin.name;
				if (string.IsNullOrWhiteSpace(skinName))
					skinName = skin._name_k__BackingField;
			}
		}
		catch { }
		// Prefer full name from data when skin-aware
		try
		{
			if (cdata != null && skin != null)
			{
				string full = cdata.GetFullName(ctype, skin, false, false);
				if (!string.IsNullOrWhiteSpace(full))
					return full.Trim();
			}
		}
		catch { }
		if (!string.IsNullOrWhiteSpace(skinName)
			&& baseName.IndexOf(skinName, StringComparison.OrdinalIgnoreCase) < 0
			&& !string.Equals(skinName, "DEFAULT", StringComparison.OrdinalIgnoreCase)
			&& !string.Equals(skinName, "Default", StringComparison.OrdinalIgnoreCase))
		{
			return $"{baseName} ({skinName.Trim()})";
		}
		return baseName;
	}

	private static string BuildBody(CharacterItemUI ui, CharacterData cdata, CharacterType ctype, Skin skin)
	{
		var sb = new StringBuilder();

		// Flavor — prefer skin-specific description
		string flavor = null;
		try
		{
			if (cdata != null && skin != null)
				flavor = cdata.GetDescription(ctype, skin);
		}
		catch { }
		if (string.IsNullOrWhiteSpace(flavor))
		{
			try { if (cdata != null) flavor = cdata.GetDescription(ctype); } catch { }
		}
		if (string.IsNullOrWhiteSpace(flavor))
		{
			try { flavor = cdata != null ? cdata.description : null; } catch { }
		}
		if (string.IsNullOrWhiteSpace(flavor) && skin != null)
		{
			try { flavor = skin._description_k__BackingField; } catch { }
		}
		if (!string.IsNullOrWhiteSpace(flavor))
			sb.AppendLine(flavor.Trim());

		// Starting weapon: UI icon first (reliable), then skin/character data
		WeaponType? starter = ResolveStartingWeapon(ui, cdata, skin);
		if (starter.HasValue && GameData.IsRealWeaponType(starter.Value))
		{
			if (sb.Length > 0) sb.AppendLine();
			string wName = GameData.GetWeaponName(starter.Value);
			if (string.IsNullOrEmpty(wName))
				wName = starter.Value.ToString();
			// Final guard — never print Void/VOID as a starter name
			if (string.Equals(wName, "Void", StringComparison.OrdinalIgnoreCase)
				|| string.Equals(wName, "VOID", StringComparison.OrdinalIgnoreCase))
			{
				sb.AppendLine("Starting weapon: (unknown)");
			}
			else
			{
				sb.AppendLine($"Starting weapon: {wName}");

				var rows = GameData.BuildEvoRowsFor(starter.Value);
				if (rows != null && rows.Count > 0)
				{
					sb.AppendLine("Evolution:");
					foreach (var row in rows)
					{
						string passives = "";
						if (row.Passives != null && row.Passives.Count > 0)
						{
							var parts = new List<string>();
							foreach (var p in row.Passives)
							{
								string pn = string.IsNullOrEmpty(p.Name) ? p.Type.ToString() : p.Name;
								if (p.RequiresMax) pn += " (max)";
								parts.Add(pn);
							}
							passives = " + " + string.Join(" + ", parts);
						}
						string evoName = string.IsNullOrEmpty(row.EvolvedName) ? row.Evolved.ToString() : row.EvolvedName;
						sb.AppendLine($"  {wName}{passives} → {evoName}");
					}
				}
			}
		}
		else
		{
			if (sb.Length > 0) sb.AppendLine();
			sb.AppendLine("Starting weapon: (unknown)");
		}

		// Other outfits with different starters (helps Para Kooleo etc.)
		string outfitBlock = FormatOtherOutfitWeapons(cdata, starter);
		if (!string.IsNullOrEmpty(outfitBlock))
		{
			if (sb.Length > 0) sb.AppendLine();
			sb.AppendLine("Other outfits:");
			sb.Append(outfitBlock);
		}

		string stats = FormatNotableStats(cdata, skin);
		if (!string.IsNullOrEmpty(stats))
		{
			if (sb.Length > 0) sb.AppendLine();
			sb.AppendLine("Notable stats:");
			sb.Append(stats);
		}

		return sb.ToString().TrimEnd();
	}

	/// <summary>
	/// Prefer the weapon icon the game already painted on the card (skin-correct),
	/// then skin/character startingWeapon fields. Il2Cpp Nullable&lt;WeaponType&gt; often
	/// reports HasValue with Value=VOID — those are ignored.
	/// </summary>
	private static WeaponType? ResolveStartingWeapon(CharacterItemUI ui, CharacterData cdata, Skin skin)
	{
		// 1) What the card is actually showing (handles outfits / Para Kooleo correctly)
		try
		{
			// Skip when game marked void / hidden weapon
			bool voidWeapon = false;
			try { voidWeapon = ui._voidWeapon; } catch { }
			if (!voidWeapon)
			{
				Image w = ui._WeaponIcon;
				if ((Object)(object)w != (Object)null
					&& ((Component)w).gameObject.activeInHierarchy
					&& (Object)(object)w.sprite != (Object)null)
				{
					if (GameData.TryIdentifyWeaponFromSprite(w.sprite, out WeaponType fromIcon)
						&& GameData.IsRealWeaponType(fromIcon))
					{
						Plugin.Dbg($"[CharacterSelect] starter from icon sprite={w.sprite.name} -> {fromIcon}");
						return fromIcon;
					}
					Plugin.Dbg($"[CharacterSelect] weapon icon sprite unmatched: {w.sprite.name}");
				}
			}
		}
		catch (Exception ex)
		{
			Plugin.Dbg("[CharacterSelect] icon resolve: " + ex.Message);
		}

		// 2) Skin override (outfit)
		WeaponType? fromSkin = ReadNullableWeapon(skin);
		if (fromSkin.HasValue) return fromSkin;

		// 3) Character default
		if (cdata != null)
		{
			WeaponType? fromChar = null;
			try { fromChar = ReadNullableWeaponValue(cdata.startingWeapon); } catch { }
			if (!fromChar.HasValue)
			{
				try { fromChar = ReadNullableWeaponValue(cdata._startingWeapon_k__BackingField); } catch { }
			}
			if (fromChar.HasValue) return fromChar;
		}

		return null;
	}

	private static WeaponType? ReadNullableWeapon(Skin skin)
	{
		if (skin == null) return null;
		try { return ReadNullableWeaponValue(skin.startingWeapon); } catch { }
		try { return ReadNullableWeaponValue(skin._startingWeapon_k__BackingField); } catch { }
		return null;
	}

	private static WeaponType? ReadNullableWeaponValue(Il2CppSystem.Nullable<WeaponType> n)
	{
		if (n == null) return null;
		try
		{
			// Prefer explicit check — Il2Cpp Nullable can lie about HasValue
			WeaponType v = default;
			bool has = false;
			try
			{
				has = n.HasValue;
				if (has) v = n.Value;
			}
			catch
			{
				try { v = n.Value; has = true; } catch { return null; }
			}
			if (!has) return null;
			return GameData.IsRealWeaponType(v) ? v : (WeaponType?)null;
		}
		catch { return null; }
	}

	private static string FormatOtherOutfitWeapons(CharacterData cdata, WeaponType? currentStarter)
	{
		if (cdata == null) return null;
		Il2CppSystem.Collections.Generic.List<Skin> skins = null;
		try { skins = cdata.skins; } catch { try { skins = cdata._skins_k__BackingField; } catch { } }
		if (skins == null || skins.Count <= 1) return null;

		var lines = new List<string>();
		var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		for (int i = 0; i < skins.Count; i++)
		{
			Skin s = skins[i];
			if (s == null) continue;
			WeaponType? w = ReadNullableWeapon(s);
			if (!w.HasValue) continue;
			if (currentStarter.HasValue && w.Value.Equals(currentStarter.Value)) continue;
			string sname = null;
			try { sname = s.name; } catch { }
			if (string.IsNullOrWhiteSpace(sname))
			{
				try { sname = s.skinType.ToString(); } catch { sname = "Outfit"; }
			}
			string wname = GameData.GetWeaponName(w.Value);
			if (string.IsNullOrEmpty(wname)
				|| string.Equals(wname, "Void", StringComparison.OrdinalIgnoreCase)
				|| string.Equals(wname, "VOID", StringComparison.OrdinalIgnoreCase))
				continue;
			string line = $"  {sname.Trim()}: {wname}";
			if (seen.Add(line))
				lines.Add(line);
			if (lines.Count >= 6) break;
		}
		return lines.Count == 0 ? null : string.Join("\n", lines);
	}

	private static string FormatNotableStats(CharacterData data, Skin skin)
	{
		// Prefer skin-overridden stats when non-zero, else character
		float pick(Func<Skin, float> skinGet, Func<CharacterData, float> charGet)
		{
			try
			{
				if (skin != null)
				{
					float sv = skinGet(skin);
					if (!Mathf.Approximately(sv, 0f)) return sv;
				}
			}
			catch { }
			try { return data != null ? charGet(data) : 0f; } catch { return 0f; }
		}

		if (data == null && skin == null) return null;
		var lines = new List<string>();
		void add(string label, float v, float ignoreNear = 0f)
		{
			if (Mathf.Abs(v - ignoreNear) < 0.001f) return;
			if (Mathf.Approximately(v, 0f)) return;
			string fmt = Mathf.Abs(v) >= 10f || Mathf.Approximately(v, Mathf.Round(v))
				? v.ToString("0")
				: v.ToString("0.##");
			lines.Add($"  {label}: {fmt}");
		}
		try
		{
			add("Max HP", pick(s => s.maxHp, d => d.maxHp));
			add("Armor", pick(s => s.armor, d => d.armor));
			add("Regen", pick(s => s.regen, d => d.regen));
			add("Move Speed", pick(s => s.moveSpeed, d => d.moveSpeed), 1f);
			add("Area", pick(s => s.area, d => d.area), 1f);
			add("Speed", pick(s => s.speed, d => d.speed), 1f);
			add("Duration", pick(s => s.duration, d => d.duration), 1f);
			add("Amount", pick(s => s.amount, d => d.amount));
			add("Cooldown", pick(s => s.cooldown, d => d.cooldown), 1f);
			add("Luck", pick(s => s.luck, d => d.luck), 1f);
			add("Growth", pick(s => s.growth, d => d.growth), 1f);
			if (data != null)
			{
				add("Greed", data.greed, 1f);
				add("Magnet", data.magnet);
				add("Revivals", data.revivals);
				add("Curse", data.curse);
			}
		}
		catch { }
		if (lines.Count == 0) return null;
		if (lines.Count > 8) lines = lines.GetRange(0, 8);
		return string.Join("\n", lines);
	}

	private static string SafeDisplayName(CharacterItemUI ui, CharacterItem item)
	{
		try
		{
			string n = ui.CharacterName;
			if (!string.IsNullOrWhiteSpace(n)) return n.Trim();
		}
		catch { }
		try
		{
			if ((Object)(object)ui._CharacterName != (Object)null)
			{
				string t = ((TMPro.TMP_Text)ui._CharacterName).text;
				if (!string.IsNullOrWhiteSpace(t)) return t.Trim();
			}
		}
		catch { }
		if (item != null)
		{
			try
			{
				var d = item.CharacterData;
				if (d != null)
				{
					string combined = $"{d.prefix} {d.charName} {d.surname}".Trim();
					if (!string.IsNullOrWhiteSpace(combined)) return combined;
				}
			}
			catch { }
		}
		return null;
	}

	private static CharacterType SafeType(CharacterItem item)
	{
		try { return item.CharacterType; } catch { }
		try { return item._characterType; } catch { }
		return default;
	}

	private static Sprite ResolveCharacterSprite(CharacterItemUI ui)
	{
		try
		{
			Image icon = ui._CharacterIcon;
			if ((Object)(object)icon != (Object)null && (Object)(object)icon.sprite != (Object)null)
				return icon.sprite;
		}
		catch { }
		try
		{
			Image w = ui._WeaponIcon;
			if ((Object)(object)w != (Object)null && (Object)(object)w.sprite != (Object)null)
				return w.sprite;
		}
		catch { }
		return null;
	}

	/// <summary>
	/// Hover hit-test: only accept UI raycast hits (won't "see through" the bottom info panel).
	/// </summary>
	public static bool TryRaycastCharacterHit(Vector2 screenPos, out GameObject hitGo)
	{
		hitGo = null;
		try
		{
			EventSystem es = EventSystem.current;
			if ((Object)(object)es == (Object)null) return false;
			var ped = new PointerEventData(es) { position = screenPos };
			// IL2CPP EventSystem.RaycastAll requires Il2CppSystem list
			var results = new Il2CppSystem.Collections.Generic.List<RaycastResult>();
			es.RaycastAll(ped, results);
			int count = results.Count;
			for (int i = 0; i < count; i++)
			{
				RaycastResult rr = results[i];
				GameObject go = rr.gameObject;
				if ((Object)(object)go == (Object)null) continue;
				// Topmost hits first — if we hit bottom info panel first without a card, stop
				// (only accept registered grid icons)
				Transform t = go.transform;
				while ((Object)(object)t != (Object)null)
				{
					int id = ((Object)t.gameObject).GetInstanceID();
					if (ItemTooltipsMod.HasCharacterIcon(id))
					{
						hitGo = t.gameObject;
						return true;
					}
					if (HitToUi.ContainsKey(id))
					{
						// Prefer weapon/portrait child if registered
						CharacterItemUI ui = HitToUi[id];
						if ((Object)(object)ui != (Object)null)
						{
							GameObject preferred = ResolveHitTarget(ui);
							if ((Object)(object)preferred != (Object)null && ItemTooltipsMod.HasCharacterIcon(((Object)preferred).GetInstanceID()))
							{
								hitGo = preferred;
								return true;
							}
						}
						hitGo = t.gameObject;
						return true;
					}
					t = t.parent;
				}
				// Hit something else on top (e.g. bottom panel chrome) — do not look through it
				// Only continue if it's a non-blocking graphic; RaycastAll is already sorted by depth.
				// If first result isn't our card, user is hovering something else.
				return false;
			}
		}
		catch { }
		return false;
	}
}
