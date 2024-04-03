using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    internal class Panel
    {
        private int _id;
        private List<Set> _sets = new List<Set>();

        public int ID => _id;
        public List<Set> Sets => _sets;

        public Panel(int id)
        {
            _id = id;
        }

        public Set AddSet(Set s)
        {
            Sets.Add(s);
            return Sets[Sets.Count - 1];
        }


        public Set GetSet(SetID.SetId id)
        {
            foreach (Set set in Sets)
            {
                if (set.ID == id)
                {
                    return set;
                }
            }
            throw new ArgumentException("Panel with the specified ID not found.", nameof(id));
        }


        //        // Flatten all louvers across all sets into a single sorted list.
        //        var sortedLouvers = Sets.SelectMany(s => s.Louvers)
        //                                .OrderBy(l => l.AbsWarp)
        //                                .ToList();

        //    // Assuming the goal is to redistribute these louvers evenly across sets.
        //    // Reset each set's louvers to empty to redistribute.
        //    foreach (var set in Sets)
        //    {
        //        set.Louvers.Clear();
        //    }

        //    // Distribute louvers evenly across sets based on sorted order.
        //    int setIndex = 0;
        //    foreach (var louver in sortedLouvers)
        //    {
        //        Sets[setIndex].Louvers.Add(louver);
        //        setIndex = (setIndex + 1) % Sets.Count; // Cycle through sets.
        //    }

        //// Note: This example assumes you want to distribute sorted louvers evenly.
        //// If the sorting logic is different, adjust the LINQ query and distribution logic accordingly.

        public List<Set> Sort()
        {
            List<Louver> CollectAll = new List<Louver>();
            foreach (var S in Sets)
            {
                CollectAll.AddRange(S.Louvers);
            }

            List<Louver> ByWarp = CollectAll.OrderBy(x => x.AbsWarp).ToList();
            List<Louver> StoreBottom = new List<Louver>();

            foreach (Set set in Sets)
            {
                set.Louvers.Add(ByWarp[0]);
                ByWarp.Remove((ByWarp[0]));
            }
            foreach (Set set in Sets)
            {
                StoreBottom.Add(ByWarp[0]);
                ByWarp.Remove((ByWarp[0]));
            }


            foreach (Louver louver in CollectAll)
            {
                foreach (Set set in Sets)
                {
                    if (ByWarp.Count > 0)
                    {
                        set.Louvers.Add(ByWarp[0]);
                        ByWarp.Remove((ByWarp[0]));
                    }
                }
            }








            foreach (Set set in Sets)
            {
                set.Louvers.Add(StoreBottom[0]);
                StoreBottom.Remove(StoreBottom[0]);
            }



            return Sets;
        }
    }
}
