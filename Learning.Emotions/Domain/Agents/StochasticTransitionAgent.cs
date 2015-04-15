// ------------------------------------------
// StochasticTransitionAgent.cs, Learning.Emotions
//
// Created by Pedro Sequeira, 2013/12/18
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using Learning.Domain.Agents;
using Learning.Domain.Managers.Behavior;
using Learning.Domain.Managers.Learning;
using Learning.Domain.Managers.Motivation;
using Learning.Domain.Memories;
using Learning.IMRL.Emotions.Domain.Managers;
using Learning.IMRL.Emotions.Domain.Memories;

namespace Learning.IMRL.Emotions.Domain.Agents
{
    public class StochasticTransitionAgent : Agent, IEmotionalAgent
    {
        public new StochasticTransitionLTM LongTermMemory
        {
            get { return base.LongTermMemory as StochasticTransitionLTM; }
        }

        public new PrioritySweepLearningManager LearningManager
        {
            get { return base.LearningManager as PrioritySweepLearningManager; }
        }

        public new DecreaseEpsilonGreedyBehaviorManager BehaviorManager
        {
            get { return base.BehaviorManager as DecreaseEpsilonGreedyBehaviorManager; }
        }

        public new StochasticMotivationManager MotivationManager
        {
            get { return base.MotivationManager as StochasticMotivationManager; }
        }

        public SchererEmotionsManager EmotionsManager { get; protected set; }

        #region IEmotionalAgent Members

        public new EmotionalSTM ShortTermMemory
        {
            get { return base.ShortTermMemory as EmotionalSTM; }
        }

        EmotionsManager IEmotionalAgent.EmotionsManager
        {
            get { return this.EmotionsManager; }
        }

        public override void Update()
        {
            if (this.LogWriter != null) this.LogWriter.WriteLine("");

            this.ShortTermMemory.PreviousState = this.ShortTermMemory.CurrentState;

            //update behavior (execute "best" action)
            this.BehaviorManager.Update();

            //gets next state
            this.LongTermMemory.GetUpdatedCurrentState();

            //update emotions
            this.MotivationManager.Update();
            this.EmotionsManager.Update();

            //update reward 
            this.ShortTermMemory.CurrentReward.Value = this.MotivationManager.IntrinsicReward.Value;

            //update memories (memorize)
            this.LongTermMemory.Update();
            this.ShortTermMemory.Update();

            //update learning
            this.LearningManager.Update();
        }

        public override void Reset()
        {
            this.EmotionsManager.Reset();
            base.Reset();
        }

        #endregion

        protected override void AddStatisticalQuantities()
        {
            base.AddStatisticalQuantities();

            this.StatisticsCollection.Add("NumBackups", this.LearningManager.NumBackups);
            this.StatisticsCollection.Add(this.ShortTermMemory.Mood.Name, this.ShortTermMemory.Mood);
            this.StatisticsCollection.Add(this.ShortTermMemory.Clarity.Name, this.ShortTermMemory.Clarity);
            foreach (var dimension in this.ShortTermMemory.AppraisalSet.Dimensions.Values)
                this.StatisticsCollection.Add(dimension.Name, dimension);
            foreach (var emotionLabelCount in this.EmotionsManager.EmotionLabelsCount)
                this.StatisticsCollection.Add(emotionLabelCount.Key, emotionLabelCount.Value);
        }

        protected override void CreateActions()
        {
        }

        protected override void CreateManagers()
        {
            base.CreateManagers();
            this.EmotionsManager = this.CreateEmotionsManager();
        }

        protected virtual SchererEmotionsManager CreateEmotionsManager()
        {
            return new SchererEmotionsManager(this);
        }

        protected override LongTermMemory CreateLongTermMemory()
        {
            return new StochasticTransitionLTM(this, this.ShortTermMemory);
        }

        protected override MotivationManager CreateMotivationManager()
        {
            return new StochasticMotivationManager(this);
        }

        protected override ShortTermMemory CreateShortTermMemory()
        {
            return new EmotionalSTM(this);
        }

        protected override LearningManager CreateLearningManager()
        {
            return new PrioritySweepLearningManager(this, this.LongTermMemory);
        }

        protected override BehaviorManager CreateBehaviorManager()
        {
            return new DecreaseEpsilonGreedyBehaviorManager(this);
        }
    }
}