// ------------------------------------------
// Move.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using Learning.Domain.Agents;
using Learning.Domain.Cells;
using MathNet.Numerics.Random;

namespace Learning.Domain.Actions
{
    public abstract class Move : CellAction
    {
        protected const double HIT_REWARD = -5;
        protected const double MOVE_REWARD = -1;
        protected static readonly Random Random = new WH2006();

        protected Move(string id, ICellAgent agent) : base(id, agent)
        {
        }

        public new ICellAgent Agent
        {
            get { return base.Agent as ICellAgent; }
        }

        public override double Execute()
        {
            var totalReward = MOVE_REWARD;

            //gets move probability
            var moveProb = Random.NextDouble();
            if (!(moveProb <= this.Agent.Scenario.TestsConfig.ActionAccuracy)) 
                return totalReward + base.Execute();

            //gets next cell and moves accordingly if possible
            var nextCell = this.GetNextCell();
            if ((nextCell == null) || !this.IsWalkable(nextCell))
                totalReward += HIT_REWARD;
            else
                this.Agent.Cell = nextCell;

            return totalReward + base.Execute(); 
        }

        protected bool IsWalkable(Cell cell)
        {
            return this.Agent.Environment.IsWalkable(cell);
        }

        protected abstract Cell GetNextCell();
    }
}