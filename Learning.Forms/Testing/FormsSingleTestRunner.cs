// ------------------------------------------
// FormsSingleTestRunner.cs, Learning.Forms
// 
// Created by Pedro Sequeira, 2014/03/10
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System.Collections.Generic;
using System.Windows.Forms;
using Learning.Forms.Simulation;
using Learning.Testing.Config;
using Learning.Testing.Runners;
using Learning.Testing.SingleTests;
using PS.Utilities.Forms;

namespace Learning.Forms.Testing
{
    public class FormsSingleTestRunner : SingleTestRunner
    {
        private const string OPTION_CONSOLE = "Run on &console";
        private const string OPTION_FORM = "Run visual &form";
        private const string OPTIONS_FORM_TITLE = "Options for single test: ";

        public FormsSingleTestRunner(ITestsConfig testsConfig) : base(testsConfig)
        {
        }

        protected override void RunSimulation(FitnessTest test)
        {
            //tests execution only on console
            if (this.ForceConsole || !this.TestsConfig.GraphicsEnabled)
            {
                this.RunConsoleApplication(test);
                return;
            }

            //shows menu
            InitApplication();
            this.RunConsoleApplication(test);
            /*using (var chooseForm =
                new ChooseOptionForm(new List<string> {OPTION_CONSOLE, OPTION_FORM})
                {Text = OPTIONS_FORM_TITLE + test.TestName})
            {
                chooseForm.Show();
                while (!chooseForm.IsDisposed)
                    Application.DoEvents();

                switch (chooseForm.ChosenOption)
                {
                    case OPTION_FORM:
                        this.RunFormApplication(this.TestsConfig.CellSize, false, test);
                        break;
                    case OPTION_CONSOLE:
                        this.RunConsoleApplication(test);
                        break;
                    default:
                        this.RunFormApplication(this.TestsConfig.CellSize, true, test);
                        break;
                }
            }*/               
        }

        protected override void RunConsoleApplication(FitnessTest test)
        {
            //also shows progress form
            using (new ProgressFormUpdater(test)
                   {
                       Visible = this.TestsConfig.GraphicsEnabled,
                       Text = test.TestName
                   })
            {
                base.RunConsoleApplication(test);
            }
        }

        protected virtual void RunFormApplication(int cellSize, bool limitedUser, FitnessTest test)
        {
            //creates form application
            var simulationForm = this.CreateSimulationForm(test, limitedUser);
            simulationForm.Text = test.TestName;
            simulationForm.CellSize = cellSize;

            //runs player form application
            simulationForm.Init();
            simulationForm.PauseResumeSimulation();
            Application.Run((Form) simulationForm);
        }

        protected virtual ISimulationForm CreateSimulationForm(FitnessTest test, bool limitedUser)
        {
            return new SimulationDisplayForm(test, limitedUser);
        }

        private static void InitApplication()
        {
            Application.EnableVisualStyles();
        }
    }
}