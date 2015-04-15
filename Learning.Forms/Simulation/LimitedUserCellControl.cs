// ------------------------------------------
// LimitedUserCellControl.cs, Learning.Forms
//
// Created by Pedro Sequeira, 2013/12/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System.Drawing;
using Learning.Domain.Cells;

namespace Learning.Forms.Simulation
{
    public class LimitedUserCellControl : CellControl
    {
        public LimitedUserCellControl(Cell cell, EnvironmentControl environmentControl, int cellSize, bool cellView)
            : base(cell, environmentControl, cellSize)
        {
            this.CellView = cellView;
        }

        public bool CellView { get; protected set; }

        public override void InitElements()
        {
            base.InitElements();
            if (this.CellView)
                this.Location = new Point(0, 0);
        }
    }
}