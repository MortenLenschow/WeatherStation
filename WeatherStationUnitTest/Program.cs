using Microsoft.AspNetCore.SignalR;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using WeatherStation.Controllers;
using WeatherStation.Hubs;
using WeatherStation.Models;
using WeatherStation.Service;

namespace WeatherStationUnitTest
{
    [TestFixture]
    public class WeatherStationUnitTest
    {
        private DbContext _dbcontext;
        private readonly IHubContext<WeatherHub> _weatherHubContext;

        public WeatherStationUnitTest()
            {
            _dbcontext = new DbContext(new DbContextSettings());
            _weatherHubContext = new HubContext<WeatherHub>();
            }

        [Test]
        public void TestGetAll()
        {
            var controller = new WeatherForecastController(_dbcontext,);
        }
    }
}


[TestFixture]
public class ProductControllerTest
{
    [Test]
    public void TestDetailsView()
    {
        var controller = new ProductController();
        var result = controller.Details(2) as ViewResult;
        Assert.AreEqual("Details", result.ViewName);

    }
}