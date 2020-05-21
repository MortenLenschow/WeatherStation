using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WeatherStation.Models;
using WeatherStation.Models.DTO;
using WeatherStation.Service;
using static BCrypt.Net.BCrypt;

//Code for JWT authentication borrowed from https://github.com/dalleman1/I4NGKassignment3/blob/master/I4NGKassignment3/Controllers/UserController.cs

namespace WeatherStation.Controllers
{
    /// <summary>
    /// Use this API to login and change password.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
            login.Email = login.Email.ToLowerInvariant();
            var account = await _context._Account.Find(acc => acc.Email == login.Email).
                FirstOrDefaultAsync().ConfigureAwait(false);

            if (account != null)
            {
                var validPwd = Verify(login.Password, account.PwHash);
                if (validPwd)
                {
                    var jwt = GenerateToken(account.Email);
                    var token = new Token() { JWT = jwt };
                    return token;

                }
            }
            return null;
        }

        // POST: api/accounts/weather
        [HttpPost("weather")]
        public async Task PostForecast(Weather weather)
        {
            await _context.CreateForecast(weather);
        }

        // api/User/GrantPermission
        [HttpGet]
        [Route("GrantPermission")]
        public IActionResult GrantPermission()
        {
            string value = Request.Headers["Authorization"];

            if (IsTokenValid(value))
            {
                return Ok(value);
            }
            else
            {
                return Unauthorized();
            }
        }

        private string GenerateToken(string email)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
            };

            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes("9523kwdwkewkrewKSADKASD2121ldassadl21321")),
                                             SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool IsTokenValid(string token)
        {
            TokenValidationParameters tokenValidationParameters = GetTokenValidation();

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                ClaimsPrincipal istokenvalid = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedtoken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private TokenValidationParameters GetTokenValidation()
        {
            return new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("9523kwdwkewkrewKSADKASD2121ldassadl21321"))
            };
        }
    }
}
