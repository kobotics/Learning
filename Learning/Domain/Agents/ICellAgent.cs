// ------------------------------------------
// ICellAgent.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using Learning.Domain.Cells;
using Learning.Domain.Managers.Perception;

namespace Learning.Domain.Agents
{
    public interface ICellAgent : ICellElement, ISituatedAgent
    {
        PerceptionManager PerceptionManager { get; }
    }
}