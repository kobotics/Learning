// ------------------------------------------
// IStimuliState.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System.Collections.Generic;

namespace Learning.Domain.States
{
    public interface IStimuliState : IState
    {
        HashSet<string> Sensations { get; }
    }
}