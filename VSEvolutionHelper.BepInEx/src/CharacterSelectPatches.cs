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
/// Character Selection: hover tooltips with starter weapon + evolution path(s).
/// </summary>
public static class CharacterSelectPatches
{
	public static void Apply(Harmony harmony)
	{
		try
		{
			// Find SetData that takes CharacterItem (4th arg)
			MethodInfo setData = null;
			foreach (var m in typeof(CharacterItemUI).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (m.Name != "SetData") continue;
				var ps = m.GetParameters();
				if (ps.Length >= 4 && ps[3].ParameterType == typeof(CharacterItem))
				{
					setData = m;
					break;
				}
			}
			if (setData == null)
			{
				foreach (var m in typeof(CharacterItemUI).GetMethods(BindingFlags.Instance | BindingFlags.Public))
				{
					if (m.Name == "SetData" && m.GetParameters().Length >= 4)
					{
						setData = m;
						break;
					}
				}
			}

			if (setData != null)
			{
				harmony.Patch(setData, postfix: new HarmonyMethod(typeof(CharacterSelectPatches), nameof(SetData_Postfix)));
				Plugin.Log.LogInfo("[CharacterSelect] Patched CharacterItemUI.SetData");
			}
			else
				Plugin.Log.LogWarning("[CharacterSelect] CharacterItemUI.SetData not found");
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[CharacterSelect] SetData patch: " + ex.Message);
		}

		try
		{
			foreach (string name in new[] { "OnHideStart", "OnHide", "Hide", "Close" })
			{
				var m = typeof(CharacterSelectionPage).GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (m != null && m.GetParameters().Length == 0)
				{
					harmony.Patch(m, postfix: new HarmonyMethod(typeof(CharacterSelectPatches), nameof(Hide_Postfix)));
					Plugin.Log.LogInfo($"[CharacterSelect] Patched CharacterSelectionPage.{name} for cleanup");
					break;
				}
			}
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[CharacterSelect] hide patch: " + ex.Message);
		}
	}

	public static void SetData_Postfix(CharacterItemUI __instance, CharacterItem characterItem)
	{
		try
		{
			if (!Plugin.CharacterTooltipsEnabled)
				return;
			if ((Object)(object)__instance == (Object)null || characterItem == null)
				return;

			GameData.EnsureLoaded();

			CharacterType ctype = default;
			CharacterData cdata = null;
			try { ctype = characterItem.CharacterType; } catch { }
			try { cdata = characterItem.CharacterData; } catch { }
			if (cdata == null)
			{
				try { cdata = characterItem._characterData; } catch { }
			}

			string name = BuildCharacterName(__instance, cdata, ctype);
			string desc = BuildCharacterTooltip(cdata, ctype, name);
			Sprite spr = ResolveCharacterSprite(__instance, cdata, ctype);
			GameObject hit = ResolveHitTarget(__instance);

			ItemTooltipsMod.RegisterCharacterIcon(hit, ctype, name, desc, spr);
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[CharacterSelect] SetData postfix: " + ex.Message);
		}
	}

	public static void Hide_Postfix()
	{
		ItemTooltipsMod.ClearCharacterIcons();
	}

	private static GameObject ResolveHitTarget(CharacterItemUI ui)
	{
		// Prefer the full card root for larger hover area
		GameObject go = ((Component)ui).gameObject;
		try
		{
			// Weapon icon is a nice secondary target if card has no raycast
			Image weapon = ui._WeaponIcon;
			if ((Object)(object)weapon != (Object)null)
			{
				// Keep root if it has a Graphic raycast; else use weapon
				var g = go.GetComponent<Graphic>();
				if ((Object)(object)g == (Object)null || !((Graphic)g).raycastTarget)
				{
					// Still register the root — Image children usually catch hits
				}
			}
		}
		catch { }
		return go;
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
			if (data != null)
			{
				string prefix = data.prefix ?? "";
				string charName = data.charName ?? "";
				string surname = data.surname ?? "";
				string combined = $"{prefix} {charName} {surname}".Trim();
				if (!string.IsNullOrWhiteSpace(combined))
					return combined;
			}
		}
		catch { }
		return type.ToString();
	}

	private static Sprite ResolveCharacterSprite(CharacterItemUI ui, CharacterData data, CharacterType type)
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
			if (data != null)
				return ui.GetCharSprite(type, data);
		}
		catch { }
		return null;
	}

	private static string BuildCharacterTooltip(CharacterData data, CharacterType type, string displayName)
	{
		var sb = new StringBuilder();

		// Flavor / description
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

		// Starting weapon + evo paths
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

		// Compact stat bonuses (only non-default-ish)
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
				// Field fallback
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
				// Many char stats are deltas (0 = default)
				string fmt = Mathf.Abs(v) >= 10f || Mathf.Approximately(v, Mathf.Round(v))
					? v.ToString("0")
					: v.ToString("0.##");
				lines.Add($"  {label}: {fmt}");
			}
			add("Max HP", data.maxHp);
			add("Armor", data.armor);
			add("Regen", data.regen);
			add("Move Speed", data.moveSpeed, 1f); // 1 often means default
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
		// Cap length so popup stays readable
		if (lines.Count > 8)
			lines = lines.GetRange(0, 8);
		return string.Join("\n", lines);
	}
}
