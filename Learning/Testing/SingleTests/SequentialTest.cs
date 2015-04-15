// ------------------------------------------
// SequentialTest.cs, Learning
// 
// Created by Pedro Sequeira, 2013/12/09
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using Learning.Testing.Config.Parameters;
using Learning.Testing.Config.Scenarios;
using Learning.Testing.Simulations;

namespace Learning.Testing.SingleTests
{
    public abstract class SequentialTest : SingleTest
    {
        protected Simulation currentSimulation;

        protected SequentialTest(SingleScenario scenario, ITestParameters testParameters)
            : base(scenario, testParameters)
        {
        }

        public override double ProgressValue
        {
            get
            {
                var simulationSteps = (long) this.currentSimulation.Agent.LongTermMemory.TimeStep;
                var curStep = (this.curSimulationIDx*this.TestsConfig.NumTimeSteps) + simulationSteps;
                var maxSteps = this.TestsConfig.NumTimeSteps*this.TestsConfig.NumSimulations;

                return (double) curStep/maxSteps;
            }
        }

        public override void RunTest()
        {
            //runs episodes sequentially
            for (this.curSimulationIDx = 0;
                (this.curSimulationIDx < this.TestsConfig.NumSimulations) && !this.TestHasFinished();
                this.curSimulationIDx++)
            {
                //runs next simulation
                this.currentSimulation = this.CreateAndSetupSimulation();
                //this.SetSimulationParams(this.currentSimulation);
                this.RunSimulation(this.currentSimulation);
            }
        }
    }
}