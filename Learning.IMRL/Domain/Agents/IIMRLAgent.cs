// ------------------------------------------
// IIMRLAgent.cs, Learning.IMRL
//
// Created by Pedro Sequeira, 2013/2/6
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using Learning.Domain.Agents;
using Learning.Domain.Managers.Behavior;
using Learning.Domain.Managers.Learning;
using Learning.Domain.Memories;
using Learning.IMRL.Domain.Managers;
using Learning.IMRL.Domain.Managers.Motivation;

namespace Learning.IMRL.Domain.Agents
{
    public interface IIMRLAgent : ICellAgent
    {
        LearningManager ExtrinsicLearningManager { get; }
        ShortTermMemory ExtrinsicSTM { get; }
        LongTermMemory ExtrinsicLTM { get; }
        new IntrinsicMotivationManager MotivationManager { get; }
        new EpsilonGreedyBehaviorManager BehaviorManager { get; }
        StateRelevanceManager StateRelevanceManager { get; }
    }
}