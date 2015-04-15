// ------------------------------------------
// PacmanPerceptionManager.cs, Learning.Tests.EmotionalOptimization
//
// Created by Pedro Sequeira, 2013/12/3
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using Learning.Domain.Agents;
using Learning.Domain.Cells;
using Learning.Domain.Managers.Perception;
using Learning.Tests.EmotionalOptimization.Domain.Environments;

namespace Learning.Tests.EmotionalOptimization.Domain.Managers
{
    public class PacmanPerceptionManager : CellPerceptionManager
    {
        public PacmanPerceptionManager(ICellAgent agent) : base(agent)
        {
        }

        protected override void AddExternalSensations()
        {
            //agent sees current location and other visible elements
            this.CurrentSensations.Add(this.Agent.Cell.CellLocation.IdToken);
            foreach (var cellElement in this.Agent.Cell.Elements)
                if (cellElement.Visible && !cellElement.Equals(this.Agent) && !(cellElement is InfoCellElement))
                    this.CurrentSensations.Add(cellElement.IdToken);

            //detect elements in corridors
            this.DetectInCorridor(PacmanEnvironment.GHOST_ID, PacmanEnvironment.GHOST_ID);
            this.DetectInCorridor(PacmanEnvironment.DOT_ID, PacmanEnvironment.DOT_ID);
            this.DetectInCorridor(PacmanEnvironment.BIG_DOT_ID, PacmanEnvironment.DOT_ID);
            this.DetectInCorridor(PacmanEnvironment.FINAL_GHOST_ID, PacmanEnvironment.GHOST_ID);
            this.DetectInCorridor(PacmanEnvironment.DEADLY_GHOST_ID, PacmanEnvironment.GHOST_ID);
            this.DetectInCorridor(PacmanEnvironment.WEAK_GHOST_ID, PacmanEnvironment.GHOST_ID);
            this.DetectInCorridor(PacmanEnvironment.FINAL_DOT_ID, PacmanEnvironment.DOT_ID);
        }
    }
}