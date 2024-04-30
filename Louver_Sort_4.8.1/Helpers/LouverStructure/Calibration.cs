using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    internal class Calibration
    {
        private double _flatReading;
        private double _stepReading;
        private double _stepValue = 0.25;
        private double _slope => (_stepValue - 0) / (_stepReading - _flatReading);
        private double _intecept => 0 - _slope * _flatReading;

        public double FlatReading
        {
            get { return _flatReading; }
            set { _flatReading = value; }
        }

        public double StepReading
        {
            get { return _stepReading; }
            set { _stepReading = value; }
        }

        // Convert voltage to distance using linear interpolation
        public double ConvertVoltageToDistance(double voltage)
        {
            return _slope * voltage + _intecept;
        }

    }
}
