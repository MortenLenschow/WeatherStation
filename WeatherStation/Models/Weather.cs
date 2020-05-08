using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherStation.Models
{
    public class Weather
    {
        public DateTime date { get; set; }
        public int TemperatureC { get; set; }
        public string Summary { get; set; }
        public int Humidity { get; set; }
        public double AirPressure { get; set; }
    }
}
