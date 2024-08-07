﻿using Newtonsoft.Json;
using System;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    /// <summary>
    /// Represents a louver object.
    /// </summary>
    [Serializable]
    public class Louver
    {
        private readonly int _ID;
        private bool _processed;
        private Reading _readings = new Reading();
        private double? _devation;
        private double _absDevation;
        private bool _orientation;

        private int _sortedId;


        private bool _rejected;
        private string _causeOfRejection;



        [JsonProperty("rejected")]
        public bool Rejected { get => _rejected; set => _rejected = value; }

        [JsonProperty("causeOfRejection")]
        public string CauseOfRejection { get => _causeOfRejection; set => _causeOfRejection = value; }



        [JsonProperty("id")]
        public int ID => _ID;

        [JsonProperty("processed")]
        public bool Processed { get => _processed; set => _processed = value; }

        [JsonProperty("readings")]
        public Reading Readings { get => _readings; set => _readings = value; }


        [JsonProperty("deviation")]
        public double? Deviation { get => _devation; set => _devation = value; }

        [JsonProperty("absDeviation")]
        public double AbsDeviation { get => _absDevation; set => _absDevation = value; }

        [JsonProperty("orientation")]
        public bool Orientation { get => _orientation; set => _orientation = value; }

        [JsonProperty("sortedId")]
        public int SortedID { get => _sortedId; set => _sortedId = value; }


        // Constructor for deserialization
        [JsonConstructor]
        public Louver(int id, bool processed, double reading1, double reading2, double deviation, double absDeviation, bool orientation)
        {
            _ID = id;
            _processed = processed;
            _readings.Reading1 = reading1;
            _readings.Reading2 = reading2;
            _devation = deviation;
            _absDevation = absDeviation;
            _orientation = orientation;
        }

        // Constructor
        /// <summary>
        /// Initializes a new instance of the Louver class.
        /// </summary>
        /// <param name="id">The ID of the louver.</param>
        public Louver(int id)
        {
            _ID = id;
            Reset();
        }

        //// Methods
        ///// <summary>
        ///// Sets the first reading of the louver.
        ///// </summary>
        ///// <param name="r1">The first reading value.</param>
        //public void SetReading1(double? r1) => _readings.Reading1 = r1;

        ///// <summary>
        ///// Sets the second reading of the louver.
        ///// </summary>
        ///// <param name="r2">The second reading value.</param>
        ///// 
        //public void SetReading2(double? r2) => _readings.Reading1 = r2;

        /// <summary>
        /// Calculates the deviation and orientation values of the louver.
        /// </summary>
        public void CalcValues(double rejectionValue)
        {

            //_processed = true;
            //_devation = Math.Abs(Reading1) > Math.Abs(Reading2) ? Reading1 : Reading2;
            //_orientation = _devation > 0;
            //_absDevation = Math.Abs(_devation);
            _devation = _readings.Reading1 - _readings.Reading2;
            if (_devation > 0)
            {
                _orientation = true;
            }
            else
            {
                _orientation = false;
            }
            _absDevation = Math.Abs(Convert.ToDouble( _devation));
            if (_absDevation > rejectionValue)
            {
                _rejected = true;
            }
            else
            {
                _rejected = false;
            }
        }



        /// <summary>
        /// Sets the warp value of the louver.
        /// </summary>
        /// <param name="w">The warp value.</param>
        public void SetWarp(double w) => _absDevation = w;

        /// <summary>
        /// Rejects the louver with the specified cause.
        /// </summary>
        /// <param name="cause">The cause of rejection.</param>
        //public void Reject(string cause)
        //{
        //    _rejected = true;
        //    _causeOfRejection = cause;
        //}

        /// <summary>
        /// Resets the louver to its initial state.
        /// </summary>
        public void Reset()
        {
            _processed = false;
            _readings.Reading1 = null;
            _readings.Reading2 = null;
            _absDevation = 0;
            _orientation = true;
            _sortedId = 0;
        }






        public void Reject(string cause)
        {
            _rejected = true;
            _causeOfRejection = cause;
        }
    }
}
