using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VampireSurvivors.Data;
using VampireSurvivors.Data.Stage;
using VampireSurvivors.UI;
using Object = UnityEngine.Object;

namespace VSItemTooltips;

/// <summary>
/// Stage Selection right column: tab switch Music | Guide above the song panel.
/// Guide reuses the SongPanel's screen real estate (does not steal empty space).
/// </summary>
public static class StageGuideUI
{
	private enum Tab { Music, Guide }

	private static Tab _tab = Tab.Music;
	private static bool _tabInitialized;
	private static GameObject _tabBar;
	private static GameObject _guideRoot;
	private static GameObject _guideViewport;
	private static GameObject _guideScrollContent;
	private static ScrollRect _guideScroll;
	private static GameObject _songPanelGo;
	private static RectTransform _songPanelRt;
	private static TextMeshProUGUI _tabMusicLabel;
	private static TextMeshProUGUI _tabGuideLabel;
	private static Image _tabMusicBg;
	private static Image _tabGuideBg;
	private static StageData _stage;
	private static StageType _stageType;
	private static StageItemUI _stageItem;
	private static readonly Color TabOn = new Color(0.75f, 0.55f, 0.2f, 0.95f);
	private static readonly Color TabOff = new Color(0.25f, 0.25f, 0.28f, 0.9f);
	private static readonly Color PanelBg = new Color(0.22f, 0.22f, 0.26f, 0.96f);
	private static readonly Color Gold = new Color(0.95f, 0.8f, 0.35f, 1f);
	private static readonly Color Soft = new Color(0.85f, 0.85f, 0.9f, 1f);
	private static readonly Color Muted = new Color(0.65f, 0.7f, 0.85f, 1f);

	public static void OnStageSelected(StageSelectPage page, StageItemUI item, StageData stage, StageType type)
	{
		try
		{
			if (!Plugin.StageGuideEnabled)
			{
				Hide();
				return;
			}
			if ((Object)(object)page == (Object)null || stage == null)
				return;
			_stage = stage;
			_stageType = type;
			_stageItem = item;
			if (!EnsureChrome(page))
				return;
			if (!_tabInitialized)
			{
				_tab = Plugin.StageGuideDefaultToGuide ? Tab.Guide : Tab.Music;
				_tabInitialized = true;
			}
			RebuildGuideContent();
			ApplyTabVisibility();
			Plugin.Dbg($"StageGuide: stage={type} tab={_tab}");
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[StageGuide] OnStageSelected: " + ex.Message);
		}
	}

	public static void Hide()
	{
		try
		{
			// Restore music panel if we hid it
			if ((Object)(object)_songPanelGo != (Object)null)
				_songPanelGo.SetActive(true);
			if ((Object)(object)_tabBar != (Object)null)
				Object.Destroy((Object)(object)_tabBar);
			if ((Object)(object)_guideRoot != (Object)null)
				Object.Destroy((Object)(object)_guideRoot);
		}
		catch { }
		_tabBar = null;
		_guideRoot = null;
		_guideViewport = null;
		_guideScrollContent = null;
		_guideScroll = null;
		_songPanelGo = null;
		_songPanelRt = null;
		_tabMusicLabel = null;
		_tabGuideLabel = null;
		_tabMusicBg = null;
		_tabGuideBg = null;
		_stage = null;
		_stageItem = null;
		_tab = Tab.Music;
		_tabInitialized = false;
	}

	private static bool EnsureChrome(StageSelectPage page)
	{
		SongSelectionPanel song = null;
		try { song = page._SongPanel; } catch { }
		if ((Object)(object)song == (Object)null)
		{
			Plugin.Log.LogWarning("[StageGuide] SongPanel missing");
			return false;
		}

		_songPanelGo = ((Component)song).gameObject;
		_songPanelRt = _songPanelGo.GetComponent<RectTransform>();
		if ((Object)(object)_songPanelRt == (Object)null)
			return false;

		Transform parent = _songPanelRt.parent;
		if ((Object)(object)parent == (Object)null)
			return false;

		TMP_FontAsset font = ItemTooltipsMod.GetUiFont();
		if ((Object)(object)font == (Object)null)
		{
			Plugin.Log.LogWarning("[StageGuide] No TMP font");
			return false;
		}

		// Tab bar — sit just above the song panel, same width
		if ((Object)(object)_tabBar == (Object)null)
		{
			_tabBar = new GameObject("EvoHelper_StageTabs");
			_tabBar.transform.SetParent(parent, false);
			RectTransform tabRt = _tabBar.AddComponent<RectTransform>();
			CopyHorizontalPlacement(tabRt, _songPanelRt);
			// Stick to top of song panel, hang upward
			tabRt.pivot = new Vector2(0.5f, 0f);
			tabRt.anchorMin = _songPanelRt.anchorMin;
			tabRt.anchorMax = _songPanelRt.anchorMax;
			// Position at top edge of song panel
			Vector2 songPos = _songPanelRt.anchoredPosition;
			Vector2 songSize = _songPanelRt.sizeDelta;
			// Prefer matching width; height for tabs
			float tabH = 34f;
			tabRt.sizeDelta = new Vector2(songSize.x, tabH);
			// Place so bottom of tab bar touches top of song panel
			float topY = songPos.y + songSize.y * (1f - _songPanelRt.pivot.y);
			// When anchors are stretch, sizeDelta.y is different — also try offsetMax
			if (Mathf.Approximately(_songPanelRt.anchorMin.y, _songPanelRt.anchorMax.y))
			{
				tabRt.anchoredPosition = new Vector2(songPos.x, topY);
			}
			else
			{
				// Stretch anchors: put tabs as overlay at top of same rect, offset upward
				tabRt.anchorMin = new Vector2(_songPanelRt.anchorMin.x, _songPanelRt.anchorMax.y);
				tabRt.anchorMax = new Vector2(_songPanelRt.anchorMax.x, _songPanelRt.anchorMax.y);
				tabRt.pivot = new Vector2(0.5f, 0f);
				tabRt.anchoredPosition = new Vector2(0f, 2f);
				tabRt.sizeDelta = new Vector2(_songPanelRt.sizeDelta.x, tabH);
				tabRt.offsetMin = new Vector2(_songPanelRt.offsetMin.x, 0f);
				tabRt.offsetMax = new Vector2(_songPanelRt.offsetMax.x, tabH);
			}

			// Two equal tabs
			_tabMusicBg = MakeTabButton(_tabBar.transform, "TabMusic", "Music", font, 0f, out _tabMusicLabel);
			_tabGuideBg = MakeTabButton(_tabBar.transform, "TabGuide", "Guide", font, 0.5f, out _tabGuideLabel);
			AddClick(_tabMusicBg.gameObject, () => SetTab(Tab.Music));
			AddClick(_tabGuideBg.gameObject, () => SetTab(Tab.Guide));
		}

		// Guide root — same top/width as song panel; scroll when content is tall
		if ((Object)(object)_guideRoot == (Object)null)
		{
			_guideRoot = new GameObject("EvoHelper_StageGuide");
			_guideRoot.transform.SetParent(parent, false);
			RectTransform gr = _guideRoot.AddComponent<RectTransform>();
			CopyRect(gr, _songPanelRt);
			Image bg = _guideRoot.AddComponent<Image>();
			bg.color = PanelBg;
			((Graphic)bg).raycastTarget = true;
			Outline ol = _guideRoot.AddComponent<Outline>();
			((Shadow)ol).effectColor = new Color(0.55f, 0.45f, 0.2f, 1f);
			((Shadow)ol).effectDistance = new Vector2(1.5f, 1.5f);

			// Viewport (masks overflowing content)
			_guideViewport = new GameObject("Viewport");
			_guideViewport.transform.SetParent(_guideRoot.transform, false);
			RectTransform vrt = _guideViewport.AddComponent<RectTransform>();
			vrt.anchorMin = Vector2.zero;
			vrt.anchorMax = Vector2.one;
			vrt.offsetMin = new Vector2(8f, 8f);
			vrt.offsetMax = new Vector2(-8f, -8f);
			Image vImg = _guideViewport.AddComponent<Image>();
			vImg.color = new Color(1f, 1f, 1f, 0.02f);
			((Graphic)vImg).raycastTarget = true;
			_guideViewport.AddComponent<RectMask2D>();

			// Content pinned to top of viewport
			_guideScrollContent = new GameObject("Content");
			_guideScrollContent.transform.SetParent(_guideViewport.transform, false);
			RectTransform cr = _guideScrollContent.AddComponent<RectTransform>();
			cr.anchorMin = new Vector2(0f, 1f);
			cr.anchorMax = new Vector2(1f, 1f);
			cr.pivot = new Vector2(0.5f, 1f);
			cr.anchoredPosition = Vector2.zero;
			cr.sizeDelta = new Vector2(0f, 400f);

			_guideScroll = _guideRoot.AddComponent<ScrollRect>();
			_guideScroll.content = cr;
			_guideScroll.viewport = vrt;
			_guideScroll.horizontal = false;
			_guideScroll.vertical = true;
			_guideScroll.movementType = ScrollRect.MovementType.Clamped;
			_guideScroll.scrollSensitivity = 40f;
			_guideScroll.inertia = true;

			_guideRoot.SetActive(false);
		}

		return true;
	}

	private static void SetTab(Tab tab)
	{
		_tab = tab;
		ApplyTabVisibility();
		if (_tab == Tab.Guide)
			RebuildGuideContent();
	}

	private static void ApplyTabVisibility()
	{
		bool guide = _tab == Tab.Guide;
		if ((Object)(object)_songPanelGo != (Object)null)
			_songPanelGo.SetActive(!guide);
		if ((Object)(object)_guideRoot != (Object)null)
			_guideRoot.SetActive(guide);

		StyleTab(_tabMusicBg, _tabMusicLabel, !guide);
		StyleTab(_tabGuideBg, _tabGuideLabel, guide);
	}

	private static void StyleTab(Image bg, TextMeshProUGUI label, bool on)
	{
		if ((Object)(object)bg != (Object)null)
			bg.color = on ? TabOn : TabOff;
		if ((Object)(object)label != (Object)null)
			((Graphic)label).color = on ? Color.white : Soft;
	}

	private static void RebuildGuideContent()
	{
		if ((Object)(object)_guideScrollContent == (Object)null || _stage == null)
			return;

		// Clear children
		for (int i = _guideScrollContent.transform.childCount - 1; i >= 0; i--)
		{
			Object.Destroy((Object)(object)_guideScrollContent.transform.GetChild(i).gameObject);
		}

		TMP_FontAsset font = ItemTooltipsMod.GetUiFont();
		if ((Object)(object)font == (Object)null) return;

		GameData.EnsureLoaded();
		float y = 0f;
		// Prefer song panel width so wrap matches the visible column
		float width = ResolveContentWidth();

		// Title only — flavor description already shows beside the stage list
		string name = SafeLoc(() => _stage.GetLocalizedName(_stageType), _stage.stageName);
		y = AddHeader(_guideScrollContent.transform, font, name, Gold, 18f, width, y);

		// Progression
		string hyper = "?";
		try
		{
			if ((Object)(object)_stageItem != (Object)null)
				hyper = _stageItem.HasHyperUnlocked() ? "Yes" : "No";
		}
		catch { }
		y = AddHeader(_guideScrollContent.transform, font, "Progression", Gold, 14f, width, y + 4f);
		y = AddBody(_guideScrollContent.transform, font, $"Hyper unlocked: {hyper}", Muted, 12f, width, y, 2f);

		// Relics / unlocks — omit entire section when empty (quieter for stages like LABORRATORY)
		var relics = CollectRelics(_stage);
		if (relics.Count > 0)
		{
			y = AddHeader(_guideScrollContent.transform, font, $"Unlocks / Relics ({relics.Count})", Gold, 14f, width, y + 6f);
			foreach (ItemType it in relics)
				y = AddRelicRow(_guideScrollContent.transform, font, it, width, y);
		}

		// Game tips — only when present
		string tips = SafeLoc(() => _stage.GetLocalizedTips(_stageType), _stage.tips);
		if (!string.IsNullOrWhiteSpace(tips))
		{
			y = AddHeader(_guideScrollContent.transform, font, "Tips", Gold, 14f, width, y + 8f);
			y = AddBody(_guideScrollContent.transform, font, tips.Trim(), Soft, 12f, width, y, 2f);
		}

		// Hyper tips
		string htips = SafeLoc(() => _stage.GetLocalizedHyperTips(_stageType), _stage.hyperTips);
		if (!string.IsNullOrWhiteSpace(htips))
		{
			y = AddHeader(_guideScrollContent.transform, font, "Hyper tips", Gold, 14f, width, y + 6f);
			y = AddBody(_guideScrollContent.transform, font, htips.Trim(), Soft, 12f, width, y, 2f);
		}

		// Extra curated tips
		if (StageExtraTips.TryGet(_stageType, out string extra))
		{
			y = AddHeader(_guideScrollContent.transform, font, "Extra notes", new Color(0.7f, 0.85f, 1f, 1f), 14f, width, y + 8f);
			y = AddBody(_guideScrollContent.transform, font, extra, Soft, 12f, width, y, 2f);
		}

		// Content height + panel size; scroll if content still taller than viewport
		float contentH = Mathf.Abs(y) + 16f;
		var contentRt = _guideScrollContent.GetComponent<RectTransform>();
		// Width 0 with stretch anchors = full viewport width; height = content
		contentRt.sizeDelta = new Vector2(0f, contentH);
		contentRt.anchoredPosition = Vector2.zero;
		FitGuidePanelHeight(contentH + 24f);
		if ((Object)(object)_guideScroll != (Object)null)
		{
			_guideScroll.verticalNormalizedPosition = 1f; // top
			_guideScroll.velocity = Vector2.zero;
		}
	}

	/// <summary>
	/// Match song panel top/width; grow downward up to a max. Taller content scrolls inside.
	/// </summary>
	private static void FitGuidePanelHeight(float neededHeight)
	{
		if ((Object)(object)_guideRoot == (Object)null || (Object)(object)_songPanelRt == (Object)null)
			return;

		RectTransform gr = _guideRoot.GetComponent<RectTransform>();
		CopyRect(gr, _songPanelRt);

		float songH = Mathf.Abs(gr.rect.height);
		if (songH < 8f)
			songH = Mathf.Max(8f, Mathf.Abs(gr.sizeDelta.y));

		// Prefer roomier panel, but cap so small screens can still scroll
		float targetH = Mathf.Max(songH, neededHeight);
		const float maxPanelH = 680f;
		targetH = Mathf.Min(targetH, maxPanelH);

		bool fixedAnchors = Mathf.Approximately(gr.anchorMin.y, gr.anchorMax.y);
		if (fixedAnchors)
		{
			float oldH = gr.sizeDelta.y;
			if (oldH < 1f) oldH = songH;
			float newH = targetH;
			float pivotY = gr.pivot.y;
			Vector2 pos = gr.anchoredPosition;
			Vector2 sd = gr.sizeDelta;
			sd.y = newH;
			gr.sizeDelta = sd;
			pos.y += (oldH - newH) * (1f - pivotY);
			gr.anchoredPosition = pos;
		}
		else
		{
			float extra = Mathf.Max(0f, targetH - songH);
			gr.offsetMin = new Vector2(gr.offsetMin.x, gr.offsetMin.y - extra);
		}
	}

	private static List<ItemType> CollectRelics(StageData stage)
	{
		var list = new List<ItemType>();
		void add(Il2CppSystem.Collections.Generic.List<ItemType> src)
		{
			if (src == null) return;
			for (int i = 0; i < src.Count; i++)
			{
				ItemType t = src[i];
				if (!list.Contains(t)) list.Add(t);
			}
		}
		try { add(stage.relics); } catch { }
		try { add(stage.relics2); } catch { }
		try { add(stage.yellowRelics); } catch { }
		return list;
	}

	private static float ResolveContentWidth()
	{
		float width = 260f;
		try
		{
			if ((Object)(object)_songPanelRt != (Object)null)
			{
				float w = _songPanelRt.rect.width;
				if (w < 40f) w = Mathf.Abs(_songPanelRt.sizeDelta.x);
				if (w > 40f) width = w - 28f;
			}
			if ((Object)(object)_guideRoot != (Object)null)
			{
				float w2 = _guideRoot.GetComponent<RectTransform>().rect.width;
				if (w2 > 40f) width = Mathf.Max(width, w2 - 28f);
			}
		}
		catch { }
		return Mathf.Clamp(width, 140f, 480f);
	}

	private static float AddHeader(Transform parent, TMP_FontAsset font, string text, Color color, float size, float width, float y)
	{
		var go = MakeTmp(parent, "H", text, font, size, color, true);
		var rt = go.GetComponent<RectTransform>();
		var tmp = go.GetComponent<TextMeshProUGUI>();
		ConfigureWrap(tmp, width);
		rt.anchoredPosition = new Vector2(0f, -y);
		float h = MeasureWrappedHeight(tmp, rt, width, size + 6f, 80f);
		rt.sizeDelta = new Vector2(width, h);
		return y + h + 6f;
	}

	private static float AddBody(Transform parent, TMP_FontAsset font, string text, Color color, float size, float width, float y, float pad)
	{
		var go = MakeTmp(parent, "B", text, font, size, color, false);
		var rt = go.GetComponent<RectTransform>();
		var tmp = go.GetComponent<TextMeshProUGUI>();
		ConfigureWrap(tmp, width);
		rt.anchoredPosition = new Vector2(0f, -y);
		float h = MeasureWrappedHeight(tmp, rt, width, size + 4f, 480f);
		rt.sizeDelta = new Vector2(width, h);
		return y + h + pad;
	}

	/// <summary>TMP only wraps after the rect has a real width.</summary>
	private static void ConfigureWrap(TextMeshProUGUI tmp, float width)
	{
		if ((Object)(object)tmp == (Object)null) return;
		((TMP_Text)tmp).enableWordWrapping = true;
		((TMP_Text)tmp).overflowMode = TextOverflowModes.Overflow;
		((TMP_Text)tmp).richText = true;
		// Horizontal alignment left, top
		((TMP_Text)tmp).alignment = (TextAlignmentOptions)257;
		try
		{
			// Unity 6 TMP: ensure wrapping mode if property exists
			var prop = typeof(TMP_Text).GetProperty("textWrappingMode");
			if (prop != null && prop.CanWrite)
			{
				// TextWrappingModes.Normal = 1 in many TMP versions
				var enumType = prop.PropertyType;
				object normal = Enum.ToObject(enumType, 1);
				prop.SetValue(tmp, normal);
			}
		}
		catch { }
		_ = width;
	}

	private static float MeasureWrappedHeight(TextMeshProUGUI tmp, RectTransform rt, float width, float minH, float maxH)
	{
		// Assign a wide-enough box first so preferredHeight accounts for wrapping
		rt.sizeDelta = new Vector2(width, maxH);
		((TMP_Text)tmp).rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
		((TMP_Text)tmp).ForceMeshUpdate(true, true);
		float h = ((TMP_Text)tmp).preferredHeight + 6f;
		if (h < minH) h = minH;
		if (h > maxH) h = maxH;
		// Second pass with final height so mesh matches
		rt.sizeDelta = new Vector2(width, h);
		((TMP_Text)tmp).ForceMeshUpdate(true, true);
		return h;
	}

	private static float AddRelicRow(Transform parent, TMP_FontAsset font, ItemType it, float width, float y)
	{
		const float iconSize = 30f;
		const float iconGap = 6f;
		float textWidth = Mathf.Max(80f, width - iconSize - iconGap - 4f);
		string itemName = GameData.GetItemName(it);
		string itemDesc = GameData.GetItemDescription(it);
		Sprite spr = GameData.GetItemSprite(it);

		// Measure name with wrap first so row height fits multi-line titles
		GameObject measureGo = MakeTmp(parent, "RelicMeasure", itemName, font, 12f, Soft, false);
		var measureTmp = measureGo.GetComponent<TextMeshProUGUI>();
		var measureRt = measureGo.GetComponent<RectTransform>();
		ConfigureWrap(measureTmp, textWidth);
		float textH = MeasureWrappedHeight(measureTmp, measureRt, textWidth, 18f, 120f);
		Object.Destroy((Object)(object)measureGo);

		float rowH = Mathf.Max(iconSize + 4f, textH + 4f);

		GameObject row = new GameObject("Relic_" + it);
		row.transform.SetParent(parent, false);
		RectTransform rt = row.AddComponent<RectTransform>();
		rt.anchorMin = new Vector2(0f, 1f);
		rt.anchorMax = new Vector2(0f, 1f);
		rt.pivot = new Vector2(0f, 1f);
		rt.anchoredPosition = new Vector2(0f, -y);
		rt.sizeDelta = new Vector2(width, rowH);

		if ((Object)(object)spr != (Object)null)
		{
			GameObject ic = new GameObject("Icon");
			ic.transform.SetParent(row.transform, false);
			RectTransform ir = ic.AddComponent<RectTransform>();
			ir.anchorMin = new Vector2(0f, 1f);
			ir.anchorMax = new Vector2(0f, 1f);
			ir.pivot = new Vector2(0f, 1f);
			ir.anchoredPosition = new Vector2(0f, -2f);
			ir.sizeDelta = new Vector2(iconSize, iconSize);
			Image img = ic.AddComponent<Image>();
			img.sprite = spr;
			img.preserveAspect = true;
			ItemTooltipsMod.RegisterStageRelicIcon(ic, it, itemName, itemDesc, spr);
		}

		var label = MakeTmp(row.transform, "Name", itemName, font, 12f, Soft, false);
		var lrt = label.GetComponent<RectTransform>();
		var ltmp = label.GetComponent<TextMeshProUGUI>();
		// Fixed top-left box next to icon (not stretch) so wrapping works
		lrt.anchorMin = new Vector2(0f, 1f);
		lrt.anchorMax = new Vector2(0f, 1f);
		lrt.pivot = new Vector2(0f, 1f);
		lrt.anchoredPosition = new Vector2(iconSize + iconGap, 0f);
		ConfigureWrap(ltmp, textWidth);
		((TMP_Text)ltmp).alignment = (TextAlignmentOptions)257; // top-left
		float finalH = MeasureWrappedHeight(ltmp, lrt, textWidth, 18f, 120f);
		lrt.sizeDelta = new Vector2(textWidth, finalH);
		// Grow row if measure drifted
		rowH = Mathf.Max(rowH, finalH + 4f);
		rt.sizeDelta = new Vector2(width, rowH);

		return y + rowH + 4f;
	}

	private static GameObject MakeTmp(Transform parent, string name, string text, TMP_FontAsset font, float size, Color color, bool bold)
	{
		GameObject go = new GameObject(name);
		go.transform.SetParent(parent, false);
		RectTransform rt = go.AddComponent<RectTransform>();
		// Left-top anchored fixed box — width is set by callers for wrapping
		rt.anchorMin = new Vector2(0f, 1f);
		rt.anchorMax = new Vector2(0f, 1f);
		rt.pivot = new Vector2(0f, 1f);
		TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
		((TMP_Text)tmp).font = font;
		((TMP_Text)tmp).text = text ?? "";
		((TMP_Text)tmp).fontSize = size;
		((TMP_Text)tmp).fontStyle = bold ? (FontStyles)1 : (FontStyles)0;
		((Graphic)tmp).color = color;
		((TMP_Text)tmp).alignment = (TextAlignmentOptions)257;
		((TMP_Text)tmp).enableWordWrapping = true;
		((TMP_Text)tmp).overflowMode = TextOverflowModes.Overflow;
		((TMP_Text)tmp).richText = true;
		((Graphic)tmp).raycastTarget = false;
		return go;
	}

	private static Image MakeTabButton(Transform parent, string name, string label, TMP_FontAsset font, float anchorX, out TextMeshProUGUI tmpOut)
	{
		GameObject go = new GameObject(name);
		go.transform.SetParent(parent, false);
		RectTransform rt = go.AddComponent<RectTransform>();
		rt.anchorMin = new Vector2(anchorX, 0f);
		rt.anchorMax = new Vector2(anchorX + 0.5f, 1f);
		rt.offsetMin = new Vector2(2f, 2f);
		rt.offsetMax = new Vector2(-2f, -2f);
		Image img = go.AddComponent<Image>();
		img.color = TabOff;
		((Graphic)img).raycastTarget = true;

		GameObject textGo = new GameObject("Label");
		textGo.transform.SetParent(go.transform, false);
		RectTransform tr = textGo.AddComponent<RectTransform>();
		tr.anchorMin = Vector2.zero;
		tr.anchorMax = Vector2.one;
		tr.offsetMin = Vector2.zero;
		tr.offsetMax = Vector2.zero;
		TextMeshProUGUI tmp = textGo.AddComponent<TextMeshProUGUI>();
		((TMP_Text)tmp).font = font;
		((TMP_Text)tmp).text = label;
		((TMP_Text)tmp).fontSize = 14f;
		((TMP_Text)tmp).fontStyle = (FontStyles)1;
		((Graphic)tmp).color = Soft;
		((TMP_Text)tmp).alignment = (TextAlignmentOptions)514; // mid
		((Graphic)tmp).raycastTarget = false;
		tmpOut = tmp;
		return img;
	}

	private static void AddClick(GameObject go, Action action)
	{
		EventTrigger et = go.GetComponent<EventTrigger>();
		if ((Object)(object)et == (Object)null)
			et = go.AddComponent<EventTrigger>();
		var entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerClick;
		Action captured = action;
		((UnityEngine.Events.UnityEvent<BaseEventData>)(object)entry.callback).AddListener(
			(UnityEngine.Events.UnityAction<BaseEventData>)(Action<BaseEventData>)(delegate { captured(); }));
		et.triggers.Add(entry);
	}

	private static void CopyRect(RectTransform dst, RectTransform src)
	{
		dst.anchorMin = src.anchorMin;
		dst.anchorMax = src.anchorMax;
		dst.pivot = src.pivot;
		dst.anchoredPosition = src.anchoredPosition;
		dst.sizeDelta = src.sizeDelta;
		dst.offsetMin = src.offsetMin;
		dst.offsetMax = src.offsetMax;
		dst.localScale = src.localScale;
		dst.localRotation = src.localRotation;
	}

	private static void CopyHorizontalPlacement(RectTransform dst, RectTransform src)
	{
		dst.anchorMin = src.anchorMin;
		dst.anchorMax = src.anchorMax;
		dst.pivot = src.pivot;
		dst.anchoredPosition = src.anchoredPosition;
		dst.sizeDelta = src.sizeDelta;
		dst.offsetMin = src.offsetMin;
		dst.offsetMax = src.offsetMax;
	}

	private static string SafeLoc(Func<string> loc, string fallback)
	{
		try
		{
			string s = loc();
			if (!string.IsNullOrEmpty(s))
			{
				// I2 sometimes returns term paths
				string t = GameData.Translate(s);
				if (!string.IsNullOrEmpty(t)) return t;
				if (!s.Contains("/") || s.Contains(" ")) return s;
			}
		}
		catch { }
		if (!string.IsNullOrEmpty(fallback))
		{
			string t = GameData.Translate(fallback);
			return !string.IsNullOrEmpty(t) ? t : fallback;
		}
		return "";
	}
}
