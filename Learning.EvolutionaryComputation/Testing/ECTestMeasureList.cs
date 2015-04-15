// ------------------------------------------
// ECTestMeasureList.cs, Learning.EvolutionaryComputation
// 
// Created by Pedro Sequeira, 2013/02/06
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using Learning.Testing;
using Learning.Testing.Config.Scenarios;

namespace Learning.EvolutionaryComputation.Testing
{
    public class ECTestMeasureList : TestMeasureList
    {
        public ECTestMeasureList(IScenario scenario, ECTestMeasure baseMeasure) : base(scenario, baseMeasure)
        {
        }

        public void Add(IECChromosome chromosome, ECTestMeasure testMeasure)
        {
            //locks from outside access
            lock (this.locker)
            {
                //changes some parameters of the stored chromosome history
                if (!this.Contains(chromosome))
                    base.Add(chromosome, testMeasure);
                else
                    this.UpdateTestMeasure(chromosome, testMeasure);
            }
        }

        public void UpdateTestMeasure(IECChromosome chromosome)
        {
            this.UpdateTestMeasure(chromosome, null);
        }

        public virtual void UpdateTestMeasure(IECChromosome chromosome, ECTestMeasure testMeasure)
        {
            //checks and gets chromosome history
            if (!this.Contains(chromosome)) return;
            var prevTestMeasure = (ECTestMeasure) this.testMeasures[chromosome];

            //updates generation number (sets to the youngest between the two)
            if (chromosome.Population != null)
                prevTestMeasure.GenerationNumber =
                    Math.Min(prevTestMeasure.GenerationNumber, chromosome.Population.GenerationNumber);

            //averages chromosome fitness with new quantity info
            if (testMeasure != null)
                prevTestMeasure.Value = 0.5f*(prevTestMeasure.Value + testMeasure.Value);

            //increases generation counter
            prevTestMeasure.TimesGenerated++;
        }
    }
}