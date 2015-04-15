// ------------------------------------------
// GPExpressionChromosome.cs, Learning.IMRL.EC
// 
// Created by Pedro Sequeira, 2013/03/26
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using AForge.Genetic;

namespace Learning.IMRL.EC.Chromosomes
{
    [Serializable]
    public class GPExpressionChromosome : GPChromosome
    {
        private string _expression;

        public GPExpressionChromosome(string expression)
            : base(null, null)
        {
            this._expression = expression;
        }

        public override string ToString()
        {
            return this._expression;
        }

        public override bool FromValue(string[] value)
        {
            if ((value == null) || (value.Length == 0)) return false;
            this._expression = value[0];
            return true;
        }

        public override IChromosome Clone()
        {
            return new GPExpressionChromosome(this._expression);
        }
    }
}