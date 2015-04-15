// ------------------------------------------
// SmartGhost.cs, Learning.Tests.EmotionalOptimization
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
    public class SmartGhost : Ghost
    {
        private const int GHOST_SMART_PROB = 6; // in 10

        [NonSerialized] protected static readonly Random Rand = new WH2006(true);

        public ICellElement Agent { get; set; }

        protected override void UpdateMovement()
        {
            //checks for same cell, nothing to do
            if ((this.Cell == null) || this.Cell.Equals(this.Agent.Cell) || (this.Cell.Environment == null)) return;

            var prob = Rand.Next(10);

            if (prob < GHOST_SMART_PROB)
            {
                //if smart prob and agent is normal, advance to agent, else keep in same place
                if (this.State == GhostState.Normal)
                    this.Cell.Environment.MoveToElement(this, this.Agent);
            }
            else
            {
                //smart probability is inverse if ghost is weak, in which case run away from agent
                if (this.State == GhostState.Weak)
                    this.Cell.Environment.MoveFromElement(this, this.Agent);
            }
        }
    }
}