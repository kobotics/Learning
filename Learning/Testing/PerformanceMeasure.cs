// ------------------------------------------
// PerformanceMeasure.cs, Learning
//
// Created by Pedro Sequeira, 2013/12/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using System.Threading;
using PS.Utilities;

namespace Learning.Testing
{
    public class PerformanceMeasure : IResetable
    {
        #region Fields

        private long _memoryStart;
        private DateTime _timer;

        #endregion

        #region Propperties

        public long MemoryUsage { get; protected set; }

        public TimeSpan TimeElapsed { get; protected set; }

        #endregion

        #region Constructor

        public PerformanceMeasure()
        {
            this.TimeElapsed = new TimeSpan();
        }

        #endregion

        #region Public Methods

        public void Reset()
        {
            // "zero"s all measures
            this.MemoryUsage = 0;
            this.TimeElapsed = new TimeSpan();
        }

        public virtual void Start()
        {
            //starts measures (time and memory)
            this._timer = DateTime.Now;
            Thread.MemoryBarrier();
            this._memoryStart = GC.GetTotalMemory(true);
        }

        public virtual void Stop()
        {
            //stops timers and measures
            Thread.MemoryBarrier();
            var memoryEnd = GC.GetTotalMemory(true);

            this.TimeElapsed = DateTime.Now.Subtract(this._timer);
            this.MemoryUsage += memoryEnd - this._memoryStart;
        }

        #endregion
    }
}