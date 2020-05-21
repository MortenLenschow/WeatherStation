using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherStation.Service
{
    public class DbContextSettings : IDbContextSettings
    {
        public string WeatherCollectionName { get; set; }
        public string LocationCollectionName { get; set; }
        public string AccountCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
