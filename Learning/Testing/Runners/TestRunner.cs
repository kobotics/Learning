// ------------------------------------------
// TestRunner.cs, Learning
// 
// Created by Pedro Sequeira, 2015/04/01
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using Learning.Testing.Config;
using Learning.Testing.Config.Scenarios;
using Learning.Testing.MultipleTests;
using PS.Utilities;
using PS.Utilities.Math;
using PS.Utilities.Parsing;

namespace Learning.Testing.Runners
{
    public abstract class TestRunner
    {
        protected TestRunner(ITestsConfig testsConfig)
        {
            this.TestsConfig = testsConfig;
            this.ForceConsole = false;
        }

        public ITestsConfig TestsConfig { get; set; }

        public bool ForceConsole { get; set; }

        protected IScenario DefaultScenario
        {
            get { return this.TestsConfig.ScenarioProfiles[this.TestsConfig.SingleTestType]; }
        }

        protected IOptimizationTestFactory DefaultTestFactory { get; private set; }

        public bool ConfigureTest(string[] args =null)
        {
            //tries to get tests config from cmd-line, otherwise uses default config
            if ((args != null) && (args.Length > 0))
                ArgumentsParser.Parse(args, this.TestsConfig);

            //checks enable graphics
            ExcelUtil.EnableGraphics =
                this.TestsConfig.GraphicsEnabled |= (args != null) && (args.Length == 0);

            //sets affinity
            ProcessUtil.SetProcessAffinity(this.TestsConfig.MaxCPUsUsed);

            //inits test config
            this.TestsConfig.Init();
            this.DefaultTestFactory = this.TestsConfig.CreateTestFactory(
                this.DefaultScenario, this.TestsConfig.NumSimulations, this.TestsConfig.NumSamples);

            return true;
        }

        public abstract void RunTest();
    }
}