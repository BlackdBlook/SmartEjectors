using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System.IO;

namespace SmartEjectors
{
    [BepInPlugin(GUID, NAME, VERSION)]
    [BepInProcess("DSPGAME.exe")]
    public class Plugin : BaseUnityPlugin
    {
        private const string GUID = "com.daniel-egg.smartejectors";
        private const string NAME = "SmartEjectors";
        private const string VERSION = "1.0.0";

        private static ConfigFile configFile;
        private static ConfigEntry<bool> enableLockEjector;

        private void Awake()
        {
            configFile = new ConfigFile(Path.Combine(Paths.ConfigPath, NAME + ".cfg"), true);
            enableLockEjector = configFile.Bind("General", "enableLockEjector", true, "When set to true, EM Rail Ejectors automatically stop firing when the local Dyson Sphere has no available cell points.");
            
            Harmony.CreateAndPatchAll(typeof(Patch));
        }

        static class Patch
        {
            [HarmonyPrefix, HarmonyPatch(typeof(EjectorComponent), "InternalUpdate")]
            private static void LockEjector(ref EjectorComponent __instance, DysonSwarm swarm, ref AnimData[] animPool)
            {
                if (enableLockEjector.Value)
                {
                    DysonSphere sphere = swarm.dysonSphere;

                    // Check for filled sphere
                    if (sphere.totalConstructedCellPoint + sphere.swarm.sailCount >= sphere.totalCellPoint)
                    {
                        // Disable firing
                        __instance.time = 0;

                        // Disable animations
                        animPool[__instance.entityId].time = 0f;
                    }
                }
            }
        }
    }
}
