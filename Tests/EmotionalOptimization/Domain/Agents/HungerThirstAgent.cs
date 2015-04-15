// ------------------------------------------
// HungerThirstAgent.cs, Learning.Tests.EmotionalOptimization
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using Learning.Domain.Managers.Perception;
using Learning.IMRL.Emotions.Domain.Agents;
using Learning.Tests.EmotionalOptimization.Domain.Environments;
using Learning.Tests.EmotionalOptimization.Domain.Managers;

namespace Learning.Tests.EmotionalOptimization.Domain.Agents
{
    [Serializable]
    public class HungerThirstAgent : EmotionalAgent, IHungerThirstAgent
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