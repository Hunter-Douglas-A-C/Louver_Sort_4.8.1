using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Louver_Sort_4._8._1.Helpers
{
    internal class Globals
    {
        public string AdminPassword = "12345";
        public double RejectionSpec = 0.04;
        public double ReCutRailSpec = 0.01;
        public double ReCutLouverToLouverSpec = 0.02;
        public double GapSpecRailToLouver = 1;
        public double GapSpecLouverToLouver = 1;
        public int DataRetentionPeriod = 90;
        public int RecalibrationPeriod = 10;
        public int OrderCount = 0;
        public double CalibrationRejectionSpec = 1;
        public string ExcelExportLocation = "Select export location ->";
        public List<string> UserIDs = new List<string> { "2014301" };
    }
}
