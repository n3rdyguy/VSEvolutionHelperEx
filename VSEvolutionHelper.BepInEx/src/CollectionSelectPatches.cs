using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using VampireSurvivors.Data;
using VampireSurvivors.UI;
using Object = UnityEngine.Object;

namespace VSItemTooltips;

/// <summary>
/// Collections-page tooltips via delayed scan after Populate/Show.
/// Avoids per-frame FindObjectsOfType&lt;Transform&gt; (that can freeze/crash Unity).
/// </summary>
public static class CollectionSelectPatches
{
	private static float _lastScan = -999f;
	private const float ScanCooldown = 1.0f;
	private static bool _scanQueued;

	public static void Apply(Harmony harmony)
	{
		// Only safe, high-signal hooks - do NOT patch Sort* (re-entrancy / mid-layout)
		TryPatch(harmony, typeof(CollectionsPage), "OnShowStart", nameof(Page_Postfix));
		TryPatch(harmony, typeof(CollectionsPage), "Populate", nameof(Page_Postfix));
	}

	private static bool TryPatch(Harmony harmony, Type type, string methodName, string postfix)
	{
		try
		{
			MethodInfo best = null;
			foreach (var m in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (m.Name != methodName) continue;
				// Prefer declared on CollectionsPage, not base virtuals
				if (m.DeclaringType != type && methodName.StartsWith("OnShow", StringComparison.Ordinal))
					continue;
				if (best == null || m.GetParameters().Length < best.GetParameters().Length)
					best = m;
			}
			// Fallback: any match
			if (best == null)
			{
				foreach (var m in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
				{
					if (m.Name != methodName) continue;
					if (best == null || m.GetParameters().Length < best.GetParameters().Length)
						best = m;
				}
			}
			if (best == null) return false;
			harmony.Patch(best, postfix: new HarmonyMethod(typeof(CollectionSelectPatches), postfix));
			Plugin.Log.LogInfo($"[Collections] Patched {type.Name}.{best.Name} (decl={best.DeclaringType?.Name})");
			return true;
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning($"[Collections] {type.Name}.{methodName}: {ex.Message}");
			return false;
		}
	}

	public static void Page_Postfix()
	{
		// One delayed scan after UI settles - never scan re-entrantly from Sort
		if (_scanQueued) return;
		_scanQueued = true;
		ItemTooltipsMod.DelayFrames(3, () =>
		{
			_scanQueued = false;
			try { ScanAndRegister(); }
			catch (Exception ex) { Plugin.Log.LogWarning("[Collections] delayed scan: " + ex.Message); }
		});
	}

	/// <summary>Light tick: only re-scan if Collections view is open and we have almost no icons.</summary>
	public static void Tick()
	{
		try
		{
			if (!IsCollectionsOpen()) return;
			if (Time.unscaledTime - _lastScan < ScanCooldown) return;
			if (ItemTooltipsMod.CollectionIconCount >= 4) return;
			if (_scanQueued) return;
			_scanQueued = true;
			ItemTooltipsMod.DelayFrames(1, () =>
			{
				_scanQueued = false;
				try { ScanAndRegister(); }
				catch (Exception ex) { Plugin.Log.LogWarning("[Collections] tick scan: " + ex.Message); }
			});
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Collections] Tick: " + ex.Message);
		}
	}

	private static bool IsCollectionsOpen()
	{
		foreach (string path in new[]
		{
			"UI/Canvas - App/Safe Area/View - Collections",
			"UI/Canvas - App/Safe Area/View - Collection",
		})
		{
			try
			{
				var go = GameObject.Find(path);
				if ((Object)(object)go != (Object)null && go.activeInHierarchy)
					return true;
			}
			catch { }
		}
		return false;
	}

	public static void ScanAndRegister()
	{
		_lastScan = Time.unscaledTime;
		try
		{
			if (!IsCollectionsOpen())
				return;

			GameData.EnsureLoaded();
			ItemTooltipsMod.ClearCollectionUnlockHints();

			CollectionItemUI[] uis = null;
			try { uis = Object.FindObjectsOfType<CollectionItemUI>(false); } // active only - safer
			catch
			{
				try { uis = Object.FindObjectsOfType<CollectionItemUI>(); } catch { }
			}
			if (uis == null || uis.Length == 0)
			{
				Plugin.Dbg("[Collections] Scan: 0 CollectionItemUI");
				return;
			}

			int n = 0;
			int len = uis.Length;
			// Cap work per scan to avoid hitches on huge grids
			int max = Math.Min(len, 400);
			for (int i = 0; i < max; i++)
			{
				CollectionItemUI ui = uis[i];
				if ((Object)(object)ui == (Object)null) continue;
				try
				{
					if (!((Component)ui).gameObject.activeInHierarchy) continue;
					if (RegisterOne(ui)) n++;
				}
				catch (Exception ex)
				{
					Plugin.Dbg("[Collections] RegisterOne: " + ex.Message);
				}
			}
			Plugin.Log.LogInfo($"[Collections] Registered {n}/{max} collection cells");
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Collections] ScanAndRegister: " + ex.Message);
		}
	}

	private static bool RegisterOne(CollectionItemUI ui)
	{
		GameObject go = ((Component)ui).gameObject;
		EnsureRaycast(go);

		// Read types via getters only (no heavy child graphic registration here)
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
			// 0 / invalid enum often means "not an item cell"
			string its = it.ToString();
			if (!string.IsNullOrEmpty(its) && its != "0" && Enum.IsDefined(typeof(ItemType), it))
			{
				ItemTooltipsMod.RegisterItemUI(((Object)go).GetInstanceID(), go, it, false);
				if (IsVisuallyLocked(ui))
				{
					string name = GameData.GetItemName(it) ?? GameData.HumanizeEnum(its);
					string hint = GameData.GetItemUnlockHint(it);
					string body = !string.IsNullOrEmpty(hint)
						? "Unlock: " + hint
						: "Locked - keep playing to unlock.";
					Sprite spr = null;
					try
					{
						if ((Object)(object)ui.LockedIcon != (Object)null)
							spr = ui.LockedIcon.sprite;
					}
					catch { }
					ItemTooltipsMod.RegisterCollectionUnlockHint(go, name, body, spr ?? GameData.GetItemSprite(it));
				}
				return true;
			}
		}
		catch { }

		try
		{
			ArcanaType at = ui.GetArcanaType();
			string ats = at.ToString();
			if (!string.IsNullOrEmpty(ats) && ats != "VOID" && ats != "0" && Enum.IsDefined(typeof(ArcanaType), at))
			{
				ItemTooltipsMod.RegisterArcanaUI(((Object)go).GetInstanceID(), go, at);
				return true;
			}
		}
		catch { }

		return false;
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
		return false;
	}

	private static void EnsureRaycast(GameObject go)
	{
		try
		{
			var g = go.GetComponent<Graphic>();
			if ((Object)(object)g != (Object)null)
			{
				g.raycastTarget = true;
				return;
			}
			var img = go.GetComponent<Image>();
			if ((Object)(object)img == (Object)null)
			{
				img = go.AddComponent<Image>();
				img.color = new Color(1f, 1f, 1f, 0.01f);
			}
			((Graphic)img).raycastTarget = true;
		}
		catch { }
	}
}
