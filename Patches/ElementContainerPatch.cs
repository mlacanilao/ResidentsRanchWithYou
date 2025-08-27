using System;

namespace ResidentsRanchWithYou
{
    internal static class ElementContainerPatch
    {
        internal static void ModExpPrefix(ElementContainer __instance, int ele, float a, bool chain)
        {
            if (EClass.core?.IsGameStarted == false ||
                EClass._zone?.IsPCFaction == false ||
                __instance.Chara?.IsPC == false ||
                EClass.pc?.IsInSpot<TraitSpotRanch>() == false ||
                ele != 237 ||
                EClass._zone?.branch == null)
            {
                return;
            }
            
            foreach (Chara chara in EClass._zone?.branch?.members)
            {
                if (chara.IsPC == true)
                {
                    continue;
                }
                
                bool hasChoreWork = false;
                bool hasChoreHobby = false;
                
                foreach (Hobby w in chara.ListWorks())
                {
                    if (w.source.alias == "Chore" == false)
                    {
                        continue;
                    }
                    hasChoreWork = true;
                }
                
                foreach (Hobby h in chara.ListHobbies())
                {
                    if (h.source.alias == "Chore" == false)
                    {
                        continue;
                    }
                    hasChoreHobby = true;
                }

                if (hasChoreWork == true &&
                    !(chara.ai is AIWork_RanchOmega || chara.ai is AI_BrushOmega || chara.ai is AI_ShearOmega))
                {
                    chara.TryMoveTowards(p: EClass.pc?.pos);
                    
                    Goal goal = chara.GetGoalWork();
                    if (goal is GoalWork goalWork)
                    {
                        goalWork.FindWork(c: chara);
                    }
                }

                if (hasChoreHobby == true &&
                    !(chara.ai is AIWork_RanchOmega || chara.ai is AI_BrushOmega || chara.ai is AI_ShearOmega))
                {
                    chara.TryMoveTowards(p: EClass.pc?.pos);
                    
                    Goal goal = chara.GetGoalHobby();
                    if (goal is GoalHobby goalHobby)
                    {
                        goalHobby.FindWork(c: chara);
                    }
                }
            }
        }
    }
}