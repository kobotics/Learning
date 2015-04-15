// ------------------------------------------
// Util.cs, Learning.Forms
// 
// Created by Pedro Sequeira, 2014/01/16
// 
// pedro.sequeira@gaips.inesc-id.pt
// ------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using Learning.Testing.Config;
using MathNet.Numerics.Random;

namespace Learning.Forms
{
    public sealed class Util
    {
        private static readonly Random Rand = new WH2006(true);

        public static Color RandomColor
        {
            get
            {
                var length = Enum.GetValues(typeof (KnownColor)).Length;
                return Color.FromKnownColor((KnownColor) Rand.Next(length));
            }
        }

        public static string GetImageFilesDialogFilter()
        {
            var codecs = ImageCodecInfo.GetImageEncoders();
            var sep = string.Empty;
            var filterStr = new StringBuilder();

            foreach (var codecInfo in codecs)
            {
                var codecName = codecInfo.CodecName.Substring(8).Replace("Codec", "Files").Trim();
                filterStr.Append(string.Format("{0}{1} ({2})|{2}", sep, codecName, codecInfo.FilenameExtension.ToLower()));
                sep = "|";
            }

            filterStr.Append(string.Format("{0}{1} ({2})|{2}", sep, "All Files", "*.*"));

            return filterStr.ToString();
        }

        public static List<ITestsConfig> SelectReadTestsConfig(string message = null)
        {
            var fileName = SelectTestsConfigFile(message);
            return fileName == null ? new List<ITestsConfig>() : TestsConfig.DeserializeJsonFile(fileName);
        }

        private static string SelectTestsConfigFile(string message = null)
        {
            var openFileDialog = new OpenFileDialog
            {
                RestoreDirectory = true,
                Filter = "Json Config files|*.json",
                Title = message ?? "Select tests configuration file",
                FileName = "tests-config.json"
            };
            return openFileDialog.ShowDialog().Equals(DialogResult.OK) ? openFileDialog.FileName : null;
        }
    }
}