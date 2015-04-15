// ------------------------------------------
// Policy.cs, Learning
//
// Created by Pedro Sequeira, 2014/5/23
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Random;
using PS.Utilities.Collections;

namespace Learning.Domain.Actions
{
    public class Policy : IPolicy
    {
        protected static readonly Random Random = new WH2006();
        private readonly DenseVector _distribution;

        public Policy(uint numActions, bool random)
        {
            this._distribution = new DenseVector((int) numActions);

            //just initialize a random or uniform policy
            for (var i = 0; i < this._distribution.Count; i++)
                this._distribution[i] = random ? Random.NextDouble() : 1;

            //normalize distribution
            this.Normalize();
        }

        public Policy(double[] distribution)
        {
            this._distribution = distribution;
        }

        #region IPolicy Members

        public double this[uint actionIdx]
        {
            get { return this._distribution[(int) actionIdx]; }
            set { this._distribution[(int) actionIdx] = value; }
        }

        public uint NumActions
        {
            get { return (uint) this._distribution.Count; }
        }

        public double GetActionProbability(uint actionID)
        {
            return this.NumActions > actionID ? this._distribution[(int) actionID] : 0f;
        }

        public void Normalize()
        {
            var min = this._distribution.Min();

            //verifies if some element is below 0, in which case all elements are shifted up
            if (min < 0)
                this.Add(-min);

            var sum = this._distribution.Sum();
            if (sum.Equals(1))
                //already normalized
                return;
            if (sum.Equals(0))
                //if all elements are zero, transform into uniform distribution
                this.Add(1d/this.NumActions);
            else
                //otherwise, normalize based on sum
                this.Multiply(1d/sum);
        }

        #endregion

        protected void Add(double scalar)
        {
            for (var i = 0; i < this.NumActions; i++)
                this._distribution[i] += scalar;
        }

        protected void Multiply(double scalar)
        {
            for (var i = 0; i < this.NumActions; i++)
                this._distribution[i] *= scalar;
        }

        public static implicit operator DenseVector(Policy policy)
        {
            return policy._distribution;
        }

        public static implicit operator Policy(DenseVector vector)
        {
            return new Policy(vector.ToArray());
        }

        public override string ToString()
        {
            return this._distribution.Values.ToVectorString();
        }
    }
}