// ------------------------------------------
// Ghost.cs, Learning.Tests.EmotionalOptimization
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using Learning.Domain.Cells;
using PS.Utilities;

namespace Learning.Tests.EmotionalOptimization.Domain.Ghosts
{
    public enum GhostState
    {
        Normal,
        Weak
    }

    [Serializable]
    public abstract class Ghost : CellElement, IUpdatable
    {
        protected GhostState state;

        public GhostState State
        {
            get { return this.state; }
            set
            {
                this.state = value;
                this.ForceRepaint = true;
            }
        }

        public string WeakenedGhostImagePath { get; set; }

        public override string ImagePath
        {
            get { return this.State == GhostState.Normal ? base.ImagePath : this.WeakenedGhostImagePath; }
        }

        #region IUpdatable Members

        public virtual void Update()
        {
            this.UpdateMovement();
        }

        #endregion

        protected abstract void UpdateMovement();
    }
}