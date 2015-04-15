// ------------------------------------------
// IHungerThirstAgent.cs, Learning.Tests.EmotionalOptimization
//
// Created by Pedro Sequeira, 2012/10/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using Learning.Domain.Agents;
using Learning.Tests.EmotionalOptimization.Domain.Environments;

namespace Learning.Tests.EmotionalOptimization.Domain.Agents
{
    public interface IHungerThirstAgent : ICellAgent
    {
        new HungerThirstEnvironment Environment { get; }
    }
}