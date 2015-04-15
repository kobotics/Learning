// ------------------------------------------
// IGPSimplifierOptimizationTestFactory.cs, Learning.IMRL.EC
//
// Created by Pedro Sequeira, 2013/3/27
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System.Collections.Generic;
using Learning.EvolutionaryComputation.Testing;
using Learning.IMRL.EC.Chromosomes;
using Learning.Testing.Config;
using Learning.Testing.Config.Parameters;
using Learning.Testing.MultipleTests;

namespace Learning.IMRL.EC.Testing.MultipleTests
{
    public interface IGPSimplifierOptimizationTestFactory : IOptimizationTestFactory
    {
        Dictionary<ECTestMeasure, uint> MinLengthChromosome { get; }
        Dictionary<ECTestMeasure, List<ECTestMeasure>> SimplifiedChromosomesMeasures { get; }
        Dictionary<IGPChromosome, ECTestMeasure> SimplifiedParamMeasures { get; }
        List<ITestParameters> DetermineChromosomeParameters(ECTestMeasureList testMeasureList);
    }
}