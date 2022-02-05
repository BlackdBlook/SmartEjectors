using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using NebulaAPI;
using System.IO;

namespace SmartEjectors
{
    [BepInPlugin(GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInProcess("DSPGAME.exe")]
    [BepInDependency(NebulaModAPI.API_GUID)]
    public class Plugin : BaseUnityPlugin, IMultiplayerModWithSettings
    {
        private const string GUID = "com.daniel-egg." + PluginInfo.PLUGIN_NAME;
        private Harmony harmony = new Harmony(GUID);

        public static new ManualLogSource Logger;

        public string Version { get { return PluginInfo.PLUGIN_VERSION; } }

        public bool CheckVersion(string hostVersion, string clientVersion)
        {
            return hostVersion.Equals(clientVersion);
        }

        public void Export(BinaryWriter w)
        {
            SmartEjectors.Config.Export(w);
        }

        public void Import(BinaryReader r)
        {
            SmartEjectors.Config.Import(r);
        }

        private void Awake()
        {
            Plugin.Logger = base.Logger;

            SmartEjectors.Config.Init(Path.Combine(Paths.ConfigPath, PluginInfo.PLUGIN_NAME + ".cfg"));

            harmony.PatchAll(typeof(Patch.LockEjectors));
            harmony.PatchAll(typeof(Patch.DefaultOrbit));
        }
    }
}
