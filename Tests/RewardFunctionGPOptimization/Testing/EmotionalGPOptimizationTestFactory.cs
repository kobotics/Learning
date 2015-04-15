// ------------------------------------------
// EmotionalGPOptimizationTestFactory.cs, Learning.Tests.RewardFunctionGPOptimization
// 
// Created by Pedro Sequeira, 2015/04/02
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using Learning.IMRL.EC.Testing.MultipleTests;
using Learning.IMRL.Testing;
using Learning.Testing.Config.Parameters;
using Learning.Testing.Config.Scenarios;
using Learning.Testing.SingleTests;

namespace Learning.Tests.RewardFunctionGPOptimization.Testing
{
    public class EmotionalGPOptimizationTestFactory : GPOptimizationTestFactory
    {
        public EmotionalGPOptimizationTestFactory(IFitnessScenario scenario) : base(scenario)
        {
        }

        public override FitnessTest CreateTest(ITestParameters parameters)
        {
            var singleTest = new IMRLTest((IFitnessScenario) this.Scenario.Clone(), parameters);
            singleTest.Reset();
            return singleTest;
        }
    }
}