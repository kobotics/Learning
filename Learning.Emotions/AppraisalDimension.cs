// ------------------------------------------
// AppraisalDimension.cs, Learning.Emotions
//
// Created by Pedro Sequeira, 2012/10/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using PS.Utilities.Math;

namespace Learning.IMRL.Emotions
{
    public class AppraisalDimension : StatisticalQuantity
    {
        public AppraisalDimension(string name) : base(0, new Range(0, 1))
        {
            this.Name = name;
        }

        public string Name { get; protected set; }
    }
}