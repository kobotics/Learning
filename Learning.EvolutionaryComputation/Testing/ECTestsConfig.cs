// ------------------------------------------
// ECTestsConfig.cs, Learning.EvolutionaryComputation
// 
// Created by Pedro Sequeira, 2013/05/22
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using AForge.Genetic;
using CommandLine;
using Learning.Testing.Config;
using Newtonsoft.Json;

namespace Learning.EvolutionaryComputation.Testing
{
    [Serializable]
    public abstract class ECTestsConfig : TestsConfig, IECTestsConfig
    {
        #region IECTestsConfig Members

        public ISelectionMethod SelectionMethod { get; set; }
        public uint FitnessImprovementThreshold { get; set; }
        public double RandomSelectionPortion { get; set; }
        public double SteadyStatePortion { get; set; }
        public double SymmetryFactor { get; set; }
        public int StdDevTimes { get; set; }
        public abstract IECChromosome CreateBaseChromosome();

        #endregion

        #region Json and parsing

        public const string NUM_TESTS_ITER_ARG = "testsPerIteration";
        private const string NUM_TESTS_ITER_MSG = "The number of tests per iteration (pop. size).";

        private const string TEST_MEASURES_PREFIX_MSG = "Prefix to use on test measures file.";
        public const string TEST_MEASURES_PREFIX_ARG = "measuresPrefix";

        public const string MAX_ITERATIONS_ARG = "iterations";

        private const string MAX_ITERATIONS_MSG =
            "The maximum number of iterations to run for each optimization test (max. generations).";

        public const string NUM_PARALLEL_TESTS_ARG = "parallelTests";
        private const string NUM_PARALLEL_TESTS_MSG = "The number of optimization tests to run (num. populations).";

        [JsonProperty(NUM_TESTS_ITER_ARG)]
        [Option(NUM_TESTS_ITER_ARG, Required = true, HelpText = NUM_TESTS_ITER_MSG)]
        public int NumTestsPerIteration { get; set; }

        [JsonProperty(MAX_ITERATIONS_ARG)]
        [Option(MAX_ITERATIONS_ARG, Required = true, HelpText = MAX_ITERATIONS_MSG)]
        public uint MaxIterations { get; set; }

        [JsonProperty(NUM_PARALLEL_TESTS_ARG)]
        [Option(NUM_PARALLEL_TESTS_ARG, Required = true, HelpText = NUM_PARALLEL_TESTS_MSG)]
        public uint NumParallelOptimTests { get; set; }

        [JsonProperty(TEST_MEASURES_PREFIX_ARG)]
        [Option(TEST_MEASURES_PREFIX_ARG, Required = false, HelpText = TEST_MEASURES_PREFIX_MSG)]
        public string TestMeasuresPrefix { get; set; }

        #endregion
    }
}