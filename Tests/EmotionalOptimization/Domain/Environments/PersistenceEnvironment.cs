// ------------------------------------------
// PersistenceEnvironment.cs, Learning.Tests.EmotionalOptimization
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using Learning.Domain.Actions;
using Learning.Domain.Agents;
using Learning.Domain.Cells;
using Learning.Domain.States;
using Learning.IMRL.Emotions.Testing;

namespace Learning.Tests.EmotionalOptimization.Domain.Environments
{
    [Serializable]
    public class PersistenceEnvironment : FixedStartRabbitHareEnvironment
    {
        protected uint currentNumMoveActions;
        protected uint numMoveActionsRequired;
        protected double passFenceReward;
        protected Cell[] possibleFenceCells;
        protected Cell previousAgentCell;

        public PersistenceEnvironment()
        {
            //creates obstacle
            this.Fence = new CellElement
                         {
                             IdToken = "fence",
                             Reward = 0f,
                             Visible = true,
                             HasSmell = false,
                             Walkable = true,
                             ImagePath = "../../../../bin/resources/fence.png",
                         };
            this.AutoEat = true;
        }

        protected new IEmotionalTestsConfig TestsConfig
        {
            get { return base.TestsConfig as IEmotionalTestsConfig; }
        }

        protected double FenceReward
        {
            get
            {
                return 0;
                //return -this.rand.Next(10001)/10000f;
                //return -0.03f*(1f-((double)this.Agent.LongTermMemory.TimeStep / this.maxSteps));
            }
        }

        public CellElement Fence { get; protected set; }

        public override void Init()
        {
            base.Init();
            this.numMoveActionsRequired = 0;
        }

        public override void Dispose()
        {
            base.Dispose();
            this.Fence.Dispose();
        }

        public override void Reset()
        {
            base.Reset();

            //places fence in one of possible cells (diff from previous)
            this.Fence.Cell = this.GetRandomCell(null, this.possibleFenceCells);
        }

        public override void Update()
        {
            //hare is automatically accessible during the exploration phase or 
            //if the agent is in the upper part of the environment (past the fence)
            this.Fence.Walkable =
                this.ExplorationPhase() || 
                (this.Agent.Cell.YCoord < this.Fence.Cell.YCoord) ||
                (this.numMoveActionsRequired == 0);

            this.passFenceReward = 0;

            //test if agent is trying to move past the electric fence
            if (this.AgentTriedToPassFence())
            {
                this.passFenceReward = this.FenceReward; //this.Fence.Reward;
                if (++this.currentNumMoveActions >= this.numMoveActionsRequired)
                {
                    //if number of tries is sufficient, hare becomes accessible
                    this.Fence.Walkable = true;
                }
            }
            else
            {
                //resets current move-up counter
                this.currentNumMoveActions = 0;
            }

            this.previousAgentCell = this.Agent.Cell;
            base.Update();

            //increments number of move actions needed to pass the fence next time
            var stm = this.Agent.ShortTermMemory;
            var agentAteHare = this.AgentFinishedTask(this.Agent, stm.PreviousState, stm.CurrentAction) &&
                               ((IStimuliState) stm.PreviousState).Sensations.Contains(this.Hare.IdToken);

            if (agentAteHare && (this.numMoveActionsRequired < this.TestsConfig.MaxMoveActionsRequired))
                this.numMoveActionsRequired++;
        }

        public override double GetAgentReward(IAgent agent, IState state, IAction action)
        {
            return base.GetAgentReward(agent, state, action) + this.passFenceReward;
        }

        public override void CreateCells(uint rows, uint cols, string configFile)
        {
            base.CreateCells(rows, cols, configFile);

            //creates array of possible fence cell locations
            this.possibleFenceCells = new[] {this.Cells[1, 2]}; //, this.Cells[1, 1], this.Cells[2, 1]};
        }

        protected virtual bool AgentTriedToPassFence()
        {
            if (this.previousAgentCell == null) return false;

            var action = this.Agent.ShortTermMemory.CurrentAction;
            var neighbourAgentCells = this.previousAgentCell.NeighbourCells;

            //checks previous agent action (must be a move action) and agent must be next to the fence
            return (action != null) && (this.previousAgentCell.YCoord <= 3) &&
                   ((this.Fence.Cell.Equals(neighbourAgentCells[Cell.UP_DIR_STR]) && (action is MoveUp)) ||
                    (this.Fence.Cell.Equals(neighbourAgentCells[Cell.RIGHT_DIR_STR]) && (action is MoveRight)));
        }
    }
}