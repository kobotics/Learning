// ------------------------------------------
// SelectTopFitnessTest.cs, Learning
// 
// Created by Pedro Sequeira, 2013/12/09
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using Learning.Testing.SingleTests;

namespace Learning.Testing.MultipleTests
{
    public class SelectTopFitnessTest : ListFitnessTest
    {
        #region Constructors

        public SelectTopFitnessTest(
            IOptimizationTestFactory optimizationTestFactory, uint numberTests, bool topTests)
            : base(optimizationTestFactory)
        {
            this.TopTests = topTests;
            this.NumberTests = numberTests;
            this.RunAllTestsAgain = true;
        }

        #endregion

        #region Public Methods

        public override bool Run()
        {
            return this.NumberTests != 0 && base.Run();
        }

        #endregion

        #region Constants

        protected const string BOTTOM_TESTS_ID = "Bottom";
        protected const string TOP_TESTS_ID = "Top";
        protected const string GENERAL_TESTS_ID = "General";

        #endregion

        #region Fields

        #endregion

        #region Properties

        public override string TestID
        {
            get { return string.Format("{0}{1}", this.TopTests ? TOP_TESTS_ID : BOTTOM_TESTS_ID, this.NumberTests); }
        }

        public bool TopTests { get; set; }

        public uint NumberTests { get; set; }

        #endregion

        #region Protected Methods

        protected override void DetermineParametersList(bool fromFile = false)
        {
            //selects top chromosomes from current history
            if (this.testParameters != null) this.testParameters.Clear();
            this.testParameters = this.TestMeasures.SelectTopParameters(this.NumberTests, this.TopTests);
        }

        protected override void PrepareSingleTest(FitnessTest test)
        {
            base.PrepareSingleTest(test);
            lock (this.locker)
                test.LogStatistics = this.TestsConfig.GraphicsEnabled;
        }

        #endregion
    }
}