using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using WeatherStation.Models;

namespace WeatherStation.Service
{
    public class DbContext
    {
        private IMongoCollection<Weather> _Weather;
        private IMongoCollection<Location> _Location;

        public DbContext(IDbContextSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
        }
    }
}
