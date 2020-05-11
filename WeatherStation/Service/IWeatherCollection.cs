using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using WeatherStation.Models;

namespace WeatherStation.Service
{
    public interface IWeatherCollection
    {
        Task<IEnumerable<Weather>> GetAllForecasts();
        Task<List<Weather>> GetForecastsForGivenDate(DateTime date);
        Task<List<Weather>> GetForecastsBetweenInterval(DateTime start, DateTime end);
        Task CreateWeatherForecast(Weather weather, Location location);
    }
}
