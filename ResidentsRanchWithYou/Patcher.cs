using HarmonyLib;

namespace ResidentsRanchWithYou
{
    internal class Patcher
    {
        [HarmonyPrefix]
        [HarmonyPatch(declaringType: typeof(GoalWork), methodName: nameof(GoalWork.TryWork), argumentTypes: new[] { typeof(BaseArea), typeof(Hobby), typeof(bool) })]
        internal static bool GoalWorkTryWork(GoalWork __instance, BaseArea destArea, Hobby h, bool setAI, ref bool __result)
        {
            return GoalWorkPatch.TryWorkPrefix(__instance: __instance, destArea: destArea, h: h, setAI: setAI, __result: ref __result);
        }
        
        [HarmonyPrefix]
        [HarmonyPatch(declaringType: typeof(ElementContainer), methodName: nameof(ElementContainer.ModExp))]
        public static void ElementContainerModExp(ElementContainer __instance, int ele, float a, bool chain)
        {
            ElementContainerPatch.ModExpPrefix(__instance: __instance, ele: ele, a: a, chain: chain);
        }
    }
}