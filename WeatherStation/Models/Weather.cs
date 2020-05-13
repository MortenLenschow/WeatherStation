using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WeatherStation.Models
{
    public class Weather
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string WeatherId { get; set; }
        public DateTime Date { get; set; }
        public double TemperatureC { get; set; }
        public string Summary { get; set; }
        public int Humidity { get; set; }
        public double AirPressure { get; set; }
        public string LocationId { get; set; }
    }
}
