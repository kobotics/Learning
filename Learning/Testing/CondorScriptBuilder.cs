// ------------------------------------------
// OptimizationCondorScriptBuilder.cs, Learning
// 
// Created by Pedro Sequeira, 2014/03/10
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Learning.Testing.Config;
using Learning.Testing.Config.Parameters;
using PS.Utilities.Collections;
using PS.Utilities.Parsing;
using PS.Utilities.Serialization;

namespace Learning.Testing
{
    public class CondorScriptBuilder
    {
        protected const string PROC_ID_STR = "proc";
        protected const int DEF_MAX_JOBS_PER_FILE = 25000;
        protected const string ARGS_LINE_STR = "Arguments = ";
        protected const string QUEUE_LINE_STR = "Queue";
        protected const string UNIVERSE_LINE_STR = "Universe = vanilla";
        protected const string EXECUTABLE_LINE_STR = "Executable = ";
        protected const string FILE_TRANSFER_LINE_STR = "should_transfer_files = NO";
        protected const string PERIODIC_RELEASE_LINE_STR = "periodic_release = TRUE";

        public CondorScriptBuilder(ITestsConfig testsConfig)
        {
            this.MaxJobsPerFile = DEF_MAX_JOBS_PER_FILE;
            this.TestsConfig = testsConfig;
        }

        protected ITestsConfig TestsConfig { get; private set; }
        protected int MaxJobsPerFile { get; set; }

        public void CreateCondorFile()
        {
            var multipleTestTypes = this.TestsConfig.MultipleTestTypes;
            if ((multipleTestTypes == null) || (multipleTestTypes.Length == 0)) return;

            //gets all test parameters for the type of test
            var testArgumentsList = new List<string[]>();
            foreach (var testType in multipleTestTypes)
                testArgumentsList.AddRange(this.GetTestArguments(testType));

            //create as many condor submit files as necessary
            var scriptFilePath = Path.GetFullPath(string.Format("{0}", this.TestsConfig.CondorScriptPath));
            var executableFile = Assembly.GetEntryAssembly().GetName().Name.Split('.').Last();
            var fileNum = 0;
            for (var i = 0; i < testArgumentsList.Count; i += this.MaxJobsPerFile, fileNum++)
            {
                var numTests = ((i + this.MaxJobsPerFile) > testArgumentsList.Count
                    ? testArgumentsList.Count - i
                    : this.MaxJobsPerFile);

                this.CreateCondorFile(scriptFilePath + fileNum, executableFile, testArgumentsList.GetRange(i, numTests));
            }
        }

        protected List<string[]> GetTestArguments(uint testType)
        {
            var testArgumentsList = new List<string[]>();

            //gets all test parameters for the type of test
            var testParamsSet = new HashSet<ITestParameters>(this.GetTestParameters(testType));

            //removes possible unnecessary test parameters
            var testsConfig = this.TestsConfig.CloneJson();
            testsConfig.Init();

            var testProfile = testsConfig.ScenarioProfiles[testType];
            if (File.Exists(testProfile.TestMeasuresFilePath))
            {
                var testFactory = testsConfig.CreateTestFactory(testProfile);
                var testMeasures = testFactory.CreateTestMeasureList();
                testMeasures.ReadFromFile(testProfile.TestMeasuresFilePath);
                testParamsSet.RemoveWhere(testMeasures.Contains);
            }

            //changes config parameters
            testsConfig.GraphicsEnabled = false;
            testsConfig.SingleTestType = testType;

            //adds test arguments for each specific test params
            foreach (var testParameters in testParamsSet)
            {
                testsConfig.SingleTestParameters = testParameters;
                var args = ArgumentsParser.GetArgs(testsConfig);
                if ((args != null) && (args.Length > 0))
                    testArgumentsList.Add(args);
            }

            return testArgumentsList;
        }

        private List<ITestParameters> GetTestParameters(uint testType)
        {
            //gets sampled parameter list for given test type
            var testProfile = this.TestsConfig.ScenarioProfiles[testType];
            var testParameters = this.TestsConfig.GetOptimizationTestParameters();

            //also adds special tests
            testParameters.AddRange(this.TestsConfig.GetSpecialTestParameters(testProfile));

            return testParameters;
        }

        private void CreateCondorFile(
            string scriptFilePath, string executableFile, List<string[]> testArgumentsList)
        {
            if (File.Exists(scriptFilePath)) File.Delete(scriptFilePath);
            var sw = new StreamWriter(scriptFilePath);

            //writes condor script header
            this.WriteHeader(executableFile, sw);

            //writes arguments for programs execution for all test parameters
            this.WriteExecutableArguments(sw, testArgumentsList);

            sw.Close();
            sw.Dispose();

            Console.WriteLine("Created condor submission file '{0}' with {1} test parameters.",
                scriptFilePath, testArgumentsList.Count);
        }

        protected void WriteHeader(string executableFile, StreamWriter sw)
        {
            //writes condor script header
            sw.WriteLine(UNIVERSE_LINE_STR);
            sw.WriteLine("{0}{1}", EXECUTABLE_LINE_STR, executableFile);
            //sw.WriteLine("Log = output/{0}$(Process).log", PROC_ID_STR);
            //sw.WriteLine("Output = output/{0}$(Process).out", PROC_ID_STR);
            //sw.WriteLine("Error = output/{0}$(Process).error", PROC_ID_STR);
            sw.WriteLine(FILE_TRANSFER_LINE_STR);
            sw.WriteLine(PERIODIC_RELEASE_LINE_STR);
        }

        protected void WriteExecutableArguments(StreamWriter sw, List<string[]> testArgumentsList)
        {
            //writes arguments and queue lines for each desired test
            foreach (var testArguments in testArgumentsList)
            {
                var argLine = string.Format("{0}{1}", ARGS_LINE_STR, testArguments.ToString(' '));
                sw.WriteLine(argLine);
                sw.WriteLine(QUEUE_LINE_STR);
            }
        }
    }
}