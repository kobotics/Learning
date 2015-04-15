// ------------------------------------------
// IShortTermMemory.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using Learning.Domain.Actions;
using Learning.Domain.States;
using PS.Utilities.Math;

namespace Learning.Domain.Memories
{
    public interface IShortTermMemory : IMemory
    {
        IState CurrentState { get; set; }
        IState PreviousState { get; set; }
        IAction CurrentAction { get; set; }
        StatisticalQuantity CurrentReward { get; }
        double PredictionError { get; set; }
    }
}