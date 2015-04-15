// ------------------------------------------
// HungerThirstEnvironment.cs, Learning.Tests.EmotionalOptimization
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
using Learning.IMRL.Domain;

namespace Learning.Tests.EmotionalOptimization.Domain.Environments
{
    [Serializable]
    public class HungerThirstEnvironment : AutoEatEnvironment
    {
        private const int THIRST_PROB = 1;

        public HungerThirstEnvironment()
        {
            this.AutoEat = true;
        }

        public CellElement Water { get; protected set; }

        private const int MAX_WATER_LEVEL = 2;

        public override void Init()
        {
            base.Init();

            //creates water element
            this.Water = new CellElement
                             {
                                 IdToken = "water",
                                 Description = "water",
                                 Reward = 1f,
                                 Visible = true,
                                 Walkable = true,
                                 HasSmell = true,
                                 ImagePath = "../../../../bin/resources/water.png",
                             };
        }

        public override void Update()
        {
            base.Update();

            var state = this.Agent.ShortTermMemory.PreviousState;
            if ((state is IStimuliState) && ((IStimuliState) state).Sensations.Contains(this.Water.IdToken) && this.WaterLevel < MAX_WATER_LEVEL)
                this.WaterLevel = this.WaterLevel + 1;
            else if(this.rand.Next(10) < THIRST_PROB && this.WaterLevel > 0){
                this.WaterLevel = this.WaterLevel-1;
            }
            else if(((state is IStimuliState) &&  ((IStimuliState) state).Sensations.Contains(this.Hare.IdToken))){
                previousWaterLevel = this.WaterLevel;
                this.WaterLevel = 0;
            }            
        
        }

        public override bool AgentFinishedTask(IAgent agent, IState state, IAction action)
        {
            
            return previousWaterLevel != 0 && base.AgentFinishedTask(agent, state, action);
            //return base.AgentFinishedTask(agent, state, action);
        }

        public override void Reset()
        {
            //agent starts from top-left position, only in the beggining
            if ((this.Agent == null) || (this.Agent.Cell != null)) return;

            this.Agent.Environment = this;

            if (this.TestsConfig.RandStart)
            {
                //places prey, agent and water in random possible cells
                this.Agent.Cell = this.Cells[5, 3];
                this.Hare.Cell = this.GetRandomCell(new[] {this.Agent.Cell}, this.possiblePreyCells);
                this.Water.Cell = this.GetRandomCell(new[] {this.Hare.Cell, this.Agent.Cell}, this.possiblePreyCells);
            }
            else
            {
                this.Agent.Cell = this.Cells[5, 3]; //middle position
                this.Hare.Cell = this.Cells[5, 5];
                this.Water.Cell = this.Cells[1, 1];
            }
        }

        public override void CreateCells(uint rows, uint cols, string configFile)
        {
            base.CreateCells(rows, cols, configFile);

            //creates array of possible prey/water cell locations (corners)
            this.possiblePreyCells =
                new[] {this.Cells[1, 1], this.Cells[1, 5], this.Cells[5, 1], this.Cells[5, 5]};
        }
    }
}