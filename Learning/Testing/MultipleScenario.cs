// ------------------------------------------
// MultipleScenarioProfile.cs, Learning
// 
// Created by Pedro Sequeira, 2013/12/09
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using System.Collections.Generic;
using Learning.Domain.Agents;
using Learning.Domain.Environments;
using Learning.Testing.Config;
using Learning.Testing.Config.Scenarios;
using PS.Utilities.Serialization;

namespace Learning.Testing
{
    [Serializable]
    public class MultipleScenario : IScenario
    {
        private int _curProfileIdx;

        public MultipleScenario(IList<IScenario> testProfiles)
        {
            this.TestProfiles = testProfiles;
        }

        public IList<IScenario> TestProfiles { get; private set; }

        public IScenario CurrentScenario
        {
            get { return this.TestProfiles[this._curProfileIdx]; }
            set { this._curProfileIdx = this.TestProfiles.IndexOf(value); }
        }

        public bool AutoUpdate { get; set; }

        #region IScenarioProfile Members

        public ITestsConfig TestsConfig
        {
            get { return this.CurrentScenario.TestsConfig; }
            set { }
        }

        public string TestMeasuresFilePath { get; set; }
        public string FilePath { get; set; }

        public string EnvironmentConfigFile
        {
            get { return this.CurrentScenario.EnvironmentConfigFile; }
            set { }
        }

        public uint MaxStates
        {
            get { return this.CurrentScenario.MaxStates; }
            set { }
        }

        public IAgent CreateAgent()
        {
            return this.CurrentScenario.CreateAgent();
        }

        public IEnvironment CreateEnvironment()
        {
            return this.CurrentScenario.CreateEnvironment();
        }

        public IScenario Clone()
        {
            return this.CloneJson();
        }

        public IScenario Clone(uint numSimulations, uint numSamples)
        {
            var clone = this.Clone();
            clone.TestsConfig.NumSimulations = numSimulations;
            clone.TestsConfig.NumSamples = numSamples;
            return clone;
        }

        #endregion

        public virtual void Update()
        {
            if (this.AutoUpdate)
                this._curProfileIdx = this._curProfileIdx == this.TestProfiles.Count - 1 ? 0 : this._curProfileIdx + 1;
        }
    }
}