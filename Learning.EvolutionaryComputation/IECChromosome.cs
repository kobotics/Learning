// ------------------------------------------
// IECChromosome.cs, Learning.EvolutionaryComputation
// 
// Created by Pedro Sequeira, 2013/02/06
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using AForge.Genetic;
using Learning.Testing.Config.Parameters;

namespace Learning.EvolutionaryComputation
{
    public interface IECChromosome : IChromosome, ITestParameters, IComparable<IECChromosome>
    {
        ECPopulation Population { get; set; }
        double CrossoverBalancer { get; set; }
        double MutationBalancer { get; set; }
        new double Fitness { get; set; }
    }
}