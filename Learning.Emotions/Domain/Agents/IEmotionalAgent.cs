// ------------------------------------------
// IEmotionalAgent.cs, Learning.Emotions
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using Learning.Domain.Agents;
using Learning.IMRL.Emotions.Domain.Managers;
using Learning.IMRL.Emotions.Domain.Memories;

namespace Learning.IMRL.Emotions.Domain.Agents
{
    public interface IEmotionalAgent : IAgent
    {
        new EmotionalSTM ShortTermMemory { get; }
        //new EmotionalMotivationManager MotivationManager { get; }
        EmotionsManager EmotionsManager { get; }
    }
}