// ------------------------------------------
// StochasticOptimzationTest.cs, Learning
//
// Created by Pedro Sequeira, 2014/2/14
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using PS.Utilities.Math;
using Learning.Testing.Config;
using PS.Utilities;

namespace Learning.Testing.StochasticOptimization
{
    public abstract class StochasticOptimzationTest : IStochasticOptimizationTest
    {
        protected const uint MINIMUM_IMPROV_THRESHOLD = 2;
        protected const uint DEFAULT_IMPROV_THRESHOLD = 10;
        protected const double IMPROV_THRESHOLD_PENALTY_FACTOR = 4d;
        protected readonly IStochasticOptimTestsConfig testsConfig;
        protected double curFitImprovThreshold;
        protected double lastFitnessMax = double.MinValue;
        protected uint numGenWithoutImprov;

        protected StochasticOptimzationTest(string id, IStochasticOptimTestsConfig testsConfig)
        {
            this.ID = id;
            this.testsConfig = testsConfig;
            this.FitnessMaxProgress = new StatisticalQuantity(testsConfig.MaxIterations);
            this.curFitImprovThreshold = testsConfig.FitnessImprovementThreshold;
        }

        #region IStochasticOptimizationTest Members

        public double FitnessImprovementThreshold
        {
            get { return (uint) System.Math.Max(this.curFitImprovThreshold, MINIMUM_IMPROV_THRESHOLD); }
        }

        public StatisticalQuantity FitnessMaxProgress { get; private set; }

        public string ID { get; protected set; }

        public int IterationNumber { get; set; }

        public double MaxFitness { get; protected set; }

        public bool Terminated { get; set; }

        public long MemoryUsage
        {
            get { return 0; }
        }

        public TimeSpan TestSpeed
        {
            get { return new TimeSpan(); }
        }

        public string FilePath { get; set; }

        public LogWriter LogWriter { get; set; }

        public bool Run()
        {
            //checks terminated
            if (this.Terminated) return true;

            //optimizes fitness (one step)
            this.RunIteration();

            //checks fit improv
            this.CheckFitnessImprovement();

            //increments generation and checks improvements on fitness
            this.IterationNumber++;

            return false;
        }

        public void PrintResults()
        {
        }

        public virtual void Dispose()
        {
        }

        #endregion

        protected abstract void RunIteration();

        protected virtual void CheckFitnessImprovement()
        {
            //compares with last max fitness
            if (this.MaxFitness > this.lastFitnessMax)
            {
                //updates improv threshold accordingly (greater interval, even smaller threshold)
                this.curFitImprovThreshold -= this.numGenWithoutImprov/IMPROV_THRESHOLD_PENALTY_FACTOR;

                //resets counter and updates max fitness
                this.numGenWithoutImprov = 0;
                this.lastFitnessMax = this.MaxFitness;
            }
            else if (++this.numGenWithoutImprov >= this.FitnessImprovementThreshold)
            {
                //threshold passed
                this.Terminated = true;
            }

            //updates max fitness quantity
            this.FitnessMaxProgress.Value = this.MaxFitness;
        }
    }
}