// ------------------------------------------
// FitnessSimulation.cs, Learning
// 
// Created by Pedro Sequeira, 2013/12/09
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using Learning.Domain.Agents;
using Learning.Testing.Config.Scenarios;
using PS.Utilities.Math;

namespace Learning.Testing.Simulations
{
    public class FitnessSimulation : Simulation
    {
        public FitnessSimulation(IAgent agent, IFitnessScenario scenario)
            : base(agent, scenario)
        {
            //controls agent fitness from here
            agent.Fitness = this.CreateStatisticalQuantity();
        }

        public new IFitnessScenario Scenario
        {
            get { return base.Scenario as IFitnessScenario; }
        }

        public override StatisticalQuantity Score
        {
            get { return this.Agent.Fitness; }
        }

        protected StatisticalQuantity CreateStatisticalQuantity()
        {
            return new StatisticalQuantity(this.TestsConfig.NumSamples)
                   {
                       SampleSteps = this.TestsConfig.NumTimeSteps/this.TestsConfig.NumSamples
                   };
        }

        public override void Dispose()
        {
            base.Dispose();
            //this.Agent.Environment.Dispose();
            this.Score.Dispose();
        }

        public override void Update()
        {
            base.Update();

            //updates agent fitness
            this.Scenario.AgentFitnessFunction.UpdateCurrentFitness(this.Agent);
        }
    }
}