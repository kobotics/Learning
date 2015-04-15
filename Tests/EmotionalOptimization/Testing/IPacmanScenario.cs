// ------------------------------------------
// IPacmanTestProfile.cs, Learning.Tests.EmotionalOptimization
//
// Created by Pedro Sequeira, 2013/12/19
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using Learning.Testing.Config;
using Learning.Testing.Config.Scenarios;

namespace Learning.Tests.EmotionalOptimization.Testing
{
    public interface IPacmanScenario : IScenario
    {
        uint MaxLives { get; set; }
        double DotReward { get; set; }
        double BigDotReward { get; set; }
        double DeathReward { get; set; }
        bool HideDots { get; set; }
        bool HideBigDot { get; set; }
        bool HideKeeperGhost { get; set; }
        bool PowerPelletEnabled { get; set; }
    }
}