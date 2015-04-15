// ------------------------------------------
// CellAgentLTM.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using Learning.Domain.Agents;

namespace Learning.Domain.Memories
{
    [Serializable]
    public abstract class CellAgentLTM : LongTermMemory
    {
        protected CellAgentLTM(CellAgent agent, ShortTermMemory shortTermMemory)
            : base(agent, shortTermMemory)
        {
        }

        public new CellAgent Agent
        {
            get { return base.Agent as CellAgent; }
        }

        public virtual double GetDistanceToOptimalState()
        {
            return this.Agent.Environment.GetCurrentDistanceToTargetCell(this.Agent);
        }
    }
}