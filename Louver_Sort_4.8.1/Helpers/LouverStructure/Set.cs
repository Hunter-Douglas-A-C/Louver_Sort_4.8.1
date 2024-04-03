using System;
using System.Collections.Generic;
using System.Windows.Controls;
using static Louver_Sort_4._8._1.Helpers.LouverStructure.SetID;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    internal class Set
    {
        private SetId _id;
        private DateTime _dateSortStarted;
        private DateTime _dateSortFinished;
        private int _louverCount;
        private List<Louver> _louvers = new List<Louver>();

        public SetId ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public DateTime DateSortStarted => _dateSortStarted;

        public DateTime DateSortFinished => _dateSortFinished;

        public double LouverCount => _louverCount;

        public List<Louver> Louvers => _louvers;

        public Set(SetId id, int louverCount)
        {
            _id = id;
            _louverCount = louverCount;
        }

        public void AssignLouverCount(int louverCount)
        {
            _louvers.Clear();
            _louverCount = louverCount;
            for (int i = 0; i < louverCount; i++)
            {
                _louvers.Add(new Louver(i));
            }
        }

        public Louver AddLouver(Louver l)
        {
            _louvers.Add(l);
            return _louvers.Contains(l) ? l : null;
        }

        public Louver AddLouvers(List<Louver> l)
        {
            _louvers.AddRange(l);
            return Louvers[Louvers.Count - 1];
        }

        public void StartSort(DateTime interactionTime)
        {
            _dateSortStarted = interactionTime;
        }

        public void StopSort(DateTime interactionTime)
        {
            _dateSortFinished = interactionTime;
        }

        public Louver GetLouver(int id)
        {
            foreach (Louver louver in Louvers)
            {
                if (louver.ID == id)
                {
                    return louver;
                }
            }
            throw new ArgumentException("Panel with the specified ID not found.", nameof(id));
        }
    }
}
