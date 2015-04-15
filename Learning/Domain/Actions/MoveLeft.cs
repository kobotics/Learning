// ------------------------------------------
// MoveLeft.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using Learning.Domain.Agents;
using Learning.Domain.Cells;

namespace Learning.Domain.Actions
{
    public class MoveLeft : Move
    {
        public MoveLeft(string id, CellAgent agent) : base(id, agent)
        {
        }

        protected override Cell GetNextCell()
        {
            return (this.Agent.Cell.XCoord == 0)
                ? null
                : this.Agent.Environment.Cells[this.Agent.Cell.XCoord - 1, this.Agent.Cell.YCoord];
        }
    }
}