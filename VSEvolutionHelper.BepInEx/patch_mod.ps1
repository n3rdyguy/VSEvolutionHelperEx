$ErrorActionPreference = "Stop"
$f = Join-Path $PSScriptRoot "src\ItemTooltipsMod.cs"
$c = [IO.File]::ReadAllText($f)

$c = $c -replace 'using MelonLoader;\r?\n', ''
$c = $c -replace 'using Il2CppVampireSurvivors\.Data\.Weapons;', 'using VampireSurvivors.Data.Weapons;'
$c = $c -replace 'using Il2CppVampireSurvivors\.Data;', 'using VampireSurvivors.Data;'
$c = $c -replace 'using Il2CppVampireSurvivors\.UI;', 'using VampireSurvivors.UI;'
$c = $c -replace 'using Il2CppTMPro;', 'using TMPro;'
$c = $c -replace 'using Il2CppSystem\.Collections\.Generic;\r?\n', ''
$c = $c -replace 'using Il2CppSystem;\r?\n', ''
$c = $c -replace 'MelonLogger\.Msg\(', 'Plugin.Log.LogInfo('
$c = $c -replace 'MelonLogger\.Warning\(', 'Plugin.Log.LogWarning('
$c = $c -replace 'MelonLogger\.Error\(', 'Plugin.Log.LogError('
$c = $c -replace 'MelonCoroutines\.Start\(DelayedHideCheck\(\)\);', 'DelayFrames(10, () => { if (mouseOverPopupIndex < 0 && popupStack.Count > 0) HideAllPopups(); });'
$c = $c -replace 'MelonCoroutines\.Start\(DelayedStackHideCheck\(thisPopupIndex\)\);', 'DelayFrames(10, () => { int closeFromIndex = mouseOverPopupIndex + 1; if (closeFromIndex < 0) closeFromIndex = 0; while (popupStack.Count > closeFromIndex) HideTopPopup(); });'
$c = $c -replace 'public class ItemTooltipsMod : MelonMod', 'public class ItemTooltipsMod'
$c = $c -replace 'public override void OnInitializeMelon\(\)', 'public static void Initialize()'
$c = $c -replace 'public override void OnSceneWasLoaded\(int buildIndex, string sceneName\)', 'public static void OnSceneLoaded(int buildIndex, string sceneName)'
$c = $c -replace 'public override void OnUpdate\(\)', 'public static void Update()'
$c = $c -replace 'private void ApplyPatches\(\)', 'private static void ApplyPatches()'
$c = $c -replace 'private void TryPatchLevelUpItemUI\(\)', 'private static void TryPatchLevelUpItemUI()'
$c = $c -replace 'private void TryPatchEquipmentIconPause\(\)', 'private static void TryPatchEquipmentIconPause()'
$c = $c -replace 'private void TryPatchMerchantPage\(\)', 'private static void TryPatchMerchantPage()'
$c = $c -replace 'private void TryEarlyCaching\(\)', 'private static void TryEarlyCaching()'
$c = $c -replace 'harmonyInstance = new Harmony\(', 'harmonyInstance = new HarmonyLib.Harmony('
$c = $c -replace 'if \(!assembly2\.FullName\.Contains\("Il2Cpp"\)\)', 'if (!(assembly2.FullName.Contains("VampireSurvivors") || assembly2.FullName.Contains("Il2Cpp") || assembly2.GetName().Name == "Assembly-CSharp"))'
$c = $c -replace '\(\(Vector3\)\(ref (\w+)\)\)\.', '$1.'
$c = $c -replace '\(\(Vector2\)\(ref (\w+)\)\)\.', '$1.'
$c = $c -replace '\(\(ColorBlock\)\(ref (\w+)\)\)\.', '$1.'
$c = $c -replace '\(\(Rect\)\(ref (\w+)\)\)\.', '$1.'
$c = $c -replace 'navigation\.mode = \(Mode\)', 'navigation.mode = (Navigation.Mode)'
$c = $c -replace '\(Mode\)0', '(Navigation.Mode)0'
$c = $c -replace '(?<![\w.])FitMode\b', 'ContentSizeFitter.FitMode'
$c = $c -replace 'ContentSizeFitter\.ContentSizeFitter\.FitMode', 'ContentSizeFitter.FitMode'
$c = $c -replace '(?<![\w.])Entry\b', 'EventTrigger.Entry'
$c = $c -replace 'EventTrigger\.EventTrigger\.Entry', 'EventTrigger.Entry'
$c = $c -replace 'UnityAction\.op_Implicit\(\(Action\)', '(UnityEngine.Events.UnityAction)(System.Action)('
$c = $c -replace 'UnityAction<BaseEventData>\.op_Implicit\(\(Action<BaseEventData>\)', '(UnityEngine.Events.UnityAction<UnityEngine.EventSystems.BaseEventData>)(System.Action<UnityEngine.EventSystems.BaseEventData>)('
$c = $c -replace 'object\?', 'object'
$c = $c -replace 'string text = "Unknown";', 'string text = weaponType.HasValue ? GameData.GetWeaponName(weaponType.Value) : (itemType.HasValue ? itemType.Value.ToString() : "Unknown");'
$c = $c -replace 'if \(weaponType\.HasValue && cachedWeaponsDict != null\)', 'if (weaponType.HasValue)'
$c = $c -replace 'else if \(itemType\.HasValue && cachedPowerUpsDict != null\)', 'else if (itemType.HasValue)'
$c = $c -replace 'Enum\.TryParse<WeaponType>\(([^,]+),\s*out WeaponType', 'GameData.TryParseWeaponType($1, out WeaponType'

if ($c -notmatch 'using Object = UnityEngine.Object') {
  $c = $c -replace 'using UnityEngine;', "using UnityEngine;`r`nusing Object = UnityEngine.Object;"
}

# Inject helpers
$helpers = @'

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
'@
if ($c -notmatch 'ProcessDelayedActions') {
  $c = $c -replace '(private static bool wasGamePaused = false;)', "`$1`r`n$helpers"
}

# Update body prefix
if ($c -notmatch 'if \(!GameData\.IsReady\)') {
  $c = $c -replace 'public static void Update\(\)\r?\n\t\{', "public static void Update()`r`n`t{`r`n`t`tProcessDelayedActions();`r`n`t`tif (!GameData.IsReady) GameData.EnsureLoaded();`r`n`t`ttry { int bi = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex; if (bi != lastSceneBuildIndex) { lastSceneBuildIndex = bi; OnSceneLoaded(bi, UnityEngine.SceneManagement.SceneManager.GetActiveScene().name); } } catch {}`r`n"
}

# Replace critical methods with GameData delegates
function Replace-Method([string]$src, [string]$name, [string]$body) {
  $pattern = "(?s)(private (?:unsafe )?static [^{]+ $name\s*\([^)]*\)\s*\{).*?(\n\t\})"
  # too fragile - use simpler markers
  return $src
}

# Direct replace known method blocks by unique start/end markers
$replacements = @(
  @{
    Start = 'private static WeaponData GetWeaponData(WeaponType type)'
    EndMarker = 'private static object GetPowerUpData'
    New = @"
private static WeaponData GetWeaponData(WeaponType type)
	{
		return GameData.GetWeaponData(type);
	}

	"@
  },
  @{
    Start = 'private static List<WeaponData> GetWeaponDataList(WeaponType type)'
    EndMarker = 'private static void PositionPopup'
    New = @"
private static List<WeaponData> GetWeaponDataList(WeaponType type)
	{
		var il2 = GameData.GetWeaponDataList(type);
		if (il2 == null || il2.Count == 0) return null;
		var list = new List<WeaponData>();
		for (int i = 0; i < il2.Count; i++) if (il2[i] != null) list.Add(il2[i]);
		return list.Count > 0 ? list : null;
	}

	"@
  },
  @{
    Start = 'private static Sprite GetSpriteForWeapon(WeaponType weaponType)'
    EndMarker = 'private static Sprite GetSpriteForItem'
    New = @"
private static Sprite GetSpriteForWeapon(WeaponType weaponType)
	{
		return GameData.GetSprite(weaponType);
	}

	"@
  },
  @{
    Start = 'private static Sprite LoadSpriteFromAtlas(string frameName, string atlasName)'
    EndMarker = 'private static Sprite GetSpriteForWeapon'
    New = @"
private static Sprite LoadSpriteFromAtlas(string frameName, string atlasName)
	{
		return GameData.LoadSprite(frameName, atlasName);
	}

	"@
  },
  @{
    Start = 'private static string GetI2Translation(string term)'
    EndMarker = 'private static TMP_FontAsset GetFont'
    New = @"
private static string GetI2Translation(string term)
	{
		return GameData.Translate(term);
	}

	"@
  },
  @{
    Start = 'private unsafe static string GetLocalizedWeaponName(WeaponData data, WeaponType type)'
    EndMarker = 'private static string GetLocalizedPowerUpDescription'
    New = @"
private static string GetLocalizedWeaponName(WeaponData data, WeaponType type)
	{
		return GameData.GetWeaponName(type);
	}

	"@
  },
  @{
    Start = 'private static string GetLocalizedWeaponDescription(WeaponData data, WeaponType type)'
    EndMarker = 'private unsafe static string GetLocalizedWeaponName'
    New = @"
private static string GetLocalizedWeaponDescription(WeaponData data, WeaponType type)
	{
		return GameData.GetWeaponDescription(type);
	}

	"@
  }
)

foreach ($r in $replacements) {
  $si = $c.IndexOf($r.Start)
  $ei = $c.IndexOf($r.EndMarker)
  if ($si -ge 0 -and $ei -gt $si) {
    $c = $c.Substring(0, $si) + $r.New + $c.Substring($ei)
    Write-Host "Replaced $($r.Start.Substring(0, [Math]::Min(50,$r.Start.Length)))..."
  } else {
    Write-Host "SKIP $($r.Start.Substring(0, [Math]::Min(40,$r.Start.Length))) si=$si ei=$ei"
  }
}

# CacheDataManager early return for typed DataManager
$marker = 'public static void CacheDataManager(object dataManager)'
$si = $c.IndexOf($marker)
if ($si -ge 0) {
  $insert = @'
public static void CacheDataManager(object dataManager)
	{
		if (dataManager == null) return;
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
'@
  # find original body start after signature {
  $brace = $c.IndexOf('{', $si)
  # find matching close is hard; instead replace signature + null check
  $old = 'public static void CacheDataManager(object dataManager)
	{
		if (dataManager == null)
		{
			return;
		}
		cachedDataManager = dataManager;'
  if ($c.Contains($old)) {
    $c = $c.Replace($old, $insert + "`r`n`t`tcachedDataManager = dataManager;")
    Write-Host "Patched CacheDataManager"
  } else {
    Write-Host "CacheDataManager pattern not exact"
  }
}

# BuildLookupTables early GameData
$oldBuild = 'private static void BuildLookupTables()
	{
		if (lookupTablesBuilt)
		{
			return;
		}
		spriteToWeaponType = new Dictionary<string, WeaponType>();
		spriteToItemType = new Dictionary<string, ItemType>();
		try
		{'
$newBuild = 'private static void BuildLookupTables()
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
		{'
if ($c.Contains($oldBuild)) {
  $c = $c.Replace($oldBuild, $newBuild)
  Write-Host "Patched BuildLookupTables"
}

# Typed EquipmentIconPaused patch in ApplyPatches
$oldApply = 'TryPatchMerchantPage();
			TryPatchLevelUpItemUI();
			TryPatchEquipmentIconPause();
			Plugin.Log.LogInfo("Patches applied successfully");'
$newApply = 'TryPatchMerchantPage();
			TryPatchLevelUpItemUI();
			TryPatchEquipmentIconPause();
			try {
				var equipMethod = typeof(VampireSurvivors.UI.EquipmentIconPaused).GetMethod("SetData", BindingFlags.Instance | BindingFlags.Public);
				if (equipMethod != null)
					harmonyInstance.Patch(equipMethod, null, new HarmonyMethod(typeof(EquipmentIconPatches), nameof(EquipmentIconPatches.SetData_Weapon_Postfix)));
			} catch (Exception ex) { Plugin.Log.LogWarning("EquipmentIconPaused patch: " + ex.Message); }
			Plugin.Log.LogInfo("Patches applied successfully");'
if ($c.Contains($oldApply)) {
  $c = $c.Replace($oldApply, $newApply)
  Write-Host "Patched ApplyPatches"
}

# Fix nested click: add PointerClick EventTrigger in addition to Button for useClick path
$oldClick = 'if (useClick)
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
		}'
$newClick = 'if (useClick)
		{
			formulaIconData[((Object)go).GetInstanceID()] = (weaponType, itemType);
			// Nested tooltips: PointerClick is reliable under Il2Cpp; Button alone often fails
			EventTrigger.Entry click = new EventTrigger.Entry();
			click.eventID = EventTriggerType.PointerClick;
			((UnityEvent<BaseEventData>)(object)click.callback).AddListener((UnityEngine.Events.UnityAction<UnityEngine.EventSystems.BaseEventData>)(System.Action<UnityEngine.EventSystems.BaseEventData>)(delegate
			{
				ShowItemPopup(go.transform, weaponType, itemType);
			}));
			val.triggers.Add(click);
			// Hover highlight via transparent Button
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
		}'
if ($c.Contains('formulaIconData[((Object)go).GetInstanceID()] = (weaponType, itemType);')) {
  # try softer match
  if ($c.Contains($oldClick)) {
    $c = $c.Replace($oldClick, $newClick)
    Write-Host "Patched nested click"
  } else {
    Write-Host "Nested click pattern mismatch - will patch after build if needed"
  }
}

# Replace AddWeaponEvolutionSection: find by start string and end at AddEvolvedFromSection
$start = $c.IndexOf('private unsafe static float AddWeaponEvolutionSection')
if ($start -lt 0) { $start = $c.IndexOf('private static float AddWeaponEvolutionSection') }
$end = $c.IndexOf('private unsafe static float AddEvolvedFromSection')
if ($end -lt 0) { $end = $c.IndexOf('private static float AddEvolvedFromSection') }
if ($start -ge 0 -and $end -gt $start) {
  $newEvo = @'
private static float AddWeaponEvolutionSection(Transform parent, TMP_FontAsset font, WeaponType weaponType, float yOffset, float maxWidth)
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
		// Base weapon
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

'@
  $c = $c.Substring(0, $start) + $newEvo + $c.Substring($end)
  Write-Host "Replaced AddWeaponEvolutionSection"
}

[IO.File]::WriteAllText($f, $c)
Write-Host "Done. Size=$((Get-Item $f).Length) Lines=$((Get-Content $f).Count)"
