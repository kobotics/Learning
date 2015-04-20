// ------------------------------------------
// IREnvironment.cs, Learning.IMRL
//
// Created by Pedro Sequeira, 2013/2/6
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using Learning.Domain.Actions;
using Learning.Domain.Agents;
using Learning.Domain.Cells;
using Learning.Domain.Environments;
using Learning.Domain.States;
using Learning.IMRL.Domain.Agents;

namespace Learning.IMRL.Domain
{
    [Serializable]
    public class IREnvironment : SingleAgentEnvironment
    {
        protected const int WORLD_SIZE = 3;

        //public string wPenaltyType = "linear";
        //public double wPenaltyParam = 1;
        //public double wPenaltyAlpha = 1.0002;
        //public string wPenaltyScale = "exp";
        private double PATH_COST = 0.1;


        protected Cell[] possiblePreyCells;

        public IREnvironment() : base(3, 3)
        {
            //creates food prey hare
            this.Hare = new CellElement
                            {
                                IdToken = "food",
                                Description = "food",
                                Reward = 10f,
                                Visible = true,
                                Walkable = true,
                                HasSmell = true,
                                ImagePath = "../../../../bin/resources/hare.png",
                            };
        }

        public CellElement Hare { get; protected set; }

        public new IRAgent Agent
        {
            get { return base.Agent as IRAgent; }
            set { base.Agent = value; }
        }

        public override void Reset()
        {
            //agent starts from top-left position, only in the beggining
            if ((this.Agent != null) && (this.Agent.Cell == null))
            {
                this.Agent.Environment = this;
                this.Agent.Cell = this.Cells[1, 1];
            }

            //places prey in random right-cell
            this.Hare.Cell = this.GetRandomCell(new[] {this.Hare.Cell}, this.possiblePreyCells);
        }

        public override double GetAgentReward(IAgent agent, IState state, IAction action)
        {
            var temp = this.AgentFinishedTask(agent, state, action) ? 
                (Math.Max(0.01,this.Hare.Reward-excessWaterPenalty(Global.wPenaltyType,Global.wPenaltyParam,this.previousWaterLevel,Global.wPenaltyAlpha,Global.wPenaltyScale))) : 0;
            return temp;
        }

        private double excessWaterPenalty(string PenaltyType, double param, int wLevel, double alpha, string PenaltyScale)
        {
            double result = 0;
            if (PenaltyType.Equals("linear"))
                result= param * wLevel;
            else if (PenaltyType.Equals("none"))
                result= 0;

            if (PenaltyScale.Equals("exp"))
                result = result * (1 - Math.Pow(alpha, -this.timeStepCounter));

            return result;
        }

        public override bool AgentFinishedTask(IAgent agent, IState state, IAction action)
        {
            //task ends when agent eats the prey
            return (action != null) && (action is Eat) && (state != null) &&
                   ((IStimuliState) state).Sensations.Contains(this.Hare.IdToken);
        }

        public override void CreateCells(uint rows, uint cols, string configFile)
        {
            base.CreateCells(rows, cols, configFile);

            //creates array of possible prey cell locations
            this.possiblePreyCells = new[] {this.Cells[3, 1], this.Cells[3, 3], this.Cells[3, 5]};
        }

        public override void Init()
        {
        }
    }
}