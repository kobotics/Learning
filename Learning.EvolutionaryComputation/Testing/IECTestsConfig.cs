// ------------------------------------------
// IECTestsConfig.cs, Learning.EvolutionaryComputation
//
// Created by Pedro Sequeira, 2013/12/18
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using AForge.Genetic;
using Learning.Testing.Config;

namespace Learning.EvolutionaryComputation.Testing
{
    public interface IECTestsConfig : IStochasticOptimTestsConfig
    {
        ISelectionMethod SelectionMethod { get; set; }
        double RandomSelectionPortion { get; set; }
        double SteadyStatePortion { get; set; }
        double SymmetryFactor { get; set; }
        int StdDevTimes { get; set; }

        IECChromosome CreateBaseChromosome();
    }
}