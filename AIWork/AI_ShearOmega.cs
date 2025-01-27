using System;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentsRanchWithYou
{
    public class AI_ShearOmega : TaskPoint
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
            this.owner?.Say(lang: "shear_start", c1: this.owner, c2: this.TargetLivestock, ref1: null, ref2: null);
        }

        public override void OnProgress()
        {
            this.owner?.PlaySound(id: "shear", v: 1f, spatial: true);
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
                
                // Shear
                livestock.renderer?.PlayAnime(id: AnimeID.Shiver, dest: default(Vector3), force: false);
                int furLv = AI_Shear.GetFurLv(c: livestock);
                Thing fur = AI_Shear.GetFur(c: livestock.Chara, mod: 100);
                
                if (fur != null)
                {
                    this.owner?.Say(lang: "shear_end", c1: this.owner, c2: this.TargetLivestock, ref1: fur.Name, ref2: null);
                    
                    livestock.TryPutShared(t: fur, containers: null, dropIfFail: true);
                }
                
                this.owner?.elements.ModExp(ele: 237, a: 50 * furLv, chain: false);

                bool enableNoMove = ResidentsRanchWithYouConfig.EnableNoMove?.Value ?? true;
                if (enableNoMove == false)
                {
                    livestock.noMove = false;
                }
            }
            
            AssignBrushTask();
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
    }
}