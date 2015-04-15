// ------------------------------------------
// StochasticParallelOptimTest.cs, Learning
//
// Created by Pedro Sequeira, 2014/3/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PS.Utilities.Math;
using Learning.Testing.Config;
using Learning.Testing.StochasticOptimization;

namespace Learning.Testing.MultipleTests
{
    public abstract class StochasticParallelOptimTest : ParallelOptimizationTest
    {
        protected readonly List<IStochasticOptimizationTest> currentOptimTests = new List<IStochasticOptimizationTest>();
        protected uint curNumOptimTests;

        protected StochasticParallelOptimTest(IOptimizationTestFactory optimizationTestFactory)
            : base(optimizationTestFactory)
        {
        }

        protected new IStochasticOptimTestsConfig TestsConfig
        {
            get { return base.TestsConfig as IStochasticOptimTestsConfig; }
        }

        public StatisticalQuantity FitnessMaxProgressAvg { get; protected set; }

        public override double ProgressValue
        {
            get
            {
                lock (this.locker)
                {
                    if (this.currentOptimTests.Count == 0) return 0;

                    var testsConfig = TestsConfig;
                    var maxTotalIterations = (double) testsConfig.MaxIterations*testsConfig.NumParallelOptimTests;
                    var curNumIterations = this.currentOptimTests.Sum(population => population.IterationNumber);
                    var previousIterations = (this.curNumOptimTests - this.currentOptimTests.Count)*
                                             testsConfig.MaxIterations;
                    var curTotalIterations = (double) previousIterations + curNumIterations;

                    return curTotalIterations/maxTotalIterations;
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            this.FitnessMaxProgressAvg.Dispose();
        }

        public override bool Run()
        {
            //runs genetic tests
            if (this.NumSimulations != 0)
                this.RunTestPerCPU(this.RunOptimizationTests);

            return true;
        }

        protected override void PrintTestPerformanceResults()
        {
            base.PrintTestPerformanceResults();

            this.FitnessMaxProgressAvg.PrintStatisticsToCSV(
                string.Format("{0}{1}FitnessMaxAvg.csv", this.FilePath, Path.DirectorySeparatorChar), false, true);
        }

        protected override void RunOptimizationTests()
        {
            //tries to run a population while possible
            while (!this.StopAllTests)
            {
                //checks maximum number of populations run
                lock (this.locker)
                    if (this.curNumOptimTests >= this.TestsConfig.NumParallelOptimTests)
                        return;

                //gets new population number
                uint optimTestNumber;
                lock (this.locker) optimTestNumber = this.curNumOptimTests++;

                //creates base population members (chromosomes)
                var optimizationTest = this.CreateStochasticOptimTest(optimTestNumber);

                //adds population to cur pop list
                lock (this.locker) this.currentOptimTests.Add(optimizationTest);

                //runs every optimization iteration according to the number of iterations
                while (!this.StopAllTests && !optimizationTest.Terminated &&
                       (optimizationTest.IterationNumber < TestsConfig.MaxIterations))
                {
                    //runs and advances one generation
                    this.RunIteration(optimizationTest);
                }

                //removes optim test from cur test list
                lock (this.locker) this.currentOptimTests.Remove(optimizationTest);

                //stores test improvements
                this.StoreOptimTestStatistics(optimizationTest);

                this.WriteLine(
                    string.Format("{0} finished in iteration {1} with improvement threshold of {2}.",
                        optimizationTest.ID, optimizationTest.IterationNumber,
                        optimizationTest.FitnessImprovementThreshold));
            }
        }

        protected virtual void StoreOptimTestStatistics(IStochasticOptimizationTest optimizationTest)
        {
            lock (this.locker)
            {
                //stores or averages optimization tests' statistics
                if (this.FitnessMaxProgressAvg == null)
                    this.FitnessMaxProgressAvg = optimizationTest.FitnessMaxProgress;
                else
                    this.FitnessMaxProgressAvg.AverageWith(optimizationTest.FitnessMaxProgress);
            }
        }

        protected virtual void RunIteration(IStochasticOptimizationTest optimizationTest)
        {
            this.WriteLine(
                string.Format("Running {0} iteration {1}...", optimizationTest.ID, optimizationTest.IterationNumber));

            // runs one iteration of the optim algorithm according to the tests results
            optimizationTest.Run();
        }

        protected abstract IStochasticOptimizationTest CreateStochasticOptimTest(uint optimTestNumber);

        protected override void ResetTest()
        {
            base.ResetTest();
            this.currentOptimTests.Clear();
        }
    }
}