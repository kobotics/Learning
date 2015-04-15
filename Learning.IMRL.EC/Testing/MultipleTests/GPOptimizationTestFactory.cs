// ------------------------------------------
// GPOptimizationTestFactory.cs, Learning.IMRL.EC
// 
// Created by Pedro Sequeira, 2013/03/27
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using Learning.EvolutionaryComputation;
using Learning.EvolutionaryComputation.Testing;
using Learning.IMRL.EC.Domain;
using Learning.Testing;
using Learning.Testing.Config.Scenarios;
using Learning.Testing.MultipleTests;
using Learning.Testing.SingleTests;

namespace Learning.IMRL.EC.Testing.MultipleTests
{
    public class GPOptimizationTestFactory : OptimizationTestFactory
    {
        #region Constructors

        public GPOptimizationTestFactory(IFitnessScenario scenario) : base(scenario)
        {
        }

        #endregion

        public override TestMeasure CreateTestMeasure(FitnessTest test)
        {
            if(test==null) return new GPTestMeasure();

            var chromosome = (IECChromosome) test.TestParameters;

            //creates new chromosome test measure
            return new GPTestMeasure
                   {
                       ID = test.TestName,
                       Parameters = chromosome,
                       Quantity = test.SimulationScoreAvg,
                       Value = test.FinalScores.Avg,
                       StdDev = test.FinalScores.StdDev,
                       TranslatedExpression = this.GetTranslatedExpression(test),
                       TimesGenerated = 1,
                       GenerationNumber =
                           chromosome.Population == null ? -1 : chromosome.Population.GenerationNumber
                   };
        }

        protected virtual string GetTranslatedExpression(SingleTest test)
        {
            var simulation = test.CreateAndSetupSimulation();
            return ((IGPAgent) simulation.Agent).MotivationManager.TranslatedExpression;
        }

        public override TestMeasureList CreateTestMeasureList()
        {
            //use as base parameters the single test params
            var baseMeasure = this.CreateTestMeasure(null);
            baseMeasure.Parameters = this.TestsConfig.SingleTestParameters;
            return new ECTestMeasureList(this.Scenario, (GPTestMeasure) baseMeasure);
        }
    }
}