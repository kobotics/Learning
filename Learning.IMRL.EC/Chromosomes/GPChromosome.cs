// ------------------------------------------
// GPChromosome.cs, Learning.IMRL.EC
// 
// Created by Pedro Sequeira, 2013/03/26
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using System.Collections.Generic;
using AForge.Genetic;
using Learning.EvolutionaryComputation;
using PS.Utilities.Conversion;

namespace Learning.IMRL.EC.Chromosomes
{
    [Serializable]
    public class GPChromosome : ECChromosome, IGPChromosome
    {
        protected GPChromosome(IChromosome baseChromosome, ECPopulation population) : base(population)
        {
            this.BaseChromosome = baseChromosome;
        }

        public GPChromosome(IGPGene ancestor) : base(null)
        {
            this.BaseChromosome = new GPTreeChromosome(ancestor);
        }

        #region IGPChromosome Members

        public IChromosome BaseChromosome { get; set; }

        public virtual HashSet<IGPChromosome> AllCombinations
        {
            get { return Util.GenerateAllCombinations(this); }
        }

        public uint Length
        {
            get
            {
                var originalExpression = this.ToString();
                return string.IsNullOrWhiteSpace(originalExpression)
                    ? 0
                    : (uint) Util.GetNodeStack(originalExpression).Count;
            }
        }

        public uint Depth
        {
            get { return Util.GetDepth(this); }
        }

        public override int CompareTo(object obj)
        {
            if (!(obj is GPChromosome)) return -1;
            return this.CompareTo((GPChromosome) obj);
        }

        public override int CompareTo(IECChromosome chromosome)
        {
            if (!(chromosome is GPChromosome)) return -1;
            return
                this.Population == chromosome.Population
                    ? this.Length.CompareTo(((GPChromosome) chromosome).Length)
                    : -1;
        }

        public override void Generate()
        {
            this.BaseChromosome.Generate();
        }

        public override IChromosome CreateNew()
        {
            var newChromosome = this.BaseChromosome.CreateNew();
            return new GPChromosome(newChromosome, this.Population);
        }

        public override IChromosome Clone()
        {
            return new GPChromosome(this.BaseChromosome.Clone(), this.Population);
        }

        public override bool FromValue(string[] value)
        {
            IChromosome baseChromosome;
            if (!ValueConverter.Convert(out baseChromosome, value[0])) return false;
            this.BaseChromosome = baseChromosome;
            return true;
        }

        public override void Mutate()
        {
            this.BaseChromosome.Mutate();
        }

        public override void Crossover(IChromosome pair)
        {
            if (pair is GPChromosome)
                this.BaseChromosome.Crossover(((GPChromosome) pair).BaseChromosome);
        }

        #endregion

        public override string ToString()
        {
            return this.BaseChromosome.ToString();
        }
    }
}