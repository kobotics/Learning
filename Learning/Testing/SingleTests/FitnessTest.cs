// ------------------------------------------
// FitnessTest.cs, Learning
// 
// Created by Pedro Sequeira, 2013/12/09
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using Learning.Domain.Agents;
using Learning.Domain.Environments;
using Learning.Testing.Config.Parameters;
using Learning.Testing.Config.Scenarios;
using Learning.Testing.Simulations;

namespace Learning.Testing.SingleTests
{
    public class FitnessTest : ParallelTest
    {
        #region Constructor

        public FitnessTest(IFitnessScenario scenario, ITestParameters testParameters)
            : base(scenario, testParameters)
        {
        }

        #endregion

        #region Properties

        protected new IFitnessScenario Scenario
        {
            get { return base.Scenario as IFitnessScenario; }
        }

        #endregion

        #region Creation Methods

        protected virtual IAgent CreateAgent()
        {
            //gets new agent from factory
            return this.Scenario.CreateAgent();
        }

        protected virtual IEnvironment CreateEnvironment()
        {
            //gets new environment from factory
            return this.Scenario.CreateEnvironment();
        }

        protected virtual Simulation CreateSimulation(IAgent agent)
        {
            return new FitnessSimulation(agent, this.Scenario);
        }

        public override Simulation CreateAndSetupSimulation()
        {
            //sets up and initiates agent
            var agent = this.CreateAgent();
            agent.StatisticsCollection.SampleSteps = this.TestsConfig.NumTimeSteps/this.TestsConfig.NumSamples;
            agent.StatisticsCollection.MaxNumSamples = this.TestsConfig.NumSamples;

            //creates new environment and cells
            var environment = this.CreateEnvironment();
            if (environment != null)
            {
                environment.CreateCells(3, 3, this.Scenario.EnvironmentConfigFile);
                ((SingleAgentEnvironment) environment).Agent = (CellAgent) agent;
                environment.Scenario = this.Scenario;
                environment.Init();
                environment.Reset();
            }

            this.SetAgentParameters(agent);

            return this.CreateSimulation(agent);
        }

        protected virtual void SetAgentParameters(IAgent agent)
        {
            //inits agent
            agent.Scenario = this.Scenario;
            agent.TestParameters = this.TestParameters;
            agent.Init();
        }

        #endregion
    }
}