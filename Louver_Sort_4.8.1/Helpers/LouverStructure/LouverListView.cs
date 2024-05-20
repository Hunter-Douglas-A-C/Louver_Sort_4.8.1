using Newtonsoft.Json; // Make sure to add this using statement
using System;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    /// <summary>
    /// Represents a view of louvers.
    /// </summary>
    [Serializable]
    public class LouverListView
    {

        private int _louverID;
        private string _side;
        private double? _distance;

        [JsonProperty("louver_id")]
        public int LouverID { get => _louverID; set => _louverID = value; }

        [JsonProperty("side")]
        public string Side { get => _side; set => _side = value; }

        [JsonProperty("distance")]
        public double? Distance { get => Math.Round(Convert.ToDouble(_distance), 3); set => _distance = Math.Round(Convert.ToDouble( value), 3); }






        /// <summary>
        /// Initializes a new instance of the LouverListView class.
        /// </summary>
        /// <param name="id">The ID of the louver.</param>
        /// <param name="side">The side of the louver.</param>
        /// <param name="distance">The distance of the louver.</param>
        public LouverListView(int id, string side, double? distance)
        {
            _louverID = id;
            _side = side;
            _distance = distance;
        }
    }
}
