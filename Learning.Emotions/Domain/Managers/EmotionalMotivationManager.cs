// ------------------------------------------
// EmotionalMotivationManager.cs, Learning.Emotions
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using Learning.IMRL.Domain.Managers.Motivation;
using Learning.IMRL.Emotions.Domain.Agents;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Learning.IMRL.Emotions.Domain.Managers
{
    [Serializable]
    public class EmotionalMotivationManager : ArrayParamMotivationManager
    {
        private AppraisalSet _appraisalSet;

        public EmotionalMotivationManager(EmotionalAgent agent)
            : base(agent)
        {
        }

        public new IEmotionalAgent Agent
        {
            get { return base.Agent as IEmotionalAgent; }
        }

        public void CreateLabel(uint prevState, uint action, uint nextState)
        {
            var rewardFeatures = this.GetRewardFeatures(prevState, action, nextState);
            this._appraisalSet = new AppraisalSet
                                 {
                                     Novelty = {Value = rewardFeatures[0]},
                                     GoalRelevance = {Value = rewardFeatures[1]},
                                     Control = {Value = rewardFeatures[2]},
                                     Valence = {Value = rewardFeatures[3]},
                                     Arousal = {Value = rewardFeatures[4]},
                                 };
        }

        public override DenseVector GetRewardFeatures(uint prevState, uint action, uint nextState)
        {
            return new DenseVector(new[]
                                   {
                                       this.Agent.EmotionsManager.GetNovelty(prevState, action, nextState),
                                       this.Agent.EmotionsManager.GetGoalRelevance(prevState, action, nextState),
                                       this.Agent.EmotionsManager.GetControl(prevState, action, nextState),
                                       this.Agent.EmotionsManager.GetValence(prevState, action, nextState),
                                       this.GetExtrinsicReward(prevState, action)
                                   });
        }
    }
}