// ------------------------------------------
// GPLairsAgent.cs, Learning.Tests.RewardFunctionGPOptimization
//
// Created by Pedro Sequeira, 2013/3/26
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using Learning.Domain.Managers.Perception;
using Learning.IMRL.EC.Domain;
using Learning.Tests.EmotionalOptimization.Domain.Actions;
using Learning.Tests.EmotionalOptimization.Domain.Agents;
using Learning.Tests.EmotionalOptimization.Domain.Environments;
using Learning.Tests.EmotionalOptimization.Domain.Managers;

namespace Learning.Tests.RewardFunctionGPOptimization.Domain.Agents
{
    [Serializable]
    public class GPLairsAgent : GPForagingAgent, ILairsAgent
    {
        private const string OPEN_ACTION_ID = "Open Lair";

        public GPLairsAgent()
        {
            //mandatory eat action here
            this.AutoEat = false;
        }

        #region ILairsAgent Members

        public new LairsEnvironment Environment
        {
            get { return base.Environment as LairsEnvironment; }
        }

        #endregion

        protected override PerceptionManager CreatePerceptionManager()
        {
            return new LairsPerceptionManager(this);
        }

        protected override void CreateActions()
        {
            base.CreateActions();

            //add action to open lair
            this.Actions.Add(OPEN_ACTION_ID, new OpenLair(OPEN_ACTION_ID, this));
        }
    }
}