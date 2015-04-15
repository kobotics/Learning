// ------------------------------------------
// PartiallyObservableState.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System.Collections.Generic;
using PS.Utilities.Serialization;

namespace Learning.Domain.States
{
    public class PartiallyObservableState : XmlResource, IState
    {
        #region Properties

        public NonObservableState NonObservablePart { get; set; }

        public ObservableState ObservablePart { get; set; }

        public Dictionary<string, object> StateFeatures { get; protected set; }

        public uint ID { get; set; }

        #endregion

        #region Constructor

        public PartiallyObservableState(ObservableState observableState, NonObservableState nonObservableState)
        {
            this.ObservablePart = observableState;
            this.NonObservablePart = nonObservableState;
            this.IdToken = this.ObservablePart.IdToken + this.NonObservablePart.IdToken;
        }

        #endregion

        #region Nested type: NonObservableState

        public class NonObservableState : XmlResource, IState
        {
            public Dictionary<string, object> StateFeatures { get; protected set; }

            #region IState Members

            public uint ID { get; set; }

            #endregion

            public override void InitElements()
            {
                this.StateFeatures = new Dictionary<string, object>();
            }
        }

        #endregion

        #region Nested type: ObservableState

        public class ObservableState : NonObservableState
        {
        }

        #endregion

        #region Overrides of XmlResource

        public override void InitElements()
        {
        }

        #endregion
    }
}