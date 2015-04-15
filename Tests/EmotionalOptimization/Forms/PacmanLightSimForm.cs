// ------------------------------------------
// PacmanLightSimForm.cs, Learning.Tests.EmotionalOptimization
//
// Created by Pedro Sequeira, 2012/10/17
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System.Windows.Forms;
using Learning.Forms.Simulation;
using Learning.Testing.SingleTests;
using Learning.Tests.EmotionalOptimization.Domain.Environments;

namespace Learning.Tests.EmotionalOptimization.Forms
{
    public partial class PacmanLightSimForm : SimulationDisplayForm
    {
        protected PictureBox[] lifePBoxes = new PictureBox[4];
        protected uint prevLives;

        public PacmanLightSimForm(FitnessTest test) : base(test, false)
        {
        }


        public override void Init()
        {
            this.lifePBoxes[0] = this.lifePBox1;
            this.lifePBoxes[1] = this.lifePBox2;
            this.lifePBoxes[2] = this.lifePBox3;
            this.lifePBoxes[3] = this.lifePBox4;

            base.Init();
        }

        protected override bool Step()
        {
            lock (this.locker)
            {
                var retValue = base.Step();

                //updates pacman lives info
                if (retValue) this.UpdateLives();

                return retValue;
            }
        }

        protected virtual void UpdateLives()
        {
            lock (this.locker)
            {
                if (!(this.environmentControl.Environment is PacmanEnvironment))
                    return;

                var curLives = System.Math.Max(
                    1, ((PacmanEnvironment) this.environmentControl.Environment).NumLives);
                if (curLives == this.prevLives) return;

                for (var i = 0; i < this.lifePBoxes.Length; i++)
                    this.lifePBoxes[i].Visible = i < curLives;

                this.prevLives = curLives;
            }
        }
    }
}