// ------------------------------------------
// GeneticsEmotionsManager.cs, Learning.Emotions
//
// Created by Pedro Sequeira, 2012/10/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using Learning.Domain.Agents;

namespace Learning.IMRL.Emotions.Domain.Managers
{
    [Serializable]
    public class GeneticsEmotionsManager : SchererEmotionsManager
    {
        private const long NORMALIZATION_FACTOR = -100000000;

        public GeneticsEmotionsManager(IAgent agent) : base(agent)
        {
        }

        public override double GetArousal(uint prevState, uint action, uint nextState)
        {
            return 0.5d;
        }

        public override double GetValence(uint prevState, uint action, uint newState)
        {
            //Valence(t) = Q(s, a) - V(s)

            //checks args and gets the value of state s after action a within current history
            var stateActionValue = ((prevState.Equals(uint.MaxValue)) || (action.Equals(uint.MaxValue)))
                                       ? 0
                                       : this.Agent.ExtrinsicLTM.GetStateActionValue(prevState, action);
            var stateValue = (prevState.Equals(uint.MaxValue))
                                 ? 0
                                 : this.Agent.ExtrinsicLTM.GetMaxStateActionValue(prevState);
            var valenceValue = stateActionValue - stateValue;

            return double.IsNaN(valenceValue) ? 0 : valenceValue;
        }

        public override double GetNovelty(uint prevState, uint action, uint nextState)
        {
            //Novelty(t) = - Count(s)^2
            var stateCount = this.Agent.ExtrinsicLTM.GetStateCount(prevState);
            var noveltyValue = (stateCount*stateCount)/NORMALIZATION_FACTOR;
            //var noveltyValue = (double)Math.Sqrt(Math.Exp(Math.Sqrt(stateCount))) / NORMALIZATION_FACTOR;

            return double.IsNaN(noveltyValue) ? 0 : noveltyValue;
        }

        public override double GetGoalRelevance(uint prevState, uint action, uint nextState)
        {
            //GoalRelevance(t) = Q(s,a)

            //checks args and gets the value of state s after action a within current history
            var goalRelevanceValue = ((prevState.Equals(uint.MaxValue)) || (action.Equals(uint.MaxValue)))
                                         ? 0
                                         : this.Agent.ExtrinsicLTM.GetStateActionValue(prevState, action);

            return double.IsNaN(goalRelevanceValue) ? 0 : goalRelevanceValue;
        }

        public override double GetControl(uint prevState, uint action, uint nextState)
        {
            //Control(t) = Prob(s'|s,a)

            //checks args and return transition prob
            var clarityValue = ((prevState.Equals(uint.MaxValue)) || (action.Equals(uint.MaxValue)) ||
                                (nextState.Equals(uint.MaxValue)))
                                   ? 0
                                   : this.Agent.ExtrinsicLTM.GetStateActionTransitionProbability(
                                       prevState, action, nextState);

            return double.IsNaN(clarityValue) ? 0 : clarityValue;
        }
    }
}