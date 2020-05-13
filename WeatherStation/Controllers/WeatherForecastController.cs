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
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        private DbContext _context;

        public WeatherForecastController(DbContext context)
        {
            _context = context;
        }

        // GET: api/Vejrobservationer
        [HttpGet]
        public async Task<ActionResult<List<Weather>>> GetAll()
        {
            return await _context.GetAllForecasts();
        }

        // GET: api/Vejrobservationer/2020-01-01T00:00:00
        [HttpGet("{date:DateTime}")]
        public async Task<ActionResult<List<Weather>>> GetWeather(DateTime date)
        {
            return await _context.GetForecastsForGivenDate(date);
        }
    }
}
