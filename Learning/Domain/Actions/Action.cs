// ------------------------------------------
// Action.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/10
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using Learning.Domain.Agents;
using PS.Utilities.Serialization;

namespace Learning.Domain.Actions
{
    public class Action : XmlResource, IAction
    {
        public Action(string id, IAgent agent)
        {
            this.Agent = agent;
            this.IdToken = id;
        }

        #region IAction Members

        public uint ID { get; set; }

        public IAgent Agent { get; private set; }

        public virtual double Execute()
        {
            return 0;
        }

        #endregion

        #region Overrides of XmlResource

        public override void InitElements()
        {
        }

        #endregion

        public override string ToString()
        {
            return this.IdToken;
        }

        public override bool Equals(object obj)
        {
            return
                ReferenceEquals(obj, this) ||
                ((obj is Action) && obj.ToString().Equals(this.ToString()));
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
    }
}