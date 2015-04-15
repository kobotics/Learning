// ------------------------------------------
// Cell.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using Learning.Domain.Agents;
using Learning.Domain.Environments;
using PS.Utilities.Serialization;

namespace Learning.Domain.Cells
{
    [Serializable]
    public class Cell : XmlResource
    {
        #region Constrctors

        public Cell(IEnvironment environment)
            : base("cell")
        {
            this.ElementIDs = new List<string>();
            this.Environment = environment;
        }

        #endregion

        #region Constants

        private const string ID_TAG = "id";
        private const string CONTENTS_TAG = "contents";
        private const string YCOORD_TAG = "y-coord";
        private const string XCOORD_TAG = "x-coord";

        public const string DOWN_DIR_STR = "down";
        public const string LEFT_DIR_STR = "left";
        public const string RIGHT_DIR_STR = "right";
        public const string UP_DIR_STR = "up";

        #endregion

        #region Properties

        public Dictionary<string, Cell> NeighbourCells { get; protected set; }

        public bool Visible { get; set; }

        public Location CellLocation { get; private set; }

        public InfoCellElement StateValue { get; private set; }

        public IEnvironment Environment { get; set; }

        public int YCoord { get; set; }

        public int XCoord { get; set; }

        public HashSet<ICellElement> Elements { get; private set; }

        public List<string> ElementIDs { get; private set; }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return this.CellLocation.ToString();
        }

        public override void Dispose()
        {
            base.Dispose();
            foreach (var cellElement in this.Elements)
                if (!(cellElement is Agent))
                    cellElement.Dispose();
            this.Elements.Clear();
        }

        public override void InitElements()
        {
            this.Elements = new HashSet<ICellElement>();
            this.CellLocation = new Location {IdToken = this.XCoord + "," + this.YCoord, Cell = this, Visible = false};
            this.IdToken = this.CellLocation.IdToken;
            this.StateValue = new InfoCellElement
                              {
                                  IdToken = "StVal",
                                  Color = new ColorInfo(0, 100, 0),
                                  Cell = this,
                                  Visible = false
                              };
            this.Visible = true;
        }

        public void SetNeighbourCells()
        {
            this.NeighbourCells = new Dictionary<string, Cell>();

            //gets all cells in up, down, left, right directions, if possible
            if (this.YCoord > 0)
                this.NeighbourCells.Add(UP_DIR_STR, this.Environment.Cells[this.XCoord, this.YCoord - 1]);
            if (this.YCoord < this.Environment.Rows - 1)
                this.NeighbourCells.Add(DOWN_DIR_STR, this.Environment.Cells[this.XCoord, this.YCoord + 1]);
            if (this.XCoord > 0)
                this.NeighbourCells.Add(LEFT_DIR_STR, this.Environment.Cells[this.XCoord - 1, this.YCoord]);
            if (this.XCoord < this.Environment.Cols - 1)
                this.NeighbourCells.Add(RIGHT_DIR_STR, this.Environment.Cells[this.XCoord + 1, this.YCoord]);
        }

        public bool ContainsElement(string elementID)
        {
            return this.Elements.Any(cellElement => cellElement.IdToken.Equals(elementID));
        }

        public ICellElement GetElement(string elementID)
        {
            return this.Elements.FirstOrDefault(element => element.IdToken == elementID);
        }

        #endregion

        #region Serialization Methods

        public override void DeserializeXML(XmlElement element)
        {
            base.DeserializeXML(element);

            this.YCoord = int.Parse(element.GetAttribute(YCOORD_TAG), CultureInfo.InvariantCulture);
            this.XCoord = int.Parse(element.GetAttribute(XCOORD_TAG), CultureInfo.InvariantCulture);

            foreach (XmlElement childElement in element.SelectNodes(CONTENTS_TAG + "/" + ID_TAG))
            {
                var contentID = childElement.InnerXml.ToLower();
                if (contentID != "") this.ElementIDs.Add(contentID);
            }

            this.InitElements();
        }

        public override void SerializeXML(XmlElement element)
        {
            base.SerializeXML(element);

            element.SetAttribute(YCOORD_TAG, this.YCoord.ToString(CultureInfo.InvariantCulture));
            element.SetAttribute(XCOORD_TAG, this.XCoord.ToString(CultureInfo.InvariantCulture));

            var childElement = element.OwnerDocument.CreateElement(CONTENTS_TAG);
            element.AppendChild(childElement);
            foreach (var content in this.Elements)
            {
                if ((content is Location) || (content is InfoCellElement)) continue;

                var newChild = element.OwnerDocument.CreateElement(ID_TAG);
                newChild.InnerXml = content.IdToken;
                childElement.AppendChild(newChild);
            }
        }

        #endregion
    }
}