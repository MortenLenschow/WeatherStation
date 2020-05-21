using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WeatherStation.Hubs;
using WeatherStation.Models;
using WeatherStation.Service;

namespace WeatherStation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        private DbContext _context;
        private readonly IHubContext<WeatherHub> _weatherHubContext;

        public WeatherForecastController(DbContext context, IHubContext<WeatherHub> weatherHubContext)
        {
            _context = context;
            _weatherHubContext = weatherHubContext;
        }

        // GET: api/weatherforecast
        [HttpGet]
        public async Task<IEnumerable<Weather>> GetAll()
        {
            return await _context.GetAllForecasts();
        }

        // GET: api/weatherforecast/latest
        [HttpGet("latest")]
        public async Task<IEnumerable<Weather>> GetForecastsLatest()
        {
            return await _context.GetForecastsLatest();
        }

        // GET: api/weatherforecast/2020-05-xxT22:00:00Z
        [HttpGet("{date:DateTime}")]
        public async Task<IEnumerable<Weather>> GetForecastsForGivenDate(DateTime date)
        {
            return await _context.GetForecastsForGivenDate(date);
        }

        // GET: api/weatherforecast/2020-05-xxT22:00:00Z/2020-05-xxT22:00:00Z
        [HttpGet("{start:DateTime}/{end:DateTime}")]
        public async Task<IEnumerable<Weather>> GetForecastsForGivenInterval(DateTime start, DateTime end)
        {
            return await _context.GetForecastsForGivenInterval(start, end);
        }

        [HttpGet("update")]
        public async Task<IActionResult> ReturnUpdatedForecast()
        {
            await _weatherHubContext.Clients.All.SendAsync("weatherUpdate", _context.ReturnUpdatedForecast());
            return Ok();
        }
    }
}
