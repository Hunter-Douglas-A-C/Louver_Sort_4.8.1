using Lextm.SharpSnmpLib.Objects;
using System;
using System.Configuration;
using System.Windows.Controls;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    public class Louver
    {
        // Fields
        public readonly int _ID;
        public bool _processed;
        public double _reading1;
        public double _reading2;
        public double _devation;
        public double _absDevation;
        public bool _orientation;
        public bool _rejected;
        public string _causeOfRejection;
        public int _sortedId;

        // Properties
        public int ID => _ID;
        public bool Processed => _processed;
        public double Reading1 => _reading1;
        public double Reading2 => _reading2;
        public double Devation => _devation;
        public double AbsDevation => _absDevation;
        public bool Orientation => _orientation;
        public bool Rejected => _rejected;
        public string CauseOfRejection => _causeOfRejection;
        public int SortedID
        {
            get { return _sortedId; }
            set { _sortedId = value; }
        }

        // Constructor
        public Louver(int id)
        {
            _ID = id;
            Reset();
        }

        // Methods
        public void SetReading1(double r1) => _reading1 = r1;
        public void SetReading2(double r2) => _reading2 = r2;

        public void CalcValues()
        {
            _processed = true;
            if (Math.Abs(Reading1) > Math.Abs(Reading2))
            {
                _devation = Reading1;
            }
            else
            {
                _devation = Reading2;
            }
            _orientation = _devation > 0 ? true : false;
            _absDevation = Math.Abs(_devation);
        }

        public void SetWarp(double w) => _absDevation = w;

        public void Reject(string cause)
        {
            _rejected = true;
            _causeOfRejection = cause;
        }

        public void Reset()
        {
            _processed = false;
            _reading1 = 0;
            _reading2 = 0;
            _absDevation = 0;
            _orientation = true;
            _rejected = false;
            _causeOfRejection = "";
            _sortedId = 0;
        }



    }
}
