using System.Windows.Forms;

namespace Learning.Forms.Simulation
{
    partial class SimulationControlForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.debugCheckBox = new System.Windows.Forms.CheckBox();
            this.timerTrackBar = new System.Windows.Forms.TrackBar();
            this.label15 = new System.Windows.Forms.Label();
            this.printResCheckBox = new System.Windows.Forms.CheckBox();
            this.advanceNumUD = new System.Windows.Forms.NumericUpDown();
            this.advanceBtn = new System.Windows.Forms.Button();
            this.printWorldBtn = new System.Windows.Forms.Button();
            this.stepBtn = new System.Windows.Forms.Button();
            this.pauseBtn = new System.Windows.Forms.Button();
            this.resetBtn = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.controlAgComboBox = new System.Windows.Forms.ComboBox();
            this.showAgInfoCBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.timerTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advanceNumUD)).BeginInit();
            this.SuspendLayout();
            // 
            // debugCheckBox
            // 
            this.debugCheckBox.AutoSize = true;
            this.debugCheckBox.Location = new System.Drawing.Point(15, 60);
            this.debugCheckBox.Name = "debugCheckBox";
            this.debugCheckBox.Size = new System.Drawing.Size(88, 17);
            this.debugCheckBox.TabIndex = 20;
            this.debugCheckBox.Text = "Debug Mode";
            this.debugCheckBox.UseVisualStyleBackColor = true;
            this.debugCheckBox.CheckedChanged += new System.EventHandler(this.DebugCheckBoxCheckedChanged);
            // 
            // timerTrackBar
            // 
            this.timerTrackBar.LargeChange = 100;
            this.timerTrackBar.Location = new System.Drawing.Point(71, 12);
            this.timerTrackBar.Maximum = 1000;
            this.timerTrackBar.Minimum = 1;
            this.timerTrackBar.Name = "timerTrackBar";
            this.timerTrackBar.Size = new System.Drawing.Size(140, 42);
            this.timerTrackBar.SmallChange = 10;
            this.timerTrackBar.TabIndex = 19;
            this.timerTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.timerTrackBar.Value = 100;
            this.timerTrackBar.Scroll += new System.EventHandler(this.TimerTrackBarScroll);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(12, 17);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(65, 13);
            this.label15.TabIndex = 18;
            this.label15.Text = "Update int.: ";
            // 
            // printResCheckBox
            // 
            this.printResCheckBox.AutoSize = true;
            this.printResCheckBox.Checked = true;
            this.printResCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.printResCheckBox.Location = new System.Drawing.Point(126, 145);
            this.printResCheckBox.Name = "printResCheckBox";
            this.printResCheckBox.Size = new System.Drawing.Size(85, 17);
            this.printResCheckBox.TabIndex = 27;
            this.printResCheckBox.Text = "Print Results";
            this.printResCheckBox.UseVisualStyleBackColor = true;
            // 
            // advanceNumUD
            // 
            this.advanceNumUD.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.advanceNumUD.Location = new System.Drawing.Point(111, 115);
            this.advanceNumUD.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.advanceNumUD.Name = "advanceNumUD";
            this.advanceNumUD.Size = new System.Drawing.Size(100, 20);
            this.advanceNumUD.TabIndex = 26;
            this.advanceNumUD.Value = new decimal(new int[] {
            99000,
            0,
            0,
            0});
            // 
            // advanceBtn
            // 
            this.advanceBtn.Location = new System.Drawing.Point(15, 112);
            this.advanceBtn.Name = "advanceBtn";
            this.advanceBtn.Size = new System.Drawing.Size(75, 23);
            this.advanceBtn.TabIndex = 24;
            this.advanceBtn.Text = "&Advance to:";
            this.advanceBtn.UseVisualStyleBackColor = true;
            this.advanceBtn.Click += new System.EventHandler(this.AdvanceBtnClick);
            // 
            // printWorldBtn
            // 
            this.printWorldBtn.Location = new System.Drawing.Point(14, 141);
            this.printWorldBtn.Name = "printWorldBtn";
            this.printWorldBtn.Size = new System.Drawing.Size(75, 23);
            this.printWorldBtn.TabIndex = 25;
            this.printWorldBtn.Text = "Print &World";
            this.printWorldBtn.UseVisualStyleBackColor = true;
            this.printWorldBtn.Click += new System.EventHandler(this.PrintWorldBtnClick);
            // 
            // stepBtn
            // 
            this.stepBtn.Enabled = false;
            this.stepBtn.Location = new System.Drawing.Point(81, 83);
            this.stepBtn.Name = "stepBtn";
            this.stepBtn.Size = new System.Drawing.Size(60, 23);
            this.stepBtn.TabIndex = 21;
            this.stepBtn.Text = "&Step";
            this.stepBtn.UseVisualStyleBackColor = true;
            this.stepBtn.Click += new System.EventHandler(this.StepBtnClick);
            // 
            // pauseBtn
            // 
            this.pauseBtn.Location = new System.Drawing.Point(15, 83);
            this.pauseBtn.Name = "pauseBtn";
            this.pauseBtn.Size = new System.Drawing.Size(60, 23);
            this.pauseBtn.TabIndex = 22;
            this.pauseBtn.Text = "&Pause";
            this.pauseBtn.UseVisualStyleBackColor = true;
            this.pauseBtn.Click += new System.EventHandler(this.PauseBtnClick);
            // 
            // resetBtn
            // 
            this.resetBtn.Location = new System.Drawing.Point(151, 83);
            this.resetBtn.Name = "resetBtn";
            this.resetBtn.Size = new System.Drawing.Size(60, 23);
            this.resetBtn.TabIndex = 23;
            this.resetBtn.Text = "&Reset";
            this.resetBtn.UseVisualStyleBackColor = true;
            this.resetBtn.Click += new System.EventHandler(this.ResetBtnClick);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(11, 173);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(87, 13);
            this.label20.TabIndex = 29;
            this.label20.Text = "Controlled agent:";
            // 
            // controlAgListBox
            // 
            this.controlAgComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.controlAgComboBox.FormattingEnabled = true;
            this.controlAgComboBox.Items.AddRange(new object[] {
            "Agent 1",
            "Agent 2",
            "Agent 3"});
            this.controlAgComboBox.Location = new System.Drawing.Point(14, 189);
            this.controlAgComboBox.Name = "controlAgComboBox";
            this.controlAgComboBox.Size = new System.Drawing.Size(197, 43);
            this.controlAgComboBox.TabIndex = 28;
            this.controlAgComboBox.SelectedIndexChanged += new System.EventHandler(this.ControlAgListBoxSelectedIndexChanged);
            // 
            // showAgInfoCBox
            // 
            this.showAgInfoCBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.showAgInfoCBox.AutoSize = true;
            this.showAgInfoCBox.Checked = true;
            this.showAgInfoCBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showAgInfoCBox.Location = new System.Drawing.Point(106, 60);
            this.showAgInfoCBox.Name = "showAgInfoCBox";
            this.showAgInfoCBox.Size = new System.Drawing.Size(105, 17);
            this.showAgInfoCBox.TabIndex = 30;
            this.showAgInfoCBox.Text = "Show Agent Info";
            this.showAgInfoCBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.showAgInfoCBox.UseVisualStyleBackColor = true;
            // 
            // SimulationControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(223, 250);
            this.ControlBox = false;
            this.Controls.Add(this.showAgInfoCBox);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.controlAgComboBox);
            this.Controls.Add(this.printResCheckBox);
            this.Controls.Add(this.advanceNumUD);
            this.Controls.Add(this.advanceBtn);
            this.Controls.Add(this.printWorldBtn);
            this.Controls.Add(this.stepBtn);
            this.Controls.Add(this.pauseBtn);
            this.Controls.Add(this.resetBtn);
            this.Controls.Add(this.debugCheckBox);
            this.Controls.Add(this.timerTrackBar);
            this.Controls.Add(this.label15);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SimulationControlForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Simulation Controller";
            ((System.ComponentModel.ISupportInitialize)(this.timerTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advanceNumUD)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox debugCheckBox;
        private System.Windows.Forms.TrackBar timerTrackBar;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.CheckBox printResCheckBox;
        private System.Windows.Forms.NumericUpDown advanceNumUD;
        private System.Windows.Forms.Button advanceBtn;
        private System.Windows.Forms.Button printWorldBtn;
        private System.Windows.Forms.Button stepBtn;
        private System.Windows.Forms.Button pauseBtn;
        private System.Windows.Forms.Button resetBtn;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.ComboBox controlAgComboBox;
        private System.Windows.Forms.CheckBox showAgInfoCBox;
    }
}