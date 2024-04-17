using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    public class OrderWithBarcode
    {
        public BarcodeSet BarcodeSet { get; set; }
        public Order Order { get; set; }

        public OrderWithBarcode(Order order)
        {
            Order = order;
            BarcodeSet = order.BarcodeHelper.Barcode;
        }
    }
}
