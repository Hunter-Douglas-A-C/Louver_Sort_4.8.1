using System;
using System.Collections.Generic;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    public class Opening
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

        public readonly int _line;
        public readonly LouverModels _modelNum;
        public readonly LouverStructure.LouverStyle.LouverStyles _style;
        public readonly double _width;
        public readonly double _length;
        public readonly List<Panel> _panels = new List<Panel>();

        public int Line => _line;

        public LouverModels ModelNum => _modelNum;

        public LouverStructure.LouverStyle.LouverStyles Style => _style;

        public double Width => _width;

        public double Length => _length;

        public List<Panel> Panels => _panels;

        public Opening(int line, LouverStructure.LouverStyle.LouverStyles style, double width, double length)
        {
            _line = line;
            _style = style;
            _width = width;
            _length = length;
            _modelNum = AssignModelNum();
        }

        private LouverModels AssignModelNum()
        {
            var combinedInput = $"{Style}:{Width}";
            switch (combinedInput)
            {
                case "Standard:2.5":
                    return LouverModels.MSL01;
                case "Standard:3.5":
                    return LouverModels.MSL02;
                case "Standard:4.5":
                    return LouverModels.MSL03;
                case "XL:2.5":
                    return LouverModels.MSL04;
                case "XL:3.5":
                    return LouverModels.MSL05;
                case "XL:4.5":
                    return LouverModels.MSL06;
                default:
                    throw new System.ArgumentException("Invalid combination of style and width");
            }
        }

        public Panel AddPanel(Panel p)
        {
            Panels.Add(p);
            return Panels[Panels.Count - 1];
        }


        public void RemovePanel(Panel p)
        {
            Panels.Remove(p);
        }

        public Panel GetPanel(int Id)
        {
            foreach (Panel panel in Panels)
            {
                if (panel.ID == Id)
                {
                    return panel;
                }
            }
            throw new ArgumentException("Panel with the specified ID not found.", nameof(Id));
        }
    }
}
