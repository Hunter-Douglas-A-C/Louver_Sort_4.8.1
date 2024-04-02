using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    internal class Panel : SetID
    {
        private string _id;
        private List<Set> _sets = new List<Set>();

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public List<Set> Sets
        {
            get { return _sets; }
            set { _sets = value; }
        }

        public Panel(string id)
        {
            ID = id;
        }








        public void AddSet(Set s)
        {
            Sets.Add(s);
        }
    }
}
