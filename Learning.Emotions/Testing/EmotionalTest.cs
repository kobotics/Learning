// ------------------------------------------
// EmotionalTest.cs, Learning.IMRL.Emotions
// 
// Created by Pedro Sequeira, 2013/02/06
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System.Collections.Generic;
using Learning.IMRL.Emotions.Domain.Agents;
using Learning.IMRL.Testing;
using Learning.Testing.Config.Parameters;
using Learning.Testing.Config.Scenarios;
using Learning.Testing.Simulations;
using PS.Utilities.Math;

namespace Learning.IMRL.Emotions.Testing
{
    public class EmotionalTest : IMRLTest
    {
        public const string CONTROL_STAT_ID = "Control";
        public const string VALENCE_STAT_ID = "Valence";
        public const string GOAL_REL_STAT_ID = "GoalRelevance";
        public const string NOVELTY_STAT_ID = "Novelty";
        protected List<string> emotionLabels;

        public EmotionalTest(IFitnessScenario scenario, ITestParameters testParameters)
            : base(scenario, testParameters)
        {
        }

        public override Simulation CreateAndSetupSimulation()
        {
            var simulation = base.CreateAndSetupSimulation();

            //stores emotion labels list
            if ((this.emotionLabels == null) && (simulation.Agent is IEmotionalAgent))
                this.emotionLabels =
                    new List<string>(((IEmotionalAgent) simulation.Agent).EmotionsManager.EmotionLabels.Keys);
            return simulation;
        }

        public override void PrintAgent()
        {
            base.PrintAgent();

            if (!this.LogStatistics) return;

            var emotionalQuantityList = new Dictionary<string, StatisticalQuantity>
                                        {
                                            {CONTROL_STAT_ID, this.testStatisticsAvg[CONTROL_STAT_ID]},
                                            {VALENCE_STAT_ID, this.testStatisticsAvg[VALENCE_STAT_ID]},
                                            {GOAL_REL_STAT_ID, this.testStatisticsAvg[GOAL_REL_STAT_ID]},
                                            {NOVELTY_STAT_ID, this.testStatisticsAvg[NOVELTY_STAT_ID]}
                                        };

            StatisticsUtil.PrintAllQuantitiesToCSV(
                string.Format("{0}/Emotions/DimensionsAvg.csv", this.FilePath), emotionalQuantityList);

            //emotionalQuantityList.Clear();
            //foreach (var emotionLabel in this.emotionLabels)
            //    emotionalQuantityList.Add(emotionLabel, this.testStatisticsAvg[emotionLabel));

            //StatisticsUtil.PrintAllQuantitiesToCSV(this.FilePath + "/Emotions/LabelsAvg.csv", emotionalQuantityList);
            //StatisticsUtil.PrintAllQuantitiesCountToXLS(this.FilePath + "/Emotions/LabelsCountAvg.csv",
            //                                     emotionalQuantityList);
        }
    }
}