// ------------------------------------------
// IControlable.cs, Learning.Forms
//
// Created by Pedro Sequeira, 2012/10/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System.Windows.Forms;

namespace Learning.Forms
{
    public interface IControlable
    {
        Control Control { get; }
    }
}