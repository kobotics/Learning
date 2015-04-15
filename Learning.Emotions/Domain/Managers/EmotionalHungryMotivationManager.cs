// ------------------------------------------
// EmotionalHungryMotivationManager.cs, Learning.Emotions
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using Learning.Domain;
using Learning.Domain.Agents;
using Learning.IMRL.Domain.Managers.Motivation;
using Learning.IMRL.Emotions.Domain.Agents;

namespace Learning.IMRL.Emotions.Domain.Managers
{
    [Serializable]
    public class EmotionalHungryMotivationManager : EmotionalMotivationManager, IHungerMotivationManager
    {
        public const string HUNGER_ID = "hunger";

        public EmotionalHungryMotivationManager(EmotionalAgent agent) : base(agent)
        {
            //agent starts hungry
            this.Hunger = new Need(HUNGER_ID, 1, 0, 0, 0) {Value = 1};
        }

        public new CellAgent Agent
        {
            get { return base.Agent as CellAgent; }
        }

        #region IHungerMotivationManager Members

        public Need Hunger { get; protected set; }

        public override double GetExtrinsicReward(uint stateID, uint actionID)
        {
            var state = this.Agent.LongTermMemory.GetState(stateID);
            var action = this.Agent.BehaviorManager.ActionList[(int) actionID];
            var agentFinishedTask = this.Agent.Environment.AgentFinishedTask(this.Agent, state, action);

            this.Hunger.Value = (uint) (agentFinishedTask ? 0 : 1);

            return base.GetExtrinsicReward(stateID, actionID);
        }

        

        #endregion
    }
}