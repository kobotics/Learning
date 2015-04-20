// ------------------------------------------
// Program.cs, Learning.Tests.EmotionalOptimization
// 
// Created by Pedro Sequeira, 2015/03/25
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using System.Collections.Generic;
using Learning.Forms;
using Learning.Testing;
using Learning.Testing.MultipleTests;
using Learning.Tests.EmotionalOptimization.Testing;

namespace Learning.Tests.EmotionalOptimization
{
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            var testsConfig = new EmotionalTestsConfig();
            testsConfig.SetDefaultConstants();
            testsConfig.Init();

            Console.WriteLine(Global.Global.wPenaltyType);
            
            var programRunner = new ProgramRunner(testsConfig,
                new EmotionalSingleTestRunner(testsConfig), new OptimizationScheme(new ListFitnessTest(null),
                    new List<TopTestScheme>
                    {
                        new TopTestScheme(208, 8),
                        new TopTestScheme(48, 48),
                        new TopTestScheme(10, 104),
                        new TopTestScheme(5, 208),
                        new TopTestScheme(1, 208)
                        //new TopTestScheme(1, 8)
                    }));

            programRunner.Run(args);
        }
    }
}