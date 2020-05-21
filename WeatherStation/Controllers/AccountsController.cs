using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using WeatherStation.Models;
using WeatherStation.Models.DTO;
using WeatherStation.Service;
using static BCrypt.Net.BCrypt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace WeatherStation.Controllers
{
    /// <summary>
    /// Use this API to login and change password.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private DbContext _context;

        public AccountsController(DbContext context)
        {
            _context = context;
        }

        // POST: api/Accounts
        [HttpPost]
        public async Task PostAccount(Login login)
        {
            await _context.CreateAccount(login);
        }

        // POST: api/Accounts/login
        [HttpPost("login"), AllowAnonymous]
        public async Task<ActionResult<Token>> Login(Login login)
        {
            return await _context.LoginAccount(login);
        }

        // POST: api/accounts/weather
        [HttpPost("weather")]
        public async Task PostForecast(Weather weather)
        {
            await _context.CreateForecast(weather);
        }
    }
}
