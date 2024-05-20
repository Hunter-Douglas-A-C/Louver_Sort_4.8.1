using Newtonsoft.Json; // Make sure to add this using statement
using System;
using System.Linq;
using System.Windows.Controls;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    /// <summary>
    /// Represents an item in the report list view.
    /// </summary>
    [Serializable]
    public class ReportListView
    {
        private int _louverID;
        private int _louverOrder;
        private double _currWarp;
        private string _status;
        private string _orientation;
        public string Orientation { get => _orientation; set => _orientation = value; }

        [JsonProperty("louver_id")]
        public int LouverID { get => _louverID; set => _louverID = value; }

        [JsonProperty("louver_order")]
        public int LouverOrder { get => _louverOrder; set => _louverOrder = value; }

        [JsonProperty("current_warp")]
        public double CurrWarp { get => Math.Round(_currWarp, 3); set => _currWarp = Math.Round(value, 3); }

        [JsonProperty("status")]
        public string Status { get => _status; set => _status = value; }








        /// <summary>
        /// Initializes a new instance of the <see cref="ReportListView"/> class.
        /// </summary>
        /// <param name="id">The ID of the louver.</param>
        /// <param name="order">The order of the louver.</param>
        /// <param name="warp">The current warp of the louver.</param>
        /// <param name="rejected">A boolean indicating whether the louver is rejected.</param>
        public ReportListView(int id, int order, double warp, bool rejected, bool _o, string s)
        {
            _louverID = id;
            _louverOrder = order;
            _currWarp = warp;
            _status = rejected ? s = s +"FAIL" : s = s + "PASS";
            if (!_o)
            {
                _orientation = "Flip";
            }
            else
            {
                _orientation = "";
            }
        }
    }
}
