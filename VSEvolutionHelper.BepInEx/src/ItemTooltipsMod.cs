using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using TMPro;
using VampireSurvivors.Data;
using VampireSurvivors.Data.Weapons;
using VampireSurvivors.UI;
using UnityEngine;
using Object = UnityEngine.Object;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

namespace VSItemTooltips;

public class ItemTooltipsMod
{
	private class TrackedIcon
	{
		public Image Image;

		public WeaponType? WeaponType;

		public ItemType? ItemType;

		public string SpriteName;

		public int InstanceId;

		public EventTrigger EventTrigger;
	}

	private struct PassiveRequirement
	{
		public WeaponType? WeaponType;

		public ItemType? ItemType;

		public Sprite Sprite;

		public bool Owned;

		public bool RequiresMaxLevel;
	}

	private struct EvolutionFormula
	{
		public WeaponType BaseWeapon;

		public List<PassiveRequirement> Passives;

		public WeaponType EvolvedWeapon;

		public string BaseName;

		public string EvolvedName;

		public Sprite BaseSprite;

		public Sprite EvolvedSprite;
	}

	private struct ArcanaInfo
	{
		public string Name;

		public string Description;

		public Sprite Sprite;

		public ArcanaType Type;

		/// <summary>Typed game ArcanaData (preferred). Legacy object kept for reflection fallbacks.</summary>
		public object ArcanaData;
	}

	private static Harmony harmonyInstance;

	private static bool wasGamePaused = false;
	private static int lastSceneBuildIndex = -1;
	private static readonly List<(int framesLeft, Action action)> pendingDelayedActions = new List<(int, Action)>();
	internal static void DelayFrames(int frames, Action action) { pendingDelayedActions.Add((frames, action)); }
	
	private static Type FindTypeByName(string typeName, string namespaceContains = null)
	{
		foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
		{
			Type[] types; try { types = a.GetTypes(); } catch { continue; }
			foreach (Type t in types)
			{
				if (t.Name != typeName) continue;
				if (namespaceContains != null && (t.Namespace == null || t.Namespace.IndexOf(namespaceContains) < 0)) continue;
				return t;
			}
		}
		return null;
	}
	private static void ProcessDelayedActions() {
		for (int i = pendingDelayedActions.Count - 1; i >= 0; i--) {
			var item = pendingDelayedActions[i];
			int left = item.framesLeft - 1;
			if (left <= 0) { pendingDelayedActions.RemoveAt(i); try { item.action(); } catch (Exception ex) { Plugin.Log.LogWarning("Delayed: " + ex.Message); } }
			else pendingDelayedActions[i] = (left, item.action);
		}
	}


	private static List<GameObject> popupStack = new List<GameObject>();

	private static List<int> popupAnchorIds = new List<int>();

	private static int mouseOverPopupIndex = -1;

	private static Dictionary<int, TrackedIcon> trackedIcons = new Dictionary<int, TrackedIcon>();

	private static float lastScanTime = 0f;

	private static float scanInterval = 0.1f;

	private static Dictionary<int, WeaponType> uiToWeaponType = new Dictionary<int, WeaponType>();

	private static Dictionary<int, ItemType> uiToItemType = new Dictionary<int, ItemType>();

	private static Dictionary<string, WeaponType> spriteToWeaponType = null;

	private static Dictionary<string, ItemType> spriteToItemType = null;

	private static bool lookupTablesBuilt = false;

	private static bool loggedLookupTables = false;

	private static object cachedDataManager = null;

	private static object cachedWeaponsDict = null;

	private static object cachedPowerUpsDict = null;

	private static object cachedGameSession = null;

	private static Type cachedArcanaTypeEnum = null;

	private static object cachedAllArcanas = null;

	private static object cachedGameManager = null;

	private static bool arcanaDebugLogged = false;

	private static HashSet<WeaponType> arcanaWeaponDebugLogged = new HashSet<WeaponType>();

	private static Dictionary<int, (HashSet<WeaponType> weapons, HashSet<ItemType> items)> arcanaUICache = new Dictionary<int, (HashSet<WeaponType>, HashSet<ItemType>)>();

	private static Dictionary<string, int> arcanaNameToInt = new Dictionary<string, int>();

	private static Dictionary<int, (GameObject go, WeaponType? weapon, ItemType? item, object arcanaType)> collectionIcons = new Dictionary<int, (GameObject, WeaponType?, ItemType?, object)>();

	/// <summary>Pause-map icons (relics, pickups, chests, tokens).</summary>
	private struct MapIconInfo
	{
		public GameObject Go;
		public WeaponType? Weapon;
		public ItemType? Item;
		public string Label;
		public string Description;
		public Sprite Sprite;
	}
	private static Dictionary<int, MapIconInfo> mapIcons = new Dictionary<int, MapIconInfo>();
	private static int currentMapHoverId = -1;
	private static int pendingMapHoverId = -1;
	private static float mapHoverStartTime = 0f;
	private static GameObject mapPopup = null;
	public static int MapIconCount => mapIcons.Count;

	/// <summary>Stage Selection "Relics in stage" icons (menu, not in-run map).</summary>
	private static Dictionary<int, MapIconInfo> stageRelicIcons = new Dictionary<int, MapIconInfo>();
	private static int currentStageRelicHoverId = -1;
	private static int pendingStageRelicHoverId = -1;
	private static float stageRelicHoverStartTime = 0f;
	private static GameObject stageRelicPopup = null;

	/// <summary>Character Selection card hover tooltips.</summary>
	private static Dictionary<int, MapIconInfo> characterIcons = new Dictionary<int, MapIconInfo>();
	private static int currentCharacterHoverId = -1;
	private static int pendingCharacterHoverId = -1;
	private static float characterHoverStartTime = 0f;
	private static GameObject characterPopup = null;
	public static int CharacterIconCount => characterIcons.Count;

	/// <summary>Adventures select card hover tooltips.</summary>
	private static Dictionary<int, MapIconInfo> adventureIcons = new Dictionary<int, MapIconInfo>();
	private static int currentAdventureHoverId = -1;
	private static int pendingAdventureHoverId = -1;
	private static float adventureHoverStartTime = 0f;
	private static GameObject adventurePopup = null;
	public static int AdventureIconCount => adventureIcons.Count;

	private static int currentCollectionHoverId = -1;

	private static GameObject collectionPopup = null;

	private static bool usingController = false;

	private static Vector3 lastMousePosition = Vector3.zero;

	private static GameObject lastSelectedObject = null;

	private static float dwellStartTime = 0f;

	private static float DwellDelay => Plugin.ControllerDwellDelay;

	private static GameObject dwellTarget = null;

	private static bool passivePopupShown = false;

	/// <summary>After Level Up opens, block auto tooltips until real hover or grace ends.</summary>
	private static float suppressUnsolicitedPopupUntil = 0f;
	/// <summary>User must move the mouse after Level Up opens before any hover tooltip (cards spawn under cursor).</summary>
	private static bool levelUpHoverUnlocked = false;
	/// <summary>Pending delayed hover on Level Up (avoid instant PointerEnter when cards appear under the mouse).</summary>
	private static Transform levelUpPendingAnchor = null;
	private static WeaponType? levelUpPendingWeapon = null;
	private static ItemType? levelUpPendingItem = null;
	private static float levelUpPendingSince = -1f;
	// Short settle delay — main guard is "mouse must move after Level Up opens" (config: Tooltips.LevelUpHoverDelay)
	private static float LevelUpHoverDelay => Plugin.LevelUpHoverDelay;

	private static bool interactiveMode = false;

	private static List<GameObject> formulaIcons = new List<GameObject>();

	private static int currentFormulaIndex = -1;

	private static GameObject interactiveHighlight = null;

	private static GameObject preDwellSelection = null;

	private static Dictionary<int, (WeaponType? weapon, ItemType? item)> formulaIconData = new Dictionary<int, (WeaponType?, ItemType?)>();

	private static GameObject interactivePopup = null;

	private static GameObject cachedNavigatorArrows = null;

	private static List<(WeaponType? weapon, ItemType? item, object arcana)> collectionPopupBackStack = new List<(WeaponType?, ItemType?, object)>();

	private static (WeaponType? weapon, ItemType? item, object arcana) currentCollectionPopupData;

	private static bool equipmentNavMode = false;

	private static List<GameObject> equipmentIcons = new List<GameObject>();

	private static int currentEquipmentIndex = -1;

	private static GameObject equipmentHighlight = null;

	private static readonly Color PopupBgColor = new Color(0.525f, 0.525f, 0.525f, 0.98f);

	private static readonly Color PopupBorderColor = new Color(0.8f, 0.604f, 0.298f, 1f);

	private static readonly float IconSize = 48f;

	private static readonly float SmallIconSize = 40f;

	private static readonly float Padding = 12f;

	private static readonly float Spacing = 8f;

	private static float lastTimeScaleLog = 0f;

	private static bool escWasPressed = false;

	private static bool triedEarlyCaching = false;

	private static GameObject levelUpView = null;

	private static GameObject merchantView = null;

	private static GameObject pauseView = null;

	private static GameObject itemFoundView = null;

	private static GameObject arcanaView = null;

	private static GameObject weaponSelectionView = null;

	private static GameObject hudInventory = null;

	private static bool hudSearched = false;

	private static bool inGameUIFound = false;

	private static List<Transform> activeUIContainers = new List<Transform>();

	private static Transform cachedSafeArea = null;

	private static bool triedFindingPauseView = false;

	private static bool loggedScanStatus = false;

	private static bool loggedScanResults = false;

	private static bool scannedPauseView = false;

	private static bool scannedWeaponSelection = false;

	private static Type cachedWeaponSelectionItemType = null;

	private static bool triedFindingWSIType = false;

	private static object cachedLevelUpFactory = null;

	private static float collectionHoverStartTime = 0f;

	private static float CollectionHoverDelay => Plugin.TooltipHoverDelay;

	private static int pendingCollectionHoverId = -1;

	private static WeaponType? pendingCollectionWeapon = null;

	private static ItemType? pendingCollectionItem = null;

	private static object pendingCollectionArcana = null;

	private static Type spriteManagerType = null;

	private static bool spriteManagerDebugLogged = false;

	private static Sprite cachedCircleSprite = null;

	private static bool spriteLoadDebugLogged = false;

	private static HashSet<WeaponType> panelCapturedWeapons = new HashSet<WeaponType>();

	private static HashSet<ItemType> panelCapturedItems = new HashSet<ItemType>();

	private static GameObject currentPopup => (popupStack.Count > 0) ? popupStack[popupStack.Count - 1] : null;

	private static int currentPopupAnchorId => (popupAnchorIds.Count > 0) ? popupAnchorIds[popupAnchorIds.Count - 1] : 0;

	public static void Initialize()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected O, but got Unknown
		harmonyInstance = new HarmonyLib.Harmony("com.nihil.vsitemtooltips");
		ApplyPatches();
		Plugin.Log.LogInfo("VS Item Tooltips initialized!");
	}

	private static void ApplyPatches()
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		try
		{
			Type typeFromHandle = typeof(LevelUpPage);
			MethodInfo method = typeFromHandle.GetMethod("OnShowStart", BindingFlags.Instance | BindingFlags.Public);
			if (method != null)
			{
				harmonyInstance.Patch((MethodBase)method, (HarmonyMethod)null, new HarmonyMethod(typeof(LevelUpPagePatches), "Show_Postfix", (Type[])null), (HarmonyMethod)null, (HarmonyMethod)null, (HarmonyMethod)null);
			}
			else
			{
				Plugin.Log.LogWarning("LevelUpPage.OnShowStart method not found!");
			}
			TryPatchMerchantPage();
			TryPatchLevelUpItemUI();
			TryPatchEquipmentIconPause();
			try {
				var equipMethod = typeof(VampireSurvivors.UI.EquipmentIconPaused).GetMethod("SetData", BindingFlags.Instance | BindingFlags.Public);
				if (equipMethod != null)
					harmonyInstance.Patch(equipMethod, null, new HarmonyMethod(typeof(EquipmentIconPatches), nameof(EquipmentIconPatches.SetData_Weapon_Postfix)));
			} catch (Exception ex) { Plugin.Log.LogWarning("EquipmentIconPaused patch: " + ex.Message); }
			try {
				GrimoirePatches.Apply(harmonyInstance);
			} catch (Exception ex) { Plugin.Log.LogWarning("Grimoire patches: " + ex.Message); }
			try {
				MapPatches.Apply(harmonyInstance);
			} catch (Exception ex) { Plugin.Log.LogWarning("Map patches: " + ex.Message); }
			try {
				StageSelectPatches.Apply(harmonyInstance);
			} catch (Exception ex) { Plugin.Log.LogWarning("StageSelect patches: " + ex.Message); }
			try {
				CharacterSelectPatches.Apply(harmonyInstance);
			} catch (Exception ex) { Plugin.Log.LogWarning("CharacterSelect patches: " + ex.Message); }
			try {
				AdventureSelectPatches.Apply(harmonyInstance);
			} catch (Exception ex) { Plugin.Log.LogWarning("AdventureSelect patches: " + ex.Message); }
			Plugin.Log.LogInfo("Patches applied successfully");
		}
		catch (Exception arg)
		{
			Plugin.Log.LogError($"Failed to apply patches: {arg}");
		}
	}

	private static void TryPatchLevelUpItemUI()
	{
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		try
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			Assembly[] array = assemblies;
			foreach (Assembly assembly in array)
			{
				try
				{
					Type type = assembly.GetTypes().FirstOrDefault((Type t) => t.Name == "LevelUpItemUI");
					if (type != null)
					{
						MethodInfo method = type.GetMethod("SetWeaponData", BindingFlags.Instance | BindingFlags.Public);
						if (method != null)
						{
							harmonyInstance.Patch((MethodBase)method, (HarmonyMethod)null, new HarmonyMethod(typeof(LevelUpItemUIPatches), "SetWeaponData_Postfix", (Type[])null), (HarmonyMethod)null, (HarmonyMethod)null, (HarmonyMethod)null);
						}
						else
						{
							Plugin.Log.LogWarning("SetWeaponData method not found on LevelUpItemUI");
						}
						MethodInfo method2 = type.GetMethod("SetItemData", BindingFlags.Instance | BindingFlags.Public);
						if (method2 != null)
						{
							harmonyInstance.Patch((MethodBase)method2, (HarmonyMethod)null, new HarmonyMethod(typeof(LevelUpItemUIPatches), "SetItemData_Postfix", (Type[])null), (HarmonyMethod)null, (HarmonyMethod)null, (HarmonyMethod)null);
						}
						else
						{
							Plugin.Log.LogWarning("SetItemData method not found on LevelUpItemUI");
						}
						return;
					}
				}
				catch
				{
				}
			}
			Plugin.Log.LogWarning("LevelUpItemUI type not found in any assembly");
		}
		catch (Exception arg)
		{
			Plugin.Log.LogError($"Error patching LevelUpItemUI: {arg}");
		}
	}

	private static void TryPatchEquipmentIconPause()
	{
		//IL_0396: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Expected O, but got Unknown
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Expected O, but got Unknown
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Expected O, but got Unknown
		try
		{
			Type arcanaTypeEnum = null;
			try
			{
				Assembly assembly = typeof(WeaponData).Assembly;
				arcanaTypeEnum = assembly.GetTypes().FirstOrDefault((Type t) => t.Name == "ArcanaType");
				if (arcanaTypeEnum != null)
				{
					cachedArcanaTypeEnum = arcanaTypeEnum;
				}
			}
			catch
			{
			}
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			Assembly[] array = assemblies;
			foreach (Assembly assembly2 in array)
			{
				if (!(assembly2.FullName.Contains("VampireSurvivors") || assembly2.FullName.Contains("Il2Cpp") || assembly2.GetName().Name == "Assembly-CSharp"))
				{
					continue;
				}
				try
				{
					IEnumerable<Type> enumerable = (from t in assembly2.GetTypes()
						where t.Name.ToLower().Contains("icon") || t.Name.ToLower().Contains("equipment") || t.Name.ToLower().Contains("itemui") || t.Name.ToLower().Contains("arcana") || t.Name.ToLower().Contains("evolution") || t.Name.ToLower().Contains("weaponui") || t.Name.ToLower().Contains("powerup")
						select t).Take(30);
					foreach (Type item in enumerable)
					{
						IEnumerable<MethodInfo> enumerable2 = (from m in item.GetMethods(BindingFlags.Instance | BindingFlags.Public)
							where m.GetParameters().Any((ParameterInfo p) => p.ParameterType == typeof(WeaponType) || p.ParameterType == typeof(ItemType) || (arcanaTypeEnum != null && p.ParameterType == arcanaTypeEnum) || p.ParameterType.Name.Contains("Weapon") || p.ParameterType.Name.Contains("Item"))
							select m).Take(5);
						foreach (MethodInfo item2 in enumerable2)
						{
							if (!item2.Name.Contains("Set") && !item2.Name.Contains("Init") && !item2.Name.Contains("Setup") && !item2.Name.Contains("Add") && !item2.Name.Contains("Spawn") && !item2.Name.Contains("Create"))
							{
								continue;
							}
							try
							{
								ParameterInfo[] parameters = item2.GetParameters();
								int num2 = -1;
								int num3 = -1;
								int num4 = -1;
								for (int num5 = 0; num5 < parameters.Length; num5++)
								{
									if (parameters[num5].ParameterType == typeof(WeaponType))
									{
										num2 = num5;
									}
									else if (parameters[num5].ParameterType == typeof(ItemType))
									{
										num3 = num5;
									}
									else if (arcanaTypeEnum != null && parameters[num5].ParameterType == arcanaTypeEnum)
									{
										num4 = num5;
									}
								}
								if (num2 >= 0)
								{
									Harmony val = harmonyInstance;
									MethodBase methodBase = item2;
									Type typeFromHandle = typeof(GenericIconPatches);
									if (1 == 0)
									{
									}
									string text = num2 switch
									{
										0 => "SetWeapon_Postfix", 
										1 => "SetWeapon_Postfix_Arg1", 
										_ => "SetWeapon_Postfix_ArgN", 
									};
									if (1 == 0)
									{
									}
									val.Patch(methodBase, (HarmonyMethod)null, new HarmonyMethod(typeFromHandle, text, (Type[])null), (HarmonyMethod)null, (HarmonyMethod)null, (HarmonyMethod)null);
								}
								else if (num3 >= 0)
								{
									Harmony val2 = harmonyInstance;
									MethodBase methodBase2 = item2;
									Type typeFromHandle2 = typeof(GenericIconPatches);
									if (1 == 0)
									{
									}
									string text = num3 switch
									{
										0 => "SetItem_Postfix", 
										1 => "SetItem_Postfix_Arg1", 
										_ => "SetItem_Postfix_ArgN", 
									};
									if (1 == 0)
									{
									}
									val2.Patch(methodBase2, (HarmonyMethod)null, new HarmonyMethod(typeFromHandle2, text, (Type[])null), (HarmonyMethod)null, (HarmonyMethod)null, (HarmonyMethod)null);
								}
								else if (num4 >= 0)
								{
									harmonyInstance.Patch((MethodBase)item2, (HarmonyMethod)null, new HarmonyMethod(typeof(GenericIconPatches), "SetArcana_Postfix_ArgN", (Type[])null), (HarmonyMethod)null, (HarmonyMethod)null, (HarmonyMethod)null);
								}
							}
							catch (Exception ex)
							{
								Plugin.Log.LogWarning("  Failed to patch: " + ex.Message);
							}
						}
					}
				}
				catch
				{
				}
			}
		}
		catch (Exception ex2)
		{
			Plugin.Log.LogWarning("Error searching for icon types: " + ex2.Message);
		}
	}

	private static void TryPatchMerchantPage()
	{
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		try
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			Assembly[] array = assemblies;
			foreach (Assembly assembly in array)
			{
				try
				{
					Type type = assembly.GetTypes().FirstOrDefault((Type t) => t.Name.Contains("Merchant") && t.Name.Contains("Page"));
					if (type != null)
					{
						MethodInfo method = type.GetMethod("Show", BindingFlags.Instance | BindingFlags.Public);
						if (method != null)
						{
							harmonyInstance.Patch((MethodBase)method, (HarmonyMethod)null, new HarmonyMethod(typeof(GenericPagePatches), "Show_Postfix", (Type[])null), (HarmonyMethod)null, (HarmonyMethod)null, (HarmonyMethod)null);
							break;
						}
					}
				}
				catch
				{
				}
			}
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("Could not patch MerchantPage: " + ex.Message);
		}
	}

	public static void OnSceneLoaded(int buildIndex, string sceneName)
	{
		triedEarlyCaching = false;
		cachedGameSession = null;
		cachedGameManager = null;
		cachedAllArcanas = null;
		cachedSafeArea = null;
		panelCapturedWeapons.Clear();
		panelCapturedItems.Clear();
		arcanaDebugLogged = false;
		arcanaWeaponDebugLogged.Clear();
	}

	private static void TryEarlyCaching()
	{
		if (triedEarlyCaching || (cachedDataManager != null && cachedWeaponsDict != null))
		{
			return;
		}
		triedEarlyCaching = true;
		try
		{
			try
			{
				Type type = FindTypeByName("DataManager", "VampireSurvivors");
				if (type != null)
				{
					MethodInfo method = typeof(Object).GetMethod("FindObjectOfType", new Type[0]);
					if (method != null)
					{
						MethodInfo methodInfo = method.MakeGenericMethod(type);
						object obj = methodInfo.Invoke(null, null);
						if (obj != null)
						{
							CacheDataManager(obj);
							return;
						}
					}
				}
			}
			catch
			{
			}
			Type type2 = FindTypeByName("GameManager");
			if (type2 != null)
			{
				MemberInfo[] members = type2.GetMembers(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				object obj3 = null;
				MemberInfo[] array = members;
				MemberInfo[] array2 = array;
				foreach (MemberInfo memberInfo in array2)
				{
					try
					{
						if (memberInfo is PropertyInfo propertyInfo && propertyInfo.PropertyType == type2)
						{
							obj3 = propertyInfo.GetValue(null);
							if (obj3 != null)
							{
								break;
							}
						}
						else if (memberInfo is FieldInfo fieldInfo && fieldInfo.FieldType == type2)
						{
							obj3 = fieldInfo.GetValue(null);
							if (obj3 != null)
							{
								break;
							}
						}
					}
					catch
					{
					}
				}
				if (obj3 != null)
				{
					PropertyInfo[] properties = obj3.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					PropertyInfo[] array3 = properties;
					PropertyInfo[] array4 = array3;
					foreach (PropertyInfo propertyInfo2 in array4)
					{
						if (!(propertyInfo2.Name == "Data") && !propertyInfo2.PropertyType.Name.Contains("DataManager"))
						{
							continue;
						}
						try
						{
							object value = propertyInfo2.GetValue(obj3);
							if (value != null)
							{
								CacheDataManager(value);
								return;
							}
						}
						catch
						{
						}
					}
				}
			}
			Il2CppArrayBase<MonoBehaviour> source = Object.FindObjectsOfType<MonoBehaviour>();
			foreach (MonoBehaviour item in ((IEnumerable<MonoBehaviour>)source).Take(100))
			{
				MethodInfo method2 = ((object)item).GetType().GetMethod("GetConvertedWeapons");
				if (method2 != null)
				{
					CacheDataManager(item);
					break;
				}
			}
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("Early caching failed: " + ex.Message);
		}
	}

	public static void Update()
	{
		ProcessDelayedActions();
		if (!GameData.IsReady) GameData.EnsureLoaded();
		try { int bi = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex; if (bi != lastSceneBuildIndex) { lastSceneBuildIndex = bi; OnSceneLoaded(bi, UnityEngine.SceneManagement.SceneManager.GetActiveScene().name); } } catch {}
		if (!triedEarlyCaching && cachedDataManager == null)
		{
			TryEarlyCaching();
		}
		DetectInputMode();
		if (Input.GetKeyDown((KeyCode)27))
		{
			escWasPressed = true;
			GameObject val = GameObject.Find("GAME UI/Canvas - Game UI/Safe Area");
			if ((Object)(object)val != (Object)null)
			{
				for (int i = 0; i < val.transform.childCount; i++)
				{
					Transform child = val.transform.GetChild(i);
					if (((Component)child).gameObject.activeInHierarchy && (((Object)child).name.ToLower().Contains("map") || ((Object)child).name.ToLower().Contains("pause")))
					{
						pauseView = ((Component)child).gameObject;
					}
				}
			}
		}
		bool flag = IsGamePaused();
		if (flag && !wasGamePaused)
		{
			// Level Up / pause / merchant just opened — never keep a stale or auto tooltip
			HideAllPopups();
			ResetHoverDwellState();
			collectionIcons.Clear();
			HideCollectionPopup();
			ClearTrackedIcons();
			if ((Object)(object)pauseView != (Object)null && pauseView.activeInHierarchy)
			{
				ScanPauseViewForEquipment(pauseView);
			}
			if ((Object)(object)hudInventory == (Object)null && inGameUIFound)
			{
				hudSearched = false;
			}
			if (cachedGameSession == null)
			{
				TryFindGameSession();
			}
			// Do not attach HUD hovers while Level Up is the active overlay (avoids stray tooltips)
			bool levelUpOpen = (Object)(object)levelUpView != (Object)null && levelUpView.activeInHierarchy;
			if (!levelUpOpen && (Object)(object)hudInventory != (Object)null && cachedGameSession != null)
			{
				SetupHUDHovers();
			}
		}
		if (!flag && wasGamePaused)
		{
			HidePopup();
			ClearTrackedIcons();
			ResetControllerState();
			loggedScanStatus = false;
			loggedScanResults = false;
			scannedPauseView = false;
			scannedWeaponSelection = false;
		}
		if (!flag && (Object)(object)levelUpView == (Object)null && (Object)(object)merchantView == (Object)null && (Object)(object)pauseView == (Object)null && (Object)(object)itemFoundView == (Object)null && inGameUIFound)
		{
			inGameUIFound = false;
			hudSearched = false;
			hudInventory = null;
			cachedSafeArea = null;
		}
		wasGamePaused = flag;
		// Map tooltips while paused (map is a pause-UI overlay)
		if (Plugin.MapTooltipsEnabled && flag && mapIcons.Count > 0)
		{
			UpdateMapHover();
		}
		else if (!flag && mapIcons.Count > 0)
		{
			// Map closed with unpause
			HideMapPopup();
		}
		else if (!Plugin.MapTooltipsEnabled && mapIcons.Count > 0)
		{
			HideMapPopup();
		}
		// Stage Selection: Guide tabs (LB/RB, focus) + relic hover
		StageGuideUI.TickInput();
		if (stageRelicIcons.Count > 0)
		{
			if (usingController)
				UpdateStageRelicControllerDwell();
			else
				UpdateStageRelicHover();
		}
		// Character Selection: scan (if needed) + starter weapon / evo tooltips
		if (Plugin.CharacterTooltipsEnabled)
		{
			CharacterSelectPatches.Tick();
			if (characterIcons.Count > 0)
			{
				if (usingController)
					UpdateCharacterControllerDwell();
				else
					UpdateCharacterHover();
			}
		}
		else if (characterIcons.Count > 0)
		{
			ClearCharacterIcons();
		}
		// Adventures select tooltips
		if (Plugin.AdventureTooltipsEnabled)
		{
			AdventureSelectPatches.Tick();
			if (adventureIcons.Count > 0)
				UpdateAdventureHover();
		}
		else if (adventureIcons.Count > 0)
		{
			ClearAdventureIcons();
		}
		if (!flag && collectionIcons.Count > 0)
		{
			if (usingController)
			{
				UpdateControllerCollectionDwell();
			}
			else
			{
				UpdateCollectionHover();
			}
		}
		if (usingController)
		{
			HandleBackButton();
			UpdateInteractiveMode();
			if (equipmentNavMode)
			{
				UpdateEquipmentNavMode();
			}
			if (IsInteractButtonPressed())
			{
				if (interactiveMode)
				{
					HandleBackButton(force: true);
				}
				else if (!equipmentNavMode && passivePopupShown)
				{
					EnterInteractiveMode();
				}
				else if (!equipmentNavMode && !passivePopupShown && flag && (Object)(object)pauseView != (Object)null && pauseView.activeInHierarchy)
				{
					EnterEquipmentNavMode();
				}
			}
			if (IsSubmitButtonPressed() && equipmentNavMode && passivePopupShown && !interactiveMode)
			{
				EnterInteractiveMode();
			}
		}
		if (!flag)
		{
			return;
		}
		if (usingController && !equipmentNavMode && !IsLevelUpViewActive())
		{
			// Never dwell-show tooltips on Level Up (auto-select first card)
			UpdateControllerDwell();
		}
		UpdateLevelUpPendingHover();
		float unscaledTime = Time.unscaledTime;
		if (unscaledTime - lastScanTime >= scanInterval)
		{
			lastScanTime = unscaledTime;
			ScanForIcons();
			if (!scannedWeaponSelection && (Object)(object)weaponSelectionView != (Object)null && weaponSelectionView.activeInHierarchy)
			{
				ScanWeaponSelectionView(weaponSelectionView);
			}
		}
	}

	private static bool IsGamePaused()
	{
		activeUIContainers.Clear();
		if ((Object)(object)levelUpView == (Object)null)
		{
			levelUpView = GameObject.Find("GAME UI/Canvas - Game UI/Safe Area/View - Level Up");
		}
		if ((Object)(object)merchantView == (Object)null)
		{
			merchantView = GameObject.Find("GAME UI/Canvas - Game UI/Safe Area/View - Merchant");
		}
		if ((Object)(object)itemFoundView == (Object)null)
		{
			itemFoundView = GameObject.Find("GAME UI/Canvas - Game UI/Safe Area/View - ItemFound");
		}
		if ((Object)(object)arcanaView == (Object)null)
		{
			arcanaView = GameObject.Find("GAME UI/Canvas - Game UI/Safe Area/View - ArcanaMainSelection");
		}
		if ((Object)(object)weaponSelectionView == (Object)null)
		{
			weaponSelectionView = GameObject.Find("GAME UI/Canvas - Game UI/Safe Area/View - WeaponSelection");
		}
		if ((Object)(object)pauseView == (Object)null)
		{
			string[] array = new string[3] { "GAME UI/Canvas - Game UI/Safe Area/View - Paused", "GAME UI/Canvas - Game UI/Safe Area/View - Pause", "GAME UI/Canvas - Game UI/Safe Area/View - Map" };
			string[] array2 = array;
			string[] array3 = array2;
			foreach (string text in array3)
			{
				pauseView = GameObject.Find(text);
				if ((Object)(object)pauseView != (Object)null)
				{
					break;
				}
			}
			if ((Object)(object)pauseView == (Object)null && !triedFindingPauseView)
			{
				triedFindingPauseView = true;
				GameObject val = GameObject.Find("GAME UI/Canvas - Game UI/Safe Area");
				if ((Object)(object)val != (Object)null)
				{
					for (int j = 0; j < val.transform.childCount; j++)
					{
						Transform child = val.transform.GetChild(j);
						if (((Object)child).name.ToLower().Contains("pause") || ((Object)child).name.ToLower().Contains("map"))
						{
							pauseView = ((Component)child).gameObject;
						}
					}
				}
			}
		}
		bool flag = (Object)(object)levelUpView != (Object)null || (Object)(object)merchantView != (Object)null || (Object)(object)pauseView != (Object)null || (Object)(object)itemFoundView != (Object)null || (Object)(object)weaponSelectionView != (Object)null;
		if (!hudSearched && flag && !inGameUIFound)
		{
			inGameUIFound = true;
			hudSearched = true;
			string[] array4 = new string[5] { "GAME UI/Canvas - Game UI/Safe Area/View - Game/PlayerGUI/Inventory", "GAME UI/Canvas - Game UI/Safe Area/View - Game/Inventory", "GAME UI/Canvas - Game UI/Safe Area/PlayerGUI/Inventory", "GAME UI/Canvas - Game UI/Safe Area/View - Game/PlayerGUI", "GAME UI/Canvas - Game UI/Safe Area/View - Game" };
			string[] array5 = array4;
			string[] array6 = array5;
			foreach (string text2 in array6)
			{
				hudInventory = GameObject.Find(text2);
				if ((Object)(object)hudInventory != (Object)null)
				{
					break;
				}
			}
		}
		bool flag2 = false;
		if ((Object)(object)levelUpView != (Object)null && levelUpView.activeInHierarchy)
		{
			activeUIContainers.Add(levelUpView.transform);
			flag2 = true;
		}
		if ((Object)(object)merchantView != (Object)null && merchantView.activeInHierarchy)
		{
			activeUIContainers.Add(merchantView.transform);
			flag2 = true;
		}
		if ((Object)(object)pauseView != (Object)null && pauseView.activeInHierarchy)
		{
			activeUIContainers.Add(pauseView.transform);
			flag2 = true;
		}
		if ((Object)(object)itemFoundView != (Object)null && itemFoundView.activeInHierarchy)
		{
			activeUIContainers.Add(itemFoundView.transform);
			flag2 = true;
		}
		if ((Object)(object)arcanaView != (Object)null && arcanaView.activeInHierarchy)
		{
			activeUIContainers.Add(arcanaView.transform);
			flag2 = true;
		}
		if ((Object)(object)weaponSelectionView != (Object)null && weaponSelectionView.activeInHierarchy)
		{
			activeUIContainers.Add(weaponSelectionView.transform);
			flag2 = true;
		}
		if (!flag2 && inGameUIFound && Time.timeScale == 0f)
		{
			if ((Object)(object)cachedSafeArea == (Object)null)
			{
				GameObject val2 = GameObject.Find("GAME UI/Canvas - Game UI/Safe Area");
				if ((Object)(object)val2 != (Object)null)
				{
					cachedSafeArea = val2.transform;
				}
			}
			if ((Object)(object)cachedSafeArea != (Object)null)
			{
				for (int l = 0; l < cachedSafeArea.childCount; l++)
				{
					Transform child2 = cachedSafeArea.GetChild(l);
					if (((Component)child2).gameObject.activeInHierarchy && (((Object)child2).name.Contains("View") || ((Object)child2).name.Contains("Map") || ((Object)child2).name.Contains("Pause")))
					{
						activeUIContainers.Add(child2);
						if ((Object)(object)pauseView == (Object)null && (((Object)child2).name.ToLower().Contains("map") || ((Object)child2).name.ToLower().Contains("pause")))
						{
							pauseView = ((Component)child2).gameObject;
						}
					}
				}
			}
			else if (!wasGamePaused)
			{
				Plugin.Log.LogWarning("Safe Area not found!");
			}
			flag2 = true;
		}
		if (flag2 && (Object)(object)hudInventory != (Object)null && hudInventory.activeInHierarchy)
		{
			activeUIContainers.Add(hudInventory.transform);
		}
		return flag2;
	}

	private static void ScanForIcons()
	{
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		if (activeUIContainers.Count == 0)
		{
			return;
		}
		if (!lookupTablesBuilt)
		{
			BuildLookupTables();
			if (!lookupTablesBuilt && !loggedScanStatus)
			{
				Plugin.Log.LogWarning("Lookup tables not built - no DataManager cached yet. Hovers won't work until level-up.");
				loggedScanStatus = true;
			}
		}
		int num = 0;
		int num2 = 0;
		foreach (Transform activeUIContainer in activeUIContainers)
		{
			if ((Object)(object)activeUIContainer == (Object)null)
			{
				continue;
			}
			Il2CppArrayBase<Image> componentsInChildren = ((Component)activeUIContainer).GetComponentsInChildren<Image>(false);
			num += componentsInChildren.Length;
			foreach (Image item in componentsInChildren)
			{
				if ((Object)(object)item == (Object)null || (Object)(object)item.sprite == (Object)null)
				{
					continue;
				}
				int instanceID = ((Object)item).GetInstanceID();
				if (trackedIcons.TryGetValue(instanceID, out var value))
				{
					if (value.SpriteName == ((Object)item.sprite).name)
					{
						continue;
					}
					RemoveTracking(value);
					trackedIcons.Remove(instanceID);
				}
				string name = ((Object)item.sprite).name;
				WeaponType? weaponType = null;
				ItemType? itemType = null;
				ItemType value3;
				if (spriteToWeaponType != null && spriteToWeaponType.TryGetValue(name, out var value2))
				{
					weaponType = value2;
				}
				else if (spriteToItemType != null && spriteToItemType.TryGetValue(name, out value3))
				{
					itemType = value3;
				}
				if (weaponType.HasValue && ((object)weaponType.Value/*cast due to constrained. prefix*/).ToString() == "DEFANG")
				{
					weaponType = null;
				}
				if (itemType.HasValue && ((object)itemType.Value/*cast due to constrained. prefix*/).ToString() == "DEFANG")
				{
					itemType = null;
				}
				if (weaponType.HasValue || itemType.HasValue)
				{
					num2++;
					TrackedIcon trackedIcon = new TrackedIcon
					{
						Image = item,
						WeaponType = weaponType,
						ItemType = itemType,
						SpriteName = name,
						InstanceId = instanceID
					};
					AddHoverToIcon(trackedIcon);
					trackedIcons[instanceID] = trackedIcon;
				}
			}
		}
		if (!loggedScanResults && num > 0)
		{
			loggedScanResults = true;
		}
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, TrackedIcon> trackedIcon2 in trackedIcons)
		{
			if ((Object)(object)trackedIcon2.Value.Image == (Object)null || !((Object)(object)trackedIcon2.Value.Image))
			{
				list.Add(trackedIcon2.Key);
			}
		}
		foreach (int item2 in list)
		{
			trackedIcons.Remove(item2);
		}
	}

	private static void AddHoverToIcon(TrackedIcon tracked)
	{
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Expected O, but got Unknown
		GameObject gameObject = ((Component)tracked.Image).gameObject;
		EventTrigger component = gameObject.GetComponent<EventTrigger>();
		if (!((Object)(object)component != (Object)null))
		{
			((Graphic)tracked.Image).raycastTarget = true;
			EventTrigger val = gameObject.AddComponent<EventTrigger>();
			tracked.EventTrigger = val;
			WeaponType? weaponType = tracked.WeaponType;
			ItemType? itemType = tracked.ItemType;
			EventTrigger.Entry val2 = new EventTrigger.Entry();
			val2.eventID = (EventTriggerType)0; // PointerEnter
			((UnityEvent<BaseEventData>)(object)val2.callback).AddListener((UnityEngine.Events.UnityAction<UnityEngine.EventSystems.BaseEventData>)(System.Action<UnityEngine.EventSystems.BaseEventData>)(delegate
			{
				var t = ((Component)tracked.Image).transform;
				if (IsLevelUpViewActive())
					RequestLevelUpHover(t, weaponType, itemType);
				else
					ShowItemPopup(t, weaponType, itemType);
			}));
			val.triggers.Add(val2);
			EventTrigger.Entry val3 = new EventTrigger.Entry();
			val3.eventID = (EventTriggerType)1; // PointerExit
			((UnityEvent<BaseEventData>)(object)val3.callback).AddListener((UnityEngine.Events.UnityAction<UnityEngine.EventSystems.BaseEventData>)(System.Action<UnityEngine.EventSystems.BaseEventData>)(delegate
			{
				CancelLevelUpHoverIfMatch(((Component)tracked.Image).transform);
				DelayFrames(10, () => { if (mouseOverPopupIndex < 0 && popupStack.Count > 0) HideAllPopups(); });
			}));
			val.triggers.Add(val3);
		}
	}

	private static void RemoveTracking(TrackedIcon tracked)
	{
		if ((Object)(object)tracked.EventTrigger != (Object)null)
		{
			Object.Destroy((Object)(object)tracked.EventTrigger);
		}
	}

	private static void ClearTrackedIcons()
	{
		foreach (KeyValuePair<int, TrackedIcon> trackedIcon in trackedIcons)
		{
			RemoveTracking(trackedIcon.Value);
		}
		trackedIcons.Clear();
	}

	private static void ScanWeaponSelectionView(GameObject viewGo)
	{
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		if (scannedWeaponSelection)
		{
			return;
		}
		scannedWeaponSelection = true;
		if (!triedFindingWSIType)
		{
			triedFindingWSIType = true;
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			Assembly[] array = assemblies;
			foreach (Assembly assembly in array)
			{
				if (!assembly.FullName.Contains("Il2Cpp"))
				{
					continue;
				}
				try
				{
					Type type = assembly.GetTypes().FirstOrDefault((Type t) => t.Name == "WeaponSelectionItemUI");
					if (type != null)
					{
						cachedWeaponSelectionItemType = type;
						break;
					}
				}
				catch
				{
				}
			}
		}
		if (cachedWeaponSelectionItemType == null)
		{
			Plugin.Log.LogWarning("WeaponSelectionItemUI type not found in assemblies");
			return;
		}
		Transform val = FindChildRecursive(viewGo.transform, "Panel");
		if ((Object)(object)val == (Object)null)
		{
			Plugin.Log.LogWarning("Panel not found in WeaponSelection");
			return;
		}
		Transform val2 = val.Find("ScrollViewWithSlider");
		if ((Object)(object)val2 == (Object)null)
		{
			Plugin.Log.LogWarning("ScrollViewWithSlider not found");
			return;
		}
		Transform val3 = val2.Find("Viewport");
		if ((Object)(object)val3 == (Object)null)
		{
			Plugin.Log.LogWarning("Viewport not found");
			return;
		}
		Transform val4 = null;
		for (int num = 0; num < val3.childCount; num++)
		{
			Transform child = val3.GetChild(num);
			if (((Object)child).name == "Content")
			{
				val4 = child;
				break;
			}
		}
		if ((Object)(object)val4 == (Object)null)
		{
			Plugin.Log.LogWarning("Content not found");
			return;
		}
		MethodInfo method = cachedWeaponSelectionItemType.GetMethod("get__type", BindingFlags.Instance | BindingFlags.Public);
		MethodInfo method2 = cachedWeaponSelectionItemType.GetMethod("GetWeaponType", BindingFlags.Instance | BindingFlags.Public);
		int num2 = 0;
		for (int num3 = 0; num3 < val4.childCount; num3++)
		{
			Transform child2 = val4.GetChild(num3);
			if (!((Component)child2).gameObject.activeInHierarchy)
			{
				continue;
			}
			WeaponType? weaponType = null;
			try
			{
				Component component = ((Component)child2).gameObject.GetComponent("WeaponSelectionItemUI");
				if ((Object)(object)component != (Object)null)
				{
					object obj2 = Activator.CreateInstance(cachedWeaponSelectionItemType, ((Il2CppObjectBase)component).Pointer);
					if (method != null && method.Invoke(obj2, null) is WeaponType value)
					{
						weaponType = value;
					}
					if (!weaponType.HasValue && method2 != null && method2.Invoke(obj2, null) is WeaponType value2)
					{
						weaponType = value2;
					}
				}
			}
			catch (Exception ex)
			{
				if (num3 == 0)
				{
					Plugin.Log.LogWarning("Error reading WSI component: " + ex.Message);
				}
			}
			if (weaponType.HasValue)
			{
				Transform val5 = child2.Find("WeaponFrame");
				GameObject go = (((Object)(object)val5 != (Object)null) ? ((Component)val5).gameObject : ((Component)child2).gameObject);
				AddHoverToGameObject(go, weaponType, null);
				num2++;
			}
		}
		Plugin.Log.LogInfo($"WeaponSelection: set up hovers on {num2}/{val4.childCount} items");
	}

	private static int ScanChildrenForTypes(Transform parent, int depth, int maxDepth)
	{
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		if (depth > maxDepth)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < parent.childCount; i++)
		{
			Transform child = parent.GetChild(i);
			if (!((Component)child).gameObject.activeInHierarchy)
			{
				continue;
			}
			WeaponType? weaponType = null;
			ItemType? itemType = null;
			Il2CppArrayBase<Component> components = ((Component)child).GetComponents<Component>();
			foreach (Component item in components)
			{
				if ((Object)(object)item == (Object)null)
				{
					continue;
				}
				Type type = ((object)item).GetType();
				if (type.Namespace != null && type.Namespace.StartsWith("UnityEngine"))
				{
					continue;
				}
				PropertyInfo property = type.GetProperty("Type", BindingFlags.Instance | BindingFlags.Public);
				if (property != null)
				{
					try
					{
						object value = property.GetValue(item);
						if (value is WeaponType value2)
						{
							weaponType = value2;
						}
						else if (value is ItemType value3)
						{
							itemType = value3;
						}
					}
					catch
					{
					}
				}
				if (!weaponType.HasValue && !itemType.HasValue)
				{
					FieldInfo field = type.GetField("_type", BindingFlags.Instance | BindingFlags.NonPublic);
					if (field != null)
					{
						try
						{
							object value4 = field.GetValue(item);
							if (value4 is WeaponType value5)
							{
								weaponType = value5;
							}
							else if (value4 is ItemType value6)
							{
								itemType = value6;
							}
						}
						catch
						{
						}
					}
				}
				if (!weaponType.HasValue && !itemType.HasValue)
				{
					PropertyInfo property2 = type.GetProperty("WeaponType", BindingFlags.Instance | BindingFlags.Public);
					if (property2 != null)
					{
						try
						{
							if (property2.GetValue(item) is WeaponType value7)
							{
								weaponType = value7;
							}
						}
						catch
						{
						}
					}
					PropertyInfo property3 = type.GetProperty("ItemType", BindingFlags.Instance | BindingFlags.Public);
					if (property3 != null)
					{
						try
						{
							if (property3.GetValue(item) is ItemType value8)
							{
								itemType = value8;
							}
						}
						catch
						{
						}
					}
				}
				if (!weaponType.HasValue && !itemType.HasValue)
				{
					continue;
				}
				break;
			}
			if (weaponType.HasValue || itemType.HasValue)
			{
				AddHoverToGameObject(((Component)child).gameObject, weaponType, itemType);
				num++;
			}
			num += ScanChildrenForTypes(child, depth + 1, maxDepth);
		}
		return num;
	}

	private static void ScanPauseViewForEquipment(GameObject pauseViewGo)
	{
		if (scannedPauseView)
		{
			return;
		}
		scannedPauseView = true;
		Transform val = FindChildRecursive(pauseViewGo.transform, "EquipmentPanel");
		if (!((Object)(object)val == (Object)null))
		{
			Transform val2 = val.Find("WeaponsPanel");
			Transform val3 = val.Find("AccessoryPanel");
			int num = 0;
			int num2 = 0;
			if ((Object)(object)val2 != (Object)null)
			{
				num = SetupEquipmentIconHovers(val2, isWeapons: true);
			}
			if ((Object)(object)val3 != (Object)null)
			{
				num2 = SetupEquipmentIconHovers(val3, isWeapons: false);
			}
		}
	}

	private static int SetupEquipmentIconHovers(Transform panel, bool isWeapons)
	{
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		for (int i = 0; i < panel.childCount; i++)
		{
			Transform child = panel.GetChild(i);
			if (!((Object)child).name.Contains("EquipmentIconPause"))
			{
				continue;
			}
			Il2CppArrayBase<Component> components = ((Component)child).GetComponents<Component>();
			WeaponType? weaponType = null;
			ItemType? itemType = null;
			foreach (Component item in components)
			{
				if ((Object)(object)item == (Object)null)
				{
					continue;
				}
				Type type = ((object)item).GetType();
				if (type.Namespace != null && type.Namespace.StartsWith("UnityEngine"))
				{
					continue;
				}
				PropertyInfo property = type.GetProperty("Type", BindingFlags.Instance | BindingFlags.Public);
				if (property != null)
				{
					object value = property.GetValue(item);
					if (value is WeaponType value2)
					{
						weaponType = value2;
					}
					else if (value is ItemType value3)
					{
						itemType = value3;
					}
				}
				FieldInfo field = type.GetField("_type", BindingFlags.Instance | BindingFlags.NonPublic);
				if (field != null && !weaponType.HasValue && !itemType.HasValue)
				{
					object value4 = field.GetValue(item);
					if (value4 is WeaponType value5)
					{
						weaponType = value5;
					}
					else if (value4 is ItemType value6)
					{
						itemType = value6;
					}
				}
			}
			if (weaponType.HasValue || itemType.HasValue)
			{
				AddHoverToGameObject(((Component)child).gameObject, weaponType, itemType);
				num++;
			}
		}
		return num;
	}

	private static void LogHierarchy(Transform t, int depth, int maxDepth)
	{
		if (depth <= maxDepth)
		{
			string text = new string(' ', depth * 2);
			Image component = ((Component)t).GetComponent<Image>();
			string text2 = (((Object)(object)component != (Object)null && (Object)(object)component.sprite != (Object)null) ? (" [sprite: " + ((Object)component.sprite).name + "]") : "");
			for (int i = 0; i < t.childCount && i < 10; i++)
			{
				LogHierarchy(t.GetChild(i), depth + 1, maxDepth);
			}
			if (t.childCount > 10)
			{
			}
		}
	}

	private static Transform FindChildRecursive(Transform parent, string name)
	{
		for (int i = 0; i < parent.childCount; i++)
		{
			Transform child = parent.GetChild(i);
			if (((Object)child).name.ToLower().Contains(name.ToLower()))
			{
				return child;
			}
			Transform val = FindChildRecursive(child, name);
			if ((Object)(object)val != (Object)null)
			{
				return val;
			}
		}
		return null;
	}

	private static string GetFullPath(Transform t)
	{
		string text = ((Object)t).name;
		Transform parent = t.parent;
		while ((Object)(object)parent != (Object)null)
		{
			text = ((Object)parent).name + "/" + text;
			parent = parent.parent;
		}
		return text;
	}

	private static IEnumerator DelayedHideCheck()
	{
		int stackSizeAtStart = popupStack.Count;
		for (int i = 0; i < 10; i++)
		{
			yield return null;
		}
		if (mouseOverPopupIndex < 0 && popupStack.Count <= stackSizeAtStart && popupStack.Count > 0)
		{
			HideAllPopups();
		}
	}

	private static IEnumerator DelayedStackHideCheck(int exitedPopupIndex)
	{
		int stackSizeAtStart = popupStack.Count;
		for (int i = 0; i < 10; i++)
		{
			yield return null;
		}
		if (popupStack.Count <= stackSizeAtStart && mouseOverPopupIndex < exitedPopupIndex)
		{
			int closeFromIndex = mouseOverPopupIndex + 1;
			if (closeFromIndex < 0)
			{
				closeFromIndex = 0;
			}
			while (popupStack.Count > closeFromIndex)
			{
				HideTopPopup();
			}
		}
	}

	private static void BuildLookupTables()
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
		{
			if (cachedWeaponsDict != null)
			{
				BuildWeaponLookup(cachedWeaponsDict);
			}
			if (cachedPowerUpsDict != null)
			{
				BuildPowerUpLookup(cachedPowerUpsDict);
			}
			if (cachedWeaponsDict != null)
			{
				BuildEvoSynergyLookup(cachedWeaponsDict);
			}
			lookupTablesBuilt = spriteToWeaponType.Count > 0 || spriteToItemType.Count > 0;
			if (lookupTablesBuilt && !loggedLookupTables)
			{
				loggedLookupTables = true;
				Plugin.Log.LogInfo($"Built lookup tables: {spriteToWeaponType.Count} weapons, {spriteToItemType.Count} items");
			}
		}
		catch (Exception arg)
		{
			Plugin.Log.LogError($"Failed to build lookup tables: {arg}");
		}
		spriteToWeaponType.Remove("goldenegg");
		spriteToWeaponType.Remove("Antidote");
		spriteToItemType.Remove("Antidote");
	}

	private static void MergeWeaponDicts(object target, object source)
	{
		try
		{
			Type type = source.GetType();
			PropertyInfo property = type.GetProperty("Keys");
			if (property == null)
			{
				return;
			}
			object value = property.GetValue(source);
			object obj = value.GetType().GetMethod("GetEnumerator").Invoke(value, null);
			MethodInfo method = obj.GetType().GetMethod("MoveNext");
			PropertyInfo property2 = obj.GetType().GetProperty("Current");
			Type type2 = target.GetType();
			MethodInfo method2 = type2.GetMethod("ContainsKey");
			MethodInfo method3 = type2.GetMethod("Add");
			PropertyInfo property3 = type2.GetProperty("Item");
			PropertyInfo property4 = type.GetProperty("Item");
			while ((bool)method.Invoke(obj, null))
			{
				object value2 = property2.GetValue(obj);
				if (!(bool)method2.Invoke(target, new object[1] { value2 }))
				{
					object value3 = property4.GetValue(source, new object[1] { value2 });
					method3.Invoke(target, new object[2] { value2, value3 });
				}
			}
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[MergeWeaponDicts] " + ex.Message);
		}
	}

	private static void BuildEvoSynergyLookup(object weaponsDict)
	{
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Expected I4, but got Unknown
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Type type = weaponsDict.GetType();
			PropertyInfo property = type.GetProperty("Keys");
			if (property == null)
			{
				return;
			}
			object value = property.GetValue(weaponsDict);
			object obj = value.GetType().GetMethod("GetEnumerator").Invoke(value, null);
			MethodInfo method = obj.GetType().GetMethod("MoveNext");
			PropertyInfo property2 = obj.GetType().GetProperty("Current");
			while ((bool)method.Invoke(obj, null))
			{
				WeaponType val = (WeaponType)property2.GetValue(obj);
				List<WeaponData> weaponDataList = GetWeaponDataList(val);
				if (weaponDataList == null)
				{
					continue;
				}
				for (int i = 0; i < weaponDataList.Count; i++)
				{
					WeaponData val2 = weaponDataList[i];
					if (val2 == null)
					{
						continue;
					}
					try
					{
						PropertyInfo property3 = ((object)val2).GetType().GetProperty("evoSynergy");
						if (property3 != null && property3.GetValue(val2) is Il2CppStructArray<WeaponType> val3)
						{
							for (int j = 0; j < ((Il2CppArrayBase<WeaponType>)(object)val3).Length; j++)
							{
								WeaponType val4 = ((Il2CppArrayBase<WeaponType>)(object)val3)[j];
								int num = (int)val4;
								if (spriteToWeaponType.ContainsValue(val4) || Enum.IsDefined(typeof(ItemType), num))
								{
									continue;
								}
								WeaponData weaponData = GetWeaponData(val4);
								if (weaponData != null)
								{
									string propertyValue = GetPropertyValue<string>(weaponData, "frameName");
									if (!string.IsNullOrEmpty(propertyValue) && !spriteToWeaponType.ContainsKey(propertyValue))
									{
										spriteToWeaponType[propertyValue] = val4;
									}
								}
							}
						}
						PropertyInfo propertyInfo = ((object)val2).GetType().GetProperty("isEvolution") ?? ((object)val2).GetType().GetProperty("_isEvolution_k__BackingField");
						if (!(propertyInfo != null) || !(bool)propertyInfo.GetValue(val2))
						{
							continue;
						}
						string[] array = new string[2] { "requires", "requiresMax" };
						foreach (string name in array)
						{
							PropertyInfo property4 = ((object)val2).GetType().GetProperty(name);
							if (property4 == null)
							{
								continue;
							}
							object value2 = property4.GetValue(val2);
							if (value2 == null)
							{
								continue;
							}
							PropertyInfo property5 = value2.GetType().GetProperty("Count");
							if (property5 == null)
							{
								continue;
							}
							int num2 = (int)property5.GetValue(value2);
							PropertyInfo property6 = value2.GetType().GetProperty("Item");
							for (int l = 0; l < num2; l++)
							{
								WeaponType val5 = (WeaponType)property6.GetValue(value2, new object[1] { l });
								if (spriteToWeaponType.ContainsValue(val5))
								{
									continue;
								}
								WeaponData weaponData2 = GetWeaponData(val5);
								if (weaponData2 != null)
								{
									string propertyValue2 = GetPropertyValue<string>(weaponData2, "frameName");
									if (!string.IsNullOrEmpty(propertyValue2) && !spriteToWeaponType.ContainsKey(propertyValue2))
									{
										spriteToWeaponType[propertyValue2] = val5;
									}
								}
							}
						}
					}
					catch (Exception ex)
					{
						Plugin.Log.LogInfo($"[EvoSynergy] Inner error for {val}: {ex.Message}");
					}
				}
			}
		}
		catch (Exception ex2)
		{
			Plugin.Log.LogWarning("Error building evo synergy lookup: " + ex2.Message);
		}
		spriteToWeaponType.Remove("goldenegg");
		spriteToWeaponType.Remove("Antidote");
	}

	private static void BuildWeaponLookup(object weaponsDict)
	{
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Type type = weaponsDict.GetType();
			PropertyInfo property = type.GetProperty("Keys");
			if (property == null)
			{
				return;
			}
			object value = property.GetValue(weaponsDict);
			int num = 0;
			object obj = value.GetType().GetMethod("GetEnumerator").Invoke(value, null);
			MethodInfo method = obj.GetType().GetMethod("MoveNext");
			PropertyInfo property2 = obj.GetType().GetProperty("Current");
			while ((bool)method.Invoke(obj, null))
			{
				num++;
				object value2 = property2.GetValue(obj);
				if (!(value2 is WeaponType value3))
				{
					continue;
				}
				PropertyInfo property3 = type.GetProperty("Item");
				if (!(property3 != null))
				{
					continue;
				}
				object value4 = property3.GetValue(weaponsDict, new object[1] { value2 });
				if (value4 == null)
				{
					continue;
				}
				Type type2 = value4.GetType();
				PropertyInfo property4 = type2.GetProperty("Count");
				PropertyInfo property5 = type2.GetProperty("Item");
				if (!(property4 != null) || !(property5 != null))
				{
					continue;
				}
				int num2 = (int)property4.GetValue(value4);
				for (int i = 0; i < num2; i++)
				{
					object value5 = property5.GetValue(value4, new object[1] { i });
					WeaponData val = (WeaponData)((value5 is WeaponData) ? value5 : null);
					if (val != null && !string.IsNullOrEmpty(val.frameName))
					{
						spriteToWeaponType[val.frameName] = value3;
					}
				}
			}
			spriteToWeaponType.Remove("goldenegg");
		}
		catch (Exception arg)
		{
			Plugin.Log.LogWarning($"Error building weapon lookup: {arg}");
		}
	}

	private static void BuildPowerUpLookup(object powerUpsDict)
	{
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Type type = powerUpsDict.GetType();
			PropertyInfo property = type.GetProperty("Keys");
			if (property == null)
			{
				return;
			}
			object value = property.GetValue(powerUpsDict);
			object obj = value.GetType().GetMethod("GetEnumerator").Invoke(value, null);
			MethodInfo method = obj.GetType().GetMethod("MoveNext");
			PropertyInfo property2 = obj.GetType().GetProperty("Current");
			int num = 0;
			while ((bool)method.Invoke(obj, null))
			{
				num++;
				object value2 = property2.GetValue(obj);
				if (!(value2 is ItemType it))
				{
					continue;
				}
				PropertyInfo property3 = type.GetProperty("Item");
				if (!(property3 != null))
				{
					continue;
				}
				object value3 = property3.GetValue(powerUpsDict, new object[1] { value2 });
				if (value3 == null)
				{
					continue;
				}
				Type type2 = value3.GetType();
				PropertyInfo property4 = type2.GetProperty("Count");
				if (property4 != null)
				{
					PropertyInfo property5 = type2.GetProperty("Item");
					int num2 = (int)property4.GetValue(value3);
					for (int i = 0; i < num2; i++)
					{
						object value4 = property5.GetValue(value3, new object[1] { i });
						AddPowerUpToLookup(value4, it);
					}
				}
				else
				{
					AddPowerUpToLookup(value3, it);
				}
			}
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("Error building powerup lookup: " + ex.Message);
		}
	}

	private static void AddPowerUpToLookup(object data, ItemType it)
	{
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		if (data == null)
		{
			return;
		}
		string text = null;
		PropertyInfo property = data.GetType().GetProperty("frameName", BindingFlags.Instance | BindingFlags.Public);
		if (property != null)
		{
			text = property.GetValue(data) as string;
		}
		if (string.IsNullOrEmpty(text))
		{
			FieldInfo field = data.GetType().GetField("frameName", BindingFlags.Instance | BindingFlags.Public);
			if (field != null)
			{
				text = field.GetValue(data) as string;
			}
		}
		if (!string.IsNullOrEmpty(text))
		{
			spriteToItemType[text] = it;
		}
	}

	public static void CacheGameSession(object gameSession)
	{
		cachedGameSession = gameSession;
		if (cachedDataManager != null || gameSession == null)
		{
			return;
		}
		try
		{
			Type type = gameSession.GetType();
			MethodInfo method = type.GetMethod("get_Data", BindingFlags.Instance | BindingFlags.Public);
			if (method != null)
			{
				object obj = method.Invoke(gameSession, null);
				if (obj != null)
				{
					CacheDataManager(obj);
					return;
				}
			}
			Plugin.Log.LogWarning("[CacheGameSession] Could not find Data on GameSession");
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("Error caching DataManager from session: " + ex.Message);
		}
	}

	private static void TryFindGameSession()
	{
		try
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			Assembly[] array = assemblies;
			foreach (Assembly assembly in array)
			{
				if (!assembly.FullName.Contains("Il2Cpp"))
				{
					continue;
				}
				try
				{
					Type type = assembly.GetTypes().FirstOrDefault((Type t) => t.Name == "GameSessionData");
					if (type != null)
					{
						MethodInfo methodInfo = typeof(Object).GetMethods().FirstOrDefault((MethodInfo m) => m.Name == "FindObjectOfType" && m.IsGenericMethod && m.GetParameters().Length == 0);
						if (methodInfo != null)
						{
							MethodInfo methodInfo2 = methodInfo.MakeGenericMethod(type);
							object obj = methodInfo2.Invoke(null, null);
							if (obj != null)
							{
								PropertyInfo property = obj.GetType().GetProperty("ActiveCharacter", BindingFlags.Instance | BindingFlags.Public);
								if (property != null)
								{
									cachedGameSession = obj;
									return;
								}
								break;
							}
							break;
						}
						break;
					}
				}
				catch
				{
				}
			}
			try
			{
				GameObject val = GameObject.Find("Game");
				if ((Object)(object)val != (Object)null)
				{
					Il2CppArrayBase<Component> components = val.GetComponents<Component>();
					foreach (Component item in components)
					{
						if ((Object)(object)item == (Object)null)
						{
							continue;
						}
						Type type2 = ((object)item).GetType();
						if (!type2.Name.Contains("GameManager"))
						{
							continue;
						}
						PropertyInfo property2 = type2.GetProperty("GameSessionData", BindingFlags.Instance | BindingFlags.Public);
						if (property2 != null)
						{
							object value = property2.GetValue(item);
							if (value != null)
							{
								CacheGameSession(value);
								GenericIconPatches.TryCacheDataManagerFromGameManager(item);
								return;
							}
						}
					}
				}
			}
			catch
			{
			}
			Assembly[] assemblies2 = AppDomain.CurrentDomain.GetAssemblies();
			Assembly[] array2 = assemblies2;
			foreach (Assembly assembly2 in array2)
			{
				if (!(assembly2.FullName.Contains("VampireSurvivors") || assembly2.FullName.Contains("Il2Cpp") || assembly2.GetName().Name == "Assembly-CSharp"))
				{
					continue;
				}
				try
				{
					IEnumerable<Type> enumerable = (from t in assembly2.GetTypes()
						where t.Name.Contains("GameSession") || t.Name == "GameManager"
						select t).Take(5);
					foreach (Type item2 in enumerable)
					{
						PropertyInfo property3 = item2.GetProperty("Instance", BindingFlags.Static | BindingFlags.Public);
						if (property3 != null)
						{
							try
							{
								object value2 = property3.GetValue(null);
								if (value2 != null)
								{
									PropertyInfo property4 = value2.GetType().GetProperty("ActiveCharacter", BindingFlags.Instance | BindingFlags.Public);
									if (property4 != null)
									{
										cachedGameSession = value2;
										return;
									}
								}
							}
							catch
							{
							}
						}
						FieldInfo field = item2.GetField("_instance", BindingFlags.Static | BindingFlags.NonPublic);
						if (field == null)
						{
							field = item2.GetField("instance", BindingFlags.Static | BindingFlags.NonPublic);
						}
						if (!(field != null))
						{
							continue;
						}
						try
						{
							object value3 = field.GetValue(null);
							if (value3 != null)
							{
								PropertyInfo property5 = value3.GetType().GetProperty("ActiveCharacter", BindingFlags.Instance | BindingFlags.Public);
								if (property5 != null)
								{
									cachedGameSession = value3;
									return;
								}
							}
						}
						catch
						{
						}
					}
				}
				catch
				{
				}
			}
			List<GameObject> list = new List<GameObject>();
			if ((Object)(object)pauseView != (Object)null && pauseView.activeInHierarchy)
			{
				list.Add(pauseView);
			}
			if ((Object)(object)merchantView != (Object)null && merchantView.activeInHierarchy)
			{
				list.Add(merchantView);
			}
			if ((Object)(object)levelUpView != (Object)null && levelUpView.activeInHierarchy)
			{
				list.Add(levelUpView);
			}
			if ((Object)(object)arcanaView != (Object)null && arcanaView.activeInHierarchy)
			{
				list.Add(arcanaView);
			}
			foreach (GameObject item3 in list)
			{
				Il2CppArrayBase<Component> componentsInChildren = item3.GetComponentsInChildren<Component>(true);
				foreach (Component item4 in componentsInChildren)
				{
					if ((Object)(object)item4 == (Object)null || !TryGetSessionFromComponent(item4))
					{
						continue;
					}
					return;
				}
			}
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("Error finding game session: " + ex.Message);
		}
	}

	private static bool TryGetSessionFromComponent(Component component)
	{
		Type type = ((object)component).GetType();
		string[] array = new string[4] { "_gameSession", "GameSession", "Session", "gameSession" };
		string[] array2 = array;
		string[] array3 = array2;
		foreach (string name in array3)
		{
			try
			{
				PropertyInfo property = type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (property != null)
				{
					object value = property.GetValue(component);
					if (value != null)
					{
						PropertyInfo property2 = value.GetType().GetProperty("ActiveCharacter", BindingFlags.Instance | BindingFlags.Public);
						if (property2 != null)
						{
							cachedGameSession = value;
							return true;
						}
					}
				}
				FieldInfo field = type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (!(field != null))
				{
					continue;
				}
				object value2 = field.GetValue(component);
				if (value2 != null)
				{
					PropertyInfo property3 = value2.GetType().GetProperty("ActiveCharacter", BindingFlags.Instance | BindingFlags.Public);
					if (property3 != null)
					{
						cachedGameSession = value2;
						return true;
					}
				}
			}
			catch
			{
			}
		}
		return false;
	}

	public static void SetupHUDHovers()
	{
		if (cachedGameSession == null || (Object)(object)hudInventory == (Object)null)
		{
			return;
		}
		try
		{
			PropertyInfo property = cachedGameSession.GetType().GetProperty("ActiveCharacter", BindingFlags.Instance | BindingFlags.Public);
			if (property == null)
			{
				return;
			}
			object value = property.GetValue(cachedGameSession);
			if (value == null)
			{
				return;
			}
			PropertyInfo property2 = value.GetType().GetProperty("WeaponsManager", BindingFlags.Instance | BindingFlags.Public);
			if (property2 != null)
			{
				object value2 = property2.GetValue(value);
				if (value2 != null)
				{
					PropertyInfo property3 = value2.GetType().GetProperty("ActiveEquipment", BindingFlags.Instance | BindingFlags.Public);
					if (property3 != null)
					{
						object value3 = property3.GetValue(value2);
						SetupHUDSlots(value3, isWeapons: true);
					}
				}
			}
			PropertyInfo property4 = value.GetType().GetProperty("AccessoriesManager", BindingFlags.Instance | BindingFlags.Public);
			if (!(property4 != null))
			{
				return;
			}
			object value4 = property4.GetValue(value);
			if (value4 != null)
			{
				PropertyInfo property5 = value4.GetType().GetProperty("ActiveEquipment", BindingFlags.Instance | BindingFlags.Public);
				if (property5 != null)
				{
					object value5 = property5.GetValue(value4);
					SetupHUDSlots(value5, isWeapons: false);
				}
			}
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("Error setting up HUD hovers: " + ex.Message);
		}
	}

	private static void SetupHUDSlots(object equipList, bool isWeapons)
	{
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		if (equipList == null || (Object)(object)hudInventory == (Object)null)
		{
			return;
		}
		try
		{
			Type type = equipList.GetType();
			PropertyInfo property = type.GetProperty("Count");
			PropertyInfo property2 = type.GetProperty("Item");
			if (property == null || property2 == null)
			{
				return;
			}
			int num = (int)property.GetValue(equipList);
			Transform val = hudInventory.transform.Find(isWeapons ? "Weapons" : "Accessories");
			if ((Object)(object)val == (Object)null)
			{
				val = hudInventory.transform.Find(isWeapons ? "WeaponSlots" : "AccessorySlots");
			}
			if ((Object)(object)val == (Object)null)
			{
				val = hudInventory.transform;
			}
			List<Transform> list = new List<Transform>();
			for (int i = 0; i < val.childCount; i++)
			{
				list.Add(val.GetChild(i));
			}
			for (int j = 0; j < num && j < list.Count; j++)
			{
				object value = property2.GetValue(equipList, new object[1] { j });
				if (value == null)
				{
					continue;
				}
				PropertyInfo property3 = value.GetType().GetProperty("Type", BindingFlags.Instance | BindingFlags.Public);
				if (!(property3 == null))
				{
					object value2 = property3.GetValue(value);
					GameObject gameObject = ((Component)list[j]).gameObject;
					GameObject val2 = FindIconInUI(gameObject);
					GameObject go = val2 ?? gameObject;
					if (isWeapons && value2 is WeaponType value3)
					{
						AddHoverToGameObject(go, value3, null);
					}
					else if (!isWeapons && value2 is ItemType value4)
					{
						AddHoverToGameObject(go, null, value4);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("Error in SetupHUDSlots: " + ex.Message);
		}
	}

	public static bool HasCachedDataManager()
	{
		return cachedDataManager != null;
	}

	private static void TryCacheDataManagerStatic()
	{
		if (cachedDataManager != null)
		{
			return;
		}
		try
		{
			Type type = FindTypeByName("GameManager");
			if (type != null)
			{
				object obj = null;
				MemberInfo[] members = type.GetMembers(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				MemberInfo[] array = members;
				MemberInfo[] array2 = array;
				foreach (MemberInfo memberInfo in array2)
				{
					try
					{
						if (memberInfo is PropertyInfo propertyInfo && propertyInfo.PropertyType == type)
						{
							obj = propertyInfo.GetValue(null);
							if (obj != null)
							{
								break;
							}
						}
						else if (memberInfo is FieldInfo fieldInfo && fieldInfo.FieldType == type)
						{
							obj = fieldInfo.GetValue(null);
							if (obj != null)
							{
								break;
							}
						}
					}
					catch
					{
					}
				}
				if (obj != null)
				{
					PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					PropertyInfo[] array3 = properties;
					PropertyInfo[] array4 = array3;
					foreach (PropertyInfo propertyInfo2 in array4)
					{
						if (!(propertyInfo2.Name == "Data") && !propertyInfo2.PropertyType.Name.Contains("DataManager"))
						{
							continue;
						}
						try
						{
							object value = propertyInfo2.GetValue(obj);
							if (value != null)
							{
								CacheDataManager(value);
								return;
							}
						}
						catch
						{
						}
					}
				}
			}
			Il2CppArrayBase<MonoBehaviour> val = Object.FindObjectsOfType<MonoBehaviour>();
			for (int num3 = 0; num3 < val.Count && num3 < 200; num3++)
			{
				MonoBehaviour val2 = val[num3];
				if (!((Object)(object)val2 == (Object)null))
				{
					MethodInfo method = ((object)val2).GetType().GetMethod("GetConvertedWeapons");
					if (method != null)
					{
						CacheDataManager(val2);
						break;
					}
				}
			}
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Collection] Data caching failed: " + ex.Message);
		}
	}

	public static void CacheDataManager(object dataManager)
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
		cachedDataManager = dataManager;
		try
		{
			Type type = dataManager.GetType();
			MethodInfo method = type.GetMethod("GetConvertedWeapons");
			MethodInfo method2 = type.GetMethod("GetConvertedPowerUpData");
			if (method != null)
			{
				cachedWeaponsDict = method.Invoke(dataManager, null);
			}
			if (method2 != null)
			{
				cachedPowerUpsDict = method2.Invoke(dataManager, null);
			}
			Type type2 = null;
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				try
				{
					type2 = assembly.GetTypes().FirstOrDefault((Type t) => t.Name == "DlcType" && t.IsEnum);
				}
				catch
				{
				}
				if (type2 != null)
				{
					break;
				}
			}
			if (type2 != null)
			{
				MethodInfo method3 = type.GetMethod("GetConvertedDlcWeaponData");
				foreach (object value in Enum.GetValues(type2))
				{
					try
					{
						object obj2 = method3.Invoke(dataManager, new object[1] { value });
						if (obj2 != null)
						{
							PropertyInfo property = obj2.GetType().GetProperty("Count");
							int num = ((property != null) ? ((int)property.GetValue(obj2)) : (-1));
							if (num > 0)
							{
								MergeWeaponDicts(cachedWeaponsDict, obj2);
							}
						}
					}
					catch
					{
					}
				}
			}
			lookupTablesBuilt = false;
			BuildLookupTables();
			List<string> list = (from kv in spriteToWeaponType
				where ((object)kv.Value/*cast due to constrained. prefix*/).ToString() == "DEFANG"
				select kv.Key).ToList();
			foreach (string item in list)
			{
				spriteToWeaponType.Remove(item);
			}
		}
		catch (Exception arg)
		{
			Plugin.Log.LogError($"Error caching data manager: {arg}");
		}
	}

	private static void DetectInputMode()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		Vector3 mousePosition = Input.mousePosition;
		Vector3 val = mousePosition - lastMousePosition;
		bool mouseMoved = val.sqrMagnitude > 1f;
		lastMousePosition = mousePosition;
		if (mouseMoved)
		{
			// Deliberate mouse movement after Level Up opens unlocks hover tooltips
			if (IsLevelUpViewActive())
				levelUpHoverUnlocked = true;
			if (usingController)
			{
				if (equipmentNavMode)
				{
					ExitEquipmentNavMode();
				}
				usingController = false;
				ExitInteractiveMode();
				dwellTarget = null;
				passivePopupShown = false;
			}
			return;
		}
		// Only enter controller mode on real gamepad/keyboard nav input —
		// NOT merely because Level Up auto-selected the first card (that was
		// causing unsolicited tooltips with a stationary mouse).
		EventSystem current = EventSystem.current;
		if ((Object)(object)current != (Object)null)
		{
			GameObject currentSelectedGameObject = current.currentSelectedGameObject;
			if (HasControllerNavInput())
			{
				usingController = true;
			}
			lastSelectedObject = currentSelectedGameObject;
		}
	}

	/// <summary>True when player is actively driving UI with pad/keys (not auto-selection).</summary>
	private static bool HasControllerNavInput()
	{
		try
		{
			if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.35f) return true;
			if (Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.35f) return true;
		}
		catch { }
		// Face / d-pad style keys used elsewhere in this mod
		if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.JoystickButton1)
			|| Input.GetKeyDown(KeyCode.JoystickButton2) || Input.GetKeyDown(KeyCode.JoystickButton3))
			return true;
		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)
			|| Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
			return true;
		if (IsInteractButtonPressed() || IsSubmitButtonPressed())
			return true;
		return false;
	}

	private static void ResetHoverDwellState()
	{
		dwellTarget = null;
		passivePopupShown = false;
		usingController = false;
		preDwellSelection = null;
		if (equipmentNavMode)
		{
			try { ExitEquipmentNavMode(); } catch { }
		}
		try { ExitInteractiveMode(); } catch { }
	}

	/// <summary>Called when Level Up UI opens — clear popups so nothing shows until real hover.</summary>
	public static void OnLevelUpOpened()
	{
		try
		{
			HideAllPopups();
			ResetHoverDwellState();
			// Cards often spawn under the cursor → PointerEnter fires immediately. Require a
			// deliberate mouse move + short hover delay before any Level Up tooltip.
			suppressUnsolicitedPopupUntil = Time.unscaledTime + 1.0f;
			levelUpHoverUnlocked = false;
			ClearLevelUpPendingHover();
			Plugin.Dbg("OnLevelUpOpened: cleared popups; hover locked until mouse moves");
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("OnLevelUpOpened: " + ex.Message);
		}
	}

	private static void ClearLevelUpPendingHover()
	{
		levelUpPendingAnchor = null;
		levelUpPendingWeapon = null;
		levelUpPendingItem = null;
		levelUpPendingSince = -1f;
	}

	/// <summary>Level Up only: queue hover for delayed show (called from EventTrigger).</summary>
	private static void RequestLevelUpHover(Transform anchor, WeaponType? weaponType, ItemType? itemType)
	{
		if (!Plugin.LevelUpTooltipsEnabled)
			return;
		if (!IsLevelUpViewActive())
		{
			ShowItemPopup(anchor, weaponType, itemType);
			return;
		}
		if (!levelUpHoverUnlocked)
		{
			Plugin.Dbg("LevelUp hover ignored (mouse not moved since open)");
			return;
		}
		levelUpPendingAnchor = anchor;
		levelUpPendingWeapon = weaponType;
		levelUpPendingItem = itemType;
		levelUpPendingSince = Time.unscaledTime;
	}

	private static void CancelLevelUpHoverIfMatch(Transform anchor)
	{
		if ((Object)(object)levelUpPendingAnchor != (Object)null
			&& (Object)(object)anchor != (Object)null
			&& ((Object)levelUpPendingAnchor).GetInstanceID() == ((Object)anchor).GetInstanceID())
		{
			ClearLevelUpPendingHover();
		}
		// Always hide when leaving a Level Up icon if no popup hover
		if (IsLevelUpViewActive() && mouseOverPopupIndex < 0)
		{
			DelayFrames(5, () =>
			{
				if (mouseOverPopupIndex < 0 && IsLevelUpViewActive()
					&& ((Object)(object)levelUpPendingAnchor == (Object)null
						|| !IsPointerOverObject(levelUpPendingAnchor.gameObject)))
				{
					HideAllPopups();
				}
			});
		}
	}

	private static void UpdateLevelUpPendingHover()
	{
		if (!IsLevelUpViewActive())
		{
			if (levelUpPendingSince >= 0f)
				ClearLevelUpPendingHover();
			return;
		}
		if (!levelUpHoverUnlocked || levelUpPendingSince < 0f || (Object)(object)levelUpPendingAnchor == (Object)null)
			return;
		if (!IsPointerOverObject(levelUpPendingAnchor.gameObject))
		{
			ClearLevelUpPendingHover();
			return;
		}
		if (Time.unscaledTime - levelUpPendingSince < LevelUpHoverDelay)
			return;
		// Confirmed dwell on icon after mouse move
		Transform a = levelUpPendingAnchor;
		WeaponType? w = levelUpPendingWeapon;
		ItemType? it = levelUpPendingItem;
		ClearLevelUpPendingHover();
		ShowItemPopupForced(a, w, it);
	}

	/// <summary>Show popup bypassing Level Up request queue (already validated).</summary>
	private static void ShowItemPopupForced(Transform anchor, WeaponType? weaponType, ItemType? itemType)
	{
		// Temporarily clear suppress so ShowItemPopup proceeds
		float saved = suppressUnsolicitedPopupUntil;
		suppressUnsolicitedPopupUntil = 0f;
		bool unlocked = levelUpHoverUnlocked;
		levelUpHoverUnlocked = true;
		try
		{
			ShowItemPopup(anchor, weaponType, itemType);
		}
		finally
		{
			suppressUnsolicitedPopupUntil = saved;
			levelUpHoverUnlocked = unlocked;
		}
	}

	private static bool IsLevelUpViewActive()
	{
		if ((Object)(object)levelUpView == (Object)null)
		{
			levelUpView = GameObject.Find("GAME UI/Canvas - Game UI/Safe Area/View - Level Up");
		}
		return (Object)(object)levelUpView != (Object)null && levelUpView.activeInHierarchy;
	}

	/// <summary>True if screen point is over this UI object (or a child).</summary>
	private static bool IsPointerOverObject(GameObject go)
	{
		if ((Object)(object)go == (Object)null) return false;
		try
		{
			RectTransform rt = go.GetComponent<RectTransform>();
			if ((Object)(object)rt == (Object)null)
				rt = go.GetComponentInChildren<RectTransform>();
			if ((Object)(object)rt == (Object)null) return false;
			Vector2 mouse = (Vector2)Input.mousePosition;
			Camera cam = null;
			var canvas = go.GetComponentInParent<Canvas>();
			if ((Object)(object)canvas != (Object)null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
				cam = canvas.worldCamera != null ? canvas.worldCamera : Camera.main;
			return RectTransformUtility.RectangleContainsScreenPoint(rt, mouse, cam)
				|| RectTransformUtility.RectangleContainsScreenPoint(rt, mouse, null)
				|| RectTransformUtility.RectangleContainsScreenPoint(rt, mouse, Camera.main);
		}
		catch
		{
			return false;
		}
	}

	private static void UpdateControllerDwell()
	{
		if (interactiveMode)
		{
			return;
		}
		EventSystem current = EventSystem.current;
		if ((Object)(object)current == (Object)null)
		{
			return;
		}
		GameObject currentSelectedGameObject = current.currentSelectedGameObject;
		if ((Object)(object)currentSelectedGameObject == (Object)null)
		{
			dwellTarget = null;
		}
		else if ((Object)(object)currentSelectedGameObject != (Object)(object)dwellTarget)
		{
			dwellTarget = currentSelectedGameObject;
			dwellStartTime = Time.unscaledTime;
			if (!passivePopupShown || interactiveMode)
			{
				return;
			}
			bool flag = false;
			foreach (GameObject item in popupStack)
			{
				if ((Object)(object)item != (Object)null && currentSelectedGameObject.transform.IsChildOf(item.transform))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				HideAllPopups();
				passivePopupShown = false;
			}
		}
		else
		{
			if (passivePopupShown)
			{
				return;
			}
			float num = Time.unscaledTime - dwellStartTime;
			if (!(num < DwellDelay))
			{
				// Level Up: never dwell-show for mouse-only (auto-select first card).
				// Controller: only after real nav input (usingController set by HasControllerNavInput).
				if (IsLevelUpViewActive() && !usingController)
					return;
				if (IsLevelUpViewActive() && usingController && !IsPointerOverObject(currentSelectedGameObject) && !HasControllerNavInput())
					return;
				(WeaponType?, ItemType?)? tuple = FindTrackedIconForObject(currentSelectedGameObject);
				if (tuple.HasValue)
				{
					preDwellSelection = currentSelectedGameObject;
					ShowItemPopup(currentSelectedGameObject.transform, tuple.Value.Item1, tuple.Value.Item2);
					passivePopupShown = true;
				}
			}
		}
	}

	private static void UpdateEquipmentNavMode()
	{
		if (!equipmentNavMode || interactiveMode)
		{
			return;
		}
		EventSystem current = EventSystem.current;
		if ((Object)(object)current == (Object)null)
		{
			return;
		}
		GameObject currentSelectedGameObject = current.currentSelectedGameObject;
		if ((Object)(object)currentSelectedGameObject == (Object)null)
		{
			dwellTarget = null;
			return;
		}
		for (int i = 0; i < equipmentIcons.Count; i++)
		{
			if ((Object)(object)equipmentIcons[i] != (Object)null && ((Object)(object)currentSelectedGameObject == (Object)(object)equipmentIcons[i] || currentSelectedGameObject.transform.IsChildOf(equipmentIcons[i].transform)))
			{
				if (i != currentEquipmentIndex)
				{
					currentEquipmentIndex = i;
					SetEquipmentHighlight(i);
				}
				break;
			}
		}
		if ((Object)(object)currentSelectedGameObject != (Object)(object)dwellTarget)
		{
			dwellTarget = currentSelectedGameObject;
			dwellStartTime = Time.unscaledTime;
			if (passivePopupShown)
			{
				HideAllPopups();
				passivePopupShown = false;
			}
		}
		else
		{
			if (passivePopupShown)
			{
				return;
			}
			float num = Time.unscaledTime - dwellStartTime;
			if (!(num < DwellDelay))
			{
				(WeaponType?, ItemType?)? tuple = FindTrackedIconForObject(currentSelectedGameObject);
				if (tuple.HasValue)
				{
					ShowItemPopup(currentSelectedGameObject.transform, tuple.Value.Item1, tuple.Value.Item2);
					passivePopupShown = true;
				}
			}
		}
	}

	private static void UpdateControllerCollectionDwell()
	{
		if (interactiveMode)
		{
			return;
		}
		EventSystem current = EventSystem.current;
		if ((Object)(object)current == (Object)null)
		{
			return;
		}
		GameObject currentSelectedGameObject = current.currentSelectedGameObject;
		if ((Object)(object)currentSelectedGameObject == (Object)null)
		{
			dwellTarget = null;
			return;
		}
		if ((Object)(object)currentSelectedGameObject != (Object)(object)dwellTarget)
		{
			dwellTarget = currentSelectedGameObject;
			dwellStartTime = Time.unscaledTime;
			passivePopupShown = false;
			return;
		}
		float num = Time.unscaledTime - dwellStartTime;
		if (num < DwellDelay || passivePopupShown)
		{
			return;
		}
		int instanceID = ((Object)currentSelectedGameObject).GetInstanceID();
		if (collectionIcons.TryGetValue(instanceID, out (GameObject, WeaponType?, ItemType?, object) value))
		{
			preDwellSelection = currentSelectedGameObject;
			ShowCollectionPopup(value.Item2, value.Item3, value.Item4);
			passivePopupShown = true;
			return;
		}
		Transform parent = currentSelectedGameObject.transform.parent;
		while ((Object)(object)parent != (Object)null)
		{
			int instanceID2 = ((Object)((Component)parent).gameObject).GetInstanceID();
			if (collectionIcons.TryGetValue(instanceID2, out (GameObject, WeaponType?, ItemType?, object) value2))
			{
				preDwellSelection = currentSelectedGameObject;
				ShowCollectionPopup(value2.Item2, value2.Item3, value2.Item4);
				passivePopupShown = true;
				break;
			}
			parent = parent.parent;
		}
	}

	private static (WeaponType? weapon, ItemType? item)? FindTrackedIconForObject(GameObject go)
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)go == (Object)null)
		{
			return null;
		}
		int instanceID = ((Object)go).GetInstanceID();
		if (trackedIcons.TryGetValue(instanceID, out var value))
		{
			return (value.WeaponType, value.ItemType);
		}
		if (uiToWeaponType.TryGetValue(instanceID, out var value2))
		{
			return (value2, null);
		}
		if (uiToItemType.TryGetValue(instanceID, out var value3))
		{
			return (null, value3);
		}
		Transform parent = go.transform.parent;
		while ((Object)(object)parent != (Object)null)
		{
			int instanceID2 = ((Object)((Component)parent).gameObject).GetInstanceID();
			if (trackedIcons.TryGetValue(instanceID2, out var value4))
			{
				return (value4.WeaponType, value4.ItemType);
			}
			if (uiToWeaponType.TryGetValue(instanceID2, out var value5))
			{
				return (value5, null);
			}
			if (uiToItemType.TryGetValue(instanceID2, out var value6))
			{
				return (null, value6);
			}
			parent = parent.parent;
		}
		return null;
	}

	private static bool IsInteractButtonPressed()
	{
		return Input.GetKeyDown((KeyCode)9) || Input.GetKeyDown((KeyCode)333);
	}

	private static bool IsBackButtonPressed()
	{
		return Input.GetKeyDown((KeyCode)8) || Input.GetKeyDown((KeyCode)331);
	}

	private static bool IsSubmitButtonPressed()
	{
		return Input.GetKeyDown((KeyCode)32) || Input.GetKeyDown((KeyCode)13) || Input.GetKeyDown((KeyCode)330);
	}

	private static void SetupFormulaNavigation()
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Expected O, but got Unknown
		if (formulaIcons.Count == 0)
		{
			return;
		}
		List<List<int>> list = new List<List<int>>();
		List<float> list2 = new List<float>();
		for (int i = 0; i < formulaIcons.Count; i++)
		{
			if ((Object)(object)formulaIcons[i] == (Object)null)
			{
				continue;
			}
			float y = formulaIcons[i].transform.localPosition.y;
			int num = -1;
			for (int j = 0; j < list2.Count; j++)
			{
				if (Mathf.Abs(y - list2[j]) < 5f)
				{
					num = j;
					break;
				}
			}
			if (num < 0)
			{
				num = list.Count;
				list.Add(new List<int>());
				list2.Add(y);
			}
			list[num].Add(i);
		}
		List<(float, List<int>)> list3 = new List<(float, List<int>)>();
		for (int k = 0; k < list.Count; k++)
		{
			list3.Add((list2[k], list[k]));
		}
		list3.Sort(((float y, List<int> indices) a, (float y, List<int> indices) b) => b.y.CompareTo(a.y));
		foreach (var item5 in list3)
		{
			item5.Item2.Sort((int a, int b) => formulaIcons[a].transform.localPosition.x.CompareTo(formulaIcons[b].transform.localPosition.x));
		}
		Dictionary<int, (int, int)> dictionary = new Dictionary<int, (int, int)>();
		for (int num2 = 0; num2 < list3.Count; num2++)
		{
			for (int num3 = 0; num3 < list3[num2].Item2.Count; num3++)
			{
				dictionary[list3[num2].Item2[num3]] = (num2, num3);
			}
		}
		for (int num4 = 0; num4 < formulaIcons.Count; num4++)
		{
			Button component = formulaIcons[num4].GetComponent<Button>();
			if ((Object)(object)component == (Object)null || !dictionary.ContainsKey(num4))
			{
				continue;
			}
			(int, int) tuple = dictionary[num4];
			int item = tuple.Item1;
			int item2 = tuple.Item2;
			Navigation val = new Navigation();
			val.mode = (Navigation.Mode)4;
			if (item2 > 0)
			{
				int index = list3[item].Item2[item2 - 1];
				Selectable component2 = formulaIcons[index].GetComponent<Selectable>();
				if ((Object)(object)component2 != (Object)null)
				{
					val.selectOnLeft = component2;
				}
			}
			if (item2 < list3[item].Item2.Count - 1)
			{
				int index2 = list3[item].Item2[item2 + 1];
				Selectable component3 = formulaIcons[index2].GetComponent<Selectable>();
				if ((Object)(object)component3 != (Object)null)
				{
					val.selectOnRight = component3;
				}
			}
			if (item > 0)
			{
				List<int> item3 = list3[item - 1].Item2;
				int index3 = Math.Min(item2, item3.Count - 1);
				Selectable component4 = formulaIcons[item3[index3]].GetComponent<Selectable>();
				if ((Object)(object)component4 != (Object)null)
				{
					val.selectOnUp = component4;
				}
			}
			if (item < list3.Count - 1)
			{
				List<int> item4 = list3[item + 1].Item2;
				int index4 = Math.Min(item2, item4.Count - 1);
				Selectable component5 = formulaIcons[item4[index4]].GetComponent<Selectable>();
				if ((Object)(object)component5 != (Object)null)
				{
					val.selectOnDown = component5;
				}
			}
			((Selectable)component).navigation = val;
		}
	}

	private static void EnterInteractiveMode()
	{
		GameObject val = null;
		if (popupStack.Count > 0)
		{
			val = popupStack[popupStack.Count - 1];
		}
		else if ((Object)(object)collectionPopup != (Object)null)
		{
			val = collectionPopup;
		}
		if ((Object)(object)val == (Object)null)
		{
			return;
		}
		interactiveMode = true;
		interactivePopup = val;
		CollectFormulaIcons(val);
		if (formulaIcons.Count > 0)
		{
			SetupFormulaNavigation();
			currentFormulaIndex = 0;
			EventSystem current = EventSystem.current;
			if ((Object)(object)current != (Object)null)
			{
				current.SetSelectedGameObject(formulaIcons[0]);
			}
			SetFormulaHighlight(0);
			HideNavigatorArrows();
		}
		else
		{
			interactiveMode = false;
			interactivePopup = null;
		}
	}

	private static void ExitInteractiveMode()
	{
		foreach (GameObject formulaIcon in formulaIcons)
		{
			if (!((Object)(object)formulaIcon == (Object)null))
			{
				Button component = formulaIcon.GetComponent<Button>();
				if ((Object)(object)component != (Object)null)
				{
					Navigation navigation = ((Selectable)component).navigation;
					navigation.mode = (Navigation.Mode)0;
					((Selectable)component).navigation = navigation;
				}
			}
		}
		interactiveMode = false;
		interactivePopup = null;
		currentFormulaIndex = -1;
		formulaIcons.Clear();
		if ((Object)(object)interactiveHighlight != (Object)null)
		{
			Object.Destroy((Object)(object)interactiveHighlight);
			interactiveHighlight = null;
		}
		ShowNavigatorArrows();
		if ((Object)(object)preDwellSelection != (Object)null)
		{
			EventSystem current2 = EventSystem.current;
			if ((Object)(object)current2 != (Object)null)
			{
				current2.SetSelectedGameObject(preDwellSelection);
			}
		}
	}

	private static void EnterEquipmentNavMode()
	{
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)pauseView == (Object)null || !pauseView.activeInHierarchy)
		{
			return;
		}
		Transform val = FindChildRecursive(pauseView.transform, "EquipmentPanel");
		if ((Object)(object)val == (Object)null)
		{
			return;
		}
		Transform panel = val.Find("WeaponsPanel");
		Transform panel2 = val.Find("AccessoryPanel");
		equipmentIcons.Clear();
		CollectEquipmentIcons(panel);
		int count = equipmentIcons.Count;
		CollectEquipmentIcons(panel2);
		if (equipmentIcons.Count == 0)
		{
			return;
		}
		foreach (GameObject equipmentIcon in equipmentIcons)
		{
			if ((Object)(object)equipmentIcon.GetComponent<Button>() == (Object)null)
			{
				Button val2 = equipmentIcon.AddComponent<Button>();
				ColorBlock colors = ((Selectable)val2).colors;
				colors.normalColor = new Color(1f, 1f, 1f, 0f);
				colors.highlightedColor = new Color(1f, 1f, 1f, 0f);
				colors.pressedColor = new Color(1f, 1f, 1f, 0f);
				colors.selectedColor = new Color(1f, 1f, 1f, 0f);
				((Selectable)val2).colors = colors;
			}
		}
		SetupEquipmentNavigation(count);
		EventSystem current2 = EventSystem.current;
		if ((Object)(object)current2 != (Object)null)
		{
			preDwellSelection = current2.currentSelectedGameObject;
		}
		equipmentNavMode = true;
		currentEquipmentIndex = 0;
		dwellTarget = null;
		dwellStartTime = 0f;
		passivePopupShown = false;
		if ((Object)(object)current2 != (Object)null)
		{
			current2.SetSelectedGameObject(equipmentIcons[0]);
		}
		SetEquipmentHighlight(0);
		HideNavigatorArrows();
	}

	private static void CollectEquipmentIcons(Transform panel)
	{
		if ((Object)(object)panel == (Object)null)
		{
			return;
		}
		for (int i = 0; i < panel.childCount; i++)
		{
			Transform child = panel.GetChild(i);
			if (((Object)child).name.Contains("EquipmentIconPause") && ((Component)child).gameObject.activeInHierarchy && FindTrackedIconForObject(((Component)child).gameObject).HasValue)
			{
				equipmentIcons.Add(((Component)child).gameObject);
			}
		}
	}

	private static void SetupEquipmentNavigation(int weaponCount)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		if (equipmentIcons.Count == 0)
		{
			return;
		}
		int num = equipmentIcons.Count - weaponCount;
		for (int i = 0; i < equipmentIcons.Count; i++)
		{
			Button component = equipmentIcons[i].GetComponent<Button>();
			if ((Object)(object)component == (Object)null)
			{
				continue;
			}
			Navigation val = new Navigation();
			val.mode = (Navigation.Mode)4;
			bool flag = i < weaponCount;
			int num2 = ((!flag) ? weaponCount : 0);
			int num3 = (flag ? weaponCount : equipmentIcons.Count);
			int num4 = i - num2;
			int num5 = num3 - num2;
			if (num4 > 0)
			{
				Selectable component2 = equipmentIcons[i - 1].GetComponent<Selectable>();
				if ((Object)(object)component2 != (Object)null)
				{
					val.selectOnLeft = component2;
				}
			}
			if (num4 < num5 - 1)
			{
				Selectable component3 = equipmentIcons[i + 1].GetComponent<Selectable>();
				if ((Object)(object)component3 != (Object)null)
				{
					val.selectOnRight = component3;
				}
			}
			if (flag && num > 0)
			{
				int index = weaponCount + Math.Min(num4, num - 1);
				Selectable component4 = equipmentIcons[index].GetComponent<Selectable>();
				if ((Object)(object)component4 != (Object)null)
				{
					val.selectOnDown = component4;
				}
			}
			else if (!flag && weaponCount > 0)
			{
				int index2 = Math.Min(num4, weaponCount - 1);
				Selectable component5 = equipmentIcons[index2].GetComponent<Selectable>();
				if ((Object)(object)component5 != (Object)null)
				{
					val.selectOnUp = component5;
				}
			}
			((Selectable)component).navigation = val;
		}
	}

	private static void ExitEquipmentNavMode()
	{
		equipmentNavMode = false;
		currentEquipmentIndex = -1;
		dwellTarget = null;
		if ((Object)(object)equipmentHighlight != (Object)null)
		{
			Object.Destroy((Object)(object)equipmentHighlight);
			equipmentHighlight = null;
		}
		foreach (GameObject equipmentIcon in equipmentIcons)
		{
			if (!((Object)(object)equipmentIcon == (Object)null))
			{
				Button component = equipmentIcon.GetComponent<Button>();
				if ((Object)(object)component != (Object)null)
				{
					Navigation navigation = ((Selectable)component).navigation;
					navigation.mode = (Navigation.Mode)0;
					((Selectable)component).navigation = navigation;
				}
			}
		}
		equipmentIcons.Clear();
		if (passivePopupShown)
		{
			HideAllPopups();
			passivePopupShown = false;
		}
		ShowNavigatorArrows();
		if ((Object)(object)preDwellSelection != (Object)null)
		{
			EventSystem current2 = EventSystem.current;
			if ((Object)(object)current2 != (Object)null)
			{
				current2.SetSelectedGameObject(preDwellSelection);
			}
		}
	}

	private static void CollectFormulaIcons(GameObject popup)
	{
		formulaIcons.Clear();
		Il2CppArrayBase<EventTrigger> componentsInChildren = popup.GetComponentsInChildren<EventTrigger>(false);
		foreach (EventTrigger item in componentsInChildren)
		{
			if (!((Object)(object)((Component)item).gameObject == (Object)(object)popup))
			{
				int instanceID = ((Object)((Component)item).gameObject).GetInstanceID();
				if (formulaIconData.ContainsKey(instanceID))
				{
					formulaIcons.Add(((Component)item).gameObject);
				}
			}
		}
	}

	private static void SetFormulaHighlight(int index)
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)interactiveHighlight != (Object)null)
		{
			Object.Destroy((Object)(object)interactiveHighlight);
			interactiveHighlight = null;
		}
		if (index >= 0 && index < formulaIcons.Count)
		{
			GameObject val = formulaIcons[index];
			if (!((Object)(object)val == (Object)null))
			{
				interactiveHighlight = new GameObject("ControllerHighlight");
				interactiveHighlight.transform.SetParent(val.transform, false);
				RectTransform val2 = interactiveHighlight.AddComponent<RectTransform>();
				val2.anchorMin = Vector2.zero;
				val2.anchorMax = Vector2.one;
				val2.offsetMin = new Vector2(-3f, -3f);
				val2.offsetMax = new Vector2(3f, 3f);
				Image val3 = interactiveHighlight.AddComponent<Image>();
				((Graphic)val3).color = new Color(0f, 0.9f, 1f, 0.25f);
				((Graphic)val3).raycastTarget = false;
				Outline val4 = interactiveHighlight.AddComponent<Outline>();
				((Shadow)val4).effectColor = new Color(0f, 0.9f, 1f, 1f);
				((Shadow)val4).effectDistance = new Vector2(2f, 2f);
			}
		}
	}

	private static void SetEquipmentHighlight(int index)
	{
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)equipmentHighlight != (Object)null)
		{
			Object.Destroy((Object)(object)equipmentHighlight);
			equipmentHighlight = null;
		}
		if (index < 0 || index >= equipmentIcons.Count)
		{
			return;
		}
		GameObject val = equipmentIcons[index];
		if ((Object)(object)val == (Object)null)
		{
			return;
		}
		Transform transform = val.transform;
		float num = 48f;
		Il2CppArrayBase<Image> componentsInChildren = val.GetComponentsInChildren<Image>(false);
		foreach (Image item in componentsInChildren)
		{
			if ((Object)(object)((Component)item).gameObject == (Object)(object)val)
			{
				continue;
			}
			RectTransform component = ((Component)item).GetComponent<RectTransform>();
			if ((Object)(object)component != (Object)null)
			{
				Rect rect = component.rect;
				float width = rect.width;
				rect = component.rect;
				float num2 = Mathf.Max(width, rect.height);
				if (num2 > num)
				{
					num = num2;
					transform = ((Component)item).transform;
				}
			}
		}
		equipmentHighlight = new GameObject("EquipmentHighlight");
		equipmentHighlight.transform.SetParent(transform, false);
		RectTransform val2 = equipmentHighlight.AddComponent<RectTransform>();
		val2.anchorMin = new Vector2(0.5f, 0.5f);
		val2.anchorMax = new Vector2(0.5f, 0.5f);
		val2.pivot = new Vector2(0.5f, 0.5f);
		val2.sizeDelta = new Vector2(num + 6f, num + 6f);
		Image val3 = equipmentHighlight.AddComponent<Image>();
		((Graphic)val3).color = new Color(0f, 0.9f, 1f, 0.25f);
		((Graphic)val3).raycastTarget = false;
		Outline val4 = equipmentHighlight.AddComponent<Outline>();
		((Shadow)val4).effectColor = new Color(0f, 0.9f, 1f, 1f);
		((Shadow)val4).effectDistance = new Vector2(2f, 2f);
	}

	private static void UpdateInteractiveMode()
	{
		if (!interactiveMode)
		{
			return;
		}
		EventSystem current = EventSystem.current;
		if ((Object)(object)current == (Object)null)
		{
			return;
		}
		GameObject currentSelectedGameObject = current.currentSelectedGameObject;
		bool flag = formulaIcons.Count == 0;
		if (!flag && (Object)(object)formulaIcons[0] == (Object)null)
		{
			flag = true;
		}
		if (flag)
		{
			GameObject val = null;
			if (popupStack.Count > 0)
			{
				val = popupStack[popupStack.Count - 1];
			}
			else if ((Object)(object)collectionPopup != (Object)null)
			{
				val = collectionPopup;
			}
			if ((Object)(object)val != (Object)null)
			{
				CollectFormulaIcons(val);
				if (formulaIcons.Count > 0)
				{
					SetupFormulaNavigation();
					currentFormulaIndex = 0;
					current.SetSelectedGameObject(formulaIcons[0]);
					SetFormulaHighlight(0);
					return;
				}
			}
			ExitInteractiveMode();
			return;
		}
		GameObject val2 = null;
		if (popupStack.Count > 0)
		{
			val2 = popupStack[popupStack.Count - 1];
		}
		else if ((Object)(object)collectionPopup != (Object)null)
		{
			val2 = collectionPopup;
		}
		if ((Object)(object)val2 != (Object)null && (Object)(object)val2 != (Object)(object)interactivePopup)
		{
			foreach (GameObject formulaIcon in formulaIcons)
			{
				if (!((Object)(object)formulaIcon == (Object)null))
				{
					Button component = formulaIcon.GetComponent<Button>();
					if ((Object)(object)component != (Object)null)
					{
						Navigation navigation = ((Selectable)component).navigation;
						navigation.mode = (Navigation.Mode)0;
						((Selectable)component).navigation = navigation;
					}
				}
			}
			if ((Object)(object)interactiveHighlight != (Object)null)
			{
				Object.Destroy((Object)(object)interactiveHighlight);
				interactiveHighlight = null;
			}
			interactivePopup = val2;
			CollectFormulaIcons(val2);
			if (formulaIcons.Count > 0)
			{
				SetupFormulaNavigation();
				currentFormulaIndex = 0;
				current.SetSelectedGameObject(formulaIcons[0]);
				SetFormulaHighlight(0);
			}
			else
			{
				ExitInteractiveMode();
			}
			return;
		}
		if ((Object)(object)currentSelectedGameObject == (Object)null)
		{
			ExitInteractiveMode();
			return;
		}
		for (int i = 0; i < formulaIcons.Count; i++)
		{
			if ((Object)(object)formulaIcons[i] != (Object)null && (Object)(object)currentSelectedGameObject == (Object)(object)formulaIcons[i])
			{
				if (i != currentFormulaIndex)
				{
					currentFormulaIndex = i;
					SetFormulaHighlight(i);
				}
				return;
			}
			if ((Object)(object)formulaIcons[i] != (Object)null && currentSelectedGameObject.transform.IsChildOf(formulaIcons[i].transform))
			{
				if (i != currentFormulaIndex)
				{
					currentFormulaIndex = i;
					SetFormulaHighlight(i);
				}
				return;
			}
		}
		bool flag2 = false;
		foreach (GameObject item in popupStack)
		{
			if ((Object)(object)item != (Object)null && currentSelectedGameObject.transform.IsChildOf(item.transform))
			{
				flag2 = true;
				break;
			}
		}
		if (!flag2 && (Object)(object)collectionPopup != (Object)null && currentSelectedGameObject.transform.IsChildOf(collectionPopup.transform))
		{
			flag2 = true;
		}
		if (!flag2)
		{
			ExitInteractiveMode();
		}
	}

	private static void HandleBackButton(bool force = false)
	{
		if (!force && !IsBackButtonPressed())
		{
			return;
		}
		if ((Object)(object)collectionPopup != (Object)null && popupStack.Count == 0)
		{
			if (interactiveMode)
			{
				ExitInteractiveMode();
			}
			HideCollectionPopup();
			passivePopupShown = false;
			collectionPopupBackStack.Clear();
			if ((Object)(object)preDwellSelection != (Object)null)
			{
				EventSystem current = EventSystem.current;
				if ((Object)(object)current != (Object)null)
				{
					current.SetSelectedGameObject(preDwellSelection);
				}
			}
		}
		else if (equipmentNavMode)
		{
			if (interactiveMode)
			{
				if (popupStack.Count > 1)
				{
					ExitInteractiveMode();
					HideTopPopup();
					EnterInteractiveMode();
					return;
				}
				ExitInteractiveMode();
				passivePopupShown = true;
				if ((Object)(object)dwellTarget != (Object)null)
				{
					EventSystem current2 = EventSystem.current;
					if ((Object)(object)current2 != (Object)null)
					{
						current2.SetSelectedGameObject(dwellTarget);
					}
				}
			}
			else if (passivePopupShown)
			{
				HideAllPopups();
				passivePopupShown = false;
			}
			else
			{
				ExitEquipmentNavMode();
			}
		}
		else if (interactiveMode)
		{
			if (popupStack.Count > 1)
			{
				ExitInteractiveMode();
				HideTopPopup();
				EnterInteractiveMode();
			}
			else
			{
				ExitInteractiveMode();
				passivePopupShown = true;
			}
		}
		else
		{
			if (!passivePopupShown || popupStack.Count <= 0)
			{
				return;
			}
			HideAllPopups();
			passivePopupShown = false;
			if ((Object)(object)preDwellSelection != (Object)null)
			{
				EventSystem current3 = EventSystem.current;
				if ((Object)(object)current3 != (Object)null)
				{
					current3.SetSelectedGameObject(preDwellSelection);
				}
			}
		}
	}

	private static void ResetControllerState()
	{
		usingController = false;
		dwellTarget = null;
		passivePopupShown = false;
		interactiveMode = false;
		interactivePopup = null;
		formulaIcons.Clear();
		currentFormulaIndex = -1;
		preDwellSelection = null;
		lastSelectedObject = null;
		collectionPopupBackStack.Clear();
		equipmentNavMode = false;
		equipmentIcons.Clear();
		currentEquipmentIndex = -1;
		ShowNavigatorArrows();
		if ((Object)(object)interactiveHighlight != (Object)null)
		{
			Object.Destroy((Object)(object)interactiveHighlight);
			interactiveHighlight = null;
		}
		if ((Object)(object)equipmentHighlight != (Object)null)
		{
			Object.Destroy((Object)(object)equipmentHighlight);
			equipmentHighlight = null;
		}
	}

	private static void HideNavigatorArrows()
	{
		if ((Object)(object)cachedNavigatorArrows == (Object)null)
		{
			cachedNavigatorArrows = GameObject.Find("GAME UI/Canvas - Game UI/Safe Area/Navigators/ButtonNavigator");
		}
		if ((Object)(object)cachedNavigatorArrows != (Object)null)
		{
			cachedNavigatorArrows.SetActive(false);
		}
	}

	private static void ShowNavigatorArrows()
	{
		if ((Object)(object)cachedNavigatorArrows != (Object)null)
		{
			cachedNavigatorArrows.SetActive(true);
		}
	}

	public static void ShowItemPopup(Transform anchor, WeaponType? weaponType, ItemType? itemType)
	{
		Plugin.Dbg($"ShowItemPopup weapon={weaponType} item={itemType} gameDataReady={GameData.IsReady}");
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		if ((weaponType.HasValue && ((object)weaponType.Value/*cast due to constrained. prefix*/).ToString() == "DEFANG") || (itemType.HasValue && ((object)itemType.Value/*cast due to constrained. prefix*/).ToString() == "DEFANG"))
		{
			return;
		}
		// Level Up: never show unless user moved mouse after open AND pointer is on the icon
		if (IsLevelUpViewActive())
		{
			if (!levelUpHoverUnlocked)
			{
				Plugin.Dbg("ShowItemPopup blocked: Level Up hover not unlocked (move mouse first)");
				return;
			}
			if ((Object)(object)anchor == (Object)null || !IsPointerOverObject(anchor.gameObject))
			{
				Plugin.Dbg("ShowItemPopup blocked: pointer not over Level Up icon");
				return;
			}
		}
		else if (Time.unscaledTime < suppressUnsolicitedPopupUntil)
		{
			if ((Object)(object)anchor == (Object)null || !IsPointerOverObject(anchor.gameObject))
			{
				Plugin.Dbg("ShowItemPopup suppressed (grace / pointer not over icon)");
				return;
			}
		}
		if (!IsGamePaused())
		{
			if (collectionIcons.Count > 0 && (weaponType.HasValue || itemType.HasValue))
			{
				currentCollectionHoverId = -1;
				pendingCollectionHoverId = -1;
				ShowCollectionPopup(weaponType, itemType);
			}
			else
			{
				HideAllPopups();
			}
			return;
		}
		int num = ((anchor != null) ? ((Object)anchor).GetInstanceID() : 0);
		for (int i = 0; i < popupAnchorIds.Count; i++)
		{
			if (popupAnchorIds[i] == num)
			{
				return;
			}
		}
		int num2 = -1;
		for (int j = 0; j < popupStack.Count; j++)
		{
			if ((Object)(object)popupStack[j] != (Object)null && (Object)(object)anchor != (Object)null && anchor.IsChildOf(popupStack[j].transform))
			{
				num2 = j;
			}
		}
		if (num2 >= 0)
		{
			while (popupStack.Count > num2 + 1)
			{
				HideTopPopup();
			}
		}
		else if (popupStack.Count > 0)
		{
			HideAllPopups();
		}
		Transform val = FindPopupParent(anchor);
		if (!((Object)(object)val == (Object)null))
		{
			GameObject val2 = CreatePopup(val, weaponType, itemType);
			if (!((Object)(object)val2 == (Object)null))
			{
				popupStack.Add(val2);
				popupAnchorIds.Add(num);
				PositionPopup(val2, anchor);
				AddPopupHoverTracking(val2);
			}
		}
	}

	private static GameObject CreatePopup(Transform parent, WeaponType? weaponType, ItemType? itemType)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0722: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Expected O, but got Unknown
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Expected O, but got Unknown
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Unknown result type (might be due to invalid IL or missing references)
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0461: Expected O, but got Unknown
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_053b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0573: Unknown result type (might be due to invalid IL or missing references)
		//IL_0600: Unknown result type (might be due to invalid IL or missing references)
		//IL_0626: Unknown result type (might be due to invalid IL or missing references)
		//IL_062b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_064a: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0666: Unknown result type (might be due to invalid IL or missing references)
		//IL_068d: Unknown result type (might be due to invalid IL or missing references)
		GameObject val = new GameObject("ItemTooltipPopup");
		val.transform.SetParent(parent, false);
		RectTransform val2 = val.AddComponent<RectTransform>();
		val2.anchorMin = new Vector2(0.5f, 0.5f);
		val2.anchorMax = new Vector2(0.5f, 0.5f);
		val2.pivot = new Vector2(0f, 1f);
		Image val3 = val.AddComponent<Image>();
		((Graphic)val3).color = PopupBgColor;
		((Graphic)val3).raycastTarget = true;
		Outline val4 = val.AddComponent<Outline>();
		((Shadow)val4).effectColor = PopupBorderColor;
		((Shadow)val4).effectDistance = new Vector2(2f, 2f);
		float num = 0f - Padding;
		float num2 = 420f;
		string text = weaponType.HasValue ? GameData.GetWeaponName(weaponType.Value) : (itemType.HasValue ? itemType.Value.ToString() : "Unknown");
		string text2 = "";
		Sprite val5 = null;
		if (text.Contains("/"))
		{
			Object.Destroy((Object)(object)val);
			return null;
		}
		if (weaponType.HasValue)
		{
			WeaponData weaponData = GetWeaponData(weaponType.Value);
			if (weaponData != null)
			{
				text = GetLocalizedWeaponName(weaponData, weaponType.Value);
				text2 = GetLocalizedWeaponDescription(weaponData, weaponType.Value);
				val5 = GetSpriteForWeapon(weaponType.Value);
			}
		}
		else if (itemType.HasValue)
		{
			object powerUpData = GetPowerUpData(itemType.Value);
			if (powerUpData != null)
			{
				text = GetLocalizedPowerUpName(powerUpData, itemType.Value);
				text2 = GetLocalizedPowerUpDescription(powerUpData, itemType.Value);
				val5 = GetSpriteForItem(itemType.Value);
			}
		}
		TMP_FontAsset font = GetFont();
		if ((Object)(object)font != (Object)null)
		{
			float contentW = num2 - Padding * 2f;
			float headerIcon = IconSize;
			bool hasHeaderIcon = (Object)(object)val5 != (Object)null;

			// Title row: fixed icon + wrapping title (not stretch-fill)
			GameObject val6 = new GameObject("TitleRow");
			val6.transform.SetParent(val.transform, false);
			RectTransform val7 = val6.AddComponent<RectTransform>();
			val7.anchorMin = new Vector2(0f, 1f);
			val7.anchorMax = new Vector2(0f, 1f);
			val7.pivot = new Vector2(0f, 1f);
			val7.anchoredPosition = new Vector2(Padding, num);

			if (hasHeaderIcon)
			{
				GameObject val11 = new GameObject("HeaderIcon");
				val11.transform.SetParent(val6.transform, false);
				RectTransform val12 = val11.AddComponent<RectTransform>();
				val12.anchorMin = new Vector2(0f, 1f);
				val12.anchorMax = new Vector2(0f, 1f);
				val12.pivot = new Vector2(0f, 1f);
				val12.anchoredPosition = Vector2.zero;
				val12.sizeDelta = new Vector2(headerIcon, headerIcon);
				Image val13 = val11.AddComponent<Image>();
				val13.sprite = val5;
				val13.preserveAspect = true;
				((Graphic)val13).raycastTarget = false;
			}

			GameObject val8 = new GameObject("Title");
			val8.transform.SetParent(val6.transform, false);
			RectTransform val9 = val8.AddComponent<RectTransform>();
			val9.anchorMin = new Vector2(0f, 1f);
			val9.anchorMax = new Vector2(0f, 1f);
			val9.pivot = new Vector2(0f, 1f);
			float titleX = hasHeaderIcon ? headerIcon + Spacing : 0f;
			val9.anchoredPosition = new Vector2(titleX, 0f);
			val9.sizeDelta = new Vector2(contentW - titleX, headerIcon);
			TextMeshProUGUI val10 = val8.AddComponent<TextMeshProUGUI>();
			((TMP_Text)val10).font = font;
			((TMP_Text)val10).text = text;
			((TMP_Text)val10).fontSize = 20f;
			((TMP_Text)val10).fontStyle = (FontStyles)1;
			((Graphic)val10).color = Color.white;
			((TMP_Text)val10).alignment = (TextAlignmentOptions)257; // top-left
			((TMP_Text)val10).enableWordWrapping = true;
			((TMP_Text)val10).overflowMode = TextOverflowModes.Overflow;
			((Graphic)val10).raycastTarget = false;
			float titleH = FitTmpHeight(val10, contentW - titleX, 24f, 72f);
			float rowH = Mathf.Max(hasHeaderIcon ? headerIcon : 0f, titleH);
			val7.sizeDelta = new Vector2(contentW, rowH);
			num -= rowH + Spacing + 2f;

			if (!string.IsNullOrEmpty(text2))
			{
				GameObject val14 = new GameObject("Description");
				val14.transform.SetParent(val.transform, false);
				RectTransform val15 = val14.AddComponent<RectTransform>();
				val15.anchorMin = new Vector2(0f, 1f);
				val15.anchorMax = new Vector2(0f, 1f);
				val15.pivot = new Vector2(0f, 1f);
				val15.anchoredPosition = new Vector2(Padding, num);
				val15.sizeDelta = new Vector2(contentW, 24f);
				TextMeshProUGUI val16 = val14.AddComponent<TextMeshProUGUI>();
				((TMP_Text)val16).font = font;
				((TMP_Text)val16).text = text2;
				((TMP_Text)val16).fontSize = 14f;
				((Graphic)val16).color = new Color(0.85f, 0.85f, 0.9f, 1f);
				((TMP_Text)val16).alignment = (TextAlignmentOptions)257;
				((TMP_Text)val16).enableWordWrapping = true;
				((TMP_Text)val16).overflowMode = TextOverflowModes.Overflow;
				((Graphic)val16).raycastTarget = false;
				float num4 = FitTmpHeight(val16, contentW, 22f, 160f);
				num -= num4 + Spacing + 4f; // breathing room before Evolutions
			}
			if (weaponType.HasValue)
			{
				num = AddWeaponEvolutionSection(val.transform, font, weaponType.Value, num, num2);
			}
			else if (itemType.HasValue)
			{
				string value = ((object)itemType.Value/*cast due to constrained. prefix*/).ToString();
				bool flag = false;
				if (GameData.TryParseWeaponType(value, out WeaponType result) && GetWeaponData(result) != null)
				{
					num = AddWeaponEvolutionSection(val.transform, font, result, num, num2);
					flag = true;
				}
				if (!flag)
				{
					num = AddItemEvolutionSection(val.transform, font, itemType.Value, num, num2);
				}
			}
			List<ArcanaInfo> list = null;
			if (weaponType.HasValue)
			{
				list = GetActiveArcanasForWeapon(weaponType.Value);
			}
			else if (itemType.HasValue)
			{
				list = GetActiveArcanasForItem(itemType.Value);
			}
			if (list != null && list.Count > 0)
			{
				num = AddArcanaSection(val.transform, font, list, num, num2);
			}
		}
		num -= Padding;
		val2.sizeDelta = new Vector2(num2, 0f - num);
		try
		{
			Canvas.ForceUpdateCanvases();
			LayoutRebuilder.ForceRebuildLayoutImmediate(val2);
		}
		catch { }
		return val;
	}

	private static List<PassiveRequirement> CollectPassiveRequirements(Il2CppStructArray<WeaponType> evoSynergy, HashSet<int> requiresMaxTypes = null)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected I4, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected I4, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		List<PassiveRequirement> list = new List<PassiveRequirement>();
		if (evoSynergy == null)
		{
			return list;
		}
		for (int i = 0; i < ((Il2CppArrayBase<WeaponType>)(object)evoSynergy).Length; i++)
		{
			WeaponType val = ((Il2CppArrayBase<WeaponType>)(object)evoSynergy)[i];
			PassiveRequirement item = new PassiveRequirement
			{
				RequiresMaxLevel = (requiresMaxTypes?.Contains((int)val) ?? false)
			};
			WeaponData weaponData = GetWeaponData(val);
			if (weaponData != null)
			{
				item.WeaponType = val;
				item.Sprite = GetSpriteForWeapon(val);
				item.Owned = PlayerOwnsWeapon(val) || PlayerOwnsAccessory(val);
			}
			else
			{
				int num = (int)val;
				if (Enum.IsDefined(typeof(ItemType), num))
				{
					ItemType val2 = (ItemType)num;
					item.ItemType = val2;
					item.Sprite = GetSpriteForItem(val2);
					item.Owned = PlayerOwnsItem(val2);
				}
				else
				{
					item.WeaponType = val;
					item.Sprite = GetSpriteForWeapon(val);
					item.Owned = PlayerOwnsWeapon(val) || PlayerOwnsAccessory(val);
				}
			}
			list.Add(item);
		}
		return list;
	}

	private static HashSet<int> GetRequiresMaxFromEvolved(string evoInto)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Expected I4, but got Unknown
		if (string.IsNullOrEmpty(evoInto))
		{
			return null;
		}
		if (!GameData.TryParseWeaponType(evoInto, out WeaponType result))
		{
			return null;
		}
		WeaponData weaponData = GetWeaponData(result);
		if (weaponData == null)
		{
			return null;
		}
		try
		{
			PropertyInfo property = ((object)weaponData).GetType().GetProperty("requiresMax", BindingFlags.Instance | BindingFlags.Public);
			if (property == null)
			{
				return null;
			}
			object value = property.GetValue(weaponData);
			if (value == null)
			{
				return null;
			}
			PropertyInfo property2 = value.GetType().GetProperty("Count");
			if (property2 == null)
			{
				return null;
			}
			int num = (int)property2.GetValue(value);
			if (num == 0)
			{
				return null;
			}
			PropertyInfo property3 = value.GetType().GetProperty("Item");
			if (property3 == null)
			{
				return null;
			}
			HashSet<int> hashSet = new HashSet<int>();
			for (int i = 0; i < num; i++)
			{
				object value2 = property3.GetValue(value, new object[1] { i });
				if (value2 is WeaponType val)
				{
					hashSet.Add((int)val);
				}
				else if (value2 != null)
				{
					hashSet.Add((int)value2);
				}
			}
			return (hashSet.Count > 0) ? hashSet : null;
		}
		catch
		{
		}
		return null;
	}

	private unsafe static bool PlayerOwnsWeapon(WeaponType weaponType)
	{
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		if (cachedGameSession == null)
		{
			return false;
		}
		try
		{
			PropertyInfo property = cachedGameSession.GetType().GetProperty("ActiveCharacter", BindingFlags.Instance | BindingFlags.Public);
			if (property == null)
			{
				return false;
			}
			object value = property.GetValue(cachedGameSession);
			if (value == null)
			{
				return false;
			}
			PropertyInfo property2 = value.GetType().GetProperty("WeaponsManager", BindingFlags.Instance | BindingFlags.Public);
			if (property2 == null)
			{
				return false;
			}
			object value2 = property2.GetValue(value);
			if (value2 == null)
			{
				return false;
			}
			PropertyInfo property3 = value2.GetType().GetProperty("ActiveEquipment", BindingFlags.Instance | BindingFlags.Public);
			if (property3 == null)
			{
				return false;
			}
			object value3 = property3.GetValue(value2);
			if (value3 == null)
			{
				return false;
			}
			PropertyInfo property4 = value3.GetType().GetProperty("Count");
			PropertyInfo property5 = value3.GetType().GetProperty("Item");
			if (property4 == null || property5 == null)
			{
				return false;
			}
			int num = (int)property4.GetValue(value3);
			string text = ((object)(*(WeaponType*)(&weaponType))/*cast due to constrained. prefix*/).ToString();
			for (int i = 0; i < num; i++)
			{
				object value4 = property5.GetValue(value3, new object[1] { i });
				if (value4 == null)
				{
					continue;
				}
				PropertyInfo property6 = value4.GetType().GetProperty("Type", BindingFlags.Instance | BindingFlags.Public);
				if (!(property6 != null))
				{
					continue;
				}
				object value5 = property6.GetValue(value4);
				if (value5 != null)
				{
					string text2 = value5.ToString();
					if (text2 == text)
					{
						return true;
					}
				}
			}
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning($"[PlayerOwnsWeapon] Error checking {weaponType}: {ex.Message}");
		}
		return false;
	}

	private unsafe static bool PlayerOwnsItem(ItemType itemType)
	{
		if (cachedGameSession == null)
		{
			return false;
		}
		try
		{
			PropertyInfo property = cachedGameSession.GetType().GetProperty("ActiveCharacter", BindingFlags.Instance | BindingFlags.Public);
			if (property == null)
			{
				return false;
			}
			object value = property.GetValue(cachedGameSession);
			if (value == null)
			{
				return false;
			}
			PropertyInfo property2 = value.GetType().GetProperty("AccessoriesManager", BindingFlags.Instance | BindingFlags.Public);
			if (property2 == null)
			{
				return false;
			}
			object value2 = property2.GetValue(value);
			if (value2 == null)
			{
				return false;
			}
			PropertyInfo property3 = value2.GetType().GetProperty("ActiveEquipment", BindingFlags.Instance | BindingFlags.Public);
			if (property3 == null)
			{
				return false;
			}
			object value3 = property3.GetValue(value2);
			if (value3 == null)
			{
				return false;
			}
			PropertyInfo property4 = value3.GetType().GetProperty("Count");
			PropertyInfo property5 = value3.GetType().GetProperty("Item");
			if (property4 == null || property5 == null)
			{
				return false;
			}
			int num = (int)property4.GetValue(value3);
			for (int i = 0; i < num; i++)
			{
				object value4 = property5.GetValue(value3, new object[1] { i });
				if (value4 == null)
				{
					continue;
				}
				PropertyInfo property6 = value4.GetType().GetProperty("Type", BindingFlags.Instance | BindingFlags.Public);
				if (property6 != null)
				{
					object value5 = property6.GetValue(value4);
					if (value5 != null && value5.ToString() == ((object)(*(ItemType*)(&itemType))/*cast due to constrained. prefix*/).ToString())
					{
						return true;
					}
				}
			}
		}
		catch
		{
		}
		return false;
	}

	private unsafe static bool PlayerOwnsAccessory(WeaponType weaponType)
	{
		if (cachedGameSession == null)
		{
			return false;
		}
		try
		{
			PropertyInfo property = cachedGameSession.GetType().GetProperty("ActiveCharacter", BindingFlags.Instance | BindingFlags.Public);
			if (property == null)
			{
				return false;
			}
			object value = property.GetValue(cachedGameSession);
			if (value == null)
			{
				return false;
			}
			PropertyInfo property2 = value.GetType().GetProperty("AccessoriesManager", BindingFlags.Instance | BindingFlags.Public);
			if (property2 == null)
			{
				return false;
			}
			object value2 = property2.GetValue(value);
			if (value2 == null)
			{
				return false;
			}
			PropertyInfo property3 = value2.GetType().GetProperty("ActiveEquipment", BindingFlags.Instance | BindingFlags.Public);
			if (property3 == null)
			{
				return false;
			}
			object value3 = property3.GetValue(value2);
			if (value3 == null)
			{
				return false;
			}
			PropertyInfo property4 = value3.GetType().GetProperty("Count");
			PropertyInfo property5 = value3.GetType().GetProperty("Item");
			if (property4 == null || property5 == null)
			{
				return false;
			}
			int num = (int)property4.GetValue(value3);
			string text = ((object)(*(WeaponType*)(&weaponType))/*cast due to constrained. prefix*/).ToString();
			for (int i = 0; i < num; i++)
			{
				object value4 = property5.GetValue(value3, new object[1] { i });
				if (value4 == null)
				{
					continue;
				}
				PropertyInfo property6 = value4.GetType().GetProperty("Type", BindingFlags.Instance | BindingFlags.Public);
				if (property6 != null)
				{
					object value5 = property6.GetValue(value4);
					if (value5 != null && value5.ToString() == text)
					{
						return true;
					}
				}
			}
		}
		catch
		{
		}
		return false;
	}

	private unsafe static bool IsWeaponBanned(WeaponType weaponType)
	{
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			object gameManager = GetGameManager();
			if (gameManager == null)
			{
				return false;
			}
			if (cachedLevelUpFactory == null)
			{
				PropertyInfo property = gameManager.GetType().GetProperty("LevelUpFactory", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (property == null)
				{
					return false;
				}
				cachedLevelUpFactory = property.GetValue(gameManager);
			}
			if (cachedLevelUpFactory == null)
			{
				return false;
			}
			PropertyInfo property2 = cachedLevelUpFactory.GetType().GetProperty("BanishedWeapons", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (property2 == null)
			{
				return false;
			}
			object value = property2.GetValue(cachedLevelUpFactory);
			if (value == null)
			{
				return false;
			}
			MethodInfo method = value.GetType().GetMethod("Contains");
			if (method != null)
			{
				return (bool)method.Invoke(value, new object[1] { weaponType });
			}
			PropertyInfo property3 = value.GetType().GetProperty("Count");
			PropertyInfo property4 = value.GetType().GetProperty("Item");
			if (property3 == null || property4 == null)
			{
				return false;
			}
			int num = (int)property3.GetValue(value);
			string text = ((object)(*(WeaponType*)(&weaponType))/*cast due to constrained. prefix*/).ToString();
			for (int i = 0; i < num; i++)
			{
				object value2 = property4.GetValue(value, new object[1] { i });
				if (value2 != null && value2.ToString() == text)
				{
					return true;
				}
			}
		}
		catch
		{
		}
		return false;
	}

	private unsafe static bool IsItemBanned(ItemType itemType)
	{
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			object gameManager = GetGameManager();
			if (gameManager == null)
			{
				return false;
			}
			if (cachedLevelUpFactory == null)
			{
				PropertyInfo property = gameManager.GetType().GetProperty("LevelUpFactory", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (property == null)
				{
					return false;
				}
				cachedLevelUpFactory = property.GetValue(gameManager);
			}
			if (cachedLevelUpFactory == null)
			{
				return false;
			}
			string[] array = new string[2] { "BanishedPowerUps", "BanishedItems" };
			string[] array2 = array;
			string[] array3 = array2;
			foreach (string name in array3)
			{
				PropertyInfo property2 = cachedLevelUpFactory.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (property2 == null)
				{
					continue;
				}
				object value = property2.GetValue(cachedLevelUpFactory);
				if (value == null)
				{
					continue;
				}
				MethodInfo method = value.GetType().GetMethod("Contains");
				if (method != null)
				{
					try
					{
						return (bool)method.Invoke(value, new object[1] { itemType });
					}
					catch
					{
					}
				}
				PropertyInfo property3 = value.GetType().GetProperty("Count");
				PropertyInfo property4 = value.GetType().GetProperty("Item");
				if (property3 == null || property4 == null)
				{
					continue;
				}
				int num = (int)property3.GetValue(value);
				string text = ((object)(*(ItemType*)(&itemType))/*cast due to constrained. prefix*/).ToString();
				for (int j = 0; j < num; j++)
				{
					object value2 = property4.GetValue(value, new object[1] { j });
					if (value2 != null && value2.ToString() == text)
					{
						return true;
					}
				}
			}
		}
		catch
		{
		}
		return false;
	}

	private static GameObject CreateFormulaIcon(Transform parent, string name, Sprite sprite, bool isOwned, bool isBanned, float size, float x, float y)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Expected O, but got Unknown
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Expected O, but got Unknown
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		GameObject val = new GameObject(name);
		val.transform.SetParent(parent, false);
		RectTransform val2 = val.AddComponent<RectTransform>();
		val2.anchorMin = new Vector2(0f, 1f);
		val2.anchorMax = new Vector2(0f, 1f);
		val2.pivot = new Vector2(0f, 1f);
		val2.anchoredPosition = new Vector2(x, y);
		val2.sizeDelta = new Vector2(size, size);
		Image val3 = val.AddComponent<Image>();
		((Graphic)val3).color = new Color(0f, 0f, 0f, 0f);
		((Graphic)val3).raycastTarget = true;
		if (isOwned)
		{
			GameObject val4 = new GameObject("OwnedBg");
			val4.transform.SetParent(val.transform, false);
			Image val5 = val4.AddComponent<Image>();
			val5.sprite = GetCircleSprite();
			((Graphic)val5).color = new Color(1f, 0.85f, 0f, 0.7f);
			((Graphic)val5).raycastTarget = false;
			RectTransform component = val4.GetComponent<RectTransform>();
			component.anchorMin = Vector2.zero;
			component.anchorMax = Vector2.one;
			component.offsetMin = new Vector2(-4f, -4f);
			component.offsetMax = new Vector2(4f, 4f);
		}
		if ((Object)(object)sprite != (Object)null)
		{
			GameObject val6 = new GameObject("Icon");
			val6.transform.SetParent(val.transform, false);
			Image val7 = val6.AddComponent<Image>();
			val7.sprite = sprite;
			val7.preserveAspect = true;
			((Graphic)val7).raycastTarget = false;
			RectTransform component2 = val6.GetComponent<RectTransform>();
			component2.anchorMin = Vector2.zero;
			component2.anchorMax = Vector2.one;
			component2.offsetMin = Vector2.zero;
			component2.offsetMax = Vector2.zero;
		}
		if (isBanned)
		{
			for (int i = 0; i < 2; i++)
			{
				GameObject val8 = new GameObject((i == 0) ? "BannedBar1" : "BannedBar2");
				val8.transform.SetParent(val.transform, false);
				Image val9 = val8.AddComponent<Image>();
				((Graphic)val9).color = new Color(1f, 0.15f, 0.15f, 0.9f);
				((Graphic)val9).raycastTarget = false;
				RectTransform component3 = val8.GetComponent<RectTransform>();
				component3.anchorMin = new Vector2(0.5f, 0.5f);
				component3.anchorMax = new Vector2(0.5f, 0.5f);
				component3.pivot = new Vector2(0.5f, 0.5f);
				component3.sizeDelta = new Vector2(size * 1.2f, size * 0.15f);
				((Transform)component3).localRotation = Quaternion.Euler(0f, 0f, (i == 0) ? 45f : (-45f));
			}
		}
		return val;
	}

	private static int CountPassiveUses(WeaponType passiveType, string ownEvoInto = null)
	{
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		if (cachedWeaponsDict == null)
		{
			return 0;
		}
		int num = 0;
		try
		{
			PropertyInfo property = cachedWeaponsDict.GetType().GetProperty("Keys");
			if (property == null)
			{
				return 0;
			}
			object value = property.GetValue(cachedWeaponsDict);
			object obj = value.GetType().GetMethod("GetEnumerator").Invoke(value, null);
			MethodInfo method = obj.GetType().GetMethod("MoveNext");
			PropertyInfo property2 = obj.GetType().GetProperty("Current");
			while ((bool)method.Invoke(obj, null))
			{
				WeaponType type = (WeaponType)property2.GetValue(obj);
				List<WeaponData> weaponDataList = GetWeaponDataList(type);
				if (weaponDataList == null)
				{
					continue;
				}
				for (int i = 0; i < weaponDataList.Count; i++)
				{
					WeaponData val = weaponDataList[i];
					if (val == null)
					{
						continue;
					}
					PropertyInfo property3 = ((object)val).GetType().GetProperty("evoSynergy");
					if (property3 != null && property3.GetValue(val) is Il2CppStructArray<WeaponType> val2)
					{
						for (int j = 0; j < ((Il2CppArrayBase<WeaponType>)(object)val2).Length; j++)
						{
							if (((Il2CppArrayBase<WeaponType>)(object)val2)[j] == passiveType)
							{
								num++;
								break;
							}
						}
					}
					object obj2 = ((object)val).GetType().GetProperty("requires")?.GetValue(val);
					if (obj2 == null)
					{
						continue;
					}
					int num2 = (int)obj2.GetType().GetProperty("Count").GetValue(obj2);
					PropertyInfo property4 = obj2.GetType().GetProperty("Item");
					for (int k = 0; k < num2; k++)
					{
						if ((WeaponType)property4.GetValue(obj2, new object[1] { k }) == passiveType)
						{
							num++;
							break;
						}
					}
				}
			}
		}
		catch
		{
		}
		return num;
	}

	private static float AddWeaponEvolutionSection(Transform parent, TMP_FontAsset font, WeaponType weaponType, float yOffset, float maxWidth)
	{
		GameData.EnsureLoaded();
		Plugin.Dbg($"Evo section for {weaponType}");
		var rows = GameData.BuildEvoRowsFor(weaponType);
		if (rows == null || rows.Count == 0)
		{
			// No forward recipes — show reverse "evolved from" if this is an evolution itself
			yOffset = AddEvolvedFromSection(parent, font, weaponType, yOffset, maxWidth);
			return yOffset;
		}

		// Multi-row: every recipe as its own base + passives → evolved line
		yOffset -= Spacing + 4f; // clear gap under description
		string headerLabel = rows.Count == 1 ? "Evolutions: (click for details)" : $"Evolutions ({rows.Count}): (click for details)";
		GameObject header = CreateTextElement(parent, "EvoHeader", headerLabel, font, 14f, new Color(0.9f, 0.75f, 0.3f, 1f), (FontStyles)1);
		RectTransform headerRt = header.GetComponent<RectTransform>();
		headerRt.anchorMin = new Vector2(0f, 1f);
		headerRt.anchorMax = new Vector2(0f, 1f);
		headerRt.pivot = new Vector2(0f, 1f);
		headerRt.anchoredPosition = new Vector2(Padding, yOffset);
		headerRt.sizeDelta = new Vector2(maxWidth - Padding * 2f, 22f);
		yOffset -= 26f; // header + gap before icons

		float iconSize = 38f;
		float iconGap = 6f;
		float midY = (iconSize - 18f) * 0.5f; // center +/→ in the icon row
		for (int ri = 0; ri < rows.Count; ri++)
		{
			var row = rows[ri];
			bool anyMax = false;
			if (row.Passives != null)
			{
				foreach (var p in row.Passives)
				{
					if (p.RequiresMax) { anyMax = true; break; }
				}
			}
			float rowHeight = iconSize + 10f + (anyMax ? 14f : 0f);
			float x = Padding + 4f;

			// Base (hovered) weapon
			GameObject baseIcon = CreateFormulaIcon(parent, $"BaseIcon{ri}", GameData.GetSprite(weaponType), PlayerOwnsWeapon(weaponType), IsWeaponBanned(weaponType), iconSize, x, yOffset);
			AddHoverToGameObject(baseIcon, weaponType, null, useClick: true);
			x += iconSize + iconGap;

			if (row.Passives != null)
			{
				foreach (var passive in row.Passives)
				{
					GameObject plus = CreateTextElement(parent, $"Plus{ri}", "+", font, 18f, new Color(0.85f, 0.85f, 0.85f, 1f), (FontStyles)1);
					RectTransform plusRt = plus.GetComponent<RectTransform>();
					plusRt.anchorMin = new Vector2(0f, 1f);
					plusRt.anchorMax = new Vector2(0f, 1f);
					plusRt.pivot = new Vector2(0f, 1f);
					plusRt.anchoredPosition = new Vector2(x, yOffset - midY);
					plusRt.sizeDelta = new Vector2(18f, 22f);
					x += 18f + 4f;
					Sprite ps = passive.Sprite ?? GameData.GetSprite(passive.Type);
					GameObject pIcon = CreateFormulaIcon(parent, $"PassiveIcon{ri}_{passive.Type}", ps, PlayerOwnsWeapon(passive.Type), IsWeaponBanned(passive.Type), iconSize, x, yOffset);
					AddHoverToGameObject(pIcon, passive.Type, null, useClick: true);
					if (passive.RequiresMax)
					{
						GameObject maxLbl = CreateTextElement(parent, $"Max{ri}", "MAX", font, 9f, new Color(1f, 0.85f, 0f, 1f), (FontStyles)1);
						RectTransform maxRt = maxLbl.GetComponent<RectTransform>();
						maxRt.anchorMin = new Vector2(0f, 1f);
						maxRt.anchorMax = new Vector2(0f, 1f);
						maxRt.pivot = new Vector2(0.5f, 1f);
						maxRt.anchoredPosition = new Vector2(x + iconSize / 2f, yOffset - iconSize - 1f);
						maxRt.sizeDelta = new Vector2(iconSize + 4f, 12f);
					}
					x += iconSize + iconGap;
				}
			}

			GameObject arrow = CreateTextElement(parent, $"Arrow{ri}", "→", font, 18f, new Color(0.85f, 0.85f, 0.85f, 1f), (FontStyles)0);
			RectTransform arrowRt = arrow.GetComponent<RectTransform>();
			arrowRt.anchorMin = new Vector2(0f, 1f);
			arrowRt.anchorMax = new Vector2(0f, 1f);
			arrowRt.pivot = new Vector2(0f, 1f);
			arrowRt.anchoredPosition = new Vector2(x, yOffset - midY);
			arrowRt.sizeDelta = new Vector2(22f, 22f);
			x += 22f + 4f;

			Sprite evoSprite = row.EvolvedSprite ?? GameData.GetSprite(row.Evolved);
			GameObject evoIcon = CreateFormulaIcon(parent, $"EvoIcon{ri}", evoSprite, false, IsWeaponBanned(row.Evolved), iconSize, x, yOffset);
			AddHoverToGameObject(evoIcon, row.Evolved, null, useClick: true);

			// Optional evolved name when multiple rows
			if (rows.Count > 1 && !string.IsNullOrEmpty(row.EvolvedName))
			{
				GameObject nameLbl = CreateTextElement(parent, $"EvoName{ri}", row.EvolvedName, font, 11f, new Color(0.75f, 0.75f, 0.8f, 1f), (FontStyles)0);
				RectTransform nameRt = nameLbl.GetComponent<RectTransform>();
				nameRt.anchorMin = new Vector2(0f, 1f);
				nameRt.anchorMax = new Vector2(0f, 1f);
				nameRt.pivot = new Vector2(0f, 1f);
				nameRt.anchoredPosition = new Vector2(x + iconSize + 6f, yOffset - midY);
				nameRt.sizeDelta = new Vector2(Mathf.Max(40f, maxWidth - x - iconSize - Padding - 10f), 20f);
			}

			yOffset -= rowHeight;
			Plugin.Dbg($"  evo row {ri}: {weaponType} + [{string.Join("+", System.Linq.Enumerable.Select(row.Passives ?? new System.Collections.Generic.List<EvoPassive>(), p => p.Type.ToString()))}] -> {row.Evolved} sprite={(evoSprite != null ? "ok" : "NULL")}");
		}
		yOffset -= 4f; // gap before Arcana
		return yOffset;
	}

private unsafe static float AddEvolvedFromSection(Transform parent, TMP_FontAsset font, WeaponType evolvedType, float yOffset, float maxWidth)
	{
		//IL_0600: Unknown result type (might be due to invalid IL or missing references)
		//IL_0620: Unknown result type (might be due to invalid IL or missing references)
		//IL_0637: Unknown result type (might be due to invalid IL or missing references)
		//IL_064e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0661: Unknown result type (might be due to invalid IL or missing references)
		//IL_0681: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0717: Unknown result type (might be due to invalid IL or missing references)
		//IL_0730: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0804: Unknown result type (might be due to invalid IL or missing references)
		//IL_081a: Unknown result type (might be due to invalid IL or missing references)
		//IL_082e: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_087c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0866: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0916: Unknown result type (might be due to invalid IL or missing references)
		//IL_096a: Unknown result type (might be due to invalid IL or missing references)
		//IL_098c: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0506: Unknown result type (might be due to invalid IL or missing references)
		//IL_0514: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Expected I4, but got Unknown
		//IL_0477: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Unknown result type (might be due to invalid IL or missing references)
		//IL_0493: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_0457: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
		if (cachedWeaponsDict == null)
		{
			return yOffset;
		}
		string text = ((object)(*(WeaponType*)(&evolvedType))/*cast due to constrained. prefix*/).ToString();
		EvolutionFormula? evolutionFormula = null;
		try
		{
			PropertyInfo property = cachedWeaponsDict.GetType().GetProperty("Keys");
			if (property == null)
			{
				return yOffset;
			}
			object value = property.GetValue(cachedWeaponsDict);
			object obj = value.GetType().GetMethod("GetEnumerator").Invoke(value, null);
			MethodInfo method = obj.GetType().GetMethod("MoveNext");
			PropertyInfo property2 = obj.GetType().GetProperty("Current");
			while ((bool)method.Invoke(obj, null))
			{
				WeaponType val = (WeaponType)property2.GetValue(obj);
				List<WeaponData> weaponDataList = GetWeaponDataList(val);
				if (weaponDataList == null)
				{
					continue;
				}
				for (int i = 0; i < weaponDataList.Count; i++)
				{
					WeaponData val2 = weaponDataList[i];
					if (val2 == null)
					{
						continue;
					}
					try
					{
						string propertyValue = GetPropertyValue<string>(val2, "evoInto");
						if (!string.IsNullOrEmpty(propertyValue) && propertyValue == text)
						{
							PropertyInfo property3 = ((object)val2).GetType().GetProperty("evoSynergy");
							if (property3 != null)
							{
								Il2CppStructArray<WeaponType> evoSynergy = property3.GetValue(val2) as Il2CppStructArray<WeaponType>;
								evolutionFormula = new EvolutionFormula
								{
									BaseWeapon = val,
									Passives = CollectPassiveRequirements(evoSynergy, GetRequiresMaxFromEvolved(propertyValue)),
									EvolvedWeapon = evolvedType,
									BaseName = GetLocalizedWeaponName(val2, val),
									BaseSprite = GetSpriteForWeapon(val),
									EvolvedSprite = GetSpriteForWeapon(evolvedType)
								};
								break;
							}
						}
						if (!GetPropertyValue<bool>(val2, "isEvolution"))
						{
							continue;
						}
						PropertyInfo property4 = ((object)val2).GetType().GetProperty("evolvesFrom");
						if (!(property4 != null))
						{
							continue;
						}
						object value2 = property4.GetValue(val2);
						if (value2 == null)
						{
							continue;
						}
						PropertyInfo property5 = value2.GetType().GetProperty("Count");
						PropertyInfo property6 = value2.GetType().GetProperty("Item");
						int num = ((property5 != null) ? ((int)property5.GetValue(value2)) : 0);
						for (int j = 0; j < num; j++)
						{
							WeaponType val3 = (WeaponType)property6.GetValue(value2, new object[1] { j });
							if (val3 != val)
							{
								continue;
							}
							List<PassiveRequirement> list = new List<PassiveRequirement>();
							PropertyInfo property7 = ((object)val2).GetType().GetProperty("requires");
							PropertyInfo property8 = ((object)val2).GetType().GetProperty("requiresMax");
							HashSet<WeaponType> hashSet = new HashSet<WeaponType>();
							if (property8 != null)
							{
								object value3 = property8.GetValue(val2);
								if (value3 != null)
								{
									PropertyInfo property9 = value3.GetType().GetProperty("Count");
									PropertyInfo property10 = value3.GetType().GetProperty("Item");
									int num2 = ((property9 != null) ? ((int)property9.GetValue(value3)) : 0);
									for (int k = 0; k < num2; k++)
									{
										hashSet.Add((WeaponType)property10.GetValue(value3, new object[1] { k }));
									}
								}
							}
							if (property7 != null)
							{
								object value4 = property7.GetValue(val2);
								if (value4 != null)
								{
									PropertyInfo property11 = value4.GetType().GetProperty("Count");
									PropertyInfo property12 = value4.GetType().GetProperty("Item");
									int num3 = ((property11 != null) ? ((int)property11.GetValue(value4)) : 0);
									for (int l = 0; l < num3; l++)
									{
										WeaponType val4 = (WeaponType)property12.GetValue(value4, new object[1] { l });
										int num4 = (int)val4;
										PassiveRequirement item = default(PassiveRequirement);
										if (Enum.IsDefined(typeof(ItemType), num4))
										{
											ItemType val5 = (ItemType)num4;
											item.ItemType = val5;
											item.Sprite = GetSpriteForItem(val5);
											item.Owned = PlayerOwnsItem(val5);
										}
										else
										{
											item.WeaponType = val4;
											item.Sprite = GetSpriteForWeapon(val4);
											item.Owned = PlayerOwnsWeapon(val4);
										}
										item.RequiresMaxLevel = hashSet.Contains(val4);
										list.Add(item);
									}
								}
							}
							evolutionFormula = new EvolutionFormula
							{
								BaseWeapon = val,
								Passives = list,
								EvolvedWeapon = evolvedType,
								BaseName = GetLocalizedWeaponName(val2, val),
								BaseSprite = GetSpriteForWeapon(val),
								EvolvedSprite = GetSpriteForWeapon(evolvedType)
							};
							break;
						}
					}
					catch (Exception ex)
					{
						Plugin.Log.LogInfo("[AddEvolvedFromSection] Error: " + ex.Message);
					}
				}
				if (!evolutionFormula.HasValue)
				{
					continue;
				}
				break;
			}
		}
		catch
		{
		}
		if (!evolutionFormula.HasValue)
		{
			return yOffset;
		}
		EvolutionFormula value5 = evolutionFormula.Value;
		yOffset -= Spacing;
		GameObject val6 = CreateTextElement(parent, "EvolvedFromHeader", "Evolved from: (click for details)", font, 14f, new Color(0.9f, 0.75f, 0.3f, 1f), (FontStyles)1);
		RectTransform component = val6.GetComponent<RectTransform>();
		component.anchorMin = new Vector2(0f, 1f);
		component.anchorMax = new Vector2(1f, 1f);
		component.pivot = new Vector2(0f, 1f);
		component.anchoredPosition = new Vector2(Padding, yOffset);
		component.sizeDelta = new Vector2(maxWidth - Padding * 2f, 20f);
		yOffset -= 22f;
		float num5 = 36f;
		bool flag = value5.Passives != null && value5.Passives.Exists((PassiveRequirement p) => p.RequiresMaxLevel);
		float num6 = num5 + 4f + (flag ? 12f : 0f);
		float num7 = Padding + 5f;
		bool isOwned = PlayerOwnsWeapon(value5.BaseWeapon);
		GameObject go = CreateFormulaIcon(parent, "EvolvedFromBase", value5.BaseSprite, isOwned, IsWeaponBanned(value5.BaseWeapon), num5, num7, yOffset);
		AddHoverToGameObject(go, value5.BaseWeapon, null, useClick: true);
		num7 += num5 + 3f;
		if (value5.Passives != null)
		{
			for (int num8 = 0; num8 < value5.Passives.Count; num8++)
			{
				PassiveRequirement passiveRequirement = value5.Passives[num8];
				GameObject val7 = CreateTextElement(parent, $"EvolvedFromPlus{num8}", "+", font, 14f, new Color(0.8f, 0.8f, 0.8f, 1f), (FontStyles)1);
				RectTransform component2 = val7.GetComponent<RectTransform>();
				component2.anchorMin = new Vector2(0f, 1f);
				component2.anchorMax = new Vector2(0f, 1f);
				component2.pivot = new Vector2(0f, 1f);
				component2.anchoredPosition = new Vector2(num7, yOffset - 4f);
				component2.sizeDelta = new Vector2(14f, num5);
				num7 += 14f;
				bool isBanned = (passiveRequirement.WeaponType.HasValue ? IsWeaponBanned(passiveRequirement.WeaponType.Value) : (passiveRequirement.ItemType.HasValue && IsItemBanned(passiveRequirement.ItemType.Value)));
				GameObject go2 = CreateFormulaIcon(parent, $"EvolvedFromPassive{num8}", passiveRequirement.Sprite, passiveRequirement.Owned, isBanned, num5, num7, yOffset);
				if (passiveRequirement.WeaponType.HasValue)
				{
					AddHoverToGameObject(go2, passiveRequirement.WeaponType.Value, null, useClick: true);
				}
				else if (passiveRequirement.ItemType.HasValue)
				{
					AddHoverToGameObject(go2, null, passiveRequirement.ItemType.Value, useClick: true);
				}
				if (passiveRequirement.RequiresMaxLevel)
				{
					GameObject val8 = CreateTextElement(parent, $"EvolvedFromMax{num8}", "MAX", font, 9f, new Color(1f, 0.85f, 0f, 1f), (FontStyles)1);
					RectTransform component3 = val8.GetComponent<RectTransform>();
					component3.anchorMin = new Vector2(0f, 1f);
					component3.anchorMax = new Vector2(0f, 1f);
					component3.pivot = new Vector2(0.5f, 1f);
					component3.anchoredPosition = new Vector2(num7 + num5 / 2f, yOffset - num5);
					component3.sizeDelta = new Vector2(num5, 12f);
					TextMeshProUGUI component4 = val8.GetComponent<TextMeshProUGUI>();
					if ((Object)(object)component4 != (Object)null)
					{
						((TMP_Text)component4).alignment = (TextAlignmentOptions)514;
					}
				}
				num7 += num5 + 3f;
			}
		}
		yOffset -= num6;
		return yOffset;
	}

	private unsafe static float AddPassiveEvolutionSection(Transform parent, TMP_FontAsset font, WeaponType passiveType, float yOffset, float maxWidth)
	{
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c84: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ced: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d34: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0678: Unknown result type (might be due to invalid IL or missing references)
		//IL_067d: Unknown result type (might be due to invalid IL or missing references)
		//IL_067f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_050f: Unknown result type (might be due to invalid IL or missing references)
		//IL_052b: Unknown result type (might be due to invalid IL or missing references)
		//IL_052d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0534: Unknown result type (might be due to invalid IL or missing references)
		//IL_0542: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Unknown result type (might be due to invalid IL or missing references)
		//IL_0560: Unknown result type (might be due to invalid IL or missing references)
		//IL_0575: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dcd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e10: Unknown result type (might be due to invalid IL or missing references)
		//IL_119e: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_1204: Unknown result type (might be due to invalid IL or missing references)
		//IL_1218: Unknown result type (might be due to invalid IL or missing references)
		//IL_1249: Unknown result type (might be due to invalid IL or missing references)
		//IL_1263: Unknown result type (might be due to invalid IL or missing references)
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0782: Unknown result type (might be due to invalid IL or missing references)
		//IL_0787: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eeb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f19: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f91: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_096b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0970: Unknown result type (might be due to invalid IL or missing references)
		//IL_0972: Unknown result type (might be due to invalid IL or missing references)
		//IL_0974: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0981: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b87: Unknown result type (might be due to invalid IL or missing references)
		//IL_083a: Unknown result type (might be due to invalid IL or missing references)
		//IL_083f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0992: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba7: Unknown result type (might be due to invalid IL or missing references)
		//IL_108d: Unknown result type (might be due to invalid IL or missing references)
		//IL_10af: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_10dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_110d: Unknown result type (might be due to invalid IL or missing references)
		//IL_091c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bcc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b05: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		if (cachedWeaponsDict == null)
		{
			return yOffset;
		}
		List<EvolutionFormula> list = new List<EvolutionFormula>();
		try
		{
			PropertyInfo property = cachedWeaponsDict.GetType().GetProperty("Keys");
			if (property == null)
			{
				return yOffset;
			}
			object value = property.GetValue(cachedWeaponsDict);
			object obj = value.GetType().GetMethod("GetEnumerator").Invoke(value, null);
			MethodInfo method = obj.GetType().GetMethod("MoveNext");
			PropertyInfo property2 = obj.GetType().GetProperty("Current");
			while ((bool)method.Invoke(obj, null))
			{
				WeaponType val = (WeaponType)property2.GetValue(obj);
				List<WeaponData> weaponDataList = GetWeaponDataList(val);
				if (weaponDataList == null)
				{
					continue;
				}
				for (int i = 0; i < weaponDataList.Count; i++)
				{
					WeaponData val2 = weaponDataList[i];
					if (val2 == null)
					{
						continue;
					}
					try
					{
						PropertyInfo property3 = ((object)val2).GetType().GetProperty("evoSynergy");
						if (property3 == null || !(property3.GetValue(val2) is Il2CppStructArray<WeaponType> val3) || ((Il2CppArrayBase<WeaponType>)(object)val3).Length == 0)
						{
							continue;
						}
						string propertyValue = GetPropertyValue<string>(val2, "evoInto");
						if (string.IsNullOrEmpty(propertyValue))
						{
							continue;
						}
						bool flag = false;
						for (int j = 0; j < ((Il2CppArrayBase<WeaponType>)(object)val3).Length; j++)
						{
							if (((Il2CppArrayBase<WeaponType>)(object)val3)[j] == passiveType)
							{
								flag = true;
								break;
							}
						}
						if (!flag || !GameData.TryParseWeaponType(propertyValue, out WeaponType result))
						{
							continue;
						}
						List<PassiveRequirement> list2 = CollectPassiveRequirements(val3, GetRequiresMaxFromEvolved(propertyValue));
						try
						{
							PropertyInfo property4 = ((object)val2).GetType().GetProperty("requiresMax");
							if (property4 != null)
							{
								object value2 = property4.GetValue(val2);
								if (value2 != null)
								{
									PropertyInfo property5 = value2.GetType().GetProperty("Count");
									PropertyInfo property6 = value2.GetType().GetProperty("Item");
									int num = ((property5 != null) ? ((int)property5.GetValue(value2)) : 0);
									for (int k = 0; k < num; k++)
									{
										WeaponType val4 = (WeaponType)property6.GetValue(value2, new object[1] { k });
										bool flag2 = false;
										foreach (PassiveRequirement item4 in list2)
										{
											if (item4.WeaponType.HasValue && item4.WeaponType.Value == val4)
											{
												flag2 = true;
												break;
											}
										}
										if (!flag2)
										{
											list2.Add(new PassiveRequirement
											{
												WeaponType = val4,
												Sprite = GetSpriteForWeapon(val4),
												Owned = PlayerOwnsWeapon(val4),
												RequiresMaxLevel = true
											});
										}
									}
								}
							}
						}
						catch
						{
						}
						EvolutionFormula item = new EvolutionFormula
						{
							BaseWeapon = val,
							Passives = list2,
							EvolvedWeapon = result,
							BaseName = GetLocalizedWeaponName(val2, val),
							BaseSprite = GetSpriteForWeapon(val),
							EvolvedSprite = GetSpriteForWeapon(result)
						};
						WeaponData weaponData = GetWeaponData(result);
						if (weaponData != null)
						{
							item.EvolvedName = GetLocalizedWeaponName(weaponData, result);
						}
						else
						{
							item.EvolvedName = propertyValue;
						}
						bool flag3 = false;
						foreach (EvolutionFormula item5 in list)
						{
							if (item5.EvolvedWeapon == item.EvolvedWeapon)
							{
								flag3 = true;
								break;
							}
						}
						if (!flag3)
						{
							list.Add(item);
						}
					}
					catch
					{
					}
				}
			}
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[AddPassiveEvo] Error: " + ex.Message);
		}
		try
		{
			WeaponData weaponData2 = GetWeaponData(passiveType);
			if (weaponData2 != null)
			{
				string propertyValue2 = GetPropertyValue<string>(weaponData2, "evoInto");
				if (!string.IsNullOrEmpty(propertyValue2) && ((object)weaponData2).GetType().GetProperty("evoSynergy")?.GetValue(weaponData2) is Il2CppStructArray<WeaponType> evoSynergy && GameData.TryParseWeaponType(propertyValue2, out WeaponType result2))
				{
					EvolutionFormula item2 = new EvolutionFormula
					{
						BaseWeapon = passiveType,
						Passives = CollectPassiveRequirements(evoSynergy, GetRequiresMaxFromEvolved(propertyValue2)),
						EvolvedWeapon = result2,
						EvolvedSprite = GetSpriteForWeapon(result2),
						BaseSprite = GetSpriteForWeapon(passiveType),
						BaseName = GetLocalizedWeaponName(weaponData2, passiveType)
					};
					WeaponData weaponData3 = GetWeaponData(result2);
					item2.EvolvedName = ((weaponData3 != null) ? GetLocalizedWeaponName(weaponData3, result2) : propertyValue2);
					bool flag4 = false;
					foreach (EvolutionFormula item6 in list)
					{
						if (item6.EvolvedWeapon == item2.EvolvedWeapon)
						{
							flag4 = true;
							break;
						}
					}
					if (!flag4)
					{
						list.Add(item2);
					}
				}
			}
		}
		catch
		{
		}
		try
		{
			PropertyInfo property7 = cachedWeaponsDict.GetType().GetProperty("Keys");
			if (property7 != null)
			{
				object value3 = property7.GetValue(cachedWeaponsDict);
				object obj5 = value3.GetType().GetMethod("GetEnumerator").Invoke(value3, null);
				MethodInfo method2 = obj5.GetType().GetMethod("MoveNext");
				PropertyInfo property8 = obj5.GetType().GetProperty("Current");
				while ((bool)method2.Invoke(obj5, null))
				{
					WeaponType val5 = (WeaponType)property8.GetValue(obj5);
					List<WeaponData> weaponDataList2 = GetWeaponDataList(val5);
					if (weaponDataList2 == null)
					{
						continue;
					}
					for (int l = 0; l < weaponDataList2.Count; l++)
					{
						WeaponData val6 = weaponDataList2[l];
						if (val6 == null)
						{
							continue;
						}
						try
						{
							if (!GetPropertyValue<bool>(val6, "isEvolution"))
							{
								continue;
							}
							PropertyInfo property9 = ((object)val6).GetType().GetProperty("evolvesFrom");
							if (property9 == null)
							{
								continue;
							}
							object value4 = property9.GetValue(val6);
							if (value4 == null)
							{
								continue;
							}
							PropertyInfo property10 = value4.GetType().GetProperty("Count");
							PropertyInfo property11 = value4.GetType().GetProperty("Item");
							int num2 = ((property10 != null) ? ((int)property10.GetValue(value4)) : 0);
							bool flag5 = false;
							for (int m = 0; m < num2; m++)
							{
								if ((WeaponType)property11.GetValue(value4, new object[1] { m }) == passiveType)
								{
									flag5 = true;
									break;
								}
							}
							if (!flag5)
							{
								object obj6 = ((object)val6).GetType().GetProperty("requires")?.GetValue(val6);
								if (obj6 != null)
								{
									int num3 = (int)obj6.GetType().GetProperty("Count").GetValue(obj6);
									PropertyInfo property12 = obj6.GetType().GetProperty("Item");
									for (int n = 0; n < num3; n++)
									{
										if ((WeaponType)property12.GetValue(obj6, new object[1] { n }) == passiveType)
										{
											flag5 = true;
											break;
										}
									}
								}
							}
							if (!flag5)
							{
								continue;
							}
							HashSet<WeaponType> hashSet = new HashSet<WeaponType>();
							PropertyInfo property13 = ((object)val6).GetType().GetProperty("requiresMax");
							if (property13 != null)
							{
								object value5 = property13.GetValue(val6);
								if (value5 != null)
								{
									PropertyInfo property14 = value5.GetType().GetProperty("Count");
									PropertyInfo property15 = value5.GetType().GetProperty("Item");
									int num4 = ((property14 != null) ? ((int)property14.GetValue(value5)) : 0);
									for (int num5 = 0; num5 < num4; num5++)
									{
										hashSet.Add((WeaponType)property15.GetValue(value5, new object[1] { num5 }));
									}
								}
							}
							List<PassiveRequirement> list3 = new List<PassiveRequirement>();
							HashSet<WeaponType> hashSet2 = new HashSet<WeaponType>();
							for (int num6 = 0; num6 < num2; num6++)
							{
								WeaponType val7 = (WeaponType)property11.GetValue(value4, new object[1] { num6 });
								if (val7 != passiveType && !hashSet2.Contains(val7))
								{
									hashSet2.Add(val7);
									list3.Add(new PassiveRequirement
									{
										WeaponType = val7,
										Sprite = GetSpriteForWeapon(val7),
										Owned = PlayerOwnsWeapon(val7),
										RequiresMaxLevel = hashSet.Contains(val7)
									});
								}
							}
							PropertyInfo property16 = ((object)val6).GetType().GetProperty("requires");
							if (property16 != null)
							{
								object value6 = property16.GetValue(val6);
								if (value6 != null)
								{
									PropertyInfo property17 = value6.GetType().GetProperty("Count");
									PropertyInfo property18 = value6.GetType().GetProperty("Item");
									int num7 = ((property17 != null) ? ((int)property17.GetValue(value6)) : 0);
									for (int num8 = 0; num8 < num7; num8++)
									{
										WeaponType val8 = (WeaponType)property18.GetValue(value6, new object[1] { num8 });
										if (val8 != passiveType && !hashSet2.Contains(val8))
										{
											hashSet2.Add(val8);
											list3.Add(new PassiveRequirement
											{
												WeaponType = val8,
												Sprite = GetSpriteForWeapon(val8),
												Owned = PlayerOwnsWeapon(val8),
												RequiresMaxLevel = hashSet.Contains(val8)
											});
										}
									}
								}
							}
							EvolutionFormula item3 = new EvolutionFormula
							{
								BaseWeapon = passiveType,
								Passives = list3,
								EvolvedWeapon = val5,
								BaseName = GetLocalizedWeaponName(GetWeaponData(passiveType), passiveType),
								BaseSprite = GetSpriteForWeapon(passiveType),
								EvolvedSprite = GetSpriteForWeapon(val5)
							};
							WeaponData weaponData4 = GetWeaponData(val5);
							item3.EvolvedName = ((weaponData4 != null) ? GetLocalizedWeaponName(weaponData4, val5) : ((object)(*(WeaponType*)(&val5))/*cast due to constrained. prefix*/).ToString());
							bool flag6 = false;
							foreach (EvolutionFormula item7 in list)
							{
								if (item7.EvolvedWeapon == val5)
								{
									flag6 = true;
									break;
								}
							}
							if (!flag6)
							{
								list.Add(item3);
							}
						}
						catch
						{
						}
					}
				}
			}
		}
		catch (Exception ex2)
		{
			Plugin.Log.LogWarning("[AddPassiveEvo NewSystem] " + ex2.Message);
		}
		if (list.Count == 0)
		{
			return yOffset;
		}
		bool flag7 = PlayerOwnsWeapon(passiveType);
		yOffset -= Spacing;
		GameObject val9 = CreateTextElement(parent, "EvoHeader", "Evolutions: (click for details)", font, 14f, new Color(0.9f, 0.75f, 0.3f, 1f), (FontStyles)1);
		RectTransform component = val9.GetComponent<RectTransform>();
		component.anchorMin = new Vector2(0f, 1f);
		component.anchorMax = new Vector2(1f, 1f);
		component.pivot = new Vector2(0f, 1f);
		component.anchoredPosition = new Vector2(Padding, yOffset);
		component.sizeDelta = new Vector2(maxWidth - Padding * 2f, 20f);
		yOffset -= 22f;
		float num9 = 36f;
		int num10 = 0;
		foreach (EvolutionFormula item8 in list)
		{
			bool flag8 = item8.Passives != null && item8.Passives.Exists((PassiveRequirement p) => p.RequiresMaxLevel);
			float num11 = num9 + 4f + (flag8 ? 12f : 0f);
			float num12 = Padding + 5f;
			bool isOwned = PlayerOwnsWeapon(item8.BaseWeapon);
			GameObject go = CreateFormulaIcon(parent, $"Weapon{num10}", item8.BaseSprite, isOwned, IsWeaponBanned(item8.BaseWeapon), num9, num12, yOffset);
			AddHoverToGameObject(go, item8.BaseWeapon, null, useClick: true);
			num12 += num9 + 3f;
			if (item8.Passives != null)
			{
				for (int num13 = 0; num13 < item8.Passives.Count; num13++)
				{
					PassiveRequirement passiveRequirement = item8.Passives[num13];
					if (passiveRequirement.WeaponType.HasValue && passiveRequirement.WeaponType.Value == passiveType)
					{
						continue;
					}
					GameObject val10 = CreateTextElement(parent, $"Plus{num10}_{num13}", "+", font, 14f, new Color(0.8f, 0.8f, 0.8f, 1f), (FontStyles)1);
					RectTransform component2 = val10.GetComponent<RectTransform>();
					component2.anchorMin = new Vector2(0f, 1f);
					component2.anchorMax = new Vector2(0f, 1f);
					component2.pivot = new Vector2(0f, 1f);
					component2.anchoredPosition = new Vector2(num12, yOffset - 4f);
					component2.sizeDelta = new Vector2(14f, num9);
					num12 += 14f;
					bool isBanned = (passiveRequirement.WeaponType.HasValue ? IsWeaponBanned(passiveRequirement.WeaponType.Value) : (passiveRequirement.ItemType.HasValue && IsItemBanned(passiveRequirement.ItemType.Value)));
					GameObject go2 = CreateFormulaIcon(parent, $"Passive{num10}_{num13}", passiveRequirement.Sprite, passiveRequirement.Owned, isBanned, num9, num12, yOffset);
					if (passiveRequirement.WeaponType.HasValue)
					{
						AddHoverToGameObject(go2, passiveRequirement.WeaponType.Value, null, useClick: true);
					}
					else if (passiveRequirement.ItemType.HasValue)
					{
						AddHoverToGameObject(go2, null, passiveRequirement.ItemType.Value, useClick: true);
					}
					if (passiveRequirement.RequiresMaxLevel)
					{
						GameObject val11 = CreateTextElement(parent, $"PassiveMax{num10}_{num13}", "MAX", font, 9f, new Color(1f, 0.85f, 0f, 1f), (FontStyles)1);
						RectTransform component3 = val11.GetComponent<RectTransform>();
						component3.anchorMin = new Vector2(0f, 1f);
						component3.anchorMax = new Vector2(0f, 1f);
						component3.pivot = new Vector2(0.5f, 1f);
						component3.anchoredPosition = new Vector2(num12 + num9 / 2f, yOffset - num9);
						component3.sizeDelta = new Vector2(num9, 12f);
						TextMeshProUGUI component4 = val11.GetComponent<TextMeshProUGUI>();
						if ((Object)(object)component4 != (Object)null)
						{
							((TMP_Text)component4).alignment = (TextAlignmentOptions)514;
						}
					}
					num12 += num9 + 3f;
				}
			}
			GameObject val12 = CreateTextElement(parent, $"Arrow{num10}", "→", font, 14f, new Color(0.8f, 0.8f, 0.8f, 1f), (FontStyles)0);
			RectTransform component5 = val12.GetComponent<RectTransform>();
			component5.anchorMin = new Vector2(0f, 1f);
			component5.anchorMax = new Vector2(0f, 1f);
			component5.pivot = new Vector2(0f, 1f);
			component5.anchoredPosition = new Vector2(num12, yOffset - 4f);
			component5.sizeDelta = new Vector2(20f, num9);
			num12 += 20f;
			GameObject go3 = CreateFormulaIcon(parent, $"Evo{num10}", item8.EvolvedSprite, isOwned: false, IsWeaponBanned(item8.EvolvedWeapon), num9, num12, yOffset);
			AddHoverToGameObject(go3, item8.EvolvedWeapon, null, useClick: true);
			yOffset -= num11;
			num10++;
		}
		return yOffset;
	}

	private unsafe static float AddItemEvolutionSection(Transform parent, TMP_FontAsset font, ItemType itemType, float yOffset, float maxWidth)
	{
		//IL_0403: Unknown result type (might be due to invalid IL or missing references)
		//IL_0423: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_051d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0546: Unknown result type (might be due to invalid IL or missing references)
		//IL_0560: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0912: Unknown result type (might be due to invalid IL or missing references)
		//IL_0928: Unknown result type (might be due to invalid IL or missing references)
		//IL_093c: Unknown result type (might be due to invalid IL or missing references)
		//IL_096d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0987: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a65: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_060f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0626: Unknown result type (might be due to invalid IL or missing references)
		//IL_063d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0653: Unknown result type (might be due to invalid IL or missing references)
		//IL_0667: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_069f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0712: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0756: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0801: Unknown result type (might be due to invalid IL or missing references)
		//IL_081d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0831: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		if (cachedWeaponsDict == null)
		{
			return yOffset;
		}
		List<EvolutionFormula> list = new List<EvolutionFormula>();
		int num = 0;
		int num2 = 0;
		try
		{
			PropertyInfo property = cachedWeaponsDict.GetType().GetProperty("Keys");
			if (property == null)
			{
				return yOffset;
			}
			object value = property.GetValue(cachedWeaponsDict);
			object obj = value.GetType().GetMethod("GetEnumerator").Invoke(value, null);
			MethodInfo method = obj.GetType().GetMethod("MoveNext");
			PropertyInfo property2 = obj.GetType().GetProperty("Current");
			while ((bool)method.Invoke(obj, null))
			{
				num++;
				WeaponType val = (WeaponType)property2.GetValue(obj);
				List<WeaponData> weaponDataList = GetWeaponDataList(val);
				if (weaponDataList == null)
				{
					continue;
				}
				for (int i = 0; i < weaponDataList.Count; i++)
				{
					WeaponData val2 = weaponDataList[i];
					if (val2 == null)
					{
						continue;
					}
					try
					{
						PropertyInfo property3 = ((object)val2).GetType().GetProperty("evoSynergy");
						if (property3 == null || !(property3.GetValue(val2) is Il2CppStructArray<WeaponType> val3) || ((Il2CppArrayBase<WeaponType>)(object)val3).Length == 0)
						{
							continue;
						}
						string propertyValue = GetPropertyValue<string>(val2, "evoInto");
						if (!string.IsNullOrEmpty(propertyValue))
						{
							string text = "";
							for (int j = 0; j < ((Il2CppArrayBase<WeaponType>)(object)val3).Length; j++)
							{
								text = text + ((object)((Il2CppArrayBase<WeaponType>)(object)val3)[j]/*cast due to constrained. prefix*/).ToString() + " ";
							}
						}
						string text2 = ((object)(*(ItemType*)(&itemType))/*cast due to constrained. prefix*/).ToString();
						bool flag = false;
						for (int k = 0; k < ((Il2CppArrayBase<WeaponType>)(object)val3).Length; k++)
						{
							if (((object)((Il2CppArrayBase<WeaponType>)(object)val3)[k]/*cast due to constrained. prefix*/).ToString() == text2)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							continue;
						}
						num2++;
						string propertyValue2 = GetPropertyValue<string>(val2, "evoInto");
						if (string.IsNullOrEmpty(propertyValue2) || !GameData.TryParseWeaponType(propertyValue2, out WeaponType result))
						{
							continue;
						}
						EvolutionFormula item = new EvolutionFormula
						{
							BaseWeapon = val,
							Passives = CollectPassiveRequirements(val3, GetRequiresMaxFromEvolved(propertyValue2)),
							EvolvedWeapon = result,
							BaseName = GetLocalizedWeaponName(val2, val),
							BaseSprite = GetSpriteForWeapon(val),
							EvolvedSprite = GetSpriteForWeapon(result)
						};
						WeaponData weaponData = GetWeaponData(result);
						if (weaponData != null)
						{
							item.EvolvedName = GetLocalizedWeaponName(weaponData, result);
						}
						else
						{
							item.EvolvedName = propertyValue2;
						}
						bool flag2 = false;
						foreach (EvolutionFormula item2 in list)
						{
							if (item2.EvolvedWeapon == item.EvolvedWeapon)
							{
								flag2 = true;
								break;
							}
						}
						if (!flag2)
						{
							list.Add(item);
						}
					}
					catch
					{
					}
				}
			}
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[AddItemEvo] Error: " + ex.Message);
		}
		if (list.Count == 0)
		{
			return yOffset;
		}
		yOffset -= Spacing;
		GameObject val4 = CreateTextElement(parent, "EvoHeader", "Evolutions: (click for details)", font, 14f, new Color(0.9f, 0.75f, 0.3f, 1f), (FontStyles)1);
		RectTransform component = val4.GetComponent<RectTransform>();
		component.anchorMin = new Vector2(0f, 1f);
		component.anchorMax = new Vector2(1f, 1f);
		component.pivot = new Vector2(0f, 1f);
		component.anchoredPosition = new Vector2(Padding, yOffset);
		component.sizeDelta = new Vector2(maxWidth - Padding * 2f, 20f);
		yOffset -= 22f;
		float num3 = 36f;
		int num4 = 0;
		foreach (EvolutionFormula item3 in list)
		{
			bool flag3 = item3.Passives != null && item3.Passives.Exists((PassiveRequirement p) => p.RequiresMaxLevel);
			float num5 = num3 + 4f + (flag3 ? 12f : 0f);
			float num6 = Padding + 5f;
			bool isOwned = PlayerOwnsWeapon(item3.BaseWeapon);
			GameObject go = CreateFormulaIcon(parent, $"Weapon{num4}", item3.BaseSprite, isOwned, IsWeaponBanned(item3.BaseWeapon), num3, num6, yOffset);
			AddHoverToGameObject(go, item3.BaseWeapon, null, useClick: true);
			num6 += num3 + 3f;
			if (item3.Passives != null)
			{
				for (int num7 = 0; num7 < item3.Passives.Count; num7++)
				{
					PassiveRequirement passiveRequirement = item3.Passives[num7];
					GameObject val5 = CreateTextElement(parent, $"Plus{num4}_{num7}", "+", font, 14f, new Color(0.8f, 0.8f, 0.8f, 1f), (FontStyles)1);
					RectTransform component2 = val5.GetComponent<RectTransform>();
					component2.anchorMin = new Vector2(0f, 1f);
					component2.anchorMax = new Vector2(0f, 1f);
					component2.pivot = new Vector2(0f, 1f);
					component2.anchoredPosition = new Vector2(num6, yOffset - 4f);
					component2.sizeDelta = new Vector2(14f, num3);
					num6 += 14f;
					bool isBanned = (passiveRequirement.WeaponType.HasValue ? IsWeaponBanned(passiveRequirement.WeaponType.Value) : (passiveRequirement.ItemType.HasValue && IsItemBanned(passiveRequirement.ItemType.Value)));
					GameObject go2 = CreateFormulaIcon(parent, $"Passive{num4}_{num7}", passiveRequirement.Sprite, passiveRequirement.Owned, isBanned, num3, num6, yOffset);
					if (passiveRequirement.WeaponType.HasValue)
					{
						AddHoverToGameObject(go2, passiveRequirement.WeaponType.Value, null, useClick: true);
					}
					else if (passiveRequirement.ItemType.HasValue)
					{
						AddHoverToGameObject(go2, null, passiveRequirement.ItemType.Value, useClick: true);
					}
					if (passiveRequirement.RequiresMaxLevel)
					{
						GameObject val6 = CreateTextElement(parent, $"Max{num4}_{num7}", "MAX", font, 9f, new Color(1f, 0.85f, 0f, 1f), (FontStyles)1);
						RectTransform component3 = val6.GetComponent<RectTransform>();
						component3.anchorMin = new Vector2(0f, 1f);
						component3.anchorMax = new Vector2(0f, 1f);
						component3.pivot = new Vector2(0.5f, 1f);
						component3.anchoredPosition = new Vector2(num6 + num3 / 2f, yOffset - num3);
						component3.sizeDelta = new Vector2(num3, 12f);
						TextMeshProUGUI component4 = val6.GetComponent<TextMeshProUGUI>();
						if ((Object)(object)component4 != (Object)null)
						{
							((TMP_Text)component4).alignment = (TextAlignmentOptions)514;
						}
					}
					num6 += num3 + 3f;
				}
			}
			GameObject val7 = CreateTextElement(parent, $"Arrow{num4}", "→", font, 14f, new Color(0.8f, 0.8f, 0.8f, 1f), (FontStyles)0);
			RectTransform component5 = val7.GetComponent<RectTransform>();
			component5.anchorMin = new Vector2(0f, 1f);
			component5.anchorMax = new Vector2(0f, 1f);
			component5.pivot = new Vector2(0f, 1f);
			component5.anchoredPosition = new Vector2(num6, yOffset - 4f);
			component5.sizeDelta = new Vector2(20f, num3);
			num6 += 20f;
			GameObject go3 = CreateFormulaIcon(parent, $"Evo{num4}", item3.EvolvedSprite, isOwned: false, IsWeaponBanned(item3.EvolvedWeapon), num3, num6, yOffset);
			AddHoverToGameObject(go3, item3.EvolvedWeapon, null, useClick: true);
			num6 += num3 + 6f;
			GameObject val8 = CreateTextElement(parent, $"EvoName{num4}", item3.EvolvedName, font, 11f, new Color(0.75f, 0.75f, 0.8f, 1f), (FontStyles)0);
			RectTransform component6 = val8.GetComponent<RectTransform>();
			component6.anchorMin = new Vector2(0f, 1f);
			component6.anchorMax = new Vector2(1f, 1f);
			component6.pivot = new Vector2(0f, 1f);
			component6.anchoredPosition = new Vector2(num6, yOffset - 5f);
			component6.sizeDelta = new Vector2(maxWidth - num6 - Padding, 16f);
			yOffset -= num5;
			num4++;
		}
		return yOffset;
	}

	private static float AddArcanaSection(Transform parent, TMP_FontAsset font, List<ArcanaInfo> arcanas, float yOffset, float maxWidth)
	{
		if (arcanas == null || arcanas.Count == 0)
		{
			return yOffset;
		}
		yOffset -= Spacing + 2f;
		GameObject val = CreateTextElement(parent, "ArcanaHeader", "Arcana: (click for details)", font, 14f, new Color(0.7f, 0.5f, 0.9f, 1f), (FontStyles)1);
		RectTransform component = val.GetComponent<RectTransform>();
		component.anchorMin = new Vector2(0f, 1f);
		component.anchorMax = new Vector2(0f, 1f);
		component.pivot = new Vector2(0f, 1f);
		component.anchoredPosition = new Vector2(Padding, yOffset);
		component.sizeDelta = new Vector2(maxWidth - Padding * 2f, 22f);
		yOffset -= 28f; // header + gap before first card
		float card = 48f;
		float padding = Padding;
		float nameX = padding + card + 10f;
		float nameW = maxWidth - nameX - Padding;
		for (int i = 0; i < arcanas.Count; i++)
		{
			ArcanaInfo arcanaInfo = arcanas[i];
			Sprite sprite = arcanaInfo.Sprite ?? GameData.GetArcanaSprite(arcanaInfo.Type);
			GameObject go = CreateFormulaIcon(parent, $"ArcanaIcon{i}", sprite, isOwned: false, isBanned: false, card, padding, yOffset);
			AddArcanaHoverToGameObject(go, arcanaInfo.Type);
			string displayName = !string.IsNullOrEmpty(arcanaInfo.Name) ? arcanaInfo.Name : GameData.GetArcanaName(arcanaInfo.Type);
			GameObject val2 = CreateTextElement(parent, $"ArcanaName{i}", displayName, font, 13f, new Color(0.8f, 0.7f, 0.95f, 1f), (FontStyles)0);
			RectTransform component2 = val2.GetComponent<RectTransform>();
			component2.anchorMin = new Vector2(0f, 1f);
			component2.anchorMax = new Vector2(0f, 1f);
			component2.pivot = new Vector2(0f, 1f);
			// Vertically center name next to the card
			float nameMid = (card - 18f) * 0.5f;
			component2.anchoredPosition = new Vector2(nameX, yOffset - nameMid);
			component2.sizeDelta = new Vector2(nameW, 22f);
			// Allow wrap for long arcana titles
			try
			{
				var tmp = val2.GetComponent<TextMeshProUGUI>();
				if ((Object)(object)tmp != (Object)null)
				{
					float nh = FitTmpHeight(tmp, nameW, 18f, 48f);
					float row = Mathf.Max(card, nh + 4f);
					yOffset -= row + 10f;
					continue;
				}
			}
			catch { }
			yOffset -= card + 10f;
		}
		return yOffset;
	}

	private static void ShowArcanaPopup(Transform anchor, ArcanaType arcanaType)
	{
		if (!IsGamePaused())
		{
			HideAllPopups();
			return;
		}
		int num = ((anchor != null) ? ((Object)anchor).GetInstanceID() : 0);
		for (int i = 0; i < popupAnchorIds.Count; i++)
		{
			if (popupAnchorIds[i] == num)
			{
				return;
			}
		}
		int num2 = -1;
		for (int j = 0; j < popupStack.Count; j++)
		{
			if ((Object)(object)popupStack[j] != (Object)null && (Object)(object)anchor != (Object)null && anchor.IsChildOf(popupStack[j].transform))
			{
				num2 = j;
			}
		}
		if (num2 >= 0)
		{
			while (popupStack.Count > num2 + 1)
			{
				HideTopPopup();
			}
		}
		else if (popupStack.Count > 0)
		{
			HideAllPopups();
		}
		Transform val = FindPopupParent(anchor);
		if (!((Object)(object)val == (Object)null))
		{
			Plugin.Dbg($"ShowArcanaPopup {arcanaType} ({GameData.GetArcanaName(arcanaType)})");
			GameObject val2 = CreateArcanaPopup(val, arcanaType);
			if (!((Object)(object)val2 == (Object)null))
			{
				popupStack.Add(val2);
				popupAnchorIds.Add(num);
				PositionPopup(val2, anchor);
				AddPopupHoverTracking(val2);
			}
		}
	}

	private static GameObject CreateArcanaPopup(Transform parent, ArcanaType arcanaType)
	{
		GameData.EnsureLoaded();
		string text = GameData.GetArcanaName(arcanaType);
		string text2 = GameData.GetArcanaDescription(arcanaType);
		Sprite val5 = GameData.GetArcanaSprite(arcanaType);
		if (string.IsNullOrEmpty(text) || text == "Unknown")
		{
			text = GameData.HumanizeEnum(arcanaType.ToString());
		}

		GameObject val = new GameObject("ArcanaTooltipPopup");
		val.transform.SetParent(parent, false);
		RectTransform val2 = val.AddComponent<RectTransform>();
		val2.anchorMin = new Vector2(0.5f, 0.5f);
		val2.anchorMax = new Vector2(0.5f, 0.5f);
		val2.pivot = new Vector2(0f, 1f);
		Image val3 = val.AddComponent<Image>();
		((Graphic)val3).color = PopupBgColor;
		((Graphic)val3).raycastTarget = true;
		Outline val4 = val.AddComponent<Outline>();
		((Shadow)val4).effectColor = new Color(0.6f, 0.4f, 0.8f, 1f);
		((Shadow)val4).effectDistance = new Vector2(2f, 2f);
		float num = 0f - Padding;
		float num2 = 420f;
		TMP_FontAsset font = GetFont();
		if ((Object)(object)font != (Object)null)
		{
			GameObject val6 = new GameObject("TitleRow");
			val6.transform.SetParent(val.transform, false);
			RectTransform val7 = val6.AddComponent<RectTransform>();
			val7.anchorMin = new Vector2(0f, 1f);
			val7.anchorMax = new Vector2(0f, 1f);
			val7.pivot = new Vector2(0f, 1f);
			val7.anchoredPosition = new Vector2(Padding, num);
			val7.sizeDelta = new Vector2(num2 - Padding * 2f, IconSize);
			GameObject val8 = new GameObject("Title");
			val8.transform.SetParent(val6.transform, false);
			RectTransform val9 = val8.AddComponent<RectTransform>();
			val9.anchorMin = Vector2.zero;
			val9.anchorMax = Vector2.one;
			val9.offsetMin = new Vector2(IconSize + Spacing, 0f);
			val9.offsetMax = Vector2.zero;
			TextMeshProUGUI val10 = val8.AddComponent<TextMeshProUGUI>();
			((TMP_Text)val10).font = font;
			((TMP_Text)val10).text = text;
			((TMP_Text)val10).fontSize = 20f;
			((TMP_Text)val10).fontStyle = (FontStyles)1;
			((Graphic)val10).color = new Color(0.8f, 0.7f, 0.95f, 1f);
			((TMP_Text)val10).alignment = (TextAlignmentOptions)513;
			((TMP_Text)val10).enableAutoSizing = true;
			((TMP_Text)val10).fontSizeMin = 12f;
			((TMP_Text)val10).fontSizeMax = 20f;
			((TMP_Text)val10).overflowMode = (TextOverflowModes)1;
			if ((Object)(object)val5 != (Object)null)
			{
				GameObject val11 = new GameObject("ArcanaHeaderIcon");
				val11.transform.SetParent(val6.transform, false);
				RectTransform val12 = val11.AddComponent<RectTransform>();
				val12.anchorMin = new Vector2(0f, 0.5f);
				val12.anchorMax = new Vector2(0f, 0.5f);
				val12.pivot = new Vector2(0f, 0.5f);
				val12.anchoredPosition = new Vector2(0f, 0f);
				val12.sizeDelta = new Vector2(IconSize, IconSize);
				Image val13 = val11.AddComponent<Image>();
				val13.sprite = val5;
				val13.preserveAspect = true;
				((Graphic)val13).raycastTarget = false;
			}
			num -= IconSize + Spacing;
			if (!string.IsNullOrEmpty(text2))
			{
				GameObject val14 = new GameObject("ArcanaDescription");
				val14.transform.SetParent(val.transform, false);
				RectTransform val15 = val14.AddComponent<RectTransform>();
				val15.anchorMin = new Vector2(0f, 1f);
				val15.anchorMax = new Vector2(0f, 1f);
				val15.pivot = new Vector2(0f, 1f);
				val15.anchoredPosition = new Vector2(Padding, num);
				float num3 = num2 - Padding * 2f;
				val15.sizeDelta = new Vector2(num3, 0f);
				TextMeshProUGUI val16 = val14.AddComponent<TextMeshProUGUI>();
				((TMP_Text)val16).font = font;
				((TMP_Text)val16).text = text2;
				((TMP_Text)val16).fontSize = 14f;
				((Graphic)val16).color = new Color(0.85f, 0.85f, 0.9f, 1f);
				((TMP_Text)val16).alignment = (TextAlignmentOptions)257;
				((TMP_Text)val16).enableWordWrapping = true;
				((TMP_Text)val16).overflowMode = (TextOverflowModes)3;
				((TMP_Text)val16).rectTransform.sizeDelta = new Vector2(num3, 0f);
				ContentSizeFitter val17 = val14.AddComponent<ContentSizeFitter>();
				val17.horizontalFit = (ContentSizeFitter.FitMode)0;
				val17.verticalFit = (ContentSizeFitter.FitMode)2;
				((TMP_Text)val16).ForceMeshUpdate(false, false);
				LayoutRebuilder.ForceRebuildLayoutImmediate(val15);
				float num4 = ((((TMP_Text)val16).preferredHeight > 0f) ? ((TMP_Text)val16).preferredHeight : 40f);
				val15.sizeDelta = new Vector2(num3, num4);
				num -= num4 + Spacing;
			}

			// Affects: typed from GameData
			List<WeaponType> allArcanaAffectedWeaponTypes = GameData.GetWeaponsAffectedByArcana(arcanaType);
			List<ItemType> allArcanaAffectedItemTypes = GameData.GetItemsAffectedByArcana(arcanaType);
			// Merge panel/UI captures as extra (non-typed sources sometimes add weapons)
			int arcanaTypeInt = (int)arcanaType;
			if (arcanaUICache.TryGetValue(arcanaTypeInt, out var cached))
			{
				foreach (WeaponType w in cached.weapons)
				{
					if (!allArcanaAffectedWeaponTypes.Contains(w))
						allArcanaAffectedWeaponTypes.Add(w);
				}
				foreach (ItemType it in cached.items)
				{
					if (!allArcanaAffectedItemTypes.Contains(it))
						allArcanaAffectedItemTypes.Add(it);
				}
			}
			int num5 = allArcanaAffectedWeaponTypes.Count + allArcanaAffectedItemTypes.Count;
			Plugin.Dbg($"Arcana popup {arcanaType}: name={text} affects W={allArcanaAffectedWeaponTypes.Count} I={allArcanaAffectedItemTypes.Count} sprite={(val5 != null ? "ok" : "NULL")}");
			if (num5 > 0)
			{
				num -= Spacing;
				GameObject val18 = CreateTextElement(val.transform, "AffectsHeader", "Affects: (click for details)", font, 14f, new Color(0.7f, 0.5f, 0.9f, 1f), (FontStyles)1);
				RectTransform component = val18.GetComponent<RectTransform>();
				component.anchorMin = new Vector2(0f, 1f);
				component.anchorMax = new Vector2(1f, 1f);
				component.pivot = new Vector2(0f, 1f);
				component.anchoredPosition = new Vector2(Padding, num);
				component.sizeDelta = new Vector2(num2 - Padding * 2f, 20f);
				num -= 22f;
				float num6 = 38f;
				float num7 = 6f;
				float num8 = num2 - Padding * 2f;
				int num9 = (int)(num8 / (num6 + num7));
				if (num9 < 1)
				{
					num9 = 1;
				}
				int num10 = 0;
				int num11 = 0;
				foreach (WeaponType item in allArcanaAffectedWeaponTypes)
				{
					float x = Padding + (float)num10 * (num6 + num7);
					bool isOwned = PlayerOwnsWeapon(item);
					Sprite spriteForWeapon = GameData.GetSprite(item) ?? GetSpriteForWeapon(item);
					GameObject go = CreateFormulaIcon(val.transform, $"AffectedWeapon{num11}", spriteForWeapon, isOwned, IsWeaponBanned(item), num6, x, num);
					AddHoverToGameObject(go, item, null, useClick: true);
					num10++;
					if (num10 >= num9)
					{
						num10 = 0;
						num -= num6 + num7;
					}
					num11++;
				}
				foreach (ItemType item2 in allArcanaAffectedItemTypes)
				{
					float x2 = Padding + (float)num10 * (num6 + num7);
					bool isOwned2 = PlayerOwnsItem(item2);
					Sprite spriteForItem = GetSpriteForItem(item2);
					GameObject go2 = CreateFormulaIcon(val.transform, $"AffectedItem{num11}", spriteForItem, isOwned2, IsItemBanned(item2), num6, x2, num);
					AddHoverToGameObject(go2, null, item2, useClick: true);
					num10++;
					if (num10 >= num9)
					{
						num10 = 0;
						num -= num6 + num7;
					}
					num11++;
				}
				if (num10 > 0)
				{
					num -= num6 + num7;
				}
			}
		}
		num -= Padding;
		val2.sizeDelta = new Vector2(num2, 0f - num);
		return val;
	}

	private static void AddArcanaHoverToGameObject(GameObject go, ArcanaType arcanaType)
	{
		EventTrigger component = go.GetComponent<EventTrigger>();
		if (!((Object)(object)component != (Object)null))
		{
			EventTrigger val = go.AddComponent<EventTrigger>();
			ArcanaType captured = arcanaType;
			EventTrigger.Entry val2 = new EventTrigger.Entry();
			// PointerClick — nested popup (same as weapon icons)
			val2.eventID = EventTriggerType.PointerClick;
			((UnityEvent<BaseEventData>)(object)val2.callback).AddListener((UnityEngine.Events.UnityAction<UnityEngine.EventSystems.BaseEventData>)(System.Action<UnityEngine.EventSystems.BaseEventData>)(delegate
			{
				ShowArcanaPopup(go.transform, captured);
			}));
			val.triggers.Add(val2);
		}
	}

	private static GameObject CreateTextElement(Transform parent, string name, string text, TMP_FontAsset font, float fontSize, Color color, FontStyles style)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		GameObject val = new GameObject(name);
		val.transform.SetParent(parent, false);
		val.AddComponent<RectTransform>();
		TextMeshProUGUI val2 = val.AddComponent<TextMeshProUGUI>();
		((TMP_Text)val2).font = font;
		((TMP_Text)val2).text = text;
		((TMP_Text)val2).fontSize = fontSize;
		((Graphic)val2).color = color;
		((TMP_Text)val2).fontStyle = style;
		((TMP_Text)val2).alignment = (TextAlignmentOptions)513;
		return val;
	}

	private static List<WeaponData> GetWeaponDataList(WeaponType type)
	{
		var il2 = GameData.GetWeaponDataList(type);
		if (il2 == null || il2.Count == 0) return null;
		var list = new List<WeaponData>();
		for (int i = 0; i < il2.Count; i++) if (il2[i] != null) list.Add(il2[i]);
		return list.Count > 0 ? list : null;
	}

	private static void PositionPopup(GameObject popup, Transform anchor)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		RectTransform component = popup.GetComponent<RectTransform>();
		if ((Object)(object)component == (Object)null)
		{
			return;
		}
		Transform parent = popup.transform.parent;
		if ((Object)(object)parent == (Object)null)
		{
			return;
		}
		Vector3 val = parent.InverseTransformPoint(anchor.position);
		RectTransform component2 = ((Component)parent).GetComponent<RectTransform>();
		Rect rect;
		if ((Object)(object)component2 != (Object)null)
		{
			rect = component2.rect;
			Vector2 center = rect.center;
			val.x -= center.x;
			val.y -= center.y;
		}
		float num;
		float num2;
		if (usingController && equipmentNavMode && !interactiveMode && popupStack.Count <= 1)
		{
			num = val.x + 40f;
			num2 = val.y - 60f;
		}
		else if (usingController && popupStack.Count <= 1 && !interactiveMode)
		{
			num = val.x - component.sizeDelta.x * 0.5f;
			num2 = val.y + 15f;
		}
		else if (usingController)
		{
			num = val.x + 15f;
			num2 = val.y + 15f;
		}
		else
		{
			num = val.x - 15f;
			num2 = val.y + 40f;
		}
		float x = component.sizeDelta.x;
		float y = component.sizeDelta.y;
		if ((Object)(object)component2 != (Object)null)
		{
			rect = component2.rect;
			float width = rect.width;
			rect = component2.rect;
			float height = rect.height;
			if (num + x > width / 2f)
			{
				num = width / 2f - x;
			}
			if (num < (0f - width) / 2f)
			{
				num = (0f - width) / 2f;
			}
			if (num2 > height / 2f)
			{
				num2 = height / 2f;
			}
			if (num2 - y < (0f - height) / 2f)
			{
				num2 = (0f - height) / 2f + y;
			}
			float num3 = num + 20f;
			float num4 = num + x - 20f;
			float num5 = num2 - y + 20f;
			float num6 = num2 - 20f;
			if (!equipmentNavMode && (val.x < num3 || val.x > num4 || val.y < num5 || val.y > num6))
			{
				if (val.x < num3)
				{
					num = val.x - 20f;
				}
				if (val.x > num4)
				{
					num = val.x - x + 20f;
				}
			}
		}
		component.anchoredPosition = new Vector2(num, num2);
	}

	private static Transform FindPopupParent(Transform anchor)
	{
		Transform val = anchor;
		while ((Object)(object)val != (Object)null)
		{
			if (((Object)val).name.StartsWith("View - ") || ((Object)val).name == "Safe Area")
			{
				return val;
			}
			val = val.parent;
		}
		return anchor.root;
	}

	private static void AddPopupHoverTracking(GameObject popup)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		EventTrigger val = popup.AddComponent<EventTrigger>();
		int thisPopupIndex = popupStack.Count - 1;
		EventTrigger.Entry val2 = new EventTrigger.Entry();
		val2.eventID = (EventTriggerType)0;
		((UnityEvent<BaseEventData>)(object)val2.callback).AddListener((UnityEngine.Events.UnityAction<UnityEngine.EventSystems.BaseEventData>)(System.Action<UnityEngine.EventSystems.BaseEventData>)(delegate
		{
			mouseOverPopupIndex = thisPopupIndex;
		}));
		val.triggers.Add(val2);
		EventTrigger.Entry val3 = new EventTrigger.Entry();
		val3.eventID = (EventTriggerType)1;
		((UnityEvent<BaseEventData>)(object)val3.callback).AddListener((UnityEngine.Events.UnityAction<UnityEngine.EventSystems.BaseEventData>)(System.Action<UnityEngine.EventSystems.BaseEventData>)(delegate
		{
			if (mouseOverPopupIndex == thisPopupIndex)
			{
				mouseOverPopupIndex = -1;
			}
			DelayFrames(10, () => { int closeFromIndex = mouseOverPopupIndex + 1; if (closeFromIndex < 0) closeFromIndex = 0; while (popupStack.Count > closeFromIndex) HideTopPopup(); });
		}));
		val.triggers.Add(val3);
	}

	public static void HidePopup()
	{
		HideAllPopups();
	}

	private static void HideAllPopups()
	{
		mouseOverPopupIndex = -1;
		interactiveMode = false;
		passivePopupShown = false;
		formulaIcons.Clear();
		currentFormulaIndex = -1;
		formulaIconData.Clear();
		if ((Object)(object)interactiveHighlight != (Object)null)
		{
			Object.Destroy((Object)(object)interactiveHighlight);
			interactiveHighlight = null;
		}
		foreach (GameObject item in popupStack)
		{
			if ((Object)(object)item != (Object)null)
			{
				Object.Destroy((Object)(object)item);
			}
		}
		popupStack.Clear();
		popupAnchorIds.Clear();
	}

	private static void HideTopPopup()
	{
		if (popupStack.Count <= 0)
		{
			return;
		}
		GameObject val = popupStack[popupStack.Count - 1];
		if ((Object)(object)val != (Object)null)
		{
			Il2CppArrayBase<EventTrigger> componentsInChildren = val.GetComponentsInChildren<EventTrigger>(false);
			foreach (EventTrigger item in componentsInChildren)
			{
				formulaIconData.Remove(((Object)((Component)item).gameObject).GetInstanceID());
			}
			Object.Destroy((Object)(object)val);
		}
		popupStack.RemoveAt(popupStack.Count - 1);
		popupAnchorIds.RemoveAt(popupAnchorIds.Count - 1);
	}

	private static void UpdateCollectionHover()
	{
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, (GameObject, WeaponType?, ItemType?, object)> collectionIcon in collectionIcons)
		{
			if ((Object)(object)collectionIcon.Value.Item1 == (Object)null)
			{
				list.Add(collectionIcon.Key);
			}
		}
		foreach (int item2 in list)
		{
			collectionIcons.Remove(item2);
		}
		Vector3 mousePosition = Input.mousePosition;
		Vector2 mouse = (Vector2)mousePosition;
		// Pick the *smallest* containing rect so per-icon children beat parent row panels
		int num = -1;
		WeaponType? weaponType = null;
		ItemType? itemType = null;
		object obj = null;
		float bestArea = float.MaxValue;
		foreach (KeyValuePair<int, (GameObject, WeaponType?, ItemType?, object)> collectionIcon2 in collectionIcons)
		{
			GameObject item = collectionIcon2.Value.Item1;
			if ((Object)(object)item == (Object)null || !item.activeInHierarchy)
			{
				continue;
			}
			RectTransform component = item.GetComponent<RectTransform>();
			if ((Object)(object)component == (Object)null)
			{
				continue;
			}
			bool hit = RectTransformUtility.RectangleContainsScreenPoint(component, mouse, (Camera)null)
				|| RectTransformUtility.RectangleContainsScreenPoint(component, mouse, Camera.main);
			if (!hit)
			{
				// Also try canvas camera if any
				var canvas = item.GetComponentInParent<Canvas>();
				if ((Object)(object)canvas != (Object)null && canvas.renderMode != RenderMode.ScreenSpaceOverlay && (Object)(object)canvas.worldCamera != (Object)null)
				{
					hit = RectTransformUtility.RectangleContainsScreenPoint(component, mouse, canvas.worldCamera);
				}
			}
			if (!hit)
			{
				continue;
			}
			float area = Mathf.Abs(component.rect.width * component.rect.height);
			// Prefer smaller hit targets (actual icons over whole EvolutionItemUI rows)
			if (area < bestArea || (Mathf.Approximately(area, bestArea) && num == -1))
			{
				bestArea = area;
				num = collectionIcon2.Key;
				weaponType = collectionIcon2.Value.Item2;
				itemType = collectionIcon2.Value.Item3;
				obj = collectionIcon2.Value.Item4;
			}
		}
		if (num != -1 && num != currentCollectionHoverId)
		{
			if (num != pendingCollectionHoverId)
			{
				pendingCollectionHoverId = num;
				pendingCollectionWeapon = weaponType;
				pendingCollectionItem = itemType;
				pendingCollectionArcana = obj;
				collectionHoverStartTime = Time.unscaledTime;
				return;
			}
			float num2 = Time.unscaledTime - collectionHoverStartTime;
			if (num2 >= CollectionHoverDelay)
			{
				currentCollectionHoverId = num;
				pendingCollectionHoverId = -1;
				ShowCollectionPopup(weaponType, itemType, pendingCollectionArcana);
			}
		}
		else if (num == currentCollectionHoverId)
		{
			pendingCollectionHoverId = -1;
		}
		else
		{
			// Left all icons — clear pending; hide popup only after leaving the popup itself
			pendingCollectionHoverId = -1;
		}
	}

	private static void ShowCollectionPopup(WeaponType? weaponType, ItemType? itemType, object arcanaType = null)
	{
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		if (interactiveMode && (Object)(object)collectionPopup != (Object)null)
		{
			collectionPopupBackStack.Add(currentCollectionPopupData);
		}
		HideCollectionPopup();
		currentCollectionPopupData = (weapon: weaponType, item: itemType, arcana: arcanaType);
		if (cachedDataManager == null)
		{
			TryCacheDataManagerStatic();
		}
		Transform val = null;
		GameObject val2 = GameObject.Find("UI/Canvas - App/Safe Area/View - Collections");
		if ((Object)(object)val2 != (Object)null)
		{
			val = val2.transform;
		}
		else
		{
			Il2CppArrayBase<Canvas> val3 = Object.FindObjectsOfType<Canvas>();
			for (int i = 0; i < val3.Count; i++)
			{
				if ((Object)(object)val3[i] != (Object)null && ((Component)val3[i]).gameObject.activeInHierarchy)
				{
					val = ((Component)val3[i]).transform;
					break;
				}
			}
		}
		if ((Object)(object)val == (Object)null)
		{
			return;
		}
		if (arcanaType != null)
		{
			ArcanaType parsed = ArcanaType.VOID;
			bool ok = false;
			try
			{
				if (arcanaType is ArcanaType at)
				{
					parsed = at;
					ok = true;
				}
				else if (Enum.TryParse(arcanaType.ToString(), ignoreCase: true, out ArcanaType at2))
				{
					parsed = at2;
					ok = true;
				}
				else
				{
					int n = GetArcanaTypeInt(arcanaType);
					if (n >= 0 && Enum.IsDefined(typeof(ArcanaType), n))
					{
						parsed = (ArcanaType)n;
						ok = true;
					}
				}
			}
			catch (Exception ex)
			{
				Plugin.Log.LogWarning($"[CollectionPopup] Arcana parse: {ex.Message}");
			}
			if (!ok || parsed == ArcanaType.VOID)
			{
				Plugin.Log.LogWarning($"[CollectionPopup] Could not resolve arcana type for {arcanaType}");
				return;
			}
			collectionPopup = CreateArcanaPopup(val, parsed);
		}
		else
		{
			collectionPopup = CreatePopup(val, weaponType, itemType);
		}
		if (!((Object)(object)collectionPopup == (Object)null))
		{
			RectTransform component = collectionPopup.GetComponent<RectTransform>();
			GameObject val4 = GameObject.Find("UI/Canvas - App/Safe Area/View - Collections/FilterPanel");
			component.anchorMin = new Vector2(0f, 0f);
			component.anchorMax = new Vector2(0f, 0f);
			component.pivot = new Vector2(0f, 1f);
			if ((Object)(object)val4 != (Object)null)
			{
				RectTransform component2 = val4.GetComponent<RectTransform>();
				float x = component2.offsetMin.x;
				float y = component2.offsetMin.y;
				component.anchoredPosition = new Vector2(x, y - 15f);
			}
			else
			{
				component.anchoredPosition = new Vector2(1450f, 930f);
			}
		}
	}

	private static void HideCollectionPopup()
	{
		if ((Object)(object)collectionPopup != (Object)null)
		{
			Il2CppArrayBase<EventTrigger> componentsInChildren = collectionPopup.GetComponentsInChildren<EventTrigger>(false);
			foreach (EventTrigger item in componentsInChildren)
			{
				formulaIconData.Remove(((Object)((Component)item).gameObject).GetInstanceID());
			}
			Object.Destroy((Object)(object)collectionPopup);
			collectionPopup = null;
		}
		passivePopupShown = false;
	}

	private static bool IsUnderGameUI(Transform t)
	{
		Transform val = t;
		while ((Object)(object)val != (Object)null)
		{
			if (((Object)val).name == "GAME UI")
			{
				return true;
			}
			val = val.parent;
		}
		return false;
	}

	public unsafe static void RegisterWeaponUI(int instanceId, GameObject go, WeaponType type, bool isAddMethod = false)
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		if (((object)(*(WeaponType*)(&type))/*cast due to constrained. prefix*/).ToString() == "DEFANG")
		{
			return;
		}
		if (!IsUnderGameUI(go.transform))
		{
			collectionIcons[instanceId] = (go, type, null, null);
			return;
		}
		uiToWeaponType[instanceId] = type;
		GameObject val = null;
		val = ((!isAddMethod) ? FindIconInUI(go) : FindLastImageChild(go));
		if ((Object)(object)val != (Object)null)
		{
			AddHoverToGameObject(val, type, null);
		}
		else
		{
			AddHoverToGameObject(go, type, null);
		}
	}

	private static GameObject FindLastImageChild(GameObject parent)
	{
		return FindLastImageChildRecursive(parent.transform, 0);
	}

	private static GameObject FindLastImageChildRecursive(Transform parent, int depth)
	{
		if (depth > 3)
		{
			return null;
		}
		for (int num = parent.childCount - 1; num >= 0; num--)
		{
			Transform child = parent.GetChild(num);
			string text = ((Object)child).name.ToLower();
			if (text.Contains("panel") || text.Contains("container") || text.Contains("group"))
			{
				GameObject val = FindLastImageChildRecursive(child, depth + 1);
				if ((Object)(object)val != (Object)null)
				{
					return val;
				}
			}
			else if (!text.Contains("background") && !text.Contains("frame"))
			{
				Image component = ((Component)child).GetComponent<Image>();
				if ((Object)(object)component != (Object)null && (Object)(object)component.sprite != (Object)null)
				{
					return ((Component)child).gameObject;
				}
			}
		}
		return null;
	}

	public unsafe static void RegisterItemUI(int instanceId, GameObject go, ItemType type, bool isAddMethod = false)
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		if (((object)(*(ItemType*)(&type))/*cast due to constrained. prefix*/).ToString() == "DEFANG")
		{
			return;
		}
		if (!IsUnderGameUI(go.transform))
		{
			collectionIcons[instanceId] = (go, null, type, null);
			return;
		}
		uiToItemType[instanceId] = type;
		GameObject val = null;
		val = ((!isAddMethod) ? FindIconInUI(go) : FindLastImageChild(go));
		if ((Object)(object)val != (Object)null)
		{
			AddHoverToGameObject(val, null, type);
		}
		else
		{
			AddHoverToGameObject(go, null, type);
		}
	}

	public static void RegisterArcanaUI(int instanceId, GameObject go, object arcanaType)
	{
		if (!IsUnderGameUI(go.transform))
		{
			collectionIcons[instanceId] = (go, null, null, arcanaType);
		}
	}

	// ── Stage Selection relic icons ──────────────────────────────────────

	public static void RegisterStageRelicIcon(GameObject go, ItemType item, string label, string description, Sprite sprite)
	{
		if ((Object)(object)go == (Object)null) return;
		int id = ((Object)go).GetInstanceID();
		stageRelicIcons[id] = new MapIconInfo
		{
			Go = go,
			Item = item,
			Weapon = null,
			Label = label ?? GameData.GetItemName(item),
			Description = description ?? GameData.GetItemDescription(item),
			Sprite = sprite ?? GameData.GetItemSprite(item)
		};
	}

	public static void RegisterCharacterIcon(GameObject go, CharacterType character, string label, string description, Sprite sprite)
	{
		if (!Plugin.CharacterTooltipsEnabled) return;
		if ((Object)(object)go == (Object)null) return;
		int id = ((Object)go).GetInstanceID();
		characterIcons[id] = new MapIconInfo
		{
			Go = go,
			Item = null,
			Weapon = null,
			Label = label ?? character.ToString(),
			Description = description ?? "",
			Sprite = sprite
		};
	}

	public static bool HasCharacterIcon(int instanceId) => characterIcons.ContainsKey(instanceId);

	public static void ClearCharacterIcons()
	{
		characterIcons.Clear();
		currentCharacterHoverId = -1;
		pendingCharacterHoverId = -1;
		HideCharacterPopup();
	}

	public static void RegisterAdventureIcon(GameObject go, string label, string description, Sprite sprite)
	{
		if (!Plugin.AdventureTooltipsEnabled) return;
		if ((Object)(object)go == (Object)null) return;
		int id = ((Object)go).GetInstanceID();
		adventureIcons[id] = new MapIconInfo
		{
			Go = go,
			Item = null,
			Weapon = null,
			Label = label ?? "Adventure",
			Description = description ?? "",
			Sprite = sprite
		};
	}

	public static bool HasAdventureIcon(int instanceId) => adventureIcons.ContainsKey(instanceId);

	public static void ClearAdventureIcons()
	{
		adventureIcons.Clear();
		currentAdventureHoverId = -1;
		pendingAdventureHoverId = -1;
		HideAdventurePopup();
	}

	private static void UpdateAdventureHover()
	{
		// prune
		List<int> dead = null;
		foreach (var kv in adventureIcons)
		{
			if ((Object)(object)kv.Value.Go == (Object)null)
			{
				dead ??= new List<int>();
				dead.Add(kv.Key);
			}
		}
		if (dead != null)
			foreach (int k in dead) adventureIcons.Remove(k);
		if (adventureIcons.Count == 0) return;

		Vector2 mouse = Input.mousePosition;
		bool hit = false;
		int hitId = -1;
		MapIconInfo hitInfo = default;
		if (AdventureSelectPatches.TryFindHovered(mouse, out hitId, out GameObject go)
			&& adventureIcons.TryGetValue(hitId, out hitInfo))
		{
			hit = true;
			if ((Object)(object)go != (Object)null)
				hitInfo.Go = go;
		}

		if (hit)
		{
			if (hitId != pendingAdventureHoverId)
			{
				pendingAdventureHoverId = hitId;
				adventureHoverStartTime = Time.unscaledTime;
				if (currentAdventureHoverId != -1 && currentAdventureHoverId != hitId)
				{
					currentAdventureHoverId = -1;
					HideAdventurePopup();
				}
			}
			else if (Time.unscaledTime - adventureHoverStartTime >= Plugin.TooltipHoverDelay)
			{
				if (currentAdventureHoverId != hitId)
				{
					currentAdventureHoverId = hitId;
					ShowAdventurePopup(hitInfo);
				}
			}
		}
		else
		{
			pendingAdventureHoverId = -1;
			if (currentAdventureHoverId != -1)
			{
				currentAdventureHoverId = -1;
				HideAdventurePopup();
			}
		}
	}

	private static void ShowAdventurePopup(MapIconInfo info)
	{
		HideAdventurePopup();
		Transform parent = null;
		try
		{
			GameObject page = GameObject.Find("UI/Canvas - App");
			if ((Object)(object)page != (Object)null)
				parent = page.transform;
		}
		catch { }
		if ((Object)(object)parent == (Object)null && (Object)(object)info.Go != (Object)null)
		{
			var c = info.Go.GetComponentInParent<Canvas>();
			if ((Object)(object)c != (Object)null)
				parent = ((Component)c).transform;
		}
		if ((Object)(object)parent == (Object)null) return;

		AdventureSelectPatches.TooltipData data = null;
		try { AdventureSelectPatches.TryGetTooltipData(info.Go, out data); } catch { }

		if (data != null)
			adventurePopup = CreateAdventureDetailPopup(parent, data);
		else
			adventurePopup = CreateSimpleMapPopup(parent, info.Label, info.Description, info.Sprite);

		if ((Object)(object)adventurePopup != (Object)null)
		{
			DisablePopupRaycasts(adventurePopup);
			if ((Object)(object)info.Go != (Object)null)
				PositionPopup(adventurePopup, info.Go.transform);
		}
	}

	/// <summary>Adventure card tooltip with weapon icon strip + character list; TMP-sized.</summary>
	private static GameObject CreateAdventureDetailPopup(Transform parent, AdventureSelectPatches.TooltipData data)
	{
		if (data == null) return null;
		GameObject root = new GameObject("AdventureTooltipPopup");
		root.transform.SetParent(parent, false);
		RectTransform rootRt = root.AddComponent<RectTransform>();
		rootRt.anchorMin = new Vector2(0.5f, 0.5f);
		rootRt.anchorMax = new Vector2(0.5f, 0.5f);
		rootRt.pivot = new Vector2(0f, 1f);
		Image bg = root.AddComponent<Image>();
		((Graphic)bg).color = PopupBgColor;
		((Graphic)bg).raycastTarget = false;
		Outline ol = root.AddComponent<Outline>();
		((Shadow)ol).effectColor = new Color(0.9f, 0.75f, 0.3f, 1f);
		((Shadow)ol).effectDistance = new Vector2(2f, 2f);

		TMP_FontAsset font = GetFont();
		if ((Object)(object)font == (Object)null)
		{
			Object.Destroy((Object)(object)root);
			return null;
		}

		const float minW = 300f;
		const float maxW = 440f;
		const float iconSz = 32f;
		float width = minW;
		try
		{
			float need = MeasureTmpPreferredWidth(font, data.Title ?? "Adventure", 17f, true) + 56f + Padding * 2f;
			width = Mathf.Clamp(need, minW, maxW);
		}
		catch { }
		// Weapon strip may need more width
		if (data.Weapons != null && data.Weapons.Count > 0)
		{
			float strip = data.Weapons.Count * (iconSz + 4f) + Padding * 2f;
			width = Mathf.Clamp(Mathf.Max(width, strip), minW, maxW);
		}

		float contentW = width - Padding * 2f;
		float y = -Padding;
		var gold = new Color(0.95f, 0.8f, 0.35f, 1f);
		var soft = SoftWhite();

		// Title
		float titleH = 40f;
		GameObject titleRow = new GameObject("Title");
		titleRow.transform.SetParent(root.transform, false);
		RectTransform tr = titleRow.AddComponent<RectTransform>();
		tr.anchorMin = new Vector2(0f, 1f);
		tr.anchorMax = new Vector2(0f, 1f);
		tr.pivot = new Vector2(0f, 1f);
		tr.anchoredPosition = new Vector2(Padding, y);
		float tx = 0f;
		if ((Object)(object)data.Icon != (Object)null)
		{
			AddUiIcon(titleRow.transform, data.Icon, 0f, -titleH * 0.5f, titleH - 4f);
			tx = titleH;
		}
		var titleTmp = AddUiText(titleRow.transform, "Name", data.Title ?? "Adventure", font, 17f, Color.white, true,
			tx + 4f, 0f, contentW - tx - 4f, titleH, (TextAlignmentOptions)513);
		float th = FitTmpHeight(titleTmp, contentW - tx - 4f, 20f, 64f);
		titleH = Mathf.Max(titleH, th + 4f);
		tr.sizeDelta = new Vector2(contentW, titleH);
		y -= titleH + 6f;

		if (!string.IsNullOrEmpty(data.StageSet))
		{
			AddUiText(root.transform, "StageSet", "Stage set: " + data.StageSet, font, 12f, soft, false,
				Padding, y, contentW, 18f, (TextAlignmentOptions)257);
			y -= 20f;
		}

		// Weapons with icons
		if (data.Weapons != null && data.Weapons.Count > 0)
		{
			AddUiText(root.transform, "WepHdr", $"Weapons ({data.Weapons.Count})", font, 12f, gold, true,
				Padding, y, contentW, 18f, (TextAlignmentOptions)257);
			y -= 18f + 4f;

			// Icon strip
			float stripH = iconSz + 4f;
			GameObject strip = new GameObject("WeaponStrip");
			strip.transform.SetParent(root.transform, false);
			RectTransform sr = strip.AddComponent<RectTransform>();
			sr.anchorMin = new Vector2(0f, 1f);
			sr.anchorMax = new Vector2(0f, 1f);
			sr.pivot = new Vector2(0f, 1f);
			sr.anchoredPosition = new Vector2(Padding, y);
			sr.sizeDelta = new Vector2(contentW, stripH);
			float x = 0f;
			int iconCount = 0;
			foreach (var w in data.Weapons)
			{
				if (iconCount >= 12) break;
				if ((Object)(object)w.Sprite != (Object)null)
				{
					AddUiIcon(strip.transform, w.Sprite, x, -stripH * 0.5f, iconSz);
					x += iconSz + 4f;
					iconCount++;
				}
			}
			y -= stripH + 4f;

			// Name list (compact)
			var names = new System.Text.StringBuilder();
			int shown = 0;
			foreach (var w in data.Weapons)
			{
				if (shown >= 10) break;
				if (shown > 0) names.Append(", ");
				names.Append(w.Name);
				shown++;
			}
			if (data.Weapons.Count > shown)
				names.Append($" … +{data.Weapons.Count - shown}");
			var namesTmp = AddUiText(root.transform, "WepNames", names.ToString(), font, 11f, soft, false,
				Padding, y, contentW, 30f, (TextAlignmentOptions)257);
			float nh = FitTmpHeight(namesTmp, contentW, 16f, 80f);
			y -= nh + 6f;
		}

		// Characters
		if (data.Characters != null && data.Characters.Count > 0)
		{
			AddUiText(root.transform, "CharHdr", $"Characters ({data.Characters.Count})", font, 12f, gold, true,
				Padding, y, contentW, 18f, (TextAlignmentOptions)257);
			y -= 18f + 2f;
			var cb = new System.Text.StringBuilder();
			int shown = 0;
			foreach (var c in data.Characters)
			{
				if (shown >= 12) break;
				if (shown > 0) cb.Append(", ");
				cb.Append(c);
				shown++;
			}
			if (data.Characters.Count > shown)
				cb.Append($" … +{data.Characters.Count - shown}");
			var ct = AddUiText(root.transform, "Chars", cb.ToString(), font, 12f, soft, false,
				Padding, y, contentW, 30f, (TextAlignmentOptions)257);
			float ch = FitTmpHeight(ct, contentW, 16f, 100f);
			y -= ch + 6f;
		}

		if (data.ProgressGoalCount > 0)
		{
			AddUiText(root.transform, "Prog", $"Progress goals: {data.ProgressGoalCount}", font, 12f, soft, false,
				Padding, y, contentW, 18f, (TextAlignmentOptions)257);
			y -= 20f;
		}

		y -= Padding;
		rootRt.sizeDelta = new Vector2(width, Mathf.Abs(y));
		try
		{
			Canvas.ForceUpdateCanvases();
			LayoutRebuilder.ForceRebuildLayoutImmediate(rootRt);
		}
		catch { }
		return root;
	}

	private static void HideAdventurePopup()
	{
		if ((Object)(object)adventurePopup != (Object)null)
		{
			Object.Destroy((Object)(object)adventurePopup);
			adventurePopup = null;
		}
	}

	public static void ClearStageRelicIcons()
	{
		stageRelicIcons.Clear();
		currentStageRelicHoverId = -1;
		pendingStageRelicHoverId = -1;
		HideStageRelicPopup();
	}

	/// <summary>Controller: dwell on EventSystem-selected stage relic icon.</summary>
	private static void UpdateStageRelicControllerDwell()
	{
		List<int> dead = null;
		foreach (var kv in stageRelicIcons)
		{
			if ((Object)(object)kv.Value.Go == (Object)null)
			{
				dead ??= new List<int>();
				dead.Add(kv.Key);
			}
		}
		if (dead != null)
			foreach (int k in dead) stageRelicIcons.Remove(k);

		EventSystem es = EventSystem.current;
		if ((Object)(object)es == (Object)null || stageRelicIcons.Count == 0)
			return;
		GameObject sel = es.currentSelectedGameObject;
		if ((Object)(object)sel == (Object)null)
		{
			pendingStageRelicHoverId = -1;
			return;
		}
		int id = ((Object)sel).GetInstanceID();
		if (!stageRelicIcons.TryGetValue(id, out MapIconInfo info))
		{
			// Parent walk
			Transform p = sel.transform;
			bool found = false;
			while ((Object)(object)p != (Object)null)
			{
				id = ((Object)p.gameObject).GetInstanceID();
				if (stageRelicIcons.TryGetValue(id, out info))
				{
					found = true;
					break;
				}
				p = p.parent;
			}
			if (!found)
			{
				pendingStageRelicHoverId = -1;
				return;
			}
		}
		if (id != pendingStageRelicHoverId)
		{
			pendingStageRelicHoverId = id;
			stageRelicHoverStartTime = Time.unscaledTime;
			return;
		}
		if (Time.unscaledTime - stageRelicHoverStartTime >= Plugin.ControllerDwellDelay)
		{
			if (currentStageRelicHoverId != id)
			{
				currentStageRelicHoverId = id;
				ShowStageRelicPopup(info);
			}
		}
	}

	private static void UpdateStageRelicHover()
	{
		List<int> dead = null;
		foreach (var kv in stageRelicIcons)
		{
			if ((Object)(object)kv.Value.Go == (Object)null)
			{
				dead ??= new List<int>();
				dead.Add(kv.Key);
			}
		}
		if (dead != null)
		{
			foreach (int k in dead) stageRelicIcons.Remove(k);
		}
		if (stageRelicIcons.Count == 0)
		{
			HideStageRelicPopup();
			return;
		}

		Vector2 mouse = (Vector2)Input.mousePosition;
		int hitId = -1;
		MapIconInfo hitInfo = default;
		float bestArea = float.MaxValue;

		foreach (var kv in stageRelicIcons)
		{
			GameObject go = kv.Value.Go;
			if ((Object)(object)go == (Object)null || !go.activeInHierarchy) continue;
			RectTransform rt = go.GetComponent<RectTransform>();
			if ((Object)(object)rt == (Object)null) continue;

			Camera cam = null;
			var canvas = go.GetComponentInParent<Canvas>();
			if ((Object)(object)canvas != (Object)null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
				cam = canvas.worldCamera != null ? canvas.worldCamera : Camera.main;

			bool hit = RectTransformUtility.RectangleContainsScreenPoint(rt, mouse, cam)
				|| RectTransformUtility.RectangleContainsScreenPoint(rt, mouse, null)
				|| RectTransformUtility.RectangleContainsScreenPoint(rt, mouse, Camera.main);

			if (!hit)
			{
				Vector3[] corners = new Vector3[4];
				rt.GetWorldCorners(corners);
				Vector2 min = RectTransformUtility.WorldToScreenPoint(cam, corners[0]);
				Vector2 max = RectTransformUtility.WorldToScreenPoint(cam, corners[2]);
				Vector2 center = (min + max) * 0.5f;
				float half = Mathf.Max(22f, Mathf.Max(Mathf.Abs(max.x - min.x), Mathf.Abs(max.y - min.y)) * 0.5f + 10f);
				hit = (mouse - center).sqrMagnitude <= half * half;
			}
			if (!hit) continue;

			float area = Mathf.Max(1f, Mathf.Abs(rt.rect.width * rt.rect.height));
			if (area < bestArea)
			{
				bestArea = area;
				hitId = kv.Key;
				hitInfo = kv.Value;
			}
		}

		if (hitId != -1 && hitId != currentStageRelicHoverId)
		{
			if (hitId != pendingStageRelicHoverId)
			{
				pendingStageRelicHoverId = hitId;
				stageRelicHoverStartTime = Time.unscaledTime;
				return;
			}
			if (Time.unscaledTime - stageRelicHoverStartTime >= CollectionHoverDelay * 0.4f)
			{
				currentStageRelicHoverId = hitId;
				pendingStageRelicHoverId = -1;
				ShowStageRelicPopup(hitInfo);
			}
		}
		else if (hitId == currentStageRelicHoverId)
		{
			pendingStageRelicHoverId = -1;
		}
		else
		{
			pendingStageRelicHoverId = -1;
			if (hitId == -1 && currentStageRelicHoverId != -1)
			{
				if ((Object)(object)stageRelicPopup != (Object)null)
				{
					RectTransform prt = stageRelicPopup.GetComponent<RectTransform>();
					bool overPopup = (Object)(object)prt != (Object)null
						&& (RectTransformUtility.RectangleContainsScreenPoint(prt, mouse, null)
							|| RectTransformUtility.RectangleContainsScreenPoint(prt, mouse, Camera.main));
					if (!overPopup)
					{
						currentStageRelicHoverId = -1;
						HideStageRelicPopup();
					}
				}
				else
				{
					currentStageRelicHoverId = -1;
				}
			}
		}
	}

	private static void ShowStageRelicPopup(MapIconInfo info)
	{
		HideStageRelicPopup();
		Transform parent = null;
		try
		{
			// Prefer StageSelect / app canvas
			GameObject page = GameObject.Find("UI/Canvas - App/Safe Area/View - StageSelection");
			if ((Object)(object)page == (Object)null)
				page = GameObject.Find("UI/Canvas - App");
			if ((Object)(object)page != (Object)null)
				parent = page.transform;
		}
		catch { }
		if ((Object)(object)parent == (Object)null && (Object)(object)info.Go != (Object)null)
		{
			var c = info.Go.GetComponentInParent<Canvas>();
			if ((Object)(object)c != (Object)null)
				parent = ((Component)c).transform;
		}
		if ((Object)(object)parent == (Object)null) return;

		string label = info.Label;
		string desc = info.Description;
		Sprite spr = info.Sprite;
		if (info.Item.HasValue)
		{
			if (string.IsNullOrEmpty(label)) label = GameData.GetItemName(info.Item.Value);
			if (string.IsNullOrEmpty(desc)) desc = GameData.GetItemDescription(info.Item.Value);
			spr = GameData.GetItemSprite(info.Item.Value) ?? spr;
		}

		stageRelicPopup = CreateSimpleMapPopup(parent, label, desc, spr);
		if ((Object)(object)stageRelicPopup != (Object)null)
		{
			DisablePopupRaycasts(stageRelicPopup);
			if ((Object)(object)info.Go != (Object)null)
				PositionPopup(stageRelicPopup, info.Go.transform);
		}
		Plugin.Dbg($"StageRelic popup item={info.Item} label={label}");
	}

	private static void HideStageRelicPopup()
	{
		if ((Object)(object)stageRelicPopup != (Object)null)
		{
			Object.Destroy((Object)(object)stageRelicPopup);
			stageRelicPopup = null;
		}
	}

	// ── Character Selection icons ────────────────────────────────────────

	private static void UpdateCharacterControllerDwell()
	{
		PruneDeadCharacterIcons();
		EventSystem es = EventSystem.current;
		if ((Object)(object)es == (Object)null || characterIcons.Count == 0)
			return;
		GameObject sel = es.currentSelectedGameObject;
		if ((Object)(object)sel == (Object)null)
		{
			pendingCharacterHoverId = -1;
			return;
		}
		if (!TryResolveCharacterIcon(sel, out int id, out MapIconInfo info))
		{
			pendingCharacterHoverId = -1;
			return;
		}
		if (id != pendingCharacterHoverId)
		{
			pendingCharacterHoverId = id;
			characterHoverStartTime = Time.unscaledTime;
			return;
		}
		if (Time.unscaledTime - characterHoverStartTime >= Plugin.ControllerDwellDelay)
		{
			if (currentCharacterHoverId != id)
			{
				currentCharacterHoverId = id;
				ShowCharacterPopup(info);
			}
		}
	}

	private static void UpdateCharacterHover()
	{
		PruneDeadCharacterIcons();
		if (characterIcons.Count == 0) return;

		Vector2 mouse = Input.mousePosition;
		bool hit = false;
		int hitId = -1;
		MapIconInfo hitInfo = default;

		if (CharacterSelectPatches.TryFindHoveredCard(mouse, out hitId, out GameObject cardGo)
			&& characterIcons.TryGetValue(hitId, out hitInfo))
		{
			hit = true;
			// Keep Go in sync for positioning / live rebuild
			if ((Object)(object)cardGo != (Object)null)
				hitInfo.Go = cardGo;
		}

		if (hit)
		{
			if (hitId != pendingCharacterHoverId)
			{
				pendingCharacterHoverId = hitId;
				characterHoverStartTime = Time.unscaledTime;
				if (currentCharacterHoverId != -1 && currentCharacterHoverId != hitId)
				{
					currentCharacterHoverId = -1;
					HideCharacterPopup();
				}
			}
			else if (Time.unscaledTime - characterHoverStartTime >= Plugin.TooltipHoverDelay)
			{
				if (currentCharacterHoverId != hitId)
				{
					currentCharacterHoverId = hitId;
					ShowCharacterPopup(hitInfo);
				}
			}
		}
		else
		{
			pendingCharacterHoverId = -1;
			if (currentCharacterHoverId != -1)
			{
				currentCharacterHoverId = -1;
				HideCharacterPopup();
			}
		}
	}

	private static void PruneDeadCharacterIcons()
	{
		List<int> dead = null;
		foreach (var kv in characterIcons)
		{
			if ((Object)(object)kv.Value.Go == (Object)null)
			{
				dead ??= new List<int>();
				dead.Add(kv.Key);
			}
		}
		if (dead != null)
			foreach (int k in dead) characterIcons.Remove(k);
	}

	private static bool TryResolveCharacterIcon(GameObject sel, out int id, out MapIconInfo info)
	{
		id = ((Object)sel).GetInstanceID();
		if (characterIcons.TryGetValue(id, out info))
			return true;
		Transform p = sel.transform;
		while ((Object)(object)p != (Object)null)
		{
			id = ((Object)p.gameObject).GetInstanceID();
			if (characterIcons.TryGetValue(id, out info))
				return true;
			p = p.parent;
		}
		info = default;
		id = -1;
		return false;
	}

	private static void ShowCharacterPopup(MapIconInfo info)
	{
		HideCharacterPopup();
		Transform parent = null;
		try
		{
			GameObject page = GameObject.Find("UI/Canvas - App/Safe Area/View - CharacterSelection");
			if ((Object)(object)page == (Object)null)
				page = GameObject.Find("UI/Canvas - App");
			if ((Object)(object)page != (Object)null)
				parent = page.transform;
		}
		catch { }
		if ((Object)(object)parent == (Object)null && (Object)(object)info.Go != (Object)null)
		{
			var c = info.Go.GetComponentInParent<Canvas>();
			if ((Object)(object)c != (Object)null)
				parent = ((Component)c).transform;
		}
		if ((Object)(object)parent == (Object)null) return;

		// Rich tooltip with weapon / evo icons when possible
		CharacterSelectPatches.TooltipData data = null;
		try { CharacterSelectPatches.TryGetTooltipData(info.Go, out data); } catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Character] tooltip data: " + ex.Message);
		}

		if (data != null)
			characterPopup = CreateCharacterDetailPopup(parent, data);
		else
			characterPopup = CreateSimpleMapPopup(parent, info.Label, info.Description ?? "(no details)", info.Sprite);

		if ((Object)(object)characterPopup != (Object)null)
		{
			// Click-through so the card under the tooltip remains selectable
			DisablePopupRaycasts(characterPopup);
			// Original placement (anchored near the character), not "to the right of screen"
			if ((Object)(object)info.Go != (Object)null)
				PositionPopup(characterPopup, info.Go.transform);
		}
		Plugin.Dbg($"Character popup title={(data != null ? data.Title : info.Label)}");
	}

	/// <summary>Character select popup with portrait, starter weapon icon, and evo icons.
	/// Sizes itself from TMP preferredWidth/preferredHeight so multi-line flavor/stats fit.</summary>
	private static GameObject CreateCharacterDetailPopup(Transform parent, CharacterSelectPatches.TooltipData data)
	{
		if (data == null) return null;
		GameObject root = new GameObject("CharacterTooltipPopup");
		root.transform.SetParent(parent, false);
		RectTransform rootRt = root.AddComponent<RectTransform>();
		rootRt.anchorMin = new Vector2(0.5f, 0.5f);
		rootRt.anchorMax = new Vector2(0.5f, 0.5f);
		rootRt.pivot = new Vector2(0f, 1f);
		Image bg = root.AddComponent<Image>();
		((Graphic)bg).color = PopupBgColor;
		((Graphic)bg).raycastTarget = false;
		Outline ol = root.AddComponent<Outline>();
		((Shadow)ol).effectColor = new Color(0.9f, 0.75f, 0.3f, 1f);
		((Shadow)ol).effectDistance = new Vector2(2f, 2f);

		TMP_FontAsset font = GetFont();
		if ((Object)(object)font == (Object)null)
		{
			Object.Destroy((Object)(object)root);
			return null;
		}

		const float minWidth = 300f;
		const float maxWidth = 420f;
		const float icon = 36f;
		const float smallIcon = 28f;
		const float gap = 6f;

		// Width: expand for long titles / evo names, clamp
		float width = minWidth;
		try
		{
			float titleNeed = MeasureTmpPreferredWidth(font, data.Title ?? "Character", 17f, true) + 52f + Padding * 2f;
			width = Mathf.Clamp(Mathf.Max(minWidth, titleNeed), minWidth, maxWidth);
		}
		catch { }

		float contentW = width - Padding * 2f;
		float y = -Padding;
		var gold = new Color(0.95f, 0.8f, 0.35f, 1f);
		var soft = SoftWhite();

		// Title row
		float titleH = 44f;
		GameObject titleRow = new GameObject("Title");
		titleRow.transform.SetParent(root.transform, false);
		RectTransform tr = titleRow.AddComponent<RectTransform>();
		tr.anchorMin = new Vector2(0f, 1f);
		tr.anchorMax = new Vector2(0f, 1f);
		tr.pivot = new Vector2(0f, 1f);
		tr.anchoredPosition = new Vector2(Padding, y);

		float titleTextX = 0f;
		if ((Object)(object)data.Portrait != (Object)null)
		{
			AddUiIcon(titleRow.transform, data.Portrait, 0f, -titleH * 0.5f, titleH - 4f);
			titleTextX = titleH;
		}
		var titleTmp = AddUiText(titleRow.transform, "Name", data.Title ?? "Character", font, 17f, Color.white, true,
			titleTextX + 4f, 0f, contentW - titleTextX - 4f, titleH, (TextAlignmentOptions)513);
		// Grow title row if name wraps
		float titleTextH = FitTmpHeight(titleTmp, contentW - titleTextX - 4f, 20f, 72f);
		titleH = Mathf.Max(titleH, titleTextH + 4f);
		tr.sizeDelta = new Vector2(contentW, titleH);
		// Re-center portrait on new height
		try
		{
			if (titleRow.transform.childCount > 0)
			{
				var ir = titleRow.transform.GetChild(0).GetComponent<RectTransform>();
				if ((Object)(object)ir != (Object)null && ir.name == "Icon")
					ir.anchoredPosition = new Vector2(0f, -titleH * 0.5f);
			}
		}
		catch { }
		y -= titleH + gap;

		// Flavor
		if (!string.IsNullOrEmpty(data.Flavor))
		{
			var flavorTmp = AddUiText(root.transform, "Flavor", data.Flavor, font, 13f, soft, false,
				Padding, y, contentW, 40f, (TextAlignmentOptions)257);
			float fh = FitTmpHeight(flavorTmp, contentW, 18f, 200f);
			y -= fh + gap + 2f;
		}

		// Starting weapon
		AddUiText(root.transform, "WeaponHdr", "Starting weapon", font, 12f, gold, true,
			Padding, y, contentW, 18f, (TextAlignmentOptions)257);
		y -= 18f + 2f;

		if (data.Starter.HasValue && !string.IsNullOrEmpty(data.StarterName))
		{
			float rowH = icon + 4f;
			GameObject wrow = new GameObject("WeaponRow");
			wrow.transform.SetParent(root.transform, false);
			RectTransform wr = wrow.AddComponent<RectTransform>();
			wr.anchorMin = new Vector2(0f, 1f);
			wr.anchorMax = new Vector2(0f, 1f);
			wr.pivot = new Vector2(0f, 1f);
			wr.anchoredPosition = new Vector2(Padding, y);
			wr.sizeDelta = new Vector2(contentW, rowH);

			float tx = 0f;
			if ((Object)(object)data.StarterSprite != (Object)null)
			{
				AddUiIcon(wrow.transform, data.StarterSprite, 0f, -rowH * 0.5f, icon);
				tx = icon + 6f;
			}
			var wName = AddUiText(wrow.transform, "WName", data.StarterName, font, 14f, Color.white, false,
				tx, 0f, contentW - tx, rowH, (TextAlignmentOptions)513);
			float nameH = FitTmpHeight(wName, contentW - tx, 18f, 48f);
			rowH = Mathf.Max(rowH, nameH + 4f);
			wr.sizeDelta = new Vector2(contentW, rowH);
			y -= rowH + gap;
		}
		else
		{
			AddUiText(root.transform, "WUnknown", "(unknown)", font, 13f, new Color(0.7f, 0.7f, 0.75f, 1f), false,
				Padding, y, contentW, 18f, (TextAlignmentOptions)257);
			y -= 20f;
		}

		// Evolutions
		if (data.Evos != null && data.Evos.Count > 0)
		{
			AddUiText(root.transform, "EvoHdr", "Evolution", font, 12f, gold, true,
				Padding, y, contentW, 18f, (TextAlignmentOptions)257);
			y -= 18f + 2f;

			foreach (var row in data.Evos)
			{
				if (row == null) continue;
				float rowH = smallIcon + 8f;
				GameObject erow = new GameObject("EvoRow");
				erow.transform.SetParent(root.transform, false);
				RectTransform er = erow.AddComponent<RectTransform>();
				er.anchorMin = new Vector2(0f, 1f);
				er.anchorMax = new Vector2(0f, 1f);
				er.pivot = new Vector2(0f, 1f);
				er.anchoredPosition = new Vector2(Padding, y);

				float x = 0f;
				Sprite baseSpr = data.StarterSprite;
				if ((Object)(object)baseSpr != (Object)null)
				{
					AddUiIcon(erow.transform, baseSpr, x, -rowH * 0.5f, smallIcon);
					x += smallIcon + 3f;
				}
				if (row.Passives != null)
				{
					foreach (var p in row.Passives)
					{
						if (p == null) continue;
						AddUiText(erow.transform, "Plus", "+", font, 14f, new Color(0.9f, 0.9f, 0.5f, 1f), true,
							x, 0f, 12f, rowH, (TextAlignmentOptions)514);
						x += 12f;
						Sprite ps = p.Sprite ?? GameData.GetSprite(p.Type);
						if ((Object)(object)ps != (Object)null)
						{
							AddUiIcon(erow.transform, ps, x, -rowH * 0.5f, smallIcon);
							x += smallIcon + 3f;
						}
						else
						{
							string pn = string.IsNullOrEmpty(p.Name) ? p.Type.ToString() : p.Name;
							var pnTmp = AddUiText(erow.transform, "PName", pn, font, 11f, soft, false,
								x, 0f, 80f, rowH, (TextAlignmentOptions)513);
							float pw = Mathf.Min(100f, MeasureTmpPreferredWidth(font, pn, 11f, false) + 4f);
							pnTmp.rectTransform.sizeDelta = new Vector2(pw, rowH);
							x += pw + 2f;
						}
					}
				}
				AddUiText(erow.transform, "Arrow", "→", font, 14f, gold, true,
					x, 0f, 18f, rowH, (TextAlignmentOptions)514);
				x += 18f;
				Sprite es = row.EvolvedSprite ?? GameData.GetSprite(row.Evolved);
				if ((Object)(object)es != (Object)null)
				{
					AddUiIcon(erow.transform, es, x, -rowH * 0.5f, smallIcon);
					x += smallIcon + 4f;
				}
				string en = string.IsNullOrEmpty(row.EvolvedName) ? row.Evolved.ToString() : row.EvolvedName;
				float nameW = Mathf.Max(40f, contentW - x);
				var evoName = AddUiText(erow.transform, "EvoName", en, font, 12f, Color.white, false,
					x, 0f, nameW, rowH, (TextAlignmentOptions)513);
				float evoNameH = FitTmpHeight(evoName, nameW, 16f, 48f);
				rowH = Mathf.Max(rowH, evoNameH + 4f);
				// Grow width if evo row needs more room for icons + name
				float rowNeed = x + MeasureTmpPreferredWidth(font, en, 12f, false) + Padding * 2f + 8f;
				if (rowNeed > width && rowNeed <= maxWidth)
				{
					width = rowNeed;
					contentW = width - Padding * 2f;
				}
				er.sizeDelta = new Vector2(contentW, rowH);
				// Re-center icons on final row height
				try
				{
					for (int ci = 0; ci < erow.transform.childCount; ci++)
					{
						var ch = erow.transform.GetChild(ci);
						if (ch.name != "Icon") continue;
						var ir = ch.GetComponent<RectTransform>();
						if ((Object)(object)ir != (Object)null)
							ir.anchoredPosition = new Vector2(ir.anchoredPosition.x, -rowH * 0.5f);
					}
				}
				catch { }
				y -= rowH + 4f;
			}
			y -= 2f;
		}

		if (!string.IsNullOrEmpty(data.OutfitsText))
		{
			AddUiText(root.transform, "OutfitHdr", "Other outfits", font, 12f, new Color(0.7f, 0.85f, 1f, 1f), true,
				Padding, y, contentW, 18f, (TextAlignmentOptions)257);
			y -= 18f + 2f;
			var ot = AddUiText(root.transform, "Outfits", data.OutfitsText, font, 12f, soft, false,
				Padding, y, contentW, 40f, (TextAlignmentOptions)257);
			float oh = FitTmpHeight(ot, contentW, 18f, 160f);
			y -= oh + gap;
		}

		if (!string.IsNullOrEmpty(data.StatsText))
		{
			AddUiText(root.transform, "StatsHdr", "Notable stats", font, 12f, gold, true,
				Padding, y, contentW, 18f, (TextAlignmentOptions)257);
			y -= 18f + 2f;
			var st = AddUiText(root.transform, "Stats", data.StatsText, font, 12f, soft, false,
				Padding, y, contentW, 40f, (TextAlignmentOptions)257);
			float sh = FitTmpHeight(st, contentW, 18f, 160f);
			y -= sh + gap;
		}

		y -= Padding;
		float height = Mathf.Abs(y);
		// Final pass: ensure all top-level rows use current content width
		try
		{
			for (int i = 0; i < root.transform.childCount; i++)
			{
				var rt = root.transform.GetChild(i).GetComponent<RectTransform>();
				if ((Object)(object)rt == (Object)null) continue;
				if (Mathf.Approximately(rt.anchorMin.x, 0f) && Mathf.Approximately(rt.anchorMax.x, 0f))
				{
					// leave x/height; widen to contentW if it was a full-width row
					if (rt.sizeDelta.x >= minWidth - Padding * 2f - 1f)
						rt.sizeDelta = new Vector2(contentW, rt.sizeDelta.y);
				}
			}
		}
		catch { }

		rootRt.sizeDelta = new Vector2(width, height);
		// Force canvas update so PositionPopup sees real size
		try
		{
			Canvas.ForceUpdateCanvases();
			LayoutRebuilder.ForceRebuildLayoutImmediate(rootRt);
		}
		catch { }
		return root;
	}

	private static Color SoftWhite() => new Color(0.85f, 0.85f, 0.9f, 1f);

	private static void AddUiIcon(Transform parent, Sprite sprite, float x, float yCenter, float size)
	{
		GameObject ic = new GameObject("Icon");
		ic.transform.SetParent(parent, false);
		RectTransform ir = ic.AddComponent<RectTransform>();
		ir.anchorMin = new Vector2(0f, 1f);
		ir.anchorMax = new Vector2(0f, 1f);
		ir.pivot = new Vector2(0f, 0.5f);
		ir.anchoredPosition = new Vector2(x, yCenter);
		ir.sizeDelta = new Vector2(size, size);
		Image img = ic.AddComponent<Image>();
		img.sprite = sprite;
		img.preserveAspect = true;
		((Graphic)img).raycastTarget = false;
	}

	private static TextMeshProUGUI AddUiText(Transform parent, string name, string text, TMP_FontAsset font,
		float size, Color color, bool bold, float x, float y, float w, float h, TextAlignmentOptions align)
	{
		GameObject go = new GameObject(name);
		go.transform.SetParent(parent, false);
		RectTransform rt = go.AddComponent<RectTransform>();
		rt.anchorMin = new Vector2(0f, 1f);
		rt.anchorMax = new Vector2(0f, 1f);
		rt.pivot = new Vector2(0f, 1f);
		rt.anchoredPosition = new Vector2(x, y);
		rt.sizeDelta = new Vector2(w, h);
		TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
		((TMP_Text)tmp).font = font;
		((TMP_Text)tmp).text = text ?? "";
		((TMP_Text)tmp).fontSize = size;
		((TMP_Text)tmp).fontStyle = bold ? (FontStyles)1 : (FontStyles)0;
		((Graphic)tmp).color = color;
		((TMP_Text)tmp).alignment = align;
		((TMP_Text)tmp).enableWordWrapping = true;
		((TMP_Text)tmp).overflowMode = TextOverflowModes.Overflow;
		((TMP_Text)tmp).richText = false;
		((Graphic)tmp).raycastTarget = false;
		return tmp;
	}

	/// <summary>Set TMP box width, force mesh, return preferred height (clamped).</summary>
	private static float FitTmpHeight(TextMeshProUGUI tmp, float width, float minH, float maxH)
	{
		if ((Object)(object)tmp == (Object)null) return minH;
		try
		{
			var rt = tmp.rectTransform;
			rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
			((TMP_Text)tmp).enableWordWrapping = true;
			((TMP_Text)tmp).overflowMode = TextOverflowModes.Overflow;
			((TMP_Text)tmp).ForceMeshUpdate(true, true);
			float h = ((TMP_Text)tmp).preferredHeight;
			if (float.IsNaN(h) || h < 1f)
				h = minH;
			h = Mathf.Clamp(h + 2f, minH, maxH);
			rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
			return h;
		}
		catch
		{
			return minH;
		}
	}

	private static float MeasureTmpPreferredWidth(TMP_FontAsset font, string text, float fontSize, bool bold)
	{
		if (string.IsNullOrEmpty(text) || (Object)(object)font == (Object)null) return 40f;
		try
		{
			// Temporary off-screen measure object
			GameObject go = new GameObject("TmpMeasure");
			var tmp = go.AddComponent<TextMeshProUGUI>();
			((TMP_Text)tmp).font = font;
			((TMP_Text)tmp).text = text;
			((TMP_Text)tmp).fontSize = fontSize;
			((TMP_Text)tmp).fontStyle = bold ? (FontStyles)1 : (FontStyles)0;
			((TMP_Text)tmp).enableWordWrapping = false;
			((TMP_Text)tmp).ForceMeshUpdate(true, true);
			float w = ((TMP_Text)tmp).preferredWidth;
			Object.Destroy((Object)(object)go);
			if (float.IsNaN(w) || w < 1f) return Mathf.Max(40f, text.Length * fontSize * 0.55f);
			return w;
		}
		catch
		{
			return Mathf.Max(40f, (text?.Length ?? 0) * fontSize * 0.55f);
		}
	}

	private static void DisablePopupRaycasts(GameObject popup)
	{
		if ((Object)(object)popup == (Object)null) return;
		try
		{
			foreach (var g in popup.GetComponentsInChildren<Graphic>(true))
			{
				if ((Object)(object)g != (Object)null)
					((Graphic)g).raycastTarget = false;
			}
		}
		catch { }
	}

	private static void HideCharacterPopup()
	{
		if ((Object)(object)characterPopup != (Object)null)
		{
			Object.Destroy((Object)(object)characterPopup);
			characterPopup = null;
		}
	}

	// ── Pause map icons ──────────────────────────────────────────────────

	public static void RegisterMapIcon(GameObject go, ItemType? item, WeaponType? weapon, string label, string description, Sprite sprite)
	{
		if (!Plugin.MapTooltipsEnabled) return;
		if ((Object)(object)go == (Object)null) return;
		int id = ((Object)go).GetInstanceID();
		mapIcons[id] = new MapIconInfo
		{
			Go = go,
			Item = item,
			Weapon = weapon,
			Label = label ?? "Unknown",
			Description = description ?? "",
			Sprite = sprite
		};
	}

	public static void ClearMapIcons()
	{
		mapIcons.Clear();
		currentMapHoverId = -1;
		pendingMapHoverId = -1;
		HideMapPopup();
	}

	/// <summary>Update map icon types using frame→ItemType map after SetPickups.</summary>
	public static void EnrichMapIconsFromItemTypes(Dictionary<string, ItemType> byFrame)
	{
		if (byFrame == null || byFrame.Count == 0 || mapIcons.Count == 0) return;
		var keys = new List<int>(mapIcons.Keys);
		foreach (int id in keys)
		{
			var info = mapIcons[id];
			if ((Object)(object)info.Go == (Object)null) continue;
			string spriteName = null;
			try
			{
				var img = info.Go.GetComponent<Image>();
				if ((Object)(object)img == (Object)null)
					img = info.Go.GetComponentInChildren<Image>(true);
				if ((Object)(object)img != (Object)null && (Object)(object)img.sprite != (Object)null)
					spriteName = ((Object)img.sprite).name;
			}
			catch { }
			if (string.IsNullOrEmpty(spriteName) && (Object)(object)info.Sprite != (Object)null)
				spriteName = ((Object)info.Sprite).name;
			if (string.IsNullOrEmpty(spriteName)) continue;

			ItemType it;
			if (!byFrame.TryGetValue(spriteName, out it))
			{
				string bare = spriteName;
				if (bare.Contains(".")) bare = System.IO.Path.GetFileNameWithoutExtension(bare);
				if (!byFrame.TryGetValue(bare, out it))
					continue;
			}

			try
			{
				info.Item = it;
				info.Weapon = null;
				info.Label = GameData.GetItemName(it);
				info.Description = GameData.GetItemDescription(it);
				var spr = GameData.GetItemSprite(it);
				if ((Object)(object)spr != (Object)null) info.Sprite = spr;
				mapIcons[id] = info;
			}
			catch { }
		}
	}

	private static void UpdateMapHover()
	{
		// Drop destroyed icons
		List<int> dead = null;
		foreach (var kv in mapIcons)
		{
			if ((Object)(object)kv.Value.Go == (Object)null)
			{
				dead ??= new List<int>();
				dead.Add(kv.Key);
			}
		}
		if (dead != null)
		{
			foreach (int k in dead) mapIcons.Remove(k);
		}

		Vector2 mouse = (Vector2)Input.mousePosition;
		int hitId = -1;
		MapIconInfo hitInfo = default;
		float bestArea = float.MaxValue;

		foreach (var kv in mapIcons)
		{
			GameObject go = kv.Value.Go;
			if ((Object)(object)go == (Object)null || !go.activeInHierarchy) continue;
			RectTransform rt = go.GetComponent<RectTransform>();
			if ((Object)(object)rt == (Object)null) continue;

			Camera cam = null;
			var canvas = go.GetComponentInParent<Canvas>();
			if ((Object)(object)canvas != (Object)null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
				cam = canvas.worldCamera != null ? canvas.worldCamera : Camera.main;

			bool hit = RectTransformUtility.RectangleContainsScreenPoint(rt, mouse, cam)
				|| RectTransformUtility.RectangleContainsScreenPoint(rt, mouse, null)
				|| RectTransformUtility.RectangleContainsScreenPoint(rt, mouse, Camera.main);

			// Map icons are tiny — also accept within a screen-space radius of the icon center
			if (!hit)
			{
				Vector3[] corners = new Vector3[4];
				rt.GetWorldCorners(corners);
				Vector2 min = RectTransformUtility.WorldToScreenPoint(cam, corners[0]);
				Vector2 max = RectTransformUtility.WorldToScreenPoint(cam, corners[2]);
				// if world camera null for overlay, use null cam
				if (cam == null)
				{
					min = RectTransformUtility.WorldToScreenPoint(null, corners[0]);
					max = RectTransformUtility.WorldToScreenPoint(null, corners[2]);
				}
				Vector2 center = (min + max) * 0.5f;
				float half = Mathf.Max(18f, Mathf.Max(Mathf.Abs(max.x - min.x), Mathf.Abs(max.y - min.y)) * 0.5f + 8f);
				hit = (mouse - center).sqrMagnitude <= half * half;
			}
			if (!hit) continue;

			float w = Mathf.Abs(rt.rect.width);
			float h = Mathf.Abs(rt.rect.height);
			float area = Mathf.Max(1f, w * h);
			if (area < bestArea)
			{
				bestArea = area;
				hitId = kv.Key;
				hitInfo = kv.Value;
			}
		}

		if (hitId != -1 && hitId != currentMapHoverId)
		{
			if (hitId != pendingMapHoverId)
			{
				pendingMapHoverId = hitId;
				mapHoverStartTime = Time.unscaledTime;
				return;
			}
			if (Time.unscaledTime - mapHoverStartTime >= CollectionHoverDelay * 0.45f) // slightly faster than collections
			{
				currentMapHoverId = hitId;
				pendingMapHoverId = -1;
				ShowMapPopup(hitInfo);
			}
		}
		else if (hitId == currentMapHoverId)
		{
			pendingMapHoverId = -1;
		}
		else
		{
			// left icon
			pendingMapHoverId = -1;
			if (hitId == -1 && currentMapHoverId != -1)
			{
				// keep popup until mouse leaves popup area — simple: hide when no icon
				// Don't immediately hide if over the popup itself
				if ((Object)(object)mapPopup != (Object)null)
				{
					RectTransform prt = mapPopup.GetComponent<RectTransform>();
					bool overPopup = (Object)(object)prt != (Object)null
						&& (RectTransformUtility.RectangleContainsScreenPoint(prt, mouse, null)
							|| RectTransformUtility.RectangleContainsScreenPoint(prt, mouse, Camera.main));
					if (!overPopup)
					{
						currentMapHoverId = -1;
						HideMapPopup();
					}
				}
				else
				{
					currentMapHoverId = -1;
				}
			}
		}
	}

	private static void ShowMapPopup(MapIconInfo info)
	{
		HideMapPopup();
		Transform parent = FindMapPopupParent(info.Go != null ? info.Go.transform : null);
		if ((Object)(object)parent == (Object)null) return;

		// Weapons: full evolution/arcana popup
		if (info.Weapon.HasValue && !info.Item.HasValue)
		{
			mapPopup = CreatePopup(parent, info.Weapon, null);
			if ((Object)(object)mapPopup != (Object)null)
			{
				PositionPopup(mapPopup, info.Go != null ? info.Go.transform : parent);
				Plugin.Dbg($"Map popup weapon={info.Weapon}");
			}
			return;
		}

		// Items / relics / pickups / unresolved: compact map tooltip (uses ItemData when available)
		string label = info.Label;
		string desc = info.Description;
		Sprite spr = info.Sprite;
		if (info.Item.HasValue)
		{
			if (string.IsNullOrEmpty(label) || label == "Unknown")
				label = GameData.GetItemName(info.Item.Value);
			if (string.IsNullOrEmpty(desc))
				desc = GameData.GetItemDescription(info.Item.Value);
			spr = GameData.GetItemSprite(info.Item.Value) ?? spr;
		}
		// Tag relics lightly
		if (info.Item.HasValue)
		{
			string raw = info.Item.Value.ToString();
			if (raw.StartsWith("RELIC", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(desc))
			{
				// keep desc
			}
			else if (raw.StartsWith("RELIC", StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(desc))
			{
				desc = "Relic";
			}
		}

		mapPopup = CreateSimpleMapPopup(parent, label, desc, spr);
		if ((Object)(object)mapPopup != (Object)null && (Object)(object)info.Go != (Object)null)
			PositionPopup(mapPopup, info.Go.transform);
		Plugin.Dbg($"Map popup item={info.Item} label={label}");
	}

	private static void HideMapPopup()
	{
		if ((Object)(object)mapPopup != (Object)null)
		{
			Object.Destroy((Object)(object)mapPopup);
			mapPopup = null;
		}
	}

	private static Transform FindMapPopupParent(Transform anchor)
	{
		// Prefer the map's canvas / GAME UI
		try
		{
			var map = Object.FindObjectOfType<MapManager>();
			if ((Object)(object)map != (Object)null)
			{
				var canvas = ((Component)map).GetComponentInParent<Canvas>();
				if ((Object)(object)canvas != (Object)null)
					return ((Component)canvas).transform;
			}
		}
		catch { }
		if ((Object)(object)anchor != (Object)null)
		{
			var c = anchor.GetComponentInParent<Canvas>();
			if ((Object)(object)c != (Object)null)
				return ((Component)c).transform;
		}
		GameObject go = GameObject.Find("GAME UI/Canvas - Game UI");
		if ((Object)(object)go != (Object)null) return go.transform;
		return FindPopupParent(anchor);
	}

	/// <summary>
	/// Simple icon + title + description popup (stage relics, map icons, adventure fallback).
	/// Width grows for long titles; title/desc heights come from TMP preferred size so wrapping
	/// does not collide (fixes cramped "Relics in stage" tooltips).
	/// </summary>
	private static GameObject CreateSimpleMapPopup(Transform parent, string title, string description, Sprite sprite)
	{
		GameObject val = new GameObject("MapTooltipPopup");
		val.transform.SetParent(parent, false);
		RectTransform val2 = val.AddComponent<RectTransform>();
		val2.anchorMin = new Vector2(0.5f, 0.5f);
		val2.anchorMax = new Vector2(0.5f, 0.5f);
		val2.pivot = new Vector2(0f, 1f);
		Image bg = val.AddComponent<Image>();
		((Graphic)bg).color = PopupBgColor;
		((Graphic)bg).raycastTarget = false;
		Outline outline = val.AddComponent<Outline>();
		((Shadow)outline).effectColor = new Color(0.9f, 0.75f, 0.3f, 1f);
		((Shadow)outline).effectDistance = new Vector2(2f, 2f);

		TMP_FontAsset font = GetFont();
		if ((Object)(object)font == (Object)null)
		{
			Object.Destroy((Object)(object)val);
			return null;
		}

		const float minW = 280f;
		const float maxW = 400f;
		const float icon = 40f; // slightly smaller than IconSize so title can breathe
		string titleText = string.IsNullOrEmpty(title) ? "Unknown" : title;

		// Prefer a width that fits the title on ~2 lines max, not a fixed 320 that wraps too tight
		float width = minW;
		try
		{
			float oneLine = MeasureTmpPreferredWidth(font, titleText, 17f, true) + icon + Spacing + Padding * 2f + 8f;
			// If one line is reasonable, use it; else cap and let wrap
			if (oneLine <= maxW)
				width = Mathf.Max(minW, oneLine);
			else
				width = maxW;
		}
		catch { width = 340f; }

		float contentW = width - Padding * 2f;
		float y = -Padding;
		float titleTextW = contentW - icon - Spacing;

		// Title row: icon + wrapping title (height from TMP, not fixed IconSize)
		GameObject titleRow = new GameObject("TitleRow");
		titleRow.transform.SetParent(val.transform, false);
		RectTransform tr = titleRow.AddComponent<RectTransform>();
		tr.anchorMin = new Vector2(0f, 1f);
		tr.anchorMax = new Vector2(0f, 1f);
		tr.pivot = new Vector2(0f, 1f);
		tr.anchoredPosition = new Vector2(Padding, y);

		if ((Object)(object)sprite != (Object)null)
		{
			GameObject ic = new GameObject("Icon");
			ic.transform.SetParent(titleRow.transform, false);
			RectTransform ir = ic.AddComponent<RectTransform>();
			ir.anchorMin = new Vector2(0f, 1f);
			ir.anchorMax = new Vector2(0f, 1f);
			ir.pivot = new Vector2(0f, 1f);
			ir.anchoredPosition = new Vector2(0f, 0f);
			ir.sizeDelta = new Vector2(icon, icon);
			Image ii = ic.AddComponent<Image>();
			ii.sprite = sprite;
			ii.preserveAspect = true;
			((Graphic)ii).raycastTarget = false;
		}

		// Title as top-left box next to icon (not stretch-fill) so multi-line works
		GameObject titleGo = new GameObject("Title");
		titleGo.transform.SetParent(titleRow.transform, false);
		RectTransform titleRt = titleGo.AddComponent<RectTransform>();
		titleRt.anchorMin = new Vector2(0f, 1f);
		titleRt.anchorMax = new Vector2(0f, 1f);
		titleRt.pivot = new Vector2(0f, 1f);
		titleRt.anchoredPosition = new Vector2(icon + Spacing, 0f);
		titleRt.sizeDelta = new Vector2(titleTextW, icon);
		TextMeshProUGUI titleTmp = titleGo.AddComponent<TextMeshProUGUI>();
		((TMP_Text)titleTmp).font = font;
		((TMP_Text)titleTmp).text = titleText;
		((TMP_Text)titleTmp).fontSize = 17f;
		((TMP_Text)titleTmp).fontStyle = (FontStyles)1;
		((Graphic)titleTmp).color = Color.white;
		((TMP_Text)titleTmp).alignment = (TextAlignmentOptions)257; // top-left
		((TMP_Text)titleTmp).enableWordWrapping = true;
		((TMP_Text)titleTmp).overflowMode = TextOverflowModes.Overflow;
		((Graphic)titleTmp).raycastTarget = false;

		float titleH = FitTmpHeight(titleTmp, titleTextW, 22f, 96f);
		float rowH = Mathf.Max(icon, titleH);
		// Vertically center icon if title taller than icon
		if ((Object)(object)sprite != (Object)null && rowH > icon)
		{
			try
			{
				var ir = titleRow.transform.Find("Icon") as RectTransform;
				if ((Object)(object)ir != (Object)null)
					ir.anchoredPosition = new Vector2(0f, -(rowH - icon) * 0.5f);
			}
			catch { }
		}
		tr.sizeDelta = new Vector2(contentW, rowH);
		y -= rowH + Spacing + 2f; // extra gap before description

		if (!string.IsNullOrEmpty(description))
		{
			GameObject descGo = new GameObject("Desc");
			descGo.transform.SetParent(val.transform, false);
			RectTransform dr = descGo.AddComponent<RectTransform>();
			dr.anchorMin = new Vector2(0f, 1f);
			dr.anchorMax = new Vector2(0f, 1f);
			dr.pivot = new Vector2(0f, 1f);
			dr.anchoredPosition = new Vector2(Padding, y);
			dr.sizeDelta = new Vector2(contentW, 24f);
			TextMeshProUGUI dt = descGo.AddComponent<TextMeshProUGUI>();
			((TMP_Text)dt).font = font;
			((TMP_Text)dt).text = description;
			((TMP_Text)dt).fontSize = 13f;
			((TMP_Text)dt).fontStyle = (FontStyles)0;
			((Graphic)dt).color = new Color(0.85f, 0.85f, 0.9f, 1f);
			((TMP_Text)dt).alignment = (TextAlignmentOptions)257; // top-left
			((TMP_Text)dt).enableWordWrapping = true;
			((TMP_Text)dt).overflowMode = TextOverflowModes.Overflow;
			((Graphic)dt).raycastTarget = false;
			float h = FitTmpHeight(dt, contentW, 20f, 180f);
			y -= h + Spacing;
		}

		y -= Padding;
		val2.sizeDelta = new Vector2(width, Mathf.Abs(y));
		try
		{
			Canvas.ForceUpdateCanvases();
			LayoutRebuilder.ForceRebuildLayoutImmediate(val2);
		}
		catch { }
		return val;
	}

	public static Type GetCachedArcanaTypeEnum()
	{
		return cachedArcanaTypeEnum;
	}

	private static GameObject FindIconInUI(GameObject parent)
	{
		string[] array = new string[6] { "Icon", "icon", "ItemIcon", "WeaponIcon", "Sprite", "Image" };
		string[] array2 = array;
		string[] array3 = array2;
		foreach (string text in array3)
		{
			Transform val = parent.transform.Find(text);
			if ((Object)(object)val != (Object)null)
			{
				return ((Component)val).gameObject;
			}
		}
		Il2CppArrayBase<Image> componentsInChildren = parent.GetComponentsInChildren<Image>(false);
		foreach (Image item in componentsInChildren)
		{
			string text2 = ((Object)((Component)item).gameObject).name.ToLower();
			if (text2.Contains("bg") || text2.Contains("frame") || text2.Contains("background") || (Object)(object)((Component)item).gameObject == (Object)(object)parent)
			{
				continue;
			}
			if (componentsInChildren.Length > 0)
			{
			}
			return ((Component)item).gameObject;
		}
		return null;
	}

	private static void AddHoverToGameObject(GameObject go, WeaponType? weaponType, ItemType? itemType, bool useClick = false)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Expected O, but got Unknown
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Expected O, but got Unknown
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		if ((weaponType.HasValue && ((object)weaponType.Value/*cast due to constrained. prefix*/).ToString() == "DEFANG") || (itemType.HasValue && ((object)itemType.Value/*cast due to constrained. prefix*/).ToString() == "DEFANG"))
		{
			return;
		}
		EventTrigger component = go.GetComponent<EventTrigger>();
		if ((Object)(object)component != (Object)null)
		{
			Object.Destroy((Object)(object)component);
		}
		EventTrigger val = go.AddComponent<EventTrigger>();
		if (useClick)
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
		}
		else
		{
			EventTrigger.Entry val3 = new EventTrigger.Entry();
			val3.eventID = (EventTriggerType)0; // PointerEnter
			((UnityEvent<BaseEventData>)(object)val3.callback).AddListener((UnityEngine.Events.UnityAction<UnityEngine.EventSystems.BaseEventData>)(System.Action<UnityEngine.EventSystems.BaseEventData>)(delegate
			{
				if (IsLevelUpViewActive())
					RequestLevelUpHover(go.transform, weaponType, itemType);
				else
					ShowItemPopup(go.transform, weaponType, itemType);
			}));
			val.triggers.Add(val3);
			EventTrigger.Entry val4 = new EventTrigger.Entry();
			val4.eventID = (EventTriggerType)1; // PointerExit
			((UnityEvent<BaseEventData>)(object)val4.callback).AddListener((UnityEngine.Events.UnityAction<UnityEngine.EventSystems.BaseEventData>)(System.Action<UnityEngine.EventSystems.BaseEventData>)(delegate
			{
				CancelLevelUpHoverIfMatch(go.transform);
				DelayFrames(10, () => { if (mouseOverPopupIndex < 0 && popupStack.Count > 0) HideAllPopups(); });
			}));
			val.triggers.Add(val4);
		}
	}

	private static WeaponData GetWeaponData(WeaponType type)
	{
		return GameData.GetWeaponData(type);
	}

	private static object GetPowerUpData(ItemType type)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		if (cachedPowerUpsDict == null)
		{
			return null;
		}
		try
		{
			Type type2 = cachedPowerUpsDict.GetType();
			MethodInfo method = type2.GetMethod("ContainsKey");
			if (method != null && (bool)method.Invoke(cachedPowerUpsDict, new object[1] { type }))
			{
				PropertyInfo property = type2.GetProperty("Item");
				if (property != null)
				{
					object value = property.GetValue(cachedPowerUpsDict, new object[1] { type });
					if (value != null)
					{
						PropertyInfo property2 = value.GetType().GetProperty("Count");
						if (property2 != null && (int)property2.GetValue(value) > 0)
						{
							PropertyInfo property3 = value.GetType().GetProperty("Item");
							if (property3 != null)
							{
								return property3.GetValue(value, new object[1] { 0 });
							}
						}
					}
				}
			}
		}
		catch
		{
		}
		return null;
	}

	private static Sprite GetCircleSprite()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)cachedCircleSprite != (Object)null)
		{
			return cachedCircleSprite;
		}
		try
		{
			int num = 64;
			Texture2D val = new Texture2D(num, num, (TextureFormat)4, false);
			((Texture)val).filterMode = (FilterMode)1;
			float num2 = (float)num / 2f;
			float num3 = num2 - 1f;
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num; j++)
				{
					float num4 = (float)j - num2;
					float num5 = (float)i - num2;
					float num6 = Mathf.Sqrt(num4 * num4 + num5 * num5);
					if (num6 <= num3)
					{
						float num7 = 1f;
						if (num6 > num3 - 2f)
						{
							num7 = (num3 - num6) / 2f;
						}
						val.SetPixel(j, i, new Color(1f, 1f, 1f, num7));
					}
					else
					{
						val.SetPixel(j, i, new Color(0f, 0f, 0f, 0f));
					}
				}
			}
			val.Apply();
			cachedCircleSprite = Sprite.Create(val, new Rect(0f, 0f, (float)num, (float)num), new Vector2(0.5f, 0.5f), 100f);
			return cachedCircleSprite;
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("Error creating circle sprite: " + ex.Message);
			return null;
		}
	}

	private static Sprite LoadSpriteFromAtlas(string frameName, string atlasName)
	{
		return GameData.LoadSprite(frameName, atlasName);
	}

	private static Sprite GetSpriteForWeapon(WeaponType weaponType)
	{
		return GameData.GetSprite(weaponType);
	}

	private static Sprite GetSpriteForItem(ItemType itemType)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		object powerUpData = GetPowerUpData(itemType);
		if (powerUpData == null)
		{
			return null;
		}
		try
		{
			string propertyValue = GetPropertyValue<string>(powerUpData, "frameName");
			string propertyValue2 = GetPropertyValue<string>(powerUpData, "texture");
			if (!string.IsNullOrEmpty(propertyValue) && !string.IsNullOrEmpty(propertyValue2))
			{
				return LoadSpriteFromAtlas(propertyValue, propertyValue2);
			}
			if (!string.IsNullOrEmpty(propertyValue))
			{
				string[] array = new string[4] { "items", "powerups", "weapons", "ui" };
				string[] array2 = array;
				string[] array3 = array2;
				foreach (string atlasName in array3)
				{
					Sprite val = LoadSpriteFromAtlas(propertyValue, atlasName);
					if ((Object)(object)val != (Object)null)
					{
						return val;
					}
				}
			}
		}
		catch
		{
		}
		return null;
	}

	private static T GetPropertyValue<T>(object obj, string propertyName)
	{
		if (obj == null)
		{
			return default(T);
		}
		try
		{
			PropertyInfo property = obj.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
			if (property != null)
			{
				return (T)property.GetValue(obj);
			}
			FieldInfo field = obj.GetType().GetField(propertyName, BindingFlags.Instance | BindingFlags.Public);
			if (field != null)
			{
				return (T)field.GetValue(obj);
			}
		}
		catch
		{
		}
		return default(T);
	}

	private static string GetLocalizedWeaponDescription(WeaponData data, WeaponType type)
	{
		return GameData.GetWeaponDescription(type);
	}

	private static string GetLocalizedWeaponName(WeaponData data, WeaponType type)
	{
		return GameData.GetWeaponName(type);
	}

	private static string GetLocalizedPowerUpDescription(object data, ItemType type)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected I4, but got Unknown
		if (data == null)
		{
			return "";
		}
		try
		{
			MethodInfo method = data.GetType().GetMethod("GetLocalizedDescription", BindingFlags.Instance | BindingFlags.Public);
			if (method != null)
			{
				Type parameterType = method.GetParameters()[0].ParameterType;
				object obj = Enum.ToObject(parameterType, (int)type);
				string text = method.Invoke(data, new object[1] { obj }) as string;
				if (!string.IsNullOrEmpty(text) && !text.Contains("/"))
				{
					return text;
				}
			}
		}
		catch
		{
		}
		string propertyValue = GetPropertyValue<string>(data, "description");
		if (!string.IsNullOrEmpty(propertyValue) && !propertyValue.Contains("/"))
		{
			return propertyValue;
		}
		return "";
	}

	private unsafe static string GetLocalizedPowerUpName(object data, ItemType type)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected I4, but got Unknown
		if (data == null)
		{
			return ((object)(*(ItemType*)(&type))/*cast due to constrained. prefix*/).ToString();
		}
		try
		{
			MethodInfo method = data.GetType().GetMethod("GetLocalizedName", BindingFlags.Instance | BindingFlags.Public);
			if (method != null)
			{
				Type parameterType = method.GetParameters()[0].ParameterType;
				object obj = Enum.ToObject(parameterType, (int)type);
				string text = method.Invoke(data, new object[1] { obj }) as string;
				if (!string.IsNullOrEmpty(text) && !text.Contains("/"))
				{
					return text;
				}
			}
		}
		catch
		{
		}
		string propertyValue = GetPropertyValue<string>(data, "name");
		if (!string.IsNullOrEmpty(propertyValue) && !propertyValue.Contains("/"))
		{
			return propertyValue;
		}
		return ((object)(*(ItemType*)(&type))/*cast due to constrained. prefix*/).ToString();
	}

	private static string GetI2Translation(string term)
	{
		return GameData.Translate(term);
	}

	private static TMP_FontAsset GetFont()
	{
		TextMeshProUGUI obj = Object.FindObjectOfType<TextMeshProUGUI>();
		return (obj != null) ? ((TMP_Text)obj).font : null;
	}

	/// <summary>Shared UI font for Stage Guide / other menus.</summary>
	public static TMP_FontAsset GetUiFont() => GetFont();

	private static object GetGameManager()
	{
		if (cachedGameManager != null)
		{
			return cachedGameManager;
		}
		try
		{
			Assembly assembly = typeof(WeaponData).Assembly;
			Type type = assembly.GetTypes().FirstOrDefault((Type t) => t.Name == "GameManager" && !t.IsInterface && typeof(Component).IsAssignableFrom(t));
			if (type == null)
			{
				return null;
			}
			MethodInfo methodInfo = typeof(Object).GetMethods().FirstOrDefault((MethodInfo m) => m.Name == "FindObjectOfType" && m.IsGenericMethod && m.GetParameters().Length == 0);
			if (methodInfo == null)
			{
				return null;
			}
			MethodInfo methodInfo2 = methodInfo.MakeGenericMethod(type);
			cachedGameManager = methodInfo2.Invoke(null, null);
			return cachedGameManager;
		}
		catch
		{
			return null;
		}
	}

	private unsafe static List<object> GetAllActiveArcanaTypes()
	{
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		List<object> list = new List<object>();
		try
		{
			Assembly assembly = typeof(WeaponData).Assembly;
			if (cachedArcanaTypeEnum == null)
			{
				cachedArcanaTypeEnum = assembly.GetTypes().FirstOrDefault((Type t) => t.Name == "ArcanaType");
			}
			if (cachedArcanaTypeEnum == null)
			{
				return list;
			}
			object gameManager = GetGameManager();
			if (gameManager == null)
			{
				return list;
			}
			PropertyInfo property = gameManager.GetType().GetProperty("_arcanaManager", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (property == null)
			{
				return list;
			}
			object value = property.GetValue(gameManager);
			if (value == null)
			{
				return list;
			}
			if (!arcanaDebugLogged)
			{
				arcanaDebugLogged = true;
				List<object> allActiveArcanaTypesInternal = GetAllActiveArcanaTypesInternal(value);
				foreach (object item in allActiveArcanaTypesInternal)
				{
					object arcanaData = GetArcanaData(item);
					if (arcanaData == null)
					{
						continue;
					}
					object obj = arcanaData.GetType().GetProperty("name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.GetValue(arcanaData);
					PropertyInfo property2 = arcanaData.GetType().GetProperty("weapons", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (property2 != null)
					{
						object value2 = property2.GetValue(arcanaData);
						if (value2 != null)
						{
							int num = (int)value2.GetType().GetProperty("Count").GetValue(value2);
							PropertyInfo property3 = value2.GetType().GetProperty("Item");
							List<string> list2 = new List<string>();
							for (int num2 = 0; num2 < num; num2++)
							{
								object value3 = property3.GetValue(value2, new object[1] { num2 });
								if (value3 == null)
								{
									list2.Add("null");
									continue;
								}
								Object val = (Object)((value3 is Object) ? value3 : null);
								if (val != null)
								{
									try
									{
										int* ptr = (int*)((Il2CppObjectBase)val).Pointer.ToPointer() + 4;
										int num3 = *ptr;
										object obj2 = (Enum.IsDefined(typeof(WeaponType), num3) ? ((object)(WeaponType)num3/*cast due to constrained. prefix*/).ToString() : "UNDEFINED");
										string arg = (string)obj2;
										list2.Add($"{arg}({num3})");
									}
									catch
									{
										list2.Add("decode_error");
									}
								}
								else
								{
									list2.Add($"not_il2cpp:{value3}");
								}
							}
						}
					}
					PropertyInfo property4 = arcanaData.GetType().GetProperty("items", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (!(property4 != null))
					{
						continue;
					}
					object value4 = property4.GetValue(arcanaData);
					if (value4 == null)
					{
						continue;
					}
					int num4 = (int)value4.GetType().GetProperty("Count").GetValue(value4);
					PropertyInfo property5 = value4.GetType().GetProperty("Item");
					List<string> list3 = new List<string>();
					for (int num5 = 0; num5 < num4; num5++)
					{
						object value5 = property5.GetValue(value4, new object[1] { num5 });
						if (value5 == null)
						{
							list3.Add("null");
							continue;
						}
						Object val2 = (Object)((value5 is Object) ? value5 : null);
						if (val2 != null)
						{
							try
							{
								int* ptr2 = (int*)((Il2CppObjectBase)val2).Pointer.ToPointer() + 4;
								int num6 = *ptr2;
								object obj4 = (Enum.IsDefined(typeof(ItemType), num6) ? ((object)(ItemType)num6/*cast due to constrained. prefix*/).ToString() : "UNDEFINED");
								string arg2 = (string)obj4;
								list3.Add($"{arg2}({num6})");
							}
							catch
							{
								list3.Add("decode_error");
							}
						}
						else
						{
							list3.Add($"not_il2cpp:{value5}");
						}
					}
				}
			}
			string[] array = new string[12]
			{
				"ActiveArcanas", "_activeArcanas", "ChosenArcanas", "_chosenArcanas", "PlayerArcanas", "_playerArcanas", "SelectedArcanas", "_selectedArcanas", "OwnedArcanas", "_ownedArcanas",
				"CurrentArcanas", "_currentArcanas"
			};
			object obj6 = null;
			string[] array2 = array;
			string[] array3 = array2;
			foreach (string name in array3)
			{
				PropertyInfo property6 = value.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (property6 != null)
				{
					obj6 = property6.GetValue(value);
					if (obj6 != null)
					{
						break;
					}
				}
				FieldInfo field = value.GetType().GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (field != null)
				{
					obj6 = field.GetValue(value);
					if (obj6 != null)
					{
						break;
					}
				}
			}
			if (obj6 != null)
			{
				PropertyInfo property7 = obj6.GetType().GetProperty("Count");
				PropertyInfo property8 = obj6.GetType().GetProperty("Item");
				if (property7 != null && property8 != null)
				{
					int num8 = (int)property7.GetValue(obj6);
					for (int num9 = 0; num9 < num8; num9++)
					{
						object value6 = property8.GetValue(obj6, new object[1] { num9 });
						if (value6 != null)
						{
							list.Add(value6);
						}
					}
					if (list.Count > 0)
					{
						return list;
					}
				}
			}
			PropertyInfo property9 = value.GetType().GetProperty("_playerOptions", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (property9 == null)
			{
				return list;
			}
			object value7 = property9.GetValue(value);
			if (value7 == null)
			{
				return list;
			}
			PropertyInfo property10 = value7.GetType().GetProperty("Config", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (property10 == null)
			{
				return list;
			}
			object value8 = property10.GetValue(value7);
			if (value8 == null)
			{
				return list;
			}
			PropertyInfo property11 = value8.GetType().GetProperty("SelectedMazzo", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (property11 != null && !(bool)property11.GetValue(value8))
			{
				return list;
			}
			PropertyInfo property12 = value8.GetType().GetProperty("SelectedArcana", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (property12 == null)
			{
				return list;
			}
			int num10 = (int)property12.GetValue(value8);
			Array values = Enum.GetValues(cachedArcanaTypeEnum);
			foreach (object item2 in values)
			{
				if ((int)item2 == num10)
				{
					list.Add(item2);
					break;
				}
			}
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Arcana] Error getting active arcanas: " + ex.Message);
		}
		return list;
	}

	private static List<object> GetAllActiveArcanaTypesInternal(object arcanaMgr)
	{
		List<object> list = new List<object>();
		try
		{
			PropertyInfo property = arcanaMgr.GetType().GetProperty("ActiveArcanas", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (property == null)
			{
				return list;
			}
			object value = property.GetValue(arcanaMgr);
			if (value == null)
			{
				return list;
			}
			PropertyInfo property2 = value.GetType().GetProperty("Count");
			PropertyInfo property3 = value.GetType().GetProperty("Item");
			if (property2 == null || property3 == null)
			{
				return list;
			}
			int num = (int)property2.GetValue(value);
			for (int i = 0; i < num; i++)
			{
				object value2 = property3.GetValue(value, new object[1] { i });
				if (value2 != null)
				{
					list.Add(value2);
				}
			}
		}
		catch
		{
		}
		return list;
	}

	private static object GetArcanaData(object arcanaType)
	{
		try
		{
			if (cachedDataManager == null || arcanaType == null)
			{
				return null;
			}
			if (cachedAllArcanas == null)
			{
				PropertyInfo property = cachedDataManager.GetType().GetProperty("AllArcanas", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (property != null)
				{
					cachedAllArcanas = property.GetValue(cachedDataManager);
				}
			}
			if (cachedAllArcanas == null)
			{
				return null;
			}
			PropertyInfo property2 = cachedAllArcanas.GetType().GetProperty("Item");
			if (property2 == null)
			{
				return null;
			}
			return property2.GetValue(cachedAllArcanas, new object[1] { arcanaType });
		}
		catch
		{
			return null;
		}
	}

	private unsafe static bool IsWeaponAffectedByArcana(WeaponType weaponType, object arcanaData)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			string text = ((object)(*(WeaponType*)(&weaponType))/*cast due to constrained. prefix*/).ToString();
			List<WeaponType> arcanaAffectedWeaponTypes = GetArcanaAffectedWeaponTypes(arcanaData);
			foreach (WeaponType item in arcanaAffectedWeaponTypes)
			{
				if (item == weaponType)
				{
					return true;
				}
			}
			return false;
		}
		catch
		{
			return false;
		}
	}

	private static bool IsItemAffectedByArcana(ItemType itemType, object arcanaData)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			List<ItemType> arcanaAffectedItemTypes = GetArcanaAffectedItemTypes(arcanaData);
			foreach (ItemType item in arcanaAffectedItemTypes)
			{
				if (item == itemType)
				{
					return true;
				}
			}
			return false;
		}
		catch
		{
			return false;
		}
	}

	private static List<WeaponType> GetArcanaAffectedWeaponTypes(object arcanaData)
	{
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		List<WeaponType> list = new List<WeaponType>();
		HashSet<string> hashSet = new HashSet<string>();
		try
		{
			if (arcanaData == null)
			{
				return list;
			}
			PropertyInfo property = arcanaData.GetType().GetProperty("weapons", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (property == null)
			{
				return list;
			}
			object value = property.GetValue(arcanaData);
			if (value == null)
			{
				return list;
			}
			PropertyInfo property2 = value.GetType().GetProperty("Count");
			if (property2 == null)
			{
				return list;
			}
			int num = (int)property2.GetValue(value);
			PropertyInfo property3 = value.GetType().GetProperty("Item");
			if (property3 == null)
			{
				return list;
			}
			for (int i = 0; i < num; i++)
			{
				object value2 = property3.GetValue(value, new object[1] { i });
				if (value2 == null)
				{
					continue;
				}
				Object val = (Object)((value2 is Object) ? value2 : null);
				if (val == null)
				{
					continue;
				}
				try
				{
					string text = val.ToString();
					if (!string.IsNullOrEmpty(text) && hashSet.Add(text) && GameData.TryParseWeaponType(text, out WeaponType result))
					{
						list.Add(result);
					}
				}
				catch
				{
				}
			}
		}
		catch
		{
		}
		return list;
	}

	private static List<ItemType> GetArcanaAffectedItemTypes(object arcanaData)
	{
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		List<ItemType> list = new List<ItemType>();
		HashSet<string> hashSet = new HashSet<string>();
		try
		{
			if (arcanaData == null)
			{
				return list;
			}
			PropertyInfo property = arcanaData.GetType().GetProperty("items", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (property == null)
			{
				return list;
			}
			object value = property.GetValue(arcanaData);
			if (value == null)
			{
				return list;
			}
			PropertyInfo property2 = value.GetType().GetProperty("Count");
			if (property2 == null)
			{
				return list;
			}
			int num = (int)property2.GetValue(value);
			PropertyInfo property3 = value.GetType().GetProperty("Item");
			if (property3 == null)
			{
				return list;
			}
			for (int i = 0; i < num; i++)
			{
				object value2 = property3.GetValue(value, new object[1] { i });
				if (value2 == null)
				{
					continue;
				}
				Object val = (Object)((value2 is Object) ? value2 : null);
				if (val == null)
				{
					continue;
				}
				try
				{
					string text = val.ToString();
					if (!string.IsNullOrEmpty(text) && hashSet.Add(text) && Enum.TryParse<ItemType>(text, out ItemType result))
					{
						list.Add(result);
					}
				}
				catch
				{
				}
			}
		}
		catch
		{
		}
		return list;
	}

	private static Sprite LoadArcanaSprite(string textureName, string frameName)
	{
		string text = frameName;
		if (text.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
		{
			text = text.Substring(0, text.Length - 4);
		}
		if (!string.IsNullOrEmpty(textureName))
		{
			string[] array = new string[3]
			{
				frameName,
				text,
				text + ".png"
			};
			string[] array2 = array;
			string[] array3 = array2;
			foreach (string frameName2 in array3)
			{
				Sprite val = LoadSpriteFromAtlas(frameName2, textureName);
				if ((Object)(object)val != (Object)null)
				{
					return val;
				}
			}
		}
		Il2CppArrayBase<Sprite> val2 = Resources.FindObjectsOfTypeAll<Sprite>();
		foreach (Sprite item in val2)
		{
			if (!((Object)(object)item == (Object)null) && !((Object)(object)item.texture == (Object)null))
			{
				string text2 = ((Object)item.texture).name.ToLower();
				string text3 = ((Object)item).name.ToLower();
				if (!string.IsNullOrEmpty(textureName) && text2.Contains(textureName.ToLower()) && (text3 == text.ToLower() || text3 == frameName.ToLower()))
				{
					return item;
				}
			}
		}
		string[] array4 = new string[5] { "arcanas", "cards", "items", "ui", "randomazzo" };
		string[] array5 = array4;
		string[] array6 = array5;
		foreach (string atlasName in array6)
		{
			Sprite val3 = LoadSpriteFromAtlas(text, atlasName);
			if ((Object)(object)val3 != (Object)null)
			{
				return val3;
			}
			val3 = LoadSpriteFromAtlas(frameName, atlasName);
			if ((Object)(object)val3 != (Object)null)
			{
				return val3;
			}
		}
		return null;
	}

	private unsafe static int GetArcanaTypeInt(object arcanaType)
	{
		if (arcanaType == null)
		{
			return -1;
		}
		try
		{
			Object val = (Object)((arcanaType is Object) ? arcanaType : null);
			if (val != null)
			{
				int* ptr = (int*)((Il2CppObjectBase)val).Pointer.ToPointer() + 4;
				return *ptr;
			}
		}
		catch
		{
		}
		try
		{
			return Convert.ToInt32(arcanaType);
		}
		catch
		{
		}
		try
		{
			MethodInfo method = arcanaType.GetType().GetMethod("Unbox", BindingFlags.Instance | BindingFlags.Public);
			if (method != null)
			{
				object value = method.Invoke(arcanaType, null);
				return Convert.ToInt32(value);
			}
		}
		catch
		{
		}
		Plugin.Log.LogWarning($"[Arcana] GetArcanaTypeInt failed for type {arcanaType.GetType().FullName}, value: {arcanaType}");
		return -1;
	}

	private static void ScanArcanaUI(int arcanaTypeInt, string arcanaName)
	{
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		if (arcanaUICache.ContainsKey(arcanaTypeInt) || !lookupTablesBuilt || spriteToWeaponType == null || spriteToItemType == null)
		{
			return;
		}
		HashSet<WeaponType> hashSet = new HashSet<WeaponType>();
		HashSet<ItemType> hashSet2 = new HashSet<ItemType>();
		try
		{
			Il2CppArrayBase<TextMeshProUGUI> val = Object.FindObjectsOfType<TextMeshProUGUI>();
			Transform val2 = null;
			string text = arcanaName.Trim();
			string value = (text.Contains(" - ") ? text.Substring(text.IndexOf(" - ") + 3).Trim() : text);
			int num = 0;
			foreach (TextMeshProUGUI item in val)
			{
				if ((Object)(object)item == (Object)null || ((TMP_Text)item).text == null)
				{
					continue;
				}
				num++;
				string text2 = ((TMP_Text)item).text.Trim();
				string text3 = Regex.Replace(text2, "<[^>]+>", "").Trim();
				bool flag = text2 == text || text3 == text || text2.Contains(text) || text3.Contains(text) || text2.Contains(value) || text3.Contains(value);
				if (text2.Contains("Gemini") || text3.Contains("Gemini") || text2.Contains("gemini") || text3.Contains("gemini") || text2.Contains("arcana") || text2.Contains("Arcana"))
				{
				}
				if (!flag)
				{
					continue;
				}
				Transform parent = ((TMP_Text)item).transform.parent;
				for (int i = 0; i < 8; i++)
				{
					if (!((Object)(object)parent != (Object)null))
					{
						break;
					}
					Il2CppArrayBase<Image> componentsInChildren = ((Component)parent).GetComponentsInChildren<Image>();
					if (componentsInChildren.Length >= 10)
					{
						val2 = parent;
						break;
					}
					parent = parent.parent;
				}
				if (!((Object)(object)val2 != (Object)null))
				{
					continue;
				}
				break;
			}
			if ((Object)(object)val2 == (Object)null)
			{
				return;
			}
			Il2CppArrayBase<Image> componentsInChildren2 = ((Component)val2).GetComponentsInChildren<Image>();
			foreach (Image item2 in componentsInChildren2)
			{
				if ((Object)(object)item2 == (Object)null || (Object)(object)item2.sprite == (Object)null)
				{
					continue;
				}
				string name = ((Object)item2.sprite).name;
				if (!string.IsNullOrEmpty(name))
				{
					string text4 = name;
					if (text4.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
					{
						text4 = text4.Substring(0, text4.Length - 4);
					}
					WeaponType value3;
					ItemType value4;
					ItemType value5;
					if (spriteToWeaponType.TryGetValue(name, out var value2))
					{
						hashSet.Add(value2);
					}
					else if (spriteToWeaponType.TryGetValue(text4, out value3))
					{
						hashSet.Add(value3);
					}
					else if (spriteToItemType.TryGetValue(name, out value4))
					{
						hashSet2.Add(value4);
					}
					else if (spriteToItemType.TryGetValue(text4, out value5))
					{
						hashSet2.Add(value5);
					}
				}
			}
			if (hashSet.Count > 0 || hashSet2.Count > 0)
			{
				arcanaUICache[arcanaTypeInt] = (hashSet, hashSet2);
			}
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Arcana] Error scanning arcana UI: " + ex.Message);
		}
	}

	private static List<WeaponType> GetAllArcanaAffectedWeaponTypes(object arcanaData, int arcanaTypeInt = -1, string arcanaName = null)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		HashSet<WeaponType> hashSet = new HashSet<WeaponType>();
		foreach (WeaponType arcanaAffectedWeaponType in GetArcanaAffectedWeaponTypes(arcanaData))
		{
			hashSet.Add(arcanaAffectedWeaponType);
		}
		if (arcanaTypeInt < 0 && arcanaName == null)
		{
			arcanaName = arcanaData.GetType().GetProperty("name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.GetValue(arcanaData)?.ToString() ?? "";
			if (arcanaNameToInt.TryGetValue(arcanaName, out var value))
			{
				arcanaTypeInt = value;
			}
		}
		foreach (WeaponType panelCapturedWeapon in panelCapturedWeapons)
		{
			hashSet.Add(panelCapturedWeapon);
		}
		if (arcanaTypeInt >= 0)
		{
			if (!arcanaUICache.ContainsKey(arcanaTypeInt))
			{
				ScanArcanaUI(arcanaTypeInt, arcanaName ?? "");
			}
			if (arcanaUICache.TryGetValue(arcanaTypeInt, out (HashSet<WeaponType>, HashSet<ItemType>) value2))
			{
				foreach (WeaponType item in value2.Item1)
				{
					hashSet.Add(item);
				}
			}
		}
		return hashSet.ToList();
	}

	private static List<ItemType> GetAllArcanaAffectedItemTypes(object arcanaData, int arcanaTypeInt = -1, string arcanaName = null)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		HashSet<ItemType> hashSet = new HashSet<ItemType>();
		foreach (ItemType arcanaAffectedItemType in GetArcanaAffectedItemTypes(arcanaData))
		{
			hashSet.Add(arcanaAffectedItemType);
		}
		if (arcanaTypeInt < 0 && arcanaName == null)
		{
			arcanaName = arcanaData.GetType().GetProperty("name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.GetValue(arcanaData)?.ToString() ?? "";
			if (arcanaNameToInt.TryGetValue(arcanaName, out var value))
			{
				arcanaTypeInt = value;
			}
		}
		foreach (ItemType panelCapturedItem in panelCapturedItems)
		{
			hashSet.Add(panelCapturedItem);
		}
		if (arcanaTypeInt >= 0)
		{
			if (!arcanaUICache.ContainsKey(arcanaTypeInt))
			{
				ScanArcanaUI(arcanaTypeInt, arcanaName ?? "");
			}
			if (arcanaUICache.TryGetValue(arcanaTypeInt, out (HashSet<WeaponType>, HashSet<ItemType>) value2))
			{
				foreach (ItemType item in value2.Item2)
				{
					hashSet.Add(item);
				}
			}
		}
		return hashSet.ToList();
	}

	public static void CaptureArcanaAffectedWeapon(object arcanaInfoPanel, WeaponType weaponType)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			panelCapturedWeapons.Add(weaponType);
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Arcana] Error capturing weapon from patch: " + ex.Message);
		}
	}

	public static void CaptureArcanaAffectedItem(object arcanaInfoPanel, ItemType itemType)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			panelCapturedItems.Add(itemType);
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning("[Arcana] Error capturing item from patch: " + ex.Message);
		}
	}

	/// <summary>
	/// Typed: all arcanas whose weapons list includes this weapon (from DataManager.AllArcanas).
	/// No longer limited to "active run" arcanas — data-driven so collection + pause work reliably.
	/// </summary>
	private static List<ArcanaInfo> GetActiveArcanasForWeapon(WeaponType weaponType)
	{
		List<ArcanaInfo> list = new List<ArcanaInfo>();
		try
		{
			GameData.EnsureLoaded();
			var found = GameData.GetArcanasAffectingWeapon(weaponType);
			var seen = new HashSet<ArcanaType>();
			foreach (var a in found)
			{
				if (!seen.Add(a.Type)) continue;
				list.Add(new ArcanaInfo
				{
					Name = a.Name,
					Description = a.Description,
					Sprite = a.Sprite,
					Type = a.Type,
					ArcanaData = a.Data
				});
			}
			// Panel captures: if we saw this weapon on an ArcanaInfoPanel during the session, include that arcana too
			if (panelCapturedWeapons.Contains(weaponType))
			{
				// Already covered by typed index when data is complete; nothing extra without type mapping
			}
			Plugin.Dbg($"GetActiveArcanasForWeapon({weaponType}): {list.Count}");
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning($"[Arcana] Error getting arcanas for weapon {weaponType}: {ex.Message}");
		}
		return list;
	}

	private static List<ArcanaInfo> GetActiveArcanasForItem(ItemType itemType)
	{
		List<ArcanaInfo> list = new List<ArcanaInfo>();
		try
		{
			GameData.EnsureLoaded();
			var found = GameData.GetArcanasAffectingItem(itemType);
			var seen = new HashSet<ArcanaType>();
			foreach (var a in found)
			{
				if (!seen.Add(a.Type)) continue;
				list.Add(new ArcanaInfo
				{
					Name = a.Name,
					Description = a.Description,
					Sprite = a.Sprite,
					Type = a.Type,
					ArcanaData = a.Data
				});
			}
			Plugin.Dbg($"GetActiveArcanasForItem({itemType}): {list.Count}");
		}
		catch (Exception ex)
		{
			Plugin.Log.LogWarning($"[Arcana] Error getting arcanas for item {itemType}: {ex.Message}");
		}
		return list;
	}
}
