// ------------------------------------------
// PerceptionManager.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using System.Collections.Generic;
using Learning.Domain.Agents;

namespace Learning.Domain.Managers.Perception
{
    [Serializable]
    public abstract class PerceptionManager : Manager, IPerceptionManager
    {
        protected PerceptionManager(IAgent agent) : base(agent)
        {
            this.CurrentSensations = new HashSet<string>();
        }

        public HashSet<string> CurrentSensations { get; protected set; }

        public override void Update()
        {
            this.CurrentSensations.Clear();

            this.AddExternalSensations();
            this.AddInternalSensations();
        }

        public override void Reset()
        {
            this.CurrentSensations.Clear();
        }

        public override void Dispose()
        {
            this.CurrentSensations.Clear();
        }

        protected abstract void AddInternalSensations();
        protected abstract void AddExternalSensations();
    }
}