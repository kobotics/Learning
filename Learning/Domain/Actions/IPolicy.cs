// ------------------------------------------
// IPolicy.cs, Learning
//
// Created by Pedro Sequeira, 2013/7/12
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
namespace Learning.Domain.Actions
{
    public interface IPolicy
    {
        uint NumActions { get; }
        double this[uint actionIdx] { get; set; }
        double GetActionProbability(uint actionID);
        void Normalize();
    }
}