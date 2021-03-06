﻿namespace Learning.Forms.Simulation
{
    partial class AgentInfoForm
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
            this.label20 = new System.Windows.Forms.Label();
            this.actionValueListBox = new System.Windows.Forms.ListBox();
            this.rewardTxtBox = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.actionTxtBox = new System.Windows.Forms.TextBox();
            this.curStateTxtBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.prevStateTxtBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.goalDistTxtBox = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numTasksTxtBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.alphaTrackBar = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.epsilonTrackBar = new System.Windows.Forms.TrackBar();
            this.fitnessTxtBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.numStatesTxtBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.agentsComboBox = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.alphaTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.epsilonTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(12, 215);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(145, 13);
            this.label20.TabIndex = 12;
            this.label20.Text = "Action Values / Reward Avg:";
            // 
            // actionValueListBox
            // 
            this.actionValueListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.actionValueListBox.FormattingEnabled = true;
            this.actionValueListBox.Items.AddRange(new object[] {
            "action1",
            "action2",
            "action3",
            "action4",
            "action5"});
            this.actionValueListBox.Location = new System.Drawing.Point(15, 231);
            this.actionValueListBox.Name = "actionValueListBox";
            this.actionValueListBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.actionValueListBox.Size = new System.Drawing.Size(196, 69);
            this.actionValueListBox.TabIndex = 5;
            // 
            // rewardTxtBox
            // 
            this.rewardTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rewardTxtBox.Location = new System.Drawing.Point(65, 115);
            this.rewardTxtBox.Name = "rewardTxtBox";
            this.rewardTxtBox.ReadOnly = true;
            this.rewardTxtBox.Size = new System.Drawing.Size(146, 20);
            this.rewardTxtBox.TabIndex = 2;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(12, 118);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(47, 13);
            this.label18.TabIndex = 4;
            this.label18.Text = "Reward:";
            // 
            // actionTxtBox
            // 
            this.actionTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.actionTxtBox.Location = new System.Drawing.Point(65, 89);
            this.actionTxtBox.Name = "actionTxtBox";
            this.actionTxtBox.ReadOnly = true;
            this.actionTxtBox.Size = new System.Drawing.Size(146, 20);
            this.actionTxtBox.TabIndex = 1;
            // 
            // curStateTxtBox
            // 
            this.curStateTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.curStateTxtBox.Location = new System.Drawing.Point(15, 157);
            this.curStateTxtBox.Name = "curStateTxtBox";
            this.curStateTxtBox.ReadOnly = true;
            this.curStateTxtBox.Size = new System.Drawing.Size(196, 20);
            this.curStateTxtBox.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 141);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Current State:";
            // 
            // prevStateTxtBox
            // 
            this.prevStateTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prevStateTxtBox.Location = new System.Drawing.Point(15, 63);
            this.prevStateTxtBox.Name = "prevStateTxtBox";
            this.prevStateTxtBox.ReadOnly = true;
            this.prevStateTxtBox.Size = new System.Drawing.Size(196, 20);
            this.prevStateTxtBox.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Previous State:";
            // 
            // goalDistTxtBox
            // 
            this.goalDistTxtBox.Location = new System.Drawing.Point(96, 183);
            this.goalDistTxtBox.Name = "goalDistTxtBox";
            this.goalDistTxtBox.ReadOnly = true;
            this.goalDistTxtBox.Size = new System.Drawing.Size(115, 20);
            this.goalDistTxtBox.TabIndex = 4;
            this.goalDistTxtBox.Text = "0";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(12, 186);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(78, 13);
            this.label19.TabIndex = 15;
            this.label19.Text = "Goal distance: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Action:";
            // 
            // numTasksTxtBox
            // 
            this.numTasksTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numTasksTxtBox.Location = new System.Drawing.Point(81, 335);
            this.numTasksTxtBox.Name = "numTasksTxtBox";
            this.numTasksTxtBox.ReadOnly = true;
            this.numTasksTxtBox.Size = new System.Drawing.Size(130, 20);
            this.numTasksTxtBox.TabIndex = 7;
            this.numTasksTxtBox.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 338);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Num tasks: ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 419);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Epsilon ε: ";
            // 
            // alphaTrackBar
            // 
            this.alphaTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.alphaTrackBar.Enabled = false;
            this.alphaTrackBar.LargeChange = 3;
            this.alphaTrackBar.Location = new System.Drawing.Point(71, 389);
            this.alphaTrackBar.Name = "alphaTrackBar";
            this.alphaTrackBar.Size = new System.Drawing.Size(140, 42);
            this.alphaTrackBar.TabIndex = 8;
            this.alphaTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.alphaTrackBar.Value = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 393);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Learning α:";
            // 
            // epsilonTrackBar
            // 
            this.epsilonTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.epsilonTrackBar.Enabled = false;
            this.epsilonTrackBar.LargeChange = 3;
            this.epsilonTrackBar.Location = new System.Drawing.Point(71, 414);
            this.epsilonTrackBar.Name = "epsilonTrackBar";
            this.epsilonTrackBar.Size = new System.Drawing.Size(140, 42);
            this.epsilonTrackBar.TabIndex = 9;
            this.epsilonTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.epsilonTrackBar.Value = 10;
            // 
            // fitnessTxtBox
            // 
            this.fitnessTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fitnessTxtBox.Location = new System.Drawing.Point(81, 309);
            this.fitnessTxtBox.Name = "fitnessTxtBox";
            this.fitnessTxtBox.ReadOnly = true;
            this.fitnessTxtBox.Size = new System.Drawing.Size(130, 20);
            this.fitnessTxtBox.TabIndex = 6;
            this.fitnessTxtBox.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 312);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "Fitness: ";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 364);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(66, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "Num states: ";
            // 
            // numStatesTxtBox
            // 
            this.numStatesTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numStatesTxtBox.Location = new System.Drawing.Point(81, 361);
            this.numStatesTxtBox.Name = "numStatesTxtBox";
            this.numStatesTxtBox.ReadOnly = true;
            this.numStatesTxtBox.Size = new System.Drawing.Size(130, 20);
            this.numStatesTxtBox.TabIndex = 7;
            this.numStatesTxtBox.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 13);
            this.label9.TabIndex = 25;
            this.label9.Text = "Agent:";
            // 
            // agentsComboBox
            // 
            this.agentsComboBox.FormattingEnabled = true;
            this.agentsComboBox.Location = new System.Drawing.Point(65, 6);
            this.agentsComboBox.Name = "agentsComboBox";
            this.agentsComboBox.Size = new System.Drawing.Size(146, 21);
            this.agentsComboBox.TabIndex = 0;
            this.agentsComboBox.SelectedIndexChanged += new System.EventHandler(this.AgentsComboBoxSelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label10.Location = new System.Drawing.Point(15, 35);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(196, 2);
            this.label10.TabIndex = 27;
            // 
            // AgentInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(223, 468);
            this.ControlBox = false;
            this.Controls.Add(this.label10);
            this.Controls.Add(this.agentsComboBox);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.fitnessTxtBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.epsilonTrackBar);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.alphaTrackBar);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numStatesTxtBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.numTasksTxtBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.goalDistTxtBox);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.actionValueListBox);
            this.Controls.Add(this.rewardTxtBox);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.actionTxtBox);
            this.Controls.Add(this.curStateTxtBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.prevStateTxtBox);
            this.Controls.Add(this.label4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "AgentInfoForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Agent Information";
            ((System.ComponentModel.ISupportInitialize)(this.alphaTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.epsilonTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.ListBox actionValueListBox;
        private System.Windows.Forms.TextBox rewardTxtBox;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox actionTxtBox;
        private System.Windows.Forms.TextBox curStateTxtBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox prevStateTxtBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox goalDistTxtBox;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox numTasksTxtBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TrackBar alphaTrackBar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar epsilonTrackBar;
        private System.Windows.Forms.TextBox fitnessTxtBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox numStatesTxtBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox agentsComboBox;
        private System.Windows.Forms.Label label10;

    }
}