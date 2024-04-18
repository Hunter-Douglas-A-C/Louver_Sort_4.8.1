using Louver_Sort_4._8._1.Helpers.LouverStructure;
using System;
using System.Text.RegularExpressions;

namespace Louver_Sort_4._8._1.Helpers
{
    public class BarcodeHelper
    {
        public readonly BarcodeSet _barcode;

        public BarcodeSet Barcode => _barcode;

        // Properties for the parsed data
        public int Order { get; private set; }
        public int Line { get; private set; }
        public int Unit { get; private set; }
        public int PanelID { get; private set; }
        public LouverStructure.SetID.SetId Set { get; private set; }
        public LouverStructure.LouverStyle.LouverStyles Style { get; private set; }
        public double Width { get; private set; }
        public double Length { get; private set; }

        public BarcodeHelper(BarcodeSet barcode)
        {
            _barcode = barcode;
            ParseBarcode();
        }

        private void ParseBarcode()
        {
            if (_barcode?.Barcode1 != null)
            {
                ParseBarcode1(_barcode.Barcode1);
            }
            if (_barcode?.Barcode2 != null)
            {
                ParseBarcode2(_barcode.Barcode2);
            }

        }

        private void ParseBarcode1(string barcode1)
        {
            string pattern = @"(?<Order>\d{8})(?<Line>\d{3})(?<Unit>\d{5})(?<L1>L\d)";
            Match match = Regex.Match(barcode1, pattern);
            if (!match.Success)
            {
                throw new FormatException("Barcode1 is not in the expected format.");
            }

            Order = int.Parse(match.Groups["Order"].Value);
            Line = int.Parse(match.Groups["Line"].Value);
            Unit = int.Parse(match.Groups["Unit"].Value);
        }

        private void ParseBarcode2(string barcode2)
        {
            string pattern = @"^(PNL[1-9])\/(LXL)\/(L\d+\.\d+)\/(L\d+\.\d+)\/(L.)$";
            Match match = Regex.Match(barcode2, pattern);
            if (!match.Success)
            {
                throw new FormatException("Barcode2 is not in the expected format.");
            }

            PanelID = int.Parse(match.Groups[1].Value.Replace("PNL", ""));
            Style = match.Groups[2].Value == "LXL" ? LouverStructure.LouverStyle.LouverStyles.XL : LouverStructure.LouverStyle.LouverStyles.Standard;
            Width = double.Parse(match.Groups[3].Value.Replace("L", ""));
            Length = double.Parse(match.Groups[4].Value.Replace("L", ""));
            Set = LouverStructure.SetID.ConvertStringToSetId(match.Groups[5].Value.Replace("L", ""));
        }
    }
}