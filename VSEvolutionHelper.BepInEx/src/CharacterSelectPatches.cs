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
/// Character Selection tooltips. Never patch CharacterItemUI.SetData (breaks IL2CPP population).
/// Register whole grid cards; build full tooltip text at show time.
/// </summary>
public static class CharacterSelectPatches
{
	private static float _lastScanTime = -999f;
	private const float ScanCooldown = 0.5f;
	private static readonly Dictionary<int, CharacterItemUI> HitToUi = new Dictionary<int, CharacterItemUI>();

	public static void Apply(Harmony harmony)
	{
		TryPatch(harmony, typeof(CharacterSelectionPage), "Populate", nameof(Populate_Postfix));
		TryPatch(harmony, typeof(CharacterSelectionPage), "RefreshCharacters", nameof(Populate_Postfix));
		TryPatch(harmony, typeof(CharacterSelectionPage), "OnShowFinish", nameof(Show_Postfix));
		TryPatch(harmony, typeof(CharacterSelectionPage), "OnShowStart", nameof(Show_Postfix));
		TryPatch(harmony, typeof(CharacterItemUI), "Refresh", nameof(CardRefresh_Postfix));
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
			if (best == null) return false;
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
			Plugin.Log.LogInfo($"[CharacterSelect] Registered {n} character cards for tooltips");
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[CharacterSelect] ScanAndRegister: " + ex.Message);
		}
	}

	/// <summary>Exclude bottom info / portrait panels by hierarchy name only (not size).</summary>
	private static bool IsGridCard(CharacterItemUI ui)
	{
		try
		{
			Transform t = ((Component)ui).transform;
			while ((Object)(object)t != (Object)null)
			{
				string n = t.name ?? "";
				// Bottom selected-character strip / detail panes
				if (n.IndexOf("InfoPanel", StringComparison.OrdinalIgnoreCase) >= 0
					|| n.IndexOf("CharacterInfo", StringComparison.OrdinalIgnoreCase) >= 0
					|| n.IndexOf("SelectedCharacter", StringComparison.OrdinalIgnoreCase) >= 0
					|| n.IndexOf("DetailPanel", StringComparison.OrdinalIgnoreCase) >= 0
					|| n.IndexOf("DescriptionPanel", StringComparison.OrdinalIgnoreCase) >= 0
					|| n.IndexOf("CharacterDetail", StringComparison.OrdinalIgnoreCase) >= 0)
					return false;
				t = t.parent;
			}
			// Very tall = bottom strip portrait (grid cells are roughly square)
			var rt = ((Component)ui).GetComponent<RectTransform>();
			if ((Object)(object)rt != (Object)null)
			{
				Rect r = rt.rect;
				if (r.height > 280f && r.width > 400f)
					return false;
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

		// Whole card for reliable hover (not just the tiny weapon icon)
		GameObject card = ((Component)ui).gameObject;
		EnsureRaycastOnCard(card);

		string name = SafeDisplayName(ui, item);
		if (string.IsNullOrWhiteSpace(name))
		{
			try { name = SafeType(item).ToString(); } catch { name = "Character"; }
		}

		// Pre-bake body so popup still has content if live rebuild hiccups
		string body;
		Sprite spr;
		try
		{
			CharacterData cdata = null;
			CharacterType ctype = SafeType(item);
			try { cdata = item.CharacterData ?? item._characterData; } catch { try { cdata = item._characterData; } catch { } }
			Skin skin = ResolveCurrentSkin(item, cdata, ui);
			body = BuildBody(ui, cdata, ctype, skin);
			spr = ResolveCharacterSprite(ui);
			name = BuildTitle(ui, cdata, ctype, skin) ?? name;
		}
		catch (Exception ex)
		{
			Plugin.Dbg("[CharacterSelect] prebuild: " + ex.Message);
			body = "";
			spr = ResolveCharacterSprite(ui);
		}

		int id = ((Object)card).GetInstanceID();
		HitToUi[id] = ui;
		ItemTooltipsMod.RegisterCharacterIcon(card, SafeType(item), name, body ?? "", spr);

		// Also map weapon + portrait icons so raycasts on them resolve
		try
		{
			if ((Object)(object)ui._WeaponIcon != (Object)null)
			{
				var go = ((Component)ui._WeaponIcon).gameObject;
				HitToUi[((Object)go).GetInstanceID()] = ui;
				((Graphic)ui._WeaponIcon).raycastTarget = true;
			}
		}
		catch { }
		try
		{
			if ((Object)(object)ui._CharacterIcon != (Object)null)
			{
				var go = ((Component)ui._CharacterIcon).gameObject;
				HitToUi[((Object)go).GetInstanceID()] = ui;
				((Graphic)ui._CharacterIcon).raycastTarget = true;
			}
		}
		catch { }

		return true;
	}

	private static void EnsureRaycastOnCard(GameObject card)
	{
		try
		{
			var g = card.GetComponent<Graphic>();
			if ((Object)(object)g != (Object)null)
				g.raycastTarget = true;
			else
			{
				// Transparent image so the card receives raycasts
				var img = card.GetComponent<Image>();
				if ((Object)(object)img == (Object)null)
				{
					img = card.AddComponent<Image>();
					img.color = new Color(0, 0, 0, 0.01f);
				}
				((Graphic)img).raycastTarget = true;
			}
		}
		catch { }
	}

	/// <summary>Structured tooltip data for rich UI (icons + text).</summary>
	public class TooltipData
	{
		public string Title;
		public string Flavor;
		public Sprite Portrait;
		public WeaponType? Starter;
		public string StarterName;
		public Sprite StarterSprite;
		public List<EvoDisplayRow> Evos = new List<EvoDisplayRow>();
		public string OutfitsText;
		public string StatsText;
		/// <summary>Plain-text fallback body.</summary>
		public string BodyText;
	}

	/// <summary>Live tooltip body for current character + current outfit/skin.</summary>
	public static bool TryBuildLiveTooltip(GameObject hitGo, out string title, out string body, out Sprite sprite)
	{
		title = null;
		body = null;
		sprite = null;
		if (!TryGetTooltipData(hitGo, out TooltipData data) || data == null)
			return false;
		title = data.Title;
		body = data.BodyText;
		sprite = data.Portrait;
		return !string.IsNullOrEmpty(title);
	}

	public static bool TryGetTooltipData(GameObject hitGo, out TooltipData data)
	{
		data = null;
		try
		{
			if ((Object)(object)hitGo == (Object)null) return false;

			CharacterItemUI ui = null;
			int id = ((Object)hitGo).GetInstanceID();
			if (!HitToUi.TryGetValue(id, out ui) || (Object)(object)ui == (Object)null)
			{
				try { ui = hitGo.GetComponentInParent<CharacterItemUI>(); } catch { }
			}
			if ((Object)(object)ui == (Object)null) return false;

			CharacterItem item = null;
			try { item = ui.CharacterItem; } catch { }
			if (item == null) return false;

			CharacterData cdata = null;
			CharacterType ctype = SafeType(item);
			try { cdata = item.CharacterData; } catch { }
			if (cdata == null) { try { cdata = item._characterData; } catch { } }

			Skin skin = ResolveCurrentSkin(item, cdata, ui);
			data = new TooltipData();
			data.Title = BuildTitle(ui, cdata, ctype, skin);
			if (string.IsNullOrEmpty(data.Title))
				data.Title = SafeDisplayName(ui, item) ?? ctype.ToString();
			data.Portrait = ResolveCharacterSprite(ui);
			data.Flavor = BuildFlavor(cdata, ctype, skin);
			data.BodyText = BuildBody(ui, cdata, ctype, skin);

			WeaponType? starter = ResolveStartingWeapon(ui, cdata, skin);
			if (starter.HasValue && GameData.IsRealWeaponType(starter.Value))
			{
				data.Starter = starter;
				data.StarterName = GameData.GetWeaponName(starter.Value);
				if (string.IsNullOrEmpty(data.StarterName))
					data.StarterName = starter.Value.ToString();
				if (string.Equals(data.StarterName, "Void", StringComparison.OrdinalIgnoreCase)
					|| string.Equals(data.StarterName, "VOID", StringComparison.OrdinalIgnoreCase))
				{
					data.Starter = null;
					data.StarterName = null;
				}
				else
				{
					data.StarterSprite = GameData.GetSprite(starter.Value);
					// Prefer the card's painted icon if sprite load failed
					if ((Object)(object)data.StarterSprite == (Object)null)
					{
						try
						{
							Image w = ui._WeaponIcon;
							if ((Object)(object)w != (Object)null)
								data.StarterSprite = w.sprite;
						}
						catch { }
					}
					try
					{
						var rows = GameData.BuildEvoRowsFor(starter.Value);
						if (rows != null)
							data.Evos = rows;
					}
					catch { }
				}
			}

			data.OutfitsText = FormatOtherOutfitWeapons(cdata, data.Starter);
			data.StatsText = FormatNotableStats(cdata, skin);
			return true;
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[CharacterSelect] TryGetTooltipData: " + ex.Message);
			data = null;
			return false;
		}
	}

	private static string BuildFlavor(CharacterData cdata, CharacterType ctype, Skin skin)
	{
		// Prefer explicit loc key → LocalizeDisplayText (never show raw itemLang/… keys)
		try
		{
			if (cdata != null)
			{
				string key = cdata.GetDescriptionLocKey(ctype);
				string t = GameData.LocalizeDisplayText(key);
				if (!string.IsNullOrEmpty(t)) return t;
			}
		}
		catch { }

		string[] candidates = new string[6];
		int n = 0;
		try
		{
			if (cdata != null && skin != null)
				candidates[n++] = cdata.GetDescription(ctype, skin);
		}
		catch { }
		try
		{
			if (cdata != null)
				candidates[n++] = cdata.GetDescription(ctype);
		}
		catch { }
		try
		{
			if (cdata != null)
				candidates[n++] = cdata.description;
		}
		catch { }
		try
		{
			if (skin != null)
				candidates[n++] = skin._description_k__BackingField;
		}
		catch { }
		try
		{
			if (skin != null)
			{
				string sn = skin.name ?? skin._name_k__BackingField;
				if (!string.IsNullOrWhiteSpace(sn))
					candidates[n++] = sn; // rare: name field holds a term
			}
		}
		catch { }

		for (int i = 0; i < n; i++)
		{
			string t = GameData.LocalizeDisplayText(candidates[i]);
			if (!string.IsNullOrEmpty(t))
				return t;
		}

		// Synthesize from CharacterType / skin type (covers skins with missing GetDescription)
		try
		{
			string t = GameData.LocalizeTypedDescription(ctype.ToString(), "description");
			if (!string.IsNullOrEmpty(t)) return t;
		}
		catch { }
		try
		{
			if (skin != null)
			{
				string st = null;
				try { st = skin.skinType.ToString(); } catch { }
				if (string.IsNullOrEmpty(st))
					try { st = skin.name ?? skin._name_k__BackingField; } catch { }
				if (!string.IsNullOrWhiteSpace(st)
					&& !string.Equals(st, "DEFAULT", StringComparison.OrdinalIgnoreCase)
					&& !string.Equals(st, ctype.ToString(), StringComparison.OrdinalIgnoreCase))
				{
					string t = GameData.LocalizeTypedDescription(st.Trim(), "description");
					if (!string.IsNullOrEmpty(t)) return t;
				}
			}
		}
		catch { }

		return null;
	}

	private static Skin ResolveCurrentSkin(CharacterItem item, CharacterData cdata, CharacterItemUI ui)
	{
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
		string baseName = SafeDisplayName(ui, null);
		baseName = GameData.LocalizeDisplayText(baseName) ?? baseName;
		if (string.IsNullOrWhiteSpace(baseName) || GameData.LooksLikeLocKey(baseName))
			baseName = GameData.LocalizeTypedDescription(ctype.ToString(), "name")
				?? GameData.HumanizeEnum(ctype.ToString());
		try
		{
			if (cdata != null && skin != null)
			{
				string full = cdata.GetFullName(ctype, skin, false, false);
				full = GameData.LocalizeDisplayText(full) ?? full;
				if (!string.IsNullOrWhiteSpace(full) && !GameData.LooksLikeLocKey(full))
					return full.Trim();
			}
		}
		catch { }
		try
		{
			if (cdata != null)
			{
				string full = cdata.GetFullName(ctype, false, false);
				full = GameData.LocalizeDisplayText(full) ?? full;
				if (!string.IsNullOrWhiteSpace(full) && !GameData.LooksLikeLocKey(full))
					return full.Trim();
			}
		}
		catch { }
		string skinName = null;
		try
		{
			if (skin != null)
			{
				skinName = skin.name;
				if (string.IsNullOrWhiteSpace(skinName))
					skinName = skin._name_k__BackingField;
				skinName = GameData.LocalizeDisplayText(skinName) ?? skinName;
				if (GameData.LooksLikeLocKey(skinName)) skinName = null;
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
		try
		{
			// Flavor (localized; never raw itemLang keys)
			string flavor = BuildFlavor(cdata, ctype, skin);
			if (!string.IsNullOrWhiteSpace(flavor))
				sb.AppendLine(flavor.Trim());

			// Starting weapon
			WeaponType? starter = ResolveStartingWeapon(ui, cdata, skin);
			if (sb.Length > 0) sb.AppendLine();
			if (starter.HasValue && GameData.IsRealWeaponType(starter.Value))
			{
				string wName = GameData.GetWeaponName(starter.Value);
				if (string.IsNullOrEmpty(wName))
					wName = starter.Value.ToString();
				if (string.Equals(wName, "Void", StringComparison.OrdinalIgnoreCase)
					|| string.Equals(wName, "VOID", StringComparison.OrdinalIgnoreCase))
				{
					sb.AppendLine("Starting weapon: (unknown)");
				}
				else
				{
					sb.AppendLine($"Starting weapon: {wName}");
					try
					{
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
					catch (Exception ex)
					{
						Plugin.Dbg("[CharacterSelect] evo rows: " + ex.Message);
					}
				}
			}
			else
			{
				sb.AppendLine("Starting weapon: (unknown)");
			}

			string outfitBlock = FormatOtherOutfitWeapons(cdata, starter);
			if (!string.IsNullOrEmpty(outfitBlock))
			{
				sb.AppendLine();
				sb.AppendLine("Other outfits:");
				sb.Append(outfitBlock);
			}

			string stats = FormatNotableStats(cdata, skin);
			if (!string.IsNullOrEmpty(stats))
			{
				sb.AppendLine();
				sb.AppendLine("Notable stats:");
				sb.Append(stats);
			}
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[CharacterSelect] BuildBody: " + ex.Message);
			if (sb.Length == 0)
				sb.Append("(tooltip error)");
		}

		string result = sb.ToString().TrimEnd();
		return string.IsNullOrEmpty(result) ? "(no details)" : result;
	}

	private static WeaponType? ResolveStartingWeapon(CharacterItemUI ui, CharacterData cdata, Skin skin)
	{
		// 1) Card weapon icon (skin/outfit-correct)
		try
		{
			bool voidWeapon = false;
			try { voidWeapon = ui._voidWeapon; } catch { }
			if (!voidWeapon)
			{
				Image w = ui._WeaponIcon;
				if ((Object)(object)w != (Object)null
					&& (Object)(object)w.sprite != (Object)null)
				{
					if (GameData.TryIdentifyWeaponFromSprite(w.sprite, out WeaponType fromIcon)
						&& GameData.IsRealWeaponType(fromIcon))
						return fromIcon;
				}
			}
		}
		catch { }

		// 2) Skin data
		WeaponType? fromSkin = ReadNullableWeapon(skin);
		if (fromSkin.HasValue) return fromSkin;

		// 3) Character data
		if (cdata != null)
		{
			try
			{
				var v = ReadNullableWeaponValue(cdata.startingWeapon);
				if (v.HasValue) return v;
			}
			catch { }
			try
			{
				var v = ReadNullableWeaponValue(cdata._startingWeapon_k__BackingField);
				if (v.HasValue) return v;
			}
			catch { }
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
			if (!n.HasValue) return null;
			WeaponType v = n.Value;
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
				|| string.Equals(wname, "Void", StringComparison.OrdinalIgnoreCase))
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
		string Scrub(string n)
		{
			if (string.IsNullOrWhiteSpace(n)) return null;
			n = n.Trim();
			// UI TMP often wraps "powerupLang/MERCHANT name" across lines
			string loc = GameData.LocalizeDisplayText(n);
			if (!string.IsNullOrEmpty(loc)) return loc;
			if (GameData.LooksLikeLocKey(n)) return null;
			return n;
		}
		try
		{
			string n = Scrub(ui.CharacterName);
			if (!string.IsNullOrWhiteSpace(n)) return n;
		}
		catch { }
		try
		{
			if ((Object)(object)ui._CharacterName != (Object)null)
			{
				string t = Scrub(((TMPro.TMP_Text)ui._CharacterName).text);
				if (!string.IsNullOrWhiteSpace(t)) return t;
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
					combined = Scrub(combined);
					if (!string.IsNullOrWhiteSpace(combined)) return combined;
				}
			}
			catch { }
			try
			{
				string t = GameData.LocalizeTypedDescription(SafeType(item).ToString(), "name");
				if (!string.IsNullOrEmpty(t)) return t;
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
	/// Hover hit: raycast first (respects bottom panel), else rect-contains on registered cards.
	/// </summary>
	public static bool TryFindHoveredCard(Vector2 screenPos, out int hitId, out GameObject hitGo)
	{
		hitId = -1;
		hitGo = null;

		// 1) UI raycast — topmost object; only accept if it maps to a registered card
		try
		{
			EventSystem es = EventSystem.current;
			if ((Object)(object)es != (Object)null)
			{
				var ped = new PointerEventData(es) { position = screenPos };
				var results = new Il2CppSystem.Collections.Generic.List<RaycastResult>();
				es.RaycastAll(ped, results);
				if (results.Count > 0)
				{
					// Walk top result parents for a registered card
					GameObject top = results[0].gameObject;
					if (TryMapToRegistered(top, out hitId, out hitGo))
						return true;
					// Top hit is not a character card (e.g. bottom info) — do not fall through
					// unless it's a non-blocking area; treat as no-hit for tooltips
					if (IsBlockingNonCard(top))
						return false;
				}
			}
		}
		catch { }

		// 2) Fallback: rect contains on registered card roots (covers cards without raycast targets)
		float bestArea = float.MaxValue;
		foreach (var kv in HitToUi)
		{
			CharacterItemUI ui = kv.Value;
			if ((Object)(object)ui == (Object)null) continue;
			GameObject card = ((Component)ui).gameObject;
			if ((Object)(object)card == (Object)null || !card.activeInHierarchy) continue;
			if (!IsGridCard(ui)) continue;

			RectTransform rt = card.GetComponent<RectTransform>();
			if ((Object)(object)rt == (Object)null) continue;

			Camera cam = null;
			try
			{
				Canvas c = card.GetComponentInParent<Canvas>();
				if ((Object)(object)c != (Object)null && c.renderMode != RenderMode.ScreenSpaceOverlay)
					cam = c.worldCamera;
			}
			catch { }

			bool inside = RectTransformUtility.RectangleContainsScreenPoint(rt, screenPos, cam)
				|| RectTransformUtility.RectangleContainsScreenPoint(rt, screenPos, null);
			if (!inside) continue;

			float area = Mathf.Max(1f, Mathf.Abs(rt.rect.width) * Mathf.Abs(rt.rect.height));
			// Prefer smaller cards (grid) over any large strip that slipped through
			if (area < bestArea)
			{
				bestArea = area;
				hitGo = card;
				hitId = ((Object)card).GetInstanceID();
			}
		}
		return (Object)(object)hitGo != (Object)null && ItemTooltipsMod.HasCharacterIcon(hitId);
	}

	private static bool TryMapToRegistered(GameObject go, out int hitId, out GameObject hitGo)
	{
		hitId = -1;
		hitGo = null;
		Transform t = go.transform;
		while ((Object)(object)t != (Object)null)
		{
			int id = ((Object)t.gameObject).GetInstanceID();
			if (ItemTooltipsMod.HasCharacterIcon(id))
			{
				hitId = id;
				hitGo = t.gameObject;
				return true;
			}
			if (HitToUi.TryGetValue(id, out CharacterItemUI ui) && (Object)(object)ui != (Object)null)
			{
				GameObject card = ((Component)ui).gameObject;
				int cid = ((Object)card).GetInstanceID();
				if (ItemTooltipsMod.HasCharacterIcon(cid))
				{
					hitId = cid;
					hitGo = card;
					return true;
				}
			}
			t = t.parent;
		}
		return false;
	}

	private static bool IsBlockingNonCard(GameObject go)
	{
		// If we hit something under an info panel, block
		Transform t = go.transform;
		while ((Object)(object)t != (Object)null)
		{
			string n = t.name ?? "";
			if (n.IndexOf("InfoPanel", StringComparison.OrdinalIgnoreCase) >= 0
				|| n.IndexOf("CharacterInfo", StringComparison.OrdinalIgnoreCase) >= 0
				|| n.IndexOf("Description", StringComparison.OrdinalIgnoreCase) >= 0
				|| n.IndexOf("Detail", StringComparison.OrdinalIgnoreCase) >= 0)
				return true;
			t = t.parent;
		}
		return false;
	}
}
