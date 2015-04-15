// ------------------------------------------
// TestParameterRanker.cs, Learning
// 
// Created by Pedro Sequeira, 2013/12/09
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Learning.Testing.Config;
using Learning.Testing.Config.Parameters;
using Learning.Testing.MultipleTests;
using PS.Utilities.Math;

namespace Learning.Testing
{
    public class TestParameterRanker
    {
        public TestParameterRanker(ITestsConfig testsConfig, IOptimizationTestFactory testFactory)
        {
            this.TestsConfig = testsConfig;
            this.TestFactory = testFactory;
            this.TestMeasures = testFactory.CreateTestMeasureList();
            this.TestMeasures.WriteTemp = false;
            this.SortedTestParameters = new Dictionary<uint, List<ITestParameters>>();
        }

        public ITestsConfig TestsConfig { get; private set; }
        public IOptimizationTestFactory TestFactory { get; private set; }
        public TestMeasureList TestMeasures { get; private set; }
        protected Dictionary<uint, List<ITestParameters>> SortedTestParameters { get; private set; }

        public void RankTests()
        {
            this.TestMeasures.Clear();

            //processes all tests types to read test results (measures)
            var multipleTestTypes = this.TestsConfig.MultipleTestTypes;
            if (multipleTestTypes == null) return;
            foreach (var testType in multipleTestTypes)
                this.ProcessTestType(testType);

            //creates a test measure list based on the average test parameters ranking
            this.CreateTestMeasureList();
        }

        protected void CreateTestMeasureList()
        {
            //goes through all the parameters 
            var testParameters = this.SortedTestParameters.Values.First();
            foreach (var testParameter in testParameters)
            {
                //gets average of rank for each test type
                var rankQuantity = new StatisticalQuantity((uint) this.TestsConfig.MultipleTestTypes.Length);
                foreach (var testType in TestsConfig.MultipleTestTypes)
                    rankQuantity.Value = this.SortedTestParameters[testType].IndexOf(testParameter);

                //creates measure based on this rank avg and adds to list
                var rankMeasure = new TestMeasure
                                  {
                                      Value = rankQuantity.Avg,
                                      StdDev = rankQuantity.StdDev,
                                      Parameters = testParameter,
                                      ID = testParameter.ToString()
                                  };
                this.TestMeasures.Add(testParameter, rankMeasure);
            }
        }

        protected void ProcessTestType(uint testType)
        {
            //gets profile and tests for measures list existence
            var testProfile = this.TestsConfig.ScenarioProfiles[testType];
            if (!File.Exists(testProfile.TestMeasuresFilePath)) return;

            //reads measure list from file, sorts measures and stores list for the type
            var testMeasures = this.TestFactory.CreateTestMeasureList();
            testMeasures.ReadFromFile(testProfile.TestMeasuresFilePath);
            this.SortedTestParameters[testType] = testMeasures.Sort();
        }
    }
}