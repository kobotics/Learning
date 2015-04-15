// ------------------------------------------
// SimulationControlForm.cs, Learning.Forms
//
// Created by Pedro Sequeira, 2013/12/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using System.Windows.Forms;
using Learning.Domain.Agents;

namespace Learning.Forms.Simulation
{
    public partial class SimulationControlForm : Form
    {
        protected readonly ISimulationForm simulationForm;

        public SimulationControlForm(ISimulationForm simulationForm)
        {
            this.simulationForm = simulationForm;
            this.InitializeComponent();
        }

        public CheckBox ShowAgInfoCBox
        {
            get { return showAgInfoCBox; }
        }

        public Button StepBtn
        {
            get { return stepBtn; }
        }

        public Button PauseBtn
        {
            get { return pauseBtn; }
        }

        public Button ResetBtn
        {
            get { return resetBtn; }
        }

        public CheckBox PrintResCheckBox
        {
            get { return printResCheckBox; }
        }

        public NumericUpDown AdvanceNumUd
        {
            get { return advanceNumUD; }
        }

        public TrackBar TimerTrackBar
        {
            get { return timerTrackBar; }
        }

        public ComboBox ControlAgComboBox
        {
            get { return controlAgComboBox; }
        }

        protected virtual void PauseBtnClick(object sender, EventArgs e)
        {
            this.simulationForm.PauseResumeSimulation();
        }

        private void StepBtnClick(object sender, EventArgs e)
        {
            this.simulationForm.StepSimulation();
        }

        private void TimerTrackBarScroll(object sender, EventArgs e)
        {
            this.simulationForm.UpdateInterval = this.timerTrackBar.Value;
        }

        private void ResetBtnClick(object sender, EventArgs e)
        {
            this.simulationForm.ResetSimulation();
        }

        private void AdvanceBtnClick(object sender, EventArgs e)
        {
            this.advanceBtn.Enabled = false;
            this.simulationForm.AdvanceSimulation((ulong) this.advanceNumUD.Value);
            this.advanceBtn.Enabled = true;
        }

        private void PrintWorldBtnClick(object sender, EventArgs e)
        {
            this.simulationForm.PrintEnvironment();
        }

        private void DebugCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            this.simulationForm.SetDebugMode(this.debugCheckBox.Checked);
        }

        private void ControlAgListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            this.simulationForm.ControlledAgent = (IAgent) this.controlAgComboBox.SelectedItem;
            this.simulationForm.Select();
        }
    }
}