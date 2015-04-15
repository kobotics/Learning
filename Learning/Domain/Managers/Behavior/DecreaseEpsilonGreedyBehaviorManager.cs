// ------------------------------------------
// DecreaseEpsilonGreedyBehaviorManager.cs, Learning
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
    public class DecreaseEpsilonGreedyBehaviorManager : EpsilonGreedyBehaviorManager
    {
        public DecreaseEpsilonGreedyBehaviorManager(IAgent agent) : base(agent)
        {
            this.ExploratoryDecay = this.Scenario.TestsConfig.ExploratoryDecay;
            this.StartingEpsilon = 1;
        }

        public double ExploratoryDecay { get; set; }

        protected override double GetUpdatedEpsilonValue()
        {
            //decreases epsilon overtime
            return this.StartingEpsilon*Math.Pow(this.ExploratoryDecay, -(double) this.Agent.LongTermMemory.TimeStep);
        }
    }
}