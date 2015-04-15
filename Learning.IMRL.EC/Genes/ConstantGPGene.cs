// ------------------------------------------
// ConstantGPGene.cs, Learning.IMRL.EC
//
// Created by Pedro Sequeira, 2013/2/6
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using AForge.Genetic;

namespace Learning.IMRL.EC.Genes
{
    public class ConstantGPGene : IGPGene
    {
        public ConstantGPGene(double value)
        {
            this.Value = value;
        }

        public double Value { get; set; }

        #region IGPGene Members

        public IGPGene Clone()
        {
            return new ConstantGPGene(this.Value);
        }

        public void Generate()
        {
        }

        public void Generate(GPGeneType type)
        {
        }

        public IGPGene CreateNew()
        {
            return this.Clone();
        }

        public IGPGene CreateNew(GPGeneType type)
        {
            return this.Clone();
        }

        public GPGeneType GeneType
        {
            get { return GPGeneType.Argument; }
        }

        public int ArgumentsCount
        {
            get { return 0; }
        }

        public int MaxArgumentsCount
        {
            get { return 0; }
        }

        #endregion

        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
}