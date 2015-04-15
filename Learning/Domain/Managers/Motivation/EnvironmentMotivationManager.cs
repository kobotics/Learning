// ------------------------------------------
// EnvironmentMotivationManager.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using Learning.Domain.Agents;

namespace Learning.Domain.Managers.Motivation
{
    [Serializable]
    public class EnvironmentMotivationManager : MotivationManager
    {
        public EnvironmentMotivationManager(ISituatedAgent agent)
            : base(agent)
        {
        }

        public new ISituatedAgent Agent
        {
            get { return base.Agent as ICellAgent; }
        }

        public override double GetExtrinsicReward(uint stateID, uint actionID)
        {
            var state = this.Agent.LongTermMemory.GetState(stateID);
            var action = this.Agent.BehaviorManager.ActionList[(int) actionID];
            return this.Agent.Environment.GetAgentReward(this.Agent, state, action);
        }
    }
}