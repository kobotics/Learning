// ------------------------------------------
// SingleAgentEnvironment.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using Learning.Domain.Agents;
using Learning.Domain.Managers.Behavior;

namespace Learning.Domain.Environments
{
    [Serializable]
    public abstract class SingleAgentEnvironment : Environment
    {
        protected SingleAgentEnvironment(uint rows, uint cols) : base(rows, cols)
        {
        }

        public ICellAgent Agent { get; set; }

        public override void Update()
        {
            this.UpdateCellDebugging();

            //resets environment when agent finishes task
            var agentFinishedTask = this.AgentFinishedTask(
                this.Agent, this.Agent.ShortTermMemory.PreviousState, this.Agent.ShortTermMemory.CurrentAction);

            if (agentFinishedTask) this.Reset();
        }

        protected virtual void UpdateCellDebugging()
        {
            if (!this.DebugMode) return;

            var oldCell = this.Agent.Cell;
            foreach (var cell in this.Cells)
            {
                this.Agent.Cell = cell;
                this.Agent.PerceptionManager.Update();
                var state = this.Agent.LongTermMemory.GetUpdatedCurrentState();
                cell.StateValue.IdToken = this.Agent.LongTermMemory.GetMaxStateActionValue(state.ID).ToString("0.00");
            }
            this.Agent.Cell = oldCell;
        }

        public override void Dispose()
        {
            base.Dispose();
            if (this.Agent != null) this.Agent.Dispose();
        }

        protected bool ExplorationPhase()
        {
            if (!(this.Agent.BehaviorManager is TwoPhaseBehaviorManager)) return false;

            var behaviorManager = (TwoPhaseBehaviorManager) this.Agent.BehaviorManager;
            return behaviorManager.ExplorationPhase &&
                   (this.Agent.LongTermMemory.TimeStep < behaviorManager.ExplorationSteps);
        }

        public override void Init()
        {
        }
    }
}