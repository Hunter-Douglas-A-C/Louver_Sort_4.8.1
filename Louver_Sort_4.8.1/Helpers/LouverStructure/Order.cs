using Louver_Sort_4._8._1.Helpers.LouverStructure;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    /// <summary>
    /// Represents an order containing barcode information and openings.
    /// </summary>
    [Serializable]
    public class Order
    { 


        private BarcodeHelper _barcodeHelper;
        private List<Opening> _openings = new List<Opening>();
        private double _unit;
        private string _user;

        [JsonProperty("barcodeHelper")]
        public BarcodeHelper BarcodeHelper
        {
            get => _barcodeHelper;
            set => _barcodeHelper = value; 
        }

        [JsonProperty("unit")]
        public double Unit
        {
            get => _unit;
            set => _unit = value;
        }

        [JsonProperty("user")]
        public string User
        {
            get => _user;
            set => _user = value;
        }

        [JsonProperty("openings")]
        public List<Opening> Openings
        {
            get => _openings;
            set => _openings = value;
        }























        // Constructor
        /// <summary>
        /// Initializes a new instance of the Order class.
        /// </summary>
        /// <param name="barcodeSet">The barcode set associated with the order.</param>
        public Order(BarcodeSet barcodeSet)
        {
            if (barcodeSet != null)
            {
                _barcodeHelper = new BarcodeHelper(barcodeSet);
            }
        }

        // Methods
        /// <summary>
        /// Adds an opening to the order.
        /// </summary>
        /// <param name="opening">The opening to add.</param>
        /// <returns>The added opening.</returns>
        public Opening AddOpening(Opening opening)
        {
            _openings.Add(opening);
            return _openings[_openings.Count - 1];
        }

        /// <summary>
        /// Gets an opening from the order by its line number.
        /// </summary>
        /// <param name="line">The line number of the opening.</param>
        /// <returns>The opening with the specified line number.</returns>
        public Opening GetOpeningByLine(int line)
        {
            foreach (Opening opening in _openings)
            {
                if (opening.Line == line)
                {
                    return opening;
                }
            }
            throw new ArgumentException("Opening with the specified line number not found.", nameof(line));
        }


    }
}
