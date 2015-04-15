// ------------------------------------------
// GPOptimizationScheme.cs, Learning.IMRL.EC
//
// Created by Pedro Sequeira, 2015/4/2
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System.Collections.Generic;
using Learning.Testing;
using Learning.Testing.Config;
using Learning.Testing.Config.Scenarios;
using Learning.Testing.MultipleTests;

namespace Learning.IMRL.EC.Testing
{
    public class GPOptimizationScheme : OptimizationScheme
    {
        private readonly uint _numSelectBestSimulations;

        public GPOptimizationScheme(ParallelOptimizationTest mainTest, uint numSelectBestSimulations = 0,
            IEnumerable<TopTestScheme> topTestsScheme = null, ParallelOptimizationTest finalTest = null)
            : base(mainTest, topTestsScheme, finalTest)
        {
            this._numSelectBestSimulations = numSelectBestSimulations;
        }

        protected override void AddMainTest(ITestsConfig testsConfig, IScenario scenario,
            List<ParallelOptimizationTest> tests)
        {
            base.AddMainTest(testsConfig, scenario, tests);
            if (!(testsConfig is IGPTestsConfig)) return;

            //adds a select best test
            var multipleTestFactory = 
                testsConfig.CreateTestFactory(scenario, this._numSelectBestSimulations, testsConfig.NumSamples);
            tests.Add(new SelectBestFitnessTest(multipleTestFactory, ((IGPTestsConfig) testsConfig).StdDevTimes));
        }
    }
}