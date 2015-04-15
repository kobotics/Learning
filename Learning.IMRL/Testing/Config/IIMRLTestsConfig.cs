// ------------------------------------------
// IIMRLTestsConfig.cs, Learning.IMRL
//
// Created by Pedro Sequeira, 2014/2/14
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using Learning.Testing.Config;
using Learning.Testing.Config.Parameters;
using PS.Utilities.Collections;

namespace Learning.IMRL.Testing.Config
{
    public interface IIMRLTestsConfig : ITestsConfig
    {
        string[] ParamIDNames { get; set; }
        char[] ParamIDLetters { get; set; }
        StepInterval<double>[] ParamsStepIntervals { get; set; }
        int NumParams { get; }
        bool AddSpecialTests { get; set; }
        bool IsValidParameter(IArrayParameter parameter);
    }
}