// ------------------------------------------
// RabbitLair.cs, Learning.Tests.EmotionalOptimization
//
// Created by Pedro Sequeira, 2012/10/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using System.Drawing;
using Learning.Domain.Cells;

namespace Learning.Tests.EmotionalOptimization.Domain
{
    public enum LairState
    {
        Closed,
        Empty,
        Rabbit
    }

    [Serializable]
    public class RabbitLair : CellElement
    {
        public LairState State { get; set; }
        public string ClosedLairImagePath { get; set; }
        public string OpenedLairImagePath { get; set; }

        public override string ImagePath
        {
            get
            {
                return this.State == LairState.Rabbit
                    ? base.ImagePath
                    : (this.State == LairState.Empty ? this.OpenedLairImagePath : this.ClosedLairImagePath);
            }
        }
    }
}