// ------------------------------------------
// NeighbourCellPerceptionManager.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using Learning.Domain.Agents;
using Learning.Domain.Cells;

namespace Learning.Domain.Managers.Perception
{
    [Serializable]
    public class NeighbourCellPerceptionManager : PerceptionManager
    {
        protected const string DOWN_SENSATION = "Down";
        protected const string LEFT_SENSATION = "Left";
        protected const string RIGHT_SENSATION = "Right";
        protected const string UP_SENSATION = "Up";

        public NeighbourCellPerceptionManager(CellAgent agent) : base(agent)
        {
        }

        public new CellAgent Agent
        {
            get { return base.Agent as CellAgent; }
        }

        public override void PrintResults(string path)
        {
        }

        protected override void AddInternalSensations()
        {
        }

        protected override void AddExternalSensations()
        {
            //adds cell contents
            foreach (var cellElement in this.Agent.Cell.Elements)
                if (!cellElement.Equals(this.Agent) && !(cellElement is InfoCellElement))
                    this.CurrentSensations.Add(cellElement.IdToken);

            //adds neighbour sensations
            this.AddNeighbourSensations();
        }

        protected virtual void AddNeighbourSensations()
        {
            var upCell = (this.Agent.Cell.YCoord > 0)
                             ? this.Agent.Environment.Cells[this.Agent.Cell.XCoord, this.Agent.Cell.YCoord - 1]
                             : null;
            var downCell = (this.Agent.Cell.YCoord < this.Agent.Environment.Rows - 1)
                               ? this.Agent.Environment.Cells[this.Agent.Cell.XCoord, this.Agent.Cell.YCoord + 1]
                               : null;
            var leftCell = (this.Agent.Cell.XCoord > 0)
                               ? this.Agent.Environment.Cells[this.Agent.Cell.XCoord - 1, this.Agent.Cell.YCoord]
                               : null;
            var rightCell = (this.Agent.Cell.XCoord < this.Agent.Environment.Cols - 1)
                                ? this.Agent.Environment.Cells[this.Agent.Cell.XCoord + 1, this.Agent.Cell.YCoord]
                                : null;

            this.AddNeighbourCell(upCell, UP_SENSATION);
            this.AddNeighbourCell(downCell, DOWN_SENSATION);
            this.AddNeighbourCell(leftCell, LEFT_SENSATION);
            this.AddNeighbourCell(rightCell, RIGHT_SENSATION);
        }

        protected virtual void AddNeighbourCell(Cell neighbourCell, string directionSensation)
        {
            if (neighbourCell == null)
                this.CurrentSensations.Add("see Wall " + directionSensation);
            else
                foreach (var cellContent in neighbourCell.Elements)
                    if (cellContent.Visible)
                        this.CurrentSensations.Add("see " + cellContent.IdToken + " " + directionSensation);
        }
    }
}