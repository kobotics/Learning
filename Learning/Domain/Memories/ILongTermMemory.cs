// ------------------------------------------
// ILongTermMemory.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System.Collections.Generic;
using Learning.Domain.Actions;
using Learning.Domain.States;
using PS.Utilities.Math;

namespace Learning.Domain.Memories
{
    public interface ILongTermMemory : IMemory
    {
        uint NumStates { get; }
        uint NumActions { get; }
        uint MaxStates { get; set; }
        ulong TimeStep { get; }
        IShortTermMemory ShortTermMemory { get; }
        double MaxStateValue { get; }
        double MinStateValue { get; }
        StatisticalQuantity NumTasks { get; }
        IState GetUpdatedCurrentState();
        IState FromString(string stateStr);
        string ToString(IState state);
        IState GetState(uint stateID);
        IAction GetAction(uint actionID);
        uint GetMaxStateAction(uint stateID);
        double GetMaxStateActionValue(uint stateID);
        double GetMinStateActionValue(uint stateID);
        double GetStateActionValue(uint stateID, uint actionID);
        double GetStateActionReward(uint stateID, uint actionID);
        uint GetStateActionNumber(uint stateID);
        double GetStateCount(uint stateID);
        double GetStateActionCount(uint stateID, uint actionID);
        double GetStateActionTransictionCount(uint initialStateID, uint actionID, uint finalStateID);
        double GetStateActionTransitionProbability(uint initialStateID, uint actionID, uint finalStateID);
        int GetMaxStateActionTransition(uint initialStateID, uint actionID);
        ulong GetTimeStepsLastStateAction(uint stateID, uint actionID);
        ulong GetTimeStepsLastState(uint stateID);
        void UpdateStateActionValue(uint stateID, uint actionID, double value);
        void UpdateStateActionReward(uint stateID, uint actionID, double reward);
        IEnumerable<KeyValuePair<uint, uint>> GetStatePredecessors(uint stateID);
        uint GetRandomStateActionTransition(uint initialStateID, uint actionID);
        void ResetAllStateActionValues();
        void UpdateMinMaxValues();
        bool ReadStateStats(string path);
        bool ReadStateActionStats(string path);
        bool ReadStateActionTransitionStats(string path);
        void WriteStateStats(string path);
        void WriteStateActionStats(string path);
        void WriteStateActionTransitionStats(string path);
        void ReadAllStats(string path);
    }
}