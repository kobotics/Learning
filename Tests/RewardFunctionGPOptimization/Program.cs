using System;
using System.Collections.Generic;
using Learning.EvolutionaryComputation.Testing.MultipleTests;
using Learning.Forms;
using Learning.Forms.Testing;
using Learning.IMRL.EC.Testing;
using Learning.IMRL.EC.Testing.MultipleTests;
using Learning.Testing;
using Learning.Tests.RewardFunctionGPOptimization.Testing;

namespace Learning.Tests.RewardFunctionGPOptimization
{
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            var testsConfig = new EmotionalGPTestsConfig();
            testsConfig.SetDefaultConstants();
            testsConfig.Init();

            var simplifierTest = new GPSimplifierFitnessTest(null);
            var programRunner = new ProgramRunner(
                testsConfig, new FormsSingleTestRunner(testsConfig),
                new GPOptimizationScheme(new ECParallelOptimTest(null), 208,
                    new List<TopTestScheme>
                    {
                        new TopTestScheme(208, 8),
                        new TopTestScheme(104, 16),
                        new TopTestScheme(48, 48),
                        new TopTestScheme(10, 104),
                        new TopTestScheme(5, 208),
                        new TopTestScheme(1, 208)
                    }, simplifierTest));

            programRunner.Run(args);
        }
    }
}