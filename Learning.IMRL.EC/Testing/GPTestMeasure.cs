// ------------------------------------------
// GPTestMeasure.cs, Learning.IMRL.EC
// 
// Created by Pedro Sequeira, 2013/02/06
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System.Collections.Generic;
using Learning.EvolutionaryComputation.Testing;
using Learning.IMRL.EC.Chromosomes;
using PS.Utilities.Collections;

namespace Learning.IMRL.EC.Testing
{
    public class GPTestMeasure : ECTestMeasure
    {
        public string TranslatedExpression { get; set; }

        public override string[] Header
        {
            get { return base.Header.Append(new List<string> {"Translated Expression", "Length", "Depth"}); }
        }

        public override string[] ToValue()
        {
            var value = base.ToValue();
            if (!(this.Parameters is GPChromosome)) return value;
            var gpChromosome = (GPChromosome) this.Parameters;
            return value.Append(new List<string>
                                {
                                    this.TranslatedExpression,
                                    gpChromosome.Length.ToString(),
                                    gpChromosome.Depth.ToString()
                                });
        }

        public override bool FromValue(string[] value)
        {
            if (!(this.Parameters is GPChromosome)) return base.FromValue(value);
            if ((value == null) || (value.Length < 9)) return false;
            var length = value.Length;
            this.TranslatedExpression = value[length - 3];
            return base.FromValue(value.SubArray(0, length - 3));
        }
    }
}