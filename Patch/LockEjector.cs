using HarmonyLib;

namespace SmartEjectors.Patch
{
    public static class LockEjectors
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
