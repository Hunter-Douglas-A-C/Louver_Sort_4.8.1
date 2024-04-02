using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    internal class Louver
    {
        private double _ID;
        private bool _processed;
        private double _reading1;
        private double _reading2;
        private double _warp;
        private double _absWarp;
        private double _sag;
        private bool _orientation;
        private bool _rejected;
        private string _causeOfRejection;
        private double _sortedId;

        public double ID
        {
            get { return _ID; }
            set { _ID = value; }
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
            get { return _causeOfRejection; }
            set { _causeOfRejection = value; }
        }

        public double SortedId
        {
            get { return _sortedId; }
            set { _sortedId = value; }
        }

        public Louver(double ID)
        {
            _ID = ID;
            _processed = false;
            _reading1 = 0;
            _reading2 = 0;
            _warp = 0;
            _absWarp = 0;
            _sag = 0;
            _orientation = true;
            _rejected = false;
            _causeOfRejection = "";
            _sortedId = 0;
        }

        public void SetReading1(double R1)
        {
            Reading1 = R1;
        }
        public void SetReading2(double R2)
        { 
            Reading2 = R2;
        }

        public void CalcValues()
        {
            Processed = true;
            //ADD REST HERE
        }


    }
}
