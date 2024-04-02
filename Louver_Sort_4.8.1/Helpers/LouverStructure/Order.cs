using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    internal class Order
    {
        private string _barcode1;
        private string _barcode2;
        private BarcodeHelper _barcodeHelper;
        private double _unit;

        private List<Opening> _openings = new List<Opening>();

        public string Barcode1
        {
            get { return _barcode1; }
            set { _barcode1 = value; }
        }

        public string Barcode2
        {
            get { return _barcode2; }
            set { _barcode2 = value; }
        }

        public BarcodeHelper BarcodeHelper
        {
            get { return _barcodeHelper; }
            set { _barcodeHelper = value; }
        }

        public double Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }

        public List<Opening> Openings
        {
            get { return _openings; }
            set { _openings = value; }
        }

        public Order(BarcodeSet b)
        {
            Barcode1 = b.Barcode1;
            Barcode2 = b.Barcode2;
            BarcodeHelper = new BarcodeHelper(b);
            Unit = BarcodeHelper.Unit;
        }

        public void AddOpening(Opening o)
        {
            Openings.Add(o);
        }
    }
}
