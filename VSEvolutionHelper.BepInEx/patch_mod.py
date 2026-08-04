#!/usr/bin/env python3
"""Restore + patch ItemTooltipsMod.cs for BepInEx + typed GameData."""
import re
from pathlib import Path

ROOT = Path(__file__).resolve().parent
SRC = ROOT / "src" / "ItemTooltipsMod.cs"
DECOMP = ROOT.parent / "decompiled" / "VSItemTooltips" / "ItemTooltipsMod.cs"

def main():
    c = DECOMP.read_text(encoding="utf-8")

    reps = [
        (r"using MelonLoader;\r?\n", ""),
        (r"using Il2CppVampireSurvivors\.Data\.Weapons;", "using VampireSurvivors.Data.Weapons;"),
        (r"using Il2CppVampireSurvivors\.Data;", "using VampireSurvivors.Data;"),
        (r"using Il2CppVampireSurvivors\.UI;", "using VampireSurvivors.UI;"),
        (r"using Il2CppTMPro;", "using TMPro;"),
        (r"using Il2CppSystem\.Collections\.Generic;\r?\n", ""),
        (r"using Il2CppSystem;\r?\n", ""),
        (r"MelonLogger\.Msg\(", "Plugin.Log.LogInfo("),
        (r"MelonLogger\.Warning\(", "Plugin.Log.LogWarning("),
        (r"MelonLogger\.Error\(", "Plugin.Log.LogError("),
        (r"MelonCoroutines\.Start\(DelayedHideCheck\(\)\);",
         "DelayFrames(10, () => { if (mouseOverPopupIndex < 0 && popupStack.Count > 0) HideAllPopups(); });"),
        (r"MelonCoroutines\.Start\(DelayedStackHideCheck\(thisPopupIndex\)\);",
         "DelayFrames(10, () => { int closeFromIndex = mouseOverPopupIndex + 1; if (closeFromIndex < 0) closeFromIndex = 0; while (popupStack.Count > closeFromIndex) HideTopPopup(); });"),
        (r"public class ItemTooltipsMod : MelonMod", "public class ItemTooltipsMod"),
        (r"public override void OnInitializeMelon\(\)", "public static void Initialize()"),
        (r"public override void OnSceneWasLoaded\(int buildIndex, string sceneName\)", "public static void OnSceneLoaded(int buildIndex, string sceneName)"),
        (r"public override void OnUpdate\(\)", "public static void Update()"),
        (r"private void ApplyPatches\(\)", "private static void ApplyPatches()"),
        (r"private void TryPatchLevelUpItemUI\(\)", "private static void TryPatchLevelUpItemUI()"),
        (r"private void TryPatchEquipmentIconPause\(\)", "private static void TryPatchEquipmentIconPause()"),
        (r"private void TryPatchMerchantPage\(\)", "private static void TryPatchMerchantPage()"),
        (r"private void TryEarlyCaching\(\)", "private static void TryEarlyCaching()"),
        (r"harmonyInstance = new Harmony\(", "harmonyInstance = new HarmonyLib.Harmony("),
        (r'if \(!assembly2\.FullName\.Contains\("Il2Cpp"\)\)',
         'if (!(assembly2.FullName.Contains("VampireSurvivors") || assembly2.FullName.Contains("Il2Cpp") || assembly2.GetName().Name == "Assembly-CSharp"))'),
        (r"\(\(Vector3\)\(ref (\w+)\)\)\.", r"\1."),
        (r"\(\(Vector2\)\(ref (\w+)\)\)\.", r"\1."),
        (r"\(\(ColorBlock\)\(ref (\w+)\)\)\.", r"\1."),
        (r"\(\(Rect\)\(ref (\w+)\)\)\.", r"\1."),
        (r"navigation\.mode = \(Mode\)", "navigation.mode = (Navigation.Mode)"),
        (r"\(Mode\)0", "(Navigation.Mode)0"),
        (r"(?<![\w.])FitMode\b", "ContentSizeFitter.FitMode"),
        (r"ContentSizeFitter\.ContentSizeFitter\.FitMode", "ContentSizeFitter.FitMode"),
        (r"(?<![\w.])Entry\b", "EventTrigger.Entry"),
        (r"EventTrigger\.EventTrigger\.Entry", "EventTrigger.Entry"),
        (r"UnityAction\.op_Implicit\(\(Action\)", "(UnityEngine.Events.UnityAction)(System.Action)("),
        (r"UnityAction<BaseEventData>\.op_Implicit\(\(Action<BaseEventData>\)",
         "(UnityEngine.Events.UnityAction<UnityEngine.EventSystems.BaseEventData>)(System.Action<UnityEngine.EventSystems.BaseEventData>)("),
        (r"object\?", "object"),
        (r'string text = "Unknown";',
         'string text = weaponType.HasValue ? GameData.GetWeaponName(weaponType.Value) : (itemType.HasValue ? itemType.Value.ToString() : "Unknown");'),
        (r"if \(weaponType\.HasValue && cachedWeaponsDict != null\)", "if (weaponType.HasValue)"),
        (r"else if \(itemType\.HasValue && cachedPowerUpsDict != null\)", "else if (itemType.HasValue)"),
        (r"Enum\.TryParse<WeaponType>\(([^,]+),\s*out WeaponType", r"GameData.TryParseWeaponType(\1, out WeaponType"),
    ]
    for pat, rep in reps:
        c = re.sub(pat, rep, c)

    if "using Object = UnityEngine.Object" not in c:
        c = c.replace("using UnityEngine;", "using UnityEngine;\nusing Object = UnityEngine.Object;")

    helpers = """
	private static int lastSceneBuildIndex = -1;
	private static readonly List<(int framesLeft, Action action)> pendingDelayedActions = new List<(int, Action)>();
	internal static void DelayFrames(int frames, Action action) { pendingDelayedActions.Add((frames, action)); }
	private static void ProcessDelayedActions() {
		for (int i = pendingDelayedActions.Count - 1; i >= 0; i--) {
			var item = pendingDelayedActions[i];
			int left = item.framesLeft - 1;
			if (left <= 0) { pendingDelayedActions.RemoveAt(i); try { item.action(); } catch (Exception ex) { Plugin.Log.LogWarning("Delayed: " + ex.Message); } }
			else pendingDelayedActions[i] = (left, item.action);
		}
	}
"""
    if "ProcessDelayedActions" not in c:
        c = c.replace("private static bool wasGamePaused = false;", "private static bool wasGamePaused = false;" + helpers)

    if "if (!GameData.IsReady)" not in c:
        c = c.replace(
            "public static void Update()\n\t{",
            "public static void Update()\n\t{\n\t\tProcessDelayedActions();\n\t\tif (!GameData.IsReady) GameData.EnsureLoaded();\n\t\ttry { int bi = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex; if (bi != lastSceneBuildIndex) { lastSceneBuildIndex = bi; OnSceneLoaded(bi, UnityEngine.SceneManagement.SceneManager.GetActiveScene().name); } } catch {}",
        )
        # also windows newlines
        c = c.replace(
            "public static void Update()\r\n\t{",
            "public static void Update()\r\n\t{\r\n\t\tProcessDelayedActions();\r\n\t\tif (!GameData.IsReady) GameData.EnsureLoaded();\r\n\t\ttry { int bi = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex; if (bi != lastSceneBuildIndex) { lastSceneBuildIndex = bi; OnSceneLoaded(bi, UnityEngine.SceneManagement.SceneManager.GetActiveScene().name); } } catch {}",
        )

    def replace_method(src: str, start: str, end_marker: str, new_body: str) -> str:
        si = src.find(start)
        ei = src.find(end_marker)
        if si < 0 or ei < 0 or ei <= si:
            print(f"SKIP {start[:50]} si={si} ei={ei}")
            return src
        print(f"OK {start[:50]}")
        return src[:si] + new_body + src[ei:]

    c = replace_method(c, "private static WeaponData GetWeaponData(WeaponType type)", "private static object GetPowerUpData",
        "private static WeaponData GetWeaponData(WeaponType type)\n\t{\n\t\treturn GameData.GetWeaponData(type);\n\t}\n\n\t")
    c = replace_method(c, "private static List<WeaponData> GetWeaponDataList(WeaponType type)", "private static void PositionPopup",
        "private static List<WeaponData> GetWeaponDataList(WeaponType type)\n\t{\n\t\tvar il2 = GameData.GetWeaponDataList(type);\n\t\tif (il2 == null || il2.Count == 0) return null;\n\t\tvar list = new List<WeaponData>();\n\t\tfor (int i = 0; i < il2.Count; i++) if (il2[i] != null) list.Add(il2[i]);\n\t\treturn list.Count > 0 ? list : null;\n\t}\n\n\t")
    c = replace_method(c, "private static Sprite LoadSpriteFromAtlas(string frameName, string atlasName)", "private static Sprite GetSpriteForWeapon",
        "private static Sprite LoadSpriteFromAtlas(string frameName, string atlasName)\n\t{\n\t\treturn GameData.LoadSprite(frameName, atlasName);\n\t}\n\n\t")
    c = replace_method(c, "private static Sprite GetSpriteForWeapon(WeaponType weaponType)", "private static Sprite GetSpriteForItem",
        "private static Sprite GetSpriteForWeapon(WeaponType weaponType)\n\t{\n\t\treturn GameData.GetSprite(weaponType);\n\t}\n\n\t")
    c = replace_method(c, "private static string GetI2Translation(string term)", "private static TMP_FontAsset GetFont",
        "private static string GetI2Translation(string term)\n\t{\n\t\treturn GameData.Translate(term);\n\t}\n\n\t")
    c = replace_method(c, "private static string GetLocalizedWeaponDescription(WeaponData data, WeaponType type)", "private unsafe static string GetLocalizedWeaponName",
        "private static string GetLocalizedWeaponDescription(WeaponData data, WeaponType type)\n\t{\n\t\treturn GameData.GetWeaponDescription(type);\n\t}\n\n\t")
    c = replace_method(c, "private unsafe static string GetLocalizedWeaponName(WeaponData data, WeaponType type)", "private static string GetLocalizedPowerUpDescription",
        "private static string GetLocalizedWeaponName(WeaponData data, WeaponType type)\n\t{\n\t\treturn GameData.GetWeaponName(type);\n\t}\n\n\t")

    # CacheDataManager
    old = """public static void CacheDataManager(object dataManager)
	{
		if (dataManager == null)
		{
			return;
		}
		cachedDataManager = dataManager;"""
    new = """public static void CacheDataManager(object dataManager)
	{
		if (dataManager == null)
		{
			return;
		}
		if (dataManager is VampireSurvivors.Data.DataManager dmTyped)
		{
			GameData.CacheFrom(dmTyped);
			cachedDataManager = dmTyped;
			cachedWeaponsDict = GameData.WeaponsDict;
			cachedPowerUpsDict = GameData.PowerUpsDict;
			if (spriteToWeaponType == null) spriteToWeaponType = new Dictionary<string, WeaponType>();
			spriteToWeaponType.Clear();
			foreach (var kv in GameData.SpriteToWeapon) spriteToWeaponType[kv.Key] = kv.Value;
			lookupTablesBuilt = GameData.IsReady;
			return;
		}
		cachedDataManager = dataManager;"""
    if old in c:
        c = c.replace(old, new)
        print("OK CacheDataManager")
    else:
        print("SKIP CacheDataManager")

    oldb = """private static void BuildLookupTables()
	{
		if (lookupTablesBuilt)
		{
			return;
		}
		spriteToWeaponType = new Dictionary<string, WeaponType>();
		spriteToItemType = new Dictionary<string, ItemType>();
		try
		{"""
    newb = """private static void BuildLookupTables()
	{
		if (lookupTablesBuilt)
		{
			return;
		}
		if (GameData.EnsureLoaded())
		{
			cachedWeaponsDict = GameData.WeaponsDict;
			cachedPowerUpsDict = GameData.PowerUpsDict;
			cachedDataManager = GameData.DataManager;
			if (spriteToWeaponType == null) spriteToWeaponType = new Dictionary<string, WeaponType>();
			if (spriteToItemType == null) spriteToItemType = new Dictionary<string, ItemType>();
			spriteToWeaponType.Clear();
			foreach (var kv in GameData.SpriteToWeapon) spriteToWeaponType[kv.Key] = kv.Value;
			lookupTablesBuilt = true;
			if (!loggedLookupTables) { loggedLookupTables = true; Plugin.Log.LogInfo("Built lookup tables via GameData (typed)"); }
			return;
		}
		spriteToWeaponType = new Dictionary<string, WeaponType>();
		spriteToItemType = new Dictionary<string, ItemType>();
		try
		{"""
    if oldb in c:
        c = c.replace(oldb, newb)
        print("OK BuildLookupTables")

    # Equipment patch
    olda = """TryPatchMerchantPage();
			TryPatchLevelUpItemUI();
			TryPatchEquipmentIconPause();
			Plugin.Log.LogInfo("Patches applied successfully");"""
    newa = """TryPatchMerchantPage();
			TryPatchLevelUpItemUI();
			TryPatchEquipmentIconPause();
			try {
				var equipMethod = typeof(VampireSurvivors.UI.EquipmentIconPaused).GetMethod("SetData", BindingFlags.Instance | BindingFlags.Public);
				if (equipMethod != null)
					harmonyInstance.Patch(equipMethod, null, new HarmonyMethod(typeof(EquipmentIconPatches), nameof(EquipmentIconPatches.SetData_Weapon_Postfix)));
			} catch (Exception ex) { Plugin.Log.LogWarning("EquipmentIconPaused patch: " + ex.Message); }
			Plugin.Log.LogInfo("Patches applied successfully");"""
    if olda in c:
        c = c.replace(olda, newa)
        print("OK ApplyPatches")

    # Nested click fix
    oldc = """if (useClick)
		{
			formulaIconData[((Object)go).GetInstanceID()] = (weaponType, itemType);
			Button val2 = go.AddComponent<Button>();
			Image component2 = go.GetComponent<Image>();
			if ((Object)(object)component2 != (Object)null)
			{
				((Selectable)val2).targetGraphic = (Graphic)(object)component2;
			}
			ColorBlock colors = ((Selectable)val2).colors;
			colors.normalColor = new Color(0f, 0f, 0f, 0f);
			colors.highlightedColor = new Color(0f, 0.9f, 1f, 0.2f);
			colors.selectedColor = new Color(0f, 0.9f, 1f, 0.35f);
			colors.pressedColor = new Color(0f, 0.9f, 1f, 0.5f);
			colors.fadeDuration = 0.1f;
			((Selectable)val2).colors = colors;
			((UnityEvent)val2.onClick).AddListener((UnityEngine.Events.UnityAction)(System.Action)(delegate
			{
				ShowItemPopup(go.transform, weaponType, itemType);
			}));
			Navigation navigation = ((Selectable)val2).navigation;
			navigation.mode = (Navigation.Mode)0;
			((Selectable)val2).navigation = navigation;
		}"""
    newc = """if (useClick)
		{
			formulaIconData[((Object)go).GetInstanceID()] = (weaponType, itemType);
			// Nested tooltips: PointerClick is reliable under Il2Cpp
			EventTrigger.Entry click = new EventTrigger.Entry();
			click.eventID = EventTriggerType.PointerClick;
			((UnityEvent<BaseEventData>)(object)click.callback).AddListener((UnityEngine.Events.UnityAction<UnityEngine.EventSystems.BaseEventData>)(System.Action<UnityEngine.EventSystems.BaseEventData>)(delegate
			{
				ShowItemPopup(go.transform, weaponType, itemType);
			}));
			val.triggers.Add(click);
			if (go.GetComponent<Button>() == null)
			{
				Button val2 = go.AddComponent<Button>();
				Image component2 = go.GetComponent<Image>();
				if ((Object)(object)component2 != (Object)null)
					((Selectable)val2).targetGraphic = (Graphic)(object)component2;
				ColorBlock colors = ((Selectable)val2).colors;
				colors.normalColor = new Color(1f, 1f, 1f, 1f);
				colors.highlightedColor = new Color(0.85f, 1f, 1f, 1f);
				colors.selectedColor = new Color(0.75f, 1f, 1f, 1f);
				colors.pressedColor = new Color(0.7f, 0.95f, 1f, 1f);
				colors.fadeDuration = 0.05f;
				((Selectable)val2).colors = colors;
				Navigation navigation = ((Selectable)val2).navigation;
				navigation.mode = (Navigation.Mode)0;
				((Selectable)val2).navigation = navigation;
			}
		}"""
    if oldc in c:
        c = c.replace(oldc, newc)
        print("OK nested click")
    else:
        print("SKIP nested click (pattern)")

    # Replace AddWeaponEvolutionSection
    start = c.find("private unsafe static float AddWeaponEvolutionSection")
    if start < 0:
        start = c.find("private static float AddWeaponEvolutionSection")
    end = c.find("private unsafe static float AddEvolvedFromSection")
    if end < 0:
        end = c.find("private static float AddEvolvedFromSection")
    if start >= 0 and end > start:
        new_evo = r'''private static float AddWeaponEvolutionSection(Transform parent, TMP_FontAsset font, WeaponType weaponType, float yOffset, float maxWidth)
	{
		GameData.EnsureLoaded();
		var rows = GameData.BuildEvoRowsFor(weaponType);
		if (rows == null || rows.Count == 0)
		{
			yOffset = AddEvolvedFromSection(parent, font, weaponType, yOffset, maxWidth);
			return yOffset;
		}
		if (rows.Count >= 2)
		{
			yOffset = AddPassiveEvolutionSection(parent, font, weaponType, yOffset, maxWidth);
			yOffset = AddEvolvedFromSection(parent, font, weaponType, yOffset, maxWidth);
			return yOffset;
		}
		var row = rows[0];
		yOffset -= Spacing;
		GameObject header = CreateTextElement(parent, "EvoHeader", "Evolutions: (click for details)", font, 14f, new Color(0.9f, 0.75f, 0.3f, 1f), (FontStyles)1);
		RectTransform headerRt = header.GetComponent<RectTransform>();
		headerRt.anchorMin = new Vector2(0f, 1f);
		headerRt.anchorMax = new Vector2(1f, 1f);
		headerRt.pivot = new Vector2(0f, 1f);
		headerRt.anchoredPosition = new Vector2(Padding, yOffset);
		headerRt.sizeDelta = new Vector2(maxWidth - Padding * 2f, 20f);
		yOffset -= 22f;
		float iconSize = 38f;
		bool anyMax = false;
		foreach (var p in row.Passives) if (p.RequiresMax) { anyMax = true; break; }
		float rowHeight = iconSize + 8f + (anyMax ? 12f : 0f);
		float x = Padding + 5f;
		GameObject baseIcon = CreateFormulaIcon(parent, "BaseIcon", GameData.GetSprite(weaponType), PlayerOwnsWeapon(weaponType), IsWeaponBanned(weaponType), iconSize, x, yOffset);
		AddHoverToGameObject(baseIcon, weaponType, null, useClick: true);
		x += iconSize + 4f;
		foreach (var passive in row.Passives)
		{
			GameObject plus = CreateTextElement(parent, "Plus", "+", font, 18f, new Color(0.8f, 0.8f, 0.8f, 1f), (FontStyles)1);
			RectTransform plusRt = plus.GetComponent<RectTransform>();
			plusRt.anchorMin = new Vector2(0f, 1f);
			plusRt.anchorMax = new Vector2(0f, 1f);
			plusRt.pivot = new Vector2(0f, 1f);
			plusRt.anchoredPosition = new Vector2(x, yOffset - 8f);
			plusRt.sizeDelta = new Vector2(20f, iconSize);
			x += 22f;
			Sprite ps = passive.Sprite ?? GameData.GetSprite(passive.Type);
			GameObject pIcon = CreateFormulaIcon(parent, "PassiveIcon", ps, PlayerOwnsWeapon(passive.Type), IsWeaponBanned(passive.Type), iconSize, x, yOffset);
			AddHoverToGameObject(pIcon, passive.Type, null, useClick: true);
			if (passive.RequiresMax)
			{
				GameObject maxLbl = CreateTextElement(parent, "Max", "MAX", font, 9f, new Color(1f, 0.85f, 0f, 1f), (FontStyles)1);
				RectTransform maxRt = maxLbl.GetComponent<RectTransform>();
				maxRt.anchorMin = new Vector2(0f, 1f);
				maxRt.anchorMax = new Vector2(0f, 1f);
				maxRt.pivot = new Vector2(0.5f, 1f);
				maxRt.anchoredPosition = new Vector2(x + iconSize / 2f, yOffset - iconSize);
				maxRt.sizeDelta = new Vector2(iconSize, 12f);
			}
			x += iconSize + 4f;
		}
		GameObject arrow = CreateTextElement(parent, "Arrow", "→", font, 18f, new Color(0.8f, 0.8f, 0.8f, 1f), (FontStyles)0);
		RectTransform arrowRt = arrow.GetComponent<RectTransform>();
		arrowRt.anchorMin = new Vector2(0f, 1f);
		arrowRt.anchorMax = new Vector2(0f, 1f);
		arrowRt.pivot = new Vector2(0f, 1f);
		arrowRt.anchoredPosition = new Vector2(x, yOffset - 8f);
		arrowRt.sizeDelta = new Vector2(24f, iconSize);
		x += 26f;
		Sprite evoSprite = row.EvolvedSprite ?? GameData.GetSprite(row.Evolved);
		GameObject evoIcon = CreateFormulaIcon(parent, "EvoIcon", evoSprite, false, IsWeaponBanned(row.Evolved), iconSize, x, yOffset);
		AddHoverToGameObject(evoIcon, row.Evolved, null, useClick: true);
		yOffset -= rowHeight;
		return yOffset;
	}

'''
        c = c[:start] + new_evo + c[end:]
        print("OK AddWeaponEvolutionSection")
    else:
        print(f"SKIP evo section start={start} end={end}")

    SRC.write_text(c, encoding="utf-8")
    print(f"Wrote {SRC} ({len(c)} chars, {c.count(chr(10))+1} lines)")

if __name__ == "__main__":
    main()
