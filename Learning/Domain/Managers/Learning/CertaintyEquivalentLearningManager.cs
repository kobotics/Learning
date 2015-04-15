// ------------------------------------------
// CertaintyEquivalentLearningManager.cs, Learning
// 
// Created by Pedro Sequeira, 2012/10/09
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using System.Linq;
using Learning.Domain.Agents;
using Learning.Domain.Memories;
using PS.Utilities.Collections;

namespace Learning.Domain.Managers.Learning
{
    [Serializable]
    public class CertaintyEquivalentLearningManager : LearningManager
    {
        private const int MAX_ITERATIONS = 20;
        private double _oldActionValue;
        private double[][] _previousStateActionValue;

        public CertaintyEquivalentLearningManager(IAgent agent, LongTermMemory longTermMemory)
            : base(agent, longTermMemory)
        {
            this.MaximalChangeThreshold = this.Scenario.TestsConfig.MaximalChangeThreshold;
        }

        public double MaximalChangeThreshold { get; set; }

        public override void Dispose()
        {
            base.Dispose();
            this._previousStateActionValue = null;
        }

        public override void Reset()
        {
            base.Reset();

            var ltm = this.LongTermMemory;
            this._previousStateActionValue = ArrayUtil.Create2DArray<double>(ltm.MaxStates, ltm.NumActions);
        }

        public override void Update()
        {
            //stores old action-value
            this.StoreOldActionValue();

            //update action-values (for all state-action)
            var shortTermMemory = this.LongTermMemory.ShortTermMemory;
            this.UpdateStateActionValue(shortTermMemory.PreviousState.ID,
                shortTermMemory.CurrentAction.ID,
                shortTermMemory.CurrentState.ID,
                shortTermMemory.CurrentReward.Value);

            //update prediction error for current state and action
            this.UpdatePredictionError();
        }

        public void UpdatePredictionError()
        {
            //checks variables
            var oldState = this.LongTermMemory.ShortTermMemory.PreviousState;
            var action = this.LongTermMemory.ShortTermMemory.CurrentAction;

            if ((oldState == null) || (action == null)) return;

            //updates prediction error (not used by the learning algorithm, but by emotions manager)
            this.LongTermMemory.ShortTermMemory.PredictionError =
                this.LongTermMemory.GetStateActionValue(oldState.ID, action.ID) - this._oldActionValue;
        }

        protected void StoreOldActionValue()
        {
            //gets args
            var oldState = this.LongTermMemory.ShortTermMemory.PreviousState;
            var action = this.LongTermMemory.ShortTermMemory.CurrentAction;

            //stores action-value
            this._oldActionValue = this.LongTermMemory.GetStateActionValue(oldState.ID, action.ID);
        }

        protected override double GetUpdatedStateActionValue(
            uint oldStateID, uint actionID, uint newStateID, double reward)
        {
            var longTermMemory = this.LongTermMemory;

            //checks args
            if ((oldStateID == uint.MaxValue) || (actionID == uint.MaxValue))
                return 0;

            //gets weighted sum of action-values of all possible future state transitions
            var futureTransitionValue = 0d;
            for (var transitionStateID = 0u; transitionStateID < longTermMemory.NumStates; transitionStateID++)
            {
                futureTransitionValue +=
                    longTermMemory.GetStateActionTransitionProbability(oldStateID, actionID, transitionStateID)*
                    this.GetMaxStateActionValue(transitionStateID);
            }

            //returns new value according to Model-based-learning algorithm
            return reward + (this.Discount.Value*futureTransitionValue);
        }

        protected virtual double GetMaxStateActionValue(uint stateID)
        {
            //checks state arg and returns max action value for given state
            return stateID >= this.LongTermMemory.NumStates ? 0 : this._previousStateActionValue[stateID].Max();
        }

        protected override void UpdateStateActionValue(
            uint oldStateID, uint curActionID, uint newStateID, double curReward)
        {
            var maximalChange = double.MaxValue;
            var numIterations = 0;

            //resets all Q-values (sets to zero)
            this.LongTermMemory.ResetAllStateActionValues();

            //updates Q-values while change in values is large
            while (maximalChange > this.MaximalChangeThreshold)
            {
                if (numIterations++ > MAX_ITERATIONS)
                    break;

                //copies previous state-action values to memory buffer
                for (var stateID = 0u; stateID < this.LongTermMemory.NumStates; stateID++)
                    for (var actionID = 0u; actionID < this.LongTermMemory.NumActions; actionID++)
                        this._previousStateActionValue[stateID][actionID] =
                            this.LongTermMemory.GetStateActionValue(stateID, actionID);

                maximalChange = 0;

                //for all state-action pairs, update Q-value
                for (var stateID = 0u; stateID < this.LongTermMemory.NumStates; stateID++)
                    for (var actionID = 0u; actionID < this.LongTermMemory.NumActions; actionID++)
                    {
                        //stores old Q-value
                        var previousActionValue = this._previousStateActionValue[stateID][actionID];

                        //gets state-action reward (estimated)
                        var reward = this.LongTermMemory.GetStateActionReward(stateID, actionID);

                        //updates new Q-value for current state-action pair
                        var updatedActionValue = this.GetUpdatedStateActionValue(stateID, actionID, uint.MaxValue,
                            reward);
                        this.LongTermMemory.UpdateStateActionValue(stateID, actionID, updatedActionValue);

                        //gets and stores change in Q-value
                        var change = Math.Abs(updatedActionValue - previousActionValue);
                        maximalChange = Math.Max(maximalChange, change);
                    }
            }

            //Console.WriteLine(@"{0} iterations", numIterations);
        }
    }
}