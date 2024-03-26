using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Louver_Sort_4._8._1.Helpers
{
    internal class LouverSet
    {
        private string _barcode1;
        private string _barcode2;
        private string _order;
        private string _line;
        private string _unit;
        private string _panelID;
        private string _setID;
        private DateTime _dateSorted;
        private bool _xl;
        private double _width;
        private double _length;
        private double _louverCount;
        private List<Louver> _louverList;

        #region Constructor
        public LouverSet()
        {
            _louverList = new List<Louver>();
        }

        public LouverSet(string barcode1, string barcode2, string order, string line, string unit, string panelID, string setID, DateTime dateSorted)
            : this()
        {
            _barcode1 = barcode1;
            _barcode2 = barcode2;
            _order = order;
            _line = line;
            _unit = unit;
            _panelID = panelID;
            _setID = setID;
            _dateSorted = dateSorted;
        }
        #endregion

        #region Properities
        public double LouverCount => _louverCount;

        public string Barcode1
        {
            get => _barcode1;
            set => _barcode1 = value;
        }

        public string Barcode2
        {
            get => _barcode2;
            set => _barcode2 = value;
        }

        public string Order
        {
            get => _order;
            set => _order = value;
        }

        public string Line
        {
            get => _line;
            set => _line = value;
        }

        public string Unit
        {
            get => _unit;
            set => _unit = value;
        }

        public string PanelID
        {
            get => _panelID;
            set => _panelID = value;
        }

        public string SetID
        {
            get => _setID;
            set => _setID = value;
        }

        public DateTime DateSorted
        {
            get => _dateSorted;
            set => _dateSorted = value;
        }

        public bool XL
        {
            get => _xl;
            set => _xl = value;
        }

        public List<Louver> LouverList
        {
            get => _louverList;
            set => _louverList = value;
        }

        public double Width
        {
            get => _width;
            set => _width = value;
        }

        public double Length
        {
            get => _length;
            set => _length = value;
        }
        #endregion

        #region Methods
        public void AssignFromBarcode2(string barcode)
        {
            try
            {
                string pattern = @"^(PST1\sP1)\/(LXL)\/(L\d+\.\d+)\/(L\d+\.\d+)\/(LT)$";
                Match match = Regex.Match(barcode, pattern);

                if (match.Success)
                {
                    Barcode1 = barcode;
                    PanelID = match.Groups[1].Value;
                    XL = match.Groups[2].Value == "LXL";
                    Width = Convert.ToDouble(match.Groups[3].Value.Replace("L", ""));
                    Length = Convert.ToDouble(match.Groups[4].Value.Replace("L", ""));
                    SetID = match.Groups[5].Value.Replace("L", "");
                }
                else
                {
                    throw new FormatException("The barcode is not in the expected format.");
                }
            }
            catch (Exception)
            {

                throw new FormatException("Error parsing barcode data");
            }

        }

        public void AssignFromBarcode1(string barcode)
        {
            try
            {
                string pattern = @"(?<Order>\d{8})(?<Line>\d{3})(?<Unit>\d{5}[A-Z]\d)";
                Match match = Regex.Match(barcode, pattern);

                if (match.Success)
                {
                    Order = match.Groups["Order"].Value;
                    Line = match.Groups["Line"].Value;
                    Unit = match.Groups["Unit"].Value;
                }
                else
                {
                    throw new FormatException("The barcode is not in the expected format.");
                }
            }
            catch (Exception)
            {
                throw new FormatException("Error parsing barcode data");
            }
        }
        #endregion

    }
}
