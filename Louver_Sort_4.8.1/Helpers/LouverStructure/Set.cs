using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using static Louver_Sort_4._8._1.Helpers.LouverStructure.SetID;
using System.Linq;
using System.Data.Odbc;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    public class Set
    {
        public SetId _id;
        public DateTime _dateSortStarted;
        public DateTime _dateSortFinished;
        public double _louverCount;
        public List<Louver> _louvers = new List<Louver>();
        public ObservableCollection<LouverListView> _recordedLouvers = new ObservableCollection<LouverListView>();
        public ObservableCollection<ReportListView> _ReportData = new ObservableCollection<ReportListView>();

        //public ObservableCollection<LouverListView> RecordedLouvers
        //{
        //    get { return _recordedLouvers; }
        //    set { _recordedLouvers = value; }
        //}

        //public ObservableCollection<ReportListView> ReportData
        //{
        //    get { return _ReportData; }
        //    set { _ReportData = value; }
        //}


        public SetId ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public DateTime DateSortStarted => _dateSortStarted;

        public DateTime DateSortFinished => _dateSortFinished;

        public double LouverCount => _louverCount;

        public List<Louver> Louvers => _louvers;

        public Set(SetId id, double louverCount)
        {
            _id = id;
            _louverCount = louverCount;
        }

        public void AssignLouverCount(double louverCount)
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

        public List<Louver> GetLouverSet()
        {
            return _louvers;
        }


        public void Sort()
        {
            // Sort Louvers by AbsWarp
            List<Louver> sortedLouvers = Louvers.OrderBy(x => x.AbsDevation).ToList();

            // Add the first Louver to the sortedLouvers list

            Louvers[Louvers.FindIndex(x => x.ID == sortedLouvers[0].ID)].SortedID = 0;
            sortedLouvers.RemoveAt(0);
            var bottom = sortedLouvers[0];
            sortedLouvers.RemoveAt(0);

            int i = 1;
            while (sortedLouvers.Count > 0)
            {
                if (sortedLouvers.Count != 0)
                {
                    Louvers[Louvers.FindIndex(x => x.ID == sortedLouvers[0].ID)].SortedID = i;
                    sortedLouvers.RemoveAt(0);
                }


                if (sortedLouvers.Count != 0)
                {
                    Louvers[Louvers.FindIndex(x => x.ID == sortedLouvers[0].ID)].SortedID = Louvers.Count - i;
                    sortedLouvers.RemoveAt(0);
                }

                i++;
            }

            Louvers[Louvers.FindIndex(x => x.ID == bottom.ID)].SortedID = Louvers.Count;







            //// Add the first Louver to the sortedLouvers list
            //if (sortedLouvers.Count > 0)
            //{
            //    Louvers.Add(sortedLouvers[0]);
            //    sortedLouvers.RemoveAt(0);

            //    // Add Louvers from the middle of the list
            //    while (sortedLouvers.Count - 2 > 0)
            //    {
            //        Louvers.Add(sortedLouvers[0]);
            //        sortedLouvers.RemoveAt(0);
            //    }

            //    // Add the last Louver to the sortedLouvers list
            //    if (sortedLouvers.Count - 1 > 0)
            //    {
            //        Louver bottom = sortedLouvers[sortedLouvers.Count - 1];
            //        sortedLouvers.RemoveAt(sortedLouvers.Count - 1);
            //        Louvers.Add(bottom);
            //    }
            //}

            // Update the collection
            _recordedLouvers = GenerateRecordedLouvers();
        }


        public ObservableCollection<LouverListView> GenerateRecordedLouvers()
        {
            _recordedLouvers.Clear();
            foreach (var item in Louvers)
            {
                _recordedLouvers.Add(new LouverListView(item.ID, "Top", item.Reading1));
                _recordedLouvers.Add(new LouverListView(item.ID, "Bottom", item.Reading2));
            }
            return _recordedLouvers;
        }


        public ObservableCollection<ReportListView> GenerateReport()
        {
            _ReportData.Clear();
            foreach (var item in Louvers)
            {
                _ReportData.Add(new ReportListView(item.ID, item.SortedID, item.AbsDevation, item.Rejected));
            }
            return _ReportData;
        }
    }
}
