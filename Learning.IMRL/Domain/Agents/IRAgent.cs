// ------------------------------------------
// IRAgent.cs, Learning.IMRL
// 
// Created by Pedro Sequeira, 2013/12/03
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using System.IO;
using Learning.Domain.Agents;
using Learning.Domain.Managers.Behavior;
using Learning.Domain.Managers.Learning;
using Learning.Domain.Managers.Motivation;
using Learning.Domain.Managers.Perception;
using Learning.Domain.Memories;
using Learning.IMRL.Domain.Managers;
using Learning.IMRL.Domain.Managers.Motivation;
using PS.Utilities;

namespace Learning.IMRL.Domain.Agents
{
    [Serializable]
    public class IRAgent : CellAgent, IIMRLAgent
    {
        protected const string EAT_ACTION_ID = "Eat";

        public IRAgent(LogWriter logWriter)
        {
            this.LogWriter = logWriter;
        }

        public IRAgent()
        {
            this.IdToken = "fox";
            this.ImagePath = "../../../../bin/resources/fox.png";
        }

        protected string ExtrinscMemoryBaseFilePath
        {
            get { return string.Format("{0}{1}Ext", this.MemoryBaseFilePath, Path.DirectorySeparatorChar); }
        }

        #region IIMRLAgent Members

        public LearningManager ExtrinsicLearningManager { get; protected set; }
        public ShortTermMemory ExtrinsicSTM { get; protected set; }
        public LongTermMemory ExtrinsicLTM { get; protected set; }

        public new EpsilonGreedyBehaviorManager BehaviorManager
        {
            get { return base.BehaviorManager as EpsilonGreedyBehaviorManager; }
        }

        public new IntrinsicMotivationManager MotivationManager
        {
            get { return base.MotivationManager as IntrinsicMotivationManager; }
        }

        public StateRelevanceManager StateRelevanceManager { get; protected set; }

        public override void Update()
        {
            var stm = this.ShortTermMemory;

            //stores previous state
            stm.PreviousState = stm.CurrentState;

            //update behavior (act)
            this.BehaviorManager.Update();

            //update environment
            this.Environment.Update();

            //update perception and stms
            this.PerceptionManager.Update();
            stm.CurrentState = this.LongTermMemory.GetUpdatedCurrentState();
            stm.Update();

            this.MotivationManager.ExtrinsicReward.Value =
                this.MotivationManager.GetExtrinsicReward(
                    stm.PreviousState.ID, stm.CurrentAction.ID);

            //update intrinsic reward, ltm and learning
            stm.CurrentReward.Value =
                this.MotivationManager.IntrinsicReward.Value =
                    this.MotivationManager.GetIntrinsicReward(
                        stm.PreviousState.ID, stm.CurrentAction.ID, stm.CurrentState.ID);
            this.LongTermMemory.Update();
            this.LearningManager.Update();
        }

        #endregion

        protected override void CreateManagers()
        {
            base.CreateManagers();
            this.StateRelevanceManager = this.CreateStateRelevanceManager();
            this.ExtrinsicLearningManager = this.CreateExtrinsicLearningManager();
        }

        protected override void CreateMemories()
        {
            base.CreateMemories();
            this.ExtrinsicSTM = CreateExtrinsicSTM();
            this.ExtrinsicLTM = CreateExtrinsicLTM();
            this.ExtrinsicLTM.Reset();
            this.ExtrinsicLTM.ReadAllStats(this.MemoryBaseFilePath);
        }

        protected virtual LearningManager CreateExtrinsicLearningManager()
        {
            return new PrioritySweepLearningManager(this, this.ExtrinsicLTM);
        }

        protected virtual LongTermMemory CreateExtrinsicLTM()
        {
            //return new StateTreeLTM(this, this.ExtrinsicSTM, this.TestProfile);
            return new StringLTM(this, this.ExtrinsicSTM);
        }

        protected virtual ShortTermMemory CreateExtrinsicSTM()
        {
            return new ShortTermMemory(this);
        }

        protected virtual StateRelevanceManager CreateStateRelevanceManager()
        {
            return new StateRelevanceManager(this);
        }

        protected override PerceptionManager CreatePerceptionManager()
        {
            return new CellPerceptionManager(this);
        }

        protected override MotivationManager CreateMotivationManager()
        {
            return new IRMotivationManager(this);
        }

        protected override LongTermMemory CreateLongTermMemory()
        {
            return new StringLTM(this, this.ShortTermMemory);
        }

        protected override BehaviorManager CreateBehaviorManager()
        {
            return new DecreaseEpsilonGreedyBehaviorManager(this);
            //return new BoltzmannBehaviorManager(this, this.TestProfile);
        }

        protected override LearningManager CreateLearningManager()
        {
            return new PrioritySweepLearningManager(this, this.LongTermMemory);
        }

        protected override void CreateActions()
        {
            base.CreateActions();

            //add eat
            this.Actions.Add(EAT_ACTION_ID, new Eat(EAT_ACTION_ID, this));
        }

        protected override void AddStatisticalQuantities()
        {
            base.AddStatisticalQuantities();
            if (this.LearningManager is PrioritySweepLearningManager)
                this.StatisticsCollection.Add(
                    "NumBackups", ((PrioritySweepLearningManager) this.LearningManager).NumBackups);
            this.StatisticsCollection.Add("IntrinsicReward", this.MotivationManager.IntrinsicReward);
            this.StatisticsCollection.Add("Epsilon", this.BehaviorManager.Epsilon);
        }
    }
}