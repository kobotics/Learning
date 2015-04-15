// ------------------------------------------
// ListFitnessTest.cs, Learning
// 
// Created by Pedro Sequeira, 2014/03/10
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Learning.Testing.Config.Parameters;
using Learning.Testing.SingleTests;
using PS.Utilities.Serialization;

namespace Learning.Testing.MultipleTests
{
    /// <summary>
    ///     Optimizes tests based on a predefined list of parameters.
    /// </summary>
    public class ListFitnessTest : ParallelOptimizationTest
    {
        protected const string LIST_TEST_ID = "Optimization";

        #region Constructor

        public ListFitnessTest(IOptimizationTestFactory optimizationTestFactory)
            : base(optimizationTestFactory)
        {
            this.AddSpecialTestParams = true;
            this.RunAllTestsAgain = false;
        }

        #endregion

        #region Fields

        protected readonly List<SingleTest> currentTests = new List<SingleTest>();
        protected int currentTestIdx;
        protected List<ITestParameters> testParameters;

        #endregion

        #region Properties

        public bool RunAllTestsAgain { get; set; }

        public bool AddSpecialTestParams { get; set; }

        #endregion

        #region Public Methods

        public override string TestID
        {
            get { return LIST_TEST_ID; }
        }

        public override double ProgressValue
        {
            get
            {
                lock (this.locker)
                {
                    if (this.currentTests.Count == 0) return 1;

                    var maxSteps = (long) this.NumTimeSteps*this.NumSimulations*this.testParameters.Count;
                    var curTestsSteps =
                        this.currentTests.Sum(test => test.ProgressValue*this.NumTimeSteps*this.NumSimulations);
                    var previousTestsSteps =
                        (this.currentTestIdx - this.currentTests.Count)*this.NumSimulations*this.NumTimeSteps;
                    var curStep = previousTestsSteps + curTestsSteps;

                    return curStep/maxSteps;
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            this.testParameters.Clear();
            this.currentTests.Clear();
        }

        public override void PrintResults()
        {
            this.PrintResults(true);
        }

        #endregion

        #region Protected Methods

        protected virtual void DetermineParametersList(bool fromFile = false)
        {
            this.testParameters = fromFile
                ? this.ReadTestsConfig(this.TestsConfig.TestListFilePath)
                : this.TestsConfig.GetOptimizationTestParameters();
        }

        protected virtual List<ITestParameters> ReadTestsConfig(string fileName)
        {
            var parametersHash = new HashSet<ITestParameters>();

            if (!File.Exists(fileName)) return parametersHash.ToList();

            var sr = new StreamReader(fileName);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.StartsWith("//")) continue;

                var parametersStrArray = line.Replace(',', '.').Split(TestMeasureList.CSV_SEPARATOR);

                var parameters = this.TestsConfig.SingleTestParameters.CloneJson();
                if (!parameters.FromValue(parametersStrArray))
                    continue;

                //checks for duplicate test
                if (parametersHash.Contains(parameters))
                    continue;

                //adds parameters to list
                parametersHash.Add(parameters);
            }
            sr.Close();

            return parametersHash.ToList();
        } 

        protected override void RunOptimizationTests()
        {
            //loop until tests end or 
            while (!this.StopAllTests)
            {
                ITestParameters parameters;

                //lock on to take next test from list
                lock (this.locker)
                {
                    //gets next test if possible, or else returns (ends thread)
                    if (this.currentTestIdx >= this.testParameters.Count) return;

                    //gets next test parameter from list
                    if (this.testParameters.Count == 0) continue;
                    parameters = this.testParameters[currentTestIdx++];

                    //ignores tests already with a measure
                    if (!this.RunAllTestsAgain && this.TestMeasures.Contains(parameters)) continue;
                }

                //runs test
                this.RunSingleTest(parameters);
            }
        }

        protected virtual void RunSingleTest(ITestParameters parameters)
        {
            var test = this.OptimizationTestFactory.CreateTest(parameters);
            this.PrepareSingleTest(test);
            test.Run();
            this.FinishSingleTest(test);
        }

        protected virtual void PrepareSingleTest(FitnessTest test)
        {
            //this.WriteLine(string.Format(@"Initiating: {0}", test.TestName));
            lock (this.locker)
                this.currentTests.Add(test);
        }

        protected virtual void FinishSingleTest(FitnessTest test)
        {
            lock (this.locker)
            {
                //test.PrintResults();

                //removes test from current list
                this.currentTests.Remove(test);

                //gets test measure and replaces on the measures list
                var testMeasure = this.OptimizationTestFactory.CreateTestMeasure(test);
                this.TestMeasures.Remove(test.TestParameters);
                this.TestMeasures.Add(test.TestParameters, testMeasure);


                this.WriteLine(
                    string.Format("{0}: {1:0.00}±{2:0.00}",
                        test.TestName, testMeasure.Value, testMeasure.StdDev));
            }
        }

        protected override void ResetTest()
        {
            base.ResetTest();

            this.currentTestIdx = 0;

            //determines chromosomes parameters
            this.DetermineParametersList();

            //adds special test parameters if not already in the list
            if (this.AddSpecialTestParams)
            {
                var specialTestParameters = this.TestsConfig.GetSpecialTestParameters(this.Scenario);
                this.testParameters.AddRange(
                    specialTestParameters.Where(testParam => !this.testParameters.Contains(testParam)));
            }

            //clears all test measures except of selected parameters (needed for measures info)
            this.TestMeasures.ClearExcept(new HashSet<ITestParameters>(this.testParameters));
        }

        protected override void PrintTestPerformanceResults()
        {
            this.WriteLine(string.Format("{0} parameters tested.", this.testParameters.Count));
            base.PrintTestPerformanceResults();
        }

        #endregion
    }
}