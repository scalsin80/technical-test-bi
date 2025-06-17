using APIUser.DTOs;
using APIUser.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APIUser.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;
        private readonly dbdataContext _context;


        public AuthController(ILogger<AuthController> logger, IConfiguration configuration, dbdataContext dbdataContext)
        {
            _logger = logger;
            _configuration = configuration;
            _context = dbdataContext;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            try
            {
                var userDb = _context.Users.FirstOrDefault(u =>
                    u.email == userLogin.email && u.password == userLogin.password);

                if (userDb == null)
                    return Unauthorized("Invalid credentials, try again");

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:secretKey"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, userDb.idUser.ToString()),
                        new Claim(ClaimTypes.Email, userDb.email),
                        new Claim(ClaimTypes.Role, userDb.role.ToLower()) 
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return Ok(new { token = tokenHandler.WriteToken(token) });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex.InnerException.ToString());
                _logger.LogError(ex.StackTrace.ToString());

                return Unauthorized();
            }
        }

    }
}
