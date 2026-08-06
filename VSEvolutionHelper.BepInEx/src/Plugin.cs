using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using Il2CppInterop.Runtime.Injection;
using UnityEngine;

namespace VSItemTooltips;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BasePlugin
{
    public const string PluginGuid = "com.nihil.vsevolutionhelper";
    public const string PluginName = "VS Evolution Helper";
    public const string PluginVersion = "1.13.0";

    internal static new ManualLogSource Log;
    internal static Plugin Instance;
    private static PluginBehaviour _behaviour;

    // ── Config (public for other types) ─────────────────────────────────
    internal static bool DebugVerbose;
    internal static float TooltipHoverDelay;
    internal static float LevelUpHoverDelay;
    internal static float ControllerDwellDelay;
    internal static bool MapTooltipsEnabled;
    internal static bool StageGuideEnabled;
    internal static bool StageGuideDefaultToGuide;
    internal static bool LevelUpTooltipsEnabled;
    internal static bool CharacterTooltipsEnabled;
    internal static bool AdventureTooltipsEnabled;
    internal static bool WeaponSelectionTooltipsEnabled;
    internal static bool SecretTooltipsEnabled;
    internal static bool SecretSpoilers;
    internal static bool BestiaryTooltipsEnabled;
    internal static bool BestiarySpoilers;
    internal static bool AchievementTooltipsEnabled;
    internal static bool PowerUpTooltipsEnabled;

    private ConfigEntry<bool> _debugVerbose;
    private ConfigEntry<float> _tooltipHoverDelay;
    private ConfigEntry<float> _levelUpHoverDelay;
    private ConfigEntry<float> _controllerDwellDelay;
    private ConfigEntry<bool> _mapTooltipsEnabled;
    private ConfigEntry<bool> _stageGuideEnabled;
    private ConfigEntry<bool> _stageGuideDefaultToGuide;
    private ConfigEntry<bool> _levelUpTooltipsEnabled;
    private ConfigEntry<bool> _characterTooltipsEnabled;
    private ConfigEntry<bool> _adventureTooltipsEnabled;
    private ConfigEntry<bool> _weaponSelectionTooltipsEnabled;
    private ConfigEntry<bool> _secretTooltipsEnabled;
    private ConfigEntry<bool> _secretSpoilers;
    private ConfigEntry<bool> _bestiaryTooltipsEnabled;
    private ConfigEntry<bool> _bestiarySpoilers;
    private ConfigEntry<bool> _achievementTooltipsEnabled;
    private ConfigEntry<bool> _powerUpTooltipsEnabled;

    public override void Load()
    {
        Instance = this;
        Log = base.Log;

        _debugVerbose = Config.Bind(
            "Debug",
            "VerboseLogging",
            false,
            "Log detailed evolution / sprite / hover diagnostics to the BepInEx console.");

        _tooltipHoverDelay = Config.Bind(
            "Tooltips",
            "HoverDelay",
            0.4f,
            "Seconds to hover collection / map / stage-relic icons before showing a tooltip (0-2).");

        _levelUpHoverDelay = Config.Bind(
            "Tooltips",
            "LevelUpHoverDelay",
            0.15f,
            "Seconds to hold over a Level Up icon after moving the mouse (0-1).");

        _controllerDwellDelay = Config.Bind(
            "Tooltips",
            "ControllerDwellDelay",
            0.5f,
            "Seconds of controller focus dwell before showing a tooltip (0-2).");

        _mapTooltipsEnabled = Config.Bind(
            "Features",
            "MapTooltips",
            true,
            "Show tooltips when hovering relics/pickups on the pause map.");

        _stageGuideEnabled = Config.Bind(
            "Features",
            "StageGuide",
            true,
            "Show Music|Guide tabs on Stage Selection (Guide panel with tips/unlocks).");

        _stageGuideDefaultToGuide = Config.Bind(
            "Features",
            "StageGuideDefaultToGuide",
            false,
            "If true, Stage Selection opens on the Guide tab instead of Music.");

        _levelUpTooltipsEnabled = Config.Bind(
            "Features",
            "LevelUpTooltips",
            true,
            "Show evolution tooltips when hovering Level Up choice icons.");

        _characterTooltipsEnabled = Config.Bind(
            "Features",
            "CharacterTooltips",
            true,
            "Show starter weapon / evolution tooltips when hovering characters on Character Selection.");

        _adventureTooltipsEnabled = Config.Bind(
            "Features",
            "AdventureTooltips",
            true,
            "Show cast/weapon summary tooltips when hovering adventures on the Adventures select screen.");

        _weaponSelectionTooltipsEnabled = Config.Bind(
            "Features",
            "WeaponSelectionTooltips",
            true,
            "Show weapon tooltips on weapon selector screens (Arma Dio, Penshin Fatcha).");

        _secretTooltipsEnabled = Config.Bind(
            "Features",
            "SecretTooltips",
            true,
            "Show what each secret unlocks on the Secrets page.");

        _secretSpoilers = Config.Bind(
            "Features",
            "SecretSpoilers",
            true,
            "Also reveal rewards for secrets you have NOT discovered yet. Set false to only show secrets you have already unlocked.");

        _bestiaryTooltipsEnabled = Config.Bind(
            "Features",
            "BestiaryTooltips",
            true,
            "Show enemy stats and resistances when hovering rows on the Bestiary page.");

        _bestiarySpoilers = Config.Bind(
            "Features",
            "BestiarySpoilers",
            true,
            "Also show stats for enemies you have NOT killed yet. Set false to only show enemies you have already encountered.");

        _achievementTooltipsEnabled = Config.Bind(
            "Features",
            "AchievementTooltips",
            true,
            "Show what each achievement unlocks when hovering rows on the Unlocks page.");

        _powerUpTooltipsEnabled = Config.Bind(
            "Features",
            "PowerUpTooltips",
            true,
            "Show the full price ladder and remaining cost when hovering upgrades on the Power Up page.");

        ApplyConfigValues();

        Log.LogInfo($"{PluginName} {PluginVersion} loading (BepInEx port)...");
        Log.LogInfo($"Debug.VerboseLogging={DebugVerbose} Tooltips.HoverDelay={TooltipHoverDelay:0.##}s LevelUpHoverDelay={LevelUpHoverDelay:0.##}s");
        Log.LogInfo($"Features: Map={MapTooltipsEnabled} StageGuide={StageGuideEnabled} LevelUpTooltips={LevelUpTooltipsEnabled} Character={CharacterTooltipsEnabled} Adventure={AdventureTooltipsEnabled} WeaponSelect={WeaponSelectionTooltipsEnabled} Secrets={SecretTooltipsEnabled} Bestiary={BestiaryTooltipsEnabled} Achievements={AchievementTooltipsEnabled} PowerUps={PowerUpTooltipsEnabled}");

        ClassInjector.RegisterTypeInIl2Cpp<PluginBehaviour>();
        _behaviour = AddComponent<PluginBehaviour>();

        ItemTooltipsMod.Initialize();
        Log.LogInfo($"{PluginName} initialized.");
    }

    private void ApplyConfigValues()
    {
        DebugVerbose = _debugVerbose.Value;
        TooltipHoverDelay = Mathf.Clamp(_tooltipHoverDelay.Value, 0f, 2f);
        LevelUpHoverDelay = Mathf.Clamp(_levelUpHoverDelay.Value, 0f, 1f);
        ControllerDwellDelay = Mathf.Clamp(_controllerDwellDelay.Value, 0f, 2f);
        MapTooltipsEnabled = _mapTooltipsEnabled.Value;
        StageGuideEnabled = _stageGuideEnabled.Value;
        StageGuideDefaultToGuide = _stageGuideDefaultToGuide.Value;
        LevelUpTooltipsEnabled = _levelUpTooltipsEnabled.Value;
        CharacterTooltipsEnabled = _characterTooltipsEnabled.Value;
        AdventureTooltipsEnabled = _adventureTooltipsEnabled.Value;
        WeaponSelectionTooltipsEnabled = _weaponSelectionTooltipsEnabled.Value;
        SecretTooltipsEnabled = _secretTooltipsEnabled.Value;
        SecretSpoilers = _secretSpoilers.Value;
        BestiaryTooltipsEnabled = _bestiaryTooltipsEnabled.Value;
        BestiarySpoilers = _bestiarySpoilers.Value;
        AchievementTooltipsEnabled = _achievementTooltipsEnabled.Value;
        PowerUpTooltipsEnabled = _powerUpTooltipsEnabled.Value;
    }

    internal static void Dbg(string message)
    {
        if (DebugVerbose)
        {
            Log.LogInfo("[DBG] " + message);
        }
    }
}

/// <summary>
/// Unity-side host for Update (Il2Cpp-injected MonoBehaviour).
/// </summary>
public class PluginBehaviour : MonoBehaviour
{
    public PluginBehaviour(System.IntPtr ptr) : base(ptr) { }

    private void Update()
    {
        ItemTooltipsMod.Update();
    }

    private void OnEnable()
    {
    }
}
