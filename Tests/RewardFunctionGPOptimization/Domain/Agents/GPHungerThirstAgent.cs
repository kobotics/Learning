// ------------------------------------------
// GPHungerThirstAgent.cs, Learning.Tests.RewardFunctionGPOptimization
// 
// Created by Pedro Sequeira, 2013/03/26
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using Learning.Domain.Managers.Perception;
using Learning.IMRL.EC.Domain;
using Learning.Tests.EmotionalOptimization.Domain.Agents;
using Learning.Tests.EmotionalOptimization.Domain.Environments;
using Learning.Tests.EmotionalOptimization.Domain.Managers;

namespace Learning.Tests.RewardFunctionGPOptimization.Domain.Agents
{
    [Serializable]
    public class GPHungerThirstAgent : GPForagingAgent, IHungerThirstAgent
    {
        #region IHungerThirstAgent Members

        public new HungerThirstEnvironment Environment
        {
            get { return base.Environment as HungerThirstEnvironment; }
        }

        #endregion

        protected override PerceptionManager CreatePerceptionManager()
        {
            return new HungerThirstPerceptionManager(this);
        }
    }
}