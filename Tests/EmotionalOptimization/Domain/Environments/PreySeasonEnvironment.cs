// ------------------------------------------
// PreySeasonEnvironment.cs, Learning.Tests.EmotionalOptimization
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using Learning.Domain.Actions;
using Learning.Domain.Agents;
using Learning.Domain.States;
using Learning.IMRL.Emotions.Testing;

namespace Learning.Tests.EmotionalOptimization.Domain.Environments
{
    [Serializable]
    public class PreySeasonEnvironment : FixedStartRabbitHareEnvironment
    {
        public const double RABBIT_PUNISHMENT = -1f;

        protected uint maxRabbits;
        protected int numEatenRabbits;
        protected bool rabbitSeason;
        protected bool randFirstSeason;

        public PreySeasonEnvironment()
        {
            this.Rabbit.Reward = 0.1f;
            this.Hare.Reward = 1f;
            this.maxRabbits = 9;

            this.AutoEat = true;
        }

        protected new IEmotionalTestsConfig TestsConfig
        {
            get { return base.TestsConfig as IEmotionalTestsConfig; }
        }

        public override void Init()
        {
            //chooses season randomly and forces changes in season
            this.rabbitSeason = this.TestsConfig.RandStart && this.rand.Next(2) == 0;
        }

        public override void Update()
        {
            base.Update();

            //tests if season has changed
            if ((this.Agent == null) || (this.Agent.ShortTermMemory.PreviousState == null) ||
                !(this.Agent.LongTermMemory.TimeStep%(double) this.TestsConfig.NumStepsPerSeason).Equals(0f))
                return;

            //changes season
            this.ChangeSeason();
        }

        public override double GetAgentReward(IAgent agent, IState state, IAction action)
        {
            //checks for "normal" reward
            if (!this.AgentFinishedTask(agent, state, action))
                return 0;

            //in hare season just return hare reward
            if (!this.rabbitSeason)
                return this.Hare.Reward;

            //if in rabbit season, check for max eaten rabbits, in which case the agent is penalized 
            if (++this.numEatenRabbits > this.maxRabbits)
                return RABBIT_PUNISHMENT;

            return this.Rabbit.Reward;
        }

        protected void ChangeSeason()
        {
            if (this.rabbitSeason)
                this.InitHareSeason();
            else
                this.InitRabbitSeason();

            this.rabbitSeason = !this.rabbitSeason;
        }

        protected void InitHareSeason()
        {
            //puts hare in cell, takes rabbit away and resets eaten rabbits counter
            this.Hare.Cell = this.Cells[3, 5];
            this.Rabbit.Cell = null;
            this.numEatenRabbits = 0;
        }

        protected void InitRabbitSeason()
        {
            //puts rabbit in cell, takes hare away
            this.Hare.Cell = null;
            this.Rabbit.Cell = this.Cells[3, 1];
        }
    }
}