using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace VSItemTooltips;

/// <summary>
/// Hover tooltips for rows of a scrolling list page (Secrets, Bestiary, Achievements,
/// Power-ups). Each page owns one registry.
///
/// Two behaviours here were paid for the hard way and must not be "simplified" away:
///
/// 1. Both the row root and the inner icon are registered. The selected row draws a highlight
///    over its contents, which swallows the pointer before it reaches the icon; a handler on
///    the root still fires, because pointer events bubble up to the first ancestor handling
///    them.
/// 2. Existing EventTrigger entries are appended to, never cleared. Clearing them on a row
///    that owns its own wiring is how a similar patch broke character select in 1.9.1.
/// </summary>
public sealed class RowTooltipRegistry
{
	/// <summary>What a registered row shows when hovered.</summary>
	public sealed class Entry
	{
		public string Title;
		public string Description;
		public Sprite Sprite;
		/// <summary>
		/// Asked for a sprite at hover time when <see cref="Sprite"/> is null. A row's own icon
		/// is not necessarily assigned yet when the row is registered, so scraping it has to
		/// wait until the row has actually been drawn. The answer is cached once it is real.
		/// </summary>
		public Func<Sprite> SpriteProvider;
		/// <summary>
		/// Asked for the description at hover time, in place of <see cref="Description"/>. Text
		/// scraped from a game panel does not exist until that panel has drawn the thing once, so
		/// a description resolved at registration is empty for anything not yet browsed.
		/// </summary>
		public Func<string> DescriptionProvider;
		public List<GameData.IconRow> Rows;
		/// <summary>
		/// Asked for the rows at hover time, in place of <see cref="Rows"/>. For anything whose
		/// numbers move while the page is open - Power Up prices climb with every purchase
		/// anywhere - rows worked out once at registration are wrong by the second purchase.
		/// </summary>
		public Func<List<GameData.IconRow>> RowsProvider;
		public string SectionHeader;
		/// <summary>
		/// What clicking this row should do, for rows that are meant to be clickable.
		///
		/// Our EventTrigger implements IPointerClickHandler whether we want it to or not, which
		/// ends EventSystem's walk up the hierarchy at this row - so a handler living on an
		/// ancestor never runs. Pages whose rows do something on click say so here; list pages
		/// whose rows are inert leave it null and nothing changes.
		/// </summary>
		public Action OnClick;
		/// <summary>
		/// Draw the panel even when there is nothing but a title and art.
		///
		/// Normally an empty panel is worse than none, so it is suppressed. Arcana cards are the
		/// exception: the game ships no description for the character and adventure groups, and a
		/// card that never responds to the pointer reads as broken rather than as empty. A name
		/// and its art are honest - they are what is actually known.
		/// </summary>
		public bool AllowTitleOnly;
		/// <summary>Placement override in Safe Area reference units; null uses the default dock.</summary>
		public Vector2? Offset;
		public Vector2? Pivot;
		/// <summary>
		/// Where a list too long for one panel continues. Null means it truncates instead, which
		/// is right for pages with no second free margin to continue into.
		/// </summary>
		public Vector2? SpillOffset;
		public Vector2? SpillPivot;
	}

	private readonly Dictionary<int, Entry> _entries = new Dictionary<int, Entry>();

	/// <summary>Rows are recycled, so each GameObject only needs its listeners once.</summary>
	private readonly HashSet<int> _wired = new HashSet<int>();

	private readonly string _logTag;

	public RowTooltipRegistry(string logTag)
	{
		_logTag = logTag;
	}

	/// <summary>
	/// Register a row. <paramref name="icon"/> may be null; the root is always registered so
	/// the selected row still responds.
	/// </summary>
	public void Register(GameObject root, GameObject icon, Entry entry)
	{
		if (entry == null) return;
		if ((Object)(object)icon != (Object)null) Add(icon, entry);
		if ((Object)(object)root != (Object)null) Add(root, entry);
	}

	private void Add(GameObject go, Entry entry)
	{
		int id = ((Object)go).GetInstanceID();
		_entries[id] = entry;
		AttachHover(go, id);
	}

	private void AttachHover(GameObject go, int id)
	{
		if (!_wired.Add(id)) return;
		try
		{
			// Nothing is added to the object's own graphics, and no raycast target is created.
			//
			// Two versions of this cost the arcana cards their clicks. Forcing raycastTarget on a
			// graphic the game had switched off, and adding a transparent Image to a row root that
			// had none, both turn the object into a raycast hit - and EventTrigger implements
			// EVERY pointer interface, IPointerClickHandler included. So the moment the raycast
			// lands on an object carrying our trigger, the click is consumed there. If the card's
			// own handler lives on a child or a sibling it never sees the click at all, because
			// pointer events travel up to ancestors and never sideways or down.
			//
			// Hover still reaches us without any of that: PointerEnter and PointerExit propagate
			// up the hierarchy, so a trigger on the root fires when any child is hovered - which
			// is the whole reason the root is registered alongside the icon. It works because the
			// row's existing art is the raycast target, not because we added one.
			EventTrigger et = go.GetComponent<EventTrigger>();
			if ((Object)(object)et == (Object)null) et = go.AddComponent<EventTrigger>();

			int captured = id;

			var enter = new EventTrigger.Entry();
			enter.eventID = EventTriggerType.PointerEnter;
			enter.callback.AddListener((UnityEngine.Events.UnityAction<BaseEventData>)(Action<BaseEventData>)(delegate
			{
				Show(captured);
			}));
			et.triggers.Add(enter);

			var exit = new EventTrigger.Entry();
			exit.eventID = EventTriggerType.PointerExit;
			exit.callback.AddListener((UnityEngine.Events.UnityAction<BaseEventData>)(Action<BaseEventData>)(delegate
			{
				_shownId = -1;
				ItemTooltipsMod.HideDockedPopup();
			}));
			et.triggers.Add(exit);

			// Hand the click back to whatever would have received it.
			//
			// EventTrigger implements every pointer interface, IPointerClickHandler included, so
			// EventSystem's walk up the hierarchy STOPS at the first object carrying one - ours.
			// Where a card's own handler sits on an ancestor rather than on the card, as the
			// arcana selection screens do, that handler simply never runs and the card is dead to
			// clicks. No amount of adjusting what the raycast hits changes this; the trigger only
			// has to be somewhere on the chain.
			//
			// Re-dispatching from the PARENT continues the walk from where it was cut off. Our own
			// object is skipped deliberately: if the game's handler is on the same object,
			// EventSystem already ran it alongside ours, and forwarding again would fire it twice.
			var click = new EventTrigger.Entry();
			click.eventID = EventTriggerType.PointerClick;
			click.callback.AddListener((UnityEngine.Events.UnityAction<BaseEventData>)(Action<BaseEventData>)(delegate
			{
				ForwardClick(captured);
			}));
			et.triggers.Add(click);
		}
		catch (Exception ex)
		{
			Plugin.Dbg($"[{_logTag}] hover: " + ex.Message);
		}
	}

	/// <summary>
	/// Run the row's own click action, which our EventTrigger would otherwise have swallowed.
	///
	/// Re-dispatching through <c>ExecuteEvents.ExecuteHierarchy</c> would be the general answer,
	/// but its generic EventFunction does not survive the IL2CPP interop boundary. Letting the
	/// page hand over a typed action is smaller and cannot mis-target: only pages whose rows are
	/// actually clickable set one.
	/// </summary>
	private void ForwardClick(int id)
	{
		if (!_entries.TryGetValue(id, out Entry e) || e == null || e.OnClick == null) return;
		try
		{
			e.OnClick();
		}
		catch (Exception ex)
		{
			Plugin.Dbg($"[{_logTag}] forward click: " + ex.Message);
		}
	}

	/// <summary>The row a tooltip is currently open for, so it can be rebuilt in place.</summary>
	private int _shownId = -1;

	/// <summary>
	/// Rebuild the open tooltip, if any. Buying on a page changes what the tooltip should say
	/// while the pointer has not moved, so nothing would otherwise re-trigger it.
	/// </summary>
	public void Refresh()
	{
		if (_shownId == -1) return;
		Show(_shownId);
	}

	private void Show(int id)
	{
		if (!_entries.TryGetValue(id, out Entry e) || e == null)
		{
			// A hover that lands on nothing is worth saying out loud: it is the difference between
			// "the panel refused to draw" and "the pointer never got here", which look identical
			// from the outside and have nothing in common as bugs.
			Plugin.Dbg($"[{_logTag}] hover on unregistered object {id}");
			return;
		}
		_shownId = id;

		List<GameData.IconRow> rows = e.Rows;
		if (e.RowsProvider != null)
		{
			try
			{
				var live = e.RowsProvider();
				if (live != null && live.Count > 0) rows = live;
			}
			catch (Exception ex)
			{
				Plugin.Dbg($"[{_logTag}] live rows: " + ex.Message);
			}
		}

		string description = e.Description;
		if (e.DescriptionProvider != null)
		{
			try
			{
				string live = e.DescriptionProvider();
				if (!string.IsNullOrWhiteSpace(live)) description = live;
			}
			catch (Exception ex)
			{
				Plugin.Dbg($"[{_logTag}] live description: " + ex.Message);
			}
		}

		if ((Object)(object)e.Sprite == (Object)null && e.SpriteProvider != null)
		{
			try
			{
				Sprite late = e.SpriteProvider();
				if ((Object)(object)late != (Object)null) e.Sprite = late;
			}
			catch (Exception ex)
			{
				Plugin.Dbg($"[{_logTag}] late sprite: " + ex.Message);
			}
		}
		if (Plugin.DebugVerbose)
			Plugin.Dbg($"[{_logTag}] hover title='{e.Title}' "
				+ $"desc={(string.IsNullOrEmpty(description) ? 0 : description.Length)}ch "
				+ $"rows={(rows == null ? 0 : rows.Count)} "
				+ $"sprite={((Object)(object)e.Sprite != (Object)null)} titleOnly={e.AllowTitleOnly}");

		ItemTooltipsMod.ShowDockedPopup(e.Title, description, e.Sprite, rows, e.SectionHeader, _logTag,
			e.Offset, e.Pivot, e.AllowTitleOnly, e.SpillOffset, e.SpillPivot);
	}

	public void Clear()
	{
		_entries.Clear();
		_wired.Clear();
		_shownId = -1;
		ItemTooltipsMod.HideDockedPopup();
	}
}
