// ------------------------------------------
// EmotionalAgent.cs, Learning.Emotions
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
using Learning.IMRL.Emotions.Domain.Managers;
using Learning.IMRL.Emotions.Domain.Memories;

namespace Learning.IMRL.Emotions.Domain.Agents
{
    [Serializable]
    public class EmotionalAgent : IRAgent, IEmotionalAgent
    {
        public EmotionalAgent()
        {
            this.AutoEat = true;
        }

        public SchererEmotionsManager EmotionsManager { get; protected set; }

        public new PrioritySweepLearningManager LearningManager
        {
            get { return base.LearningManager as PrioritySweepLearningManager; }
        }

        public bool AutoEat { get; set; }

        #region IEmotionalAgent Members

        EmotionsManager IEmotionalAgent.EmotionsManager
        {
            get { return this.EmotionsManager; }
        }

        public new EmotionalSTM ShortTermMemory
        {
            get { return base.ShortTermMemory as EmotionalSTM; }
        }

        public override void Update()
        {
            var stm = this.ShortTermMemory;

            //stores previous state
            this.ExtrinsicSTM.PreviousState =
                this.ShortTermMemory.PreviousState = this.ShortTermMemory.CurrentState;

            //update behavior (act)
            this.BehaviorManager.Update();
            this.ExtrinsicSTM.CurrentAction = this.ShortTermMemory.CurrentAction;

            //update environment
            this.Environment.Update();

            //update perception and stms
            this.PerceptionManager.Update();
            this.ExtrinsicSTM.CurrentState =
                this.ShortTermMemory.CurrentState = this.LongTermMemory.GetUpdatedCurrentState();
            this.ShortTermMemory.Update();
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
            this.EmotionsManager.Update();

            //update intrinsic reward, ltm and learning
            if (stm.PreviousState != null)
                this.ShortTermMemory.CurrentReward.Value =
                    this.MotivationManager.IntrinsicReward.Value = this.MotivationManager.GetIntrinsicReward(
                        stm.PreviousState.ID, stm.CurrentAction.ID, stm.CurrentState.ID);

            this.LongTermMemory.Update();
            this.LearningManager.Update();
        }

        public override void PrintAll(string path, string imgFormat)
        {
            //base.PrintAll(path, imgFormat);

            this.EmotionsManager.PrintResults(path);
        }

        public override void Dispose()
        {
            base.Dispose();
            this.EmotionsManager.Dispose();
        }

        public override void Reset()
        {
            this.EmotionsManager.Reset();
            base.Reset();
        }

        #endregion

        protected override void CreateActions()
        {
            base.CreateActions();
            if (this.AutoEat && this.Actions.ContainsKey(EAT_ACTION_ID))
                this.Actions.Remove(EAT_ACTION_ID);
        }

        protected override void CreateManagers()
        {
            base.CreateManagers();
            this.EmotionsManager = this.CreateEmotionsManager();
        }

        protected virtual SchererEmotionsManager CreateEmotionsManager()
        {
            //return new GeneticsEmotionsManager(this);
            return new SchererEmotionsManager(this);
        }

        protected override ShortTermMemory CreateShortTermMemory()
        {
            return new EmotionalSTM(this);
        }

        protected override MotivationManager CreateMotivationManager()
        {
            return new EmotionalHungryMotivationManager(this);
        }

        protected override LongTermMemory CreateLongTermMemory()
        {
            //return new StateTreeLTM(this, this.ShortTermMemory);
            return new StringLTM(this, this.ShortTermMemory);
        }

        protected override BehaviorManager CreateBehaviorManager()
        {
            return new DecreaseEpsilonGreedyBehaviorManager(this);
            //return new BoltzmannBehaviorManager(this);
        }

        protected override LearningManager CreateLearningManager()
        {
            return new PrioritySweepLearningManager(this, this.LongTermMemory);
        }

        protected override void AddStatisticalQuantities()
        {
            base.AddStatisticalQuantities();

            foreach (var dimension in this.ShortTermMemory.AppraisalSet.Dimensions.Values)
                this.StatisticsCollection.Add(dimension.Name, dimension);
            foreach (var emotionLabelCount in this.EmotionsManager.EmotionLabelsCount)
                this.StatisticsCollection.Add(emotionLabelCount.Key, emotionLabelCount.Value);
        }
    }
}