// ------------------------------------------
// IStochasticOptimizationTest.cs, Learning
//
// Created by Pedro Sequeira, 2014/2/14
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using PS.Utilities.Math;

namespace Learning.Testing.StochasticOptimization
{
    public interface IStochasticOptimizationTest :ITest
    {
        StatisticalQuantity FitnessMaxProgress { get; }
        string ID { get; }
        int IterationNumber { get; set; }
        double MaxFitness { get; }
        bool Terminated { get; set; }
        double FitnessImprovementThreshold { get; }
    }
}