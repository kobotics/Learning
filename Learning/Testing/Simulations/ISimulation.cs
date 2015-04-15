// ------------------------------------------
// ISimulation.cs, Learning
//
// Created by Pedro Sequeira, 2013/12/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using Learning.Domain.Agents;
using PS.Utilities;
using PS.Utilities.Math;
using Learning.Testing.Config;
using Learning.Testing.Config.Scenarios;

namespace Learning.Testing.Simulations
{
    public interface ISimulation : IResetable, IDisposable
    {
        long MemoryUsage { get; }
        TimeSpan TimeElapsed { get; }
        IAgent Agent { get; }
        LogWriter LogWriter { get; }
        IScenario Scenario { get; }
        StatisticalQuantity Score { get; }
        bool Run();
    }
}