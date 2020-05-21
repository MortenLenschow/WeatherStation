using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using WeatherStation.Models;
using System.Web.Script.Serialization;

namespace WeatherStation.Service
{
    public class DbContext
    {
        public Weather CurrentlyAddedWeather { get; set; }
        private IMongoCollection<Weather> _Weather;
        private IMongoCollection<Location> _Location;

        public DbContext(IDbContextSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            DropAllCollections(settings, database);
            GetCollections(settings, database);
            Task.Run(() => Seed()).Wait();
        }

        #region API

        //Returns all weather forecasts
        public async Task<IEnumerable<Weather>> GetAllForecasts()
        {
            return await _Weather.Find(_ => true).ToListAsync();
            //return await _Weather.Find(weather => weather.Date != null).ToListAsync();
        }

        //Returns all weather forecasts for a given date
        public async Task<IEnumerable<Weather>> GetForecastsForGivenDate(DateTime date)
        {
            return await _Weather.Find(weather => weather.Date == date).ToListAsync();
        }

        //Returns all weather forecasts between a start date and an end date
        public async Task<IEnumerable<Weather>> GetForecastsForGivenInterval(DateTime start, DateTime end)
        {
            return await _Weather.Find(day => day.Date >= start && day.Date <= end).ToListAsync();
        }

        //Returns the 3 latest weather forecasts
        public async Task<IEnumerable<Weather>> GetForecastsLatest()
        {
            var list = await _Weather.Find(_ => true).ToListAsync();
            return list.OrderByDescending(weather => weather.Date).Take(3);
        }

        public Location GetLocation(string name) => _Location.Find(location => location.Name == name).FirstOrDefault();

        //Add weather forecast
        public async Task CreateForecast(Weather weather, string cityName)
        {
            CurrentlyAddedWeather = null;
            weather.LocationId = GetLocation(cityName).LocationId;

            await _Weather.InsertOneAsync(weather);
            await _Location.UpdateOneAsync(Builders<Location>.Filter.Eq("Name", cityName),
                Builders<Location>.Update.Push("WeatherId", weather.WeatherId));
            CurrentlyAddedWeather = weather;
        }

        public string ReturnUpdatedForecast()
        {
            var jsonweather = Newtonsoft.Json.JsonConvert.SerializeObject(CurrentlyAddedWeather);
            return jsonweather;
        }

        //Adds location id to weather forecast
        private async Task CreateLocation(Location location)
        {
            await _Location.InsertOneAsync(location);
        }
            #endregion

            #region Seeding/Deleting

            public async Task Seed()
        {
            await CreateLocation(new Location() { Name = "Aarhus", Latitude = 10.203921, Longitude = 56.162939 });
            await CreateForecast(new Weather() { Date = DateTime.Today, TemperatureC = 25.3, Summary = "Konge sommervejr", Humidity = 3, AirPressure = 5.3 }, "Aarhus" );
            await CreateForecast(new Weather() { Date = DateTime.Today.AddDays(1), TemperatureC = 20.7, Summary = "Klar himmel, ingen skyer", Humidity = 5, AirPressure = 7.5 }, "Aarhus" );
            await CreateForecast(new Weather() { Date = DateTime.Today.AddDays(2), TemperatureC = 17.1, Summary = "Let overskyet og mild vind", Humidity = 7, AirPressure = 10.4 }, "Aarhus" );


            await CreateLocation(new Location() { Name = "Esbjerg", Latitude = 55.476466, Longitude = 8.459405 });
            await CreateForecast(new Weather() { Date = DateTime.Today, TemperatureC = 9.2, Summary = "Stærk vind og kraftig overskyet", Humidity = 30, AirPressure = 101.3 }, "Esbjerg" );
            await CreateForecast(new Weather() { Date = DateTime.Today.AddDays(1), TemperatureC = 18, Summary = "Høj luftfugtighed", Humidity = 69, AirPressure = 13.37 }, "Esbjerg" );

        }

        public void GetCollections(IDbContextSettings settings, IMongoDatabase database)
        {
            _Weather = database.GetCollection<Weather>(settings.WeatherCollectionName); //"Weather"
            _Location = database.GetCollection<Location>(settings.LocationCollectionName); //"Location"

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

        #endregion
    }
}

