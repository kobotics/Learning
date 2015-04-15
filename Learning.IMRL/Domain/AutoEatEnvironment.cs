// ------------------------------------------
// AutoEatEnvironment.cs, Learning.IMRL
//
// Created by Pedro Sequeira, 2012/10/17
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using Learning.Domain.Actions;
using Learning.Domain.Agents;
using Learning.Domain.States;

namespace Learning.IMRL.Domain
{
    [Serializable]
    public class AutoEatEnvironment : IREnvironment
    {
        public AutoEatEnvironment()
        {
            this.AutoEat = true;
        }

        public bool AutoEat { get; set; }

        public override bool AgentFinishedTask(IAgent agent, IState state, IAction action)
        {
            return this.AutoEat
                       ? (state is IStimuliState) && ((IStimuliState) state).Sensations.Contains(this.Hare.IdToken)
                       : base.AgentFinishedTask(agent, state, action);
        }

        protected override void UpdateCellDebugging()
        {
            if (!this.DebugMode) return;

            //stores agent's current cell
            var curCell = this.Agent.Cell;

            //for each cell in the environment
            foreach (var cell in this.Cells)
            {
                //puts the agent in that cell and simulates perception
                this.Agent.Cell = cell;
                this.Agent.PerceptionManager.Update();

                //gets resulting state and respective value and extrinsic pred error
                var state = this.Agent.LongTermMemory.GetUpdatedCurrentState();
                cell.StateValue.IdToken = this.Agent.LongTermMemory.GetMaxStateActionValue(state.ID).ToString("0.00");
            }
            this.Agent.Cell = curCell;
        }
    }
}