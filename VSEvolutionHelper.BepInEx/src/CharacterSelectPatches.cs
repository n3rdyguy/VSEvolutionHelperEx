using System;
using System.Reflection;
using System.Text;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using VampireSurvivors.Data;
using VampireSurvivors.Data.Characters;
using VampireSurvivors.UI;
using Object = UnityEngine.Object;

namespace VSItemTooltips;

/// <summary>
/// Character Selection tooltips (starter weapon / evo).
/// IMPORTANT: do NOT Harmony-patch CharacterItemUI.SetData — under IL2CPP that detour
/// breaks population (every card stuck on prefab default Pasqualina / blank sprites).
/// Instead: after Populate / show, scan existing CharacterItemUI instances read-only.
/// </summary>
public static class CharacterSelectPatches
{
	private static float _lastScanTime = -999f;
	private const float ScanCooldown = 0.75f;

	public static void Apply(Harmony harmony)
	{
		// Never patch CharacterItemUI.SetData (IL2CPP detour corrupts char select).

		// After list is built
		TryPatch(harmony, typeof(CharacterSelectionPage), "Populate", nameof(Populate_Postfix), requireZeroParams: false);
		TryPatch(harmony, typeof(CharacterSelectionPage), "RefreshCharacters", nameof(Populate_Postfix), requireZeroParams: false);
		TryPatch(harmony, typeof(CharacterSelectionPage), "OnShowFinish", nameof(Show_Postfix), requireZeroParams: false);
		TryPatch(harmony, typeof(CharacterSelectionPage), "OnShowStart", nameof(Show_Postfix), requireZeroParams: false);

		// Cleanup
		foreach (string name in new[] { "OnHideStart", "OnHideFinish", "OnHide", "Hide", "Close" })
		{
			if (TryPatch(harmony, typeof(CharacterSelectionPage), name, nameof(Hide_Postfix), requireZeroParams: false))
				break;
		}
	}

	private static bool TryPatch(Harmony harmony, Type type, string methodName, string postfix, bool requireZeroParams)
	{
		try
		{
			MethodInfo best = null;
			foreach (var m in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (m.Name != methodName) continue;
				if (requireZeroParams && m.GetParameters().Length != 0) continue;
				// Prefer zero-arg overloads when several exist
				if (best == null || m.GetParameters().Length < best.GetParameters().Length)
					best = m;
			}
			if (best == null)
				return false;
			harmony.Patch(best, postfix: new HarmonyMethod(typeof(CharacterSelectPatches), postfix));
			Plugin.Log.LogInfo($"[CharacterSelect] Patched {type.Name}.{best.Name} ({best.GetParameters().Length} args)");
			return true;
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning($"[CharacterSelect] {methodName} patch: {ex.Message}");
			return false;
		}
	}

	public static void Populate_Postfix()
	{
		ScheduleScan(2);
	}

	public static void Show_Postfix()
	{
		ScheduleScan(3);
	}

	public static void Hide_Postfix()
	{
		ItemTooltipsMod.ClearCharacterIcons();
	}

	private static void ScheduleScan(int frames)
	{
		if (!Plugin.CharacterTooltipsEnabled)
			return;
		ItemTooltipsMod.DelayFrames(frames, ScanAndRegister);
	}

	/// <summary>Optional light rescan from Update if icons empty while page looks open.</summary>
	public static void Tick()
	{
		if (!Plugin.CharacterTooltipsEnabled)
			return;
		if (ItemTooltipsMod.CharacterIconCount > 0)
			return;
		if (Time.unscaledTime - _lastScanTime < ScanCooldown)
			return;
		// Cheap probe: is a CharacterItemUI active?
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
		if (!Plugin.CharacterTooltipsEnabled)
			return;
		_lastScanTime = Time.unscaledTime;
		try
		{
			GameData.EnsureLoaded();
			ItemTooltipsMod.ClearCharacterIcons();

			CharacterItemUI[] uis = null;
			try
			{
				// includeInactive: pooled / off-screen cards
				uis = Object.FindObjectsOfType<CharacterItemUI>(true);
			}
			catch
			{
				try { uis = Object.FindObjectsOfType<CharacterItemUI>(); } catch { }
			}
			if (uis == null || uis.Length == 0)
			{
				Plugin.Dbg("[CharacterSelect] scan: no CharacterItemUI found");
				return;
			}

			int n = 0;
			foreach (CharacterItemUI ui in uis)
			{
				if ((Object)(object)ui == (Object)null) continue;
				try
				{
					if (!((Component)ui).gameObject.activeInHierarchy)
						continue;
					if (!RegisterOne(ui))
						continue;
					n++;
				}
				catch (Exception ex)
				{
					Plugin.Dbg("[CharacterSelect] register one: " + ex.Message);
				}
			}
			Plugin.Dbg($"[CharacterSelect] scan registered {n}/{uis.Length} cards");
			if (n > 0)
				Plugin.Log.LogInfo($"[CharacterSelect] Registered tooltips for {n} characters");
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[CharacterSelect] ScanAndRegister: " + ex.Message);
		}
	}

	private static bool RegisterOne(CharacterItemUI ui)
	{
		CharacterItem characterItem = null;
		try { characterItem = ui.CharacterItem; } catch { }
		if (characterItem == null)
			return false;

		CharacterType ctype = default;
		CharacterData cdata = null;
		try { ctype = characterItem.CharacterType; } catch { try { ctype = characterItem._characterType; } catch { } }
		try { cdata = characterItem.CharacterData; } catch { }
		if (cdata == null)
		{
			try { cdata = characterItem._characterData; } catch { }
		}

		string name = BuildCharacterName(ui, cdata, ctype);
		// Skip still-uninitialized prefab shells
		if (string.IsNullOrWhiteSpace(name))
			return false;

		string desc = BuildCharacterTooltip(cdata, ctype);
		Sprite spr = ResolveCharacterSprite(ui);
		GameObject hit = ((Component)ui).gameObject;

		ItemTooltipsMod.RegisterCharacterIcon(hit, ctype, name, desc, spr);
		return true;
	}

	private static string BuildCharacterName(CharacterItemUI ui, CharacterData data, CharacterType type)
	{
		try
		{
			string n = ui.CharacterName;
			if (!string.IsNullOrWhiteSpace(n))
				return n.Trim();
		}
		catch { }
		try
		{
			if ((Object)(object)ui._CharacterName != (Object)null)
			{
				string t = ((TMPro.TMP_Text)ui._CharacterName).text;
				if (!string.IsNullOrWhiteSpace(t))
					return t.Trim();
			}
		}
		catch { }
		try
		{
			if (data != null)
			{
				string combined = $"{data.prefix} {data.charName} {data.surname}".Trim();
				if (!string.IsNullOrWhiteSpace(combined))
					return combined;
			}
		}
		catch { }
		try
		{
			string s = type.ToString();
			if (!string.IsNullOrEmpty(s) && s != "0")
				return s;
		}
		catch { }
		return null;
	}

	private static Sprite ResolveCharacterSprite(CharacterItemUI ui)
	{
		// Read-only — do not call GetCharSprite (can hit asset pipelines / side effects)
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

	private static string BuildCharacterTooltip(CharacterData data, CharacterType type)
	{
		var sb = new StringBuilder();

		string flavor = null;
		try
		{
			if (data != null)
				flavor = data.GetDescription(type);
		}
		catch { }
		if (string.IsNullOrWhiteSpace(flavor))
		{
			try { flavor = data != null ? data.description : null; } catch { }
		}
		if (!string.IsNullOrWhiteSpace(flavor))
			sb.AppendLine(flavor.Trim());

		WeaponType? starter = TryGetStartingWeapon(data);
		if (starter.HasValue)
		{
			if (sb.Length > 0) sb.AppendLine();
			string wName = GameData.GetWeaponName(starter.Value);
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
						var parts = new System.Collections.Generic.List<string>();
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
			else
			{
				sb.AppendLine("  (no known evolution path)");
			}
		}
		else if (sb.Length == 0)
		{
			sb.Append("No starting weapon data.");
		}

		string stats = FormatNotableStats(data);
		if (!string.IsNullOrEmpty(stats))
		{
			if (sb.Length > 0) sb.AppendLine();
			sb.AppendLine("Notable stats:");
			sb.Append(stats);
		}

		return sb.ToString().TrimEnd();
	}

	private static WeaponType? TryGetStartingWeapon(CharacterData data)
	{
		if (data == null) return null;
		try
		{
			Il2CppSystem.Nullable<WeaponType> sw = data.startingWeapon;
			if (sw != null && sw.HasValue)
				return sw.Value;
		}
		catch
		{
			try
			{
				var field = data._startingWeapon_k__BackingField;
				if (field != null && field.HasValue)
					return field.Value;
			}
			catch { }
		}
		return null;
	}

	private static string FormatNotableStats(CharacterData data)
	{
		if (data == null) return null;
		var lines = new System.Collections.Generic.List<string>();
		try
		{
			void add(string label, float v, float ignoreNear = 0f)
			{
				if (Mathf.Abs(v - ignoreNear) < 0.001f) return;
				if (Mathf.Approximately(v, 0f)) return;
				string fmt = Mathf.Abs(v) >= 10f || Mathf.Approximately(v, Mathf.Round(v))
					? v.ToString("0")
					: v.ToString("0.##");
				lines.Add($"  {label}: {fmt}");
			}
			add("Max HP", data.maxHp);
			add("Armor", data.armor);
			add("Regen", data.regen);
			add("Move Speed", data.moveSpeed, 1f);
			add("Area", data.area, 1f);
			add("Speed", data.speed, 1f);
			add("Duration", data.duration, 1f);
			add("Amount", data.amount);
			add("Cooldown", data.cooldown, 1f);
			add("Luck", data.luck, 1f);
			add("Growth", data.growth, 1f);
			add("Greed", data.greed, 1f);
			add("Magnet", data.magnet);
			add("Revivals", data.revivals);
			add("Curse", data.curse);
			add("Rerolls", data.reRolls);
			add("Skips", data.skips);
			add("Banish", data.banish);
		}
		catch (Exception ex)
		{
			Plugin.Dbg("[CharacterSelect] stats: " + ex.Message);
		}
		if (lines.Count == 0) return null;
		if (lines.Count > 8)
			lines = lines.GetRange(0, 8);
		return string.Join("\n", lines);
	}
}
