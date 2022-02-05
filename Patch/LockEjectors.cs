using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace SmartEjectors.Patch
{
    public static class LockEjectors
    {
        [HarmonyTranspiler, HarmonyPatch(typeof(EjectorComponent), "InternalUpdate")]
        private static IEnumerable<CodeInstruction> EjectorComponent_InternalUpdate_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            try
            {
                CodeMatcher matcher = new CodeMatcher(instructions)
                    // IL_0638
                    .MatchForward(false,
                        new CodeMatch(OpCodes.Ldloc_3),
                        new CodeMatch(OpCodes.Ldloc_S),
                        new CodeMatch(OpCodes.And)
                    )
                    // flag = flag & !(Util.IsSphereFilled(swarm.dysonSphere) | Util.IsNodeLimitReached(swarm.dysonSphere))
                    .InsertAndAdvance(
                        new CodeInstruction(OpCodes.Ldarg_2),
                        new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(DysonSwarm), nameof(DysonSwarm.dysonSphere))),
                        Transpilers.EmitDelegate<Func<DysonSphere, bool>>((DysonSphere sphere) =>
                        {
                            return !(Util.IsSphereFilled(sphere) || Util.IsNodeLimitReached(sphere));
                        }),
                        new CodeInstruction(OpCodes.Ldloc_3),
                        new CodeInstruction(OpCodes.And),
                        new CodeInstruction(OpCodes.Stloc_3)
                    );

                return matcher.InstructionEnumeration();
            }
            catch
            {
                Plugin.Logger.LogError("EjectorComponent_InternalUpdate_Transpiler failed. Check game version.");
                return instructions;
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(UIEjectorWindow), "_OnUpdate")]
        private static void UIEjectorWindow__OnUpdate_Postfix(ref UIEjectorWindow __instance)
        {
            if (Util.IsSphereFilled(__instance.factory.dysonSphere))
            {
                // Show text for disabled status
                __instance.stateText.text = Locale.disabledEjector1[Localization.language];
                __instance.stateText.color = __instance.workStoppedColor;
                __instance.valueText2.text = Locale.disabledEjector2[Localization.language];
                __instance.valueText2.color = __instance.workStoppedColor;
                __instance.valueText3.color = __instance.factorySystem.ejectorPool[__instance.ejectorId].targetState == EjectorComponent.ETargetState.AngleLimit ? __instance.workStoppedColor : __instance.idleColor;
            }

            else if (Util.IsNodeLimitReached(__instance.factory.dysonSphere))
            {
                // Show text for disabled status
                __instance.stateText.text = Locale.disabledEjector3[Localization.language];
                __instance.stateText.color = __instance.workStoppedColor;
                __instance.valueText2.text = Locale.disabledEjector2[Localization.language];
                __instance.valueText2.color = __instance.workStoppedColor;
                __instance.valueText3.color = __instance.factorySystem.ejectorPool[__instance.ejectorId].targetState == EjectorComponent.ETargetState.AngleLimit ? __instance.workStoppedColor : __instance.idleColor;
            }
        }
    }
}
