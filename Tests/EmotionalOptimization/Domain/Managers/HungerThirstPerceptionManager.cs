// ------------------------------------------
// HungerThirstPerceptionManager.cs, Learning.Tests.EmotionalOptimization
//
// Created by Pedro Sequeira, 2013/12/3
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using Learning.Tests.EmotionalOptimization.Domain.Agents;
using Learning.Domain.Managers.Perception;

namespace Learning.Tests.EmotionalOptimization.Domain.Managers
{
	public class HungerThirstPerceptionManager : CellPerceptionManager
	{
		public HungerThirstPerceptionManager (IHungerThirstAgent agent)
            : base (agent)
		{
		}

		public new IHungerThirstAgent Agent {
			get { return base.Agent as IHungerThirstAgent; }
		}

		protected override void AddInternalSensations ()
		{
			this.CurrentSensations.Add (this.Agent.Environment.WaterLevel.ToString());
		}
	}
}