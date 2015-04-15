// ------------------------------------------
// QLearningManager.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using Learning.Domain.Agents;
using Learning.Domain.Memories;

namespace Learning.Domain.Managers.Learning
{
    [Serializable]
    public class QLearningManager : LearningManager
    {
        public QLearningManager(IAgent agent, ILongTermMemory longTermMemory)
            : base(agent, longTermMemory)
        {
        }

        public override void Update()
        {
            var stm = this.LongTermMemory.ShortTermMemory;
            var previousStateID = stm.PreviousState.ID;
            var currentActionID = stm.CurrentAction.ID;
            var currentStateID = stm.CurrentState.ID;
            var reward = stm.CurrentReward.Value;

            //first update prediction error and then action-value function
            stm.PredictionError = this.GetPredictionError(previousStateID, currentActionID, currentStateID, reward);

            this.UpdateStateActionValue(previousStateID, currentActionID, currentStateID, reward);
        }

        protected override double GetUpdatedStateActionValue(
            uint oldStateID, uint actionID, uint newStateID, double reward)
        {
            //checks args
            if ((oldStateID == uint.MaxValue) || (actionID == uint.MaxValue) || (newStateID == uint.MaxValue))
                return 0;

            //returns new value according to Q-Learning algorithm
            var stateActionValue = this.LongTermMemory.GetStateActionValue(oldStateID, actionID);
            var predictionError = this.GetPredictionError(oldStateID, actionID, newStateID, reward);
            return stateActionValue + (this.LearningRate.Value*predictionError);
        }

        public virtual double GetPredictionError(uint oldStateID, uint actionID, uint newStateID, double reward)
        {
            //checks args
            if ((oldStateID == uint.MaxValue) || (actionID == uint.MaxValue) || (newStateID == uint.MaxValue))
                return 0;

            //gets max future value
            var maxFutureValue = this.Discount.Value.Equals(0)
                                     ? 0
                                     : this.Discount.Value * this.LongTermMemory.GetMaxStateActionValue(newStateID);

            //updates prediction error
            var stateActionValue = this.LongTermMemory.GetStateActionValue(oldStateID, actionID);
            return reward + maxFutureValue - stateActionValue;
        }
    }
}