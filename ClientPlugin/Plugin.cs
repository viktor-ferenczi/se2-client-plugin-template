using HarmonyLib;
using Keen.Game2.Game.Plugins;
using Keen.VRage.Library.Diagnostics;
using System.Reflection;

namespace ClientPlugin;

public class Plugin : IPlugin
{
    public const string Name = "ClientPluginTemplate";
    public static Plugin Instance;
    
    // The data directory will be provided by a proper SDK in the future.
    // This static function is currently injected by Pulsar, which will
    // remain compatible, even after the SDK's release.
    private static Func<string, string, string> GetConfigPath;
    public string DataDir { get; private set; } = GetConfigPath(Name, null);

    public Config Config;
    
    public Plugin()
    {
        Instance = this;
        
        Config = ConfigStorage.Load();
        
        Log.Default.WriteLine($"[{Name}] Loaded plugin.");
#if DEBUG
        Harmony.DEBUG = true;
#endif
        Harmony harmony = new Harmony(Name);
        harmony.PatchAll(Assembly.GetExecutingAssembly());
        Log.Default.WriteLine($"[{Name}] Applied patches");
    }
}