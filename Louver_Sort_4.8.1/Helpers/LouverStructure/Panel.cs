using Louver_Sort_4._8._1.Helpers.LouverStructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    /// <summary>
    /// Represents a panel containing sets of louvers.
    /// </summary>
    [Serializable]
    public class Panel
    {
        private int _id;
        private List<Set> _sets = new List<Set>();

        [JsonProperty("id")]
        public int ID { get => _id; set => _id = value; }

        [JsonProperty("sets")]
        public List<Set> Sets
        {
            get => _sets;
            set => _sets = value;
        }



















        /// <summary>
        /// Initializes a new instance of the <see cref="Panel"/> class with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the panel.</param>
        public Panel(int id)
        {
            _id = id;
        }

        /// <summary>
        /// Adds a set to the panel.
        /// </summary>
        /// <param name="s">The set to add.</param>
        /// <returns>The added set.</returns>
        public Set AddSet(Set s)
        {
            _sets.Add(s);
            return _sets[_sets.Count - 1];
        }

        /// <summary>
        /// Gets the set with the specified ID from the panel.
        /// </summary>
        /// <param name="id">The ID of the set to retrieve.</param>
        /// <returns>The set with the specified ID.</returns>
        public Set GetSet(SetID.SetId id)
        {
            foreach (Set set in _sets)
            {
                if (set.ID == id)
                {
                    return set;
                }
            }
            throw new ArgumentException("Set with the specified ID not found.", nameof(id));
        }

        /// <summary>
        /// Gets all sets in the panel.
        /// </summary>
        /// <returns>An enumerable collection of sets.</returns>
        public IEnumerable<Set> GetAllSets()
        {
            return _sets;
        }

        /// <summary>
        /// Sorts the louvers within the sets of the panel.
        /// </summary>
        /// <returns>The sorted sets of louvers.</returns>
        public List<Set> Sort()
        {
            List<Louver> CollectAll = new List<Louver>();
            foreach (var S in _sets)
            {
                CollectAll.AddRange(S.Louvers);
            }

            List<Louver> ByWarp = CollectAll.OrderBy(x => x.AbsDeviation).ToList();
            List<Louver> StoreBottom = new List<Louver>();

            foreach (Set set in _sets)
            {
                set.Louvers.Add(ByWarp[0]);
                ByWarp.Remove(ByWarp[0]);
            }
            foreach (Set set in _sets)
            {
                StoreBottom.Add(ByWarp[0]);
                ByWarp.Remove(ByWarp[0]);
            }

            foreach (Louver louver in CollectAll)
            {
                foreach (Set set in _sets)
                {
                    if (ByWarp.Count > 0)
                    {
                        set.Louvers.Add(ByWarp[0]);
                        ByWarp.Remove(ByWarp[0]);
                    }
                }
            }

            foreach (Set set in _sets)
            {
                set.Louvers.Add(StoreBottom[0]);
                StoreBottom.Remove(StoreBottom[0]);
            }
            return _sets;
        }
    }
}
