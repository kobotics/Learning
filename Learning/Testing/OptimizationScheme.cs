// ------------------------------------------
// OptimizationScheme.cs, Learning
// 
// Created by Pedro Sequeira, 2015/04/01
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Learning.Testing.Config;
using Learning.Testing.Config.Scenarios;
using Learning.Testing.MultipleTests;

namespace Learning.Testing
{
    public struct TopTestScheme
    {
        public TopTestScheme(uint numTests, uint numSimulations) : this()
        {
            this.NumTests = numTests;
            this.NumSimulations = numSimulations;
        }

        public uint NumTests { get; private set; }
        public uint NumSimulations { get; private set; }
    }

    public class OptimizationScheme : IDisposable
    {
        private readonly ParallelOptimizationTest _finalTest;
        private readonly ParallelOptimizationTest _mainTest;
        private readonly List<TopTestScheme> _topTestsScheme;

        public OptimizationScheme(ParallelOptimizationTest mainTest,
            IEnumerable<TopTestScheme> topTestsScheme = null, ParallelOptimizationTest finalTest = null)
        {
            if (mainTest == null) throw new ArgumentNullException("mainTest");

            this._mainTest = mainTest;
            this._finalTest = finalTest;
            if (topTestsScheme != null) this._topTestsScheme = topTestsScheme.ToList();
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (this._topTestsScheme != null) this._topTestsScheme.Clear();
        }

        #endregion

        public IEnumerable<ParallelOptimizationTest> CreateFor(uint testType, ITestsConfig testsConfig)
        {
            var tests = new List<ParallelOptimizationTest>();
            var scenario = testsConfig.ScenarioProfiles[testType];
            if (scenario == null) return tests;

            //changes parameters of main optim test according to scenario
            this.AddMainTest(testsConfig, scenario, tests);

            //creates a top test according to scheme and scenario
            if (this._topTestsScheme == null) return tests;
            this.AddTopTests(testsConfig, scenario, tests);

            //changes parameters of final optim test according to scenario
            if (this._finalTest == null) return tests;
            this.AddFinalTest(testsConfig, scenario, tests);

            return tests;
        }

        protected virtual void AddFinalTest(
            ITestsConfig testsConfig, IScenario scenario, List<ParallelOptimizationTest> tests)
        {
            if (this._finalTest == null) return;
            var testFactory = testsConfig.CreateTestFactory(
                scenario, testsConfig.NumSimulations, testsConfig.NumTimeSteps);
            this._finalTest.OptimizationTestFactory = testFactory;
            tests.Add(this._finalTest);
        }

        protected virtual void AddMainTest(
            ITestsConfig testsConfig, IScenario scenario, List<ParallelOptimizationTest> tests)
        {
            var testFactory = testsConfig.CreateTestFactory(scenario, testsConfig.NumSimulations, testsConfig.NumSamples);
            this._mainTest.OptimizationTestFactory = testFactory;
            tests.Add(this._mainTest);
        }

        protected virtual void AddTopTests(
            ITestsConfig testsConfig, IScenario scenario, List<ParallelOptimizationTest> tests)
        {
            if (this._topTestsScheme == null) return;
            for (var i = 0; i < this._topTestsScheme.Count; i++)
            {
                var topTestScheme = this._topTestsScheme[i];

                //in last test always make 1 sample-per-step
                var testFactory = testsConfig.CreateTestFactory(
                    scenario, topTestScheme.NumSimulations,
                    i == this._topTestsScheme.Count - 1 ? testsConfig.NumTimeSteps : testsConfig.NumSamples);

                tests.Add(new SelectTopFitnessTest(testFactory, topTestScheme.NumTests, true));
            }
        }
    }
}