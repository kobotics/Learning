// ------------------------------------------
// IMRLSimAnnOptimTest.cs, Learning.IMRL
// 
// Created by Pedro Sequeira, 2014/02/14
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using Learning.Testing;
using Learning.Testing.Config.Parameters;
using Learning.Testing.MultipleTests;
using Learning.Testing.StochasticOptimization;

namespace Learning.IMRL.Testing
{
    public class IMRLSimAnnOptimTest : SimulatedAnnealingOptimTest
    {
        public IMRLSimAnnOptimTest(
            string id, IOptimizationTestFactory optimizationTestFactory, TestMeasureList testMeasures)
            : base(id, optimizationTestFactory, testMeasures)
        {
        }

        protected override ITestParameters ComputeNeighbour(ITestParameters currentParam)
        {
            //first tested params
            if (currentParam == null)
                return this.testsConfig.SingleTestParameters;

            //define search radius based on temperature
            var next = (ArrayParameter) currentParam.Clone();
            var initialTemperature = this.TestsConfig.InitialTemperature;
            var searchRadius = 2*Random.NextDouble()*(1d - ((initialTemperature - temperature)/initialTemperature));

            //generate random point in hyper-sphere according to radius
            for (var i = 0; i < next.Length; i++)
                //"moves" current point in the direction of that point
                next[i] = ((ArrayParameter) currentParam)[i] + (searchRadius*((Random.NextDouble()*2) - 1d));

            //normalizes result
            next.NormalizeUnitSum();
            next.Round();

            return next;
        }
    }
}