// ------------------------------------------
// CumulativeFitnessFunction.cs, Learning
//
// Created by Pedro Sequeira, 2014/1/28
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using Learning.Domain.Agents;

namespace Learning.Testing.Simulations
{
    [Serializable]
    public class CumulativeFitnessFunction : IAgentFitnessFunction
    {
        #region Implementation of IAgentFitnessFunction

        public void UpdateCurrentFitness(IAgent agent)
        {
            if (agent.MotivationManager.ExtrinsicReward.Value > 0)
            {
                //agent.Fitness.Value += agent.MotivationManager.ExtrinsicReward.Value;
                agent.Fitness.Value += 10;
            }
        }

        #endregion
    }
}