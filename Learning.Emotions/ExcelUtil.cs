// ------------------------------------------
// ExcelUtil.cs, Learning.Emotions
//
// Created by Pedro Sequeira, 2012/10/9
//
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------
using CarlosAg.ExcelXmlWriter;

namespace Learning.IMRL.Emotions
{
    public class ExcelUtil : PS.Utilities.Math.ExcelUtil
    {
        public static void PrintDimension(
            AppraisalDimension dimension, Workbook workbook, Worksheet sheet)
        {
            PrintStatisticalQuantity(dimension, workbook, sheet, dimension.Name);
        }

        public static void FillDimensionSampleData(
            Worksheet sheet, AppraisalDimension dimension, WorksheetRow row)
        {
            FillQuantitySampleData(sheet, dimension, dimension.Name, row);
        }
    }
}