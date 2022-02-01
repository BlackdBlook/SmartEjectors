using BepInEx;
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

        public string Version { get { return PluginInfo.PLUGIN_VERSION; } }

        public bool CheckVersion(string hostVersion, string clientVersion)
        {
            return hostVersion.Equals(clientVersion);
        }

        public void Export(BinaryWriter w)
        {
            w.Write(SmartEjectors.Config.enableLockEjector.local);
        }

        public void Import(BinaryReader r)
        {
            SmartEjectors.Config.enableLockEjector.inUse = r.ReadBoolean();
        }

        private void Awake()
        {
            SmartEjectors.Config.Init(Path.Combine(Paths.ConfigPath, PluginInfo.PLUGIN_NAME + ".cfg"));

            harmony.PatchAll(typeof(Patch.LockEjectors));
        }
    }
}
