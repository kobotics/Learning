// ------------------------------------------
// SingleStatePerceptionManager.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using Learning.Domain.Agents;

namespace Learning.Domain.Managers.Perception
{
    [Serializable]
    public class SingleStatePerceptionManager : PerceptionManager
    {
        private const string THE_STATE_ID_STR = "State";

        public SingleStatePerceptionManager(IAgent agent) : base(agent)
        {
        }

        public override void PrintResults(string path)
        {
        }

        protected override void AddInternalSensations()
        {
        }

        protected override void AddExternalSensations()
        {
            //only a single state
            this.CurrentSensations.Add(THE_STATE_ID_STR);
        }
    }
}