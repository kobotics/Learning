// ------------------------------------------
// ICsvConvertible.cs, Learning
// 
// Created by Pedro Sequeira, 2015/03/31
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using PS.Utilities.Conversion;

namespace Learning.Testing
{
    public interface ICsvConvertible : IValueConvertible<string[]>
    {
        string[] Header { get; }
    }
}