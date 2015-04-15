// ------------------------------------------
// LairsPerceptionManager.cs, Learning.Tests.EmotionalOptimization
//
// Created by Pedro Sequeira, 2013/12/3
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using Learning.Domain.Cells;
using Learning.Tests.EmotionalOptimization.Domain.Agents;
using Learning.Domain.Managers.Perception;

namespace Learning.Tests.EmotionalOptimization.Domain.Managers
{
	public class LairsPerceptionManager : CellPerceptionManager
	{
		public LairsPerceptionManager (ILairsAgent agent)
            : base (agent)
		{
		}

		public new ILairsAgent Agent {
			get { return base.Agent as ILairsAgent; }
		}

		protected override void AddExternalSensations ()
		{
			//adds cell contents (including cell location), but doesn't see neighbour cells
			foreach (var cellElement in this.Agent.Cell.Elements)
				if (!cellElement.Equals (this.Agent) && !(cellElement is InfoCellElement) && !(cellElement is RabbitLair))
					this.CurrentSensations.Add (cellElement.IdToken);
		}

		protected override void AddInternalSensations ()
		{
			//adds states of both lairs
			this.AddLairSensations (this.Agent.Environment.Lair1);
			this.AddLairSensations (this.Agent.Environment.Lair2);
		}

		protected void AddLairSensations (RabbitLair lair)
		{
			//adds lair state
			this.CurrentSensations.Add ((lair.State != LairState.Rabbit ? lair.IdToken + "-" : string.Empty) + lair.State);
		}
	}
}