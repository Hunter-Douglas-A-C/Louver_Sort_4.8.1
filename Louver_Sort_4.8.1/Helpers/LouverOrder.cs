using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Louver_Sort_4._8._1.Helpers
{
    internal class LouverOrder
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
    }
}
