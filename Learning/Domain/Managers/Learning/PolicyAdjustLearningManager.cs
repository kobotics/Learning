// ------------------------------------------
// PolicyAdjustLearningManager.cs, Learning
//
// Created by Pedro Sequeira, 2014/1/22
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using Learning.Domain.Agents;
using Learning.Domain.Memories;

namespace Learning.Domain.Managers.Learning
{
    [Serializable]
    public class PolicyAdjustLearningManager : LearningManager
    {
        public PolicyAdjustLearningManager(IAgent agent, ILongTermMemory longTermMemory)
            : base(agent, longTermMemory)
        {
        }

        public override void Update()
        {
            var stm = this.LongTermMemory.ShortTermMemory;
            var ltm = this.Agent.LongTermMemory;
            var oldStateID = stm.PreviousState.ID;
            var actionID = stm.CurrentAction.ID;
            var newStateID = stm.CurrentState.ID;
            var reward = stm.CurrentReward.Value;

            //first update prediction error and then action-value function
            stm.PredictionError = this.GetPredictionError(oldStateID, actionID, newStateID, reward);

            //adjust q-value/policy from given action
            this.UpdateStateActionValue(oldStateID, actionID, newStateID, reward);

            if ((oldStateID == uint.MaxValue) || (actionID == uint.MaxValue))
                return;

            //gets state policy based on (updated) q-values
            var policy = this.Agent.BehaviorManager.GetPolicy(oldStateID);

            //updates state-action values directly based on (normalized) policy
            foreach (var action in this.Agent.Actions.Values)
                ltm.UpdateStateActionValue(oldStateID, action.ID, policy[action.ID]);
        }

        protected override double GetUpdatedStateActionValue(
            uint oldStateID, uint actionID, uint newStateID, double reward)
        {
            //checks args
            if ((oldStateID == uint.MaxValue) || (actionID == uint.MaxValue) || (newStateID == uint.MaxValue))
                return 0;

            //adjust value according to new reward
            var stateActionValue = this.LongTermMemory.GetStateActionValue(oldStateID, actionID);
            var adjustment = this.GetPredictionError(oldStateID, actionID, newStateID, reward);

            return stateActionValue + adjustment;
        }

        public virtual double GetPredictionError(uint oldStateID, uint actionID, uint newStateID, double reward)
        {
            //checks args
            if ((oldStateID == uint.MaxValue) || (actionID == uint.MaxValue) || (newStateID == uint.MaxValue))
                return 0;

            //updates prediction error -> adjustment
            return (this.LearningRate.Value*reward);
        }
    }
}