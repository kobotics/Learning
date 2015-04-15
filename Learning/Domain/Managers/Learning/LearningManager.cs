// ------------------------------------------
// LearningManager.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using Learning.Domain.Agents;
using Learning.Domain.Memories;
using PS.Utilities.Math;

namespace Learning.Domain.Managers.Learning
{
    [Serializable]
    public abstract class LearningManager : Manager
    {
        protected LearningManager(IAgent agent, ILongTermMemory longTermMemory)
            : base(agent)
        {
            this.LongTermMemory = longTermMemory;
            this.LearningRate = new StatisticalQuantity(this.Scenario.TestsConfig.LearningRate, new Range(0, 1));
            this.Discount = new StatisticalQuantity(this.Scenario.TestsConfig.Discount, new Range(0, 1));
        }

        public ILongTermMemory LongTermMemory { get; protected set; }

        public StatisticalQuantity LearningRate { get; protected set; }

        public StatisticalQuantity Discount { get; protected set; }

        public override void Update()
        {
            //updates action-value function of learning algorithm
            var stm = this.LongTermMemory.ShortTermMemory;
            if (stm.PreviousState == null) return;

            this.UpdateStateActionValue(
                stm.PreviousState.ID, stm.CurrentAction.ID, stm.CurrentState.ID, stm.CurrentReward.Value);
        }

        public override void PrintResults(string path)
        {
            this.LearningRate.PrintStatisticsToCSV(path + "/LearningRate.csv");
        }

        public override void Dispose()
        {
            this.LearningRate.Dispose();
            this.Discount.Dispose();
        }

        public override void Reset()
        {
            this.LearningRate = new StatisticalQuantity(this.LearningRate.Value, new Range(0, 1));
            this.Discount = new StatisticalQuantity(this.Discount.Value, new Range(0, 1));
        }

        protected virtual void UpdateStateActionValue(uint oldStateID, uint actionID, uint newStateID, double reward)
        {
            //checks args
            if ((oldStateID == uint.MaxValue) || (actionID == uint.MaxValue) || (newStateID == uint.MaxValue))
                return;

            //gets updated Q-value
            var updatedActionValueFunction = this.GetUpdatedStateActionValue(oldStateID, actionID, newStateID, reward);

            //updates action-value 
            this.LongTermMemory.UpdateStateActionValue(oldStateID, actionID, updatedActionValueFunction);
        }

        protected abstract double GetUpdatedStateActionValue(
            uint oldStateID, uint actionID, uint newStateID, double reward);
    }
}