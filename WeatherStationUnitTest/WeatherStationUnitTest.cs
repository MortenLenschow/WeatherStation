using DnsClient.Internal;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly ILogger<WeatherForecastController> _logger;
        private WeatherForecastController uut;

        [SetUp]
        public void Setup()
        {
            _dbcontext = new DbContext();
            uut = new WeatherForecastController(_dbcontext);
        }

        [Test]
        public async Task TestGetAll()
        {
            var obj = await uut.GetAll();
            Assert.That(obj.Count(), Is.GreaterThan(0));
        }

        [Test]
        public async Task TestGet3LatestForecasts()
        {
            var obj = await uut.GetForecastsLatest();
            Assert.That(obj.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task GetForecastsForGivenDate()
        {
            var obj = await uut.GetForecastsForGivenDate(DateTime.Today);
            Assert.That(obj.Count(), Is.GreaterThan(0));
        }

        [Test]
        public async Task GetForecastsForGivenInterval()
        {
            var obj = await uut.GetForecastsForGivenInterval(DateTime.Today, DateTime.Today.AddDays(2));
            Assert.That(obj.Count(), Is.GreaterThan(0));
        }
    }
}
