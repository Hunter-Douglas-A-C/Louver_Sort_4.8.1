using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    internal class LabelID
    {
        private int _unsortedID;
        private int _sortedID;
        private string _orientation;

        public int UnsortedID { get => _unsortedID; set => _unsortedID = value; }

        public int SortedID { get => _sortedID; set => _sortedID = value; }

        public string Orientation { get => _orientation; set => _orientation = value; }

        public LabelID (int unsortedID, int sortedID, string _o)
        {
            UnsortedID = unsortedID;
            SortedID = sortedID;
            _orientation = _o;
        }
    }
}
