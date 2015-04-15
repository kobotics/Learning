// ------------------------------------------
// ITestProfile.cs, Learning
//
// Created by Pedro Sequeira, 2013/12/18
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using Learning.Domain.Agents;
using Learning.Domain.Environments;

namespace Learning.Testing.Config.Scenarios
{
    /// <summary>
    ///   Configuration for a specific test. Allows creation of agents and environment for a test.
    /// </summary>
    public interface IScenario
    {
        ITestsConfig TestsConfig { get; }
        string TestMeasuresFilePath { get; set; }
        string EnvironmentConfigFile { get; set; }
        string FilePath { get; set; }
        uint MaxStates { get; set; }
        IAgent CreateAgent();
        IEnvironment CreateEnvironment();
        IScenario Clone();
        IScenario Clone(uint numSimulations, uint numSamples);
    }
}