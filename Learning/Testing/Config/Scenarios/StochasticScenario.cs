// ------------------------------------------
// StochasticTestProfile.cs, Learning
//
// Created by Pedro Sequeira, 2013/12/18
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using Learning.Domain.Agents;
using Learning.Domain.Environments;

namespace Learning.Testing.Config.Scenarios
{
    public class StochasticScenario : SingleScenario, IStochasticScenario
    {
        public StochasticScenario(IAgent baseAgent, IEnvironment baseEnvironment, IStochasticTestsConfig testsConfig)
            : base(baseAgent, baseEnvironment, testsConfig)
        {
        }

        public new IStochasticTestsConfig TestsConfig
        {
            get { return base.TestsConfig as IStochasticTestsConfig; }
        }

        #region IStochasticTestProfile Members

        public string LtmXmlFilePath { get; set; }

        #endregion
    }
}