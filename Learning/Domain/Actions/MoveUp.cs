// ------------------------------------------
// MoveUp.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using Learning.Domain.Agents;
using Learning.Domain.Cells;

namespace Learning.Domain.Actions
{
    public class MoveUp : Move
    {
        public MoveUp(string id, ICellAgent agent) : base(id, agent)
        {
        }

        protected override Cell GetNextCell()
        {
            return (this.Agent.Cell.YCoord == 0)
                ? null
                : this.Agent.Environment.Cells[this.Agent.Cell.XCoord, this.Agent.Cell.YCoord - 1];
        }
    }
}