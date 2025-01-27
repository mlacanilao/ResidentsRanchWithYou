using System;

namespace ResidentsRanchWithYou
{
    internal class GoalWorkPatch
    {
        internal static bool TryWorkPrefix(GoalWork __instance, BaseArea destArea, Hobby h, bool setAI,
            ref bool __result)
        {
            if (EClass.core?.IsGameStarted == false ||
                EClass._zone?.IsPCFaction == false ||
                h.source?.alias != "Chore")
            {
                return true;
            }
            
            AIWork ai = new AIWork_RanchOmega();
            ai.owner = __instance.owner;
            ai.sourceWork = h.source;
            ai.destArea = destArea;
            ai.destThing = EClass._map?.FindThing(type: Type.GetType(typeName: "TraitSpotRanch, Elin"), c: __instance.owner);
            
            if (ai.destThing != null)
            {
                if (setAI)
                {
                    __instance.owner.SetAI(g: ai);
                }

                __result = true;
                return false;
            }
            __result = false;
            return false;
        }
    }
}