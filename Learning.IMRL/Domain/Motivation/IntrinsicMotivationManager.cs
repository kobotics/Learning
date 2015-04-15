// ------------------------------------------
// IntrinsicMotivationManager.cs, Learning.IMRL
//
// Created by Pedro Sequeira, 2014/2/20
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using Learning.Domain.Agents;
using Learning.Domain.Managers.Motivation;
using PS.Utilities.Math;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Learning.IMRL.Domain.Managers.Motivation
{
    public abstract class IntrinsicMotivationManager : EnvironmentMotivationManager, IIntrinsicMotivationManager
    {
        protected IntrinsicMotivationManager(ISituatedAgent agent)
            : base(agent)
        {
            this.IntrinsicReward = new StatisticalQuantity();
        }

        #region IIntrinsicMotivationManager Members

        public StatisticalQuantity IntrinsicReward { get; protected set; }

        public override void Update()
        {
            base.Update();
            var stm = this.Agent.ShortTermMemory;
            this.IntrinsicReward.Value =
                this.GetIntrinsicReward(stm.PreviousState.ID, stm.CurrentAction.ID, stm.CurrentState.ID);
        }

        public override void Dispose()
        {
            base.Dispose();
            this.IntrinsicReward.Dispose();
        }

        public override void Reset()
        {
            base.Reset();
            this.IntrinsicReward = new StatisticalQuantity();
        }

        public abstract double GetIntrinsicReward(uint prevState, uint action, uint nextState);

        public override double GetReward(uint stateID, uint actionID, uint nextStateID)
        {
            return this.GetIntrinsicReward(stateID, actionID, nextStateID);
        }

        #endregion

        public abstract DenseVector GetRewardFeatures(uint prevState, uint action, uint nextState);
    }
}