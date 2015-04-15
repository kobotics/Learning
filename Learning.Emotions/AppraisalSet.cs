// ------------------------------------------
// AppraisalSet.cs, Learning.Emotions
//
// Created by Pedro Sequeira, 2012/10/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using System.Collections.Generic;

namespace Learning.IMRL.Emotions
{
    public class AppraisalSet : IDisposable
    {
        public AppraisalSet()
        {
            this.Novelty = new AppraisalDimension("Novelty");
            this.Arousal = new AppraisalDimension("Arousal");
            this.Valence = new AppraisalDimension("Valence");
            this.GoalRelevance = new AppraisalDimension("GoalRelevance");
            this.Control = new AppraisalDimension("Control");

            this.Dimensions = new Dictionary<string, AppraisalDimension>
                                  {
                                      {this.Novelty.Name, this.Novelty},
                                      {this.Arousal.Name, this.Arousal},
                                      {this.Valence.Name, this.Valence},
                                      {this.GoalRelevance.Name, this.GoalRelevance},
                                      {this.Control.Name, this.Control}
                                  };
        }

        public AppraisalDimension Novelty { get; protected set; }

        public AppraisalDimension Arousal { get; protected set; }

        public AppraisalDimension Valence { get; protected set; }

        public AppraisalDimension GoalRelevance { get; protected set; }

        public AppraisalDimension Control { get; protected set; }

        public Dictionary<string, AppraisalDimension> Dimensions { get; protected set; }

        #region IDisposable Members

        public virtual void Dispose()
        {
            foreach (var appraisalDimension in this.Dimensions.Values) appraisalDimension.Dispose();
        }

        #endregion

        public double DifferenceTo(AppraisalSet appraisalSet)
        {
            var sum = 0d;
            foreach (var appraisalDimensionId in this.Dimensions.Keys)
            {
                var diff = appraisalSet.Dimensions[appraisalDimensionId].Value -
                           this.Dimensions[appraisalDimensionId].Value;
                sum += diff*diff;
            }
            return System.Math.Sqrt(sum);
        }
    }
}