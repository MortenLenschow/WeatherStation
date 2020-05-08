using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherStation.Models;

namespace WeatherStation.Service
{
    public class WeatherCollection : IWeatherCollection
    {
        private IMongoCollection<Weather> _weatherCollection;

        public WeatherCollection(IMongoCollection<Weather> weatherCollection)
        {
            _weatherCollection = weatherCollection;
        }

        //Returns all weather forecasts
        public async Task<List<Weather>> GetAllForecasts() =>
            await _weatherCollection.Find(_ => true).ToListAsync();

        //Returns all weather forecasts for a given date
        public async Task<List<Weather>> GetForecastsForGivenDate(DateTime date) =>
            await _weatherCollection.Find(weather => weather.date == date).ToListAsync();

        //Returns all weather forecasts between a start date and an end date
        public async Task<List<Weather>> GetForecastsBetweenInterval(DateTime start, DateTime end) =>
            await _weatherCollection.Find(day => day.date >= start && day.date <= end).ToListAsync();

        //Add weather forecast
        public async Task CreateWeatherForecast(Weather weather, Location location)
        {
            var item = new Weather
            {
                date = DateTime.Now,
                AirPressure = weather.AirPressure,
                Humidity = weather.Humidity,
                Location = weather.Location,
                Summary = weather.Summary,
                TemperatureC = weather.TemperatureC
            };

            await _weatherCollection.InsertOneAsync(item);
            await WeatherAddLocation(item, location);
        }

        //Adds location id to weather forecast
        private async Task WeatherAddLocation(Weather weather, Location location)
        {
            await _weatherCollection.UpdateOneAsync(Builders<Weather>.Filter
                .Eq("Id", weather.Id), Builders<Weather>.Update
                .Push("LocationId", location.LocationId));
        }
    }
}
