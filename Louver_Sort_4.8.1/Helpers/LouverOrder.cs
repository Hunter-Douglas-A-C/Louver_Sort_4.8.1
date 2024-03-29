using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Louver_Sort_4._8._1.Helpers
{
    internal class LouverOrder : IEnumerable<LouverSet>
    {
        private LouverSet _louverSetTop;
        private LouverSet _louverSetMiddle;
        private LouverSet _louverSetBottom;

        // Properties
        public LouverSet LouverSetTop
        {
            get { return _louverSetTop; }
            set { _louverSetTop = value; }
        }

        public LouverSet LouverSetMiddle
        {
            get { return _louverSetMiddle; }
            set { _louverSetMiddle = value; }
        }

        public LouverSet LouverSetBottom
        {
            get { return _louverSetBottom; }
            set { _louverSetBottom = value; }
        }

        // Constructors
        public LouverOrder()
        {
            // Default constructor
        }

        public LouverOrder(LouverSet louverSetTop, LouverSet louverSetMiddle, LouverSet louverSetBottom)
        {
            _louverSetTop = louverSetTop;
            _louverSetMiddle = louverSetMiddle;
            _louverSetBottom = louverSetBottom;
        }

        // Implementation of the IEnumerable<LouverSet> interface
        public IEnumerator<LouverSet> GetEnumerator()
        {
            yield return _louverSetTop;
            yield return _louverSetMiddle;
            yield return _louverSetBottom;
        }

        // Explicit non-generic interface implementation for IEnumerable
        IEnumerator IEnumerable.GetEnumerator()
        {
            // This simply calls the generic version above
            return GetEnumerator();
        }


        public LouverOrder Sort(List<Louver> louvers, LouverOrder louverOrder)
        {
            List<Louver> ByWarp = louvers.OrderBy(x => x.AbsWarp).ToList();
            List<Louver> StoreBottom = new List<Louver>();

            foreach (LouverSet set in louverOrder)
            {
                set.LouverList.Add(ByWarp[0]);
                ByWarp.Remove((ByWarp[0]));
            }
            foreach (LouverSet set in louverOrder)
            {
                StoreBottom.Add(ByWarp[0]);
                ByWarp.Remove((ByWarp[0]));
            }


            foreach (Louver louver in louvers)
            {
                foreach (LouverSet set in louverOrder)
                {
                    if (ByWarp.Count > 0)
                    {
                        set.LouverList.Add(ByWarp[0]);
                        ByWarp.Remove((ByWarp[0]));
                    }
                }
            }








            foreach (LouverSet set in louverOrder)
            {
                set.LouverList.Add(StoreBottom[0]);
                StoreBottom.Remove(StoreBottom[0]);
            }



            return louverOrder;
        }



    }
}
