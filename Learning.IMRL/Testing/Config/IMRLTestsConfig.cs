// ------------------------------------------
// IMRLTestsConfig.cs, Learning.IMRL
// 
// Created by Pedro Sequeira, 2014/02/14
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using CommandLine.Text;
using Learning.Testing.Config;
using Learning.Testing.Config.Parameters;
using Learning.Testing.Config.Scenarios;
using PS.Utilities.Collections;
using PS.Utilities.Math;

namespace Learning.IMRL.Testing.Config
{
    [Serializable]
    public abstract class IMRLTestsConfig : TestsConfig, IIMRLTestsConfig
    {
        protected IMRLTestsConfig()
        {
            this.AddSpecialTests = true;
        }

        #region IIMRLTestsConfig Members

        public string[] ParamIDNames { get; set; }
        public char[] ParamIDLetters { get; set; }
        public StepInterval<double>[] ParamsStepIntervals { get; set; }

        public int NumParams
        {
            get { return this.ParamsStepIntervals.Length; }
        }

        public bool AddSpecialTests { get; set; }

        public virtual bool IsValidParameter(IArrayParameter parameter)
        {
            //only params that have sum of absolute values of 1, eg (-0.1, 0.9) or (0.4, 0.6)
            return parameter.AbsoulteSum.Equals(1);
        }

        public override List<ITestParameters> GetSpecialTestParameters(IScenario scenario)
        {
            var specialParams = new List<ITestParameters>();

            if (!this.AddSpecialTests) return specialParams;

            //adds random test parameter (eg. (0, 0, 0))
            var randomParam = new ArrayParameter(new double[this.NumParams]);
            specialParams.Add(randomParam);

            //adds fitness only test parameter (eg. (0, 0, 1))
            var fitOnlyParam = new ArrayParameter(new double[this.NumParams]);
            fitOnlyParam[(this.NumParams - 1)] = 1d;
            specialParams.Add(fitOnlyParam);

            return specialParams;
        }

        public override List<ITestParameters> GetOptimizationTestParameters()
        {
            //creates tests for different weight parameters according to TestsConfig
            var testParameters = new List<ITestParameters>();
            var elements = NumericArrayUtil<double>.NumericArrayFromInterval(this.ParamsStepIntervals);
            var allParamsComb = elements.AllCombinations();
            foreach (var paramsComb in allParamsComb)
            {
                //creates array parameter
                var arrayParameter = new ArrayParameter(paramsComb);

                //only considers combinations with abs sum of 1, eg (-0.1, 0.9), (0.4, 0.6)
                if (this.IsValidParameter(arrayParameter))
                    testParameters.Add(arrayParameter);
            }

            return testParameters;
        }

        public override string GetTestName(IScenario scenario, ITestParameters testParameters)
        {
            //create test name from parameters, eg (a-0.1_b0.7_c0.2)
            var paramLetterIDs = this.ParamIDLetters;
            var arrayParameter = (ArrayParameter) testParameters;
            var sb = new StringBuilder("(");

            for (var i = 0; i < arrayParameter.Length; i++)
                sb.AppendFormat("{0}{1:0.0}_", paramLetterIDs[i], arrayParameter[i]);

            sb.Remove(sb.Length - 1, 1);
            sb.Append(")");
            return sb.ToString();
        }

        #endregion

        protected override void OnFormatOptionHelpText(object sender, FormatOptionHelpTextEventArgs e)
        {
            if (e.Option.LongName.Equals(SINGLE_TEST_PARAMS_ARG))
                e.Option.HelpText += string.Format(" Usage: --{0}={1}.", SINGLE_TEST_PARAMS_ARG,
                    this.ParamIDNames.ToString(' ', false, "'", "'"));
        }
    }
}