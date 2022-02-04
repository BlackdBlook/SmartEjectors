using HarmonyLib;

namespace SmartEjectors.Patch
{
    public static class LockEjectors
    {
        [HarmonyPostfix, HarmonyPatch(typeof(EjectorComponent), "InternalUpdate")]
        private static void EjectorComponent_InternalUpdate_Postfix(ref EjectorComponent __instance, DysonSwarm swarm, ref AnimData[] animPool)
        {
            if (!Config.enableLockEjector.isActive()) return;

            if (Util.IsSphereFilled(swarm.dysonSphere))
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
            if (!Config.enableLockEjector.isActive()) return;

            if (Util.IsSphereFilled(__instance.factory.dysonSphere))
            {
                // Show text for disabled status
                __instance.stateText.text = Locale.disabledEjector1[Localization.language];
                __instance.stateText.color = __instance.idleColor;
                __instance.valueText2.text = Locale.disabledEjector2[Localization.language];
                __instance.valueText2.color = __instance.idleColor;
                __instance.valueText3.color = __instance.factorySystem.ejectorPool[__instance.ejectorId].targetState == EjectorComponent.ETargetState.AngleLimit ? __instance.workStoppedColor : __instance.idleColor;
            }
        }
    }
}
