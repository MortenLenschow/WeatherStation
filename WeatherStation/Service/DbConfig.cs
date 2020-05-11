using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using WeatherStation.Models;

namespace WeatherStation.Service
{
    public class DbConfig
    {
        private IMongoCollection<Weather> _weatherCollection;
        private IDbContextSettings _settings;
        private IMongoDatabase _database;

        public DbConfig(IMongoCollection<Weather> weather, IDbContextSettings settings, IMongoDatabase database)
        {
            _weatherCollection = weather;
            _settings = settings;
            _database = database;

        }

        //Fills the Collections with data from database
        public IMongoCollection<Weather> GetCollection()
        {
            _weatherCollection =  _database.GetCollection<Weather>(_settings.WeatherCollectionName);
            return _weatherCollection;
        }

        
    }
}
