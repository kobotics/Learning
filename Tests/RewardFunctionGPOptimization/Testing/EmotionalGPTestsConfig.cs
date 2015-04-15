// ------------------------------------------
// EmotionalGPTestsConfig.cs, Learning.Tests.RewardFunctionGPOptimization
// 
// Created by Pedro Sequeira, 2013/02/06
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using AForge.Genetic;
using Learning.Domain.Agents;
using Learning.Domain.Environments;
using Learning.EvolutionaryComputation;
using Learning.IMRL.EC;
using Learning.IMRL.EC.Chromosomes;
using Learning.IMRL.EC.Domain;
using Learning.IMRL.EC.Genes;
using Learning.IMRL.EC.Testing;
using Learning.IMRL.EC.Testing.MultipleTests;
using Learning.IMRL.Emotions.Testing;
using Learning.Testing.Config.Parameters;
using Learning.Testing.Config.Scenarios;
using Learning.Testing.MultipleTests;
using Learning.Tests.EmotionalOptimization.Domain.Environments;
using Learning.Tests.EmotionalOptimization.Testing;
using Learning.Tests.RewardFunctionGPOptimization.Domain.Agents;
using PS.Utilities;

namespace Learning.Tests.RewardFunctionGPOptimization.Testing
{
    [Serializable]
    public class EmotionalGPTestsConfig : GPTestsConfig, IEmotionalTestsConfig
    {
        protected string EnvironmentFile { get; set; }
        protected string CornersEnvironmentFile { get; set; }
        protected string PacmanEnvironmentFile { get; set; }

        #region IEmotionalTestsConfig Members

        public uint MaxMoveActionsRequired { get; set; }
        public uint NumStepsPerSeason { get; set; }

        public override void SetDefaultConstants()
        {
            base.SetDefaultConstants();

            //this.DestinationTestsDir = Path.GetFullPath("../../../../../../experiments/EMOT-EMERGE/GPAllForagingTests/");
            this.DestinationTestsDir = Path.GetFullPath("New folder/");
            this.PreviousPhaseID = "Top5";
            //this.PreviousPhaseID = "Temp";
            this.TestMeasuresName = "ChromosomesHistory";
            this.TestIDPrefix = "GP";

            //directories and config
            this.BaseFilePath = "./";
            this.EnvBaseFilePath = "../../../../bin/config";
            this.CondorServerBaseEnvPath = "../config/";
            this.CondorScriptPath = "./submit";

            //test-related constants
            this.StdDevTimes = 2;
            this.NumTimeSteps = 100000;
            this.NumSimulations = 1; //208; //1; //4; //16; //208; //104; //48;
            this.NumSamples = 100; //100; // this.NumTimeSteps;
            this.SampleSteps = this.NumTimeSteps/this.NumSamples;

            //genetic algorithms constants
            this.NumBaseVariables = 8;
            this.NumTestsPerIteration = 5; //20; //10; //100; (pop. size)
            this.MaxIterations = 5; //50; //25; //50; (num. generations)
            this.NumParallelOptimTests = 2; //16; //32; //56; //multiple of CPUs (num. populations)
            this.FitnessImprovementThreshold = 15; //15; //5; //this.MaxIterations;
            this.RandomSelectionPortion = 0.2f;
            this.SteadyStatePortion = 0.1f;
            this.SymmetryFactor = 0.7f;

            //environment-related constants
            this.NumStepsPerSeason = 5000;
            this.MaxMoveActionsRequired = 30;
            this.RandStart = true; //false;

            //learning constants
            this.Epsilon = 1.0f;
            this.Discount = 0.9f;
            this.LearningRate = 0.3f;
            this.MaximalChangeThreshold = 0.0001f; //0f; //
            this.ExploratoryDecay = 1.0001f; //1.00004f; //1.0001f;

            // $8 $9 $10 $11 $12
            this.Constants = new[] {0d, 1, 2, 3, 5}; //, 7};

            //$0 = Re, $1=Csa, $2=Cs, $3=D, $4=Vs, $5=Qsa, $6=Esa, $7=Pssa
            this.SingleTestParameters = new GPExpressionChromosome("0 $2 $2 * - "); //MovingPreys
            //this.SingleTestParameters = new GPExpressionChromosome("$5 $4 - "); //Persistence
            //this.SingleTestParameters = new GPExpressionChromosome("$0 $5 + $7 - "); //DifferentPreySeason
            //this.SingleTestParameters = new GPExpressionChromosome("$0 $12 - $12 * $5 - "); //PoisonedSeason
            //this.SingleTestParameters = new GPExpressionChromosome("$8 $4 - $10 $5 - - "); //HungerThirst
            //this.SingleTestParameters = new GPExpressionChromosome("$5 $4 - "); //Lairs
            //this.SingleTestParameters = new GPExpressionChromosome("$0 "); //fit-based only

            this.SingleTestType = (uint) TestType.MovingPreys;
            this.CellSize = 50;
            this.GraphicsEnabled = true;
            this.MaxCPUsUsed = ProcessUtil.GetCPUCount(); //3; //ProcessUtil.GetCPUCount();

            this.SelectionMethod = new EliteSelection();
            //new RouletteWheelSelection();//new RankSelection();

            this.AllowedFunctions = //CustomGeneFunction.AllFunctions;
                new List<FunctionType>
                {
                    FunctionType.Add,
                    FunctionType.Subtract,
                    FunctionType.Multiply,
                    FunctionType.Divide,
                    FunctionType.Exp,
                    FunctionType.Ln,
                    FunctionType.Sqrt
                };

            this.MultipleTestTypes = new[]
                                     {
                                         (uint) TestType.Lairs,
                                         (uint) TestType.HungerThirst,
                                         (uint) TestType.MovingPreys,
                                         (uint) TestType.Persistence,
                                         (uint) TestType.PoisonedSeason,
                                         (uint) TestType.DifferentPreySeason,
                                         (uint) TestType.PoisonedSeason
                                     };
        }

        public override IOptimizationTestFactory CreateTestFactory(
            IScenario scenario, uint numSimulations, uint numSamples)
        {
            return new EmotionalGPOptimizationTestFactory(
                (IFitnessScenario) scenario.Clone(numSimulations, numSamples));
        }

        public override void Init()
        {
            this.EnvironmentFile = this.GetEnvironmentFilePath("ir-environment.xml");
            this.CornersEnvironmentFile = this.GetEnvironmentFilePath("corners-environment.xml");
            this.PacmanEnvironmentFile = this.GetEnvironmentFilePath("pacman-environment.xml");

            base.Init();
        }

        public override string GetTestName(IScenario scenario, ITestParameters testParameters)
        {
            return Util.GetTranslatedExpression(testParameters.ToString(),
                this.Constants, ForagingGPMotivationManager.VariableNames);
        }

        #endregion

        public override IGPSimplifierOptimizationTestFactory CreateSimplifierTestFactory(
            IScenario scenario, uint numSimulations, uint numSamples)
        {
            return new GPSimplifierOptimizationTestFactory(
                (IFitnessScenario) scenario.Clone(numSimulations, numSamples));
        }

        protected override string GetTestID(uint testType)
        {
            return ((TestType) testType).ToString();
        }

        protected override void CreateTestsProfiles()
        {
            this.ScenarioProfiles = new Dictionary<uint, IScenario>();

            var testType = (uint) TestType.MovingPreys;
            this.ScenarioProfiles.Add(
                testType, this.CreateTestProfile(
                    testType, new GPForagingAgent(), new MovingPreysEnvironment(), this.EnvironmentFile, 14));

            testType = (uint) TestType.DifferentPreySeason;
            this.ScenarioProfiles.Add(
                testType, this.CreateTestProfile(
                    testType, new GPForagingAgent(), new PreySeasonEnvironment(), this.EnvironmentFile, 13));

            testType = (uint) TestType.Persistence;
            this.ScenarioProfiles.Add(
                testType, this.CreateTestProfile(
                    testType, new GPForagingAgent(), new PersistenceEnvironment(), this.EnvironmentFile, 11));
            testType = (uint) TestType.PoisonedSeason;
            this.ScenarioProfiles.Add(
                testType, this.CreateTestProfile(
                    testType, new GPForagingAgent(), new PoisonedSeasonEnvironment(), this.EnvironmentFile, 13));
            testType = (uint) TestType.HungerThirst;
            this.ScenarioProfiles.Add(
                testType, this.CreateTestProfile(
                    testType, new GPHungerThirstAgent(), new HungerThirstEnvironment(), this.CornersEnvironmentFile, 50));

            testType = (uint) TestType.Lairs;
            this.ScenarioProfiles.Add(
                testType, this.CreateTestProfile(
                    testType, new GPLairsAgent(), new LairsEnvironment(), this.CornersEnvironmentFile, 80));
        }

        protected virtual SingleScenario CreateTestProfile(
            uint testType, IAgent agent, IEnvironment environment, string environmentConfigFile, uint maxStates)
        {
            return new SingleScenario(agent, environment, this)
                   {
                       FilePath = this.GetFilePath(testType),
                       EnvironmentConfigFile = environmentConfigFile,
                       TestMeasuresFilePath = this.GetTestMeasuresFilePath(testType),
                       MaxStates = maxStates
                   };
        }

        public override IECChromosome CreateBaseChromosome()
        {
            //creates a base chromosome with the test variables
            return new GPChromosome(new FlexibleGPGene(this.AllowedFunctions, this.VariablesCount));
        }
    }
}