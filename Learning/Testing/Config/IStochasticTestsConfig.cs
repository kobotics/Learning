// ------------------------------------------
// IStochasticTestsConfig.cs, Learning
//
// Created by Pedro Sequeira, 2015/4/1
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

namespace Learning.Testing.Config
{
    public interface IStochasticTestsConfig : ITestsConfig
    {
        uint MaxActions { get; set; }
        uint MinActions { get; set; }
        uint MaxObservableStates { get; set; }
        uint MinObservableStates { get; set; }
        uint MaxNonObservableStates { get; set; }
        uint MaxTransitionsPerStateAction { get; set; }
        double StateActionRewardPercent { get; set; }
    }
}