// ------------------------------------------
// ISimAnnTestConfig.cs, Learning
//
// Created by Pedro Sequeira, 2014/2/14
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
namespace Learning.Testing.Config
{
    public interface ISimAnnTestConfig :  IStochasticOptimTestsConfig
    {
        double TempThreshold { get; set; }
        double Alpha { get; set; }
        double InitialTemperature { get; set; }


    }
}