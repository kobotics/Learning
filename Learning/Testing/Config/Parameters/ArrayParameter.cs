// ------------------------------------------
// ArrayParameter.cs, Learning
// 
// Created by Pedro Sequeira, 2015/03/30
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PS.Utilities.Collections;

namespace Learning.Testing.Config.Parameters
{
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class ArrayParameter : IArrayParameter
    {
        private static readonly StepInterval<double> DefaultStepInterval = new StepInterval<double>(-1, 1, 0.1);
        private string[] _customHeader;

        public ArrayParameter() : this(new double[1])
        {
        }

        public ArrayParameter(double[] array) : this(2, array)
        {
        }

        public ArrayParameter(double[] array, StepInterval<double>[] domains)
            : this(2, array, domains)
        {
        }

        public ArrayParameter(uint numDecimalPlaces, double[] array)
            : this(numDecimalPlaces, array, null)
        {
        }

        public ArrayParameter(IArrayParameter baseParam)
            : this(baseParam.NumDecimalPlaces, baseParam.Array, baseParam.Domains)
        {
        }

        public ArrayParameter(uint numDecimalPlaces, double[] array, StepInterval<double>[] domains)
        {
            this.NumDecimalPlaces = numDecimalPlaces;
            this.Array = (double[]) array.Clone();
            this.Domains = domains ?? this.DefaultDomains;
        }

        protected StepInterval<double>[] DefaultDomains
        {
            get
            {
                var domains = new StepInterval<double>[this.Length];
                for (var i = 0; i < this.Length; i++)
                    domains[i] = DefaultStepInterval;

                return domains;
            }
        }

        #region IArrayParameter Members

        public double AbsoulteSum
        {
            get { return this.Array.Sum(val => Math.Abs(val)); }
        }

        public double Sum
        {
            get { return this.Array.Sum(); }
        }

        [JsonProperty]
        public uint NumDecimalPlaces { get; set; }

        [JsonProperty]
        public StepInterval<double>[] Domains { get; private set; }

        public uint Length
        {
            get { return (uint) this.Array.Length; }
        }

        [JsonProperty]
        public double[] Array { get; protected set; }

        public virtual bool Equals(ITestParameters other)
        {
            if (!(other is ArrayParameter)) return false;
            return this.Equals((ArrayParameter) other);
        }

        public void NormalizeVector()
        {
            var sumSquare = this.Array.Sum(param => param*param);
            var t = Math.Sqrt(sumSquare);

            if (t.Equals(0)) return;

            for (var i = 0; i < this.Length; i++)
                this.Array[i] /= t;
        }

        public void NormalizeSum()
        {
            var sum = this.Array.Sum();
            if (sum.Equals(0)) return;

            for (var i = 0; i < this.Length; i++)
                this.Array[i] /= sum;
        }

        public void NormalizeUnitSum()
        {
            do
            {
                //normalize elements (abs values sum)
                var sum = this.AbsoulteSum;
                for (var i = 0; i < this.Length; i++)
                    this[i] /= sum;

                //checks if elements are within domain, caps if necessary
                for (var i = 0; i < this.Length; i++)
                {
                    var curDomain = this.Domains[i];
                    if (this[i] < curDomain.min)
                        this[i] = curDomain.min;
                    else if (this[i] > curDomain.max)
                        this[i] = curDomain.max;
                }

                //checks terminal case, where values were not changed
            } while (!this.AbsoulteSum.Equals(1d));
        }

        public virtual void Round()
        {
            for (var i = 0; i < this.Length; i++)
                this.Array[i] = Math.Round(this.Array[i], 2);
        }

        public virtual object Clone()
        {
            return new ArrayParameter((IArrayParameter) this);
        }

        public double this[int index]
        {
            get { return this.Array[index]; }
            set { this.Array[index] = value; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<double> GetEnumerator()
        {
            return this.Array.ToList().GetEnumerator();
        }

        public void SetMidDomainValues()
        {
            //sets default values according to mid points in each param domain
            for (var i = 0; i < this.Length; i++)
                this.Array[i] = 0.5*(this.Domains[i].max - this.Domains[i].min);
        }

        public string[] ToValue()
        {
            return this.Array.ToStringArray();
        }

        public bool FromValue(string[] value)
        {
            var array = ArrayUtil.FromStringArray<double>(value);
            if (array != null)
                this.Array = array;
            return array != null;
        }

        public string[] Header
        {
            get
            {
                if (this._customHeader != null) return this._customHeader;

                var length = this.Array.Length;
                var header = new string[length];
                for (var i = 0; i < length; i++)
                    header[i] = string.Format("param{0}", i);
                return header;
            }
            set { this._customHeader = value; }
        }

        #endregion

        public void CapValuesToDomains()
        {
            //caps all param values according to domains
            for (var i = 0; i < this.Length; i++)
                if (this.Array[i] < this.Domains[i].min)
                    this.Array[i] = this.Domains[i].min;
                else if (this.Array[i] > this.Domains[i].max)
                    this.Array[i] = this.Domains[i].max;
        }

        public virtual bool Equals(ArrayParameter other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (this.Length != other.Length) return false;

            for (var i = 0; i < this.Length; i++)
                if (!this.Array[i].Equals(other.Array[i]))
                    return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            return (obj is ArrayParameter) && this.Equals((ArrayParameter) obj);
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public static bool operator ==(ArrayParameter left, ArrayParameter right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ArrayParameter left, ArrayParameter right)
        {
            return !Equals(left, right);
        }

        public static implicit operator double[](ArrayParameter arrayParam)
        {
            return arrayParam.Array;
        }

        public override string ToString()
        {
            var format = "{0}{1:#.";
            for (var i = 0; i < this.NumDecimalPlaces; i++)
                format += "#";
            format += "}_";
            return this.Array.Aggregate(
                string.Empty, (current, value) => string.Format(format, current, value));
        }

        public string ToVectorString()
        {
            var format = "{0}{1:0.";
            for (var i = 0; i < this.NumDecimalPlaces; i++)
                format += "0";
            format += "};";
            var str = this.Aggregate("[", (current, value) => string.Format(format, current, value));
            str = str.Remove(str.Length - 1, 1);
            return str + "]";
        }
    }
}