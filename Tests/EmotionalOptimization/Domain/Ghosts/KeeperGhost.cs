// ------------------------------------------
// KeeperGhost.cs, Learning.Tests.EmotionalOptimization
//
// Created by Pedro Sequeira, 2012/10/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using Learning.Domain.Cells;
using MathNet.Numerics.Random;

namespace Learning.Tests.EmotionalOptimization.Domain.Ghosts
{
    [Serializable]
    public class KeeperGhost : Ghost
    {
        private const int GHOST_KEEP_PROB = 5; // in 10

        [NonSerialized] protected static readonly Random Rand = new WH2006(true);
        protected readonly ICellElement otherElement;

        public KeeperGhost(ICellElement otherElement)
        {
            this.otherElement = otherElement;
        }

        protected override void UpdateMovement()
        {
            //checks for same cell, nothing to do
            if ((this.Cell == null) || (this.Cell.Environment == null)) return;

            //if greedy prob, advance to bottom, else advance to other element
            var prob = Rand.Next(10);
            if (prob < GHOST_KEEP_PROB)
                this.AdvanceToBottom();
            else
                this.Cell.Environment.MoveToElement(this, this.otherElement);
        }

        protected virtual void AdvanceToBottom()
        {
            var downCell = this.Cell.YCoord < this.Cell.Environment.Rows - 1
                               ? this.Cell.Environment.Cells[this.Cell.XCoord, this.Cell.YCoord + 1]
                               : null;
            if ((downCell != null) && this.Cell.Environment.IsWalkable(downCell))
                this.Cell = downCell;
        }
    }
}