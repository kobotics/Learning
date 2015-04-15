// ------------------------------------------
// EmotionLabel.cs, Learning.Emotions
//
// Created by Pedro Sequeira, 2012/10/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
namespace Learning.IMRL.Emotions
{
    public class EmotionLabel : AppraisalSet
    {
        public EmotionLabel(string name)
        {
            this.Label = name;
        }

        public string Label { get; protected set; }
    }
}