// ------------------------------------------
// PoisonedSeasonEnvironment.cs, Learning.Tests.EmotionalOptimization
//
// Created by Pedro Sequeira, 2012/10/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using Learning.IMRL.Emotions.Testing;

namespace Learning.Tests.EmotionalOptimization.Domain.Environments
{
    [Serializable]
    public class PoisonedSeasonEnvironment : FixedStartRabbitHareEnvironment
    {
        public PoisonedSeasonEnvironment()
        {
            this.Rabbit.Reward = 0.1f;
            this.AutoEat = true;
        }

        protected new IEmotionalTestsConfig TestsConfig
        {
            get { return base.TestsConfig as IEmotionalTestsConfig; }
        }

        public override void Update()
        {
            base.Update();

            //tests if season has changed
            if ((this.Agent == null) || (this.Agent.ShortTermMemory.PreviousState == null) ||
                ((!(this.Agent.LongTermMemory.TimeStep%(double) this.TestsConfig.NumStepsPerSeason).Equals(0f))))
                return;

            //changes season
            this.ChangeReward();
        }

        protected void ChangeReward()
        {
            this.Hare.Reward = this.Hare.Reward.Equals(1d) ? -1d : 1d;
        }
    }
}