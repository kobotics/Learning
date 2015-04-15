// ------------------------------------------
// DirectPolicyBehaviorManager.cs, Learning
//
// Created by Pedro Sequeira, 2014/1/22
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using Learning.Domain.Actions;
using Learning.Domain.Agents;

namespace Learning.Domain.Managers.Behavior
{
    public class DirectPolicyBehaviorManager : EpsilonGreedyBehaviorManager
    {
        public DirectPolicyBehaviorManager(IAgent agent) : base(agent)
        {
        }

        #region Overrides of Manager

        public override void Reset()
        {
        }

        public override void PrintResults(string path)
        {
        }

        #endregion

        #region Overrides of BehaviorManager

        protected override Policy GetStatePolicy(uint stateID)
        {
            //gets policy directly from q-values
            var policy = new Policy(new double[this.Actions.Count]);
            foreach (var action in ActionList)
                policy[action.ID] = this.Agent.LongTermMemory.GetStateActionValue(stateID, action.ID);

            return policy;
        }

        #endregion
    }
}