// ------------------------------------------
// ISimulationForm.cs, Learning.Forms
//
// Created by Pedro Sequeira, 2013/12/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using Learning.Domain.Agents;
using PS.Utilities;

namespace Learning.Forms.Simulation
{
    public interface ISimulationForm : IResetable
    {
        int CellSize { get; set; }
        int UpdateInterval { set; }
        void SetDebugMode(bool value);
        string Text { get; set; }
        IAgent ControlledAgent { set; }
        bool Controlable { set; get; }
        void Init();
        void PauseResumeSimulation();
        void StepSimulation();
        void ResetSimulation();
        void PrintEnvironment();
        void AdvanceSimulation(ulong toTimeStep);
        void Select();
    }
}