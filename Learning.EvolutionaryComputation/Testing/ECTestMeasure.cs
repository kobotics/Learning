// ------------------------------------------
// ECTestMeasure.cs, Learning.EvolutionaryComputation
// 
// Created by Pedro Sequeira, 2013/02/06
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System.Collections.Generic;
using Learning.Testing;
using PS.Utilities.Collections;

namespace Learning.EvolutionaryComputation.Testing
{
    public class ECTestMeasure : TestMeasure
    {
        public int GenerationNumber { get; set; }
        public int TimesGenerated { get; set; }

        public override string[] Header
        {
            get { return base.Header.Append(new List<string> {"Generation", "Times Generated"}); }
        }

        public override string ToString()
        {
            return this.Parameters.ToString();
        }

        public override string[] ToValue()
        {
            return base.ToValue().Append(
                new List<string> {this.GenerationNumber.ToString(), this.TimesGenerated.ToString()});
        }

        public override bool FromValue(string[] value)
        {
            if ((value == null) || (value.Length < 6)) return false;
            var length = value.Length;
            this.TimesGenerated = int.Parse(value[length - 1]);
            this.GenerationNumber = int.Parse(value[length - 2]);
            return base.FromValue(value.SubArray(0, length - 2));
        }
    }
}