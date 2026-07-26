using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JobContext _jobContext;
        private readonly IConfiguration _configuration;

        public AuthController(JobContext jobContext, IConfiguration configuration)
        {
            _jobContext = jobContext;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            var existingUser = _jobContext.Users.FirstOrDefault(u => u.Email == registerDTO.Email);

            if (existingUser != null) return BadRequest("User with this email already exists.");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password);

            var user = new User
            {
                Email = registerDTO.Email,
                passwordHash = passwordHash
            };
            _jobContext.Users.Add(user);
            await _jobContext.SaveChangesAsync();

            return Ok("User Registered Successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LogiDTO loginDTO)
        {
            var user = _jobContext.Users.FirstOrDefault(u => u.Email == loginDTO.Email);

            if (user == null) return Unauthorized("Invalid email or password");

            //Verify Password
            var isValidPassword = BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.passwordHash);

            if(!isValidPassword) return Unauthorized("Invalid email or password");

            //Generate JWT Token
            var token = GenerateToken(user);

            return Ok(new { Token = token });
        }

        private string GenerateToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JWTSettings");
            var secretKey = jwtSettings.GetValue<string>("SecretKey");

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryInMinutes"])),
                signingCredentials: cred
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
