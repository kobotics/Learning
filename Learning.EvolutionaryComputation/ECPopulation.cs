// ------------------------------------------
// ECPopulation.cs, Learning.EvolutionaryComputation
//
// Created by Pedro Sequeira, 2013/2/6
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using System.Collections.Generic;
using AForge.Genetic;
using Learning.EvolutionaryComputation.Testing;

namespace Learning.EvolutionaryComputation
{
    [Serializable]
    public class ECPopulation : Population
    {
        protected const double DEFAULT_CROSSOVER_RATE = 1; //all parents are paired
        protected const double DEFAULT_MUTATION_RATE = 0.1;
        protected readonly IECTestsConfig testsConfig;

        public ECPopulation(
            IECTestsConfig testsConfig, IECChromosome ancestor, FitnessFunction fitnessFunction) :
                base(testsConfig.NumTestsPerIteration, ancestor, fitnessFunction, testsConfig.SelectionMethod)
        {
            this.testsConfig = testsConfig;

            //sets population ref to all chromosomes
            ancestor.Population = this;
            for (var i = 0; i < this.Size; i++)
                ((IECChromosome) this[i]).Population = this;

            //default values
            this.CrossoverRate = DEFAULT_CROSSOVER_RATE;
            this.MutationRate = DEFAULT_MUTATION_RATE;
            this.RandomSelectionPortion = testsConfig.RandomSelectionPortion;
        }

        public List<IChromosome> Chromosomes
        {
            get { return this.population; }
        }

        public int GenerationNumber { get; set; }

        public override void Selection()
        {
            //amount of random chromosomes in the new population
            var randomAmount = (int) (this.RandomSelectionPortion*this.Size);

            //amount of top chromosomes in the new population to keep
            var steadyStateAmount = (int) (this.testsConfig.SteadyStatePortion*this.Size);

            //do selection
            this.SelectionMethod.ApplySelection(population, this.Size - randomAmount - steadyStateAmount);

            //add random chromosomes
            this.AddRandomChromosomes(randomAmount);
        }

        public virtual IEnumerable<IChromosome> GetTopChromosomes(int steadyStateAmount)
        {
            // sort chromosomes
            this.population.Sort();

            // returns top chromosomes
            var topChromosomes = new IChromosome[steadyStateAmount];
            this.population.CopyTo(0, topChromosomes, 0, steadyStateAmount);
            return topChromosomes;
        }

        protected virtual void AddRandomChromosomes(int randomAmount)
        {
            if (randomAmount <= 0) return;

            var ancestor = this.population[0];

            for (var i = 0; i < randomAmount; i++)
            {
                // create new chromosome
                var c = ancestor.CreateNew();
                // calculate it's fitness
                c.Evaluate(this.FitnessFunction);
                // add it to population
                this.population.Add(c);
            }
        }

        public void ShuffleSort()
        {
            this.population.Sort((a, b) => rand.Next(-1, 2));
        }

        public void AddRange(IEnumerable<IChromosome> chromosomes)
        {
            this.population.AddRange(chromosomes);
        }

        public void UpdateBestChromosome()
        {
            this.FindBestChromosome();
        }
    }
}