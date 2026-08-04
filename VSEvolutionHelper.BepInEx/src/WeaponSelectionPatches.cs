using System;
using HarmonyLib;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VampireSurvivors.Data;
using VampireSurvivors.UI;
using Object = UnityEngine.Object;

namespace VSItemTooltips;

/// <summary>
/// Weapon selector screens (Arma Dio, Penshin Fatcha, and any other EME_SELECTOR weapon)
/// share one "View - WeaponSelection" and one cell type, WeaponSelectionItemUI.
///
/// The old approach was a one-shot scan of the view's Content transform that resolved the
/// cell type by walking assemblies for one whose name contains "Il2Cpp". That is a
/// MelonLoader assumption: under BepInEx the interop assemblies are unprefixed
/// (VampireSurvivors.Runtime), so the search never matched, the type stayed null, and the
/// scan returned before touching a cell. No selector ever got tooltips.
///
/// Two cells bind through *different* methods — SetData for the ordinary selector and
/// SetPenshinData for the Penshin Fatcha tuna list — so both are patched. Binding per cell
/// also removes the need for the scan's one-shot flag, which was only reset on unpause and
/// so never re-armed for a selector opened mid-run.
/// </summary>
public static class WeaponSelectionPatches
{
	public static void Apply(Harmony harmony)
	{
		if (!Plugin.WeaponSelectionTooltipsEnabled)
		{
			Plugin.Log.LogInfo("[WeaponSelect] Disabled by config");
			return;
		}

		TryPatch(harmony, "SetData",
			new Type[] { typeof(BaseWeaponSelectionPage), typeof(WeaponType), typeof(VampireSurvivors.Data.Weapons.WeaponData) },
			nameof(SetData_Postfix));

		TryPatch(harmony, "SetPenshinData",
			new Type[] { typeof(BaseWeaponSelectionPage), typeof(WeaponType), typeof(VampireSurvivors.Data.Weapons.WeaponData), typeof(bool) },
			nameof(SetData_Postfix));
	}

	private static void TryPatch(Harmony harmony, string name, Type[] args, string postfix)
	{
		try
		{
			var method = typeof(WeaponSelectionItemUI).GetMethod(
				name,
				System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public,
				null,
				args,
				null);
			if (method == null)
			{
				Plugin.Log.LogWarning($"[WeaponSelect] WeaponSelectionItemUI.{name} not found");
				return;
			}
			harmony.Patch(method, postfix: new HarmonyMethod(typeof(WeaponSelectionPatches), postfix));
			Plugin.Log.LogInfo($"[WeaponSelect] Patched WeaponSelectionItemUI.{name}");
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning($"[WeaponSelect] {name} patch: " + ex.Message);
		}
	}

	/// <summary>
	/// Instance-only postfix — the weapon is read back off the cell rather than taken from the
	/// patched call's arguments, which is how the other IL2CPP postfixes here avoid Harmony
	/// argument-marshaling crashes (see GrimoirePatches).
	/// </summary>
	public static void SetData_Postfix(WeaponSelectionItemUI __instance)
	{
		RegisterCell(__instance);
	}

	/// <summary>
	/// Give one selector cell a weapon tooltip. Shared by the bind postfixes and by the
	/// fallback view scan in ItemTooltipsMod.
	/// </summary>
	public static void RegisterCell(WeaponSelectionItemUI item)
	{
		try
		{
			if ((Object)(object)item == (Object)null) return;

			// Do this before the weapon check — the view root is worth having even for a cell
			// we end up skipping, since the popup layer depends on it.
			ItemTooltipsMod.AdoptWeaponSelectionView(((Component)item).transform);

			WeaponType wt = item._type;
			if (!GameData.IsRealWeaponType(wt))
			{
				// _type is populated by SetData itself; GetWeaponType() is the accessor the
				// game uses, so fall back to it before giving up on the cell.
				try { wt = item.GetWeaponType(); } catch { }
				if (!GameData.IsRealWeaponType(wt)) return;
			}

			GameObject target = HoverTarget(item);
			if ((Object)(object)target == (Object)null)
			{
				Plugin.Dbg($"WeaponSelect: no safe hover target for {wt}");
				return;
			}

			ItemTooltipsMod.RegisterWeaponUI(((Object)target).GetInstanceID(), target, wt);
			Plugin.Dbg($"WeaponSelect: registered {wt} on {((Object)target).name}");
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[WeaponSelect] RegisterCell: " + ex.Message);
		}
	}

	/// <summary>
	/// Prefer a child graphic (weapon frame, then icon) over the cell root. The root carries the
	/// game's own Button/selection wiring, and registering a hover replaces any EventTrigger on
	/// the target — clobbering the root is how a similar patch broke character select in 1.9.1.
	/// The root is only used when it has no EventTrigger of its own to destroy.
	/// </summary>
	private static GameObject HoverTarget(WeaponSelectionItemUI item)
	{
		GameObject root = ((Component)item).gameObject;

		GameObject frame = GraphicObject(item._WeaponFrame);
		if ((Object)(object)frame != (Object)null) return Raycastable(frame);

		GameObject icon = GraphicObject(item._Icon);
		if ((Object)(object)icon != (Object)null) return Raycastable(icon);

		if ((Object)(object)root.GetComponent<EventTrigger>() == (Object)null)
			return Raycastable(root);

		return null;
	}

	private static GameObject GraphicObject(Image image)
	{
		if ((Object)(object)image == (Object)null) return null;
		return ((Component)image).gameObject;
	}

	private static GameObject Raycastable(GameObject go)
	{
		try
		{
			var graphic = go.GetComponent<Graphic>();
			if ((Object)(object)graphic != (Object)null)
				graphic.raycastTarget = true;
		}
		catch { }
		return go;
	}
}
