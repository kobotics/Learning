// ------------------------------------------
// IManager.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using Learning.Domain.Agents;
using PS.Utilities;
using PS.Utilities.Serialization;

namespace Learning.Domain.Managers
{
    public interface IManager : IDisposable, IUpdatable, IXmlSerializable
    {
        IAgent Agent { get; }
        void Reset();
    }
}