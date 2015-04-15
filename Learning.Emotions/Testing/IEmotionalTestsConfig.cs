// ------------------------------------------
// IEmotionalTestsConfig.cs, Learning.IMRL.Emotions
// 
// Created by Pedro Sequeira, 2015/04/02
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using Learning.Testing.Config;

namespace Learning.IMRL.Emotions.Testing
{
    public interface IEmotionalTestsConfig : ITestsConfig
    {
        uint MaxMoveActionsRequired { get; set; }
        uint NumStepsPerSeason { get; set; }
    }
}