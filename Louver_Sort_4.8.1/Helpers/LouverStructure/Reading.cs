using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{

    public  class Reading
    {
        private double? _reading1;
        private double? _reading2;
        private DateTime _dateReading1;
        private DateTime _dateReading2;


        [JsonProperty("reading1")]
        public double? Reading1 { get => _reading1; set => _reading1 = value; }

        [JsonProperty("reading2")]
        public double? Reading2 { get => _reading2; set => _reading2 = value; }

        [JsonProperty("dateReading1")]
        public DateTime DateReading1 { get => _dateReading1; set => _dateReading1 = value; }

        [JsonProperty("DateReading2")]
        public DateTime DateReading2 { get => _dateReading2; set => _dateReading2 = value; }


        public void SetReading1(double? r1)
        { 
            _reading1 = r1; 
            _dateReading1 = DateTime.Now;
        }

        public void SetReading2(double? r2)
        {
            _reading2 = r2;
            _dateReading2 = DateTime.Now;
        }
    }
}
