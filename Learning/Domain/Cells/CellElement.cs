// ------------------------------------------
// CellElement.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using System.Globalization;
using System.Xml;
using PS.Utilities.Serialization;

namespace Learning.Domain.Cells
{
    [Serializable]
    public class CellElement : Sensation, ICellElement
    {
        #region Fields

        //private Bitmap _image;
        protected Cell cell;

        #endregion

        #region Constructor

        public CellElement()
        {
            this.Description = "";
            this.Visible = true;
            this.Walkable = true;
            this.Color = new ColorInfo(0, 0, 0);
        }

        #endregion

        #region Constants

        private const string IMAGE_PATH_TAG = "image-path";
        private const string VISIBLE_TAG = "visible";
        private const string SMELL_TAG = "smell";
        private const string WALKABLE_TAG = "walkable";
        private const string COLOR_TAG = "color";

        #endregion

        #region Properties

        public bool ForceRepaint { get; set; }

        public ColorInfo Color { get; set; }

        public virtual Cell Cell
        {
            get { return this.cell; }
            set
            {
                if (Equals(value, this.cell)) return;

                if (this.cell != null) this.cell.Elements.Remove(this);
                this.cell = value;

                if (value == null) return;

                this.cell.Elements.Add(this);
                if ((this.cell.Environment != null) && !(this is Location) && !(this is InfoCellElement) &&
                    !this.cell.Environment.CellElements.ContainsKey(this.IdToken))
                    this.cell.Environment.CellElements.Add(this.IdToken, this);
            }
        }

        public virtual string ImagePath { get; set; }

        public bool Visible { get; set; }

        public bool Walkable { get; set; }

        public bool HasSmell { get; set; }

        #endregion

        #region Public Methods

        public virtual ICellElement Clone()
        {
            return new CellElement
                   {
                       Color = this.Color,
                       ImagePath = this.ImagePath,
                       Reward = this.Reward,
                       IdToken = this.IdToken,
                       Visible = this.Visible,
                       Walkable = this.Walkable,
                       HasSmell = this.HasSmell,
                       Description = this.Description
                   };
        }

        public bool Equals(ICellElement cellElement)
        {
            return cellElement is CellElement &&
                   cellElement.IdToken == this.IdToken &&
                   cellElement.Color.Equals(this.Color) &&
                   cellElement.ImagePath == this.ImagePath &&
                   ((CellElement) cellElement).Reward.Equals(this.Reward) &&
                   cellElement.Visible == this.Visible &&
                   cellElement.Walkable == this.Walkable &&
                   cellElement.HasSmell == this.HasSmell;
        }

        public override void Dispose()
        {
            base.Dispose();
            //if (this.Image != null) this.Image.Dispose();
        }

        public override string ToString()
        {
            return this.IdToken;
        }

        #region Serialization Methods

        public override void DeserializeXML(XmlElement element)
        {
            base.DeserializeXML(element);

            bool hasSmell, visible, walkable;
            bool.TryParse(element.GetAttribute(VISIBLE_TAG), out visible);
            this.Visible = visible;
            bool.TryParse(element.GetAttribute(SMELL_TAG), out hasSmell);
            this.HasSmell = hasSmell;
            bool.TryParse(element.GetAttribute(WALKABLE_TAG), out walkable);
            this.Walkable = walkable;
            this.ImagePath = element.GetAttribute(IMAGE_PATH_TAG);

            var childElement = (XmlElement) element.SelectSingleNode(COLOR_TAG);
            if (childElement != null) this.Color.DeserializeXML(childElement);
        }

        public override void SerializeXML(XmlElement element)
        {
            base.SerializeXML(element);

            element.SetAttribute(VISIBLE_TAG, this.Visible.ToString(CultureInfo.InvariantCulture));
            element.SetAttribute(WALKABLE_TAG, this.Walkable.ToString(CultureInfo.InvariantCulture));
            element.SetAttribute(SMELL_TAG, this.HasSmell.ToString(CultureInfo.InvariantCulture));
            element.SetAttribute(IMAGE_PATH_TAG, this.ImagePath);

            var childElement = element.OwnerDocument.CreateElement(COLOR_TAG);
            this.Color.SerializeXML(childElement);
            element.AppendChild(childElement);
        }

        #endregion

        #endregion
    }
}