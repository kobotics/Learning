// ------------------------------------------
// StochasticTest.cs, Learning.Emotions
// 
// Created by Pedro Sequeira, 2013/12/18
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using Learning.Testing.Config.Parameters;
using Learning.Testing.Config.Scenarios;
using Learning.Testing.Simulations;

namespace Learning.IMRL.Emotions.Testing
{
    public class StochasticTest : EmotionalTest
    {
        public StochasticTest(IStochasticScenario scenario, ArrayParameter testParameters)
            : base(scenario, testParameters)
        {
        }

        public new IStochasticScenario Scenario
        {
            get { return base.Scenario as IStochasticScenario; }
        }

        public override Simulation CreateAndSetupSimulation()
        {
            //sets up and initiates agent
            var agent = this.CreateAgent();
            this.SetAgentParameters(agent);
            return new FitnessSimulation(agent, this.Scenario);
        }
    }
}