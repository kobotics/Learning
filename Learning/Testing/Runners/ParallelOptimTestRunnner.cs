// ------------------------------------------
// ParallelOptimTestRunnner.cs, Learning
// 
// Created by Pedro Sequeira, 2015/04/01
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using System.IO;
using Learning.Testing.Config;
using Learning.Testing.MultipleTests;

namespace Learning.Testing.Runners
{
    public class ParallelOptimTestRunnner : TestRunner
    {
        protected readonly OptimizationScheme optimizationScheme;
        protected string prevTestMeasuresPath;

        public ParallelOptimTestRunnner(ITestsConfig testsConfig, OptimizationScheme optimizationScheme)
            : base(testsConfig)
        {
            this.optimizationScheme = optimizationScheme;
        }

        public override void RunTest()
        {
            //checks multiple test list 
            var multipleTestTypes = this.TestsConfig.MultipleTestTypes;
            if (multipleTestTypes == null) return;

            //runs all tests from list
            foreach (var testType in multipleTestTypes)
            {
                //resets variables
                this.prevTestMeasuresPath = null;

                //runs all tests for selected type
                this.RunTestsForType(testType);
            }
        }

        private void RunTestsForType(uint testType)
        {
            //creates optimization tests for a test type
            var optimizationTests = this.optimizationScheme.CreateFor(testType, this.TestsConfig);

            //runs tests in sequence
            foreach (var test in optimizationTests)
                this.RunTest(test);
        }

        protected virtual void RunTest(ParallelOptimizationTest test)
        {
            //tries to recover previous tests measures
            if (this.prevTestMeasuresPath != null)
                test.OptimizationTestFactory.Scenario.TestMeasuresFilePath = this.prevTestMeasuresPath;

            //runs test
            Console.WriteLine(@"----------------------------------------------");
            Console.WriteLine(@"Starting {0} test...", test.TestID);
            test.LogStatistics = this.TestsConfig.GraphicsEnabled;
            test.Run();

            //prints results and disposes
            Console.WriteLine(@"----------------------------------------------");
            Console.WriteLine(@"Printing test results...");

            if (this.TestsConfig.GraphicsEnabled)
                test.PrintResults();
            else
                this.PrintTestMeasures(test);

            Console.WriteLine(@"{0} test has finished.", test.TestID);

            this.prevTestMeasuresPath = test.TestMeasures.LastFilePath;

            test.Dispose();
        }

        private void PrintTestMeasures(ParallelOptimizationTest test)
        {
            //verifies path
            var filePath = Path.GetFullPath(string.Format("{0}{1}", test.FilePath, Path.DirectorySeparatorChar));
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            //prints only test measures
            var testMeasuresFilePath =
                string.Format("{0}{1}{2}.csv", filePath, this.TestsConfig.TestMeasuresName, test.TestID);
            test.TestMeasures.PrintToFile(testMeasuresFilePath);
        }
    }
}