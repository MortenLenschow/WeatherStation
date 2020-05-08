using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using WeatherStation.Models;

namespace WeatherStation.Service
{
    public interface IDbConfig
    {
        Task Seed();
        IMongoCollection<Weather> GetCollection();
    }
}
