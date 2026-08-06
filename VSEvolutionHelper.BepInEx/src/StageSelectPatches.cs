using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using VampireSurvivors.Data;
using VampireSurvivors.Data.Stage;
using VampireSurvivors.UI;
using Object = UnityEngine.Object;
using Il2CppListGO = Il2CppSystem.Collections.Generic.List<UnityEngine.GameObject>;
using Il2CppListItem = Il2CppSystem.Collections.Generic.List<VampireSurvivors.Data.ItemType>;

namespace VSItemTooltips;

/// <summary>
/// Stage Selection:
/// A) tooltips on "Relics in stage" icons (RelicPanel)
/// B) Music | Guide tab switch sharing the SongPanel real estate
/// </summary>
public static class StageSelectPatches
{
	public static void Apply(Harmony harmony)
	{
		try
		{
			var setRelics = typeof(RelicPanel).GetMethod(
				"SetRelics",
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
				null,
				new Type[] { typeof(StageData), typeof(StageType) },
				null);
			if (setRelics != null)
			{
				harmony.Patch(setRelics, postfix: new HarmonyMethod(typeof(StageSelectPatches), nameof(SetRelics_Postfix)));
				Plugin.Log.LogInfo("[StageSelect] Patched RelicPanel.SetRelics");
			}
			else
				Plugin.Log.LogWarning("[StageSelect] RelicPanel.SetRelics not found");
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[StageSelect] SetRelics patch: " + ex.Message);
		}

		try
		{
			var setInfo = typeof(StageSelectPage).GetMethod(
				"SetInfoPanel",
				BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
				null,
				new Type[] { typeof(StageItemUI), typeof(StageData), typeof(StageType) },
				null);
			if (setInfo != null)
			{
				harmony.Patch(setInfo, postfix: new HarmonyMethod(typeof(StageSelectPatches), nameof(SetInfoPanel_Postfix)));
				Plugin.Log.LogInfo("[StageSelect] Patched StageSelectPage.SetInfoPanel");
			}
			else
				Plugin.Log.LogWarning("[StageSelect] SetInfoPanel not found");
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[StageSelect] SetInfoPanel patch: " + ex.Message);
		}

		// Clear when leaving stage select
		try
		{
			foreach (string name in new[] { "OnHideStart", "OnHide", "Hide", "Close" })
			{
				var m = typeof(StageSelectPage).GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (m != null && m.GetParameters().Length == 0)
				{
					harmony.Patch(m, postfix: new HarmonyMethod(typeof(StageSelectPatches), nameof(StageSelect_Hide_Postfix)));
					Plugin.Log.LogInfo($"[StageSelect] Patched StageSelectPage.{name} for cleanup");
					break;
				}
			}
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[StageSelect] hide patch: " + ex.Message);
		}
	}

	public static void SetInfoPanel_Postfix(StageSelectPage __instance, StageItemUI stageItemUI, StageData stage, StageType stageType)
	{
		try
		{
			StageGuideUI.OnStageSelected(__instance, stageItemUI, stage, stageType);
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[StageSelect] SetInfoPanel postfix: " + ex.Message);
		}
	}

	public static void SetRelics_Postfix(RelicPanel __instance, StageData stage, StageType stageType)
	{
		try
		{
			ItemTooltipsMod.ClearStageRelicIcons();
			if ((Object)(object)__instance == (Object)null)
				return;

			GameData.EnsureLoaded();

			Il2CppListGO spawned = null;
			Il2CppListItem types = null;
			try { spawned = __instance._spawned; } catch { }
			try { types = __instance._spawnedType; } catch { }

			int nGo = spawned != null ? spawned.Count : 0;
			int nTy = types != null ? types.Count : 0;
			int n = Math.Min(nGo, nTy);

			// Prefer parallel spawned lists from the game
			if (n > 0)
			{
				for (int i = 0; i < n; i++)
				{
					GameObject go = spawned[i];
					if ((Object)(object)go == (Object)null) continue;
					ItemType it = types[i];
					RegisterOne(go, it);
				}
				Plugin.Dbg($"StageSelect relics: registered {n} from _spawned (stage={stageType})");
				return;
			}

			// Fallback: spawn-order from StageData relic lists (no GO match) - skip
			// If GOs exist without types, try resolve from stage relic lists by index
			if (nGo > 0 && stage != null)
			{
				var flat = FlattenRelics(stage);
				int m = Math.Min(nGo, flat.Count);
				for (int i = 0; i < m; i++)
				{
					GameObject go = spawned[i];
					if ((Object)(object)go == (Object)null) continue;
					RegisterOne(go, flat[i]);
				}
				Plugin.Dbg($"StageSelect relics: fallback registered {m} from StageData lists (stage={stageType})");
			}
			else
			{
				Plugin.Dbg($"StageSelect relics: nothing to register (go={nGo} types={nTy} stage={stageType})");
			}
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[StageSelect] SetRelics postfix: " + ex.Message);
		}
	}

	public static void StageSelect_Hide_Postfix()
	{
		ItemTooltipsMod.ClearStageRelicIcons();
		StageGuideUI.Hide();
	}

	private static void RegisterOne(GameObject go, ItemType it)
	{
		string label = GameData.GetItemName(it);
		string desc = GameData.GetItemDescription(it);
		Sprite spr = GameData.GetItemSprite(it);
		// Prefer Image child for hit testing
		GameObject hit = go;
		try
		{
			var img = go.GetComponentInChildren<UnityEngine.UI.Image>(true);
			if ((Object)(object)img != (Object)null)
				hit = ((Component)img).gameObject;
		}
		catch { }
		ItemTooltipsMod.RegisterStageRelicIcon(hit, it, label, desc, spr);
	}

	private static System.Collections.Generic.List<ItemType> FlattenRelics(StageData stage)
	{
		var list = new System.Collections.Generic.List<ItemType>();
		void addList(Il2CppListItem src)
		{
			if (src == null) return;
			for (int i = 0; i < src.Count; i++)
			{
				ItemType t = src[i];
				if (!list.Contains(t)) list.Add(t);
			}
		}
		try { addList(stage.relics); } catch { }
		try { addList(stage.relics2); } catch { }
		try { addList(stage.yellowRelics); } catch { }
		return list;
	}
}
