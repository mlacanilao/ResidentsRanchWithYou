using System;
using System.Collections.Generic;

namespace ResidentsRanchWithYou
{
    public class AIWork_RanchOmega : AIWork
    {
        public override AIAct GetWork(Point p)
        {
            this._previousWork = this;
            
            return new AI_BrushOmega
            {
                pos = p,
                TargetLivestock = this._targetLivestock,
                PreviousWork = this._previousWork
            };
        }
        
        public override void SetDestPos()
        {
            if (EClass.core?.IsGameStarted == false ||
                EClass._zone?.IsPCFaction == false ||
                this.destThing == null ||
                this.destThing?.ExistsOnMap == false)
            {
                return;
            }
            
            Point destPos = FindDestinationPosition(destThing: this.destThing);
            this.destPos = destPos;
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
                    if (chara.memberType == FactionMemberType.Livestock)
                    {
                        livestock.Add(item: chara);
                    }
                }
            }

            Chara randomLivestock = livestock.RandomItem<Chara>();
            
            if (randomLivestock != null)
            {
                this._targetLivestock = randomLivestock;
                
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
        
        public override IEnumerable<AIAct.Status> Run()
        {
            yield return base.DoIdle(repeat: 100);
            AIWork.Work_Type workType = this.WorkType;
            this.SetDestPos();
            if (this.destPos != null)
            {
                yield return base.DoGoto(pos: this.destPos, dist: this.destDist, ignoreConnection: false, _onChildFail: null);
                AIAct work = this.GetWork(p: this.destPos);
                if (work != null)
                {
                    this.owner.Talk(idTopic: "work_" + this.sourceWork.talk, ref1: null, ref2: null, forceSync: false);
                    yield return base.Do(_seq: work, _onChildFail: new Func<AIAct.Status>(base.KeepRunning));
                }
            }
            else
            {
                this.Cancel();
            }
            yield return base.Restart();
            yield break;
        }

        private Chara _targetLivestock;
        private AIWork_RanchOmega _previousWork;
    }
}