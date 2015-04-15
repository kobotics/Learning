// ------------------------------------------
// CellState.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using Learning.Domain.Cells;

namespace Learning.Domain.States
{
    public class CellState : State
    {
        public CellState(Cell cell)
        {
            this.Cell = cell;
        }

        public Cell Cell { get; protected set; }
    }
}