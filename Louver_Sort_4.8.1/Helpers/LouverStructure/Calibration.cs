using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    internal class Calibration
    {
        private double _flatReading = 1;
        private double _stepReading = 1.1;
        private double _stepValue = 0.0393701;
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

        public double StepValue
        {
            get { return _stepValue; }
            set { _stepValue = value; }
        }

        public double Slope
        {
            get
            { 
                return (-1/StepValue)/(StepReading-FlatReading); 
            }
        }

        public double ConvertVoltageToDistance(double voltage)
        {
            return Slope * (voltage - FlatReading);
        }




        //private double _slope => (_stepValue - 0) / (_stepReading - _flatReading);
        //private double _intecept => 0 - _slope * _flatReading;

        //public double FlatReading
        //{
        //    get { return _flatReading; }
        //    set { _flatReading = value; }
        //}

        //public double StepReading
        //{
        //    get { return _stepReading; }
        //    set { _stepReading = value; }
        //}

        //// Convert voltage to distance using linear interpolation
        //public double ConvertVoltageToDistance(double voltage)
        //{
        //    return _slope * voltage + _intecept;
        //}

    }
}
