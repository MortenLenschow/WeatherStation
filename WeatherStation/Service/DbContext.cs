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

        public DbContext(IDbContextSettings settings, IWeatherCollection collection)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            var dbConfig = new DbConfig(_Weather, settings, database);
            _collection = collection;

            dbConfig.GetCollection();

            //One way to be able to call async function in constructor
            //Might cause problems
            Task.Run(() => dbConfig.Seed()).Wait();
        }

        public async Task<List<Weather>> GetForecasts()
        {
            return await _collection.GetAllForecasts();
        }

        public async Task<List<Weather>> GetForecastsForGivenDate(DateTime date)
        {
            return await _collection.GetForecastsForGivenDate(date);
        }

        public async Task<List<Weather>> GetForecastsBetweenInterval(DateTime start, DateTime end)
        {
            return await _collection.GetForecastsBetweenInterval(start, end);
        }

        public async Task CreateWeatherForecast(Weather weather, Location location)
        {
            await _collection.CreateWeatherForecast(weather, location);
        }

    }
}

