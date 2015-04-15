// ------------------------------------------
// EpsilonGreedyBehaviorManager.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using System.IO;
using Learning.Domain.Actions;
using Learning.Domain.Agents;
using PS.Utilities.Math;

namespace Learning.Domain.Managers.Behavior
{
    [Serializable]
    public class EpsilonGreedyBehaviorManager : BehaviorManager
    {
        private const string EPS_FILE_NAME = "Epsilon.csv";

        public EpsilonGreedyBehaviorManager(IAgent agent) : base(agent)
        {
            this.Epsilon = new StatisticalQuantity();
            this.StartingEpsilon = this.Scenario.TestsConfig.Epsilon;
        }

        public double StartingEpsilon { get; set; }

        public StatisticalQuantity Epsilon { get; private set; }

        protected override Policy GetStatePolicy(uint stateID)
        {
            //initialize policy uniformly
            var policy = new Policy((uint) this.Actions.Count, false);

            //gets best action and tests
            var bestActionID = this.Agent.LongTermMemory.GetMaxStateAction(stateID);
            if (bestActionID == uint.MaxValue) return policy;

            var prob = Rand.NextDouble();
            var greedy = prob > this.Epsilon.Value;

            //sets prob according to whether action is the best and current epsilon value
            var greedyProb = 1d - this.Epsilon.Value;
            var otherActionsProb = this.Epsilon.Value/(this.Actions.Count - 1d);

            for (var i = 0u; i < this.Actions.Count; i++)
                policy[i] = greedy ? (i == bestActionID ? 1 : 0) : 1d/this.Actions.Count;
            //policy[i] = i == bestActionID ? greedyProb : otherActionsProb;

            return policy;
        }

        public override void Update()
        {
            base.Update();
            this.UpdateEpsilon();
        }

        public override void Dispose()
        {
            this.Epsilon.Dispose();
        }

        public override void PrintResults(string path)
        {
            this.Epsilon.PrintStatisticsToCSV(
                string.Format("{0}{1}{2}", path, Path.DirectorySeparatorChar, EPS_FILE_NAME));
        }

        public override void Reset()
        {
            this.Epsilon = new StatisticalQuantity(this.Epsilon.Value, new Range(0, 1));
        }

        public virtual void UpdateEpsilon()
        {
            //updates epsilon
            this.Epsilon.Value = this.GetUpdatedEpsilonValue();
        }

        protected virtual double GetUpdatedEpsilonValue()
        {
            //just keep epsilon constant
            return this.StartingEpsilon;
        }
    }
}