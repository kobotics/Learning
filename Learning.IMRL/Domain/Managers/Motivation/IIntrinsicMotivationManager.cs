// ------------------------------------------
// IIntrinsicMotivationManager.cs, Learning.IMRL
//
// Created by Pedro Sequeira, 2013/7/11
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using Learning.Domain.Managers.Motivation;
using PS.Utilities.Math;

namespace Learning.IMRL.Domain.Managers.Motivation
{
    public interface IIntrinsicMotivationManager : IMotivationManager
    {
        StatisticalQuantity IntrinsicReward { get; }
        double GetIntrinsicReward(uint prevState, uint action, uint nextState);
        double GetExtrinsicReward(uint prevState, uint action);
    }
}