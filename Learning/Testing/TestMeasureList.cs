// ------------------------------------------
// TestMeasureList.cs, Learning
// 
// Created by Pedro Sequeira, 2013/12/09
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Learning.Testing.Config;
using Learning.Testing.Config.Parameters;
using Learning.Testing.Config.Scenarios;
using PS.Utilities.Collections;
using PS.Utilities.IO;
using PS.Utilities.Math;
using PS.Utilities.Serialization;

namespace Learning.Testing
{
    public class TestMeasureList : IDisposable, IEnumerable<ITestParameters>
    {
        public const char CSV_SEPARATOR = ';';
        private const string CSV_EXTENSION = "csv";

        #region Constructor

        public TestMeasureList(IScenario scenario, TestMeasure baseMeasure)
        {
            if ((scenario == null) || (baseMeasure == null))
                throw new ArgumentException("scenario and base measure cannot be null");

            this._baseMeasure = baseMeasure;
            this.scenario = scenario;
            this.WriteTemp = true;
        }

        #endregion

        #region IDisposable Members

        public virtual void Dispose()
        {
            if (this._tempWriter != null)
            {
                this._tempWriter.Close();
                this._tempWriter.Dispose();
            }
            this.Clear();
        }

        #endregion

        #region IEnumerable<ITestParameters> Members

        public IEnumerator<ITestParameters> GetEnumerator()
        {
            return this.testMeasures.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Fields

        protected readonly object locker = new object();

        protected readonly Dictionary<ITestParameters, TestMeasure> testMeasures =
            new Dictionary<ITestParameters, TestMeasure>();

        private StreamWriter _tempWriter;
        protected readonly IScenario scenario;
        private readonly TestMeasure _baseMeasure;

        #endregion

        #region Properties

        public ITestsConfig TestsConfig
        {
            get { return this.scenario.TestsConfig; }
        }

        public double MaxMeasure
        {
            get
            {
                return this.testMeasures.Count == 0
                    ? 0
                    : this.testMeasures.Values.Max(testMeasure => testMeasure.Value);
            }
        }

        public double MinMeasure
        {
            get
            {
                return this.testMeasures.Count == 0
                    ? 0
                    : this.testMeasures.Values.Min(testMeasure => testMeasure.Value);
            }
        }

        public string LastFilePath { get; protected set; }

        public bool WriteTemp { get; set; }

        #endregion

        #region Serialization Methods

        public virtual void CreateTempWriter(string tempFilePath)
        {
            if (string.IsNullOrWhiteSpace(tempFilePath)) return;

            //verifies path conditions
            PathUtil.CheckDeleteFile(tempFilePath, true);

            //creates temp file writer for chromosome histories
            this._tempWriter = new StreamWriter(tempFilePath) {AutoFlush = true};
            this._tempWriter.WriteLine(this._baseMeasure.Header.ToString(CSV_SEPARATOR));
        }

        public virtual void PrintToFile(string filePath)
        {
            lock (this.locker)
            {
                //checks file existence and creates file writer
                if (File.Exists(filePath)) File.Delete(filePath);
                using (var sw = new StreamWriter(new FileStream(filePath, FileMode.Create), Encoding.UTF8))
                {
                    //writes header
                    sw.WriteLine(this._baseMeasure.Header.ToString(CSV_SEPARATOR));

                    //writes all test measures, one test per line
                    foreach (var testMeasure in this.testMeasures.Values)
                    {
                        //tests that values created can be read
                        var value = testMeasure.ToValue();
                        if (this.ReadTestMeasure(value) == null)
                            continue;
                        sw.WriteLine(value.ToString(CSV_SEPARATOR));
                    }
                }
                this.LastFilePath = filePath;
            }
        }

        public virtual void ReadFromFile(string filePath)
        {
            lock (this.locker)
            {
                //checks file
                if (!File.Exists(filePath)) return;

                //creates reader
                var sr = new StreamReader(filePath);

                //reads first line (ignore header)
                sr.ReadLine();

                //ignore temp creation
                var writeTemp = this.WriteTemp;
                this.WriteTemp = false;

                //reads all lines from file and create a test measure for each one
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    //checks read test measure from line
                    var testMeasure = this.ReadTestMeasure(line.Split(CSV_SEPARATOR));
                    if (testMeasure == null)
                        continue;

                    //tries to add test measure to list
                    if (!this.Contains(testMeasure.Parameters))
                        this.Add(testMeasure.Parameters, testMeasure);
                }

                sr.Close();
                sr.Dispose();
                this.WriteTemp = writeTemp;
            }
        }

        public virtual void PrintFitnessStatistics(string filePath, bool printToCSV)
        {
            //prints performance throughout time
            var cumulativeFitnessAvgList = this.GetFitnessQuantityList();

            //only print to csv the top tests
            if (printToCSV)
                StatisticsUtil.PrintAllQuantitiesToCSV(
                    PathUtil.ReplaceExtension(filePath, CSV_EXTENSION), cumulativeFitnessAvgList);

            //always print images
            StatisticsUtil.PrintAllQuantitiesToImage(filePath, cumulativeFitnessAvgList);
        }

        protected virtual Dictionary<string, StatisticalQuantity> GetFitnessQuantityList()
        {
            var cumulativeFitnessAvgList = new Dictionary<string, StatisticalQuantity>();
            foreach (var testParameters in this)
            {
                var testMeasure = this[testParameters];
                if ((testMeasure.Quantity != null) &&
                    !cumulativeFitnessAvgList.ContainsKey(testMeasure.ToString()))
                    cumulativeFitnessAvgList.Add(testMeasure.ToString(), testMeasure.Quantity);
            }
            return cumulativeFitnessAvgList;
        }

        private TestMeasure ReadTestMeasure(string[] values)
        {
            var testMeasure = this._baseMeasure.CloneJson();
            return !testMeasure.FromValue(values) ? null : testMeasure;
        }

        #endregion

        #region List Methods

        public TestMeasure this[ITestParameters parameters]
        {
            get { return this.GetTestMeasure(parameters); }
        }

        public int Count
        {
            get { return this.testMeasures.Count; }
        }

        public virtual void Add(ITestParameters parameters, TestMeasure measure)
        {
            //locks from outside access
            lock (this.locker)
                if (!this.Contains(parameters))
                {
                    this.testMeasures.Add(parameters, measure);

                    //writes test results to temp file
                    if (this.WriteTemp && (this._tempWriter != null) &&
                        (this._tempWriter.BaseStream != null) && this._tempWriter.BaseStream.CanWrite)
                        this._tempWriter.WriteLine(measure.ToValue().ToString(CSV_SEPARATOR));
                }
        }

        public virtual bool Contains(ITestParameters parameters)
        {
            lock (this.locker)
                return this.testMeasures.ContainsKey(parameters);
        }

        public virtual TestMeasure GetTestMeasure(ITestParameters parameters)
        {
            lock (this.locker)
                return !this.testMeasures.ContainsKey(parameters)
                    ? null
                    : this.testMeasures[parameters];
        }

        public virtual void Remove(ITestParameters parameters)
        {
            lock (this.locker)
                if (this.Contains(parameters))
                    this.testMeasures.Remove(parameters);
        }

        public virtual void RemoveRange(IEnumerable<ITestParameters> testParameters)
        {
            lock (this.locker)
                foreach (var parameters in new List<ITestParameters>(testParameters))
                    this.Remove(parameters);
        }

        public virtual void Clear()
        {
            lock (this.locker)
                this.testMeasures.Clear();
        }

        public void ClearExcept(HashSet<string> parameterIDs)
        {
            lock (this.locker)
                foreach (var parameter in this.ToList())
                    if (!parameterIDs.Contains(parameter.ToString()))
                        this.Remove(parameter);
        }

        public virtual List<ITestParameters> Sort(bool topBottom = true)
        {
            lock (this.locker)
            {
                var sortedTestMeasures = new List<TestMeasure>(this.testMeasures.Values);
                sortedTestMeasures.Sort();
                if (topBottom) sortedTestMeasures.Reverse();
                return sortedTestMeasures.Select(testMeasure => testMeasure.Parameters).ToList();
            }
        }

        #endregion

        #region Selection Methods

        public void ClearExcept(HashSet<ITestParameters> testParameters)
        {
            lock (this.locker)
                this.RemoveRange(this.Where(parameters => !testParameters.Contains(parameters)));
        }

        public List<ITestParameters> SelectTopParameters(uint number)
        {
            return this.SelectTopParameters(number, true);
        }

        public List<ITestParameters> SelectTopParameters(uint number, bool top)
        {
            //selects only the "k" best measures, reverse if top measures
            var sortedParams = this.Sort(top);
            if (sortedParams.Count > number)
                sortedParams.RemoveRange((int) number, (int) (sortedParams.Count - number));

            return sortedParams;
        }

        public List<ITestParameters> SelectBestMeasures(int stdDevTimes)
        {
            //creates quantity from all tests fitness
            var fitnessQuantity = new StatisticalQuantity((uint) this.testMeasures.Count);
            foreach (var testMeasure in this.testMeasures.Values)
                fitnessQuantity.Value = testMeasure.Value;

            //determines the minimum fitness threshold to consider the best test measures
            var minThreshold = fitnessQuantity.Avg + (stdDevTimes*fitnessQuantity.StdDev);

            //returns only those parameters of tests which belong to the given threshold
            return (from testMeasure in this.testMeasures.Values
                where testMeasure.Value >= minThreshold
                select testMeasure.Parameters).ToList();
        }

        #endregion
    }
}