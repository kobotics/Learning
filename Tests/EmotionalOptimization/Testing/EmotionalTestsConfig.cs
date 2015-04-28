// ------------------------------------------
// EmotionalTestsConfig.cs, Learning.Tests.EmotionalOptimization
// 
// Created by Pedro Sequeira, 2013/06/20
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using Learning.Domain.Agents;
using Learning.Domain.Environments;
using Learning.IMRL.Emotions.Domain.Agents;
using Learning.IMRL.Emotions.Testing;
using Learning.Testing;
using Learning.Testing.Config.Parameters;
using Learning.Testing.Config.Scenarios;
using Learning.Testing.MultipleTests;
using Learning.Tests.EmotionalOptimization.Domain.Agents;
using Learning.Tests.EmotionalOptimization.Domain.Environments;
using PS.Utilities;
using PS.Utilities.Collections;

namespace Learning.Tests.EmotionalOptimization.Testing
{

    #region TestType enum

    public enum TestType
    {
        MovingPreys,
        Persistence,
        DifferentPreySeason,
        PoisonedSeason,
        HungerThirst,
        Lairs,
        Pacman,
        PowerPellets,
        EatAllDots,
        RewardingDots,
        AllTests,
        AllPacManTests,
        AllForagingTests
    }

    #endregion

    [Serializable]
    public class EmotionalTestsConfig : IMRL.Emotions.Testing.EmotionalTestsConfig
    {
        private const string SCORE_TEXT = "Score";
        protected string EnvironmentFile { get; set; }
        protected string CornersEnvironmentFile { get; set; }
        protected string PacmanEnvironmentFile { get; set; }

        public override void SetDefaultConstants()
        {
            base.SetDefaultConstants();

            //directories and files config
            this.BaseFilePath = "./";
            this.EnvBaseFilePath = "../../../../bin/config";
            this.CondorServerBaseEnvPath = "../config/";
            this.CondorScriptPath = "./submit";

            this.DestinationTestsDir =
                Path.GetFullPath("../../Test_results"); //../../../../../../experiments/EMOT-EMERGE/IMAllForagingTests/");
            this.PreviousPhaseID = "Optim";
            this.TestMeasuresName = "TestMeasures";
            this.TestIDPrefix = "IM"; //"IMGP"; //"IM";

            //test-related constants
            var stepInterval = new StepInterval<double>(-1, 1, .1); //(-1, 1, 1); //.1m; //.25m; //1m; //.2m
            this.ParamsStepIntervals = StepInterval<double>.CreateArray(stepInterval, 5);
            this.NumTimeSteps = 100000;
            this.NumSimulations = 200; //208; //1; //4; //16; //208; //104; //48;
            this.NumSamples = 100; //100; // this.NumTimeSteps;
            this.SampleSteps = this.NumTimeSteps/this.NumSamples;

            //environment-related constants
            this.NumStepsPerSeason = 5000; //10000;
            this.MaxMoveActionsRequired = 30;
            this.RandStart = false; //true; //false;

            //learning-related constants
            this.Temperature = 3.0f; // this.NumTimeSteps;
            this.Epsilon = 1.0f;
            this.Discount = 0.9f;
            this.LearningRate = 0.3f;
            this.MaximalChangeThreshold = 0.0001f; //0; //
            this.ExploratoryDecay = 1.0001f; //1.0001f; //1.00004f;

            //                                                      n    gr    c     v    er
            //this.SingleTestParameters = new ArrayParameter(new[] { -.1, .10, -.1, .10, .60 });    //persistence
            //this.SingleTestParameters = new ArrayParameter(new[] { .00, .10, .60, .00, .30 });    //diff prey season
            //this.SingleTestParameters = new ArrayParameter(new[] { .10, -.2, .10, .00, .60 });    //poisoned prey season
            //this.SingleTestParameters = new ArrayParameter(new[] { .10, .00, -.2, .00, .70 });    //lairs
            this.SingleTestParameters = new ArrayParameter(new[] { -.4, .00, .00, .50, .10 });    //hunger-thirst
            //this.SingleTestParameters = new ArrayParameter(new[] { .00, .00,-.30, .00, .70 });    //UNIVERSALadded
            //this.SingleTestParameters = new ArrayParameter(new[] {.40, .00, -.1, .20, -.3}); //moving preys
            //this.SingleTestParameters = new ArrayParameter(new[] { 1.0, .00, .00, .00, .00 });    //novelty only

            //this.SingleTestParameters = new ArrayParameter(new[] { .10, .10, .10, .60, .10 });    //eat all dots
            //this.SingleTestParameters = new ArrayParameter(new[] { .20, .10, .20, .20, .30 });    //pacman
            //this.SingleTestParameters = new ArrayParameter(new[] { -.2, .20, .10, .50, .00 });    //power pellets
            //this.SingleTestParameters = new ArrayParameter(new[] { .50, .00, .10, .20, .20 });    //rewarding dots

            //this.SingleTestParameters = new ArrayParameter(new[] { .00, .00, -.3, .00, .70 });    //universal
            //this.SingleTestParameters = new ArrayParameter(new[] { .00, .00, .00, .00, 1.0 });    //extrinsic only
            //this.SingleTestParameters = new ArrayParameter(new[] { .20, .20, .20, .20, .20 });    //equal-weights
            //this.SingleTestParameters = new ArrayParameter(new[] { .00, .00, .00, .00, .00 });    //random

            ((ArrayParameter) this.SingleTestParameters).Header = this.ParamIDNames;

            this.SingleTestType = (uint) TestType.HungerThirst;
            this.CellSize = 50;
            this.MaxCPUsUsed = ProcessUtil.GetCPUCount(); //3; //ProcessUtil.GetCPUCount();

            this.MultipleTestTypes = new[]
                                     {
                                         (uint) TestType.Lairs,
                                         (uint) TestType.HungerThirst,
                                         //(uint) TestType.MovingPreys,
                                         //(uint) TestType.Persistence,
                                         //(uint) TestType.PoisonedSeason,
                                         //(uint) TestType.DifferentPreySeason
                                         //(uint) TestType.PowerPellets,
                                         //(uint) TestType.RewardingDots,
                                         //(uint) TestType.EatAllDots,
                                         //(uint) TestType.Pacman,
                                         //(uint) TestType.AllForagingTests
                                     };
        }

        protected override string GetTestID(uint testType)
        {
            return ((TestType) testType).ToString();
        }

        public override IOptimizationTestFactory CreateTestFactory(
            IScenario scenario, uint numSimulations, uint numSamples)
        {
            return new EmotionalOptimizationTestFactory(
                    (IFitnessScenario) scenario.Clone(numSimulations, numSamples));
        }

        public override void Init()
        {
            this.EnvironmentFile = this.GetEnvironmentFilePath("ir-environment.xml");
            this.CornersEnvironmentFile = this.GetEnvironmentFilePath("corners-environment.xml");
            this.PacmanEnvironmentFile = this.GetEnvironmentFilePath("pacman-environment.xml");

            base.Init();
        }

        protected override void CreateTestsProfiles()
        {
            this.ScenarioProfiles = new Dictionary<uint, IScenario>();

            var testType = (uint) TestType.MovingPreys;
            this.ScenarioProfiles.Add(
                testType, this.CreateForagingTestProfile(
                    testType, new EmotionalAgent(), new MovingPreysEnvironment(), this.EnvironmentFile, 14));

            testType = (uint) TestType.DifferentPreySeason;
            this.ScenarioProfiles.Add(
                testType, this.CreateForagingTestProfile(
                    testType, new EmotionalAgent(), new PreySeasonEnvironment(), this.EnvironmentFile, 13));

            testType = (uint) TestType.Persistence;
            this.ScenarioProfiles.Add(
                testType, this.CreateForagingTestProfile(
                    testType, new EmotionalAgent(), new PersistenceEnvironment(), this.EnvironmentFile, 11));
            testType = (uint) TestType.PoisonedSeason;
            this.ScenarioProfiles.Add(
                testType, this.CreateForagingTestProfile(
                    testType, new EmotionalAgent(), new PoisonedSeasonEnvironment(), this.EnvironmentFile, 13));
            testType = (uint) TestType.HungerThirst;
            this.ScenarioProfiles.Add(
                testType, this.CreateForagingTestProfile(
                    testType, new HungerThirstAgent(), new HungerThirstEnvironment(), this.CornersEnvironmentFile, 275));

            testType = (uint) TestType.Lairs;
            this.ScenarioProfiles.Add(
                testType, this.CreateForagingTestProfile(
                    testType, new LairsAgent(), new LairsEnvironment(), this.CornersEnvironmentFile, 80));

            testType = (uint) TestType.Pacman;
            this.ScenarioProfiles.Add(
                testType, this.CreatePacManTestProfile(testType, 1200, false, false, true, true, 3, 0, 0, -0.1));

            testType = (uint) TestType.EatAllDots;
            this.ScenarioProfiles.Add(
                testType, this.CreatePacManTestProfile(testType, 1000, false, false, true, false, 3, 0.5, 0, -0.5));

            testType = (uint) TestType.PowerPellets;
            this.ScenarioProfiles.Add(
                testType, this.CreatePacManTestProfile(testType, 200, true, false, false, true, 0, 0.8, 0, -1));

            testType = (uint) TestType.RewardingDots;
            this.ScenarioProfiles.Add(
                testType, this.CreatePacManTestProfile(testType, 1000, false, false, false, false, 0, 0.8, 0.1, -1));

            testType = (uint) TestType.AllTests;
            this.ScenarioProfiles.Add(
                testType,
                new MultipleScenario(new List<IScenario>(this.ScenarioProfiles.Values))
                {
                    FilePath = this.GetFilePath(testType),
                    TestMeasuresFilePath = this.GetTestMeasuresFilePath(testType)
                });

            testType = (uint) TestType.AllPacManTests;
            this.ScenarioProfiles.Add(
                testType,
                new MultipleScenario(new List<IScenario>
                                     {
                                         this.ScenarioProfiles[(uint) TestType.EatAllDots],
                                         this.ScenarioProfiles[(uint) TestType.Pacman],
                                         this.ScenarioProfiles[(uint) TestType.RewardingDots],
                                         this.ScenarioProfiles[(uint) TestType.PowerPellets]
                                     })
                {
                    FilePath = this.GetFilePath(testType),
                    TestMeasuresFilePath = this.GetTestMeasuresFilePath(testType)
                });

            testType = (uint) TestType.AllForagingTests;
            this.ScenarioProfiles.Add(
                testType,
                new MultipleScenario(new List<IScenario>
                                     {
                                         this.ScenarioProfiles[(uint) TestType.MovingPreys],
                                         this.ScenarioProfiles[(uint) TestType.Persistence],
                                         this.ScenarioProfiles[(uint) TestType.HungerThirst],
                                         this.ScenarioProfiles[(uint) TestType.Lairs],
                                         this.ScenarioProfiles[(uint) TestType.PoisonedSeason],
                                         this.ScenarioProfiles[(uint) TestType.DifferentPreySeason]
                                     })
                {
                    FilePath = this.GetFilePath(testType),
                    TestMeasuresFilePath = this.GetTestMeasuresFilePath(testType)
                });
        }

        private PacmanScenario CreatePacManTestProfile(
            uint testType, uint maxStates, bool hideDots, bool hideBigDot,
            bool hideKeeperGhost, bool powerPelletEnabled, uint maxLives,
            double bigDotReward, double dotReward, double deathReward)
        {
            return new PacmanScenario(new PacmanAgent(), new PacmanEnvironment(), this)
                   {
                       FilePath = this.GetFilePath(testType),
                       EnvironmentConfigFile = this.PacmanEnvironmentFile,
                       TestMeasuresFilePath = this.GetTestMeasuresFilePath(testType),
                       FitnessText = SCORE_TEXT,
                       MaxStates = maxStates,
                       HideDots = hideDots,
                       HideBigDot = hideBigDot,
                       HideKeeperGhost = hideKeeperGhost,
                       PowerPelletEnabled = powerPelletEnabled,
                       MaxLives = maxLives,
                       BigDotReward = bigDotReward,
                       DotReward = dotReward,
                       DeathReward = deathReward
                   };
        }

        private SingleScenario CreateForagingTestProfile(
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
    }
}