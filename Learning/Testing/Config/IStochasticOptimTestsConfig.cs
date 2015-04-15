// ------------------------------------------
// IStochasticOptimTestsConfig.cs, Learning
//
// Created by Pedro Sequeira, 2014/2/14
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
namespace Learning.Testing.Config
{
    public interface IStochasticOptimTestsConfig : ITestsConfig
    {
        int NumTestsPerIteration { get; set; }
        uint MaxIterations { get; set; }
        uint NumParallelOptimTests { get; set; }
        uint FitnessImprovementThreshold { get; set; }
    }
}