// ------------------------------------------
// ArrayParamMotivationManager.cs, Learning.IMRL
// 
// Created by Pedro Sequeira, 2013/07/11
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using Learning.Domain.Agents;
using Learning.Testing.Config.Parameters;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Learning.IMRL.Domain.Managers.Motivation
{
    public abstract class ArrayParamMotivationManager : IntrinsicMotivationManager
    {
        protected ArrayParamMotivationManager(ICellAgent agent) : base(agent)
        {
            this.RewardParameters = ((IArrayParameter) this.Agent.TestParameters).Array;
        }

        public DenseVector RewardParameters { get; set; }

        public override double GetIntrinsicReward(uint prevState, uint action, uint nextState)
        {
            //gets the reward given by the weighted sum of all features
            return this.RewardParameters*this.GetRewardFeatures(prevState, action, nextState);
        }
    }
}