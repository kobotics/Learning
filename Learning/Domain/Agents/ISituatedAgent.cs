// ------------------------------------------
// ISituatedAgent.cs, Learning
//
// Created by Pedro Sequeira, 2014/6/19
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using Learning.Domain.Environments;

namespace Learning.Domain.Agents
{
    public interface ISituatedAgent : IAgent
    {
        IEnvironment Environment { get; set; }
    }
}