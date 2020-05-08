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

        public DbContext(IDbContextSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            var dbConfig = new DbConfig(_Weather, settings, database);

            dbConfig.GetCollection();

            //One way to be able to call async function in constructor
            //Might cause problems
            Task.Run(() => dbConfig.Seed()).Wait();
        }

        

        
    }
}

