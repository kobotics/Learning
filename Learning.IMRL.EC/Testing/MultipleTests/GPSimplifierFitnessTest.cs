// ------------------------------------------
// GPSimplifierFitnessTest.cs, Learning.IMRL.EC
// 
// Created by Pedro Sequeira, 2014/03/10
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System.Collections.Generic;
using System.IO;
using Learning.EvolutionaryComputation.Testing;
using Learning.IMRL.EC.Chromosomes;
using Learning.Testing.Config.Parameters;
using Learning.Testing.MultipleTests;
using Learning.Testing.SingleTests;

namespace Learning.IMRL.EC.Testing.MultipleTests
{
    public class GPSimplifierFitnessTest : ListFitnessTest
    {
        private const string SIMPLIFY_TEST_ID = "ExpressionSimplify";
        private const double MIN_P_VALUE = 0.2;

        #region Constructors

        public GPSimplifierFitnessTest(IGPSimplifierOptimizationTestFactory optimizationTestFactory)
            : base(optimizationTestFactory)
        {
            this.AddSpecialTestParams = false;
        }

        #endregion

        #region Protected Methods

        protected override void RunOptimizationTests()
        {
            //loop until chromosomes end or external stop request
            while (!this.StopAllTests)
            {
                IGPChromosome nextTestParams;
                ECTestMeasure originalGPTestMeasure;

                //lock on to take next chromosome from list
                lock (this.locker)
                {
                    //gets next test if possible, or else returns (ends thread)
                    if (this.currentTestIdx >= this.testParameters.Count) return;
                    if (this.testParameters.Count == 0) continue;
                    nextTestParams = (IGPChromosome) this.testParameters[this.currentTestIdx++];

                    //gets history associated with this simplified chromosome and compares lengths
                    originalGPTestMeasure =
                        this.OptimizationTestFactory.SimplifiedParamMeasures[nextTestParams];
                    var minHistLength = this.OptimizationTestFactory.MinLengthChromosome[originalGPTestMeasure];

                    //if new length is higher, ignore new chromosome as we have found a simpler version
                    if (nextTestParams.Length > minHistLength) continue;
                }

                //runs test
                this.RunSingleTest(nextTestParams);

                lock (this.locker)
                {
                    //calculates chromosomes' fitness diff. statistical signif.
                    var nextSimpChromosomeMeasure = (GPTestMeasure) this.TestMeasures.GetTestMeasure(nextTestParams);
                    var statisticallyDifferent = nextSimpChromosomeMeasure.IsStatiscallyDifferentFrom(
                        originalGPTestMeasure, (int) this.NumSimulations, MIN_P_VALUE);

                    //checks whether results are better than original expression or statistically insignificant
                    if ((nextSimpChromosomeMeasure.Value < originalGPTestMeasure.Value) ||
                        !statisticallyDifferent) continue;

                    //tests whether the length is minimal in relation to the original history's simplifications
                    if (this.OptimizationTestFactory.MinLengthChromosome[originalGPTestMeasure] >
                        ((IGPChromosome) nextSimpChromosomeMeasure.Parameters).Length)
                        this.OptimizationTestFactory.SimplifiedChromosomesMeasures[originalGPTestMeasure].Clear();

                    //sets minimal length and adds history to minimal list
                    this.OptimizationTestFactory.MinLengthChromosome[originalGPTestMeasure] =
                        ((IGPChromosome) nextSimpChromosomeMeasure.Parameters).Length;
                    this.OptimizationTestFactory.SimplifiedChromosomesMeasures[originalGPTestMeasure].Add(
                        nextSimpChromosomeMeasure);
                }
            }
        }

        protected override void PrepareSingleTest(FitnessTest test)
        {
            base.PrepareSingleTest(test);
            lock (this.locker)
                test.LogStatistics = this.TestsConfig.GraphicsEnabled;
        }

        #endregion

        #region Properties

        public override string TestID
        {
            get { return SIMPLIFY_TEST_ID; }
        }

        public new IGPSimplifierOptimizationTestFactory OptimizationTestFactory
        {
            get { return base.OptimizationTestFactory as IGPSimplifierOptimizationTestFactory; }
        }

        #endregion

        #region Public Methods

        protected override void DetermineParametersList(bool readFromFile = false)
        {
            this.testParameters = this.OptimizationTestFactory.DetermineChromosomeParameters(
                (ECTestMeasureList) this.TestMeasures);
        }

        public override void PrintResults()
        {
            base.PrintResults();

            //add histories of simplified versions of chromosomes if possible
            var simplifiedChromosomeParams = new HashSet<ITestParameters>();
            foreach (var simplifiedChromosomeHistories in this.OptimizationTestFactory.SimplifiedChromosomesMeasures)
            {
                //if no simplified version exists, adds original chromosome history
                if (simplifiedChromosomeHistories.Value.Count == 0)
                    simplifiedChromosomeParams.Add(simplifiedChromosomeHistories.Key.Parameters);
                else
                    foreach (var simplifiedChromosomeHistory in simplifiedChromosomeHistories.Value)
                        simplifiedChromosomeParams.Add(simplifiedChromosomeHistory.Parameters);
            }

            //only keep simplified chromosome histories
            this.TestMeasures.ClearExcept(simplifiedChromosomeParams);
            this.TestMeasures.PrintToFile(string.Format("{0}{1}{2}{3}.csv",
                this.FilePath, Path.DirectorySeparatorChar, SIMPLIFY_TEST_ID, TestsConfig.TestMeasuresName));
        }

        #endregion
    }
}