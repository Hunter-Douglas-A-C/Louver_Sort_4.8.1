using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Louver_Sort_4._8._1.Helpers
{
    internal class Louver
    {
        private double _ID;
        private DateTime _timestamp;
        private double _length;
        private double _width;
        private string _model;
        private bool _XL;
        private bool _processed;
        private double _reading1;
        private double _reading2;
        private double _warp;
        private double _absWarp;
        private double _sag;
        private bool _orientation;
        private bool _rejected;
        private string _CauseofRejection;

        // Parameterless constructor
        public Louver()
        {

        }

        // Parameterized constructor
        public Louver(double ID, DateTime timestamp, double length, double width, string model, bool XL, bool processed,
                      double reading1, double reading2, double warp, double absWarp, double sag, bool orientation,
                      bool rejected, string causeOfRejection)
        {
            _ID = ID;
            _timestamp = timestamp;
            _length = length;
            _width = width;
            _model = model;
            _XL = XL;
            _processed = processed;
            _reading1 = reading1;
            _reading2 = reading2;
            _warp = warp;
            _absWarp = absWarp;
            _sag = sag;
            _orientation = orientation;
            _rejected = rejected;
            _CauseofRejection = causeOfRejection;
        }

        // Properties
        public double ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public DateTime Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }

        public double Length
        {
            get { return _length; }
            set { _length = value; }
        }

        public double Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public string Model
        {
            get { return _model; }
            set { _model = value; }
        }

        public bool XL
        {
            get { return _XL; }
            set { _XL = value; }
        }

        public bool Processed
        {
            get { return _processed; }
            set { _processed = value; }
        }

        public double Reading1
        {
            get { return _reading1; }
            set { _reading1 = value; }
        }

        public double Reading2
        {
            get { return _reading2; }
            set { _reading2 = value; }
        }

        public double Warp
        {
            get { return _warp; }
            set { _warp = value; }
        }

        public double AbsWarp
        {
            get { return _absWarp; }
            set { _absWarp = value; }
        }

        public double Sag
        {
            get { return _sag; }
            set { _sag = value; }
        }

        public bool Orientation
        {
            get { return _orientation; }
            set { _orientation = value; }
        }

        public bool Rejected
        {
            get { return _rejected; }
            set { _rejected = value; }
        }

        public string CauseOfRejection
        {
            get { return _CauseofRejection; }
            set { _CauseofRejection = value; }
        }
    }
}
