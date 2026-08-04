using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using VampireSurvivors.App.Data.Adventures;
using VampireSurvivors.App.Scripts.Data;
using VampireSurvivors.App.Scripts.UI;
using VampireSurvivors.App.UI;
using VampireSurvivors.Data;
using Object = UnityEngine.Object;

namespace VSItemTooltips;

/// <summary>
/// Adventures list: hover tooltips with cast / weapons summary.
/// Scan after Populate — do not Harmony-patch AdventureItemUI.SetData (IL2CPP risk).
/// </summary>
public static class AdventureSelectPatches
{
	private static float _lastScan = -999f;
	private const float ScanCooldown = 0.6f;
	private static readonly Dictionary<int, AdventureItemUI> HitToUi = new Dictionary<int, AdventureItemUI>();

	public static void Apply(Harmony harmony)
	{
		TryPatch(harmony, typeof(SelectAdventuresPage), "Populate", nameof(Populate_Postfix));
		TryPatch(harmony, typeof(SelectAdventuresPage), "SelectAdventure", nameof(Select_Postfix));
		foreach (string name in new[] { "OnHideStart", "OnHide", "Hide", "Close" })
		{
			if (TryPatch(harmony, typeof(SelectAdventuresPage), name, nameof(Hide_Postfix)))
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
			harmony.Patch(best, postfix: new HarmonyMethod(typeof(AdventureSelectPatches), postfix));
			Plugin.Log.LogInfo($"[AdventureSelect] Patched {type.Name}.{best.Name}");
			return true;
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning($"[AdventureSelect] {type.Name}.{methodName}: {ex.Message}");
			return false;
		}
	}

	public static void Populate_Postfix() => ScheduleScan(3);
	public static void Select_Postfix() => ScheduleScan(1);

	public static void Hide_Postfix()
	{
		HitToUi.Clear();
		ItemTooltipsMod.ClearAdventureIcons();
	}

	private static void ScheduleScan(int frames)
	{
		if (!Plugin.AdventureTooltipsEnabled) return;
		ItemTooltipsMod.DelayFrames(frames, ScanAndRegister);
	}

	public static void Tick()
	{
		if (!Plugin.AdventureTooltipsEnabled) return;
		if (ItemTooltipsMod.AdventureIconCount > 0) return;
		if (Time.unscaledTime - _lastScan < ScanCooldown) return;
		try
		{
			var any = Object.FindObjectOfType<AdventureItemUI>();
			if ((Object)(object)any == (Object)null || !((Component)any).gameObject.activeInHierarchy)
				return;
		}
		catch { return; }
		ScanAndRegister();
	}

	public static void ScanAndRegister()
	{
		if (!Plugin.AdventureTooltipsEnabled) return;
		_lastScan = Time.unscaledTime;
		try
		{
			GameData.EnsureLoaded();
			HitToUi.Clear();
			ItemTooltipsMod.ClearAdventureIcons();

			AdventureItemUI[] uis = null;
			try { uis = Object.FindObjectsOfType<AdventureItemUI>(true); }
			catch { try { uis = Object.FindObjectsOfType<AdventureItemUI>(); } catch { } }
			if (uis == null || uis.Length == 0) return;

			int n = 0;
			foreach (var ui in uis)
			{
				if ((Object)(object)ui == (Object)null) continue;
				if (!((Component)ui).gameObject.activeInHierarchy) continue;
				if (RegisterOne(ui)) n++;
			}
			if (n > 0)
				Plugin.Log.LogInfo($"[AdventureSelect] Registered {n} adventure cards");
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[AdventureSelect] ScanAndRegister: " + ex.Message);
		}
	}

	private static bool RegisterOne(AdventureItemUI ui)
	{
		AdventureData data = null;
		AdventureType type = default;
		try { data = ui.GetAdventureData(); } catch { try { data = ui._data; } catch { } }
		try { type = ui.GetAdventureType(); } catch { try { type = ui._type; } catch { } }
		if (data == null) return false;

		string title = ResolveTitle(ui, data, type);
		string body = BuildBody(data, type);
		Sprite spr = null;
		try
		{
			if ((Object)(object)ui._Icon != (Object)null)
				spr = ui._Icon.sprite;
		}
		catch { }

		GameObject card = ((Component)ui).gameObject;
		EnsureRaycast(card);
		int id = ((Object)card).GetInstanceID();
		HitToUi[id] = ui;
		ItemTooltipsMod.RegisterAdventureIcon(card, title, body, spr);
		return true;
	}

	private static void EnsureRaycast(GameObject card)
	{
		try
		{
			var g = card.GetComponent<Graphic>();
			if ((Object)(object)g != (Object)null)
				g.raycastTarget = true;
			else
			{
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

	private static string ResolveTitle(AdventureItemUI ui, AdventureData data, AdventureType type)
	{
		try
		{
			if ((Object)(object)ui._Title != (Object)null)
			{
				string t = ((TMPro.TMP_Text)ui._Title).text;
				if (!string.IsNullOrWhiteSpace(t)) return t.Trim();
			}
		}
		catch { }
		try
		{
			CoreAdventureData core = data.CoreAdventureData;
			if (core != null && !string.IsNullOrWhiteSpace(core.AdventureName))
				return core.AdventureName.Trim();
		}
		catch { }
		return type.ToString();
	}

	private static string BuildBody(AdventureData data, AdventureType type)
	{
		var sb = new StringBuilder();
		try
		{
			sb.AppendLine($"Adventure: {type}");
			try
			{
				var sst = data.StageSetType;
				sb.AppendLine($"Stage set: {sst}");
			}
			catch { }

			// Characters
			try
			{
				var chars = data.CharacterTypes;
				if (chars != null && chars.Count > 0)
				{
					sb.AppendLine();
					sb.AppendLine($"Characters ({chars.Count}):");
					int shown = 0;
					for (int i = 0; i < chars.Count && shown < 12; i++)
					{
						sb.AppendLine("  • " + HumanizeEnum(chars[i].ToString()));
						shown++;
					}
					if (chars.Count > shown)
						sb.AppendLine($"  … +{chars.Count - shown} more");
				}
			}
			catch { }

			// Weapons
			try
			{
				var weps = data.WeaponTypes;
				if (weps != null && weps.Count > 0)
				{
					sb.AppendLine();
					sb.AppendLine($"Weapons ({weps.Count}):");
					int shown = 0;
					for (int i = 0; i < weps.Count && shown < 12; i++)
					{
						WeaponType wt = weps[i];
						string n = GameData.GetWeaponName(wt);
						if (string.IsNullOrEmpty(n)) n = HumanizeEnum(wt.ToString());
						sb.AppendLine("  • " + n);
						shown++;
					}
					if (weps.Count > shown)
						sb.AppendLine($"  … +{weps.Count - shown} more");
				}
			}
			catch { }

			try
			{
				var prog = data.ProgressData;
				if (prog != null && prog.Count > 0)
					sb.AppendLine().AppendLine($"Progress goals: {prog.Count}");
			}
			catch { }
		}
		catch (Exception ex)
		{
			Plugin.Dbg("[AdventureSelect] BuildBody: " + ex.Message);
		}
		string s = sb.ToString().TrimEnd();
		return string.IsNullOrEmpty(s) ? "(no adventure details)" : s;
	}

	private static string HumanizeEnum(string raw)
	{
		if (string.IsNullOrEmpty(raw)) return raw;
		raw = raw.Replace("ADV_", "").Replace("TP_", "").Replace('_', ' ');
		return raw;
	}

	public static bool TryFindHovered(Vector2 screenPos, out int hitId, out GameObject hitGo)
	{
		hitId = -1;
		hitGo = null;
		float best = float.MaxValue;
		foreach (var kv in HitToUi)
		{
			var ui = kv.Value;
			if ((Object)(object)ui == (Object)null) continue;
			GameObject card = ((Component)ui).gameObject;
			if (!card.activeInHierarchy) continue;
			var rt = card.GetComponent<RectTransform>();
			if ((Object)(object)rt == (Object)null) continue;
			Camera cam = null;
			try
			{
				var c = card.GetComponentInParent<Canvas>();
				if ((Object)(object)c != (Object)null && c.renderMode != RenderMode.ScreenSpaceOverlay)
					cam = c.worldCamera;
			}
			catch { }
			if (!RectTransformUtility.RectangleContainsScreenPoint(rt, screenPos, cam)
				&& !RectTransformUtility.RectangleContainsScreenPoint(rt, screenPos, null))
				continue;
			float area = Mathf.Max(1f, Mathf.Abs(rt.rect.width) * Mathf.Abs(rt.rect.height));
			if (area < best)
			{
				best = area;
				hitGo = card;
				hitId = ((Object)card).GetInstanceID();
			}
		}
		return (Object)(object)hitGo != (Object)null && ItemTooltipsMod.HasAdventureIcon(hitId);
	}
}
