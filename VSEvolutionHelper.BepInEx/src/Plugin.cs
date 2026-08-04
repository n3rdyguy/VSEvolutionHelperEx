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
    public const string PluginVersion = "1.6.3";

    internal static new ManualLogSource Log;
    internal static Plugin Instance;
    private static PluginBehaviour _behaviour;

    /// <summary>Verbose tooltip/evo/sprite logging. Toggle in BepInEx/config/com.nihil.vsevolutionhelper.cfg</summary>
    internal static bool DebugVerbose;

    private ConfigEntry<bool> _debugVerbose;

    public override void Load()
    {
        Instance = this;
        Log = base.Log;

        _debugVerbose = Config.Bind(
            "Debug",
            "VerboseLogging",
            true,
            "Log detailed evolution / sprite / hover diagnostics to the BepInEx console.");
        DebugVerbose = _debugVerbose.Value;

        Log.LogInfo($"{PluginName} {PluginVersion} loading (BepInEx port)...");
        Log.LogInfo($"Debug.VerboseLogging = {DebugVerbose} (edit config to change)");

        ClassInjector.RegisterTypeInIl2Cpp<PluginBehaviour>();
        _behaviour = AddComponent<PluginBehaviour>();

        ItemTooltipsMod.Initialize();
        Log.LogInfo($"{PluginName} initialized.");
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
        // Track scene changes via Unity lifecycle if needed
    }
}
