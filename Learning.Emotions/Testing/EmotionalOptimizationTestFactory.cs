// ------------------------------------------
// EmotionalOptimizationTestFactory.cs, Learning.IMRL.Emotions
// 
// Created by Pedro Sequeira, 2012/10/15
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Learning.IMRL.Emotions.Domain.Managers;
using Learning.Testing.Config.Parameters;
using Learning.Testing.Config.Scenarios;
using Learning.Testing.MultipleTests;
using Learning.Testing.SingleTests;

namespace Learning.IMRL.Emotions.Testing
{
    public class EmotionalOptimizationTestFactory : OptimizationTestFactory
    {
        public EmotionalOptimizationTestFactory(IFitnessScenario scenario)
            : base(scenario)
        {
        }

        public new EmotionalTestsConfig TestsConfig
        {
            get { return base.TestsConfig as EmotionalTestsConfig; }
        }

        public virtual List<ITestParameters> GenerateEmotionalLabelParameters()
        {
            var manager = new SchererEmotionsManager(null);

            return manager.EmotionLabels.Values.Select(
                label => new ArrayParameter(new[]
                                            {
                                                label.Arousal.Value, label.Valence.Value, label.GoalRelevance.Value,
                                                label.Novelty.Value, label.Control.Value
                                            })).Cast<ITestParameters>().ToList();
        }

        public override FitnessTest CreateTest(ITestParameters parameters)
        {
            var singleTest = new EmotionalTest((IFitnessScenario) this.Scenario.Clone(), (ArrayParameter) parameters);
            singleTest.Reset();
            return singleTest;
        }
    }
}