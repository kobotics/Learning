// ------------------------------------------
// FixedStartRabbitHareEnvironment.cs, Learning.Tests.EmotionalOptimization
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using Learning.Domain.Actions;
using Learning.Domain.Agents;
using Learning.Domain.Cells;
using Learning.Domain.States;
using Learning.IMRL.Domain;

namespace Learning.Tests.EmotionalOptimization.Domain.Environments
{
    [Serializable]
    public class FixedStartRabbitHareEnvironment : AutoEatEnvironment
    {
        public FixedStartRabbitHareEnvironment()
        {
            //creates food prey rabbit
            this.Rabbit = new CellElement
                              {
                                  IdToken = "rabbit",
                                  Description = "food",
                                  Reward = 0.01f,
                                  Visible = true,
                                  Walkable = true,
                                  HasSmell = true,
                                  ImagePath = "../../../../bin/resources/rabbit.png",
                              };
        }

        public CellElement Rabbit { get; protected set; }

        public override void Reset()
        {
            if (this.Agent == null) return;

            //agent always starts from 2nd row right position;
            this.Agent.Cell = this.Cells[3, 3];

            if (this.Agent.Environment == null)
                this.Agent.Environment = this;
        }

        public override void CreateCells(uint rows, uint cols, string configFile)
        {
            base.CreateCells(rows, cols, configFile);

            //sets food locations
            this.Hare.Cell = this.Cells[3, 1];
            this.Rabbit.Cell = this.Cells[3, 5];
        }

        public override double GetAgentReward(IAgent agent, IState state, IAction action)
        {
            return this.AgentFinishedTask(agent, state, action)
                       ? (((IStimuliState)state).Sensations.Contains(this.Hare.IdToken)
                              ? this.Hare.Reward
                              : this.Rabbit.Reward)
                       : 0;
        }

        public override bool AgentFinishedTask(IAgent agent, IState state, IAction action)
        {
            if ((state == null) || !(state is IStimuliState)) return false;
            var sensations = ((IStimuliState)state).Sensations;
            return (this.AutoEat || ((action != null) && (action is Eat))) &&
                   (sensations.Contains(this.Hare.IdToken) || sensations.Contains(this.Rabbit.IdToken));
        }
    }
}