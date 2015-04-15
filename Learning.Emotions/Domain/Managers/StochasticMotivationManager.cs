// ------------------------------------------
// StochasticMotivationManager.cs, Learning.Emotions
//
// Created by Pedro Sequeira, 2013/12/18
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using Learning.Domain.Memories;
using Learning.IMRL.Emotions.Domain.Agents;

namespace Learning.IMRL.Emotions.Domain.Managers
{
    [Serializable]
    public class StochasticMotivationManager : EmotionalMotivationManager
    {
        public StochasticMotivationManager(IEmotionalAgent agent) : base((EmotionalAgent) agent)
        {
        }

        public override double GetExtrinsicReward(uint state, uint action)
        {
            //gets reward according to reward table, current state and action
            return ((StochasticTransitionLTM) this.Agent.LongTermMemory).GetCurrentStateActionReward();
        }
    }
}