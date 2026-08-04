using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using VampireSurvivors.Data;
using VampireSurvivors.Data.Items;
using VampireSurvivors.Data.Weapons;
using VampireSurvivors.UI;
using Object = UnityEngine.Object;

namespace VSItemTooltips;

/// <summary>
/// Reliable Collections-page tooltips via post-Populate scan (SetData-only registration
/// can miss pooled / filtered / tab-switched cells). Also surfaces unlock tips for locked entries.
/// </summary>
public static class CollectionSelectPatches
{
	private static float _lastScan = -999f;
	private const float ScanCooldown = 0.4f;

	public static void Apply(Harmony harmony)
	{
		TryPatch(harmony, typeof(CollectionsPage), "OnShowStart", nameof(Page_Postfix));
		TryPatch(harmony, typeof(CollectionsPage), "OnShowFinish", nameof(Page_Postfix));
		TryPatch(harmony, typeof(CollectionsPage), "Populate", nameof(Page_Postfix));
		// Filter / sort rebuilds the grid without always re-calling every SetData path we expect
		foreach (string n in new[] { "SortByType", "SortByVersion", "Filter", "Refresh", "ShowWeapons", "ShowItems", "ShowArcanas", "ShowRelics" })
			TryPatch(harmony, typeof(CollectionsPage), n, nameof(Page_Postfix));
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
			harmony.Patch(best, postfix: new HarmonyMethod(typeof(CollectionSelectPatches), postfix));
			Plugin.Log.LogInfo($"[Collections] Patched {type.Name}.{best.Name}");
			return true;
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning($"[Collections] {type.Name}.{methodName}: {ex.Message}");
			return false;
		}
	}

	public static void Page_Postfix() => ItemTooltipsMod.DelayFrames(2, ScanAndRegister);

	/// <summary>Called from Update when Collections view is visible.</summary>
	public static void Tick()
	{
		try
		{
			// App UI collections (not in-run pause)
			GameObject view = GameObject.Find("UI/Canvas - App/Safe Area/View - Collections");
			if ((Object)(object)view == (Object)null)
				view = GameObject.Find("UI/Canvas - App/Safe Area/View - Collection");
			bool open = (Object)(object)view != (Object)null && view.activeInHierarchy;
			if (!open)
			{
				// Secrets is often a sibling page under App canvas
				GameObject secrets = FindSecretsView();
				open = (Object)(object)secrets != (Object)null && secrets.activeInHierarchy;
			}
			if (!open) return;
			if (Time.unscaledTime - _lastScan < ScanCooldown) return;
			// Keep registration warm while open (tab switches)
			if (ItemTooltipsMod.CollectionIconCount < 8)
				ScanAndRegister();
		}
		catch { }
	}

	private static GameObject FindSecretsView()
	{
		foreach (string path in new[]
		{
			"UI/Canvas - App/Safe Area/View - Secrets",
			"UI/Canvas - App/Safe Area/View - Secret",
			"UI/Canvas - App/Safe Area/View - SecretsPage",
		})
		{
			try
			{
				var go = GameObject.Find(path);
				if ((Object)(object)go != (Object)null) return go;
			}
			catch { }
		}
		// Name contains search among active canvases (cheap-ish)
		try
		{
			// Any active GO with "Secret" in the name under App canvas
			var all = Object.FindObjectsOfType<Transform>(true);
			if (all != null)
			{
				int len = all.Length;
				for (int i = 0; i < len; i++)
				{
					var t = all[i];
					if ((Object)(object)t == (Object)null) continue;
					string n = ((Object)t).name ?? "";
					if (n.IndexOf("Secret", StringComparison.OrdinalIgnoreCase) < 0) continue;
					if (n.IndexOf("View", StringComparison.OrdinalIgnoreCase) < 0
						&& n.IndexOf("Page", StringComparison.OrdinalIgnoreCase) < 0
						&& n.IndexOf("Panel", StringComparison.OrdinalIgnoreCase) < 0)
						continue;
					if (t.gameObject.activeInHierarchy)
						return t.gameObject;
				}
			}
		}
		catch { }
		return null;
	}

	public static void ScanAndRegister()
	{
		_lastScan = Time.unscaledTime;
		try
		{
			GameData.EnsureLoaded();
			ItemTooltipsMod.ClearCollectionUnlockHints();
			CollectionItemUI[] uis = null;
			try { uis = Object.FindObjectsOfType<CollectionItemUI>(true); }
			catch { try { uis = Object.FindObjectsOfType<CollectionItemUI>(); } catch { } }
			if (uis == null || uis.Length == 0)
			{
				Plugin.Dbg("[Collections] Scan: 0 CollectionItemUI");
				return;
			}

			int n = 0;
			foreach (var ui in uis)
			{
				if ((Object)(object)ui == (Object)null) continue;
				if (!((Component)ui).gameObject.activeInHierarchy) continue;
				if (RegisterOne(ui)) n++;
			}
			Plugin.Log.LogInfo($"[Collections] Registered {n}/{uis.Length} collection cells for tooltips");
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Collections] ScanAndRegister: " + ex.Message);
		}
	}

	private static bool RegisterOne(CollectionItemUI ui)
	{
		try
		{
			GameObject go = ((Component)ui).gameObject;
			EnsureRaycast(go);

			// Prefer typed getters on CollectionItemUI
			if (TryWeapon(ui, go)) return true;
			if (TryItem(ui, go)) return true;
			if (TryArcana(ui, go)) return true;

			// Fallback: fields
			try
			{
				WeaponType wt = ui.GetWeaponType();
				if (GameData.IsRealWeaponType(wt))
				{
					ItemTooltipsMod.RegisterWeaponUI(((Object)go).GetInstanceID(), go, wt, false);
					return true;
				}
			}
			catch { }
			try
			{
				ItemType it = ui.GetItemType();
				if (Enum.IsDefined(typeof(ItemType), it))
				{
					RegisterItemWithUnlockHint(go, it, ui);
					return true;
				}
			}
			catch { }
			try
			{
				ArcanaType at = ui.GetArcanaType();
				if (at.ToString() != "VOID" && Enum.IsDefined(typeof(ArcanaType), at))
				{
					ItemTooltipsMod.RegisterArcanaUI(((Object)go).GetInstanceID(), go, at);
					return true;
				}
			}
			catch { }

			return false;
		}
		catch (Exception ex)
		{
			Plugin.Dbg("[Collections] RegisterOne: " + ex.Message);
			return false;
		}
	}

	private static bool TryWeapon(CollectionItemUI ui, GameObject go)
	{
		try
		{
			if (!ui.IsWeapon() && !ui.IsPassive()) return false;
		}
		catch { /* fall through and try type anyway */ }
		try
		{
			WeaponType wt = ui.GetWeaponType();
			if (!GameData.IsRealWeaponType(wt)) return false;
			ItemTooltipsMod.RegisterWeaponUI(((Object)go).GetInstanceID(), go, wt, false);
			// Also map icon child for easier hit
			try
			{
				if ((Object)(object)ui.UnlockedIcon != (Object)null)
				{
					var ig = ((Component)ui.UnlockedIcon).gameObject;
					ItemTooltipsMod.RegisterWeaponUI(((Object)ig).GetInstanceID(), ig, wt, false);
				}
			}
			catch { }
			return true;
		}
		catch { return false; }
	}

	private static bool TryItem(CollectionItemUI ui, GameObject go)
	{
		try
		{
			if (!ui.IsItem() && !ui.IsRelic()) return false;
		}
		catch { }
		try
		{
			ItemType it = ui.GetItemType();
			RegisterItemWithUnlockHint(go, it, ui);
			try
			{
				if ((Object)(object)ui.UnlockedIcon != (Object)null)
				{
					var ig = ((Component)ui.UnlockedIcon).gameObject;
					RegisterItemWithUnlockHint(ig, it, ui);
				}
			}
			catch { }
			return true;
		}
		catch { return false; }
	}

	private static bool TryArcana(CollectionItemUI ui, GameObject go)
	{
		try
		{
			if (!ui.IsArcana()) return false;
		}
		catch { }
		try
		{
			ArcanaType at = ui.GetArcanaType();
			ItemTooltipsMod.RegisterArcanaUI(((Object)go).GetInstanceID(), go, at);
			return true;
		}
		catch { return false; }
	}

	/// <summary>
	/// Items/relics: normal item tooltip registration. Unlock hints are folded into description
	/// lookup via GameData when achievement tips exist (see GetItemUnlockHint).
	/// </summary>
	private static void RegisterItemWithUnlockHint(GameObject go, ItemType it, CollectionItemUI ui)
	{
		ItemTooltipsMod.RegisterItemUI(((Object)go).GetInstanceID(), go, it, false);
		// If locked, also register as simple map-style so we can show unlock tip even when
		// full item tooltip would be empty/hidden
		bool locked = IsVisuallyLocked(ui);
		if (locked)
		{
			string name = GameData.GetItemName(it) ?? GameData.HumanizeEnum(it.ToString());
			string hint = GameData.GetItemUnlockHint(it);
			string body = !string.IsNullOrEmpty(hint)
				? "Unlock: " + hint
				: "Locked — keep playing to unlock.";
			Sprite spr = null;
			try
			{
				if ((Object)(object)ui.LockedIcon != (Object)null)
					spr = ui.LockedIcon.sprite;
				if ((Object)(object)spr == (Object)null && (Object)(object)ui.UnlockedIcon != (Object)null)
					spr = ui.UnlockedIcon.sprite;
			}
			catch { }
			ItemTooltipsMod.RegisterCollectionUnlockHint(go, name, body, spr ?? GameData.GetItemSprite(it));
		}
	}

	private static bool IsVisuallyLocked(CollectionItemUI ui)
	{
		try
		{
			var locked = ui.LockedIcon;
			if ((Object)(object)locked != (Object)null && ((Component)locked).gameObject.activeInHierarchy)
				return true;
		}
		catch { }
		try
		{
			var unlocked = ui.UnlockedIcon;
			if ((Object)(object)unlocked != (Object)null && !((Component)unlocked).gameObject.activeInHierarchy)
				return true;
		}
		catch { }
		return false;
	}

	private static void EnsureRaycast(GameObject go)
	{
		try
		{
			var g = go.GetComponent<Graphic>();
			if ((Object)(object)g != (Object)null)
				g.raycastTarget = true;
			else
			{
				var img = go.GetComponent<Image>();
				if ((Object)(object)img == (Object)null)
				{
					img = go.AddComponent<Image>();
					img.color = new Color(1f, 1f, 1f, 0.01f);
				}
				((Graphic)img).raycastTarget = true;
			}
		}
		catch { }
	}
}
