// ------------------------------------------
// SingleTest.cs, Learning
// 
// Created by Pedro Sequeira, 2013/12/09
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Learning.Testing.Config;
using Learning.Testing.Config.Parameters;
using Learning.Testing.Config.Scenarios;
using Learning.Testing.Simulations;
using PS.Utilities;
using PS.Utilities.IO;
using PS.Utilities.Math;
using PS.Utilities.Serialization;

namespace Learning.Testing.SingleTests
{
    public abstract class SingleTest : ITest, IProgressHandler, IResetable
    {
        #region Constructors

        protected SingleTest(IScenario scenario, ITestParameters testParameters)
        {
            //checks arguments
            if (scenario == null)
                throw new ArgumentNullException("scenario", @"testProfile can't be null");

            this.TestParameters = testParameters;
            this.Scenario = scenario;
            this.TestName = scenario.TestsConfig.GetTestName(scenario, testParameters);
        }

        #endregion

        #region IProgressHandler Members

        public abstract double ProgressValue { get; }

        #endregion

        #region IResetable Members

        public virtual void Reset()
        {
            if (this.LogWriter != null)
                this.LogWriter.Close();
            if (this.testStatisticsAvg != null)
                this.testStatisticsAvg.Dispose();

            this.FinalScores = new StatisticalQuantity(this.TestsConfig.NumSimulations);
        }

        #endregion

        #region ITest Members

        public virtual void Dispose()
        {
            if (this.LogWriter != null)
                this.LogWriter.Close();
            if (this.testStatisticsAvg != null)
                this.testStatisticsAvg.Dispose();
            if (this.FinalScores != null)
                this.FinalScores.Dispose();
            this.testStatisticsAvg = null;
        }

        public virtual bool Run()
        {
            this.StartTest();
            this.RunTest();
            this.FinishTest();
            return true;
        }

        public virtual void PrintResults()
        {
            this.PrintPerformanceResults();
            this.PrintAgent();
        }

        #endregion

        #region Fields

        private const char FILE_PATH_REPLACER_CHAR = '_';
        private const string TEST_CONFIG_FILE = "TestConfig.json";
        public const string SCORE_AVG_STAT_ID = "Score";
        protected readonly Dictionary<Simulation, uint> currentSimulations = new Dictionary<Simulation, uint>();
        protected uint curSimulationIDx;
        protected Simulation lastSimulation;
        protected PerformanceMeasure performanceMeasure = new PerformanceMeasure();
        protected StatisticsCollection testStatisticsAvg;

        #endregion

        #region Properties

        public string TestName { get; private set; }

        public bool LogStatistics { get; set; }

        public ITestParameters TestParameters { get; private set; }

        public IScenario Scenario { get; private set; }

        protected ITestsConfig TestsConfig
        {
            get { return this.Scenario.TestsConfig; }
        }

        /// <summary>
        ///     Stores the progress in cumulative fitness averaged for all the test runs.
        /// </summary>
        public StatisticalQuantity SimulationScoreAvg
        {
            get { return this.testStatisticsAvg != null ? this.testStatisticsAvg[SCORE_AVG_STAT_ID] : null; }
        }

        /// <summary>
        ///     Stores the cumulative fitness final values for each test run.
        /// </summary>
        public StatisticalQuantity FinalScores { get; protected set; }

        public long MemoryUsage
        {
            get { return this.performanceMeasure.MemoryUsage; }
        }

        public TimeSpan TestSpeed
        {
            get { return this.performanceMeasure.TimeElapsed; }
        }

        public string FilePath
        {
            get
            {
                return string.Format("{0}{1}{2}",
                    this.Scenario.FilePath, Path.DirectorySeparatorChar,
                    PathUtil.ReplaceInvalidChars(this.TestName, FILE_PATH_REPLACER_CHAR));
            }
        }

        public LogWriter LogWriter { get; set; }

        #endregion

        #region Public Methods

        public void WriteProperties()
        {
            var filePath = string.Format("{0}{1}{2}", this.FilePath, Path.DirectorySeparatorChar, TEST_CONFIG_FILE);
            this.SerializeJsonFile(filePath, JsonUtil.ConfigSettings);
        }

        public virtual void StartTest()
        {
            //start performance measures
            this.performanceMeasure.Start();
        }

        public abstract void RunTest();

        public virtual void FinishTest()
        {
            //stops performance measures
            this.performanceMeasure.Stop();

            this.WriteTestResults();
        }

        public abstract Simulation CreateAndSetupSimulation();

        public virtual void RunSimulation(Simulation simulation)
        {
            simulation.Run();
            this.FinishSimulation(simulation);
        }

        public virtual void FinishSimulation(Simulation simulation)
        {
            //writes score to screen
            if (this.currentSimulations.ContainsKey(simulation))
                this.WriteLine(string.Format("Simulation {0}: {1:0.00}",
                    this.currentSimulations[simulation], simulation.Score.Value));

            //averages test stats
            this.AverageTestStatistics(simulation);
        }

        #endregion

        #region Protected Methods

        protected abstract bool TestHasFinished();

        #region Statistics methods

        protected virtual void AverageTestStatistics(Simulation simulation)
        {
            //stores and replaces ref to last simulation
            if (this.lastSimulation != null)
                if (this.lastSimulation.Equals(simulation)) return;
                else this.lastSimulation.Dispose();
            this.lastSimulation = simulation;

            //adds cumulative (final) value to statistical variable
            this.FinalScores.Value = simulation.Score.Value;

            if (!this.LogStatistics) return;

            //stores or averages tests' statistics
            if (this.testStatisticsAvg == null)
                this.testStatisticsAvg = this.GetTestStatistics(simulation);
            else
                this.testStatisticsAvg.AverageWith(this.GetTestStatistics(simulation));

            //removes simulation from list
            this.currentSimulations.Remove(simulation);
        }

        protected virtual StatisticsCollection GetTestStatistics(Simulation simulation)
        {
            //adds all statistics relevant for the test from the given simulation
            // for single agent tests, this includes all the agent's stats and simulation score
            var statistics = simulation.Agent.StatisticsCollection.Clone();
            statistics.Add(SCORE_AVG_STAT_ID, simulation.Score.Clone());

            return statistics;
        }

        #endregion

        #region Printing and logging

        protected virtual void WriteTestResults()
        {
            this.WriteLine(string.Format("{0}{1}", this.TestName, (" has finished.")));
            this.WriteLine(string.Format("Time taken: {0}", this.TestSpeed.ToString(@"hh\:mm\:ss")));
            this.WriteLine(string.Format("Memory used: {0} bytes", this.MemoryUsage));
            this.WriteLine(string.Format("Test score: {0:0.00} ± {1:0.00}",
                this.FinalScores.Avg, this.FinalScores.StdDev));
        }

        protected virtual void PrintPerformanceResults()
        {
            //prints simulation cumulative score
            this.WriteLine(@"Printing agent performance...");
            this.SimulationScoreAvg.PrintStatisticsToCSV(
                string.Format("{0}{1}CumulativeFitnessAvg.csv", this.FilePath, Path.DirectorySeparatorChar));
            this.FinalScores.PrintStatisticsToCSV(
                string.Format("{0}{1}CumulativeFitnessValues.csv", this.FilePath, Path.DirectorySeparatorChar));
        }

        public virtual void PrintAgent()
        {
            if (!this.LogStatistics) return;

            //prints agent statistics
            this.WriteLine(@"Printing agent statistical quantities...");
            this.PrintStatistic("StateActionValue", "/STM/StateActionValueAvg.csv");
            this.PrintStatistic("Epsilon", "/Learning/EpsilonAvg.csv");
            this.PrintStatistic("NumBackups", "/Learning/NumBackups.csv");

            //creates/cleans output directory
            var path = string.Format("{0}{1}LTM", this.FilePath, Path.DirectorySeparatorChar);
            PathUtil.CreateOrClearDirectory(path);
            this.lastSimulation.Agent.LongTermMemory.PrintResults(path);
            //this.PrintAgentQuantity("PredictionError", "/Learning/PredictionErrorAvg.csv");

            this.PrintActionsStatistics();
        }

        protected void PrintActionsStatistics()
        {
            //prints action info
            var actionQuantitiesList = this.lastSimulation.Agent.Actions.Keys.ToDictionary(
                actionID => actionID,
                quantityName => this.testStatisticsAvg[quantityName]);
            StatisticsUtil.PrintAllQuantitiesToCSV(
                string.Format("{0}/Behavior/ActionsAvg.csv", this.FilePath), actionQuantitiesList);
            StatisticsUtil.PrintAllQuantitiesToImage(
                string.Format("{0}/Behavior/ActionsAvg.png", this.FilePath), actionQuantitiesList);
        }

        protected void WriteLine(string line)
        {
            if (this.LogWriter != null)
                this.LogWriter.WriteLine(line);
        }

        protected void PrintStatistic(string quantityName, string quantityFilePath)
        {
            this.testStatisticsAvg[quantityName].
                PrintStatisticsToCSV(string.Format("{0}{1}", this.FilePath, quantityFilePath));
        }

        #endregion

        #endregion
    }
}