// ------------------------------------------
// StateRelevanceManager.cs, Learning.IMRL
//
// Created by Pedro Sequeira, 2013/3/26
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using Learning.Domain.Managers;
using Learning.Domain.Managers.Perception;
using Learning.Domain.States;
using Learning.IMRL.Domain.Agents;

namespace Learning.IMRL.Domain.Managers
{
    [Serializable]
    public class StateRelevanceManager : Manager
    {
        public StateRelevanceManager(IIMRLAgent agent) : base(agent)
        {
        }

        public new IIMRLAgent Agent
        {
            get { return base.Agent as IIMRLAgent; }
        }

        public override void Update()
        {
            var prevState = this.Agent.ShortTermMemory.PreviousState;
            if (prevState != null)
                this.VerifyTargetState(prevState);
        }

        public double GetDistanceToGoal(uint stateID)
        {
            if (!(this.Agent.PerceptionManager is CellPerceptionManager))
                return double.MaxValue;

            //tries to get cell corresponding to given state
            var state = this.Agent.LongTermMemory.GetState(stateID);
            var stateCell = ((CellPerceptionManager) this.Agent.PerceptionManager).GetCellFromState(state);

            //updates relevance of state
            this.VerifyTargetState(state);

            //returns distance from given cell to target cell
            return stateCell == null
                       ? double.MaxValue
                       : this.Agent.Environment.GetDistanceToTargetCell(stateCell);
        }

        protected void VerifyTargetState(IState state)
        {
            //verifies state and manager
            if ((state == null) || !(this.Agent.PerceptionManager is CellPerceptionManager))
                return;

            //gets current rwd and compares with best rwd found
            var stateRelevanceValue = this.GetStateRelevanceValue(state.ID);
            if (!this.IsMostRelevantValue(stateRelevanceValue)) return;

            //if better, updates target cells on the environment
            var stateCell = ((CellPerceptionManager) this.Agent.PerceptionManager).GetCellFromState(state);
            this.Agent.Environment.TargetCells.Clear();
            this.Agent.Environment.TargetCells.Add(stateCell);
        }

        protected bool IsMostRelevantValue(double curRelevanceValue)
        {
            return curRelevanceValue.Equals(1f);
            //return curRelevanceValue >= this.BestRewardFound;
        }

        protected double GetStateRelevanceValue(uint state)
        {
            if (state.Equals(uint.MaxValue)) return 0;

            var extrinsicLTM = this.Agent.ExtrinsicLTM;
            var maxStateValue = extrinsicLTM.MaxStateValue;
            var minStateValue = extrinsicLTM.MinStateValue;
            var stateValue = extrinsicLTM.GetMaxStateActionValue(state);
            var relevanceValue = (stateValue - minStateValue)/(maxStateValue - minStateValue);

            return double.IsNaN(relevanceValue) ? 0 : relevanceValue;
            //return this.Agent.MotivationManager.ExtrinsicReward.Value;
        }

        public override void Reset()
        {
            this.Agent.Environment.TargetCells.Clear();
        }


        public override void Dispose()
        {
        }

        public override void PrintResults(string path)
        {
        }
    }
}