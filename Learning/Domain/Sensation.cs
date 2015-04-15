// ------------------------------------------
// Sensation.cs, Learning
//
// Created by Pedro Sequeira, 2012/10/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using System;
using System.Globalization;
using System.Xml;
using PS.Utilities.Serialization;

namespace Learning.Domain
{
    public interface ISensation : IXmlSerializable
    {
        double Reward { get; set; }
    }

    [Serializable]
    public class Sensation : XmlResource, ISensation
    {
        private const string REWARD_TAG = "reward";

        public Sensation()
        {
        }

        public Sensation(double reward)
        {
            this.Reward = reward;
        }

        #region ISensation Members

        public double Reward { get; set; }

        public override void InitElements()
        {
            this.Reward = 0;
        }

        public override void DeserializeXML(XmlElement element)
        {
            base.DeserializeXML(element);

            this.Reward = double.Parse(element.GetAttribute(REWARD_TAG), CultureInfo.InvariantCulture);
        }

        public override void SerializeXML(XmlElement element)
        {
            base.SerializeXML(element);

            element.SetAttribute(REWARD_TAG, this.Reward.ToString(CultureInfo.InvariantCulture));
        }

        #endregion
    }
}