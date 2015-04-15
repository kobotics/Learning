// ------------------------------------------
// SituatedAgent.cs, Learning
//
// Created by Pedro Sequeira, 2014/6/19
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using Learning.Domain.Environments;

namespace Learning.Domain.Agents
{
    [Serializable]
    public abstract class SituatedAgent : Agent, ISituatedAgent
    {
        public const string AGENT_LABEL = "agent";

        #region ISituatedAgent Members

        public IEnvironment Environment { get; set; }

        #endregion
    }
}