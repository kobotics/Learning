// ------------------------------------------
// EmotionsManager.cs, Learning.Emotions
// 
// Created by Pedro Sequeira, 2012/10/10
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Learning.Domain.Agents;
using Learning.Domain.Managers;
using Learning.IMRL.Emotions.Domain.Memories;
using PS.Utilities.Math;

namespace Learning.IMRL.Emotions.Domain.Managers
{
    [Serializable]
    public abstract class EmotionsManager : Manager
    {
        protected EmotionalSTM emotionalSTM;

        protected EmotionsManager(IAgent agent)
            : base(agent)
        {
            this.EmotionLabelsCount = new Dictionary<string, StatisticalQuantity>();
            this.EmotionLabels = new Dictionary<string, EmotionLabel>();
            //this.ReadEmotionLabelsConfig();
            //this.InitEmotionLabelCounts();
            this.NoveltyDecay = 1.001f;
            this.emotionalSTM = this.Agent == null
                ? null
                : ((EmotionalSTM) this.Agent.ShortTermMemory);
        }

        public double NoveltyDecay { get; set; }

        protected AppraisalSet AppraisalSet
        {
            get { return this.emotionalSTM.AppraisalSet; }
        }

        protected string EmotionLabel
        {
            get { return this.emotionalSTM.EmotionLabel; }
            set { this.emotionalSTM.EmotionLabel = value; }
        }

        protected StatisticalQuantity PredictionError
        {
            get { return this.Agent.ShortTermMemory.PredictionErrorAbs; }
        }

        public Dictionary<string, EmotionLabel> EmotionLabels { get; private set; }
        public Dictionary<string, StatisticalQuantity> EmotionLabelsCount { get; private set; }

        public override void Update()
        {
            var stm = this.emotionalSTM;
            if (stm.PreviousState == null) return;
            this.UpdateAppraisal(stm.PreviousState.ID, stm.CurrentAction.ID, stm.CurrentState.ID);
        }

        public override void Dispose()
        {
            this.EmotionLabels.Clear();
            this.EmotionLabelsCount.Clear();
        }

        public override void Reset()
        {
            this.EmotionLabelsCount.Clear();
            this.InitEmotionLabelCounts();
        }

        public override void PrintResults(string path)
        {
            path += "/Emotions";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            StatisticsUtil.PrintAllQuantitiesCountToCSV(path + "/EmotionsCount.xls", this.EmotionLabelsCount);
            StatisticsUtil.PrintAllQuantitiesToCSV(path + "/EmotionLabel.csv", this.EmotionLabelsCount);
            this.PrintEmotionDimensions(path + "/EmotionDimensions.csv");
        }

        protected void InitEmotionLabelCounts()
        {
            if (this.Agent == null) return;

            //creates statistical quantities for emotion labels count
            foreach (var emotionLabel in this.EmotionLabels.Keys)
            {
                this.EmotionLabelsCount[emotionLabel] =
                    new StatisticalQuantity
                    {
                        SampleSteps = this.Agent.StatisticsCollection.SampleSteps,
                        MaxNumSamples = this.Agent.StatisticsCollection.MaxNumSamples
                    };
            }
        }

        protected void ReadEmotionLabelsConfig()
        {
            var fileName = Path.GetFullPath("../../../../bin/config/emotions.csv");
            if (!File.Exists(fileName)) return;

            var sr = new StreamReader(fileName);
            var line = sr.ReadLine();
            if (line == null) return;

            var dimensionIds = line.Split(';');
            if (dimensionIds.Length < 2) return;

            while ((line = sr.ReadLine()) != null)
            {
                if (line.StartsWith("//")) continue;

                var parameters = line.Split(';');
                if (parameters.Length != dimensionIds.Length) continue;

                var name = parameters[0];
                var emotionLabel = new EmotionLabel(name);
                for (var i = 1; i < dimensionIds.Length; i++)
                {
                    var dimensionId = dimensionIds[i];
                    var dimensionValue = Convert.ToSingle(parameters[i], CultureInfo.InvariantCulture);
                    emotionLabel.Dimensions[dimensionId].Value = dimensionValue;
                }
                this.EmotionLabels.Add(name, emotionLabel);
            }
        }

        public abstract double GetValence(uint prevState, uint action, uint newState);
        public abstract double GetNovelty(uint prevState, uint action, uint newState);
        public abstract double GetGoalRelevance(uint prevState, uint action, uint newState);
        public abstract double GetControl(uint prevState, uint action, uint newState);
        public abstract double GetArousal(uint prevState, uint action, uint newState);
        public abstract double GetMood(uint prevState, uint action, uint newState);

        protected void PrintEmotionDimensions(string filePath)
        {
            if (File.Exists(filePath)) File.Delete(filePath);

            var allDimensions = new Dictionary<string, StatisticalQuantity>();
            allDimensions.Add(this.emotionalSTM.Mood.Name, this.emotionalSTM.Mood);
            allDimensions.Add(this.emotionalSTM.Clarity.Name, this.emotionalSTM.Clarity);
            foreach (var dimension in this.emotionalSTM.AppraisalSet.Dimensions.Values)
                allDimensions.Add(dimension.Name, dimension);

            StatisticsUtil.PrintAllQuantitiesToCSV(filePath, allDimensions);
        }

        protected void PrintEmotionLabelsCountToCSV(string filePath)
        {
            filePath += "/EmotionsCount.csv";

            if (File.Exists(filePath))
                File.Delete(filePath);

            var sw = new StreamWriter(filePath);
            sw.WriteLine("Label; Count");
            foreach (var emotionCount in this.EmotionLabelsCount)
                sw.WriteLine("{0};{1}", emotionCount.Key, emotionCount.Value.Sum);
            sw.Close();
        }

        protected virtual void UpdateAppraisal(uint prevState, uint action, uint newState)
        {
            //updates dimensions
            this.AppraisalSet.Arousal.Value = this.GetArousal(prevState, action, newState);
            this.AppraisalSet.GoalRelevance.Value = this.GetGoalRelevance(prevState, action, newState);
            this.AppraisalSet.Novelty.Value = this.GetNovelty(prevState, action, newState);
            this.AppraisalSet.Control.Value = this.GetControl(prevState, action, newState);
            this.AppraisalSet.Valence.Value = this.GetValence(prevState, action, newState);

            ////updates label
            //this.UpdateEmotionLabel();
        }

        protected virtual void UpdateEmotionLabel()
        {
            this.EmotionLabel = "ND";

            //finds the label corresponding to the minimal distance
            var minDistance = double.MaxValue;
            foreach (var emotionLabel in this.EmotionLabels.Values)
            {
                var distance = this.AppraisalSet.DifferenceTo(emotionLabel);
                if (distance >= minDistance) continue;
                minDistance = distance;
                this.EmotionLabel = emotionLabel.Label;
            }

            //checks label
            if (this.EmotionLabel == "ND") return;

            //updates label count (1 for current emotion label, 0 for the rest)
            foreach (var emotionLabelCount in this.EmotionLabelsCount)
                emotionLabelCount.Value.Value = emotionLabelCount.Key.Equals(this.EmotionLabel) ? 1 : 0;
        }
    }
}