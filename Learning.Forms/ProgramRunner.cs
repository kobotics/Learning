// ------------------------------------------
// ProgramRunner.cs, Learning.Forms
// 
// Created by Pedro Sequeira, 2015/04/01
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Learning.Forms.Testing;
using Learning.Testing;
using Learning.Testing.Config;
using Learning.Testing.Config.Parameters;
using Learning.Testing.Runners;
using PS.Utilities;
using PS.Utilities.Forms;
using PS.Utilities.IO;
using PS.Utilities.Serialization;

namespace Learning.Forms
{
    public class ProgramRunner
    {
        private const string SINGLE_TEST_OPTION = "&Single Test";
        private const string OPTIM_TEST_OPTION = "&Optimization Test";
        private const string MANUAL_SELECT_OPTION = "&Open Test List...";
        private const string RANK_TEST_OPTION = "&Ranking Test";
        private const string CREATE_CONDOR_OPTION = "Create &Condor Script";
        private const string CHOOSE_FORM_TEXT = "Choose Program";
        private const string RANKING_TEST_PATH = "./RankingTest/";
        private readonly OptimizationScheme _optimizationScheme;
        private readonly SingleTestRunner _singleTestRunner;
        private readonly ITestsConfig _testsConfig;

        public ProgramRunner(
            ITestsConfig testsConfig, SingleTestRunner singleTestRunner, OptimizationScheme optimizationScheme)
        {
            this._testsConfig = testsConfig;
            this._singleTestRunner = singleTestRunner;
            this._optimizationScheme = optimizationScheme;
        }

        protected virtual List<string> GetOptions(ITestParameters singleTestParams)
        {
            return new List<string>
                   {
                       GetSingleTestOption(singleTestParams),
                       OPTIM_TEST_OPTION,
                       MANUAL_SELECT_OPTION,
                       RANK_TEST_OPTION,
                       CREATE_CONDOR_OPTION
                   };
        }

        private static string GetSingleTestOption(ITestParameters singleTestParams)
        {
            return string.Format("{0} ({1})", SINGLE_TEST_OPTION, singleTestParams);
        }

        public virtual void Run(string[] args = null)
        {
            if ((args != null) && (args.Length > 0))
            {
                //assume if args given is single test
                this.RunSingleTest(args);
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var testsConfig = this._testsConfig.CloneJson();
                var testParams = testsConfig.SingleTestParameters;
                var option = ChooseOptionForm.ShowOptions(this.GetOptions(testParams), CHOOSE_FORM_TEXT);
                if (option.Equals(GetSingleTestOption(testParams)))
                    this.RunSingleTest(args);
                else if (option.Equals(OPTIM_TEST_OPTION))
                    this.RunOptimTest(args);
                else if (option.Equals(MANUAL_SELECT_OPTION))
                    this.OpenTestList();
                else if (option.Equals(RANK_TEST_OPTION))
                    this.RunRankTest();
                else if (option.Equals(CREATE_CONDOR_OPTION))
                    this.CreateCondorScript();
            }
        }

        private void RunSingleTest(string[] args)
        {
            this._singleTestRunner.ConfigureTest(args);
            this._singleTestRunner.RunTest();
        }

        private void RunOptimTest(string[] args)
        {
            ProcessUtil.SetMaximumProcessAffinity();

            var optimTestRunner =
                new FormsParallelOptimTestRunnner(this._testsConfig.CloneJson(), this._optimizationScheme);
            optimTestRunner.ConfigureTest(args);
            optimTestRunner.RunTest();
        }

        private void OpenTestList()
        {
            var testConfigList = Util.SelectReadTestsConfig();
            if (testConfigList.Count == 0)
            {
                MessageBox.Show("Invalid or empty file provided.", "Problem opening tests configuration file.",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show(
                string.Format("{0} test configurations found. Run all tests now?", testConfigList.Count),
                "Test list execution", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) != DialogResult.OK)
                return;

            //runs each single test in sequence, on the console
            foreach (var testsConfig in testConfigList)
            {
                var singleTestRunner = this._singleTestRunner.CloneJson();
                singleTestRunner.ForceConsole = true;
                singleTestRunner.TestsConfig = testsConfig;
                singleTestRunner.ConfigureTest();
                singleTestRunner.RunTest();
            }
        }

        private void CreateCondorScript()
        {
            //setup tests config
            var testsConfig = this._testsConfig.CloneJson();
            testsConfig.SetDefaultConstants();
            testsConfig.Init();

            //creates and writes condor script based on generated parameters and test types
            var condorScript = new CondorScriptBuilder(testsConfig);
            condorScript.CreateCondorFile();
        }

        private void RunRankTest()
        {
            var baseFileDir = Path.GetFullPath(RANKING_TEST_PATH);
            PathUtil.CreateOrClearDirectory(baseFileDir);

            var testsConfig = this._testsConfig.CloneJson();
            testsConfig.SetDefaultConstants();
            testsConfig.Init();

            var scenario = testsConfig.ScenarioProfiles[testsConfig.SingleTestType];
            if (scenario == null) return;

            //creates an optimization test factory to generate the test parameters
            var multipleTestFactory = testsConfig.CreateTestFactory(
                scenario, testsConfig.NumSimulations, testsConfig.NumSamples);

            var testRanker = new TestParameterRanker(testsConfig, multipleTestFactory);
            testRanker.RankTests();
            testRanker.TestMeasures.PrintToFile(
                string.Format("{0}{1}Rank.csv", baseFileDir, testsConfig.TestMeasuresName));
        }
    }
}