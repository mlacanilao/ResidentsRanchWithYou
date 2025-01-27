using System;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentsRanchWithYou
{
    public class AI_BrushOmega : TaskPoint
    {
        public override int LeftHand
        {
            get
            {
                return -1;
            }
        }
        
        public override int RightHand
        {
            get
            {
                return 1006;
            }
        }
        
        public override bool HasProgress
        {
            get
            {
                return true;
            }
        }
        
        public override bool CancelWhenMoved
        {
            get
            {
                return false;
            }
        }
        
        public override void OnStart()
        {
            this.TargetLivestock.noMove = true;
            this.owner?.Say(lang: "tame_start", c1: this.owner, c2: this.TargetLivestock, ref1: null, ref2: null);
            this.progress = this.CurrentProgress;
        }

        public override void OnProgress()
        {
            bool enableBrush = ResidentsRanchWithYouConfig.EnableBrush?.Value ?? true;

            if (enableBrush == true)
            {
                if (this.progress % 5 == 0)
                {
                    this.owner?.PlaySound(id: "brushing", v: 1f, spatial: true);
                }
            }

            this.progress++;
        }
        
        public override void OnProgressComplete()
        {
            List<Chara> neighborLivestock = this.pos?.ListCharasInNeighbor(func: c =>
                c.memberType == FactionMemberType.Livestock);

            if (neighborLivestock == null || neighborLivestock.Count == 0)
            {
                return;
            }
            
            foreach (Chara livestock in neighborLivestock)
            {
                if (livestock == null)
                {
                    continue;
                }
                
                livestock.noMove = true;
                
                // Brush
                bool enableBrush = ResidentsRanchWithYouConfig.EnableBrush?.Value ?? true;

                if (enableBrush == true)
                {
                    livestock.PlayEffect(id: "heal_tick", useRenderPos: true, range: 0f, fix: default(Vector3));
                    livestock.ModAffinity(c: EClass.pc, a: 1, show: true, showOnlyEmo: false);
                }
                
                // Pasture
                bool enableRequirePasture = ResidentsRanchWithYouConfig.EnableRequirePasture?.Value ?? false;
                
                if (enableRequirePasture == true)
                {
                    Thing pasture = EClass._map?.Stocked?.Find(id: "pasture", idMat: -1, refVal: -1, shared: true);

                    if (pasture == null)
                    {
                        pasture = EClass._map?.Installed?.Find(id: "pasture", idMat: -1, refVal: -1, shared: false);
                    }

                    if (pasture != null)
                    {
                        ProcessLivestockLogic(livestock: livestock);
                        pasture.ModNum(a: -1, notify: true);
                        if (pasture.isDestroyed || pasture.Num == 0)
                        {
                            pasture = null;
                        }
                    }
                }
                else
                {
                    ProcessLivestockLogic(livestock: livestock);
                }
                
                bool enableNoMove = ResidentsRanchWithYouConfig.EnableNoMove?.Value ?? true;
                if (enableNoMove == false)
                {
                    livestock.noMove = false;
                }
            }
            
            this.owner?.Say(lang: "tame_end", c1: this.TargetLivestock, c2: null, ref1: null, ref2: null);

            bool hasFur = this.TargetLivestock.HaveFur();
            if (hasFur == true)
            {
                int furLv = AI_Shear.GetFurLv(c: this.TargetLivestock);
                bool enableShear = ResidentsRanchWithYouConfig.EnableShear?.Value ?? true;
                int furLevelThreshold = ResidentsRanchWithYouConfig.FurLevelThreshold?.Value ?? 5;
                
                if (this.TargetLivestock?.isDead == false &&
                    enableShear == true &&
                    furLv >= furLevelThreshold)
                {
                    this.TargetLivestock.noMove = true;

                    Point randomNeighbor = null;

                    int maxTries = 16;
                
                    for (int i = 0; i < maxTries; i++)
                    {
                        randomNeighbor = this.TargetLivestock?.pos?.GetRandomNeighbor();
                        if (randomNeighbor != null && 
                            randomNeighbor.HasChara == false &&
                            this.owner?.CanMoveTo(p: randomNeighbor) == true)
                        {
                            break;
                        }
                    }
                    
                    this.owner?.TryMoveTowards(p: randomNeighbor);
                
                    this.owner?.SetAI(g: new AI_ShearOmega
                    {
                        pos = randomNeighbor,
                        TargetLivestock = this.TargetLivestock,
                        PreviousWork = this.PreviousWork
                    });
                }

                AssignBrushTask();
                return;
            }
            
            AssignBrushTask();
        }
        
        private void ProcessLivestockLogic(Chara livestock)
        {
            if (livestock == null)
            {
                return;
            }
            
            // Food
            bool enableFood = ResidentsRanchWithYouConfig.EnableFood?.Value ?? true;
            if (enableFood == true && livestock?.hunger?.GetPhase() >= 3)
            {
                Thing food = livestock.things?.Find(func: (Thing a) => livestock.CanEat(t: a, shouldEat: livestock.IsPCFaction) && !a.c_isImportant, recursive: false);

                if (food == null && livestock.IsPCFaction)
                {
                    food = livestock.things?.Find(func: (Thing a) => livestock.CanEat(t: a, shouldEat: false) && !a.c_isImportant, recursive: false);
                }
                if (food == null && livestock.IsPCFaction && EClass._zone.IsPCFaction)
                {
                    food = EClass._zone?.branch?.GetMeal(c: livestock);
                    if (food != null)
                    {
                        livestock?.Pick(t: food, msg: true, tryStack: true);
                    }
                }
                if (food == null && livestock.things?.IsFull(y: 0) == false)
                {
                    food = ThingGen.CreateFromCategory(idCat: "food", lv: EClass.rnd(a: EClass.rnd(a: 60) + 1) + 10);
                    food.isNPCProperty = true;

                    if ((food.ChildrenAndSelfWeight < 5000 || !livestock.IsPCParty) && food.trait.CanEat(c: livestock))
                    {
                        food = livestock.AddThing(t: food, tryStack: true, destInvX: -1, destInvY: -1);
                    }
                }

                if (food != null)
                {
                    livestock.SetAIImmediate(g: new AI_Eat
                    {
                        target = food
                    });
                }
            }
            
            // Level
            bool enableLivestockLevel = ResidentsRanchWithYouConfig.EnableLivestockLevel?.Value ?? true;
            if (enableLivestockLevel == true)
            {
                livestock.LV += 1;
            }
                
            // Egg
            bool enableEgg = ResidentsRanchWithYouConfig.EnableEgg?.Value ?? true;
            int eggChance = ResidentsRanchWithYouConfig.EggChance?.Value ?? 10;
            if (enableEgg == true)
            {
                int makeEgg = EClass.rnd(a: 100);
                if (makeEgg < eggChance)
                {
                    Thing egg = livestock.MakeEgg();
                
                    if (egg != null)
                    {
                        livestock.TryPutShared(t: egg, containers: null, dropIfFail: true);
                    }
                }
            }
            
            // Milk
            bool enableMilk = ResidentsRanchWithYouConfig.EnableMilk?.Value ?? true;
            int milkChance = ResidentsRanchWithYouConfig.MilkChance?.Value ?? 10;
            int babyFeat = livestock.Evalue(ele: 1232);
            if (enableMilk == true &&
                babyFeat == 0)
            {
                int makeMilk = EClass.rnd(a: 100);
                if (makeMilk < milkChance)
                {
                    Thing milk = livestock.MakeMilk();
                
                    if (milk != null)
                    {
                        livestock.TryPutShared(t: milk, containers: null, dropIfFail: true);
                    }
                }
            }
            
            // Baby Milk
            bool enableFeedMilkToBaby = ResidentsRanchWithYouConfig.EnableFeedMilkToBaby?.Value ?? true;
            if (enableFeedMilkToBaby == true &&
                babyFeat > 0)
            {
                Thing milk = livestock.MakeMilk();

                if (milk != null)
                {
                    livestock.SetAIImmediate(g: new AI_Eat
                    {
                        target = milk
                    });
                }
            }
            
            // Fur
            bool enableFurLevel = ResidentsRanchWithYouConfig.EnableFurLevel?.Value ?? true;
            if (enableFurLevel == true &&
                livestock.HaveFur() == true)
            {
                livestock.c_fur += 1;
            }
        }
        
        private Point FindDestinationPosition(Thing destThing)
        {
            List<Point> destPoints = destThing?.trait?.ListPoints();
            if (destPoints == null || destPoints.Count == 0)
            {
                return null;
            }

            List<Chara> livestock = new List<Chara>();
            foreach (Point point in destPoints)
            {
                foreach (Chara chara in point.ListCharas())
                {
                    if (chara.memberType == FactionMemberType.Livestock && chara != this.TargetLivestock)
                    {
                        livestock.Add(item: chara);
                    }
                }
            }

            Chara randomLivestock = livestock.RandomItem<Chara>();
            if (randomLivestock != null)
            {
                this.TargetLivestock = randomLivestock;
                
                randomLivestock.noMove = true;
                
                int maxTries = 16;
                
                for (int i = 0; i < maxTries; i++)
                {
                    Point randomNeighbor = randomLivestock.pos?.GetRandomNeighbor();
                    if (randomNeighbor != null && 
                        randomNeighbor.HasChara == false &&
                        this.owner?.CanMoveTo(p: randomNeighbor) == true)
                    {
                        return randomNeighbor;
                    }
                }
            }

            return null;
        }
        
        private void AssignBrushTask()
        {
            
            Point destPos = FindDestinationPosition(destThing: this.PreviousWork?.destThing);

            if (destPos == null)
            {
                return;
            }
            
            this.owner?.TryMoveTowards(p: destPos);
            
            this.owner?.SetAIImmediate(g: new AI_BrushOmega
            {
                pos = destPos,
                TargetLivestock = this.TargetLivestock,
                PreviousWork = this.PreviousWork
            });
        }
        
        public Chara TargetLivestock;
        public AIWork_RanchOmega PreviousWork;
        public int progress;
    }
}