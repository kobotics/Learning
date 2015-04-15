// ------------------------------------------
// CellControl.cs, Learning.Forms
//
// Created by Pedro Sequeira, 2013/12/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using AForge.Imaging.Filters;
using Learning.Domain.Agents;
using Learning.Domain.Cells;
using Image = AForge.Imaging.Image;

namespace Learning.Forms.Simulation
{
    public class CellControl : Panel
    {
        #region Constructors

        public CellControl(Cell cell, EnvironmentControl environmentControl, int cellSize)
        {
            this.Cell = cell;
            this._cellSize = cellSize;
            this._environmentControl = environmentControl;
            this.InitElements();
        }

        #endregion

        #region Fields

        private readonly int _cellSize;
        private readonly EnvironmentControl _environmentControl;
        private bool _forceRepaint = true;
        private HashSet<ICellElement> _previousElements;

        #endregion

        #region Properties

        public Cell Cell { get; private set; }

        public bool ForceRepaint
        {
            set { this._forceRepaint = value; }
        }

        #endregion

        #region Public Methods

        public virtual void InitElements()
        {
            this.BackColor = this._environmentControl.Environment.Color;
            this.ForeColor = Color.FromArgb(0, 0, 0);
            this.Size = new Size(_cellSize, _cellSize);
            //this.Location = new Point((_cellSize + 1) * this.Cell.XCoord, (_cellSize + 1) * this.Cell.YCoord);
            this.Location = new Point(_cellSize*this.Cell.XCoord, _cellSize*this.Cell.YCoord);
            this.BringToFront();
            this.Parent = this._environmentControl.Control;
            this._environmentControl.Control.Controls.Add(this);
        }

        protected override void Dispose(bool disposing)
        {
            //this._environmentControl.Control.Controls.Remove(this);
            this.Visible = false;
            base.Dispose(disposing);
        }

        public bool NeedsRepaint()
        {
            var forceRepaint = this._forceRepaint;
            this.ForceRepaint = false;

            //checks conditions for repaint
            forceRepaint = forceRepaint ||
                           (this._previousElements == null) ||
                           (this.Cell.Elements.Count != this._previousElements.Count) ||
                           this.Cell.Elements.Any(
                               element => element.ForceRepaint || !this._previousElements.Contains(element));

            this._previousElements = new HashSet<ICellElement>(this.Cell.Elements);

            return forceRepaint;
        }

        #endregion

        #region Protected Methods

        protected override void OnPaint(PaintEventArgs e)
        {
            //don't display cell unless visible
            if (this.Cell.Visible)
            {
                this.PaintContents(e);
            }
            else
            {
                this.BackColor = Color.FromArgb(0, 0, 0);
                this.ForceRepaint = true;
            }

            base.OnPaint(e);
        }

        protected void PaintContents(PaintEventArgs e)
        {
            this.BackColor = this._environmentControl.Environment.Color;
            var elements = new List<ICellElement>(this.Cell.Elements);

            var imageList = new List<Bitmap>();

            for (var i = 0; i < elements.Count; i++)
            {
                var cellElement = elements[i];
                cellElement.ForceRepaint = false;

                //if not visible, don't draw content
                if (!cellElement.Visible) continue;

                //adds image to list or draws text
                var bitmap = this._environmentControl.GetElementBitmap(cellElement);
                if (bitmap != null)
                    //imageList.Add(cellElement.Image);
                    e.Graphics.DrawImage(bitmap, new Rectangle(new Point(0, 0), this.Size));
                else
                //e.Graphics.DrawString(cellElement.IdToken, new Font("Arial", 7), cellElement.Brush, 0.3f, 12*i);
                    e.Graphics.FillRectangle(new SolidBrush(cellElement.Color),
                        new Rectangle(new Point(0, 0), this.Size));

                this.DrawFinishedTask(cellElement, e.Graphics);
            }

            this.DrawImages(imageList, e.Graphics);
        }

        protected void DrawImages(List<Bitmap> imageList, Graphics graphics)
        {
            if (imageList.Count == 0) return;

            var mergedImage = Image.Clone(imageList[0], PixelFormat.Format24bppRgb);
            var filter = new Morph();

            //merges all images in the list
            for (var i = 1; i < imageList.Count; i++)
            {
                //sets filter parameters
                filter.OverlayImage = mergedImage;
                filter.SourcePercent = 1.0/(i + 1.0);

                //applies morph (merge) filter
                var newImage = Image.Clone(imageList[i], PixelFormat.Format24bppRgb);
                //var newMergedImage = filter.Apply(newImage);

                //disposes of images created
                mergedImage.Dispose();
                newImage.Dispose();
                //mergedImage = newMergedImage;
            }

            //draws resulting image in graphics
            graphics.DrawImage(mergedImage, new Rectangle(new Point(0, 0), this.Size));
            mergedImage.Dispose();
        }

        protected void DrawFinishedTask(ICellElement cellElement, Graphics graphics)
        {
            if (!(cellElement is CellAgent)) return;

            //checks if agent finished task
            var agent = (CellAgent) cellElement;
            var agentFinishedTask = agent.Environment.AgentFinishedTask(
                agent, agent.ShortTermMemory.PreviousState, agent.ShortTermMemory.CurrentAction);
            if (agentFinishedTask)
                graphics.DrawRectangle(new Pen(Color.Red, 10), new Rectangle(new Point(0, 0), this.Size));
        }

        #endregion
    }
}