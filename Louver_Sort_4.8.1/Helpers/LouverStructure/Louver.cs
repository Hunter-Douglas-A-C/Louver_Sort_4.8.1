using System;
using System.Windows.Controls;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    internal class Louver
    {
        // Fields
        private readonly int _ID;
        private bool _processed;
        private double _reading1;
        private double _reading2;
        private double _absWarp;
        private bool _orientation;
        private bool _rejected;
        private string _causeOfRejection;
        private double _sortedId;

        // Properties
        public int ID => _ID;
        public bool Processed => _processed;
        public double Reading1 => _reading1;
        public double Reading2 => _reading2;
        public double Warp => Math.Abs(_reading2 - _reading1);
        public double AbsWarp => _absWarp;
        public bool Orientation => _orientation;
        public bool Rejected => _rejected;
        public string CauseOfRejection => _causeOfRejection;
        public double SortedId => _sortedId;

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
            // Perform calculations here
            // For example:
            _absWarp = Math.Abs(_reading2 - _reading1);
        }

        public void SetWarp(double w) => _absWarp = w;

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
            _absWarp = 0;
            _orientation = true;
            _rejected = false;
            _causeOfRejection = "";
            _sortedId = 0;
        }



    }
}
