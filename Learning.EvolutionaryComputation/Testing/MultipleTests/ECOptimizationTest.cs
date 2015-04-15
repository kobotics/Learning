// ------------------------------------------
// ECOptimizationTest.cs, Learning.EvolutionaryComputation
// 
// Created by Pedro Sequeira, 2014/02/14
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System.Collections.Generic;
using AForge.Genetic;
using Learning.Testing.StochasticOptimization;
using PS.Utilities.Math;

namespace Learning.EvolutionaryComputation.Testing.MultipleTests
{
    public class ECOptimizationTest : StochasticOptimzationTest
    {
        protected readonly ECPopulation population;

        public ECOptimizationTest(
            string id, IECTestsConfig testsConfig, IECChromosome ancestor, FitnessFunction fitnessFunction)
            : base(id, testsConfig)
        {
            this.population = new ECPopulation(testsConfig, ancestor, fitnessFunction);
            this.RandomProportionProgress = new StatisticalQuantity(testsConfig.MaxIterations);
        }

        public StatisticalQuantity RandomProportionProgress { get; private set; }

        protected override void RunIteration()
        {
            //updates generation number
            this.population.GenerationNumber = this.IterationNumber;

            //amount of top chromosomes in the new population to keep
            var steadyStateAmount = (int) (((IECTestsConfig) this.testsConfig).SteadyStatePortion*this.population.Size);

            //keep top chromosomes before running epoch
            var topChromosomes = this.population.GetTopChromosomes(steadyStateAmount);

            //shuffles elements
            this.population.ShuffleSort();

            //gets random proportion
            this.UpdateRandomProportion();

            //run epoch
            this.population.Crossover();
            this.population.Mutate();
            this.population.Selection();

            //add top chromosomes
            this.population.AddRange(topChromosomes);
        }

        protected override void CheckFitnessImprovement()
        {
            //updates max fitness based on population
            this.population.UpdateBestChromosome();
            this.MaxFitness = this.population.FitnessMax;

            base.CheckFitnessImprovement();
        }

        protected virtual void UpdateRandomProportion()
        {
            //calculates num of distinct chromosomes
            var distinctChromosomes = new HashSet<IChromosome>();
            foreach (var chromosome in this.population.Chromosomes)
                if (!distinctChromosomes.Contains(chromosome))
                    distinctChromosomes.Add(chromosome);

            //random amount proportional to the number of diff chromosomes
            var genFactor = (double) this.IterationNumber/this.testsConfig.MaxIterations;
            var chromFactor = (double) distinctChromosomes.Count/this.population.Size;
            this.RandomProportionProgress.Value =
                (this.population.MutationRate = this.population.RandomSelectionPortion =
                    (1d - chromFactor)*(1d - (genFactor*genFactor)));
        }
    }
}