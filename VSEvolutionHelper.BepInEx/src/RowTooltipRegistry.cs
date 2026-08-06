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
		public List<GameData.IconRow> Rows;
		public string SectionHeader;
		/// <summary>Placement override in Safe Area reference units; null uses the default dock.</summary>
		public Vector2? Offset;
		public Vector2? Pivot;
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
			Graphic g = go.GetComponent<Graphic>();
			if ((Object)(object)g == (Object)null)
			{
				Image img = go.AddComponent<Image>();
				img.color = new Color(1f, 1f, 1f, 0.01f);
				g = img;
			}
			g.raycastTarget = true;

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
				ItemTooltipsMod.HideDockedPopup();
			}));
			et.triggers.Add(exit);
		}
		catch (Exception ex)
		{
			Plugin.Dbg($"[{_logTag}] hover: " + ex.Message);
		}
	}

	private void Show(int id)
	{
		if (!_entries.TryGetValue(id, out Entry e) || e == null) return;
		ItemTooltipsMod.ShowDockedPopup(e.Title, e.Description, e.Sprite, e.Rows, e.SectionHeader, _logTag,
			e.Offset, e.Pivot);
	}

	public void Clear()
	{
		_entries.Clear();
		_wired.Clear();
		ItemTooltipsMod.HideDockedPopup();
	}
}
