// ------------------------------------------
// CellPerceptionManager.cs, Learning
//
// Created by Pedro Sequeira, 2013/12/3
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using Learning.Domain.Agents;
using Learning.Domain.Cells;
using Learning.Domain.States;

namespace Learning.Domain.Managers.Perception
{
    [Serializable]
    public class CellPerceptionManager : PerceptionManager
    {
        public CellPerceptionManager(ICellAgent agent) : base(agent)
        {
        }

        public new ICellAgent Agent
        {
            get { return base.Agent as CellAgent; }
        }

        protected override void AddInternalSensations()
        {
        }

        protected override void AddExternalSensations()
        {
            //adds cell contents (including cell location)
            foreach (var cellElement in this.Agent.Cell.Elements)
                if ((cellElement.Visible || (cellElement is Location))
                    && !cellElement.Equals(this.Agent) && !(cellElement is InfoCellElement))
                    this.CurrentSensations.Add(cellElement.IdToken);
        }

        public override void PrintResults(string path)
        {
        }

        public virtual Cell GetCellFromState(IState state)
        {
            if (!(state is StringState)) return null;

            //this.XCoord + "," + this.YCoord
            string[] sensations;
            foreach (var sensation in ((IStimuliState) state).Sensations)
                if (sensation.Contains(",") && ((sensations = sensation.Split(',')).Length == 2))
                    return this.Agent.Environment.Cells[int.Parse(sensations[0]), int.Parse(sensations[1])];
            return null;
        }

        protected void DetectInCorridor(string elementID, string sensation)
        {
            var agentX = this.Agent.Cell.XCoord;
            var agentY = this.Agent.Cell.YCoord;
            var env = this.Agent.Environment;

            if (env.DetectInRightCorridor(agentX, agentY, elementID))
                this.CurrentSensations.Add(string.Format("{0}-r", sensation));
            if (env.DetectInLeftCorridor(agentX, agentY, elementID))
                this.CurrentSensations.Add(string.Format("{0}-l", sensation));
            if (env.DetectInUpCorridor(agentX, agentY, elementID))
                this.CurrentSensations.Add(string.Format("{0}-u", sensation));
            if (env.DetectInDownCorridor(agentX, agentY, elementID))
                this.CurrentSensations.Add(string.Format("{0}-d", sensation));
        }
    }
}