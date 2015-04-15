// ------------------------------------------
// SingleScenario.cs, Learning
// 
// Created by Pedro Sequeira, 2015/03/30
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using Learning.Domain.Agents;
using Learning.Domain.Environments;
using Learning.Testing.Simulations;
using PS.Utilities.Serialization;

namespace Learning.Testing.Config.Scenarios
{
    [Serializable]
    public class SingleScenario : IFitnessScenario
    {
        private readonly IAgent _baseAgent;
        private readonly IEnvironment _baseEnvironment;

        public SingleScenario(IAgent baseAgent, IEnvironment baseEnvironment, ITestsConfig testsConfig)
        {
            this._baseAgent = baseAgent;
            this._baseEnvironment = baseEnvironment;
            this.TestsConfig = testsConfig;

            //defaults, can be change in TestsConfig
            this.AgentFitnessFunction = new CumulativeFitnessFunction();
            this.FitnessText = "Fitness";
        }

        #region IFitnessScenario Members

        public ITestsConfig TestsConfig { get; set; }
        public string TestMeasuresFilePath { get; set; }
        public string EnvironmentConfigFile { get; set; }
        public string FilePath { get; set; }
        public uint MaxStates { get; set; }
        public string FitnessText { get; set; }

        public IAgent CreateAgent()
        {
            return this._baseAgent.CreateNew();
        }

        public IEnvironment CreateEnvironment()
        {
            return this._baseEnvironment.CreateNew();
        }

        public IScenario Clone()
        {
            return this.CloneJson();
        }

        public IScenario Clone(uint numSimulations, uint numSamples)
        {
            var profile = this.Clone();
            profile.TestsConfig.NumSimulations = numSimulations;
            profile.TestsConfig.NumSamples = numSamples;
            return profile;
        }

        public IAgentFitnessFunction AgentFitnessFunction { get; set; }

        #endregion
    }
}