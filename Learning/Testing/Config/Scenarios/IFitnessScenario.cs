// ------------------------------------------
// IFitnessTestProfile.cs, Learning
//
// Created by Pedro Sequeira, 2014/1/28
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using Learning.Testing.Simulations;

namespace Learning.Testing.Config.Scenarios
{
    public interface IFitnessScenario : IScenario
    {
        IAgentFitnessFunction AgentFitnessFunction { get; set; }
        string FitnessText { get; set; }
    }
}