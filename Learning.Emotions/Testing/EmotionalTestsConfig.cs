// ------------------------------------------
// EmotionalTestsConfig.cs, Learning.Emotions
// 
// Created by Pedro Sequeira, 2013/05/22
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using Learning.IMRL.Testing.Config;

namespace Learning.IMRL.Emotions.Testing
{
    [Serializable]
    public abstract class EmotionalTestsConfig : IMRLTestsConfig, IEmotionalTestsConfig
    {
        public uint MaxMoveActionsRequired { get; set; }
        public uint NumStepsPerSeason { get; set; }

        public override void SetDefaultConstants()
        {
            base.SetDefaultConstants();

            this.ParamIDNames = new[] {"Novelty", "Goal relevance", "Control", "Valence", "Fit. Param"};
            this.ParamIDLetters = new[] {'n', 'g', 'c', 'v', 'f'};
        }
    }
}