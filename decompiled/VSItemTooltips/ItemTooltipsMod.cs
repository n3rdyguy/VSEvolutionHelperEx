using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppTMPro;
using Il2CppVampireSurvivors.Data;
using Il2CppVampireSurvivors.Data.Weapons;
using Il2CppVampireSurvivors.UI;
using MelonLoader;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

namespace VSItemTooltips;

public class ItemTooltipsMod : MelonMod
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

		public object ArcanaData;

		public object ArcanaType;
	}

	private static Harmony harmonyInstance;

	private static bool wasGamePaused = false;

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

	private static int currentCollectionHoverId = -1;

	private static GameObject collectionPopup = null;

	private static bool usingController = false;

	private static Vector3 lastMousePosition = Vector3.zero;

	private static GameObject lastSelectedObject = null;

	private static float dwellStartTime = 0f;

	private static readonly float DwellDelay = 0.5f;

	private static GameObject dwellTarget = null;

	private static bool passivePopupShown = false;

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

	private static readonly float CollectionHoverDelay = 1f;

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

	public override void OnInitializeMelon()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected O, but got Unknown
		harmonyInstance = new Harmony("com.nihil.vsitemtooltips");
		ApplyPatches();
		MelonLogger.Msg("VS Item Tooltips initialized!");
	}

	private void ApplyPatches()
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
				MelonLogger.Warning("LevelUpPage.OnShowStart method not found!");
			}
			TryPatchMerchantPage();
			TryPatchLevelUpItemUI();
			TryPatchEquipmentIconPause();
			MelonLogger.Msg("Patches applied successfully");
		}
		catch (Exception arg)
		{
			MelonLogger.Error($"Failed to apply patches: {arg}");
		}
	}

	private void TryPatchLevelUpItemUI()
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
							MelonLogger.Warning("SetWeaponData method not found on LevelUpItemUI");
						}
						MethodInfo method2 = type.GetMethod("SetItemData", BindingFlags.Instance | BindingFlags.Public);
						if (method2 != null)
						{
							harmonyInstance.Patch((MethodBase)method2, (HarmonyMethod)null, new HarmonyMethod(typeof(LevelUpItemUIPatches), "SetItemData_Postfix", (Type[])null), (HarmonyMethod)null, (HarmonyMethod)null, (HarmonyMethod)null);
						}
						else
						{
							MelonLogger.Warning("SetItemData method not found on LevelUpItemUI");
						}
						return;
					}
				}
				catch
				{
				}
			}
			MelonLogger.Warning("LevelUpItemUI type not found in any assembly");
		}
		catch (Exception arg)
		{
			MelonLogger.Error($"Error patching LevelUpItemUI: {arg}");
		}
	}

	private void TryPatchEquipmentIconPause()
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
				if (!assembly2.FullName.Contains("Il2Cpp"))
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
								MelonLogger.Warning("  Failed to patch: " + ex.Message);
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
			MelonLogger.Warning("Error searching for icon types: " + ex2.Message);
		}
	}

	private void TryPatchMerchantPage()
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
			MelonLogger.Warning("Could not patch MerchantPage: " + ex.Message);
		}
	}

	public override void OnSceneWasLoaded(int buildIndex, string sceneName)
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

	private void TryEarlyCaching()
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
				Type type = AppDomain.CurrentDomain.GetAssemblies().SelectMany(delegate(Assembly a)
				{
					try
					{
						return a.GetTypes();
					}
					catch
					{
						return new Type[0];
					}
				}).FirstOrDefault((Type t) => t.Name == "DataManager" && t.Namespace != null && t.Namespace.Contains("VampireSurvivors"));
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
			Type type2 = AppDomain.CurrentDomain.GetAssemblies().SelectMany(delegate(Assembly a)
			{
				try
				{
					return a.GetTypes();
				}
				catch
				{
					return new Type[0];
				}
			}).FirstOrDefault((Type t) => t.Name == "GameManager");
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
			MelonLogger.Warning("Early caching failed: " + ex.Message);
		}
	}

	public override void OnUpdate()
	{
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
			if ((Object)(object)hudInventory != (Object)null && cachedGameSession != null)
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
		if (usingController && !equipmentNavMode)
		{
			UpdateControllerDwell();
		}
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
				MelonLogger.Warning("Safe Area not found!");
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
				MelonLogger.Warning("Lookup tables not built - no DataManager cached yet. Hovers won't work until level-up.");
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
			if ((Object)(object)trackedIcon2.Value.Image == (Object)null || !Object.op_Implicit((Object)(object)trackedIcon2.Value.Image))
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
			Entry val2 = new Entry();
			val2.eventID = (EventTriggerType)0;
			((UnityEvent<BaseEventData>)(object)val2.callback).AddListener(UnityAction<BaseEventData>.op_Implicit((Action<BaseEventData>)delegate
			{
				ShowItemPopup(((Component)tracked.Image).transform, weaponType, itemType);
			}));
			val.triggers.Add(val2);
			Entry val3 = new Entry();
			val3.eventID = (EventTriggerType)1;
			((UnityEvent<BaseEventData>)(object)val3.callback).AddListener(UnityAction<BaseEventData>.op_Implicit((Action<BaseEventData>)delegate
			{
				MelonCoroutines.Start(DelayedHideCheck());
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
			MelonLogger.Warning("WeaponSelectionItemUI type not found in assemblies");
			return;
		}
		Transform val = FindChildRecursive(viewGo.transform, "Panel");
		if ((Object)(object)val == (Object)null)
		{
			MelonLogger.Warning("Panel not found in WeaponSelection");
			return;
		}
		Transform val2 = val.Find("ScrollViewWithSlider");
		if ((Object)(object)val2 == (Object)null)
		{
			MelonLogger.Warning("ScrollViewWithSlider not found");
			return;
		}
		Transform val3 = val2.Find("Viewport");
		if ((Object)(object)val3 == (Object)null)
		{
			MelonLogger.Warning("Viewport not found");
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
			MelonLogger.Warning("Content not found");
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
					MelonLogger.Warning("Error reading WSI component: " + ex.Message);
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
		MelonLogger.Msg($"WeaponSelection: set up hovers on {num2}/{val4.childCount} items");
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
				MelonLogger.Msg($"Built lookup tables: {spriteToWeaponType.Count} weapons, {spriteToItemType.Count} items");
			}
		}
		catch (Exception arg)
		{
			MelonLogger.Error($"Failed to build lookup tables: {arg}");
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
			MelonLogger.Warning("[MergeWeaponDicts] " + ex.Message);
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
						MelonLogger.Msg($"[EvoSynergy] Inner error for {val}: {ex.Message}");
					}
				}
			}
		}
		catch (Exception ex2)
		{
			MelonLogger.Warning("Error building evo synergy lookup: " + ex2.Message);
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
					object? value5 = property5.GetValue(value4, new object[1] { i });
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
			MelonLogger.Warning($"Error building weapon lookup: {arg}");
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
			MelonLogger.Warning("Error building powerup lookup: " + ex.Message);
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
			MelonLogger.Warning("[CacheGameSession] Could not find Data on GameSession");
		}
		catch (Exception ex)
		{
			MelonLogger.Warning("Error caching DataManager from session: " + ex.Message);
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
				if (!assembly2.FullName.Contains("Il2Cpp"))
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
			MelonLogger.Warning("Error finding game session: " + ex.Message);
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
			MelonLogger.Warning("Error setting up HUD hovers: " + ex.Message);
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
			MelonLogger.Warning("Error in SetupHUDSlots: " + ex.Message);
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
			Type type = AppDomain.CurrentDomain.GetAssemblies().SelectMany(delegate(Assembly a)
			{
				try
				{
					return a.GetTypes();
				}
				catch
				{
					return new Type[0];
				}
			}).FirstOrDefault((Type t) => t.Name == "GameManager");
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
			MelonLogger.Warning("[Collection] Data caching failed: " + ex.Message);
		}
	}

	public static void CacheDataManager(object dataManager)
	{
		if (dataManager == null)
		{
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
			MelonLogger.Error($"Error caching data manager: {arg}");
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
		bool flag = ((Vector3)(ref val)).sqrMagnitude > 1f;
		lastMousePosition = mousePosition;
		if (flag)
		{
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
		EventSystem current = EventSystem.current;
		if (!((Object)(object)current == (Object)null))
		{
			GameObject currentSelectedGameObject = current.currentSelectedGameObject;
			if ((Object)(object)currentSelectedGameObject != (Object)(object)lastSelectedObject && (Object)(object)currentSelectedGameObject != (Object)null && !usingController)
			{
				usingController = true;
			}
			lastSelectedObject = currentSelectedGameObject;
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
			val.mode = (Mode)4;
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
					navigation.mode = (Mode)0;
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
				((ColorBlock)(ref colors)).normalColor = new Color(1f, 1f, 1f, 0f);
				((ColorBlock)(ref colors)).highlightedColor = new Color(1f, 1f, 1f, 0f);
				((ColorBlock)(ref colors)).pressedColor = new Color(1f, 1f, 1f, 0f);
				((ColorBlock)(ref colors)).selectedColor = new Color(1f, 1f, 1f, 0f);
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
			val.mode = (Mode)4;
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
					navigation.mode = (Mode)0;
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
				float width = ((Rect)(ref rect)).width;
				rect = component.rect;
				float num2 = Mathf.Max(width, ((Rect)(ref rect)).height);
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
						navigation.mode = (Mode)0;
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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		if ((weaponType.HasValue && ((object)weaponType.Value/*cast due to constrained. prefix*/).ToString() == "DEFANG") || (itemType.HasValue && ((object)itemType.Value/*cast due to constrained. prefix*/).ToString() == "DEFANG"))
		{
			return;
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
		string text = "Unknown";
		string text2 = "";
		Sprite val5 = null;
		if (text.Contains("/"))
		{
			Object.Destroy((Object)(object)val);
			return null;
		}
		if (weaponType.HasValue && cachedWeaponsDict != null)
		{
			WeaponData weaponData = GetWeaponData(weaponType.Value);
			if (weaponData != null)
			{
				text = GetLocalizedWeaponName(weaponData, weaponType.Value);
				text2 = GetLocalizedWeaponDescription(weaponData, weaponType.Value);
				val5 = GetSpriteForWeapon(weaponType.Value);
			}
		}
		else if (itemType.HasValue && cachedPowerUpsDict != null)
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
			((Graphic)val10).color = Color.white;
			((TMP_Text)val10).alignment = (TextAlignmentOptions)513;
			((TMP_Text)val10).enableAutoSizing = true;
			((TMP_Text)val10).fontSizeMin = 12f;
			((TMP_Text)val10).fontSizeMax = 20f;
			((TMP_Text)val10).overflowMode = (TextOverflowModes)1;
			if ((Object)(object)val5 != (Object)null)
			{
				GameObject val11 = new GameObject("HeaderIcon");
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
				GameObject val14 = new GameObject("Description");
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
				val17.horizontalFit = (FitMode)0;
				val17.verticalFit = (FitMode)2;
				((TMP_Text)val16).ForceMeshUpdate(false, false);
				LayoutRebuilder.ForceRebuildLayoutImmediate(val15);
				float num4 = ((((TMP_Text)val16).preferredHeight > 0f) ? ((TMP_Text)val16).preferredHeight : 40f);
				val15.sizeDelta = new Vector2(num3, num4);
				num -= num4 + Spacing;
			}
			if (weaponType.HasValue)
			{
				num = AddWeaponEvolutionSection(val.transform, font, weaponType.Value, num, num2);
			}
			else if (itemType.HasValue)
			{
				string value = ((object)itemType.Value/*cast due to constrained. prefix*/).ToString();
				bool flag = false;
				if (Enum.TryParse<WeaponType>(value, out WeaponType result) && GetWeaponData(result) != null)
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
		if (!Enum.TryParse<WeaponType>(evoInto, out WeaponType result))
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
			MelonLogger.Warning($"[PlayerOwnsWeapon] Error checking {weaponType}: {ex.Message}");
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

	private unsafe static float AddWeaponEvolutionSection(Transform parent, TMP_FontAsset font, WeaponType weaponType, float yOffset, float maxWidth)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Unknown result type (might be due to invalid IL or missing references)
		//IL_0457: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0910: Unknown result type (might be due to invalid IL or missing references)
		//IL_0927: Unknown result type (might be due to invalid IL or missing references)
		//IL_093e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0951: Unknown result type (might be due to invalid IL or missing references)
		//IL_0971: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a71: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d26: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0557: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0692: Unknown result type (might be due to invalid IL or missing references)
		//IL_0697: Unknown result type (might be due to invalid IL or missing references)
		//IL_0699: Unknown result type (might be due to invalid IL or missing references)
		//IL_069b: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0637: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ed: Expected I4, but got Unknown
		//IL_0846: Unknown result type (might be due to invalid IL or missing references)
		//IL_0854: Unknown result type (might be due to invalid IL or missing references)
		//IL_0862: Unknown result type (might be due to invalid IL or missing references)
		//IL_0814: Unknown result type (might be due to invalid IL or missing references)
		//IL_0818: Unknown result type (might be due to invalid IL or missing references)
		//IL_0826: Unknown result type (might be due to invalid IL or missing references)
		//IL_0834: Unknown result type (might be due to invalid IL or missing references)
		//IL_0873: Unknown result type (might be due to invalid IL or missing references)
		WeaponData weaponData = GetWeaponData(weaponType);
		if (weaponData == null)
		{
			return yOffset;
		}
		string text = null;
		Il2CppStructArray<WeaponType> val = null;
		string text2 = null;
		try
		{
			text = GetPropertyValue<string>(weaponData, "evoInto");
			PropertyInfo property = ((object)weaponData).GetType().GetProperty("evoSynergy");
			if (property != null)
			{
				val = property.GetValue(weaponData) as Il2CppStructArray<WeaponType>;
			}
			if (string.IsNullOrEmpty(text) && cachedWeaponsDict != null)
			{
				PropertyInfo property2 = cachedWeaponsDict.GetType().GetProperty("Keys");
				if (property2 != null)
				{
					object value = property2.GetValue(cachedWeaponsDict);
					object obj = value.GetType().GetMethod("GetEnumerator").Invoke(value, null);
					MethodInfo method = obj.GetType().GetMethod("MoveNext");
					PropertyInfo property3 = obj.GetType().GetProperty("Current");
					while ((bool)method.Invoke(obj, null))
					{
						WeaponType type = (WeaponType)property3.GetValue(obj);
						List<WeaponData> weaponDataList = GetWeaponDataList(type);
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
								if (!GetPropertyValue<bool>(val2, "isEvolution"))
								{
									continue;
								}
								PropertyInfo property4 = ((object)val2).GetType().GetProperty("evolvesFrom");
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
								PropertyInfo property6 = value2.GetType().GetProperty("Item");
								int num = ((property5 != null) ? ((int)property5.GetValue(value2)) : 0);
								for (int j = 0; j < num; j++)
								{
									WeaponType val3 = (WeaponType)property6.GetValue(value2, new object[1] { j });
									if (val3 == weaponType)
									{
										text2 = ((object)(*(WeaponType*)(&type))/*cast due to constrained. prefix*/).ToString();
										break;
									}
								}
								if (text2 == null)
								{
									object obj2 = ((object)val2).GetType().GetProperty("requires")?.GetValue(val2);
									if (obj2 != null)
									{
										int num2 = (int)obj2.GetType().GetProperty("Count").GetValue(obj2);
										PropertyInfo property7 = obj2.GetType().GetProperty("Item");
										for (int k = 0; k < num2; k++)
										{
											WeaponType val4 = (WeaponType)property7.GetValue(obj2, new object[1] { k });
											if (val4 == weaponType)
											{
												text2 = ((object)(*(WeaponType*)(&type))/*cast due to constrained. prefix*/).ToString();
												break;
											}
										}
									}
								}
								goto IL_0311;
							}
							catch
							{
								goto IL_0311;
							}
							IL_0311:
							if (text2 != null)
							{
								break;
							}
						}
						if (text2 == null)
						{
							continue;
						}
						break;
					}
				}
			}
		}
		catch (Exception ex)
		{
			MelonLogger.Msg("[EvoError] " + ex.Message);
		}
		bool flag = !string.IsNullOrEmpty(text) || !string.IsNullOrEmpty(text2);
		if (string.IsNullOrEmpty(text))
		{
			text = text2;
		}
		int num3 = CountPassiveUses(weaponType, text);
		if (num3 >= 2)
		{
			yOffset = AddPassiveEvolutionSection(parent, font, weaponType, yOffset, maxWidth);
			yOffset = AddEvolvedFromSection(parent, font, weaponType, yOffset, maxWidth);
			return yOffset;
		}
		WeaponType? val5 = null;
		Sprite sprite = null;
		if (Enum.TryParse<WeaponType>(text, out WeaponType result))
		{
			val5 = result;
			sprite = GetSpriteForWeapon(result);
		}
		Sprite spriteForWeapon = GetSpriteForWeapon(weaponType);
		bool flag2 = PlayerOwnsWeapon(weaponType);
		HashSet<int> requiresMaxFromEvolved = GetRequiresMaxFromEvolved(text);
		List<PassiveRequirement> list;
		if (val != null)
		{
			list = CollectPassiveRequirements(val, requiresMaxFromEvolved);
		}
		else
		{
			list = new List<PassiveRequirement>();
			if (Enum.TryParse<WeaponType>(text, out WeaponType result2))
			{
				List<WeaponData> weaponDataList2 = GetWeaponDataList(result2);
				if (weaponDataList2 != null)
				{
					for (int l = 0; l < weaponDataList2.Count; l++)
					{
						WeaponData val6 = weaponDataList2[l];
						if (val6 == null || !GetPropertyValue<bool>(val6, "isEvolution"))
						{
							continue;
						}
						PropertyInfo property8 = ((object)val6).GetType().GetProperty("evolvesFrom");
						if (property8 == null)
						{
							continue;
						}
						object value3 = property8.GetValue(val6);
						if (value3 == null)
						{
							continue;
						}
						PropertyInfo property9 = value3.GetType().GetProperty("Count");
						PropertyInfo property10 = value3.GetType().GetProperty("Item");
						int num4 = ((property9 != null) ? ((int)property9.GetValue(value3)) : 0);
						bool flag3 = false;
						for (int m = 0; m < num4; m++)
						{
							if ((WeaponType)property10.GetValue(value3, new object[1] { m }) == weaponType)
							{
								flag3 = true;
								break;
							}
						}
						if (!flag3)
						{
							continue;
						}
						HashSet<WeaponType> hashSet = new HashSet<WeaponType>();
						PropertyInfo property11 = ((object)val6).GetType().GetProperty("requiresMax");
						if (property11 != null)
						{
							object value4 = property11.GetValue(val6);
							if (value4 != null)
							{
								PropertyInfo property12 = value4.GetType().GetProperty("Count");
								PropertyInfo property13 = value4.GetType().GetProperty("Item");
								int num5 = ((property12 != null) ? ((int)property12.GetValue(value4)) : 0);
								for (int n = 0; n < num5; n++)
								{
									hashSet.Add((WeaponType)property13.GetValue(value4, new object[1] { n }));
								}
							}
						}
						PropertyInfo property14 = ((object)val6).GetType().GetProperty("requires");
						HashSet<WeaponType> hashSet2 = new HashSet<WeaponType>();
						for (int num6 = 0; num6 < num4; num6++)
						{
							WeaponType val7 = (WeaponType)property10.GetValue(value3, new object[1] { num6 });
							if (val7 != weaponType && !hashSet2.Contains(val7))
							{
								hashSet2.Add(val7);
								list.Add(new PassiveRequirement
								{
									WeaponType = val7,
									Sprite = GetSpriteForWeapon(val7),
									Owned = PlayerOwnsWeapon(val7),
									RequiresMaxLevel = hashSet.Contains(val7)
								});
							}
						}
						if (!(property14 != null))
						{
							break;
						}
						object value5 = property14.GetValue(val6);
						if (value5 == null)
						{
							break;
						}
						PropertyInfo property15 = value5.GetType().GetProperty("Count");
						PropertyInfo property16 = value5.GetType().GetProperty("Item");
						int num7 = ((property15 != null) ? ((int)property15.GetValue(value5)) : 0);
						for (int num8 = 0; num8 < num7; num8++)
						{
							WeaponType val8 = (WeaponType)property16.GetValue(value5, new object[1] { num8 });
							if (val8 != weaponType && !hashSet2.Contains(val8))
							{
								hashSet2.Add(val8);
								int num9 = (int)val8;
								PassiveRequirement item = default(PassiveRequirement);
								if (Enum.IsDefined(typeof(ItemType), num9))
								{
									ItemType val9 = (ItemType)num9;
									item.ItemType = val9;
									item.Sprite = GetSpriteForItem(val9);
									item.Owned = PlayerOwnsItem(val9);
								}
								else
								{
									item.WeaponType = val8;
									item.Sprite = GetSpriteForWeapon(val8);
									item.Owned = PlayerOwnsWeapon(val8);
								}
								item.RequiresMaxLevel = hashSet.Contains(val8);
								list.Add(item);
							}
						}
						break;
					}
				}
			}
		}
		yOffset -= Spacing;
		GameObject val10 = CreateTextElement(parent, "EvoHeader", "Evolutions: (click for details)", font, 14f, new Color(0.9f, 0.75f, 0.3f, 1f), (FontStyles)1);
		RectTransform component = val10.GetComponent<RectTransform>();
		component.anchorMin = new Vector2(0f, 1f);
		component.anchorMax = new Vector2(1f, 1f);
		component.pivot = new Vector2(0f, 1f);
		component.anchoredPosition = new Vector2(Padding, yOffset);
		component.sizeDelta = new Vector2(maxWidth - Padding * 2f, 20f);
		yOffset -= 22f;
		float num10 = 38f;
		bool flag4 = list.Exists((PassiveRequirement p) => p.RequiresMaxLevel);
		float num11 = num10 + 8f + (flag4 ? 12f : 0f);
		float num12 = Padding + 5f;
		for (int num13 = 0; num13 < list.Count; num13++)
		{
			PassiveRequirement passiveRequirement = list[num13];
			GameObject val11 = CreateTextElement(parent, $"Plus{num13}", "+", font, 18f, new Color(0.8f, 0.8f, 0.8f, 1f), (FontStyles)1);
			RectTransform component2 = val11.GetComponent<RectTransform>();
			component2.anchorMin = new Vector2(0f, 1f);
			component2.anchorMax = new Vector2(0f, 1f);
			component2.pivot = new Vector2(0f, 1f);
			component2.anchoredPosition = new Vector2(num12, yOffset - 8f);
			component2.sizeDelta = new Vector2(20f, num10);
			num12 += 22f;
			bool isBanned = (passiveRequirement.WeaponType.HasValue ? IsWeaponBanned(passiveRequirement.WeaponType.Value) : (passiveRequirement.ItemType.HasValue && IsItemBanned(passiveRequirement.ItemType.Value)));
			GameObject go = CreateFormulaIcon(parent, $"PassiveIcon{num13}", passiveRequirement.Sprite, passiveRequirement.Owned, isBanned, num10, num12, yOffset);
			if (passiveRequirement.WeaponType.HasValue)
			{
				AddHoverToGameObject(go, passiveRequirement.WeaponType.Value, null, useClick: true);
			}
			else if (passiveRequirement.ItemType.HasValue)
			{
				AddHoverToGameObject(go, null, passiveRequirement.ItemType.Value, useClick: true);
			}
			if (passiveRequirement.RequiresMaxLevel)
			{
				GameObject val12 = CreateTextElement(parent, $"Max{num13}", "MAX", font, 9f, new Color(1f, 0.85f, 0f, 1f), (FontStyles)1);
				RectTransform component3 = val12.GetComponent<RectTransform>();
				component3.anchorMin = new Vector2(0f, 1f);
				component3.anchorMax = new Vector2(0f, 1f);
				component3.pivot = new Vector2(0.5f, 1f);
				component3.anchoredPosition = new Vector2(num12 + num10 / 2f, yOffset - num10);
				component3.sizeDelta = new Vector2(num10, 12f);
				TextMeshProUGUI component4 = val12.GetComponent<TextMeshProUGUI>();
				if ((Object)(object)component4 != (Object)null)
				{
					((TMP_Text)component4).alignment = (TextAlignmentOptions)514;
				}
			}
			num12 += num10 + 4f;
		}
		GameObject val13 = CreateTextElement(parent, "Arrow", "→", font, 18f, new Color(0.8f, 0.8f, 0.8f, 1f), (FontStyles)0);
		RectTransform component5 = val13.GetComponent<RectTransform>();
		component5.anchorMin = new Vector2(0f, 1f);
		component5.anchorMax = new Vector2(0f, 1f);
		component5.pivot = new Vector2(0f, 1f);
		component5.anchoredPosition = new Vector2(num12, yOffset - 8f);
		component5.sizeDelta = new Vector2(24f, num10);
		num12 += 26f;
		bool isBanned2 = val5.HasValue && IsWeaponBanned(val5.Value);
		GameObject go2 = CreateFormulaIcon(parent, "EvoIcon", sprite, isOwned: false, isBanned2, num10, num12, yOffset);
		if (val5.HasValue)
		{
			AddHoverToGameObject(go2, val5.Value, null, useClick: true);
		}
		yOffset -= num11;
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
						MelonLogger.Msg("[AddEvolvedFromSection] Error: " + ex.Message);
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
						if (!flag || !Enum.TryParse<WeaponType>(propertyValue, out WeaponType result))
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
			MelonLogger.Warning("[AddPassiveEvo] Error: " + ex.Message);
		}
		try
		{
			WeaponData weaponData2 = GetWeaponData(passiveType);
			if (weaponData2 != null)
			{
				string propertyValue2 = GetPropertyValue<string>(weaponData2, "evoInto");
				if (!string.IsNullOrEmpty(propertyValue2) && ((object)weaponData2).GetType().GetProperty("evoSynergy")?.GetValue(weaponData2) is Il2CppStructArray<WeaponType> evoSynergy && Enum.TryParse<WeaponType>(propertyValue2, out WeaponType result2))
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
			MelonLogger.Warning("[AddPassiveEvo NewSystem] " + ex2.Message);
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
						if (string.IsNullOrEmpty(propertyValue2) || !Enum.TryParse<WeaponType>(propertyValue2, out WeaponType result))
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
			MelonLogger.Warning("[AddItemEvo] Error: " + ex.Message);
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
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		if (arcanas == null || arcanas.Count == 0)
		{
			return yOffset;
		}
		yOffset -= Spacing;
		GameObject val = CreateTextElement(parent, "ArcanaHeader", "Arcana: (click for details)", font, 14f, new Color(0.7f, 0.5f, 0.9f, 1f), (FontStyles)1);
		RectTransform component = val.GetComponent<RectTransform>();
		component.anchorMin = new Vector2(0f, 1f);
		component.anchorMax = new Vector2(1f, 1f);
		component.pivot = new Vector2(0f, 1f);
		component.anchoredPosition = new Vector2(Padding, yOffset);
		component.sizeDelta = new Vector2(maxWidth - Padding * 2f, 20f);
		yOffset -= 26f;
		float num = 52f;
		float padding = Padding;
		for (int i = 0; i < arcanas.Count; i++)
		{
			ArcanaInfo arcanaInfo = arcanas[i];
			GameObject go = CreateFormulaIcon(parent, $"ArcanaIcon{i}", arcanaInfo.Sprite, isOwned: false, isBanned: false, num, padding, yOffset);
			AddArcanaHoverToGameObject(go, arcanaInfo.ArcanaData);
			GameObject val2 = CreateTextElement(parent, $"ArcanaName{i}", arcanaInfo.Name, font, 13f, new Color(0.8f, 0.7f, 0.95f, 1f), (FontStyles)0);
			RectTransform component2 = val2.GetComponent<RectTransform>();
			component2.anchorMin = new Vector2(0f, 1f);
			component2.anchorMax = new Vector2(0f, 1f);
			component2.pivot = new Vector2(0f, 1f);
			component2.anchoredPosition = new Vector2(padding + num + 8f, yOffset - (num / 2f - 8f));
			component2.sizeDelta = new Vector2(maxWidth - padding - num - Padding - 8f, 20f);
			yOffset -= num + 8f;
		}
		return yOffset;
	}

	private static void ShowArcanaPopup(Transform anchor, object arcanaData)
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
			GameObject val2 = CreateArcanaPopup(val, arcanaData);
			if (!((Object)(object)val2 == (Object)null))
			{
				popupStack.Add(val2);
				popupAnchorIds.Add(num);
				PositionPopup(val2, anchor);
				AddPopupHoverTracking(val2);
			}
		}
	}

	private static GameObject CreateArcanaPopup(Transform parent, object arcanaData)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0920: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Expected O, but got Unknown
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Expected O, but got Unknown
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Expected O, but got Unknown
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Expected O, but got Unknown
		//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_051f: Unknown result type (might be due to invalid IL or missing references)
		//IL_056a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0680: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0704: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0769: Unknown result type (might be due to invalid IL or missing references)
		//IL_076e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0782: Unknown result type (might be due to invalid IL or missing references)
		//IL_078b: Unknown result type (might be due to invalid IL or missing references)
		//IL_07af: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0836: Unknown result type (might be due to invalid IL or missing references)
		//IL_083b: Unknown result type (might be due to invalid IL or missing references)
		//IL_084f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0858: Unknown result type (might be due to invalid IL or missing references)
		//IL_087c: Unknown result type (might be due to invalid IL or missing references)
		//IL_089c: Unknown result type (might be due to invalid IL or missing references)
		if (arcanaData == null)
		{
			return null;
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
		PropertyInfo property = arcanaData.GetType().GetProperty("name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		PropertyInfo property2 = arcanaData.GetType().GetProperty("description", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		PropertyInfo property3 = arcanaData.GetType().GetProperty("frameName", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		PropertyInfo property4 = arcanaData.GetType().GetProperty("texture", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		string text = property?.GetValue(arcanaData)?.ToString() ?? "Unknown Arcana";
		string text2 = property2?.GetValue(arcanaData)?.ToString() ?? "";
		string frameName = property3?.GetValue(arcanaData)?.ToString() ?? "";
		string textureName = property4?.GetValue(arcanaData)?.ToString() ?? "";
		Sprite val5 = LoadArcanaSprite(textureName, frameName);
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
				val17.horizontalFit = (FitMode)0;
				val17.verticalFit = (FitMode)2;
				((TMP_Text)val16).ForceMeshUpdate(false, false);
				LayoutRebuilder.ForceRebuildLayoutImmediate(val15);
				float num4 = ((((TMP_Text)val16).preferredHeight > 0f) ? ((TMP_Text)val16).preferredHeight : 40f);
				val15.sizeDelta = new Vector2(num3, num4);
				num -= num4 + Spacing;
			}
			List<WeaponType> allArcanaAffectedWeaponTypes = GetAllArcanaAffectedWeaponTypes(arcanaData);
			List<ItemType> allArcanaAffectedItemTypes = GetAllArcanaAffectedItemTypes(arcanaData);
			int num5 = allArcanaAffectedWeaponTypes.Count + allArcanaAffectedItemTypes.Count;
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
					Sprite spriteForWeapon = GetSpriteForWeapon(item);
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

	private static void AddArcanaHoverToGameObject(GameObject go, object arcanaData)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		EventTrigger component = go.GetComponent<EventTrigger>();
		if (!((Object)(object)component != (Object)null))
		{
			EventTrigger val = go.AddComponent<EventTrigger>();
			object capturedData = arcanaData;
			Entry val2 = new Entry();
			val2.eventID = (EventTriggerType)4;
			((UnityEvent<BaseEventData>)(object)val2.callback).AddListener(UnityAction<BaseEventData>.op_Implicit((Action<BaseEventData>)delegate
			{
				ShowArcanaPopup(go.transform, capturedData);
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
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		if (cachedWeaponsDict == null)
		{
			return null;
		}
		try
		{
			PropertyInfo property = cachedWeaponsDict.GetType().GetProperty("Item");
			if (property != null)
			{
				return property.GetValue(cachedWeaponsDict, new object[1] { type }) as List<WeaponData>;
			}
		}
		catch
		{
		}
		return null;
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
			Vector2 center = ((Rect)(ref rect)).center;
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
			float width = ((Rect)(ref rect)).width;
			rect = component2.rect;
			float height = ((Rect)(ref rect)).height;
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
		Entry val2 = new Entry();
		val2.eventID = (EventTriggerType)0;
		((UnityEvent<BaseEventData>)(object)val2.callback).AddListener(UnityAction<BaseEventData>.op_Implicit((Action<BaseEventData>)delegate
		{
			mouseOverPopupIndex = thisPopupIndex;
		}));
		val.triggers.Add(val2);
		Entry val3 = new Entry();
		val3.eventID = (EventTriggerType)1;
		((UnityEvent<BaseEventData>)(object)val3.callback).AddListener(UnityAction<BaseEventData>.op_Implicit((Action<BaseEventData>)delegate
		{
			if (mouseOverPopupIndex == thisPopupIndex)
			{
				mouseOverPopupIndex = -1;
			}
			MelonCoroutines.Start(DelayedStackHideCheck(thisPopupIndex));
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
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
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
		int num = -1;
		WeaponType? weaponType = null;
		ItemType? itemType = null;
		object obj = null;
		foreach (KeyValuePair<int, (GameObject, WeaponType?, ItemType?, object)> collectionIcon2 in collectionIcons)
		{
			GameObject item = collectionIcon2.Value.Item1;
			if (!((Object)(object)item == (Object)null) && item.activeInHierarchy)
			{
				RectTransform component = item.GetComponent<RectTransform>();
				if (!((Object)(object)component == (Object)null) && (RectTransformUtility.RectangleContainsScreenPoint(component, Vector2.op_Implicit(mousePosition), (Camera)null) || RectTransformUtility.RectangleContainsScreenPoint(component, Vector2.op_Implicit(mousePosition), Camera.main)))
				{
					num = collectionIcon2.Key;
					weaponType = collectionIcon2.Value.Item2;
					itemType = collectionIcon2.Value.Item3;
					obj = collectionIcon2.Value.Item4;
					break;
				}
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
			object arcanaData = GetArcanaData(arcanaType);
			if (arcanaData == null)
			{
				MelonLogger.Warning($"[CollectionPopup] Could not get arcana data for {arcanaType}");
				return;
			}
			collectionPopup = CreateArcanaPopup(val, arcanaData);
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
			Button val2 = go.AddComponent<Button>();
			Image component2 = go.GetComponent<Image>();
			if ((Object)(object)component2 != (Object)null)
			{
				((Selectable)val2).targetGraphic = (Graphic)(object)component2;
			}
			ColorBlock colors = ((Selectable)val2).colors;
			((ColorBlock)(ref colors)).normalColor = new Color(0f, 0f, 0f, 0f);
			((ColorBlock)(ref colors)).highlightedColor = new Color(0f, 0.9f, 1f, 0.2f);
			((ColorBlock)(ref colors)).selectedColor = new Color(0f, 0.9f, 1f, 0.35f);
			((ColorBlock)(ref colors)).pressedColor = new Color(0f, 0.9f, 1f, 0.5f);
			((ColorBlock)(ref colors)).fadeDuration = 0.1f;
			((Selectable)val2).colors = colors;
			((UnityEvent)val2.onClick).AddListener(UnityAction.op_Implicit((Action)delegate
			{
				ShowItemPopup(go.transform, weaponType, itemType);
			}));
			Navigation navigation = ((Selectable)val2).navigation;
			navigation.mode = (Mode)0;
			((Selectable)val2).navigation = navigation;
		}
		else
		{
			Entry val3 = new Entry();
			val3.eventID = (EventTriggerType)0;
			((UnityEvent<BaseEventData>)(object)val3.callback).AddListener(UnityAction<BaseEventData>.op_Implicit((Action<BaseEventData>)delegate
			{
				ShowItemPopup(go.transform, weaponType, itemType);
			}));
			val.triggers.Add(val3);
			Entry val4 = new Entry();
			val4.eventID = (EventTriggerType)1;
			((UnityEvent<BaseEventData>)(object)val4.callback).AddListener(UnityAction<BaseEventData>.op_Implicit((Action<BaseEventData>)delegate
			{
				MelonCoroutines.Start(DelayedHideCheck());
			}));
			val.triggers.Add(val4);
		}
	}

	private static WeaponData GetWeaponData(WeaponType type)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		if (cachedWeaponsDict == null)
		{
			return null;
		}
		try
		{
			Type type2 = cachedWeaponsDict.GetType();
			MethodInfo method = type2.GetMethod("ContainsKey");
			if (method != null && (bool)method.Invoke(cachedWeaponsDict, new object[1] { type }))
			{
				PropertyInfo property = type2.GetProperty("Item");
				if (property != null && property.GetValue(cachedWeaponsDict, new object[1] { type }) is List<WeaponData> val && val.Count > 0)
				{
					return val[0];
				}
			}
		}
		catch
		{
		}
		return null;
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
			MelonLogger.Warning("Error creating circle sprite: " + ex.Message);
			return null;
		}
	}

	private static Sprite LoadSpriteFromAtlas(string frameName, string atlasName)
	{
		try
		{
			if (spriteManagerType == null)
			{
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				Assembly[] array = assemblies;
				foreach (Assembly assembly in array)
				{
					spriteManagerType = assembly.GetTypes().FirstOrDefault((Type t) => t.Name == "SpriteManager");
					if (spriteManagerType != null)
					{
						if (!spriteManagerDebugLogged)
						{
							spriteManagerDebugLogged = true;
						}
						break;
					}
				}
				if (spriteManagerType == null && !spriteManagerDebugLogged)
				{
					MelonLogger.Warning("[LoadSpriteFromAtlas] SpriteManager type not found!");
					spriteManagerDebugLogged = true;
				}
			}
			if (spriteManagerType == null)
			{
				return null;
			}
			MethodInfo method = spriteManagerType.GetMethod("GetSpriteFast", BindingFlags.Static | BindingFlags.Public, null, new Type[2]
			{
				typeof(string),
				typeof(string)
			}, null);
			if (method != null)
			{
				object? obj = method.Invoke(null, new object[2] { frameName, atlasName });
				Sprite val = (Sprite)((obj is Sprite) ? obj : null);
				if ((Object)(object)val != (Object)null)
				{
					return val;
				}
				if (frameName.Contains("."))
				{
					string text = frameName.Substring(0, frameName.LastIndexOf('.'));
					object? obj2 = method.Invoke(null, new object[2] { text, atlasName });
					val = (Sprite)((obj2 is Sprite) ? obj2 : null);
				}
				return val;
			}
		}
		catch
		{
		}
		return null;
	}

	private static Sprite GetSpriteForWeapon(WeaponType weaponType)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		WeaponData weaponData = GetWeaponData(weaponType);
		if (weaponData == null)
		{
			if (!spriteLoadDebugLogged)
			{
			}
			return null;
		}
		try
		{
			string frameName = weaponData.frameName;
			string propertyValue = GetPropertyValue<string>(weaponData, "texture");
			if (!spriteLoadDebugLogged)
			{
				spriteLoadDebugLogged = true;
			}
			if (!string.IsNullOrEmpty(frameName) && !string.IsNullOrEmpty(propertyValue))
			{
				return LoadSpriteFromAtlas(frameName, propertyValue);
			}
			if (!string.IsNullOrEmpty(frameName))
			{
				string[] array = new string[4] { "weapons", "items", "characters", "ui" };
				string[] array2 = array;
				string[] array3 = array2;
				foreach (string atlasName in array3)
				{
					Sprite val = LoadSpriteFromAtlas(frameName, atlasName);
					if ((Object)(object)val != (Object)null)
					{
						return val;
					}
				}
			}
		}
		catch (Exception ex)
		{
			MelonLogger.Warning("[GetSpriteForWeapon] Error: " + ex.Message);
		}
		return null;
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
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		if (data == null)
		{
			return "";
		}
		try
		{
			MethodInfo method = ((object)data).GetType().GetMethod("GetLocalizedDescriptionTerm", BindingFlags.Instance | BindingFlags.Public);
			if (method != null)
			{
				string text = method.Invoke(data, new object[1] { type }) as string;
				if (!string.IsNullOrEmpty(text))
				{
					string i2Translation = GetI2Translation(text);
					if (!string.IsNullOrEmpty(i2Translation))
					{
						return i2Translation;
					}
				}
			}
		}
		catch
		{
		}
		if (!string.IsNullOrEmpty(data.description))
		{
			return data.description;
		}
		return "";
	}

	private unsafe static string GetLocalizedWeaponName(WeaponData data, WeaponType type)
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		if (data == null)
		{
			return ((object)(*(WeaponType*)(&type))/*cast due to constrained. prefix*/).ToString();
		}
		try
		{
			MethodInfo method = ((object)data).GetType().GetMethod("GetLocalizedNameTerm", BindingFlags.Instance | BindingFlags.Public);
			if (method != null)
			{
				string text = method.Invoke(data, new object[1] { type }) as string;
				if (!string.IsNullOrEmpty(text))
				{
					string i2Translation = GetI2Translation(text);
					if (!string.IsNullOrEmpty(i2Translation))
					{
						return i2Translation;
					}
				}
			}
		}
		catch
		{
		}
		return data.name ?? ((object)(*(WeaponType*)(&type))/*cast due to constrained. prefix*/).ToString();
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
		if (string.IsNullOrEmpty(term))
		{
			return null;
		}
		try
		{
			Type type = Type.GetType("Il2CppI2.Loc.LocalizationManager, Il2Cppl2localization");
			if (type != null)
			{
				MethodInfo method = type.GetMethod("GetTranslation", BindingFlags.Static | BindingFlags.Public);
				if (method != null)
				{
					return method.Invoke(null, new object[8] { term, false, 0, false, true, null, null, false }) as string;
				}
			}
		}
		catch
		{
		}
		return null;
	}

	private static TMP_FontAsset GetFont()
	{
		TextMeshProUGUI obj = Object.FindObjectOfType<TextMeshProUGUI>();
		return (obj != null) ? ((TMP_Text)obj).font : null;
	}

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
			MelonLogger.Warning("[Arcana] Error getting active arcanas: " + ex.Message);
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
					if (!string.IsNullOrEmpty(text) && hashSet.Add(text) && Enum.TryParse<WeaponType>(text, out WeaponType result))
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
		MelonLogger.Warning($"[Arcana] GetArcanaTypeInt failed for type {arcanaType.GetType().FullName}, value: {arcanaType}");
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
			MelonLogger.Warning("[Arcana] Error scanning arcana UI: " + ex.Message);
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
			MelonLogger.Warning("[Arcana] Error capturing weapon from patch: " + ex.Message);
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
			MelonLogger.Warning("[Arcana] Error capturing item from patch: " + ex.Message);
		}
	}

	private static List<ArcanaInfo> GetActiveArcanasForWeapon(WeaponType weaponType)
	{
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		List<ArcanaInfo> list = new List<ArcanaInfo>();
		try
		{
			List<object> allActiveArcanaTypes = GetAllActiveArcanaTypes();
			foreach (object item in allActiveArcanaTypes)
			{
				object arcanaData = GetArcanaData(item);
				if (arcanaData == null)
				{
					continue;
				}
				int arcanaTypeInt = GetArcanaTypeInt(item);
				string text = arcanaData.GetType().GetProperty("name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.GetValue(arcanaData)?.ToString() ?? "?";
				if (arcanaTypeInt >= 0 && !string.IsNullOrEmpty(text))
				{
					arcanaNameToInt[text] = arcanaTypeInt;
				}
				bool flag = IsWeaponAffectedByArcana(weaponType, arcanaData);
				bool flag2 = !flag && panelCapturedWeapons.Contains(weaponType);
				bool flag3 = false;
				if (!flag && !flag2 && arcanaTypeInt >= 0)
				{
					if (!arcanaUICache.ContainsKey(arcanaTypeInt))
					{
						ScanArcanaUI(arcanaTypeInt, text);
					}
					if (arcanaUICache.TryGetValue(arcanaTypeInt, out (HashSet<WeaponType>, HashSet<ItemType>) value))
					{
						flag3 = value.Item1.Contains(weaponType);
					}
				}
				if (flag || flag2 || flag3)
				{
					PropertyInfo property = arcanaData.GetType().GetProperty("name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					PropertyInfo property2 = arcanaData.GetType().GetProperty("description", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					PropertyInfo property3 = arcanaData.GetType().GetProperty("frameName", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					PropertyInfo property4 = arcanaData.GetType().GetProperty("texture", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					string name = property?.GetValue(arcanaData)?.ToString() ?? "";
					string description = property2?.GetValue(arcanaData)?.ToString() ?? "";
					string frameName = property3?.GetValue(arcanaData)?.ToString() ?? "";
					string textureName = property4?.GetValue(arcanaData)?.ToString() ?? "";
					Sprite sprite = LoadArcanaSprite(textureName, frameName);
					list.Add(new ArcanaInfo
					{
						Name = name,
						Description = description,
						Sprite = sprite,
						ArcanaData = arcanaData,
						ArcanaType = item
					});
				}
			}
		}
		catch (Exception ex)
		{
			MelonLogger.Warning($"[Arcana] Error getting arcanas for weapon {weaponType}: {ex.Message}");
		}
		return list;
	}

	private static List<ArcanaInfo> GetActiveArcanasForItem(ItemType itemType)
	{
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		List<ArcanaInfo> list = new List<ArcanaInfo>();
		try
		{
			List<object> allActiveArcanaTypes = GetAllActiveArcanaTypes();
			foreach (object item in allActiveArcanaTypes)
			{
				object arcanaData = GetArcanaData(item);
				if (arcanaData == null)
				{
					continue;
				}
				int arcanaTypeInt = GetArcanaTypeInt(item);
				string text = arcanaData.GetType().GetProperty("name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.GetValue(arcanaData)?.ToString() ?? "?";
				if (arcanaTypeInt >= 0 && !string.IsNullOrEmpty(text))
				{
					arcanaNameToInt[text] = arcanaTypeInt;
				}
				bool flag = IsItemAffectedByArcana(itemType, arcanaData);
				bool flag2 = !flag && panelCapturedItems.Contains(itemType);
				bool flag3 = false;
				if (!flag && !flag2 && arcanaTypeInt >= 0)
				{
					if (!arcanaUICache.ContainsKey(arcanaTypeInt))
					{
						ScanArcanaUI(arcanaTypeInt, text);
					}
					if (arcanaUICache.TryGetValue(arcanaTypeInt, out (HashSet<WeaponType>, HashSet<ItemType>) value))
					{
						flag3 = value.Item2.Contains(itemType);
					}
				}
				if (flag || flag2 || flag3)
				{
					PropertyInfo property = arcanaData.GetType().GetProperty("name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					PropertyInfo property2 = arcanaData.GetType().GetProperty("description", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					PropertyInfo property3 = arcanaData.GetType().GetProperty("frameName", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					PropertyInfo property4 = arcanaData.GetType().GetProperty("texture", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					string name = property?.GetValue(arcanaData)?.ToString() ?? "";
					string description = property2?.GetValue(arcanaData)?.ToString() ?? "";
					string frameName = property3?.GetValue(arcanaData)?.ToString() ?? "";
					string textureName = property4?.GetValue(arcanaData)?.ToString() ?? "";
					Sprite sprite = LoadArcanaSprite(textureName, frameName);
					list.Add(new ArcanaInfo
					{
						Name = name,
						Description = description,
						Sprite = sprite,
						ArcanaData = arcanaData,
						ArcanaType = item
					});
				}
			}
		}
		catch (Exception ex)
		{
			MelonLogger.Warning($"[Arcana] Error getting arcanas for item {itemType}: {ex.Message}");
		}
		return list;
	}
}
