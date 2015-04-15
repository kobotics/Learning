// ------------------------------------------
// IMRLSimAnnStochasticParallelOptimTest.cs, Learning.IMRL
// 
// Created by Pedro Sequeira, 2014/02/14
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using Learning.Testing.MultipleTests;
using Learning.Testing.StochasticOptimization;

namespace Learning.IMRL.Testing
{
    public class IMRLSimAnnStochasticParallelOptimTest : StochasticParallelOptimTest
    {
        protected uint curNumPopulations;

        public IMRLSimAnnStochasticParallelOptimTest(IOptimizationTestFactory optimizationTestFactory)
            : base(optimizationTestFactory)
        {
        }

        protected override IStochasticOptimizationTest CreateStochasticOptimTest(uint optimTestNumber)
        {
            return new IMRLSimAnnOptimTest(
                string.Format("Test{0}", optimTestNumber), this.OptimizationTestFactory, this.TestMeasures);
        }
    }
}