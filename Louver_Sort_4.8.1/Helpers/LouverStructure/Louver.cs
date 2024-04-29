using Newtonsoft.Json;
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
        private double _reading1;
        private double _reading2;
        private double _devation;
        private double _absDevation;
        private bool _orientation;
        private bool _rejected;
        private string _causeOfRejection;
        private int _sortedId;

        [JsonProperty("id")]
        public int ID => _ID;

        [JsonProperty("processed")]
        public bool Processed { get => _processed; set => _processed = value; }

        [JsonProperty("reading1")]
        public double Reading1 { get => _reading1; set => _reading1 = value; }

        [JsonProperty("reading2")]
        public double Reading2 { get => _reading2; set => _reading2 = value; }

        [JsonProperty("deviation")]
        public double Deviation { get => _devation; set => _devation = value; }

        [JsonProperty("absDeviation")]
        public double AbsDeviation { get => _absDevation; set => _absDevation = value; }

        [JsonProperty("orientation")]
        public bool Orientation { get => _orientation; set => _orientation = value; }

        [JsonProperty("rejected")]
        public bool Rejected { get => _rejected; set => _rejected = value; }

        [JsonProperty("causeOfRejection")]
        public string CauseOfRejection { get => _causeOfRejection; set => _causeOfRejection = value; }

        [JsonProperty("sortedId")]
        public int SortedID { get => _sortedId; set => _sortedId = value; }















        // Constructor for deserialization
        [JsonConstructor]
        public Louver(int id, bool processed, double reading1, double reading2, double deviation, double absDeviation, bool orientation, bool rejected, string causeOfRejection)
        {
            _ID = id;
            _processed = processed;
            _reading1 = reading1;
            _reading2 = reading2;
            _devation = deviation;
            _absDevation = absDeviation;
            _orientation = orientation;
            _rejected = rejected;
            _causeOfRejection = causeOfRejection;
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

        // Methods
        /// <summary>
        /// Sets the first reading of the louver.
        /// </summary>
        /// <param name="r1">The first reading value.</param>
        public void SetReading1(double r1) => _reading1 = r1;

        /// <summary>
        /// Sets the second reading of the louver.
        /// </summary>
        /// <param name="r2">The second reading value.</param>
        public void SetReading2(double r2) => _reading2 = r2;

        /// <summary>
        /// Calculates the deviation and orientation values of the louver.
        /// </summary>
        public void CalcValues(double rejectionValue)
        {



            //_processed = true;
            //_devation = Math.Abs(Reading1) > Math.Abs(Reading2) ? Reading1 : Reading2;
            //_orientation = _devation > 0;
            //_absDevation = Math.Abs(_devation);

            _processed = true;
            _devation = Reading1 - Reading2;
            if (_devation > 0)
            {
                _orientation = true;
            }
            else
            {
                _orientation = false;
            }
            _absDevation = Math.Abs(_devation);
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
        public void Reject(string cause)
        {
            _rejected = true;
            _causeOfRejection = cause;
        }

        /// <summary>
        /// Resets the louver to its initial state.
        /// </summary>
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
