// ------------------------------------------
// CellStateLTM.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using Learning.Domain.Agents;
using Learning.Domain.Cells;
using Learning.Domain.States;

namespace Learning.Domain.Memories
{
    [Serializable]
    public class CellStateLTM : LongTermMemory
    {
        public const char STATE_SEPARATOR = ':';

        protected Dictionary<Cell, CellState> cellStates = new Dictionary<Cell, CellState>();

        public CellStateLTM(CellAgent agent, ShortTermMemory shortTermMemory)
            : base(agent, shortTermMemory)
        {
        }

        public new CellAgent Agent
        {
            get { return base.Agent as CellAgent; }
        }

        public override void Reset()
        {
            base.Reset();
            this.cellStates.Clear();
        }

        public override string ToString(IState state)
        {
            if (!(state is CellState)) return string.Empty;
            var cell = ((CellState) state).Cell;
            return string.Format(CultureInfo.InvariantCulture,
                                 "{0}{1}{2}", cell.XCoord, STATE_SEPARATOR, cell.YCoord);
        }

        public override IState GetUpdatedCurrentState()
        {
            return this.GetState(this.Agent.Cell);
        }

        public override IState FromString(string stateStr)
        {
            if (string.IsNullOrWhiteSpace(stateStr)) return null;

            var coordStrs = stateStr.Split(STATE_SEPARATOR);
            if (coordStrs.Length != 2) return null;
            var xCoord = uint.Parse(coordStrs[0]);
            var yCoord = uint.Parse(coordStrs[1]);

            return this.GetState(this.Agent.Environment.Cells[xCoord, yCoord]);
        }

        protected virtual IState GetState(Cell cell)
        {
            if (cell == null) return null;

            //checks if state already exists, or create new one
            var cellState = this.cellStates.ContainsKey(cell)
                                ? this.cellStates[cell]
                                : new CellState(cell);

            //stores new/existing state in current state
            this.cellStates[cell] = cellState;
            return cellState;
        }
    }
}