// ------------------------------------------
// EnvironmentControl.cs, Learning.Forms
//
// Created by Pedro Sequeira, 2013/12/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Learning.Domain.Cells;
using Learning.Domain.Environments;

namespace Learning.Forms.Simulation
{
    public class EnvironmentControl : IControlable, IDisposable
    {
        #region Constructors

        public EnvironmentControl(IEnvironment environment, Panel control, int cellSize, bool limitedUser, bool cellView)
        {
            this.CellControls = new Dictionary<Cell, CellControl>();
            this.CellView = cellView;
            this._control = control;
            this._limitedUser = limitedUser;
            //this._control.BackColor = Color.FromArgb(0, 0, 0);
            this._control.Paint += this.ControlOnPaint;
            this.Environment = environment;
            this.CellSize = cellSize;
            this.CreateCellControls();
        }

        #endregion

        #region Fields

        private readonly Panel _control;
        private readonly bool _limitedUser;
        private Graphics _previousGraphics;
        private bool _visibleCells = true;
        private readonly Dictionary<ICellElement, Bitmap> _elementBitmaps = new Dictionary<ICellElement, Bitmap>();
        private readonly Dictionary<ICellElement, string> _elementBitmapPaths = new Dictionary<ICellElement, string>();

        #endregion

        #region Properties

        public IEnvironment Environment { get; private set; }

        public int CellSize { get; protected set; }

        public bool CellView { get; protected set; }

        public bool VisibleCells
        {
            set
            {
                this._visibleCells = value;
                foreach (var cellControl in this.CellControls.Values)
                    cellControl.Cell.Visible = value;
            }
            get { return this._visibleCells; }
        }

        public Dictionary<Cell, CellControl> CellControls { get; private set; }

        public Control Control
        {
            get { return this._control; }
        }

        #endregion

        #region Public Methods

        public virtual void Dispose()
        {
            //this.Environment.Dispose();
            foreach (var cellControl in this.CellControls.Values)
            {
                cellControl.Dispose();
                this._control.Controls.Remove(cellControl);
            }
            this.CellControls.Clear();
            this._control.Paint -= this.ControlOnPaint;
            this._control.Controls.Clear();

            foreach (var bitmap in this._elementBitmaps.Values)
                bitmap.Dispose();
            this._elementBitmaps.Clear();
            this._elementBitmapPaths.Clear();
        }

        public Bitmap GetElementBitmap(ICellElement element)
        {
            //check new image (not loaded or path changed)
            if (!this._elementBitmapPaths.ContainsKey(element) ||
                element.ImagePath != this._elementBitmapPaths[element])
            {
                Bitmap bitmap = null;
                if (File.Exists(element.ImagePath))
                    bitmap = (Bitmap) Image.FromFile(element.ImagePath);
                if (this._elementBitmaps.ContainsKey(element) && (this._elementBitmaps[element] != null))
                    this._elementBitmaps[element].Dispose();

                this._elementBitmaps[element] = bitmap;
                this._elementBitmapPaths[element] = element.ImagePath;
            }
            return this._elementBitmaps[element];
        }

        public void SaveToImage(string path)
        {
            if (this._previousGraphics == null) return;

            var bitmap = new Bitmap(this.Control.Width, this.Control.Height);
            this.Control.DrawToBitmap(bitmap, new Rectangle(0, 0, this.Control.Width, this.Control.Height));
            bitmap.Save(path, ImageFormat.Png);
        }

        public void UnPrintBlack()
        {
            foreach (var cellControl in this.CellControls.Values)
                cellControl.Cell.Visible = true;
        }

        #endregion

        #region Protected Methods

        protected virtual void CreateCellControls()
        {
            //creates cell control for each cell
            foreach (var cell in this.Environment.Cells)
            {
                cell.Visible = !this._limitedUser;
                cell.CellLocation.Visible = this._limitedUser && this.CellView;
                this.CellControls.Add(cell, this._limitedUser
                    ? new LimitedUserCellControl(cell, this, this.CellSize, this.CellView)
                    : new CellControl(cell, this, this.CellSize));
            }
        }

        protected void ControlOnPaint(object sender, PaintEventArgs e)
        {
            //paints cells
            this._previousGraphics = e.Graphics;
            foreach (var cellControl in this.CellControls.Values)
                if (cellControl.NeedsRepaint() || this.Environment.DebugMode)
                    cellControl.Invalidate(true);
        }

        #endregion
    }
}