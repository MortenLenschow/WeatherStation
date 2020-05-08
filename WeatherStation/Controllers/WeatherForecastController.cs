using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeatherStation.Models;
using WeatherStation.Service;

namespace WeatherStation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private DbContext _context;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IDbContextSettings context, IWeatherCollection weatherCollection)
        {
            _logger = logger;

            _context = new DbContext(context, weatherCollection);
        }

        [HttpGet]
        public async ActionResult<List<Weather>> Get()
        {
            return await _context.GetForecasts();
        }
    }
}
