using Newtonsoft.Json; // Make sure to add this using statement
using System;
using System.Collections.Generic;
using System.Collections;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    /// <summary>
    /// Represents an association between an order and its corresponding barcode set.
    /// </summary>
    [Serializable]
    public class OrderWithBarcode
    { 


        private BarcodeSet _barcodeSet;
        private Order _order;


        [JsonProperty("barcode_set")]
        public BarcodeSet BarcodeSet { get => _barcodeSet; set => _barcodeSet = value; }

        [JsonProperty("order")]
        public Order Order { get => _order; set => _order = value; }







        /// <summary>
        /// Initializes a new instance of the <see cref="OrderWithBarcode"/> class.
        /// </summary>
        /// <param name="order">The order associated with the barcode set.</param>
        public OrderWithBarcode(Order order)
        {
            _order = order;
            _barcodeSet = order.BarcodeHelper.BarcodeSet;
        }


    }
}
