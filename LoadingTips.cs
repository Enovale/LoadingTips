using System.Runtime.CompilerServices;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace LoadingTips;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency("BMX.LobbyCompatibility", BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency("ainavt.lc.lethalconfig", BepInDependency.DependencyFlags.SoftDependency)]
public class LoadingTips : BaseUnityPlugin
{
    public static LoadingTips Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger { get; private set; } = null!;
    internal static Harmony? Harmony { get; set; }
    
    internal new static LoadingTipsConfig Config { get; private set; } = null!;

    private void Awake()
    {
        Logger = base.Logger;
        Instance = this;
        Config = new LoadingTipsConfig(base.Config);
        
        if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("ainavt.lc.lethalconfig"))
            Config.InitializeLethalConfig();

        if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("BMX.LobbyCompatibility"))
            InitializeLobbyCompatibility();

        Patch();

        Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
    }

    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    private static void InitializeLobbyCompatibility() => LobbyCompatibility.Features.PluginHelper.RegisterPlugin(
        MyPluginInfo.PLUGIN_GUID, new(MyPluginInfo.PLUGIN_VERSION),
        LobbyCompatibility.Enums.CompatibilityLevel.ClientOnly, LobbyCompatibility.Enums.VersionStrictness.Major);

    internal static void Patch()
    {
        Harmony ??= new Harmony(MyPluginInfo.PLUGIN_GUID);

        Logger.LogDebug("Patching...");

        Harmony.PatchAll();

        Logger.LogDebug("Finished patching!");
    }

    internal static void Unpatch()
    {
        Logger.LogDebug("Unpatching...");

        Harmony?.UnpatchSelf();

        Logger.LogDebug("Finished unpatching!");
    }
}