// ------------------------------------------
// IOptimizationTestFactory.cs, Learning
// 
// Created by Pedro Sequeira, 2013/12/09
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using Learning.Testing.Config.Parameters;
using Learning.Testing.Config.Scenarios;
using Learning.Testing.SingleTests;

namespace Learning.Testing.MultipleTests
{
    public interface IOptimizationTestFactory
    {
        IFitnessScenario Scenario { get; }
        TestMeasureList CreateAndInitTestMeasureList();
        TestMeasureList CreateTestMeasureList();
        FitnessTest CreateTest(ITestParameters parameters);
        TestMeasure CreateTestMeasure(FitnessTest test);
    }
}