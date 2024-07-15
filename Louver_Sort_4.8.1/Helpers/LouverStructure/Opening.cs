using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    /// <summary>
    /// Represents an opening with louvers.
    /// </summary>
    [Serializable]
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

        private LouverModels _modelNum;
        private LouverStyle.LouverStyles _style;
        private double _width;
        private double _length;
        private List<Panel> _panels = new List<Panel>();
        private int _line;

        [JsonProperty("modelNum")]
        public LouverModels ModelNum { get => _modelNum; set => _modelNum = value; }

        [JsonProperty("style")]
        public LouverStyle.LouverStyles Style { get => _style; set => _style = value; }

        [JsonProperty("width")]
        public double Width { get => _width; set => _width = value; }

        [JsonProperty("length")]
        public double Length { get => _length; set => _length = value; }

        [JsonProperty("line")]
        public int Line { get => _line; set => _line = value; }

        [JsonProperty("panels")]
        public List<Panel> Panels
        {
            get => _panels;
            set => _panels.AddRange(value);
        }



















        // Constructor
        /// <summary>
        /// Initializes a new instance of the Opening class.
        /// </summary>
        /// <param name="line">The line number.</param>
        /// <param name="style">The louver style.</param>
        /// <param name="width">The width of the opening.</param>
        /// <param name="length">The length of the opening.</param>
        public Opening(int line, LouverStyle.LouverStyles style, double width, double length)
        {
            _line = line;
            _style = style;
            _width = width;
            _length = length;
            _modelNum = AssignModelNum();
        }

        // Methods
        /// <summary>
        /// Assigns a louver model number based on the style and width of the opening.
        /// </summary>
        /// <returns>The assigned louver model number.</returns>
        private LouverModels AssignModelNum()
        {
            var combinedInput = $"{_style}:{_width}";
            switch (combinedInput)
            {
                case "Standard:2.5":
                case "Standard:25":
                    return LouverModels.MSL01;
                case "Standard:3.5":
                case "Standard:35":
                    return LouverModels.MSL02;
                case "Standard:4.5":
                case "Standard:45":
                    return LouverModels.MSL03;
                case "XL:2.5":
                case "XL:25":
                    return LouverModels.MSL04;
                case "XL:3.5":
                case "XL:35":
                    return LouverModels.MSL05;
                case "XL:4.5":
                case "XL:45":
                    return LouverModels.MSL06;
                default:
                    throw new ArgumentException("Invalid combination of style and width");
            }
        }

        /// <summary>
        /// Adds a panel to the opening.
        /// </summary>
        /// <param name="p">The panel to add.</param>
        /// <returns>The added panel.</returns>
        public Panel AddPanel(Panel p)
        {
            _panels.Add(p);
            return _panels[_panels.Count - 1];
        }

        /// <summary>
        /// Removes a panel from the opening.
        /// </summary>
        /// <param name="p">The panel to remove.</param>
        public void RemovePanel(Panel p)
        {
            _panels.Remove(p);
        }

        /// <summary>
        /// Gets a panel from the opening by its ID.
        /// </summary>
        /// <param name="id">The ID of the panel.</param>
        /// <returns>The panel with the specified ID.</returns>
        public Panel GetPanel(int id)
        {
            foreach (Panel panel in _panels)
            {
                if (panel.ID == id)
                {
                    return panel;
                }
            }
            throw new ArgumentException("Panel with the specified ID not found.", nameof(id));
        }
    }
}
