// ------------------------------------------
// IGPTestsConfig.cs, Learning.IMRL.EC
// 
// Created by Pedro Sequeira, 2013/12/18
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System.Collections.Generic;
using Learning.EvolutionaryComputation.Testing;
using Learning.IMRL.EC.Genes;
using Learning.IMRL.EC.Testing.MultipleTests;
using Learning.Testing.Config.Scenarios;

namespace Learning.IMRL.EC.Testing
{
    public interface IGPTestsConfig : IECTestsConfig
    {
        uint NumBaseVariables { get; set; }
        double[] Constants { get; set; }
        List<FunctionType> AllowedFunctions { get; set; }
        int MaxProgTreeDepth { get; set; }
        int MaxInitialLevel { get; set; }

        IGPSimplifierOptimizationTestFactory CreateSimplifierTestFactory(
            IScenario scenario, uint numSimulations, uint numSamples);
    }
}