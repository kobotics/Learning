// ------------------------------------------
// PacmanScenario.cs, Learning.Tests.EmotionalOptimization
// 
// Created by Pedro Sequeira, 2013/12/19
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using Learning.Domain.Agents;
using Learning.Domain.Environments;
using Learning.Testing.Config;
using Learning.Testing.Config.Scenarios;

namespace Learning.Tests.EmotionalOptimization.Testing
{
    [Serializable]
    public class PacmanScenario : SingleScenario, IPacmanScenario
    {
        public PacmanScenario(IAgent baseAgent, IEnvironment baseEnvironment, ITestsConfig testsConfig)
            : base(baseAgent, baseEnvironment, testsConfig)
        {
        }

        #region IPacmanScenario Members

        public uint MaxLives { get; set; }
        public double DotReward { get; set; }
        public double BigDotReward { get; set; }
        public double DeathReward { get; set; }
        public bool HideDots { get; set; }
        public bool HideBigDot { get; set; }
        public bool HideKeeperGhost { get; set; }
        public bool PowerPelletEnabled { get; set; }

        #endregion
    }
}