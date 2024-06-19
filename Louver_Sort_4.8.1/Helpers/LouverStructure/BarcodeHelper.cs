using Louver_Sort_4._8._1.Helpers.LouverStructure;
using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    /// <summary>
    /// Helper class for parsing barcode data.
    /// </summary>
    [Serializable]
    public class BarcodeHelper
    {
        private BarcodeSet _barcodeSet;
        private string _barcode1;
        private string _barcode2;
        private int _order;
        private int _line;
        private int _unit;
        private int _panelID;
        private SetID.SetId _set;
        private LouverStyle.LouverStyles _style;
        private double _width;
        private double _length;


        [JsonProperty("barcodSet")]
        public BarcodeSet BarcodeSet { get => _barcodeSet; set => _barcodeSet = value; }

        [JsonProperty("barcode1")]
        public string Barcode1 { get => _barcode1; set => _barcode1 = value; }

        [JsonProperty("barcode2")]
        public string Barcode2 { get => _barcode2; set => _barcode2 = value; }

        [JsonProperty("order")]
        public int Order { get => _order; set => _order = value; }

        [JsonProperty("line")]
        public int Line { get => _line; set => _line = value; }

        [JsonProperty("unit")]
        public int Unit { get => _unit; set => _unit = value; }

        [JsonProperty("panelID")]
        public int PanelID { get => _panelID; set => _panelID = value; }

        [JsonProperty("set")]
        public SetID.SetId Set { get => _set; set => _set = value; }

        [JsonProperty("style")]
        public LouverStyle.LouverStyles Style { get => _style; set => _style = value; }

        [JsonProperty("width")]
        public double Width { get => _width; set => _width = value; }

        [JsonProperty("length")]
        public double Length { get => _length; set => _length = value; }





        /// <summary>
        /// Constructor to initialize the BarcodeHelper with a BarcodeSet.
        /// </summary>
        /// <param name="barcode">The BarcodeSet containing barcode data.</param>
        public BarcodeHelper(BarcodeSet barcode)
        {
            if (barcode != null)
            {
                _barcodeSet = barcode;
                ParseBarcode(); // Parse the barcode data
            }
        }

        /// <summary>
        /// Parses the barcode data.
        /// </summary>
        private void ParseBarcode()
        {
            if (_barcodeSet?.Barcode1 != null)
            {
                ParseBarcode1(_barcodeSet.Barcode1); // Parse data from Barcode1
            }
            if (_barcodeSet?.Barcode2 != null)
            {
                ParseBarcode2(_barcodeSet.Barcode2); // Parse data from Barcode2
            }
        }

        /// <summary>
        /// Parses data from Barcode1.
        /// </summary>
        /// <param name="barcode1">The Barcode1 data string.</param>
        private void ParseBarcode1(string barcode1)
        {
            string pattern = @"(?<Order>\d{8})(?<Line>\d{3})(?<Unit>\d{5})(?<Panel>.\d{1})";
            Match match = Regex.Match(barcode1, pattern);
            if (!match.Success)
            {
                throw new FormatException("Barcode1 is not in the expected format.");
            }

            // Extract and parse data from Barcode1
            _order = int.Parse(match.Groups["Order"].Value);
            _line = int.Parse(match.Groups["Line"].Value);
            _unit = int.Parse(match.Groups["Unit"].Value);
            _panelID = int.Parse(match.Groups["Panel"].Value.Replace("P", ""));
        }

        /// <summary>
        /// Parses data from Barcode2.
        /// </summary>
        /// <param name="barcode2">The Barcode2 data string.</param>
        private void ParseBarcode2(string barcode2)
        {
            string pattern = @"(P..[1-9]\s{0,1}.{0,2})\/(L.{2,3})\/(L\d+\.\d+)\/(L\d+\.\d+)\/(L.)$";
            Match match = Regex.Match(barcode2, pattern);
            if (!match.Success)
            {
                throw new FormatException("Barcode2 is not in the expected format.");
            }

            // Extract and parse data from Barcode2
            string value = match.Groups[1].Value;

            if (value.Contains("PNL"))
            {
                _panelID = int.Parse(value.Replace("PNL", ""));
            }
            else if (value.Contains("PST"))
            {
                _panelID = int.Parse(value.Replace("PST1 P", ""));
            }
            _style = match.Groups[2].Value == "LXL" ? LouverStyle.LouverStyles.XL : LouverStyle.LouverStyles.Standard;
            _width = double.Parse(match.Groups[3].Value.Replace("L", ""));
            _length = double.Parse(match.Groups[4].Value.Replace("L", ""));
            _set = SetID.ConvertStringToSetId(match.Groups[5].Value.Replace("L", ""));
        }
    }
}
