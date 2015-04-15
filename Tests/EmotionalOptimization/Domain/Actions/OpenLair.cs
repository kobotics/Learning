// ------------------------------------------
// OpenLair.cs, Learning.Tests.EmotionalOptimization
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using Learning.Domain.Actions;
using Learning.Domain.Agents;

namespace Learning.Tests.EmotionalOptimization.Domain.Actions
{
    public class OpenLair : CellAction
    {
        public OpenLair(string id, ICellAgent agent) : base(id, agent)
        {
        }
    }
}