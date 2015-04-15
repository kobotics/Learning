// ------------------------------------------
// INORCAgent.cs, Learning.IMRL
//
// Created by Pedro Sequeira, 2013/7/16
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using Learning.IMRL.Domain.Managers.Reward;

namespace Learning.IMRL.Domain.Agents
{
    public interface INORCAgent : IIMRLAgent
    {
        RewardParametersManager RewardParametersManager { get; }
    }
}