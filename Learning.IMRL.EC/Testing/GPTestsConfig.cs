// ------------------------------------------
// GPTestsConfig.cs, Learning.IMRL.EC
// 
// Created by Pedro Sequeira, 2013/05/22
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using System.Collections.Generic;
using AForge.Genetic;
using Learning.EvolutionaryComputation.Testing;
using Learning.IMRL.EC.Chromosomes;
using Learning.IMRL.EC.Genes;
using Learning.IMRL.EC.Testing.MultipleTests;
using Learning.Testing.Config.Parameters;
using Learning.Testing.Config.Scenarios;

namespace Learning.IMRL.EC.Testing
{
    [Serializable]
    public abstract class GPTestsConfig : ECTestsConfig, IGPTestsConfig
    {
        private const string FIT_CHROMOSOME_EXPRESSION = "$0 ";
        private const string RAND_CHROMOSOME_EXPRESSION = "0 ";

        public virtual int VariablesCount
        {
            get { return (int) (this.Constants.Length + this.NumBaseVariables); }
        }

        #region IGPTestsConfig Members

        public int MaxInitialLevel { get; set; }
        public uint NumBaseVariables { get; set; }
        public double[] Constants { get; set; }
        public int MaxProgTreeDepth { get; set; }
        public List<FunctionType> AllowedFunctions { get; set; }

        public abstract IGPSimplifierOptimizationTestFactory CreateSimplifierTestFactory(
            IScenario scenario, uint numSimulations, uint numSamples);

        public override List<ITestParameters> GetSpecialTestParameters(IScenario scenario)
        {
            //adds random and fitness only test parameters 
            return new List<ITestParameters>
                   {
                       new GPExpressionChromosome(FIT_CHROMOSOME_EXPRESSION),
                       new GPExpressionChromosome(RAND_CHROMOSOME_EXPRESSION)
                   };
        }

        public override void SetDefaultConstants()
        {
            base.SetDefaultConstants();

            this.MaxProgTreeDepth = 3; //4;
            this.MaxInitialLevel = 1; //4;

            //sets maximum level of tree depth for genetic programs
            GPTreeChromosome.MaxLevel = this.MaxProgTreeDepth;
            GPTreeChromosome.MaxInitialLevel = this.MaxInitialLevel;
        }

        public override List<ITestParameters> GetOptimizationTestParameters()
        {
            return new List<ITestParameters>();
        }

        #endregion
    }
}