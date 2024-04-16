using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    internal class LouverListView
    {
        public int LouverID { get; set; }
        public string Side { get; set; }
        public double Distance { get; set; }

        public LouverListView(int id, string side, double distance)
        {
            LouverID = id;
            Side = side;
            Distance = distance;
        }
    }
}
