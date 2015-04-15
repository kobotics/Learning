// ------------------------------------------
// IGPChromosome.cs, Learning.IMRL.EC
// 
// Created by Pedro Sequeira, 2013/03/26
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System.Collections.Generic;
using Learning.EvolutionaryComputation;

namespace Learning.IMRL.EC.Chromosomes
{
    public interface IGPChromosome : IECChromosome
    {
        uint Length { get; }
        uint Depth { get; }
        HashSet<IGPChromosome> AllCombinations { get; }
    }
}