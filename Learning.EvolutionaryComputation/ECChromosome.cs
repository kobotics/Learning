// ------------------------------------------
// ECChromosome.cs, Learning.EvolutionaryComputation
// 
// Created by Pedro Sequeira, 2014/01/11
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using AForge.Genetic;
using Learning.Testing.Config.Parameters;
using PS.Utilities;

namespace Learning.EvolutionaryComputation
{
    [Serializable]
    public abstract class ECChromosome : IECChromosome
    {
        [NonSerialized] private ECPopulation _population;

        protected ECChromosome(ECPopulation population)
        {
            this.Population = population;
        }

        #region IECChromosome Members

        public double Fitness { get; set; }

        public ECPopulation Population
        {
            get { return this._population; }
            set { this._population = value; }
        }

        public double CrossoverBalancer { get; set; }
        public double MutationBalancer { get; set; }
        public abstract void Generate();

        public virtual IChromosome CreateNew()
        {
            var chromosome = this.Clone();
            chromosome.Generate();
            return chromosome;
        }

        public abstract IChromosome Clone();
        public abstract void Mutate();
        public abstract void Crossover(IChromosome pair);

        public void Evaluate(IFitnessFunction function)
        {
            this.Fitness = function.Evaluate(this);
        }

        public bool Equals(ITestParameters other)
        {
            return (other is ECChromosome) &&
                   this.ToString().Equals(other.ToString());
        }

        public virtual int CompareTo(object obj)
        {
            if (!(obj is ECChromosome)) return -1;
            return CompareTo((ECChromosome) obj);
        }

        public virtual int CompareTo(IECChromosome chromosome)
        {
            if (!(chromosome is ECChromosome)) return -1;
            return this.Population == chromosome.Population ? 0 : -1;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public virtual string[] ToValue()
        {
            return new[] {this.ToString().ToLiteral()};
        }

        public abstract bool FromValue(string[] value);

        public virtual string[] Header
        {
            get { return new[] {"Chromosome"}; }
        }

        #endregion

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return (obj is ECChromosome) && this.Equals((ECChromosome) obj);
        }
    }
}