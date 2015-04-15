// ------------------------------------------
// IStochasticScenario.cs, Learning
// 
// Created by Pedro Sequeira, 2015/03/30
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

namespace Learning.Testing.Config.Scenarios
{
    public interface IStochasticScenario : IFitnessScenario
    {
        string LtmXmlFilePath { get; set; }
    }
}