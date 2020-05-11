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
        public async Task<List<Weather>> GetAllForecasts()
        {
            return await _weatherCollection.Find(_ => true).ToListAsync();
        }
            

        //Returns all weather forecasts for a given date
        public async Task<IEnumerable<Weather>> GetForecastsForGivenDate(DateTime date) =>
            await _weatherCollection.Find(weather => weather.Date == date).ToListAsync();

        //Returns all weather forecasts between a start date and an end date
        public async Task<IEnumerable<Weather>> GetForecastsBetweenInterval(DateTime start, DateTime end) =>
            await _weatherCollection.Find(day => day.Date >= start && day.Date <= end).ToListAsync();

        //Add weather forecast
        public async Task CreateWeatherForecast(Weather weather, Location location)
        {
            var item = new Weather
            {
                Date = DateTime.Now,
                AirPressure = weather.AirPressure,
                Humidity = weather.Humidity,
                LocationId = location.LocationId,
                Summary = weather.Summary,
                TemperatureC = weather.TemperatureC
            };

            await _weatherCollection.InsertOneAsync(item);
            //await WeatherAddLocation(item, location);
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
