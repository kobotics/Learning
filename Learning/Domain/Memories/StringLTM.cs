// ------------------------------------------
// StringLTM.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using Learning.Domain.Agents;
using Learning.Domain.States;

namespace Learning.Domain.Memories
{
    [Serializable]
    public class StringLTM : CellAgentLTM
    {
        public const char STATE_SEPARATOR = ':';

        protected Dictionary<string, StringState> stringStates = new Dictionary<string, StringState>();

        public StringLTM(CellAgent agent, ShortTermMemory shortTermMemory)
            : base(agent, shortTermMemory)
        {
        }

        public override void Dispose()
        {
            this.stringStates.Clear();
            base.Dispose();
        }

        public override string ToString(IState state)
        {
            return state.ToString();
        }

        public override IState GetUpdatedCurrentState()
        {
            return this.GetState(this.Agent.PerceptionManager.CurrentSensations);
        }

        public override IState FromString(string stateStr)
        {
            //verifies if state already created
            if (this.stringStates.ContainsKey(stateStr))
                return this.stringStates[stateStr];

            //gets sensations from string
            var sensations = stateStr.Split(new[] {STATE_SEPARATOR});

            //returns new state
            return this.GetState(new HashSet<string>(sensations));
        }

        protected virtual IState GetState(HashSet<string> sensations)
        {
            //creates list of stimuli sorted alphabetically
            var stimuliList = new List<string>(sensations);
            stimuliList.Sort();

            var stateStr = (stimuliList.Count == 0)
                               ? string.Empty
                               : stimuliList.Aggregate(
                                   (current, stimulus) => string.Format("{0}{1}{2}", current, STATE_SEPARATOR, stimulus));

            //checks for new state
            if (!this.stringStates.ContainsKey(stateStr))
            {
                var stringState = new StringState(stateStr);
                this.stringStates[stateStr] = stringState;
            }

            return this.stringStates[stateStr];
        }
    }
}