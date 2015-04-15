// ------------------------------------------
// FitnessFunction.cs, Learning.EvolutionaryComputation
// 
// Created by Pedro Sequeira, 2013/02/06
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using AForge.Genetic;
using Learning.EvolutionaryComputation.Testing;
using Learning.Testing.MultipleTests;
using PS.Utilities;

namespace Learning.EvolutionaryComputation
{
    public class FitnessFunction : IFitnessFunction
    {
        #region Constructors

        public FitnessFunction(
            IOptimizationTestFactory optimizationTestFactory, ECTestMeasureList testMeasureList)
        {
            this.optimizationTestFactory = optimizationTestFactory;
            this.testMeasureList = testMeasureList;
        }

        #endregion

        #region IFitnessFunction Members

        #region Public Methods

        public virtual double Evaluate(IChromosome chromosome)
        {
            //checks argument
            if (!(chromosome is IECChromosome))
                throw new ArgumentException(@"Parameter is not IECChromosome", "chromosome");

            var chromosomeParam = (IECChromosome) chromosome;

            //checks whether test has been already executed, updating information
            if (this.testMeasureList.Contains(chromosomeParam))
                this.testMeasureList.UpdateTestMeasure(chromosomeParam);
            else
            {
                //create new test with given chromosome params
                var test = this.optimizationTestFactory.CreateTest(chromosomeParam);

                //runs test
                test.Run();

                //prints test results
                if (this.PrintResults) test.PrintResults();

                this.testMeasureList.Add(chromosomeParam, this.optimizationTestFactory.CreateTestMeasure(test));
            }

            var testMeasure = (ECTestMeasure) this.testMeasureList.GetTestMeasure(chromosomeParam);

            this.WriteLine(
                string.Format("\"{0}\": {1:0.00}±{2:0.00}",
                    testMeasure.ID, testMeasure.Value, testMeasure.StdDev));
            return testMeasure.Value;
        }

        #endregion

        #endregion

        #region Protected Methods

        protected void WriteLine(string line)
        {
            if (this.LogWriter != null)
                this.LogWriter.WriteLine(line);
        }

        #endregion

        #region Fields

        protected readonly IOptimizationTestFactory optimizationTestFactory;
        protected readonly ECTestMeasureList testMeasureList;

        #endregion

        #region Properties

        public LogWriter LogWriter { get; set; }

        public bool PrintResults { get; set; }

        #endregion
    }
}