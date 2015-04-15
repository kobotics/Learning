// ------------------------------------------
// GPSimplifierMultipleTestFactory.cs, Learning.IMRL.EC
// 
// Created by Pedro Sequeira, 2013/03/27
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System.Collections.Generic;
using Learning.EvolutionaryComputation.Testing;
using Learning.IMRL.EC.Chromosomes;
using Learning.Testing.Config.Parameters;
using Learning.Testing.Config.Scenarios;

namespace Learning.IMRL.EC.Testing.MultipleTests
{
    public class GPSimplifierOptimizationTestFactory :
        GPOptimizationTestFactory, IGPSimplifierOptimizationTestFactory
    {
        public GPSimplifierOptimizationTestFactory(IFitnessScenario scenario)
            : base(scenario)
        {
            this.SimplifiedParamMeasures = new Dictionary<IGPChromosome, ECTestMeasure>();
            this.SimplifiedChromosomesMeasures = new Dictionary<ECTestMeasure, List<ECTestMeasure>>();
            this.MinLengthChromosome = new Dictionary<ECTestMeasure, uint>();
        }

        #region IGPSimplifierOptimizationTestFactory Members

        public Dictionary<ECTestMeasure, uint> MinLengthChromosome { get; private set; }
        public Dictionary<ECTestMeasure, List<ECTestMeasure>> SimplifiedChromosomesMeasures { get; private set; }
        public Dictionary<IGPChromosome, ECTestMeasure> SimplifiedParamMeasures { get; private set; }

        public virtual List<ITestParameters> DetermineChromosomeParameters(ECTestMeasureList testMeasureList)
        {
            var testParameters = new List<ITestParameters>();

            this.SimplifiedParamMeasures.Clear();
            this.MinLengthChromosome.Clear();

            //adds all possible sub-combinations
            var subCombinations = new HashSet<IGPChromosome>();
            foreach (IGPChromosome parameters in testMeasureList)
            {
                var testMeasure = (ECTestMeasure) testMeasureList.GetTestMeasure(parameters);
                this.MinLengthChromosome[testMeasure] = parameters.Length;
                this.SimplifiedChromosomesMeasures[testMeasure] = new List<ECTestMeasure>();

                var allParamCombinations = parameters.AllCombinations;
                subCombinations.UnionWith(allParamCombinations);

                foreach (var paramCombination in allParamCombinations)
                    this.SimplifiedParamMeasures[paramCombination] = testMeasure;
            }
            testParameters.AddRange(subCombinations);

            //sorts chromosome parameters by length
            testParameters.Sort();

            return testParameters;
        }

        #endregion
    }
}