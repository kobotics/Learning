// ------------------------------------------
// IHungerMotivationManager.cs, Learning.IMRL
//
// Created by Pedro Sequeira, 2013/7/11
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using Learning.Domain;

namespace Learning.IMRL.Domain.Managers.Motivation
{
    public interface IHungerMotivationManager : IIntrinsicMotivationManager
    {
        Need Hunger { get; }
    }
}