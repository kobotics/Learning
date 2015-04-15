// ------------------------------------------
// SmellPerceptionManager.cs, Learning.Emotions
//
// Created by Pedro Sequeira, 2012/10/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using System.Linq;
using Learning.Domain.Agents;
using Learning.Domain.Cells;
using Learning.Domain.Managers.Perception;

namespace Learning.IMRL.Emotions.Domain.Managers
{
    [Serializable]
    public class SmellPerceptionManager : PerceptionManager
    {
        public const string SMELL_STR = "smell";

        public SmellPerceptionManager(CellAgent agent) : base(agent)
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
            //adds cell contents (including cell location)
            foreach (var cellElement in this.Agent.Cell.Elements.Where(
                cellElement => !cellElement.Equals(this.Agent) && !(cellElement is InfoCellElement)))
                this.CurrentSensations.Add(cellElement.IdToken);

            //adds smell sensations for neighbour cells
            this.AddNeighbourSensations();
        }

        protected virtual void AddNeighbourSensations()
        {
            foreach (var neighbourCell in this.Agent.Cell.NeighbourCells.Values)
                this.AddNeighbourSensation(neighbourCell);
        }


        protected virtual void AddNeighbourSensation(Cell neighbourCell)
        {
            //adds smell sensations for neighbour cells with smell
            if (neighbourCell != null)
                foreach (var cellElement in neighbourCell.Elements.Where(cellElement => cellElement.HasSmell))
                    this.CurrentSensations.Add(string.Format("{0}-{1}", SMELL_STR, cellElement.IdToken));
        }
    }
}