// ------------------------------------------
// MotivationManager.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using Learning.Domain.Agents;
using PS.Utilities.Math;

namespace Learning.Domain.Managers.Motivation
{
    [Serializable]
    public abstract class MotivationManager : Manager, IMotivationManager
    {
        protected double extrinsicReward;

        protected MotivationManager(IAgent agent) : base(agent)
        {
            this.ExtrinsicReward = new StatisticalQuantity();
        }

        #region IMotivationManager Members

        public StatisticalQuantity ExtrinsicReward { get; protected set; }

        public virtual double GetReward(uint prevStateID, uint actionID, uint nextStateID)
        {
            return this.GetExtrinsicReward(prevStateID, actionID);
        }

        public override void Update()
        {
            this.ExtrinsicReward.Value = this.GetExtrinsicReward(
                this.Agent.ShortTermMemory.PreviousState.ID, this.Agent.ShortTermMemory.CurrentAction.ID);
        }

        public override void Dispose()
        {
            this.ExtrinsicReward.Dispose();
        }

        public override void Reset()
        {
            this.ExtrinsicReward = new StatisticalQuantity();
        }

        #endregion

        public abstract double GetExtrinsicReward(uint stateID, uint actionID);

        public override void PrintResults(string path)
        {
        }
    }
}