// ------------------------------------------
// CellAgent.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using System.Drawing;
using System.Xml;
using Learning.Domain.Actions;
using Learning.Domain.Cells;
using Learning.Domain.Managers.Motivation;
using Learning.Domain.Managers.Perception;
using PS.Utilities.Serialization;

namespace Learning.Domain.Agents
{
    [Serializable]
    public abstract class CellAgent : SituatedAgent, ICellAgent
    {
        #region Fields

        protected const string MOVE_UP = "MoveUp";
        protected const string MOVE_DOWN = "MoveDown";
        protected const string MOVE_LEFT = "MoveLeft";
        protected const string MOVE_RIGHT = "MoveRight";

        protected Cell cell;

        #endregion

        #region Constructors

        protected CellAgent(string label)
        {
            this.CellElement = new CellElement {Walkable = true};
            this.IdToken = label;
        }

        protected CellAgent() : this(AGENT_LABEL)
        {
        }

        #endregion

        #region Properties

        protected CellElement CellElement { get; private set; }

        public PerceptionManager PerceptionManager { get; private set; }

        public new MotivationManager MotivationManager
        {
            get { return base.MotivationManager as MotivationManager; }
            protected set { base.MotivationManager = value; }
        }

        public string IdToken
        {
            get { return this.CellElement.IdToken; }
            set { this.CellElement.IdToken = value; }
        }

        public string Description
        {
            get { return this.CellElement.Description; }
            set { this.CellElement.Description = value; }
        }

        #endregion

        #region Public Methods

        public override void Update()
        {
            this.LogWriter.WriteLine("");

            this.ShortTermMemory.PreviousState = this.ShortTermMemory.CurrentState;

            var oldCell = this.Cell;

            this.BehaviorManager.Update();

            if ((oldCell != this.Cell) && (this.LogWriter != null))
                this.LogWriter.WriteLine(string.Format(@"{0} -> {1}: ", oldCell.CellLocation.IdToken,
                    this.Cell.CellLocation.IdToken));

            this.PerceptionManager.Update();
            this.ShortTermMemory.CurrentState = this.LongTermMemory.GetUpdatedCurrentState();

            this.MotivationManager.Update();
            var reward = this.MotivationManager.ExtrinsicReward.Value;

            this.ShortTermMemory.CurrentReward.Value = reward;

            this.LongTermMemory.Update();
            this.LearningManager.Update();
            this.ShortTermMemory.Update();
        }

        public override void Dispose()
        {
            base.Dispose();
            this.CellElement.Dispose();
            this.PerceptionManager.Dispose();
            this.MotivationManager.Dispose();
        }

        public override void PrintAll(string path, string imgFormat)
        {
            base.PrintAll(path, imgFormat);
            this.MotivationManager.PrintResults(path);
            this.PerceptionManager.PrintResults(path);
        }

        public override string ToString()
        {
            return this.IdToken;
        }

        #endregion

        #region Protected Methods

        protected override void CreateManagers()
        {
            base.CreateManagers();
            this.PerceptionManager = this.CreatePerceptionManager();
        }

        protected override void CreateActions()
        {
            this.Actions.Add(MOVE_UP, new MoveUp(MOVE_UP, this));
            this.Actions.Add(MOVE_DOWN, new MoveDown(MOVE_DOWN, this));
            this.Actions.Add(MOVE_LEFT, new MoveLeft(MOVE_LEFT, this));
            this.Actions.Add(MOVE_RIGHT, new MoveRight(MOVE_RIGHT, this));
        }

        protected virtual PerceptionManager CreatePerceptionManager()
        {
            return new NeighbourCellPerceptionManager(this);
        }

        protected override MotivationManager CreateMotivationManager()
        {
            return new EnvironmentMotivationManager(this);
        }

        #endregion

        #region Implementation of IXmlSerializable

        public void DeserializeXML(XmlElement element)
        {
            this.CellElement.DeserializeXML(element);
        }

        public void SerializeXML(XmlElement element)
        {
            this.CellElement.SerializeXML(element);
        }

        public virtual void InitElements()
        {
            this.CellElement.InitElements();
        }

        #endregion

        #region Implementation of ICellElement

        //public Brush Brush
        //{
        //    get { return this.CellElement.Brush; }
        //}

        public ColorInfo Color
        {
            get { return this.CellElement.Color; }
            set { this.CellElement.Color = value; }
        }

        public bool ForceRepaint { get; set; }

        public Cell Cell
        {
            get { return this.cell; }
            set
            {
                if (this.cell != null) this.cell.Elements.Remove(this);
                this.cell = value;

                if (value == null) return;

                this.cell.Elements.Add(this);
                if ((this.cell.Environment != null) && !this.cell.Environment.CellElements.ContainsKey(this.IdToken))
                    this.cell.Environment.CellElements.Add(this.IdToken, this);
            }
        }

        //public Bitmap Image
        //{
        //    get { return this.CellElement.Image; }
        //}

        public string ImagePath
        {
            get { return this.CellElement.ImagePath; }
            set { this.CellElement.ImagePath = value; }
        }

        public bool Visible
        {
            get { return this.CellElement.Visible; }
            set { this.CellElement.Visible = value; }
        }

        public bool HasSmell
        {
            get { return this.CellElement.HasSmell; }
            set { this.CellElement.HasSmell = value; }
        }

        public bool Walkable
        {
            get { return this.CellElement.Walkable; }
            set { this.CellElement.Walkable = value; }
        }

        public ICellElement Clone()
        {
            return this.CellElement.Clone();
        }

        public bool Equals(ICellElement cellElem)
        {
            return (cellElem is CellAgent) && (((CellAgent) cellElem).IdToken == this.IdToken);
        }

        #endregion

        #region ICellAgent Members

        public double Reward { get; set; }

        #endregion
    }
}