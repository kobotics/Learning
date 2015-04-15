// ------------------------------------------
// ICellElement.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using PS.Utilities.Serialization;

namespace Learning.Domain.Cells
{
    public interface ICellElement : IDisposable, ISensation
    {
        //Brush Brush { get; }
        ColorInfo Color { get; set; }
        bool ForceRepaint { get; set; }
        Cell Cell { get; set; }
        //Bitmap Image { get; }
        string ImagePath { get; set; }
        bool Visible { get; set; }
        bool HasSmell { get; set; }
        bool Walkable { get; set; }
        ICellElement Clone();
        bool Equals(ICellElement cellElement);
    }
}