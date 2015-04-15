// ------------------------------------------
// ShortTermMemory.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using System.IO;
using Learning.Domain.Actions;
using Learning.Domain.Agents;
using Learning.Domain.Managers;
using Learning.Domain.States;
using PS.Utilities.Math;

namespace Learning.Domain.Memories
{
    [Serializable]
    public class ShortTermMemory : Manager, IShortTermMemory
    {
        private double _predictionError;

        public ShortTermMemory(IAgent agent) : base(agent)
        {
            this.CurrentReward = new StatisticalQuantity();
            this.PredictionErrorAbs = new StatisticalQuantity();
            this.CurrentStateActionValue = new StatisticalQuantity();
        }

        public StatisticalQuantity CurrentStateActionValue { get; protected set; }

        public StatisticalQuantity PredictionErrorAbs { get; protected set; }

        #region IShortTermMemory Members

        public IState CurrentState { get; set; }

        public IState PreviousState { get; set; }

        public IAction CurrentAction { get; set; }

        public StatisticalQuantity CurrentReward { get; protected set; }

        public double PredictionError
        {
            get { return this._predictionError; }
            set
            {
                this._predictionError = value;
                this.PredictionErrorAbs.Value = System.Math.Abs(value);
            }
        }

        public override void Update()
        {
            //updates current action value in short memory
            if ((this.PreviousState != null) && (this.CurrentAction != null))
                this.CurrentStateActionValue.Value =
                    this.Agent.LongTermMemory.GetStateActionValue(this.PreviousState.ID, this.CurrentAction.ID);

            if (this.Agent.LogWriter == null) return;

            this.Agent.LogWriter.WriteLine(
                string.Format(@"Total reward after this action: {0}", this.CurrentReward.Value));
            this.Agent.LogWriter.WriteLine(@"Current State: " + this.CurrentState);
        }

        public override void Reset()
        {
            this.CurrentAction = null;
            this.CurrentState = null;
            this.PreviousState = null;
            this.CurrentReward = new StatisticalQuantity();
            this.PredictionErrorAbs = new StatisticalQuantity();
            this.CurrentStateActionValue = new StatisticalQuantity();
        }

        public virtual void InitElements()
        {
        }

        public override void Dispose()
        {
            this.CurrentReward.Dispose();
            this.CurrentStateActionValue.Dispose();
            this.PredictionErrorAbs.Dispose();
        }

        #endregion

        public override void PrintResults(string path)
        {
            path += "/STM";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            this.CurrentReward.PrintStatisticsToCSV(path + "/Reward.csv");
            this.PredictionErrorAbs.PrintStatisticsToCSV(path + "/PredictionError.csv");
            this.CurrentStateActionValue.PrintStatisticsToCSV(path + "/CurrentStateActionValue.csv");
        }
    }
}