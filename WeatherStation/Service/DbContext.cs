using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using WeatherStation.Models;

namespace WeatherStation.Service
{
    public class DbContext
    {
        private IMongoCollection<Weather> _Weather;
        private IWeatherCollection _collection;

        public DbContext(IDbContextSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _Weather = database.GetCollection<Weather>(settings.WeatherCollectionName);
            _collection = new WeatherCollection(_Weather);


            //One way to be able to call async function in constructor
            //Might cause problems
            Task.Run(() => Seed()).Wait();
        }

        public async Task<List<Weather>> GetForecasts()
        {
            return await _collection.GetAllForecasts();
        }

        public async Task<IEnumerable<Weather>> GetForecastsForGivenDate(DateTime date)
        {
            return await _collection.GetForecastsForGivenDate(date);
        }

        public async Task<IEnumerable<Weather>> GetForecastsBetweenInterval(DateTime start, DateTime end)
        {
            return await _collection.GetForecastsBetweenInterval(start, end);
        }

        public async Task CreateWeatherForecast(Weather weather, Location location)
        {
            await _collection.CreateWeatherForecast(weather, location);
        }

        public async Task Seed()
        {
            WeatherCollection wc = new WeatherCollection(_Weather);

            await wc.CreateWeatherForecast(
                new Weather
                {
                    AirPressure = 20.0,
                    Date = DateTime.Now,
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

