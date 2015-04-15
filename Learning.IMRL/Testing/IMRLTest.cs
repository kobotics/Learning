// ------------------------------------------
// IMRLTest.cs, Learning.IMRL
// 
// Created by Pedro Sequeira, 2014/01/27
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System.Collections.Generic;
using Learning.Testing.Config.Parameters;
using Learning.Testing.Config.Scenarios;
using Learning.Testing.SingleTests;
using PS.Utilities.Math;

namespace Learning.IMRL.Testing
{
    public class IMRLTest : FitnessTest
    {
        public IMRLTest(IFitnessScenario scenario, ITestParameters testParameters)
            : base(scenario, testParameters)
        {
        }

        public override void PrintAgent()
        {
            base.PrintAgent();

            if (!this.LogStatistics) return;

            this.PrintStatistic("NumBackups", "/Learning/NumBackupsAvg.csv");

            var rewardQuantityList = new Dictionary<string, StatisticalQuantity>
                                     {
                                         //{"Reward", this.GetAgentQuantityAverage("Reward")},
                                         {"ExtrinsicReward", this.testStatisticsAvg["ExtrinsicReward"]},
                                         {"IntrinsicReward", this.testStatisticsAvg["IntrinsicReward"]}
                                     };
            StatisticsUtil.PrintAllQuantitiesToCSV(this.FilePath + "/STM/RewardAvg.csv", rewardQuantityList);
        }
    }
}