// ------------------------------------------
// MovingPreysEnvironment.cs, Learning.Tests.EmotionalOptimization
//
// Created by Pedro Sequeira, 2012/10/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using Learning.IMRL.Domain;

namespace Learning.Tests.EmotionalOptimization.Domain.Environments
{
    [Serializable]
    public class MovingPreysEnvironment : AutoEatEnvironment
    {
        public MovingPreysEnvironment()
        {
            this.AutoEat = true;
        }
    }
}