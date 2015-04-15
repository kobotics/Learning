// ------------------------------------------
// FormsParallelOptimTestRunnner.cs, Learning.Forms
// 
// Created by Pedro Sequeira, 2015/04/01
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using Learning.Testing;
using Learning.Testing.Config;
using Learning.Testing.MultipleTests;
using Learning.Testing.Runners;
using PS.Utilities.Forms;

namespace Learning.Forms.Testing
{
    public class FormsParallelOptimTestRunnner : ParallelOptimTestRunnner
    {
        public FormsParallelOptimTestRunnner(ITestsConfig testsConfig, OptimizationScheme optimizationScheme)
            : base(testsConfig, optimizationScheme)
        {
        }

        protected override void RunTest(ParallelOptimizationTest test)
        {
            //starts progress form, also links close button to interrupt test
            using (var progressFormUpdater = new ProgressFormUpdater(test)
                                             {
                                                 Visible = this.TestsConfig.GraphicsEnabled,
                                                 Text = string.Format("{0}", test.TestID)
                                             })
            {
                progressFormUpdater.FormTerminated += test.InterruptProcessing;
                base.RunTest(test);
            }
        }
    }
}