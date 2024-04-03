using System;
using System.Collections.Generic;
using System.Linq;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    internal class Order
    {
        private readonly BarcodeHelper _barcodeHelper;
        private readonly List<Opening> _openings = new List<Opening>();

        public BarcodeHelper BarcodeHelper => _barcodeHelper;
        public double Unit => _barcodeHelper.Unit;
        public List<Opening> Openings => _openings;

        public Order(BarcodeSet barcodeSet)
        {
            _barcodeHelper = new BarcodeHelper(barcodeSet);
        }

        public Opening AddOpening(Opening opening)
        {
            Openings.Add(opening);
            return Openings[Openings.Count -1];
        }

        public Opening GetOpeningByLine(int l)
        {
            // Searching for the opening with the specified ID
            foreach (Opening opening in Openings)
            {
                if (opening.Line == l)
                {
                    return opening;
                }
            }

            // If no opening with the specified ID is found, throw an exception
            throw new ArgumentException("Opening with the specified ID not found.", nameof(l));
        }
    
    }
}