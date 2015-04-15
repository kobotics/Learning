// ------------------------------------------
// EmotionalSTM.cs, Learning.Emotions
//
// Created by Pedro Sequeira, 2012/10/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System.IO;
using Learning.Domain.Agents;
using Learning.Domain.Memories;

namespace Learning.IMRL.Emotions.Domain.Memories
{
    public class EmotionalSTM : ShortTermMemory
    {
        public EmotionalSTM(IAgent agent) : base(agent)
        {
            this.AppraisalSet = new AppraisalSet();
            this.Clarity = new AppraisalDimension("Clarity");
            this.Mood = new AppraisalDimension("Mood");
        }

        public double Novelty
        {
            get { return this.AppraisalSet.Novelty.Value; }
        }

        public double Arousal
        {
            get { return this.AppraisalSet.Arousal.Value; }
        }

        public double Valence
        {
            get { return this.AppraisalSet.Valence.Value; }
        }

        public string EmotionLabel { get; set; }

        public bool WriteDimensionsLog { get; set; }

        public double Motivation
        {
            get { return this.AppraisalSet.GoalRelevance.Value; }
        }

        public double Control
        {
            get { return this.AppraisalSet.Control.Value; }
        }

        public AppraisalDimension Clarity { get; protected set; }

        public AppraisalDimension Mood { get; protected set; }

        public AppraisalSet AppraisalSet { get; protected set; }

        public override void PrintResults(string path)
        {
            base.PrintResults(path);
            //this.PrintDimensionsStats(path + "/STM");
        }

        public override void Dispose()
        {
            base.Dispose();
            this.AppraisalSet.Dispose();
            this.Mood.Dispose();
            this.Clarity.Dispose();
        }

        public override void Reset()
        {
            base.Reset();
            this.AppraisalSet = new AppraisalSet();
            this.Clarity = new AppraisalDimension("Clarity");
            this.Mood = new AppraisalDimension("Mood");
        }

        public override void Update()
        {
            base.Update();

            if (this.Agent.LogWriter != null)
                this.Agent.LogWriter.WriteLine("Emotion label: " + this.EmotionLabel);
        }

        protected void PrintDimensionsStats(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            //prints appraisal dimensions
            foreach (var dimension in this.AppraisalSet.Dimensions.Values)
                dimension.PrintStatisticsToCSV(string.Format("{0}/{1}.csv", path, dimension.Name));

            //prints other dimensions
            this.Clarity.PrintStatisticsToCSV(string.Format("{0}/{1}.csv", path, this.Clarity.Name));
            this.Mood.PrintStatisticsToCSV(string.Format("{0}/{1}.csv", path, this.Mood.Name));
        }
    }
}