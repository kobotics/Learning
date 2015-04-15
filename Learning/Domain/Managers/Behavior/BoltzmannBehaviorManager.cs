// ------------------------------------------
// BoltzmannBehaviorManager.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using Learning.Domain.Actions;
using Learning.Domain.Agents;

namespace Learning.Domain.Managers.Behavior
{
    [Serializable]
    public class BoltzmannBehaviorManager : DecreaseEpsilonGreedyBehaviorManager
    {
        public BoltzmannBehaviorManager(IAgent agent) : base(agent)
        {
            //default eps value
            this.StartingEpsilon = this.Scenario.TestsConfig.Temperature;
        }

        protected override Policy GetStatePolicy(uint stateID)
        {
            //for each action get weightevalue according to Q and Eps (temp)
            var policy = new Policy((uint) this.Actions.Count, false);
            for (var i = 0u; i < this.Actions.Count; i++)
                policy[i] = this.GetWeightedQValue(stateID, i);
            return policy;
        }

        private double GetWeightedQValue(uint stateID, uint actionID)
        {
            var qValue = this.Agent.LongTermMemory.GetStateActionValue(stateID, actionID);
            return System.Math.Exp(qValue/this.Epsilon.Value);
        }

        protected override double GetUpdatedEpsilonValue()
        {
            //decreases epsilon overtime
            return this.StartingEpsilon*
                   System.Math.Pow(this.ExploratoryDecay, -(double) this.Agent.LongTermMemory.TimeStep);
        }
    }
}