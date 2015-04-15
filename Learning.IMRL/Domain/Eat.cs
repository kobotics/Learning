// ------------------------------------------
// Eat.cs, Learning.IMRL
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using Learning.Domain.Actions;
using Learning.Domain.Agents;

namespace Learning.IMRL.Domain
{
    public class Eat : CellAction
    {
        public Eat(string id, ICellAgent agent) : base(id, agent)
        {
        }
    }
}