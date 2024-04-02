using Louver_Sort_4._8._1.Helpers.LouverStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace Louver_Sort_4._8._1.Helpers
{
    internal class BarcodeHelper
    {
        private BarcodeSet _barcode;
        private double _order;
        private double _line;
        private double _unit;
        private string _PanelID;
        private SetID.SetId _Set;
        private LouverStyle.LouverStyles _Style;
        private double _Width;
        private double _Length;
        private double _LouverCount;

        public BarcodeSet Barcode
        {
            get { return _barcode; }
            set { _barcode = value; }
        }

        public double Order
        {
            get { return _order; }
            set { _order = value; }
        }

        public double Line
        {
            get { return _line; }
            set { _line = value; }
        }

        public double Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }

        public string PanelID
        {
            get { return _PanelID; }
            set { _PanelID = value; }
        }

        public SetID.SetId Set
        {
            get { return _Set; }
            set { _Set = value; }
        }

        public LouverStyle.LouverStyles Style
        {
            get { return _Style; }
            set { _Style = value; }
        }

        public double Width
        {
            get { return _Width; }
            set { _Width = value; }
        }

        public double Length
        {
            get { return _Length; }
            set { _Length = value; }
        }

        public double LouverCount
        {
            get { return _LouverCount; }
            set { _LouverCount = value; }
        }

        public BarcodeHelper(BarcodeSet b)
        {
            Barcode = new BarcodeSet(b);
            try
            {
                //string pattern = @"^(PST1\sP1)\/(LXL)\/(L\d+\.\d+)\/(L\d+\.\d+)\/(L[A-Za-z])$";
                //Match match = Regex.Match(b.Barcode2, pattern);

                //if (match.Success)
                //{
                //    PanelID = match.Groups[1].Value;
                //    Style = LouverStyle.ConvertToStyle( match.Groups[2].Value == "LXL");
                //    Width = Convert.ToDouble(match.Groups[3].Value.Replace("L", ""));
                //    Length = Convert.ToDouble(match.Groups[4].Value.Replace("L", ""));
                //    Set = SetID.ConvertStringToSetId( match.Groups[5].Value.Replace("L", ""));
                //}
                string pattern = @"^(PST1\sP1)\/(LXL)\/(L\d+\.\d+)\/(L\d+\.\d+)\/(LB|LT)$";
                Match match = Regex.Match(b.Barcode2, pattern);

                if (match.Success)
                {
                    PanelID = match.Groups[1].Value;
                    Style = LouverStyle.ConvertToStyle(match.Groups[2].Value == "LXL");
                    Width = Convert.ToDouble(match.Groups[3].Value.Replace("L", ""));
                    Length = Convert.ToDouble(match.Groups[4].Value.Replace("L", ""));
                    // Assuming you want to handle the last group as the Set ID
                    Set = SetID.ConvertStringToSetId(match.Groups[5].Value); // The last group will be "LB" or "LT"
                }
                else
                {
                    throw new FormatException("The barcode1 is not in the expected format.");
                }
            }
            catch (Exception)
            {

                throw new FormatException("Error parsing barcode1 data");
            }

            //string pattern = @"(?<Order>\d{8})(?<Line>\d{3})(?<Unit>\d{5}[A-Z]\d)";
            try
            {
                string pattern = @"(?<Order>\d{8})(?<Line>\d{3})(?<Unit>\d{5})(?<L1>L\d)";
                Match match = Regex.Match(b.Barcode1, pattern);

                if (match.Success)
                {
                    Order = Convert.ToDouble(match.Groups["Order"].Value);
                    Line = Convert.ToDouble(match.Groups["Line"].Value);
                    Unit = Convert.ToDouble(match.Groups["Unit"].Value);
                    var L1 = match.Groups["L1"].Value;
                }
                else
                {
                    throw new FormatException("The barcode2 is not in the expected format.");
                }
            }
            catch (Exception)
            {
                throw new FormatException("Error parsing barcode2 data");
            }


        }
    }
}
