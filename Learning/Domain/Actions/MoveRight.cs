// ------------------------------------------
// MoveRight.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using Learning.Domain.Agents;
using Learning.Domain.Cells;

namespace Learning.Domain.Actions
{
    public class MoveRight : Move
    {
        public MoveRight(string id, CellAgent agent) : base(id, agent)
        {
        }

        protected override Cell GetNextCell()
        {
            return (this.Agent.Cell.XCoord == this.Agent.Environment.Cols - 1)
                ? null
                : this.Agent.Environment.Cells[this.Agent.Cell.XCoord + 1, this.Agent.Cell.YCoord];
        }
    }
}