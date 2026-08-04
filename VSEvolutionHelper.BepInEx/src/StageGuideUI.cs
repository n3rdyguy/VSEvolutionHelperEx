using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using VampireSurvivors.App.Objects;
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
	private static Button _tabMusicBtn;
	private static Button _tabGuideBtn;
	private static readonly List<GameObject> _guideRelicSelectables = new List<GameObject>();
	private static StageData _stage;
	private static StageType _stageType;
	private static StageItemUI _stageItem;
	// VS-style gold tab strip (matches song panel frame)
	private static readonly Color TabOn = new Color(0.82f, 0.62f, 0.22f, 0.98f);
	private static readonly Color TabOff = new Color(0.18f, 0.16f, 0.2f, 0.96f);
	private static readonly Color TabOnBorder = new Color(1f, 0.88f, 0.45f, 1f);
	private static readonly Color TabOffBorder = new Color(0.55f, 0.45f, 0.22f, 0.9f);
	private static readonly Color TabBarBg = new Color(0.12f, 0.1f, 0.12f, 0.95f);
	private static readonly Color PanelBg = new Color(0.22f, 0.22f, 0.26f, 0.96f);
	private static readonly Color Gold = new Color(0.95f, 0.8f, 0.35f, 1f);
	private static readonly Color Soft = new Color(0.9f, 0.88f, 0.82f, 1f);
	private static readonly Color Muted = new Color(0.65f, 0.7f, 0.85f, 1f);
	private const float TabHeight = 36f;
	private const float TabGap = 3f;

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
		_tabMusicBtn = null;
		_tabGuideBtn = null;
		_guideRelicSelectables.Clear();
		_stage = null;
		_stageItem = null;
		_tab = Tab.Music;
		_tabInitialized = false;
	}

	/// <summary>
	/// Controller / keyboard helpers while Stage Selection Guide chrome is alive.
	/// LB/RB (or Q/E) switch Music|Guide; vertical axis scrolls Guide; D-pad left/right on tabs.
	/// </summary>
	public static void TickInput()
	{
		if (!Plugin.StageGuideEnabled || (Object)(object)_tabBar == (Object)null)
			return;
		if (!_tabBar.activeInHierarchy)
			return;

		// Shoulder / hotkeys — work even when focus is on the stage list
		if (Input.GetKeyDown(KeyCode.JoystickButton4) || Input.GetKeyDown(KeyCode.Q)
			|| Input.GetKeyDown(KeyCode.PageUp))
		{
			SetTab(Tab.Music);
			FocusTabButton(Tab.Music);
			return;
		}
		if (Input.GetKeyDown(KeyCode.JoystickButton5) || Input.GetKeyDown(KeyCode.E)
			|| Input.GetKeyDown(KeyCode.PageDown))
		{
			SetTab(Tab.Guide);
			FocusTabButton(Tab.Guide);
			return;
		}

		// When a tab button is focused, left/right switches
		EventSystem es = EventSystem.current;
		if ((Object)(object)es != (Object)null)
		{
			GameObject sel = es.currentSelectedGameObject;
			bool onMusic = (Object)(object)_tabMusicBtn != (Object)null
				&& (Object)(object)sel == (Object)(object)_tabMusicBtn.gameObject;
			bool onGuide = (Object)(object)_tabGuideBtn != (Object)null
				&& (Object)(object)sel == (Object)(object)_tabGuideBtn.gameObject;
			if (onMusic || onGuide)
			{
				float h = 0f;
				try { h = Input.GetAxisRaw("Horizontal"); } catch { }
				if (h > 0.5f || Input.GetKeyDown(KeyCode.RightArrow))
				{
					SetTab(Tab.Guide);
					FocusTabButton(Tab.Guide);
				}
				else if (h < -0.5f || Input.GetKeyDown(KeyCode.LeftArrow))
				{
					SetTab(Tab.Music);
					FocusTabButton(Tab.Music);
				}
			}
		}

		// Scroll Guide content with vertical stick / arrows when Guide is visible
		if (_tab == Tab.Guide && (Object)(object)_guideScroll != (Object)null && _guideRoot != null && _guideRoot.activeInHierarchy)
		{
			float v = 0f;
			try { v = Input.GetAxisRaw("Vertical"); } catch { }
			if (Input.GetKey(KeyCode.UpArrow)) v = 1f;
			if (Input.GetKey(KeyCode.DownArrow)) v = -1f;
			if (Mathf.Abs(v) > 0.2f)
			{
				float speed = 0.9f * Time.unscaledDeltaTime;
				_guideScroll.verticalNormalizedPosition = Mathf.Clamp01(
					_guideScroll.verticalNormalizedPosition + v * speed);
			}
		}
	}

	private static void FocusTabButton(Tab tab)
	{
		try
		{
			EventSystem es = EventSystem.current;
			if ((Object)(object)es == (Object)null) return;
			Button b = tab == Tab.Guide ? _tabGuideBtn : _tabMusicBtn;
			if ((Object)(object)b != (Object)null)
				es.SetSelectedGameObject(b.gameObject);
		}
		catch { }
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

		// Tab bar — same horizontal bounds as song panel, sit flush on its top edge
		if ((Object)(object)_tabBar == (Object)null)
		{
			_tabBar = new GameObject("EvoHelper_StageTabs");
			_tabBar.transform.SetParent(parent, false);
			RectTransform tabRt = _tabBar.AddComponent<RectTransform>();
			PlaceTabBarAboveSong(tabRt, _songPanelRt);

			// Shared dark strip + gold outline so tabs read as one control with the panel
			Image barBg = _tabBar.AddComponent<Image>();
			barBg.color = TabBarBg;
			((Graphic)barBg).raycastTarget = false;
			Outline barOl = _tabBar.AddComponent<Outline>();
			((Shadow)barOl).effectColor = TabOnBorder;
			((Shadow)barOl).effectDistance = new Vector2(1.5f, -1.5f);

			// Two equal tabs (Buttons for mouse + EventSystem / controller)
			_tabMusicBg = MakeTabButton(_tabBar.transform, "TabMusic", "Music", font, 0, out _tabMusicLabel, out _tabMusicBtn);
			_tabGuideBg = MakeTabButton(_tabBar.transform, "TabGuide", "Guide", font, 1, out _tabGuideLabel, out _tabGuideBtn);
			WireTabNavigation();
			AddClick(_tabMusicBg.gameObject, () => SetTab(Tab.Music));
			AddClick(_tabGuideBg.gameObject, () => SetTab(Tab.Guide));
			// Button onClick (controller Submit / A)
			try
			{
				if ((Object)(object)_tabMusicBtn != (Object)null)
					_tabMusicBtn.onClick.AddListener((UnityAction)(() => SetTab(Tab.Music)));
				if ((Object)(object)_tabGuideBtn != (Object)null)
					_tabGuideBtn.onClick.AddListener((UnityAction)(() => SetTab(Tab.Guide)));
			}
			catch (Exception ex)
			{
				Plugin.Log.LogWarning("[StageGuide] tab onClick: " + ex.Message);
			}
		}
		else
		{
			// Keep aligned if song panel rect moved (resolution / layout refresh)
			try
			{
				RectTransform tabRt = _tabBar.GetComponent<RectTransform>();
				if ((Object)(object)tabRt != (Object)null)
					PlaceTabBarAboveSong(tabRt, _songPanelRt);
			}
			catch { }
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

	private static void WireTabNavigation()
	{
		if ((Object)(object)_tabMusicBtn == (Object)null || (Object)(object)_tabGuideBtn == (Object)null)
			return;
		try
		{
			Navigation nMusic = _tabMusicBtn.navigation;
			nMusic.mode = Navigation.Mode.Explicit;
			nMusic.selectOnRight = _tabGuideBtn;
			nMusic.selectOnLeft = _tabGuideBtn;
			_tabMusicBtn.navigation = nMusic;

			Navigation nGuide = _tabGuideBtn.navigation;
			nGuide.mode = Navigation.Mode.Explicit;
			nGuide.selectOnLeft = _tabMusicBtn;
			nGuide.selectOnRight = _tabMusicBtn;
			_tabGuideBtn.navigation = nGuide;
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[StageGuide] tab navigation: " + ex.Message);
		}
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
		{
			bg.color = on ? TabOn : TabOff;
			Outline ol = bg.GetComponent<Outline>();
			if ((Object)(object)ol != (Object)null)
			{
				((Shadow)ol).effectColor = on ? TabOnBorder : TabOffBorder;
				((Shadow)ol).effectDistance = on ? new Vector2(1.8f, -1.8f) : new Vector2(1.2f, -1.2f);
			}
		}
		if ((Object)(object)label != (Object)null)
		{
			((Graphic)label).color = on ? Color.white : Soft;
			((TMP_Text)label).fontStyle = on ? (FontStyles)1 : (FontStyles)0;
		}
	}

	/// <summary>
	/// Match song panel left/right edges; place tab strip sitting on the panel's top edge.
	/// Handles both fixed and stretch-anchored song panels.
	/// </summary>
	private static void PlaceTabBarAboveSong(RectTransform tabRt, RectTransform songRt)
	{
		if ((Object)(object)tabRt == (Object)null || (Object)(object)songRt == (Object)null)
			return;

		bool stretchX = !Mathf.Approximately(songRt.anchorMin.x, songRt.anchorMax.x);

		if (stretchX)
		{
			// Stretch horizontally like the song panel; pin to its top edge
			tabRt.anchorMin = new Vector2(songRt.anchorMin.x, songRt.anchorMax.y);
			tabRt.anchorMax = new Vector2(songRt.anchorMax.x, songRt.anchorMax.y);
			tabRt.pivot = new Vector2(0.5f, 0f);
			// Horizontal offsets match the song frame; height via sizeDelta.y
			tabRt.offsetMin = new Vector2(songRt.offsetMin.x, 0f);
			tabRt.offsetMax = new Vector2(songRt.offsetMax.x, TabHeight);
			tabRt.anchoredPosition = Vector2.zero;
		}
		else
		{
			// Point anchors: same X as song, width = song width, bottom on song top
			tabRt.anchorMin = songRt.anchorMin;
			tabRt.anchorMax = songRt.anchorMax;
			tabRt.pivot = new Vector2(songRt.pivot.x, 0f);
			Vector2 songPos = songRt.anchoredPosition;
			Vector2 songSize = songRt.sizeDelta;
			float topY = songPos.y + songSize.y * (1f - songRt.pivot.y);
			tabRt.anchoredPosition = new Vector2(songPos.x, topY);
			tabRt.sizeDelta = new Vector2(songSize.x, TabHeight);
		}
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
		_guideRelicSelectables.Clear();

		TMP_FontAsset font = ItemTooltipsMod.GetUiFont();
		if ((Object)(object)font == (Object)null) return;

		GameData.EnsureLoaded();
		float y = 0f;
		// Prefer song panel width so wrap matches the visible column
		float width = ResolveContentWidth();

		// Title only — flavor description already shows beside the stage list
		string name = SafeLoc(() => _stage.GetLocalizedName(_stageType), _stage.stageName);
		y = AddHeader(_guideScrollContent.transform, font, name, Gold, 18f, width, y);

		// Progression + modifier summary
		string hyper = "?";
		try
		{
			if ((Object)(object)_stageItem != (Object)null)
				hyper = _stageItem.HasHyperUnlocked() ? "Yes" : "No";
		}
		catch { }
		var progLines = new List<string> { $"Hyper unlocked: {hyper}" };
		try
		{
			int minutes = _stage.minute;
			if (minutes > 0)
				progLines.Add($"Stage length: {minutes} min" + (_stage.randomMinutes ? " (random)" : ""));
		}
		catch { }
		string modSummary = FormatStageMods("Normal", SafeStageMods(() => _stage.mods));
		if (!string.IsNullOrEmpty(modSummary)) progLines.Add(modSummary);
		string hyperMods = FormatStageMods("Hyper", SafeStageMods(() => _stage.hyper));
		if (!string.IsNullOrEmpty(hyperMods)) progLines.Add(hyperMods);
		string invMods = FormatStageMods("Inverse", SafeStageMods(() => _stage.inverse));
		if (!string.IsNullOrEmpty(invMods)) progLines.Add(invMods);

		y = AddHeader(_guideScrollContent.transform, font, "Progression", Gold, 14f, width, y + 4f);
		y = AddBody(_guideScrollContent.transform, font, string.Join("\n", progLines), Muted, 12f, width, y, 2f);

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

	private static StageModifiers SafeStageMods(Func<StageModifiers> getter)
	{
		try { return getter(); } catch { return null; }
	}

	/// <summary>One-line summary of notable StageModifiers (skips empty / default).</summary>
	private static string FormatStageMods(string label, StageModifiers mods)
	{
		if (mods == null) return null;
		var parts = new List<string>();
		void add(string name, Il2CppSystem.Nullable<float> n)
		{
			try
			{
				if (n != null && n.HasValue)
				{
					float v = n.Value;
					if (Mathf.Abs(v) < 0.001f) return;
					parts.Add($"{name}×{v:0.##}");
				}
			}
			catch { }
		}
		try
		{
			add("HP", mods.EnemyHealthMultiplier);
			add("Gold", mods.GoldMultiplier);
			add("EnemySpd", mods.EnemySpeed);
			add("PlayerSpd", mods.PlayerPxSpeed);
			add("XP", mods.XpBonus);
			add("Luck", mods.LuckBonus);
			add("Proj", mods.ProjectileSpeed);
			add("Clock", mods.ClockSpeed);
			try
			{
				var tl = mods.TimeLimit;
				if (tl != null && tl.HasValue && tl.Value > 0f)
					parts.Add($"TimeLimit {tl.Value:0.#}m");
			}
			catch { }
		}
		catch { return null; }
		if (parts.Count == 0) return null;
		return $"{label}: {string.Join(", ", parts)}";
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
			((Graphic)img).raycastTarget = true;
			// Selectable for controller focus + mouse hover registration
			try
			{
				Button btn = ic.AddComponent<Button>();
				((Selectable)btn).targetGraphic = img;
				Navigation nav = ((Selectable)btn).navigation;
				nav.mode = Navigation.Mode.Vertical;
				((Selectable)btn).navigation = nav;
				ColorBlock colors = ((Selectable)btn).colors;
				colors.highlightedColor = new Color(0.9f, 0.95f, 1f, 1f);
				colors.selectedColor = new Color(0.85f, 0.9f, 1f, 1f);
				((Selectable)btn).colors = colors;
			}
			catch { }
			ItemTooltipsMod.RegisterStageRelicIcon(ic, it, itemName, itemDesc, spr);
			_guideRelicSelectables.Add(ic);
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

	/// <param name="index">0 = Music (left), 1 = Guide (right)</param>
	private static Image MakeTabButton(Transform parent, string name, string label, TMP_FontAsset font, int index, out TextMeshProUGUI tmpOut, out Button btnOut)
	{
		GameObject go = new GameObject(name);
		go.transform.SetParent(parent, false);
		RectTransform rt = go.AddComponent<RectTransform>();
		// Half width each with a small gap in the middle
		float aMin = index == 0 ? 0f : 0.5f;
		float aMax = index == 0 ? 0.5f : 1f;
		rt.anchorMin = new Vector2(aMin, 0f);
		rt.anchorMax = new Vector2(aMax, 1f);
		// Inset from bar edges; gap between the two tabs
		float leftPad = index == 0 ? 3f : TabGap * 0.5f;
		float rightPad = index == 0 ? TabGap * 0.5f : 3f;
		rt.offsetMin = new Vector2(leftPad, 3f);
		rt.offsetMax = new Vector2(-rightPad, -3f);

		Image img = go.AddComponent<Image>();
		img.color = TabOff;
		((Graphic)img).raycastTarget = true;
		Outline ol = go.AddComponent<Outline>();
		((Shadow)ol).effectColor = TabOffBorder;
		((Shadow)ol).effectDistance = new Vector2(1.2f, -1.2f);

		Button btn = go.AddComponent<Button>();
		((Selectable)btn).targetGraphic = img;
		ColorBlock colors = ((Selectable)btn).colors;
		colors.normalColor = Color.white;
		colors.highlightedColor = new Color(1.05f, 1f, 0.9f, 1f);
		colors.selectedColor = new Color(1.08f, 1.02f, 0.85f, 1f);
		colors.pressedColor = new Color(0.92f, 0.88f, 0.75f, 1f);
		colors.fadeDuration = 0.06f;
		((Selectable)btn).colors = colors;
		btnOut = btn;

		GameObject textGo = new GameObject("Label");
		textGo.transform.SetParent(go.transform, false);
		RectTransform tr = textGo.AddComponent<RectTransform>();
		tr.anchorMin = Vector2.zero;
		tr.anchorMax = Vector2.one;
		tr.offsetMin = new Vector2(2f, 1f);
		tr.offsetMax = new Vector2(-2f, -1f);
		TextMeshProUGUI tmp = textGo.AddComponent<TextMeshProUGUI>();
		((TMP_Text)tmp).font = font;
		((TMP_Text)tmp).text = label;
		((TMP_Text)tmp).fontSize = 16f;
		((TMP_Text)tmp).fontStyle = (FontStyles)1;
		((Graphic)tmp).color = Soft;
		((TMP_Text)tmp).alignment = (TextAlignmentOptions)514; // mid
		((TMP_Text)tmp).enableWordWrapping = false;
		((TMP_Text)tmp).overflowMode = TextOverflowModes.Ellipsis;
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
