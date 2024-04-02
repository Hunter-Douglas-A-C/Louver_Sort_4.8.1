using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    internal class BarcodeSet
    {
        private string _barcode1;
        private string _barcode2;

        public string Barcode1
        {
            get { return _barcode1; }
            set
            {
                _barcode1 = value;
            }
        }

        public string Barcode2
        {
            get { return _barcode2; }
            set
            {
                _barcode2 = value;
            }
        }


        public BarcodeSet(string b1, string b2)
        {
            Barcode1 = b1;
            Barcode2 = b2;
        }

        public BarcodeSet(BarcodeSet b)
        {
            Barcode1 = b.Barcode1;
            Barcode2 = b.Barcode2;
        }

    }
}
