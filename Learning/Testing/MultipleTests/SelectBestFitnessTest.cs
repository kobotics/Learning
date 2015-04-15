// ------------------------------------------
// SelectBestFitnessTest.cs, Learning
// 
// Created by Pedro Sequeira, 2013/12/09
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

namespace Learning.Testing.MultipleTests
{
    public class SelectBestFitnessTest : ListFitnessTest
    {
        private const string BEST_TEST_ID = "Best";
        private const string STDDEV = "StdDev";

        #region Constructors

        public SelectBestFitnessTest(
            IOptimizationTestFactory optimizationTestFactory, int stdDevTimes) : base(optimizationTestFactory)
        {
            this.StdDevTimes = stdDevTimes;
            this.RunAllTestsAgain = true;
        }

        #endregion

        #region Protected Methods

        protected override void DetermineParametersList(bool readFromFile = false)
        {
            //clears vars
            if (this.testParameters != null) this.testParameters.Clear();
            this.testParameters = this.TestMeasures.SelectBestMeasures(this.StdDevTimes);
        }

        #endregion

        #region Properties

        public override string TestID
        {
            get { return string.Format("{0}{1}{2}", BEST_TEST_ID, this.StdDevTimes, STDDEV); }
        }

        public int StdDevTimes { get; set; }

        #endregion
    }
}