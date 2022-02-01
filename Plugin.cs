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

            Harmony.CreateAndPatchAll(typeof(Patch));
        }

        static class Patch
        {
            [HarmonyPostfix, HarmonyPatch(typeof(EjectorComponent), "InternalUpdate")]
            private static void EjectorComponent_InternalUpdate_Postfix(ref EjectorComponent __instance, DysonSwarm swarm, ref AnimData[] animPool)
            {
                if (!SmartEjectors.Config.enableLockEjector.inUse) return;

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

            [HarmonyPostfix, HarmonyPatch(typeof(UIEjectorWindow), "_OnUpdate")]
            private static void UIEjectorWindow__OnUpdate_Postfix(ref UIEjectorWindow __instance)
            {
                if (!SmartEjectors.Config.enableLockEjector.inUse) return;

                DysonSphere sphere = __instance.factory.dysonSphere;

                // Check for filled sphere
                if (sphere.totalConstructedCellPoint + sphere.swarm.sailCount >= sphere.totalCellPoint)
                {
                    // Show text for disabled status
                    __instance.stateText.text = "Disabled - Filled Sphere";
                    __instance.stateText.color = __instance.idleColor;
                    __instance.valueText2.text = "Disabled";
                    __instance.valueText2.color = __instance.idleColor;
                    __instance.valueText3.color = __instance.factorySystem.ejectorPool[__instance.ejectorId].targetState == EjectorComponent.ETargetState.AngleLimit ? __instance.workStoppedColor : __instance.idleColor;
                }
            }
        }
    }
}
