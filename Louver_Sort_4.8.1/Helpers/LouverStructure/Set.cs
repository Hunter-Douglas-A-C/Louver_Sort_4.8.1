using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    internal class Set : SetID
    {
        private SetId _ID; 
        private DateTime _dateSortStarted = new DateTime();
        private DateTime _dateSortFinished = new DateTime();
        private double _louverCount;
        private List<Louver> _louvers = new List<Louver>();

        public SetId ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public DateTime DateSortStarted
        {
            get { return _dateSortStarted; }
            set { _dateSortStarted = value; }
        }

        public DateTime DateSortFinished
        {
            get { return _dateSortFinished; }
            set { _dateSortFinished = value; }
        }

        public double LouverCount
        {
            get { return _louverCount; }
            set { _louverCount = value; }
        }

        public List<Louver> Louvers
        {
            get { return _louvers; }
            set { _louvers = value; }
        }


        public Set(SetId id, double louverCount)
        {
            _ID = id;
            _louverCount = louverCount;
        }





























        public void AssignLouverCount(double louvercount)
        {
            LouverCount = louvercount;
            for (int i = 0; i < louvercount - 1; i++)
            {
                Louvers.Add(new Louver(i));
            }
        }

        public void AddLouver(Louver l)
        {
            Louvers.Add(l);
        }

        public void AddLouvers(List<Louver> l)
        {
            foreach (var Louver in l)
            {
                Louvers.Add(Louver);
            }
        }

        public void StartSort()
        {
            DateSortStarted = DateTime.Now;
        }

        public void StopSort()
        {
            DateSortFinished = DateTime.Now;
        }
    }
}
