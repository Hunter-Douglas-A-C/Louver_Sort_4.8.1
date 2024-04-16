using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.ObjectModel;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    internal class ReportListView
    {
        public int LouverID { get; set; }
        public int LouverOrder { get; set; }
        public double CurrWarp { get; set; }
        public string Status { get; set; }

        public ReportListView(int id, int order, double warp, bool rejected)
        {
            LouverID = id;
            LouverOrder = order;
            CurrWarp = warp;
            Status = rejected ? "FAIL" : "PASS";
        }

    }
}
