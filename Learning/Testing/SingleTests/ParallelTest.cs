// ------------------------------------------
// ParallelTest.cs, Learning
// 
// Created by Pedro Sequeira, 2013/12/09
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System.Linq;
using Learning.Testing.Config.Parameters;
using Learning.Testing.Config.Scenarios;
using Learning.Testing.Simulations;
using PS.Utilities;

namespace Learning.Testing.SingleTests
{
    public abstract class ParallelTest : SingleTest
    {
        protected readonly object locker = new object();

        protected ParallelTest(IScenario scenario, ITestParameters testParameters)
            : base(scenario, testParameters)
        {
        }

        public bool StopAllSimulations { get; set; }

        public override double ProgressValue
        {
            get
            {
                lock (this.locker)
                {
                    var simulationSteps =
                        this.currentSimulations.Keys.Sum(
                            curSimulation => (int) curSimulation.Agent.LongTermMemory.TimeStep);
                    var curStep = ((this.curSimulationIDx - this.currentSimulations.Count)*this.TestsConfig.NumTimeSteps) +
                                  simulationSteps;
                    var maxSteps = this.TestsConfig.NumTimeSteps*this.TestsConfig.NumSimulations;

                    return (double) curStep/maxSteps;
                }
            }
        }

        public override void RunTest()
        {
            //resets simulation counter
            this.curSimulationIDx = 0;

            //runs run-simulation-method threads for each processor and waits for them to finish
            ProcessUtil.RunThreads(this.RunSingleSimulation, this.TestsConfig.MaxCPUsUsed);
        }

        protected virtual void RunSingleSimulation()
        {
            //loop until simulations end or external stop signal is given 
            while (!this.TestHasFinished())
            {
                Simulation nextSimulation;

                //lock on to take next simulation from list
                lock (this.locker)
                {
                    //gets next simulation if possible, or else returns (ends thread)
                    if (this.curSimulationIDx >= this.TestsConfig.NumSimulations) return;

                    //creates simulation, sets params and adds to current list
                    nextSimulation = this.CreateAndSetupSimulation();
                    //this.SetSimulationParams(nextSimulation);
                    this.currentSimulations.Add(nextSimulation, this.curSimulationIDx++);
                }

                //runs next simulation
                this.RunSimulation(nextSimulation);
            }
        }

        protected override bool TestHasFinished()
        {
            return this.StopAllSimulations;
        }

        protected override void AverageTestStatistics(Simulation simulation)
        {
            lock (this.locker)
                base.AverageTestStatistics(simulation);
        }
    }
}