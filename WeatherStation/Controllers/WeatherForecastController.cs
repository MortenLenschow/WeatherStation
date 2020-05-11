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

        private DbContext _context;

        public WeatherForecastController(DbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Weather>>> GetAll()
        {
            return await _context.GetForecasts();
        }
    }
}
