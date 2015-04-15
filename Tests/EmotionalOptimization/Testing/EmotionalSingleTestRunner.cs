// ------------------------------------------
// EmotionalSingleTestRunner.cs, Learning.Tests.EmotionalOptimization
// 
// Created by Pedro Sequeira, 2012/10/17
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using Learning.Forms.Simulation;
using Learning.Forms.Testing;
using Learning.Testing.Config;
using Learning.Testing.SingleTests;
using Learning.Tests.EmotionalOptimization.Forms;

namespace Learning.Tests.EmotionalOptimization.Testing
{
    public class EmotionalSingleTestRunner : FormsSingleTestRunner
    {
        public EmotionalSingleTestRunner(ITestsConfig testsConfig) : base(testsConfig)
        {
        }

        protected override ISimulationForm CreateSimulationForm(FitnessTest test, bool limitedUser)
        {
            return test.Scenario is PacmanScenario
                ? new PacmanLightSimForm(test)
                : base.CreateSimulationForm(test, limitedUser);
        }
    }
}