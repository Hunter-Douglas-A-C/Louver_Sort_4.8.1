using Louver_Sort_4._8._1.Helpers.LouverStructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    /// <summary>
    /// Represents a set of louvers.
    /// </summary>
    [Serializable]
    public class Set
    {

        private SetID.SetId _id;
        private DateTime _dateSortStarted;
        private DateTime _dateSortFinished;
        private double _louverCount;
        private List<Louver> _louvers = new List<Louver>();
        private List<Louver> _rejectedlouvers = new List<Louver>();
        private ObservableCollection<LouverListView> _recordedLouvers = new ObservableCollection<LouverListView>();
        private ObservableCollection<ReportListView> _reportData = new ObservableCollection<ReportListView>();

        [JsonProperty("recordedLouvers")]
        public ObservableCollection<LouverListView> RecordedLouvers
        {
            get => _recordedLouvers;
            set => _recordedLouvers = value;
        }

        [JsonProperty("reportData")]
        public ObservableCollection<ReportListView> ReportData
        {
            get => _reportData;
            set => _reportData = value;
        }
        [JsonProperty("id")]
        public SetID.SetId ID { get => _id; set => _id = value; }

        [JsonProperty("dateSortStarted")]
        public DateTime DateSortStarted { get => _dateSortStarted; set => _dateSortStarted = value; }

        [JsonProperty("dateSortFinished")]
        public DateTime DateSortFinished { get => _dateSortFinished; set => _dateSortFinished = value; }

        [JsonProperty("louverCount")]
        public double LouverCount { get => _louverCount; set => _louverCount = value; }

        [JsonProperty("louvers")]
        public List<Louver> Louvers
        {
            get => _louvers;
            set => _louvers = value;
        }

        [JsonProperty("rejectedlouvers")]
        public List<Louver> RejectedLouvers
        {
            get => _rejectedlouvers;
            set => _rejectedlouvers = value;
        }





        /// <summary>
        /// Initializes a new instance of the <see cref="Set"/> class.
        /// </summary>
        /// <param name="id">The ID of the set.</param>
        /// <param name="louverCount">The count of louvers in the set.</param>
        public Set(SetID.SetId id, double louverCount)
        {
            _id = id;
            _louverCount = louverCount;
        }

        /// <summary>
        /// Assigns the louver count to the set.
        /// </summary>
        /// <param name="louverCount">The count of louvers in the set.</param>
        public void AssignLouverCount(double louverCount)
        {
            _louvers.Clear();
            _louverCount = louverCount;
            for (int i = 0; i < louverCount; i++)
            {
                _louvers.Add(new Louver(i));
            }
        }

        /// <summary>
        /// Adds a louver to the set.
        /// </summary>
        /// <param name="l">The louver to add.</param>
        /// <returns>The added louver.</returns>
        public Louver AddLouver(Louver l)
        {
            _louvers.Add(l);
            return _louvers.Contains(l) ? l : null;
        }

        /// <summary>
        /// Adds a list of louvers to the set.
        /// </summary>
        /// <param name="l">The list of louvers to add.</param>
        /// <returns>The last added louver.</returns>
        public Louver AddLouvers(List<Louver> l)
        {
            _louvers.AddRange(l);
            return Louvers[Louvers.Count - 1];
        }

        /// <summary>
        /// Starts the sorting process of the set.
        /// </summary>
        /// <param name="interactionTime">The time when the sorting process started.</param>
        public void StartSort(DateTime interactionTime)
        {
            _dateSortStarted = interactionTime;
        }

        /// <summary>
        /// Stops the sorting process of the set.
        /// </summary>
        /// <param name="interactionTime">The time when the sorting process stopped.</param>
        public void StopSort(DateTime interactionTime)
        {
            _dateSortFinished = interactionTime;
        }

        /// <summary>
        /// Gets the louver with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the louver to get.</param>
        /// <returns>The louver with the specified ID.</returns>
        public Louver GetLouver(int id)
        {
            foreach (Louver louver in Louvers)
            {
                if (louver.ID == id)
                {
                    return louver;
                }
            }
            throw new ArgumentException("Louver with the specified ID not found.", nameof(id));
        }

        /// <summary>
        /// Gets all louvers in the set.
        /// </summary>
        /// <returns>The list of all louvers in the set.</returns>
        public List<Louver> GetLouverSet()
        {
            return _louvers;
        }

        /// <summary>
        /// Sorts the louvers in the set.
        /// </summary>
        public void Sort()
        {
            foreach (var item in Louvers)
            {
                item.Processed = true;
            }

            // Sort Louvers by AbsWarp
            List<Louver> sortedLouvers = Louvers.OrderBy(x => x.AbsDeviation).ToList();

            // Add the first Louver to the sortedLouvers list
            _louvers[Louvers.FindIndex(x => x.ID == sortedLouvers[0].ID)].SortedID = 1;
            sortedLouvers.RemoveAt(0);
            if (sortedLouvers.Count > 0)
            {
                var bottom = sortedLouvers[0];
                sortedLouvers.RemoveAt(0);

                int startIndex = 2;
                int endIndex = _louvers.Count - 1; // Start from the end

                while (sortedLouvers.Count > 0)
                {
                    if (sortedLouvers.Count != 0)
                    {
                        _louvers[Louvers.FindIndex(x => x.ID == sortedLouvers[0].ID)].SortedID = startIndex;
                        sortedLouvers.RemoveAt(0);
                        startIndex++;
                    }

                    if (sortedLouvers.Count != 0)
                    {
                        _louvers[Louvers.FindIndex(x => x.ID == sortedLouvers[0].ID)].SortedID = endIndex;
                        sortedLouvers.RemoveAt(0);
                        endIndex--;
                    }
                }

                //int i = 2;
                //while (sortedLouvers.Count > 0)
                //{
                //    if (sortedLouvers.Count != 0)
                //    {
                //        _louvers[Louvers.FindIndex(x => x.ID == sortedLouvers[0].ID)].SortedID = i;
                //        sortedLouvers.RemoveAt(0);
                //    }

                //    if (sortedLouvers.Count != 0)
                //    {
                //        _louvers[Louvers.FindIndex(x => x.ID == sortedLouvers[0].ID)].SortedID = _louvers.Count - i;
                //        sortedLouvers.RemoveAt(0);
                //    }

                //    i++;
                //}

                _louvers[Louvers.FindIndex(x => x.ID == bottom.ID)].SortedID = _louvers.Count;
            }


            // Update the collection
            _recordedLouvers = GenerateRecordedLouvers();
        }

        /// <summary>
        /// Generates the recorded louvers in the set.
        /// </summary>
        /// <returns>The recorded louvers.</returns>
        public ObservableCollection<LouverListView> GenerateRecordedLouvers()
        {
            _recordedLouvers.Clear();
            _louvers = _louvers.OrderBy(x => x.ID).ToList();
            foreach (var item in _louvers)
            {
                //if (!item.Processed)
                //{
                //    _recordedLouvers.Add(new LouverListView(item.ID, "Top", item.Reading1));
                //    _recordedLouvers.Add(new LouverListView(item.ID, "Bottom", item.Reading2));
                //}

                _recordedLouvers.Add(new LouverListView(item.ID, "Top", item.Readings.Reading1));
                _recordedLouvers.Add(new LouverListView(item.ID, "Bottom", item.Readings.Reading2));

            }
            return _recordedLouvers;
        }



        /// <summary>
        /// Generates the report data of the set.
        /// </summary>
        /// <returns>The report data.</returns>
        public ObservableCollection<ReportListView> GenerateReport(double GapSpecLouverToRail, double GapSpecLouverToLouver)
        {
            _reportData.Clear();
            double LastDev = 0;
            _louvers = _louvers.OrderBy(louver => louver.SortedID).ToList();
            foreach (var item in _louvers)
            {
                double gapSpec = (item.SortedID == _louvers.First().SortedID || item.SortedID == _louvers.Last().SortedID) ? GapSpecLouverToRail : GapSpecLouverToLouver;

                if ((LastDev + item.AbsDeviation) < gapSpec)
                {
                    _reportData.Add(new ReportListView(item.ID, item.SortedID, item.AbsDeviation, item.Rejected, item.Orientation, "Pass"));
                }
                else
                {
                    _reportData.Add(new ReportListView(item.ID, item.SortedID, item.AbsDeviation, item.Rejected, item.Orientation, "Fail"));
                }

                LastDev = item.AbsDeviation;
            }
            return _reportData;
        }
    }
}
