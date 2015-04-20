// ------------------------------------------
// SingleTestRunner.cs, Learning
// 
// Created by Pedro Sequeira, 2015/04/01
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using System.IO;
using Learning.Testing.Config;
using Learning.Testing.SingleTests;
using PS.Utilities;
using PS.Utilities.Collections;
using PS.Utilities.IO;

//using Learning.Tests.EmotionalOptimization; //added by Kim

namespace Learning.Testing.Runners
{
    public class SingleTestRunner : TestRunner
    {
        protected SingleTestRunner(ITestsConfig testsConfig)
            : base(testsConfig)
        {
        }

        public override void RunTest()
        {
            //creates test of default TestType and with default parameters 
            var test = this.DefaultTestFactory.CreateTest(this.TestsConfig.SingleTestParameters);
            this.PrepareTest(test);
             

            //executes test
            this.RunSimulation(test);

            //creates and prints test measure
            this.PrintTestMeasure(test);

            test.Dispose();
        }

        private void PrepareTest(FitnessTest test)
        {
            test.LogStatistics = this.TestsConfig.GraphicsEnabled;

            if (!this.TestsConfig.GraphicsEnabled)
                return;

            //initiates log and writes all properties to file
            PathUtil.CreateOrClearDirectory(test.FilePath);
            test.LogWriter =
                new LogWriter(String.Format("{0}{1}test.log", test.FilePath, Path.DirectorySeparatorChar));
            test.WriteProperties();
        }

        private void PrintTestMeasure(FitnessTest test)
        {
            //creates and prints test measure
            var testMeasure = this.DefaultTestFactory.CreateTestMeasure(test);
            var testMeasuresFilePath = Path.GetFullPath(
                String.Format("{0}{1}..{1}{2}.csv", test.FilePath, Path.DirectorySeparatorChar,
                    this.TestsConfig.TestMeasuresName));

            if (!File.Exists(testMeasuresFilePath))
                ExclusiveFileWriter.AppendLine(testMeasuresFilePath, ArrayUtil.ToString(testMeasure.Header));
            ExclusiveFileWriter.AppendLine(testMeasuresFilePath, ArrayUtil.ToString(testMeasure.ToValue()));
        }

        protected virtual void RunSimulation(FitnessTest test)
        {
            //just run test on console
            this.RunConsoleApplication(test);
        }

        protected virtual void RunConsoleApplication(FitnessTest test)
        {
            //runs test
            Console.WriteLine(@"----------------------------------------------");
            Console.WriteLine(@"Starting test {0}...", test.TestName);

            test.Run();

            if (this.TestsConfig.GraphicsEnabled)
            {
                //prints results
                Console.WriteLine(@"----------------------------------------------");
                Console.WriteLine(@"Printng test results...");
                test.PrintResults();
            }

            Console.WriteLine(@"----------------------------------------------");
            Console.WriteLine(@"Test has finished!");
        }
    }
}