using System;
using System.Text;
using UnityEngine;
using VampireSurvivors.UI;
using Object = UnityEngine.Object;

namespace VSItemTooltips;

/// <summary>
/// Reach the Achievements page on Steam.
///
/// MainMenuPage exposes ShowAchievements alongside ShowCollections / ShowBestiary /
/// ShowSecrets, but unlike those three it has no button field on the page, and the button does
/// not appear on this build. The likely reason is that Steam tracks achievements itself, so the
/// in-game list is only surfaced on platforms that have no native equivalent - the game ships
/// both a SteamworksAchievementsManager and a DummyAchievementsManager.
///
/// That cannot be confirmed by reading, because interop assemblies carry signatures only, so
/// this does two things and the log says which one worked:
///   1. Looks for an achievements button that exists but is hidden, and re-enables it.
///   2. Offers a hotkey that calls ShowAchievements() directly.
/// </summary>
public static class MainMenuAchievements
{
	private static bool _dumped;
	private static int _lastMenuId = -1;

	public static void Tick()
	{
		if (!Plugin.AchievementsMenuEnabled) return;

		MainMenuPage page = null;
		try { page = Object.FindObjectOfType<MainMenuPage>(); } catch { }
		if ((Object)(object)page == (Object)null) return;
		if (!((Component)page).gameObject.activeInHierarchy) return;

		int id = ((Object)page).GetInstanceID();
		if (id != _lastMenuId)
		{
			_lastMenuId = id;
			TryRevealButton(page);
		}

		if (Input.GetKeyDown(Plugin.AchievementsMenuKey))
		{
			try
			{
				Plugin.Log.LogInfo("[Achievements] Opening the achievements page via ShowAchievements()");
				page.ShowAchievements();
			}
			catch (Exception ex)
			{
				Plugin.Log.LogWarning("[Achievements] ShowAchievements failed: " + ex.Message);
			}
		}
	}

	/// <summary>
	/// If the button is present but switched off, turning it back on is far better than a
	/// hotkey - it puts the page where a player would look for it.
	/// </summary>
	private static void TryRevealButton(MainMenuPage page)
	{
		try
		{
			Transform root = ((Component)page).transform;
			Transform found = FindByName(root, "achiev");
			if ((Object)(object)found != (Object)null)
			{
				if (!found.gameObject.activeSelf)
				{
					found.gameObject.SetActive(true);
					Plugin.Log.LogInfo($"[Achievements] Re-enabled hidden menu object '{found.name}'");
				}
				else
				{
					// Active but not on screen. Report the things that hide an active object:
					// an inactive ancestor, zero alpha, zero scale, or an off-screen rect.
					Describe(found);
				}
			}
			else
			{
				Plugin.Dbg("[Achievements] No achievements button found in the main menu hierarchy");
			}

			if (Plugin.DebugVerbose && !_dumped)
			{
				_dumped = true;
				var sb = new StringBuilder();
				DumpChildren(root, sb, 0);
				Plugin.Dbg("[Achievements] Main menu objects:\n" + sb);
			}
		}
		catch (Exception ex)
		{
			Plugin.Dbg("[Achievements] reveal: " + ex.Message);
		}
	}

	/// <summary>
	/// Why an active object is still not on screen. Checked in the order that actually explains
	/// it: a switched-off ancestor beats everything, then transparency, then scale, then
	/// position.
	/// </summary>
	private static void Describe(Transform t)
	{
		try
		{
			GameObject go = t.gameObject;
			var sb = new StringBuilder();
			sb.Append("[Achievements] '").Append(t.name).Append("' active=").Append(go.activeSelf)
			  .Append(" inHierarchy=").Append(go.activeInHierarchy);

			// An ancestor being off is the usual reason an "active" object is invisible.
			Transform p = t.parent;
			while ((Object)(object)p != (Object)null)
			{
				if (!p.gameObject.activeSelf)
				{
					sb.Append(" | ancestor OFF: ").Append(p.name);
					break;
				}
				p = p.parent;
			}

			var cg = go.GetComponentInParent<CanvasGroup>();
			if ((Object)(object)cg != (Object)null)
				sb.Append(" | canvasGroup alpha=").Append(cg.alpha).Append(" on ").Append(((Object)cg).name);

			var rt = go.GetComponent<RectTransform>();
			if ((Object)(object)rt != (Object)null)
			{
				sb.Append(" | rect=").Append(rt.rect.ToString())
				  .Append(" anchored=").Append(rt.anchoredPosition.ToString())
				  .Append(" scale=").Append(rt.localScale.ToString());
			}

			Plugin.Log.LogInfo(sb.ToString());

			// Which graphics are not drawn, and why. A button whose background and label are
			// both off occupies space while showing nothing.
			foreach (var g in go.GetComponentsInChildren<UnityEngine.UI.Graphic>(true))
			{
				bool drawn = g.enabled && ((Component)g).gameObject.activeInHierarchy;
				string text = "";
				var tmp = ((Component)g).GetComponent<TMPro.TMP_Text>();
				if ((Object)(object)tmp != (Object)null) text = " text='" + (tmp.text ?? "") + "'";
				Plugin.Log.LogInfo($"[Achievements]   graphic '{((Object)g).name}' {g.GetType().Name} "
					+ $"drawn={drawn} enabled={g.enabled} active={((Component)g).gameObject.activeInHierarchy} "
					+ $"alpha={g.color.a:0.##}{text}");
			}

			// Sibling buttons, to see whether this one is laid out with the rest or parked
			// somewhere off on its own.
			Transform container = t.parent;
			if ((Object)(object)container != (Object)null)
			{
				for (int i = 0; i < container.childCount; i++)
				{
					Transform c = container.GetChild(i);
					var crt = ((Component)c).GetComponent<RectTransform>();
					Plugin.Log.LogInfo($"[Achievements]   sibling '{c.name}' active={c.gameObject.activeSelf} "
						+ $"anchored={((Object)(object)crt != (Object)null ? crt.anchoredPosition.ToString() : "?")}");
				}
			}
		}
		catch (Exception ex)
		{
			Plugin.Dbg("[Achievements] describe: " + ex.Message);
		}
	}

	private static Transform FindByName(Transform root, string contains)
	{
		try
		{
			foreach (var t in root.GetComponentsInChildren<Transform>(true))
			{
				if ((Object)(object)t == (Object)null) continue;
				string n = t.name ?? "";
				if (n.IndexOf(contains, StringComparison.OrdinalIgnoreCase) >= 0) return t;
			}
		}
		catch { }
		return null;
	}

	/// <summary>Names and active state of the menu's own objects, to locate the button by eye.</summary>
	private static void DumpChildren(Transform t, StringBuilder sb, int depth)
	{
		if (depth > 3) return;
		for (int i = 0; i < t.childCount; i++)
		{
			Transform c = t.GetChild(i);
			if ((Object)(object)c == (Object)null) continue;
			sb.Append(' ', depth * 2).Append("- ").Append(c.name);
			if (!c.gameObject.activeSelf) sb.Append("   [INACTIVE]");
			sb.Append('\n');
			DumpChildren(c, sb, depth + 1);
		}
	}
}
