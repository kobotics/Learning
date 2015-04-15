// ------------------------------------------
// PacmanAgent.cs, Learning.Tests.EmotionalOptimization
//
// Created by Pedro Sequeira, 2012/10/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using Learning.Domain.Managers.Perception;
using Learning.IMRL.Emotions.Domain.Agents;
using Learning.Tests.EmotionalOptimization.Domain.Managers;

namespace Learning.Tests.EmotionalOptimization.Domain.Agents
{
    [Serializable]
    public class PacmanAgent : EmotionalAgent
    {
        public PacmanAgent()
        {
            this.IdToken = "pacman";
            this.ImagePath = "../../../../bin/resources/pacman/pacman.png";
        }

        protected override PerceptionManager CreatePerceptionManager()
        {
            return new PacmanPerceptionManager(this);
        }
    }
}