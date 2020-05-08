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

        public async Task Seed()
        {
            WeatherCollection wc = new WeatherCollection(_weatherCollection);

            await wc.CreateWeatherForecast(
                new Weather
                {
                    AirPressure = 20.0,
                    date = DateTime.Now,
                    Humidity = 10,
                    TemperatureC = 23,
                    Summary = "Guttes Sonne"
                },
                new Location
                {
                    City = "Esbjerg",
                    Latitude = 10.0,
                    Longitude = 10.0
                });
        }
    }
}
