// ------------------------------------------
// StateDecreaseEpsilonBehaviorManager.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using Learning.Domain.Agents;

namespace Learning.Domain.Managers.Behavior
{
    [Serializable]
    public class StateDecreaseEpsilonBehaviorManager : DecreaseEpsilonGreedyBehaviorManager
    {
        public StateDecreaseEpsilonBehaviorManager(IAgent agent) : base(agent)
        {
            //default decay
            this.ExploratoryDecay = 1.0001f;
        }

        protected override double GetUpdatedEpsilonValue()
        {
            //checks current state
            var currentState = this.Agent.ShortTermMemory.CurrentState;
            if (currentState == null) return 1;

            //decreases epsilon according to state count
            var stateCount = this.Agent.LongTermMemory.GetStateCount(currentState.ID);
            return (0.5*System.Math.Pow(this.ExploratoryDecay, -stateCount));
        }
    }
}