// ------------------------------------------
// ITest.cs, Learning
//
// Created by Pedro Sequeira, 2013/12/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using PS.Utilities;

namespace Learning.Testing
{
    public interface ITest : IDisposable
    {
        long MemoryUsage { get; }
        TimeSpan TestSpeed { get; }
        string FilePath { get; }
        LogWriter LogWriter { get; }
        bool Run();
        void PrintResults();
    }
}