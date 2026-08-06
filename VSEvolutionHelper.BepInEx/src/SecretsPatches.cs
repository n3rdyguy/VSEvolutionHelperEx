using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using VampireSurvivors.Data;
using VampireSurvivors.UI;
using Object = UnityEngine.Object;

namespace VSItemTooltips;

/// <summary>
/// Secrets page: show what each secret awards.
///
/// SecretData keeps every reward kind in its own nullable field (character, weapon, stage,
/// hyper, relic, arcana, power-up, skins, gold), so the rewards are readable whether or not
/// the player has found the secret. Revealing un-achieved secrets is a deliberate choice -
/// the game obscures them by design - so it is behind a config switch.
///
/// Rows are SelectableUI. The hover is attached to the reward image rather than the row root
/// so the row's own button wiring is untouched, and the root is mapped separately for
/// keyboard/pad, which resolve the selection by walking up from the selected object.
/// </summary>
public static class SecretsPatches
{
	private static readonly RowTooltipRegistry Rows = new RowTooltipRegistry("Secrets");

	public static void Apply(Harmony harmony)
	{
		if (!Plugin.SecretTooltipsEnabled)
		{
			Plugin.Log.LogInfo("[Secrets] Disabled by config");
			return;
		}
		try
		{
			foreach (var m in typeof(SecretItemUI).GetMethods(
				System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
			{
				if (m.Name != "SetData") continue;
				harmony.Patch(m, postfix: new HarmonyMethod(typeof(SecretsPatches), nameof(SetData_Postfix)));
				Plugin.Log.LogInfo("[Secrets] Patched SecretItemUI.SetData");
				return;
			}
			Plugin.Log.LogWarning("[Secrets] SecretItemUI.SetData not found");
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Secrets] patch: " + ex.Message);
		}
	}

	/// <summary>
	/// Instance-only postfix - the data is read back off the row rather than taken from the
	/// patched call's arguments, matching the other IL2CPP postfixes here.
	/// </summary>
	public static void SetData_Postfix(SecretItemUI __instance)
	{
		try
		{
			if ((Object)(object)__instance == (Object)null) return;

			SecretType type = default;
			bool haveType = false;
			try { type = __instance._type; haveType = true; } catch { }

			// Only used for the debug line and the typed fallback - the rewards themselves
			// come from the JSON, since both the row's copy and the catalog record read back
			// with every reward field VOID.
			SecretData data = null;
			if (haveType) data = GameData.GetSecretData(type);
			if (data == null) { try { data = __instance._data; } catch { } }

			bool achieved = false;
			try { achieved = __instance._hasAchieved; } catch { }
			if (!achieved && !Plugin.SecretSpoilers) return;

			string source = "json";
			string key = haveType ? type.ToString() : null;
			var rows = GameData.GetSecretRewards(key);
			if ((rows == null || rows.Count == 0) && data != null)
			{
				source = "typed";
				rows = GameData.GetSecretRewards(data);
			}
			if (rows == null || rows.Count == 0)
			{
				if (Plugin.DebugVerbose)
					Plugin.Dbg($"Secrets: no readable rewards ({source}) for " + DescribeSecret(__instance, data));
				return;
			}
			if (Plugin.DebugVerbose)
			{
				var labels = new List<string>();
				foreach (var r in rows) labels.Add(r.Label);
				Plugin.Dbg($"Secrets({source}): {DescribeSecret(__instance, data)} -> {string.Join(" | ", labels)}");
			}

			GameObject root = ((Component)__instance).gameObject;
			GameObject target = RewardImage(__instance);

			Rows.Register(root, target, new RowTooltipRegistry.Entry
			{
				Title = achieved ? "Secret (found)" : "Secret (not found yet)",
				Rows = rows,
				SectionHeader = "Unlocks:",
				Offset = new Vector2(ItemTooltipsMod.SecretPanelX, ItemTooltipsMod.SecretPanelTopY),
				Pivot = ItemTooltipsMod.SecretPanelPivot,
			});

			Plugin.Dbg($"Secrets: registered {rows.Count} on "
				+ $"{((Object)(object)target != (Object)null ? ((Object)target).name : "-")}+root achieved={achieved}");
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Secrets] SetData postfix: " + ex.Message);
		}
	}

	/// <summary>Identify a secret in the log by its type and raw field state.</summary>
	private static string DescribeSecret(SecretItemUI item, SecretData data)
	{
		string type = "?";
		try { type = item._type.ToString(); } catch { }
		string chr = "-", wpn = "-", stg = "-", hyp = "-", rel = "-", arc = "-", pwr = "-", gold = "-";
		try { if (data.characterToUnlock != null && data.characterToUnlock.HasValue) chr = data.characterToUnlock.Value.ToString(); } catch { }
		try { if (data.weaponToUnlock != null && data.weaponToUnlock.HasValue) wpn = data.weaponToUnlock.Value.ToString(); } catch { }
		try { if (data.stageToUnlock != null && data.stageToUnlock.HasValue) stg = data.stageToUnlock.Value.ToString(); } catch { }
		try { if (data.hyperToUnlock != null && data.hyperToUnlock.HasValue) hyp = data.hyperToUnlock.Value.ToString(); } catch { }
		try { if (data.relicToUnlock != null && data.relicToUnlock.HasValue) rel = data.relicToUnlock.Value.ToString(); } catch { }
		try { if (data.arcanaToUnlock != null && data.arcanaToUnlock.HasValue) arc = data.arcanaToUnlock.Value.ToString(); } catch { }
		try { if (data.powerUpToUnlock != null && data.powerUpToUnlock.HasValue) pwr = data.powerUpToUnlock.Value.ToString(); } catch { }
		try { if (data.goldPrize != null && data.goldPrize.HasValue) gold = data.goldPrize.Value.ToString(); } catch { }
		string desc = "-", spell = "-", special = "-", flags = "";
		try { desc = Trim(data.description); } catch { }
		try { spell = Trim(data.spell); } catch { }
		try { special = Trim(data.special); } catch { }
		try { flags = $"myst={data.mistery} hid={data.hidden} spellF={data.isSpell} mod={data.isModifier}"; } catch { }
		// What the game itself paints as the reward icon - if that sprite resolves to a type,
		// it is a usable source even though the data fields are empty.
		string rewardSprite = "-";
		try
		{
			Image r = item._Reward;
			if ((Object)(object)r != (Object)null && (Object)(object)r.sprite != (Object)null)
				rewardSprite = ((Object)r.sprite).name;
		}
		catch { }
		return $"{type}[c={chr} w={wpn} s={stg} h={hyp} r={rel} a={arc} p={pwr} g={gold}] "
			+ $"sprite={rewardSprite} desc='{desc}' spell='{spell}' special='{special}' {flags}";
	}

	private static string Trim(string s)
	{
		if (string.IsNullOrEmpty(s)) return "-";
		s = s.Replace('\n', ' ').Replace('\r', ' ');
		return s.Length > 60 ? s.Substring(0, 60) + "..." : s;
	}

	private static GameObject RewardImage(SecretItemUI item)
	{
		try
		{
			Image img = item._Reward;
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
