// ------------------------------------------
// IMotivationManager.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using PS.Utilities.Math;

namespace Learning.Domain.Managers.Motivation
{
    public interface IMotivationManager : IManager
    {
        StatisticalQuantity ExtrinsicReward { get; }
        double GetReward(uint prevStateID, uint actionID, uint nextStateID);
    }
}