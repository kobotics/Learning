// ------------------------------------------
// AgentInfoForm.cs, Learning.Forms
//
// Created by Pedro Sequeira, 2013/12/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Learning.Domain.Agents;
using Learning.Domain.Managers.Behavior;
using Learning.Domain.Memories;

namespace Learning.Forms.Simulation
{
    public partial class AgentInfoForm : Form
    {
        protected IAgent agent;

        public AgentInfoForm(IEnumerable<IAgent> agents)
        {
            this.InitializeComponent();

            this.agentsComboBox.Items.AddRange(agents.ToArray());
            this.agentsComboBox.SelectedIndex = 0;
        }

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.Text = string.Format("Agent Information ({0} available)", this.agentsComboBox.Items.Count);
        }

        public virtual void UpdateVariables()
        {
            //updates all info related to the selected agent
            var stm = this.agent.ShortTermMemory;
            var ltm = this.agent.LongTermMemory;
            var prevState = stm.PreviousState;
            var curAction = stm.CurrentAction;
            if ((prevState == null) || (curAction == null)) return;

            this.prevStateTxtBox.Text = prevState.ToString();
            this.curStateTxtBox.Text = stm.CurrentState.ToString();
            this.actionTxtBox.Text = curAction.ToString();
            this.rewardTxtBox.Text = stm.CurrentReward.Value.ToString("#,0.####");
            this.goalDistTxtBox.Text = this.agent is ICellAgent
                ? ((CellAgentLTM) ltm).GetDistanceToOptimalState().ToString("#,0.##")
                : "NA";
            this.actionValueListBox.Items.Clear();
            for (var actionID = 0u; actionID < ltm.NumActions; actionID++)
            {
                var actionValue = ltm.GetStateActionValue(stm.CurrentState.ID, actionID);
                var actionRwdAvg = ltm.GetStateActionReward(stm.CurrentState.ID, actionID);
                this.actionValueListBox.Items.Add(
                    string.Format("{0}: {1:#,0.##}/{2:#,0.##}",
                        ltm.GetAction(actionID), actionValue, actionRwdAvg));
            }

            this.fitnessTxtBox.Text = this.agent.Fitness.Value.ToString("#,0.##");
            this.numTasksTxtBox.Text = ltm.NumTasks.Value.ToString(CultureInfo.InvariantCulture);
            this.numStatesTxtBox.Text = ltm.NumStates.ToString(CultureInfo.InvariantCulture);
            this.alphaTrackBar.Value = (int) (this.agent.LearningManager.LearningRate.Value*10);

            var behaviorManager = this.agent.BehaviorManager;
            if (behaviorManager is EpsilonGreedyBehaviorManager)
                this.epsilonTrackBar.Value =
                    (int) System.Math.Min(
                        ((EpsilonGreedyBehaviorManager) behaviorManager).Epsilon.Value*10, 10);
        }

        private void AgentsComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            this.agent = (IAgent) this.agentsComboBox.SelectedItem;
            this.UpdateVariables();
        }
    }
}