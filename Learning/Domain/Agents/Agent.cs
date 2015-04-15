// ------------------------------------------
// Agent.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using Learning.Domain.Actions;
using Learning.Domain.Managers.Behavior;
using Learning.Domain.Managers.Learning;
using Learning.Domain.Managers.Motivation;
using Learning.Domain.Memories;
using Learning.Testing.Config;
using Learning.Testing.Config.Parameters;
using Learning.Testing.Config.Scenarios;
using PS.Utilities;
using PS.Utilities.Math;
using PS.Utilities.Serialization;

namespace Learning.Domain.Agents
{
    [Serializable]
    public abstract class Agent : IAgent
    {
        #region Fields

        [NonSerialized] private LogWriter _logWriter;
        [NonSerialized] private StatisticsCollection _statisticsCollection;

        #endregion

        #region Constructors

        protected Agent()
        {
            this.StatisticsCollection = new StatisticsCollection();
            this.Actions = new Dictionary<string, IAction>();
        }

        #endregion

        #region Properties

        string IIdentifiableObject.IdToken { get; set; }

        string IIdentifiableObject.Description { get; set; }

        public Dictionary<string, IAction> Actions { get; protected set; }

        public BehaviorManager BehaviorManager { get; private set; }

        public LearningManager LearningManager { get; private set; }

        public LongTermMemory LongTermMemory { get; private set; }

        public ShortTermMemory ShortTermMemory { get; private set; }


        public LogWriter LogWriter
        {
            get { return this._logWriter; }
            set { this._logWriter = value; }
        }

        public StatisticsCollection StatisticsCollection
        {
            get { return this._statisticsCollection; }
            protected set { this._statisticsCollection = value; }
        }

        public IMotivationManager MotivationManager { get; protected set; }

        public StatisticalQuantity Fitness { get; set; }

        public IScenario Scenario { get; set; }

        public ITestParameters TestParameters { get; set; }

        #endregion

        #region Public Methods

        public abstract void Update();

        public virtual void Dispose()
        {
            this.BehaviorManager.Dispose();
            this.LearningManager.Dispose();
            this.LongTermMemory.Dispose();
            this.ShortTermMemory.Dispose();
            this.Actions.Clear();
            this.StatisticsCollection.Dispose();
        }

        public virtual void Reset()
        {
            this.LongTermMemory.Reset();
            this.ShortTermMemory.Reset();
            this.LearningManager.Reset();
            this.BehaviorManager.Reset();
            this.MotivationManager.Reset();

            var sampleSteps = this.StatisticsCollection.SampleSteps;
            var numSamples = this.StatisticsCollection.MaxNumSamples;
            this.StatisticsCollection = new StatisticsCollection
                                        {
                                            SampleSteps = sampleSteps,
                                            MaxNumSamples = numSamples,
                                        };
            this.AddStatisticalQuantities();
            this.StatisticsCollection.InitParameters();
        }

        public virtual void PrintAll(string path, string imgFormat)
        {
            //this.StatisticsCollection.PrintAllQuantities(path + "/OverallStatistics.xls");
            this.LongTermMemory.ImgFormat = imgFormat;
            this.LongTermMemory.PrintResults(path);
            this.ShortTermMemory.PrintResults(path);
            this.LearningManager.PrintResults(path);
            this.BehaviorManager.PrintResults(path);
        }

        public virtual void Init()
        {
            this.Actions.Clear();
            this.CreateActions();
            this.CreateMemories();
            this.CreateManagers();
            this.BehaviorManager.Init();
            this.InitStatisticsCollection();
        }

        public IAgent CreateNew()
        {
            return CloneUtil.CreateInstance(this);
        }

        #endregion

        #region Protected Methods

        protected virtual string MemoryBaseFilePath
        {
            get
            {
                return Path.GetFullPath(
                    string.Format("{0}{1}LTM", this.Scenario.TestsConfig.MemoryBaseFilePath,
                        Path.DirectorySeparatorChar));
            }
        }

        protected virtual void InitStatisticsCollection()
        {
            this.AddStatisticalQuantities();
            this.StatisticsCollection.SampleSteps = this.Scenario.TestsConfig.SampleSteps;
            this.StatisticsCollection.MaxNumSamples = this.Scenario.TestsConfig.NumSamples;
            this.StatisticsCollection.InitParameters();
        }

        protected abstract void CreateActions();

        protected virtual void AddStatisticalQuantities()
        {
            //adds statistical quantities to the log file
            //this.StatisticsCollection.Add("PredictionError", this.ShortTermMemory.PredictionErrorAbs);
            this.StatisticsCollection.Add("Reward", this.ShortTermMemory.CurrentReward);
            //this.StatisticsCollection.Add("LearningRate", this.LearningManager.LearningRate);
            //this.StatisticsCollection.Add("Discount", this.LearningManager.Discount);
            this.StatisticsCollection.Add("StateActionValue", this.ShortTermMemory.CurrentStateActionValue);
            this.StatisticsCollection.Add("ExtrinsicReward", this.MotivationManager.ExtrinsicReward);
            this.StatisticsCollection.Add("NumTasks", this.LongTermMemory.NumTasks);

            //adds actions statistics
            this.StatisticsCollection.AddRange(this.BehaviorManager.ActionStatistics);
        }

        protected virtual void CreateMemories()
        {
            this.ShortTermMemory = this.CreateShortTermMemory();
            this.LongTermMemory = this.CreateLongTermMemory();
            this.LongTermMemory.Reset();
            this.LongTermMemory.ReadAllStats(this.MemoryBaseFilePath);
        }

        protected virtual void CreateManagers()
        {
            this.BehaviorManager = this.CreateBehaviorManager();
            this.LearningManager = this.CreateLearningManager();
            this.MotivationManager = this.CreateMotivationManager();
        }

        protected abstract MotivationManager CreateMotivationManager();

        protected abstract LongTermMemory CreateLongTermMemory();

        protected virtual ShortTermMemory CreateShortTermMemory()
        {
            return new ShortTermMemory(this);
        }

        protected abstract BehaviorManager CreateBehaviorManager();

        protected virtual LearningManager CreateLearningManager()
        {
            return new QLearningManager(this, this.LongTermMemory);
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (!(obj is Agent)) return false;
            return ((IIdentifiableObject) obj).IdToken.Equals(((IIdentifiableObject) this).IdToken);
        }

        public override int GetHashCode()
        {
            return ((IIdentifiableObject) this).IdToken.GetHashCode();
        }
    }
}