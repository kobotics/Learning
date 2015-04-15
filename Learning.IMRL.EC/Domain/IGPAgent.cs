// ------------------------------------------
// IGPAgent.cs, Learning.IMRL.EC
//
// Created by Pedro Sequeira, 2013/2/6
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using Learning.Domain.Agents;
using Learning.IMRL.EC.Chromosomes;

namespace Learning.IMRL.EC.Domain
{
    public interface IGPAgent: ICellAgent
    {
        IGPChromosome Chromosome { get; }
        new GPMotivationManager MotivationManager { get; }
    }
}