// ------------------------------------------
// IAction.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using Learning.Domain.Agents;
using PS.Utilities;

namespace Learning.Domain.Actions
{
    public interface IAction : IIdentifiableObject
    {
        uint ID { get; set; }
        IAgent Agent { get; }
        double Execute();
    }
}