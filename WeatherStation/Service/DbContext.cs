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
        private IMongoCollection<Location> _Location;

        public DbContext(IDbContextSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            DropAllCollections(settings, database);
            GetCollections(settings, database);
        }

        public Location GetLocation(string name) => _Location.Find(location => location.Name == name).FirstOrDefault();

        //Returns all weather forecasts
        public async Task<List<Weather>> GetAllForecasts()
        {
            return await _Weather.Find(_ => true).ToListAsync();
        }

        //Returns all weather forecasts for a given date
        public async Task<List<Weather>> GetForecastsForGivenDate(DateTime date)
        {
            return await _Weather.Find(weather => weather.Date == date).ToListAsync();
        }

        //Returns all weather forecasts between a start date and an end date
        public async Task<IEnumerable<Weather>> GetForecastsBetweenInterval(DateTime start, DateTime end) =>
            await _Weather.Find(day => day.Date >= start && day.Date <= end).ToListAsync();

        //Add weather forecast
        public async Task CreateForecast(Weather weather, string cityName)
        {
            weather.LocationId = GetLocation(cityName).LocationId;

            await _Weather.InsertOneAsync(weather);
            await _Location.UpdateOneAsync(Builders<Location>.Filter.Eq("Name", cityName),
                Builders<Location>.Update.Push("WeatherId", weather.WeatherId));
        }

        //Adds location id to weather forecast
        private async Task CreateLocation(Location location)
        {
            await _Location.InsertOneAsync(location);
        }

        public async Task Seed()
        {
            await CreateLocation(new Location() { Name = "Aarhus", Latitude = 10.203921, Longitude = 56.162939 });
            await CreateForecast(new Weather() { Date = DateTime.Now, TemperatureC = 25.3, Summary = "Konge sommervejr", Humidity = 3, AirPressure = 5.3 }, "Aarhus" );
            await CreateForecast(new Weather() { Date = DateTime.Now, TemperatureC = 20.7, Summary = "Klar himmel, ingen skyer", Humidity = 5, AirPressure = 7.5 }, "Aarhus" );
            await CreateForecast(new Weather() { Date = DateTime.Now, TemperatureC = 17.1, Summary = "Let overskyet og mild vind", Humidity = 7, AirPressure = 10.4 }, "Aarhus" );
        }

        public async Task GetCollections(IDbContextSettings settings, IMongoDatabase database)
        {
            _Weather = database.GetCollection<Weather>(settings.WeatherCollectionName); //"Weather"
            _Location = database.GetCollection<Location>(settings.LocationCollectionName); //"Location"

            if (_Weather.Find(_ => true).ToList().Count() <= 0) await Seed();
        }

        public void DropAllCollections(IDbContextSettings settings, IMongoDatabase database)
        {
            try
            {
                database.DropCollection(settings.WeatherCollectionName);
                database.DropCollection(settings.LocationCollectionName);
            }

            catch(Exception ex) { Console.WriteLine($"Exception DropAllCollections: {ex}"); }
        }

        //public async Task<List<Weather>> GetForecasts()
        //{
        //    return await _collection.GetAllForecasts();
        //}

        //public async Task<IEnumerable<Weather>> GetForecastsForGivenDate(DateTime date)
        //{
        //    return await _collection.GetForecastsForGivenDate(date);
        //}

        //public async Task<IEnumerable<Weather>> GetForecastsBetweenInterval(DateTime start, DateTime end)
        //{
        //    return await _collection.GetForecastsBetweenInterval(start, end);
        //}

        //public async Task CreateWeatherForecast(Weather weather, Location location)
        //{
        //    await _collection.CreateWeatherForecast(weather, location);
        //}

    }
}

