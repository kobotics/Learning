// ------------------------------------------
// Manager.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using Learning.Domain.Agents;
using Learning.Testing.Config;
using Learning.Testing.Config.Scenarios;
using PS.Utilities.Serialization;

namespace Learning.Domain.Managers
{
    [Serializable]
    public abstract class Manager : XmlResource, IManager
    {
        protected Manager(IAgent agent)
        {
            this.Agent = agent;
        }

        protected IScenario Scenario
        {
            get { return this.Agent.Scenario; }
        }

        protected ITestsConfig TestsConfig
        {
            get { return this.Agent.Scenario.TestsConfig; }
        }

        #region IManager Members

        public IAgent Agent { get; private set; }

        public abstract void Update();
        public abstract void Reset();

        public override void InitElements()
        {
        }

        #endregion

        public abstract void PrintResults(string path);
    }
}