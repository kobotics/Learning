// ------------------------------------------
// RewardParametersManager.cs, Learning.IMRL
//
// Created by Pedro Sequeira, 2013/7/11
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System.Collections.Generic;
using Learning.Domain.Managers;
using Learning.IMRL.Domain.Agents;
using Learning.IMRL.Domain.Managers.Motivation;
using PS.Utilities.Math;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Learning.IMRL.Domain.Managers.Reward
{
    public abstract class RewardParametersManager : Manager
    {
        private const int UPDATE_STEPS_DEF = 500;
        private uint _updateSteps;
        protected uint curNumSteps;
        protected StatisticalQuantity extrinsicReward;

        protected RewardParametersManager(INORCAgent agent, uint numParams) : base(agent)
        {
            this.UpdateSteps = UPDATE_STEPS_DEF;
            this.NumParams = numParams;

            this.ResetVariables();

            this.RwdParamsStats = new List<StatisticalQuantity>();
            for (var i = 0; i < this.NumParams; i++)
                this.RwdParamsStats.Add(new StatisticalQuantity());
        }

        public uint UpdateSteps
        {
            get { return this._updateSteps; }
            set
            {
                this._updateSteps = value;
                this.extrinsicReward = new StatisticalQuantity(value);
            }
        }

        public new INORCAgent Agent
        {
            get { return base.Agent as INORCAgent; }
        }

        protected DenseVector RewardParameters
        {
            get { return ((ArrayParamMotivationManager) this.Agent.MotivationManager).RewardParameters; }
            set { ((ArrayParamMotivationManager) this.Agent.MotivationManager).RewardParameters = value; }
        }

        public List<StatisticalQuantity> RwdParamsStats { get; private set; }

        public uint NumParams { get; private set; }

        public override void Reset()
        {
        }

        public override void Dispose()
        {
            if (this.extrinsicReward != null)
                this.extrinsicReward.Dispose();

            this.RwdParamsStats.Clear();
            this.RwdParamsStats = null;
        }

        public override void Update()
        {
            //verifies params
            var prevState = this.Agent.ShortTermMemory.PreviousState;
            var action = this.Agent.ShortTermMemory.CurrentAction;
            var curState = this.Agent.ShortTermMemory.CurrentState;
            if ((prevState == null) || (action == null) || (curState == null)) return;

            //stores current extrinsic reward
            this.extrinsicReward.Value = this.Agent.MotivationManager.ExtrinsicReward.Value;

            //updates stats for each param
            for (var i = 0; i < this.NumParams; i++)
                this.RwdParamsStats[i].Value = this.RewardParameters[i];

            //NORC basic implementation, check for rwd params update after some time-interval
            if (++this.curNumSteps < this.UpdateSteps) return;

            //updates params
            this.RewardParameters = this.GetUpdatedRewardParameters(prevState.ID, action.ID, curState.ID);
            //if (this.RewardParameters.Contains(double.NaN))
            this.NormalizeParams();

            //if (this.RewardParameters.Contains(double.NaN))
            //{
            //    int i = 0;
            //    i++;
            //}

            this.ResetVariables();
        }

        protected void NormalizeParams()
        {
            for (var i = 0; i < this.RewardParameters.Count; i++)
                if (this.RewardParameters[i] > 1)
                    this.RewardParameters[i] = 1;
                else if (this.RewardParameters[i] < -1)
                    this.RewardParameters[i] = -1;
        }

        protected virtual void ResetVariables()
        {
            //resets algorithms variables
            this.curNumSteps = 0;
            this.extrinsicReward.Dispose();
            this.extrinsicReward = new StatisticalQuantity(this.UpdateSteps);
        }

        protected abstract DenseVector GetUpdatedRewardParameters(uint prevStateID, uint actionID, uint nextStateID);
    }
}