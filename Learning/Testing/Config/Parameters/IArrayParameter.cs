// ------------------------------------------
// IArrayParameter.cs, Learning
//
// Created by Pedro Sequeira, 2014/6/19
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System.Collections.Generic;
using PS.Utilities.Collections;

namespace Learning.Testing.Config.Parameters
{
    public interface IArrayParameter : ITestParameters, IEnumerable<double>
    {
        double this[int paramIdx] { get; set; }
        uint NumDecimalPlaces { get; set; }
        uint Length { get; }
        double[] Array { get; }
        double AbsoulteSum { get; }
        double Sum { get; }
        StepInterval<double>[] Domains { get; }
        void NormalizeVector();
        void NormalizeSum();
        void NormalizeUnitSum();
        void Round();
        void SetMidDomainValues();
    }
}