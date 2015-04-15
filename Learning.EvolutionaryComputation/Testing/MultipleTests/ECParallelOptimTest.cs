// ------------------------------------------
// ECParallelOptimTest.cs, Learning.EvolutionaryComputation
// 
// Created by Pedro Sequeira, 2014/03/10
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System.IO;
using Learning.Testing.MultipleTests;
using Learning.Testing.StochasticOptimization;
using PS.Utilities.Math;

namespace Learning.EvolutionaryComputation.Testing.MultipleTests
{
    public class ECParallelOptimTest : StochasticParallelOptimTest
    {
        private const string TEST_ID = "EvoOptimization";

        public ECParallelOptimTest(IOptimizationTestFactory optimizationTestFactory)
            : base(optimizationTestFactory)
        {
        }

        public StatisticalQuantity RandomProportionProgressAvg { get; protected set; }

        protected new IECTestsConfig TestsConfig
        {
            get { return base.TestsConfig as IECTestsConfig; }
        }

        public override string TestID
        {
            get { return TEST_ID; }
        }

        protected override void PrintTestPerformanceResults()
        {
            base.PrintTestPerformanceResults();
            this.RandomProportionProgressAvg.PrintStatisticsToCSV(
                string.Format("{0}{1}RandomProportionAvg.csv", this.FilePath, Path.DirectorySeparatorChar), false, true);
        }

        public override void Dispose()
        {
            base.Dispose();
            this.RandomProportionProgressAvg.Dispose();
        }

        protected override void StoreOptimTestStatistics(IStochasticOptimizationTest optimizationTest)
        {
            base.StoreOptimTestStatistics(optimizationTest);

            lock (this.locker)
            {
                var ecOptimizationTest = (ECOptimizationTest) optimizationTest;

                //stores or averages populations' statistics
                if (this.RandomProportionProgressAvg == null)
                    this.RandomProportionProgressAvg = ecOptimizationTest.RandomProportionProgress;
                else
                    this.RandomProportionProgressAvg.AverageWith(ecOptimizationTest.RandomProportionProgress);
            }
        }

        protected override IStochasticOptimizationTest CreateStochasticOptimTest(uint optimTestNumber)
        {
            this.WriteLine(@"__________________________________________");
            this.WriteLine(string.Format("Creating population {0}...", optimTestNumber));

            return new ECOptimizationTest(string.Format("Population {0}", optimTestNumber),
                this.TestsConfig, this.TestsConfig.CreateBaseChromosome(), this.CreateFitnessFunction());
        }

        private FitnessFunction CreateFitnessFunction()
        {
            //create fitness function
            return new FitnessFunction(this.OptimizationTestFactory, (ECTestMeasureList) this.TestMeasures)
                   {
                       LogWriter = this.LogWriter
                   };
        }
    }
}