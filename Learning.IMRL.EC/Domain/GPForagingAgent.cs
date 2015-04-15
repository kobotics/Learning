// ------------------------------------------
// GPForagingAgent.cs, Learning.IMRL.EC
// 
// Created by Pedro Sequeira, 2013/03/26
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using Learning.Domain.Managers.Behavior;
using Learning.Domain.Managers.Learning;
using Learning.Domain.Managers.Motivation;
using Learning.Domain.Memories;
using Learning.IMRL.Domain.Agents;
using Learning.IMRL.Domain.Managers.Motivation;
using Learning.IMRL.EC.Chromosomes;
using Learning.IMRL.EC.Testing;

namespace Learning.IMRL.EC.Domain
{
    [Serializable]
    public class GPForagingAgent : IRAgent, IGPAgent, IIMRLAgent
    {
        public GPForagingAgent()
        {
            this.AutoEat = true;
        }

        public new StringLTM LongTermMemory
        {
            get { return base.LongTermMemory as StringLTM; }
        }

        public new PrioritySweepLearningManager LearningManager
        {
            get { return base.LearningManager as PrioritySweepLearningManager; }
        }

        public bool AutoEat { get; set; }

        #region IGPAgent Members

        public IGPChromosome Chromosome
        {
            get { return this.TestParameters as GPChromosome; }
        }

        public new GPMotivationManager MotivationManager
        {
            get { return base.MotivationManager as GPMotivationManager; }
        }

        public override void Update()
        {
            var stm = this.ShortTermMemory;

            //stores previous state
            this.ExtrinsicSTM.PreviousState =
                stm.PreviousState = stm.CurrentState;

            //update behavior (act)
            this.BehaviorManager.Update();
            this.ExtrinsicSTM.CurrentAction = stm.CurrentAction;

            //update environment
            this.Environment.Update();

            //update perception and stm
            this.PerceptionManager.Update();
            this.ExtrinsicSTM.CurrentState =
                stm.CurrentState = this.LongTermMemory.GetUpdatedCurrentState();
            stm.Update();
            this.ExtrinsicSTM.Update();

            //update extrinsic reward, ltm and learning
            if (stm.PreviousState != null)
                this.ExtrinsicSTM.CurrentReward.Value =
                    this.MotivationManager.ExtrinsicReward.Value =
                        this.MotivationManager.GetExtrinsicReward(stm.PreviousState.ID, stm.CurrentAction.ID);

            this.ExtrinsicLTM.Update();
            this.ExtrinsicLearningManager.Update();
            this.ExtrinsicLTM.UpdateMinMaxValues();

            //update emotions (feel)
            this.StateRelevanceManager.Update();

            //update intrinsic reward, ltm and learning
            if (stm.PreviousState != null)
                stm.CurrentReward.Value =
                    this.MotivationManager.IntrinsicReward.Value =
                        this.MotivationManager.GetIntrinsicReward(
                            stm.PreviousState.ID, stm.CurrentAction.ID, stm.CurrentState.ID);
            this.LongTermMemory.Update();
            this.LearningManager.Update();
        }

        #endregion

        #region IIMRLAgent Members

        EpsilonGreedyBehaviorManager IIMRLAgent.BehaviorManager
        {
            get { return this.BehaviorManager; }
        }

        IntrinsicMotivationManager IIMRLAgent.MotivationManager
        {
            get { return this.MotivationManager; }
        }

        #endregion

        protected override void CreateActions()
        {
            base.CreateActions();
            if (this.AutoEat && this.Actions.ContainsKey(EAT_ACTION_ID))
                this.Actions.Remove(EAT_ACTION_ID);
        }

        protected override LongTermMemory CreateLongTermMemory()
        {
            return new StringLTM(this, this.ShortTermMemory);
        }

        protected override MotivationManager CreateMotivationManager()
        {
            var testsConfig = (GPTestsConfig) this.Scenario.TestsConfig;
            return new ForagingGPMotivationManager(this, testsConfig.Constants);
        }
    }
}