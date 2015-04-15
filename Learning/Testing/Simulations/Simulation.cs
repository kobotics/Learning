// ------------------------------------------
// Simulation.cs, Learning
//
// Created by Pedro Sequeira, 2013/12/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using Learning.Domain.Agents;
using PS.Utilities.Math;
using Learning.Testing.Config;
using Learning.Testing.Config.Scenarios;
using PS.Utilities;

namespace Learning.Testing.Simulations
{
    public abstract class Simulation : ISimulation, IProgressHandler, IUpdatable
    {
        #region Fields

        protected PerformanceMeasure performanceMeasure = new PerformanceMeasure();

        #endregion

        #region Constructor

        public Simulation(IAgent agent, IScenario scenario)
        {
            //checks arguments
            if (agent == null) throw new ArgumentNullException("agent", "Agent can't be null");

            this.Agent = agent;
            this.Scenario = scenario;
        }

        #endregion

        #region Properties

        public ITestsConfig TestsConfig
        {
            get { return this.Scenario.TestsConfig; }
        }

        #region IProgressHandler Members

        public virtual double ProgressValue
        {
            get { return (double) this.Agent.LongTermMemory.TimeStep/this.TestsConfig.NumTimeSteps; }
        }

        #endregion

        #region ISimulation Members

        public IScenario Scenario { get; private set; }

        public long MemoryUsage
        {
            get { return this.performanceMeasure.MemoryUsage; }
        }

        public TimeSpan TimeElapsed
        {
            get { return this.performanceMeasure.TimeElapsed; }
        }

        public IAgent Agent { get; set; }

        public LogWriter LogWriter { get; protected set; }
        public abstract StatisticalQuantity Score { get; }

        #endregion

        #endregion

        #region Public Methods

        public virtual bool Run()
        {
            this.StartSimulation();
            this.RunSimulation();
            this.FinishSimulation();

            return true;
        }

        public virtual void Reset()
        {
            this.performanceMeasure.Reset();
        }

        public virtual void Dispose()
        {
            this.Agent.Dispose();
        }

        #endregion

        #region Protected Methods

        public virtual void Update()
        {
            //updates agent
            this.Agent.Update();
        }

        public virtual void StartSimulation()
        {
            //starts performance measures
            this.performanceMeasure.Start();
        }

        public virtual void RunSimulation()
        {
            //updates simulation while simulation doesn't terminate
            while (!this.SimulationFinished())
                this.Update();
        }

        public virtual bool SimulationFinished()
        {
            //generic simulation terminates when agent performs maximum steps
            return this.Agent.LongTermMemory.TimeStep >= this.TestsConfig.NumTimeSteps;
        }

        public virtual void FinishSimulation()
        {
            //stops performance measures
            this.performanceMeasure.Stop();
        }

        #endregion
    }
}