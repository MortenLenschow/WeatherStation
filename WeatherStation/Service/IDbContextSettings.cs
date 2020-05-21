using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherStation.Service
{
    public interface IDbContextSettings
    {
        string WeatherCollectionName { get; set; }
        string LocationCollectionName { get; set; }
        string AccountCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
