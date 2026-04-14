using DRRest3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DRRest3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        // Hardcoded brugere (admin + almindelig bruger)
        private static readonly List<(string Username, string Password, string Role)> _users = new()
        {
            ("admin", "admin123", "Admin"),
            ("user", "user123", "User")
        };

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        // POST api/auth/login
        [HttpPost("login")]
        public ActionResult<object> Login([FromBody] User loginRequest)
        {
            var user = _users.FirstOrDefault(u =>
                u.Username == loginRequest.Username && u.Password == loginRequest.Password);

            if (user == default)
                return Unauthorized("Forkert brugernavn eller adgangskode");

            var token = GenerateToken(user.Username, user.Role);
            return Ok(new { token });
        }

        private string GenerateToken(string username, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
