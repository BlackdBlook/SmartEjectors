using HarmonyLib;

namespace SmartEjectors.Patch
{
    public static class DefaultOrbit
    {
        [HarmonyPostfix, HarmonyPatch(typeof(PlanetFactory), "CreateEntityLogicComponents")]
        private static void PlanetFactory_CreateEntityLogicComponents_Postfix(PlanetFactory __instance, int entityId, PrefabDesc desc)
        {
            if (!Config.enableDefaultOrbit.isActive()) return;

            if (desc.isEjector)
            {
                __instance.factorySystem.ejectorPool[__instance.entityPool[entityId].ejectorId].SetOrbit(1);
            }
        }
    }
}
