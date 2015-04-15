// ------------------------------------------
// ITestParameters.cs, Learning
// 
// Created by Pedro Sequeira, 2015/03/30
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;

namespace Learning.Testing.Config.Parameters
{
    /// <summary>
    ///     Parameterization for a single test instance. Usually defines the agent's parameters.
    /// </summary>
    public interface ITestParameters : ICloneable, IEquatable<ITestParameters>, ICsvConvertible
    {
    }
}