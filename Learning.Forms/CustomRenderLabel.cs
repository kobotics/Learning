// ------------------------------------------
// CustomRenderLabel.cs, Learning.Forms
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System.Drawing.Text;
using System.Windows.Forms;

namespace Learning.Forms
{
    public class CustomRenderLabel : Label
    {
        public CustomRenderLabel()
        {
            this.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        }

        public TextRenderingHint TextRenderingHint { get; set; }

        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.TextRenderingHint = TextRenderingHint;
            base.OnPaint(pe);
        }
    }
}