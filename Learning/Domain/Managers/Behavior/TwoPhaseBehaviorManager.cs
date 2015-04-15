// ------------------------------------------
// TwoPhaseBehaviorManager.cs, Learning
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
    public class TwoPhaseBehaviorManager : DecreaseEpsilonGreedyBehaviorManager
    {
        public TwoPhaseBehaviorManager(IAgent agent) : base(agent)
        {
            //sets default exploration phase
            this.ExplorationSteps = 40000;
        }

        public uint ExplorationSteps { get; set; }

        public bool ExplorationPhase { get; set; }

        protected override double GetUpdatedEpsilonValue()
        {
            //in the exploration phase, epsilon is 1 during some steps, 0 after
            //in the "normal" phase, epsilon is decreased according to e-greedy
            return this.ExplorationPhase
                       ? (this.Agent.LongTermMemory.TimeStep < this.ExplorationSteps ? 1 : 0)
                       : base.GetUpdatedEpsilonValue();
        }
    }
}