// ------------------------------------------
// TestMeasure.cs, Learning
// 
// Created by Pedro Sequeira, 2013/12/09
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using Learning.Testing.Config.Parameters;
using PS.Utilities.Collections;
using PS.Utilities.Math;

namespace Learning.Testing
{
    public class TestMeasure : IComparable<TestMeasure>, ICsvConvertible
    {
        private static readonly CultureInfo CultureInfo = CultureInfo.InvariantCulture;

        #region IComparable<TestMeasure> Members

        public int CompareTo(TestMeasure other)
        {
            return this.Value.CompareTo(other.Value);
        }

        #endregion

        #region ICsvConvertible Members

        public virtual string[] ToValue()
        {
            return this.Parameters.ToValue().Append(
                new List<string> {this.Value.ToString(CultureInfo), this.StdDev.ToString(CultureInfo), this.ID});
        }

        public virtual bool FromValue(string[] value)
        {
            if ((value == null) || (value.Length < 4)) return false;

            //gets test measure values
            var length = value.Length;
            this.ID = value[length - 1];
            this.StdDev = double.Parse(value[length - 2], CultureInfo);
            this.Value = double.Parse(value[length - 3], CultureInfo);

            //let rest of values to parameter (has to be set beforehand!)
            return this.Parameters == null ||
                   this.Parameters.FromValue(value.SubArray(0, length - 3));
        }

        public virtual string[] Header
        {
            get { return this.Parameters.Header.Append(new List<string> {"Fitness", "Std Dev", "ID"}); }
        }

        #endregion

        public override string ToString()
        {
            return this.ID;
        }

        public bool IsStatiscallyDifferentFrom(TestMeasure testMeasure, int n, double minPvalue)
        {
            var ttestResult = TTest.CalcTTest(this.Value, testMeasure.Value, this.StdDev*this.StdDev,
                testMeasure.StdDev*testMeasure.StdDev, n, n);

            return ttestResult.PValue <= minPvalue;
        }

        #region Properties

        public ITestParameters Parameters { get; set; }
        public string ID { get; set; }
        public double Value { get; set; }
        public double StdDev { get; set; }
        public StatisticalQuantity Quantity { get; set; }

        #endregion
    }
}