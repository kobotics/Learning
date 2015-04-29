using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global
{
    public static class Global
    {
        public static int waterCount = 0;
        public static string wPenaltyType = "none";
        public static double wPenaltyParam = 0;
        public static double wPenaltyAlpha = 1.0002;
        public static string wPenaltyScale = "exp";
        public static bool Hard = false;
    }
}