// ------------------------------------------
// Need.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
namespace Learning.Domain
{
    public class Need
    {
        protected uint value;

        public Need(string name, uint maxValue, double highReward, double mediumReward, double lowReward)
        {
            this.Name = name;
            this.MaxValue = maxValue;
            this.HighLevelReward = highReward;
            this.LowLevelReward = lowReward;
            this.MediumLevelReward = mediumReward;
        }

        public string HighLevelSensation
        {
            get { return this.Name + "-high"; }
        }

        public string MediumLevelSensation
        {
            get { return this.Name + "-medium"; }
        }

        public string LowLevelSensation
        {
            get { return this.Name + "-low"; }
        }

        public double HighLevelReward { get; protected set; }
        public double MediumLevelReward { get; protected set; }
        public double LowLevelReward { get; protected set; }
        public uint MaxValue { get; protected set; }
        public string Name { get; protected set; }

        public uint Value
        {
            get { return this.value; }
            set { if ((value >= 0) && (value <= this.MaxValue)) this.value = value; }
        }

        public double Reward
        {
            get
            {
                var percent = (double) this.Value/this.MaxValue;
                if (percent < (1f/3f))
                    return this.LowLevelReward;
                return percent < (2f/3f) ? this.MediumLevelReward : this.HighLevelReward;
            }
        }

        public string Sensation
        {
            get
            {
                var percent = (double) this.Value/this.MaxValue;
                if (percent < (1f/3f))
                    return this.LowLevelSensation;
                return percent < (2f/3f) ? this.MediumLevelSensation : this.HighLevelSensation;
            }
        }
    }
}