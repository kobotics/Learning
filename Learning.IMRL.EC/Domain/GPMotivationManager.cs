// ------------------------------------------
// GPMotivationManager.cs, Learning.IMRL.EC
// 
// Created by Pedro Sequeira, 2014/05/23
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using AForge;
using Learning.IMRL.Domain.Managers.Motivation;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Learning.IMRL.EC.Domain
{
    public abstract class GPMotivationManager : IntrinsicMotivationManager
    {
        protected readonly double[] constants;
        protected string translatedExpression;

        protected GPMotivationManager(IGPAgent agent, double[] constants)
            : base(agent)
        {
            this.constants = constants;
        }

        protected abstract string[] VariablesNames { get; }

        protected int NumVariables
        {
            get { return this.VariablesNames.Length; }
        }

        public new IGPAgent Agent
        {
            get { return base.Agent as IGPAgent; }
        }

        public string TranslatedExpression
        {
            get { return this.translatedExpression ?? this.GetTranslatedExpression(); }
        }

        protected long NumInputElements
        {
            get { return this.NumVariables + this.constants.Length; }
        }

        public override double GetIntrinsicReward(uint prevState, uint action, uint nextState)
        {
            //just execute the program with given input
            var rwd = PolishExpression.Evaluate(
                this.Agent.Chromosome.ToString(), this.GetRewardFeatures(prevState, action, nextState).ToArray());

            return (double.IsNaN(rwd) || double.IsInfinity(rwd)) ? 0 : rwd;
        }

        protected void CopyConstants(DenseVector vector)
        {
            //just copy constants into given vector
            Array.Copy(constants, 0, vector.ToArray(), this.VariablesNames.Length, this.constants.Length);
        }

        protected virtual string GetTranslatedExpression()
        {
            return Util.GetTranslatedExpression(this.Agent.Chromosome.ToString(), this.constants, this.VariablesNames);
        }
    }
}