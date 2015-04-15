// ------------------------------------------
// State.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
namespace Learning.Domain.States
{
    public abstract class State : IState
    {
        public const uint NEW_STATE_ID = uint.MaxValue;

        protected State()
        {
            this.ID = NEW_STATE_ID;
        }

        #region IState Members

        public uint ID { get; set; }

        #endregion

        public static implicit operator uint(State state)
        {
            return state.ID;
        }

        public override bool Equals(object obj)
        {
            return
                ReferenceEquals(obj, this) ||
                ((obj is State) && obj.ToString().Equals(this.ToString()));
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
    }
}