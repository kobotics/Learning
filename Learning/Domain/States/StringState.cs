// ------------------------------------------
// StringState.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System.Collections.Generic;
using Learning.Domain.Memories;

namespace Learning.Domain.States
{
    public class StringState : State, IStimuliState
    {
        private readonly string _id;
        private readonly HashSet<string> _sensations;

        public StringState(string id)
        {
            this._id = id;
            this._sensations = new HashSet<string>(this._id.Split(StringLTM.STATE_SEPARATOR));
        }

        #region IStimuliState Members

        public HashSet<string> Sensations
        {
            get { return this._sensations; }
        }

        #endregion

        public override string ToString()
        {
            return this._id;
        }
    }
}