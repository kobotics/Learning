// ------------------------------------------
// IEnvironment.cs, Learning
//
// Created by Pedro Sequeira, 2013/12/4
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using System.Collections.Generic;
using Learning.Domain.Actions;
using Learning.Domain.Agents;
using Learning.Domain.Cells;
using Learning.Domain.States;
using Learning.Testing.Config;
using Learning.Testing.Config.Scenarios;
using PS.Utilities;
using PS.Utilities.Serialization;

namespace Learning.Domain.Environments
{
    public interface IEnvironment : IDisposable, IUpdatable, IXmlSerializable, IInitializable
    {
        bool DebugMode { get; set; }
        int WaterLevel { get; }
        int previousWaterLevel { get; }
        uint Rows { get; }
        uint Cols { get; }
        Cell[,] Cells { get; }
        ColorInfo Color { get; }
        Dictionary<string, ICellElement> CellElements { get; }
        List<Cell> TargetCells { get; }
        IScenario Scenario { get; set; }
        void Reset();
        double GetAgentReward(IAgent agent, IState state, IAction action);
        bool AgentFinishedTask(IAgent agent, IState state, IAction action);
        double GetDistanceBetweenCells(Cell source, Cell target);
        IList<Cell> GetShortPathBetweenCells(Cell source, Cell target);
        Cell GetNextCellInShortPath(Cell source, Cell target);
        bool IsWalkable(Cell cell);
        bool IsWalkable(int x, int y);
        bool DetectInCorridors(int startPosX, int startPosY, string elementID);
        bool DetectInLeftCorridor(int startPosX, int startPosY, string elementID);
        bool DetectInRightCorridor(int startPosX, int startPosY, string elementID);
        bool DetectInUpCorridor(int startPosX, int startPosY, string elementID);
        bool DetectInDownCorridor(int startPosX, int startPosY, string elementID);
        IList<Cell> GetUpCells(int startPosX, int startPosY);
        IList<Cell> GetDownCells(int startPosX, int startPosY);
        IList<Cell> GetLeftCells(int startPosX, int startPosY);
        IList<Cell> GetRightCells(int startPosX, int startPosY);
        bool DetectInCells(IList<Cell> cells, string elementID);
        bool DetectInCell(int x, int y, string elementID);
        bool DetectInCell(Cell cell, string elementID);
        void MoveRandomly(ICellElement element);
        void MoveToElement(ICellElement element, ICellElement toElement);
        void MoveFromElement(ICellElement element, ICellElement fromElement);
        double GetCurrentDistanceToTargetCell(ICellAgent agent);
        double GetDistanceToTargetCell(Cell source);
        IList<Cell> GetNeighbourCells(Cell source);
        Cell GetFarthestCell(ICellElement fromElement);
        Cell GetRandomCell();
        Cell GetRandomCell(IEnumerable<Cell> forbiddenCells, IEnumerable<Cell> possibleCells);
        void SaveToXmlFile(string xmlFileName);
        void LoadFromXmlFile(string xmlFileName);
        IEnvironment CreateNew();
        void CreateCells(uint cols, uint rows, string environmentConfigFile);
        Cell GetFarthestCell(Cell fromElement);
        Cell GetNextCellToFartherFromCell(Cell startCell, Cell fromCell);
        Cell GetMedianWalkableCell(IEnumerable<ICellElement> elements);
        Cell GetClosestWalkableCell(int xCoord, int yCoord);
        Cell GetCell(int xCoord, int yCoord);
        bool IsValidCoord(int xCoord, int yCoord);
        bool IsValidXCoord(int xCoord);
        bool IsValidYCoord(int yCoord);
        IList<Cell> GetCornerCells();
    }
}