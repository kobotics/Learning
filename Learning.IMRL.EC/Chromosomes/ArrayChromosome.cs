// ------------------------------------------
// ArrayChromosome.cs, Learning.IMRL.EC
// 
// Created by Pedro Sequeira, 2014/01/10
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using AForge.Genetic;
using Learning.EvolutionaryComputation;
using Learning.Testing.Config.Parameters;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.Random;
using PS.Utilities.Collections;

namespace Learning.IMRL.EC.Chromosomes
{
    [Serializable]
    public class ArrayChromosome : ECChromosome, IArrayChromosome
    {
        [NonSerialized] protected static readonly Random Rand = new WH2006(true);
        protected ArrayParameter arrayParameter;
        protected IContinuousDistribution[] randomGenerators;

        public ArrayChromosome(ArrayChromosome source) : base(source.Population)
        {
            this.randomGenerators = source.randomGenerators;
            this.MutationBalancer = source.MutationBalancer;
            this.CrossoverBalancer = source.CrossoverBalancer;
            this.arrayParameter = (ArrayParameter) source.arrayParameter.Clone();
        }

        public ArrayChromosome(ArrayParameter arrayParameter)
            : this(null, arrayParameter)
        {
        }

        public ArrayChromosome(ECPopulation population, ArrayParameter arrayParameter)
            : base(population)
        {
            this.MutationBalancer = 0.5;
            this.CrossoverBalancer = 0.5;
            this.randomGenerators = this.CreateRandomGenerators(arrayParameter.Domains);
            this.arrayParameter = (ArrayParameter) arrayParameter.Clone();
            this.Generate();
        }

        #region IArrayChromosome Members

        public StepInterval<double>[] Domains
        {
            get { return this.arrayParameter.Domains; }
        }

        public override void Generate()
        {
            for (var i = 0; i < this.arrayParameter.Length; i++)
                this.arrayParameter[i] = randomGenerators[i].Sample();

            this.arrayParameter.NormalizeUnitSum();
            this.arrayParameter.Round();
        }

        public override void Mutate()
        {
            var mutationGene = Rand.Next((int) this.arrayParameter.Length);

            if (Rand.NextDouble() < this.MutationBalancer)
            {
                var next = this.randomGenerators[mutationGene].Sample();
                this.arrayParameter[mutationGene] *= (Rand.NextDouble() < 0.5) ? next : 1/next;
            }
            else
            {
                var next = this.randomGenerators[mutationGene].Sample();
                this.arrayParameter[mutationGene] += (Rand.NextDouble() < 0.5) ? next : -next;
            }

            this.arrayParameter.NormalizeUnitSum();
            this.arrayParameter.Round();
        }

        public override void Crossover(IChromosome pair)
        {
            if (!(pair is ArrayChromosome)) return;
            var p = (ArrayChromosome) pair;

            // check for correct pair
            var length = this.arrayParameter.Length;
            if (p.arrayParameter.Length != length) return;

            if (Rand.NextDouble() < this.CrossoverBalancer)
                this.PointCrossover(p);
            else
                this.ApproximateCrossover(p);

            //normalizes and rounds after crossover
            this.arrayParameter.NormalizeUnitSum();
            this.arrayParameter.Round();
            p.arrayParameter.NormalizeUnitSum();
            p.arrayParameter.Round();
        }

        public double this[int paramIdx]
        {
            get { return this.arrayParameter[paramIdx]; }
            set { this.arrayParameter[paramIdx] = value; }
        }

        public uint NumDecimalPlaces
        {
            get { return this.arrayParameter.NumDecimalPlaces; }
            set { this.arrayParameter.NumDecimalPlaces = value; }
        }

        public uint Length
        {
            get { return this.arrayParameter.Length; }
        }

        public double[] Array
        {
            get { return this.arrayParameter.Array; }
        }

        public double AbsoulteSum
        {
            get { return this.arrayParameter.AbsoulteSum; }
        }

        public double Sum
        {
            get { return this.arrayParameter.Sum; }
        }

        public void NormalizeVector()
        {
            this.arrayParameter.NormalizeVector();
        }

        public void NormalizeSum()
        {
            this.arrayParameter.NormalizeSum();
        }

        public void NormalizeUnitSum()
        {
            this.arrayParameter.NormalizeUnitSum();
        }

        public void Round()
        {
            this.arrayParameter.Round();
        }

        public void SetMidDomainValues()
        {
            this.arrayParameter.SetMidDomainValues();
        }

        public override IChromosome Clone()
        {
            return new ArrayChromosome(this);
        }

        public override string[] ToValue()
        {
            return this.arrayParameter.ToValue();
        }

        public override bool FromValue(string[] value)
        {
            return this.arrayParameter.FromValue(value);
        }

        public override string[] Header
        {
            get
            {
                var length = this.Array.Length;
                var header = new string[length];
                for (var i = 0; i < length; i++)
                    header[i] = string.Format("param{0}", i);
                return header;
            }
        }

        public IEnumerator<double> GetEnumerator()
        {
            return this.arrayParameter.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        protected void ApproximateCrossover(ArrayChromosome p)
        {
            var pairVal = p.arrayParameter;
            var factor = Rand.NextDouble();
            if (Rand.Next(2) == 0)
                factor = -factor;

            for (var i = 0; i < this.Length; i++)
            {
                var portion = (this.arrayParameter[i] - pairVal[i])*factor;
                this.arrayParameter[i] -= portion;
                pairVal[i] += portion;
            }
        }

        protected void PointCrossover(ArrayChromosome p)
        {
            // crossover point
            var crossOverPoint = Rand.Next((int) (this.Length - 1)) + 1;
            var crossOverLength = this.Length - crossOverPoint;

            // temporary array
            var temp = new double[crossOverLength];

            System.Array.Copy(this.arrayParameter, crossOverPoint, temp, 0, crossOverLength);
            System.Array.Copy(p.arrayParameter, crossOverPoint, this.arrayParameter, crossOverPoint,
                crossOverLength);
            System.Array.Copy(temp, 0, p.arrayParameter, crossOverPoint, crossOverLength);
        }

        protected IContinuousDistribution[] CreateRandomGenerators(StepInterval<double>[] ranges)
        {
            var randomGens = new IContinuousDistribution[ranges.Length];
            for (var i = 0; i < ranges.Length; i++)
                randomGens[i] = new ContinuousUniform(ranges[i].min, ranges[i].max);
            return randomGens;
        }

        public override string ToString()
        {
            return this.arrayParameter.ToString();
        }
    }
}