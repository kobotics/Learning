// ------------------------------------------
// CustomRenderPictureBox.cs, Learning.Forms
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Learning.Forms
{
    public class CustomRenderPictureBox : PictureBox
    {
        public SmoothingMode? SmoothingMode { get; set; }

        public CompositingQuality? CompositingQuality { get; set; }

        public InterpolationMode? InterpolationMode { get; set; }

        protected override void OnPaint(PaintEventArgs pe)
        {
            if (SmoothingMode.HasValue)
                pe.Graphics.SmoothingMode = SmoothingMode.Value;
            if (CompositingQuality.HasValue)
                pe.Graphics.CompositingQuality = CompositingQuality.Value;
            if (InterpolationMode.HasValue)
                pe.Graphics.InterpolationMode = InterpolationMode.Value;

            // this line is needed for .net to draw the contents.
            base.OnPaint(pe);
        }
    }
}