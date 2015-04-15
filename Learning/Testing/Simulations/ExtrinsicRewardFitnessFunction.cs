// ------------------------------------------
// ExtrinsicRewardFitnessFunction.cs, Learning
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
    public class ExtrinsicRewardFitnessFunction : IAgentFitnessFunction
    {
        #region Implementation of IAgentFitnessFunction

        public void UpdateCurrentFitness(IAgent agent)
        {
            agent.Fitness.Value = agent.MotivationManager.ExtrinsicReward.Value;
        }

        #endregion
    }
}