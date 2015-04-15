// ------------------------------------------
// OptimizationTestFactory.cs, Learning
// 
// Created by Pedro Sequeira, 2013/12/09
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using System.IO;
using Learning.Testing.Config;
using Learning.Testing.Config.Parameters;
using Learning.Testing.Config.Scenarios;
using Learning.Testing.SingleTests;

namespace Learning.Testing.MultipleTests
{
    [Serializable]
    public abstract class OptimizationTestFactory : IDisposable, IOptimizationTestFactory
    {
        protected const string TEMP_FILE_PREFIX = "Temp-";

        protected OptimizationTestFactory(IFitnessScenario scenario)
        {
            this.Scenario = scenario;
        }

        #region IDisposable Members

        public virtual void Dispose()
        {
        }

        #endregion

        #region IOptimizationTestFactory Members

        public virtual TestMeasureList CreateAndInitTestMeasureList()
        {
            var testMeasures = this.CreateTestMeasureList();
            var tempFilePath = string.Format(
                "{0}{1}{2}{3}.csv", this.Scenario.FilePath, Path.DirectorySeparatorChar,
                TEMP_FILE_PREFIX, this.TestsConfig.TestMeasuresName);
            testMeasures.CreateTempWriter(tempFilePath);
            return testMeasures;
        }

        public virtual FitnessTest CreateTest(ITestParameters parameters)
        {
            var singleTest = new FitnessTest(this.Scenario, parameters);
            singleTest.Reset();
            return singleTest;
        }

        public virtual TestMeasure CreateTestMeasure(FitnessTest test)
        {
            var testMeasure = new TestMeasure();
            if (test == null) return testMeasure;

            testMeasure.ID = test.TestName;
            testMeasure.Parameters = test.TestParameters;
            testMeasure.Quantity = test.SimulationScoreAvg;
            testMeasure.Value = test.FinalScores.Avg;
            testMeasure.StdDev = test.FinalScores.StdDev;
            return testMeasure;
        }

        public virtual TestMeasureList CreateTestMeasureList()
        {
            //use as base parameters the single test params
            var baseMeasure = this.CreateTestMeasure(null);
            baseMeasure.Parameters = this.TestsConfig.SingleTestParameters;
            return new TestMeasureList(this.Scenario, baseMeasure);
        }

        #endregion

        #region Properties

        protected ITestsConfig TestsConfig
        {
            get { return this.Scenario.TestsConfig; }
        }

        public IFitnessScenario Scenario { get; private set; }

        #endregion
    }
}