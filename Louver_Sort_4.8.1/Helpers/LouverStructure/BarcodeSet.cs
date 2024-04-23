using Newtonsoft.Json; // Make sure to add this using statement
using System;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    /// <summary>
    /// Represents a set of barcodes.
    /// </summary>
    [Serializable]
    public class BarcodeSet
    {
        private string _barcode1;
        private string _barcode2;


        [JsonProperty("barcode1")]
        public string Barcode1 { get => _barcode1; set => _barcode1 = value; }

        [JsonProperty("barcode2")]
        public string Barcode2 { get => _barcode2; set => _barcode2 = value; }














        /// <summary>
        /// Initializes a new instance of the BarcodeSet class.
        /// </summary>
        /// <param name="b1">First barcode string.</param>
        /// <param name="b2">Second barcode string.</param>
        public BarcodeSet(string b1, string b2)
        {
            Barcode1 = b1;
            Barcode2 = b2;
        }

        // Override ToString method to provide string representation of BarcodeSet
        public override string ToString()
        {
            return $"{Barcode1},{Barcode2}";
        }

        // Override Equals method to compare BarcodeSet objects
        public override bool Equals(object obj)
        {
            return obj is BarcodeSet other &&
                   Barcode1 == other.Barcode1 &&
                   Barcode2 == other.Barcode2;
        }

        // Override GetHashCode method to provide a unique hash code for BarcodeSet
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Add hash codes of Barcode1 and Barcode2
                hash = hash * 23 + (Barcode1 != null ? Barcode1.GetHashCode() : 0);
                hash = hash * 23 + (Barcode2 != null ? Barcode2.GetHashCode() : 0);
                return hash;
            }
        }
    }
}
