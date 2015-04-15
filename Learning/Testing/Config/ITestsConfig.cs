// ------------------------------------------
// ITestsConfig.cs, Learning
// 
// Created by Pedro Sequeira, 2014/03/10
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using System.Collections.Generic;
using Learning.Testing.Config.Parameters;
using Learning.Testing.Config.Scenarios;
using Learning.Testing.MultipleTests;
using PS.Utilities.Parsing;

namespace Learning.Testing.Config
{
    /// <summary>
    ///     Configuration for a group of tests. General tests configuration.
    /// </summary>
    public interface ITestsConfig : ICloneable, IArgumentsParsable
    {
        List<ITestParameters> GetSpecialTestParameters(IScenario scenario);
        List<ITestParameters> GetOptimizationTestParameters();
        string GetTestName(IScenario scenario, ITestParameters testParameters);

        IOptimizationTestFactory CreateTestFactory(
            IScenario scenario, uint numSimulations = 100, uint numSamples = 100);

        void Init();
        void SetDefaultConstants();

        #region Directories and files config

        string BaseFilePath { get; set; }
        string EnvBaseFilePath { get; set; }
        string TestListFilePath { get; set; }
        string CondorServerBaseEnvPath { get; set; }
        string CondorScriptPath { get; set; }
        string MemoryBaseFilePath { get; set; }
        string TestMeasuresName { get; set; }

        #endregion

        #region Test-related parameterization

        uint NumSamples { get; set; }
        uint NumTimeSteps { get; set; }
        uint SampleSteps { get; set; }
        uint NumSimulations { get; set; }
        bool RandStart { get; set; }
        uint MaxCPUsUsed { get; set; }

        #endregion

        #region Learning-related parameterization

        double Epsilon { get; set; }
        double Temperature { get; set; }
        double Discount { get; set; }
        double LearningRate { get; set; }
        double MaximalChangeThreshold { get; set; }
        double ExploratoryDecay { get; set; }
        double InitialActionValue { get; set; }
        double ActionAccuracy { get; set; }

        #endregion

        #region UI options

        int CellSize { get; set; }
        bool GraphicsEnabled { get; set; }

        #endregion

        #region Tests configuration

        uint[] MultipleTestTypes { get; set; }
        uint SingleTestType { get; set; }
        ITestParameters SingleTestParameters { get; set; }
        Dictionary<uint, IScenario> ScenarioProfiles { get; set; }
        
        #endregion
    }
}