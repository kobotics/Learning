// ------------------------------------------
// IAgent.cs, Learning
//
// Created by Pedro Sequeira, 2013/12/3
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using System.Collections.Generic;
using Learning.Domain.Actions;
using Learning.Domain.Managers.Behavior;
using Learning.Domain.Managers.Learning;
using Learning.Domain.Managers.Motivation;
using Learning.Domain.Memories;
using PS.Utilities.Math;
using Learning.Testing.Config;
using Learning.Testing.Config.Parameters;
using Learning.Testing.Config.Scenarios;
using PS.Utilities;

namespace Learning.Domain.Agents
{
    public interface IAgent : IUpdatable, IDisposable, IIdentifiableObject, IInitializable
    {
        Dictionary<string, IAction> Actions { get; }
        BehaviorManager BehaviorManager { get; }
        LearningManager LearningManager { get; }
        LongTermMemory LongTermMemory { get; }
        ShortTermMemory ShortTermMemory { get; }
        IMotivationManager MotivationManager { get; }
        LogWriter LogWriter { get; set; }
        StatisticsCollection StatisticsCollection { get; }
        StatisticalQuantity Fitness { get; set; }
        IScenario Scenario { get; set; }
        ITestParameters TestParameters { get; set; }
        void Reset();
        void PrintAll(string path, string imgFormat);
        IAgent CreateNew();
    }
}