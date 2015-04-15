// ------------------------------------------
// IAgentFitnessFunction.cs, Learning
//
// Created by Pedro Sequeira, 2014/1/28
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using Learning.Domain.Agents;

namespace Learning.Testing.Simulations
{
    public interface IAgentFitnessFunction
    {
        void UpdateCurrentFitness(IAgent agent);
    }
}