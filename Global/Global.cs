using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global
{
    public static class Global
    {
        public static string wPenaltyType = "linear";
        public static double wPenaltyParam = 0.3;
        public static double wPenaltyAlpha = 10000;//~1.0002 for Scale, ~10000 for sigmoid
        public static string wPenaltyScale = "sigmoid";
        public static double wPenaltyOffset = -1.0;
        public static int waterCount = 0;
        public static bool Hard = false;
    }
}