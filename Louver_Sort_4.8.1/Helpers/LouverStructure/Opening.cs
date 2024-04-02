using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    internal class Opening : LouverStyle
    {
        public enum LouverModels
        {
            MSL01,
            MSL02,
            MSL03,
            MSL04,
            MSL05,
            MSL06
        }



        private double _line;
        private LouverModels _modelNum;
        private LouverStyles _style;
        private double _width;
        private double _length;
        private List<Panel> _panels = new List<Panel>();

        public double Line
        {
            get { return _line; }
            set { _line = value; }
        }

        public LouverModels ModelNum
        {
            get { return _modelNum; }
            set { _modelNum = value; }
        }

        public LouverStyles Style
        {
            get { return _style; }
            set { _style = value; }
        }

        public double Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public double Length
        {
            get { return _length; }
            set { _length = value; }
        }

        public List<Panel> Panels
        {
            get { return _panels; }
            set { _panels = value; }
        }

        public Opening(double line, LouverModels modelNum, LouverStyles style, double width, double length)
        {
            Line = line;
            ModelNum = modelNum;
            Style = style;
            Width = width;
            Length = length;
        }

















        private void AssignModelNum()
        {
            var combinedInput = $"{Style}:{Width}";
            switch (combinedInput)
            {
                case ("Standard:2.5"):
                    ModelNum = LouverModels.MSL01;
                    break;
                case ("Standard:3.5"):
                    ModelNum = LouverModels.MSL02;
                    break;
                case ("Standard:4.5"):
                    ModelNum = LouverModels.MSL03;
                    break;
                case ("XL:2.5"):
                    ModelNum = LouverModels.MSL04;
                    break;
                case ("XL:3.5"):
                    ModelNum = LouverModels.MSL05;
                    break;
                case ("XL:4.5"):
                    ModelNum = LouverModels.MSL06;
                    break;
                default:
                    break;
            }
        }



        public void AddPanel(Panel p)
        {
            Panels.Add(p);
        }
    }
}
