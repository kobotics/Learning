// ------------------------------------------
// CellAction.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System.Linq;
using Learning.Domain.Agents;
using Learning.Domain.Cells;

namespace Learning.Domain.Actions
{
    public abstract class CellAction : Action
    {
        protected CellAction(string id, ICellAgent agent):base(id, agent)
        {
        }

        public override double Execute()
        {
            return
                ((ICellAgent) this.Agent).Cell.Elements.Where(element => element is CellElement).Sum(
                    cellContent => ((CellElement) cellContent).Reward);
        }
    }
}