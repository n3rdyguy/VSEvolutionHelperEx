using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using VampireSurvivors.Data;
using VampireSurvivors.UI;
using Object = UnityEngine.Object;

namespace VSItemTooltips;

/// <summary>
/// The weapons an arcana affects, listed along the bottom of the arcana info panel.
///
/// The card tooltip already names them, but a name is only half the question a player is asking
/// there: an arcana is worth taking for what those weapons *become*, and the evolution is exactly
/// what the screen does not say. Hovering an icon answers it without leaving the pick.
///
/// Only evolutions and unions are shown. Which arcana is in play is the one thing already on
/// screen, so listing arcanas back would spend the panel on what the player can see.
///
/// <para>
/// Three things about this panel had to be learned at runtime, and each shapes the code below.
/// </para>
///
/// <para>
/// <b>Nothing casts at the info panel.</b> Its icons live under <c>ArcanaInfoGroup</c>, a nested
/// <c>Canvas</c> with <c>overrideSorting</c> and no <c>GraphicRaycaster</c>. A graphic registers to
/// the nearest enabled Canvas above it and a raycaster only tests graphics on its own canvas, so
/// the view's raycaster never sees anything in that panel - and neither does the game. That is the
/// whole reason hovers never arrived, and it is invisible to every obvious check: the rect contains
/// the point, the target is on, nothing masks it, nothing blocks raycasts, the graphic is not
/// culled. See <see cref="EnsureRaycaster"/>. The view's <c>BlackFader</c> sat over the top of it
/// as a second, independent blocker; see <see cref="UnblockFader"/>.
/// </para>
///
/// <para>
/// <b>The panel keeps two full sets of icons</b> - a fixed row and a dynamic grid, filled with the
/// same weapons in the same order, one scaled up and one not. Rather than predict which, both are
/// wired; whichever is up when the pointer arrives already carries the tooltip.
/// </para>
///
/// <para>
/// <b>The panel's fields cannot be read from <c>LateUpdate</c>.</b> Reading <c>_affectedWeapons</c>
/// there returned a Count of -1595577652, and <c>_AffectedWeaponGroup.transform</c> threw "The
/// component is not attached to any game object!" as a native exception that took the process down
/// with it - a hard crash, not a caught error. The instance <c>LateUpdate</c> runs on is not the
/// live panel. Nothing here touches an <c>ArcanaInfoPanel</c> field outside the two Add postfixes,
/// where the instance is known good, and the wiring pass navigates by scene lookup instead.
/// </para>
/// </summary>
public static class ArcanaWeaponPatches
{
	private static readonly RowTooltipRegistry Rows = new RowTooltipRegistry("ArcanaWeapons");

	public static void Apply(Harmony harmony)
	{
		if (!Plugin.ArcanaCardTooltipsEnabled)
		{
			Plugin.Log.LogInfo("[ArcanaWeapons] Disabled by config");
			return;
		}
		try
		{
			// Patched at the point the panel is told WHICH weapon it is adding. The image is
			// generated one call deeper, by GenerateImageForAffectedWeapon(Sprite, bool), which
			// takes a sprite and no type - so from there the icon can no longer be identified.
			Bind(harmony, "AddAffectedWeapon", nameof(AddAffectedWeapon_Postfix));
			Bind(harmony, "AddAffectedItem", nameof(AddAffectedItem_Postfix));
			// Drives the deferred wiring pass. See TryWire.
			Bind(harmony, "LateUpdate", nameof(LateUpdate_Postfix));
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[ArcanaWeapons] patch: " + ex.Message);
		}
	}

	private static void Bind(Harmony harmony, string method, string postfix)
	{
		var m = typeof(ArcanaInfoPanel).GetMethod(method,
			System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
		if (m == null)
		{
			Plugin.Log.LogWarning($"[ArcanaWeapons] ArcanaInfoPanel.{method} not found");
			return;
		}
		harmony.Patch(m, postfix: new HarmonyMethod(typeof(ArcanaWeaponPatches), postfix));
		Plugin.Log.LogInfo($"[ArcanaWeapons] Patched ArcanaInfoPanel.{method}");
	}

	/// <summary>One affected entry, held until the icon that represents it can be found.</summary>
	private sealed class Pending
	{
		public string Id;
		public string Title;
		public string Description;
		public Sprite Sprite;
		public List<GameData.IconRow> Rows;
	}

	/// <summary>
	/// The current arcana's entries, in the order the panel was told about them.
	///
	/// Order is the only link back to an icon: the images carry no type of their own, and matching
	/// by sprite would fail wherever a weapon shares art with something else in the same list.
	/// </summary>
	private static readonly List<Pending> Queue = new List<Pending>();

	private static int _lastAddFrame = -999;
	private static int _wireAt = -1;
	private static int _giveUpAt = -1;

	public static void AddAffectedWeapon_Postfix(WeaponType weaponType)
	{
		try
		{
			Enqueue(new Pending
			{
				Id = weaponType.ToString(),
				Title = GameData.GetWeaponName(weaponType),
				Description = GameData.GetWeaponDescription(weaponType),
				Sprite = GameData.GetSprite(weaponType),
				Rows = GameData.GetWeaponEvoIconRows(weaponType),
			});
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[ArcanaWeapons] weapon postfix: " + ex.Message);
		}
	}

	public static void AddAffectedItem_Postfix(ItemType itemType)
	{
		try
		{
			// Passives are carried in both enums under the same names, so an item that also exists
			// as a weapon can answer the evolution question too - which is the whole point for a
			// passive, since its evolutions are what it contributes to.
			List<GameData.IconRow> rows = null;
			if (GameData.TryParseWeaponType(itemType.ToString(), out WeaponType asWeapon))
				rows = GameData.GetWeaponEvoIconRows(asWeapon);

			Enqueue(new Pending
			{
				Id = itemType.ToString(),
				Title = GameData.GetItemName(itemType),
				Sprite = GameData.GetItemSprite(itemType),
				Rows = rows,
			});
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[ArcanaWeapons] item postfix: " + ex.Message);
		}
	}

	/// <summary>
	/// Queue an entry, starting a new list when this is the first add of a new arcana.
	///
	/// A panel fills itself in one burst, so a gap of more than a frame since the last add means
	/// the player has moved to a different card and the previous list is stale.
	/// </summary>
	private static void Enqueue(Pending p)
	{
		if (Time.frameCount - _lastAddFrame > 1) Queue.Clear();
		_lastAddFrame = Time.frameCount;
		Queue.Add(p);

		// The icons are not laid out, scaled or enabled yet. Look for them once the panel has had
		// a few frames to appear, and keep looking for a couple of seconds if it is slow.
		_wireAt = Time.frameCount + 5;
		_giveUpAt = Time.frameCount + 180;
	}

	public static void LateUpdate_Postfix()
	{
		try
		{
			if (_wireAt < 0 || Time.frameCount < _wireAt) return;
			if (Queue.Count == 0) { _wireAt = -1; return; }

			if (TryWire())
			{
				_wireAt = -1;
			}
			else if (Time.frameCount > _giveUpAt)
			{
				_wireAt = -1;
				Plugin.Dbg($"[ArcanaWeapons] gave up finding {Queue.Count} drawn icons");
			}
			else
			{
				_wireAt = Time.frameCount + 5;
			}
		}
		catch (Exception ex)
		{
			Plugin.Dbg("[ArcanaWeapons] wire: " + ex.Message);
		}
	}

	/// <summary>
	/// Attach the queued entries to every container that holds a full set of icons.
	///
	/// Both of the panel's containers are filled with the same weapons in the same order, and only
	/// one is scaled up. Picking between them means predicting which, and getting it wrong wires
	/// icons the player never sees. Wiring both costs one EventTrigger on some hidden objects and
	/// removes the question - whichever container is up when the pointer arrives is already wired.
	///
	/// This is why the walk does not prune zero-scaled branches: the hidden container has to stay
	/// reachable for it to be wired at all.
	/// </summary>
	private static bool TryWire()
	{
		Transform root = FindPanelRoot();
		if ((Object)(object)root == (Object)null) return false;

		var all = new List<Group>();
		Collect(root, all, 0);

		// The panel itself is never an icon row. It draws one thing of its own - the arcana's own
		// art - so on a single-weapon arcana it would otherwise match and wire the card art.
		var matches = new List<Group>();
		foreach (Group g in all)
			if (g.Depth > 0 && g.Members.Count == Queue.Count) matches.Add(g);

		if (matches.Count == 0)
		{
			if (Plugin.DebugVerbose && Time.frameCount > _giveUpAt - 6)
			{
				// Only once the retries are nearly out, or it repeats every pass. Identical names
				// are collapsed: 49 clones each reporting one icon says nothing 49 times.
				var seen = new Dictionary<string, int>();
				foreach (Group g in all)
				{
					string k = $"{g.Name} x{g.Members.Count}";
					seen.TryGetValue(k, out int n);
					seen[k] = n + 1;
				}
				foreach (var kv in seen)
					Plugin.Dbg($"[ArcanaWeapons] candidate {kv.Key}"
						+ (kv.Value > 1 ? $" ({kv.Value} such)" : "") + $", need {Queue.Count}");
			}
			return false;
		}

		UnblockFader(root);
		EnsureRaycaster(matches[0].Members[0]);

		foreach (Group g in matches)
		{
			g.Members.Sort((a, b) => a.GetSiblingIndex().CompareTo(b.GetSiblingIndex()));
			for (int i = 0; i < g.Members.Count && i < Queue.Count; i++)
				Register(g.Members[i], Queue[i]);

			if (Plugin.DebugVerbose)
				Plugin.Dbg($"[ArcanaWeapons] wired {g.Members.Count} icons under '{g.Name}'");
		}
		return true;
	}

	/// <summary>
	/// Give the panel's own canvas something that casts at it.
	///
	/// This is what all the silence was, and it is not a fault in anything we wrote. The icons sit
	/// under <c>ArcanaInfoGroup</c>, which is a nested <c>Canvas</c> with <c>overrideSorting</c> and
	/// <b>no GraphicRaycaster</b>:
	///
	/// <code>
	/// graphic canvas='.../View - ArcanaMainSelection/ArcanaInfoGroup'
	/// raycasterCanvas='View - ArcanaMainSelection'  sameCanvas=False
	/// canvas on 'ArcanaInfoGroup': enabled=True override=True order=10 raycaster=False
	/// </code>
	///
	/// A graphic registers to the nearest enabled Canvas above it, and a <c>GraphicRaycaster</c>
	/// only tests graphics registered to its own canvas. So the info panel owns every graphic inside
	/// it and nothing anywhere casts at that canvas - not for us, and not for the game either. Every
	/// earlier reading was consistent with this and none of them could see it: the rect contains the
	/// point, the target is on, nothing masks or blocks, and the pointer still never arrives.
	///
	/// Adding the raycaster makes the panel hittable. It cannot steal clicks from the cards behind
	/// it: this canvas draws above them at order 10, so anything it now catches was already covered
	/// by the panel and unreachable by pointer.
	/// </summary>
	private static void EnsureRaycaster(Transform slot)
	{
		try
		{
			var g = ((Component)slot).GetComponentInChildren<UnityEngine.UI.Graphic>();
			if ((Object)(object)g == (Object)null) return;

			Canvas owner = g.canvas;
			if ((Object)(object)owner == (Object)null) return;

			GameObject go = ((Component)owner).gameObject;
			if ((Object)(object)go.GetComponent<UnityEngine.UI.GraphicRaycaster>() != (Object)null) return;

			go.AddComponent<UnityEngine.UI.GraphicRaycaster>();
			Plugin.Dbg($"[ArcanaWeapons] added GraphicRaycaster to '{((Object)go).name}' "
				+ $"(nested canvas, order {owner.sortingOrder}, nothing was casting at it)");
		}
		catch (Exception ex)
		{
			Plugin.Dbg("[ArcanaWeapons] raycaster: " + ex.Message);
		}
	}

	/// <summary>
	/// Let the pointer through the view's fade overlay.
	///
	/// This is what all the silence was. A raycast aimed at a wired icon came back with two hits and
	/// neither was the icon:
	///
	/// <code>
	/// hit[0] .../View - ArcanaMainSelection/BlackFader
	/// hit[1] .../Game Renderer/GameRenderOutput
	/// </code>
	///
	/// <c>BlackFader</c> covers the whole view and is a raycast target, so it takes every pointer
	/// event on the screen and nothing below it is reachable - not our icons, not anything the game
	/// itself puts there. The wiring was never the problem.
	///
	/// Guarding this on "only while transparent" was wrong: the fader reports <c>alpha 0.50</c> the
	/// whole time the screen is up, so it is not a fade at all - it is a permanent dimmer behind the
	/// UI, and the guard meant the fix never once applied.
	///
	/// Cleared unconditionally now. <c>raycastTarget</c> has no effect on what is drawn: the dimmer
	/// still dims exactly as before, it only stops claiming pointer events it has no handler for.
	/// </summary>
	private static void UnblockFader(Transform root)
	{
		try
		{
			// The fader is a sibling of the panel, up under the view that owns the raycaster.
			Transform view = root;
			for (int i = 0; i < 6 && (Object)(object)view != (Object)null; i++)
			{
				var rc = ((Component)view).GetComponent<UnityEngine.UI.GraphicRaycaster>();
				if ((Object)(object)rc != (Object)null) break;
				view = view.parent;
			}
			if ((Object)(object)view == (Object)null) return;

			Transform fader = view.Find("BlackFader");
			if ((Object)(object)fader == (Object)null) return;

			var img = ((Component)fader).GetComponent<UnityEngine.UI.Graphic>();
			if ((Object)(object)img == (Object)null) return;

			if (!img.raycastTarget) return;

			img.raycastTarget = false;
			Plugin.Dbg($"[ArcanaWeapons] cleared raycastTarget on '{((Object)view).name}/BlackFader' "
				+ $"(alpha {img.color.a:F2}, still drawn)");
		}
		catch (Exception ex)
		{
			Plugin.Dbg("[ArcanaWeapons] fader: " + ex.Message);
		}
	}

	/// <summary>
	/// The info panel, reached by scene lookup rather than through the patched instance.
	///
	/// <c>GameObject.Find</c> only returns active objects, and the fixed weapon row is active even
	/// when it is scaled to nothing, so it is a reliable landmark for the container above it.
	/// </summary>
	private static Transform FindPanelRoot()
	{
		try
		{
			GameObject group = GameObject.Find("AffectedWeaponsGroup");
			if ((Object)(object)group != (Object)null && (Object)(object)group.transform.parent != (Object)null)
				return group.transform.parent;

			GameObject container = GameObject.Find("InfoContainer");
			if ((Object)(object)container != (Object)null) return container.transform;
		}
		catch (Exception ex)
		{
			Plugin.Dbg("[ArcanaWeapons] find panel: " + ex.Message);
		}
		return null;
	}

	/// <summary>A container and the children of it that show something.</summary>
	private sealed class Group
	{
		public string Name;
		public int Depth;
		public List<Transform> Members;
	}

	/// <summary>
	/// Walk the panel gathering containers whose children draw something.
	///
	/// A child counts if there is an enabled <c>Image</c> with a sprite <b>anywhere below it</b>,
	/// not only on the child itself. The clone the panel makes per weapon carries a disabled
	/// graphic of its own and holds the visible art one level down, so a direct-component test
	/// grouped each clone as its own container of one icon and never saw the row of 49.
	///
	/// Only inactive branches are pruned. Zero scale is deliberately not a filter here: the hidden
	/// container is scaled to nothing and still has to be found, because it may be the one that is
	/// up by the time the pointer arrives.
	/// </summary>
	private static void Collect(Transform t, List<Group> groups, int depth)
	{
		if (depth > 6 || (Object)(object)t == (Object)null) return;
		if (!((Component)t).gameObject.activeInHierarchy) return;

		List<Transform> members = null;
		for (int i = 0; i < t.childCount; i++)
		{
			Transform c = t.GetChild(i);
			if (!((Component)c).gameObject.activeInHierarchy) continue;

			if (DrawsSomething(c, 0))
			{
				if (members == null) members = new List<Transform>();
				members.Add(c);
			}
			Collect(c, groups, depth + 1);
		}

		if (members != null)
			groups.Add(new Group { Name = ((Object)t).name, Depth = depth, Members = members });
	}

	/// <summary>An enabled sprite-bearing Image on this object or below it.</summary>
	private static bool DrawsSomething(Transform t, int depth)
	{
		if (depth > 4) return false;
		var img = ((Component)t).GetComponent<UnityEngine.UI.Image>();
		if ((Object)(object)img != (Object)null && ((Behaviour)img).enabled
			&& (Object)(object)img.sprite != (Object)null)
			return true;

		for (int i = 0; i < t.childCount; i++)
		{
			Transform c = t.GetChild(i);
			if (((Component)c).gameObject.activeInHierarchy && DrawsSomething(c, depth + 1)) return true;
		}
		return false;
	}

	/// <summary>The drawn Image on or below this object, which is what a raycast can land on.</summary>
	private static GameObject DrawnImage(Transform t, int depth)
	{
		if (depth > 4) return null;
		var img = ((Component)t).GetComponent<UnityEngine.UI.Image>();
		if ((Object)(object)img != (Object)null && ((Behaviour)img).enabled
			&& (Object)(object)img.sprite != (Object)null)
			return ((Component)t).gameObject;

		for (int i = 0; i < t.childCount; i++)
		{
			Transform c = t.GetChild(i);
			if (!((Component)c).gameObject.activeInHierarchy) continue;
			GameObject found = DrawnImage(c, depth + 1);
			if ((Object)(object)found != (Object)null) return found;
		}
		return null;
	}

	/// <summary>
	/// Wire one icon.
	///
	/// The slot and the art below it are both registered, for the same reason list rows are: the
	/// slot's own graphic is disabled, so only the art can be hit, but pointer events travel up -
	/// so a trigger on the slot fires either way and survives the art being replaced.
	/// </summary>
	private static void Register(Transform slot, Pending p)
	{
		string title = string.IsNullOrWhiteSpace(p.Title) || GameData.LooksLikeLocKey(p.Title)
			? GameData.HumanizeEnum(p.Id)
			: p.Title.Trim();

		GameObject icon = DrawnImage(slot, 0);

		Rows.Register(((Component)slot).gameObject, icon, new RowTooltipRegistry.Entry
		{
			Title = title,
			Description = p.Description,
			Sprite = p.Sprite,
			Rows = p.Rows,
			SectionHeader = (p.Rows != null && p.Rows.Count > 0) ? "Evolves into:" : null,
			// A weapon with no evolution still answers the pointer. Silence on half the icons
			// reads as a broken tooltip rather than as "this one does not evolve".
			AllowTitleOnly = true,
			Offset = new Vector2(ItemTooltipsMod.ArcanaPanelX, ItemTooltipsMod.ArcanaPanelY),
			Pivot = ItemTooltipsMod.ArcanaPanelPivot,
		});

		MakeHoverable(icon, p.Id);
	}

	/// <summary>
	/// Let the pointer land on an affected-weapon icon.
	///
	/// These images are generated purely to be looked at, so they need not be raycast targets.
	/// Turning that on is the one thing the row registry refuses to do, because doing it to an
	/// arcana card made every card unclickable - our EventTrigger implements IPointerClickHandler,
	/// so a raycast landing on it ends the walk before the card's own handler on an ancestor ever
	/// runs. That reasoning does not reach here: these icons are inside the info panel, are not
	/// selectable, and have no click behaviour of their own or above them to intercept. The
	/// distinction is the presence of a handler up the chain, not the act of enabling the target.
	/// </summary>
	private static void MakeHoverable(GameObject icon, string id)
	{
		try
		{
			if ((Object)(object)icon == (Object)null)
			{
				Plugin.Dbg($"[ArcanaWeapons] {id}: no drawn image under the slot");
				return;
			}
			var g = icon.GetComponent<UnityEngine.UI.Graphic>();
			if ((Object)(object)g == (Object)null)
			{
				Plugin.Dbg($"[ArcanaWeapons] {id}: icon has no Graphic, cannot be hovered");
				return;
			}
			if (!g.raycastTarget) g.raycastTarget = true;
		}
		catch (Exception ex)
		{
			Plugin.Dbg("[ArcanaWeapons] hoverable: " + ex.Message);
		}
	}

	public static void Clear()
	{
		Queue.Clear();
		_wireAt = -1;
		Rows.Clear();
	}
}
