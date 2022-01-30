using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System.IO;

namespace SmartEjectors
{
    [BepInPlugin(GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInProcess("DSPGAME.exe")]
    public class Plugin : BaseUnityPlugin
    {
        private const string GUID = "com.daniel-egg." + PluginInfo.PLUGIN_NAME;

        private static ConfigFile configFile;
        private static ConfigEntry<bool> enableLockEjector;

        private void Awake()
        {
            configFile = new ConfigFile(Path.Combine(Paths.ConfigPath, PluginInfo.PLUGIN_NAME + ".cfg"), true);
            enableLockEjector = configFile.Bind("General", "enableLockEjector", true, "When set to true, EM Rail Ejectors automatically stop firing when the local Dyson Sphere has no available cell points.");
            
            Harmony.CreateAndPatchAll(typeof(Patch));
        }

        static class Patch
        {
            [HarmonyPostfix, HarmonyPatch(typeof(EjectorComponent), "InternalUpdate")]
            private static void LockEjector(ref EjectorComponent __instance, DysonSwarm swarm, ref AnimData[] animPool)
            {
                if (!enableLockEjector.Value) return;
                
                DysonSphere sphere = swarm.dysonSphere;

                // Check for filled sphere
                if (sphere.totalConstructedCellPoint + sphere.swarm.sailCount >= sphere.totalCellPoint)
                {
                    // Disable firing
                    __instance.time = 0;

                    // Disable animations
                    animPool[__instance.entityId].time = 0f;

                    // Reduce power consumption to idle state
                    __instance.direction = 0;
                }
            }
        }
    }
}
