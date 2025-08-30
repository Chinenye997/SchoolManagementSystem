using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SchoolManagementTask6.Domain.DTOs;
using SchoolManagementTask6.Domain.UserManager;
using SchoolManagementTask6.Persistence;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SchoolManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SchoolManagementTask6DbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(SchoolManagementTask6DbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // implement the register endpoint

        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRegistrationRequest userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // check if the db email is equal to the frontend email
            var isUserExist = await _context.Users.AnyAsync(em => em.Email == userDto.Email);

            if (isUserExist)
            {
                return BadRequest("Email is already in use.");
            }

            var user = new User
            {
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Role = userDto.Role,
                UserStatus = UserStatus.Active,
                Gender = userDto.Gender,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.PasswordHash)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User Registered Successfully.", UserId = user.Id });
        }

        // Implementation for login endpoint
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginRequest loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.PasswordHash))
            {
                return BadRequest("Email and password are required");
            }

            var user = await _context.Users.FirstOrDefaultAsync(em => em.Email == loginRequest.Email);

            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }

            var isPasswordValid = VerifyPassword(loginRequest.PasswordHash, user.PasswordHash!);

            if (!isPasswordValid)
            {
                return Unauthorized("Invalid email or password");
            }

            var token = GenerateJwtToken(user);

            return Ok(new LoginResponseDTO
            {
                Token = token,
                Email = user.Email,
                Id = user.Id,
                Role = user.Role
            });
        }

        private string GenerateJwtToken(User user)
        {
            //Claim
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            // Keys
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["key"]));

            // Credential
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Assign the claims, the key, creds to the token
            var token = new JwtSecurityToken(
                issuer: _configuration["Issuer"],
                audience: _configuration["Audience"],
                claims: claims,
                signingCredentials: creds,
                expires: DateTime.Now.AddHours(1));

            // Final is the JwtSecurity token handler that creates the token of strings
            var newToken = new JwtSecurityTokenHandler().WriteToken(token);

            return newToken;
        }



        private bool VerifyPassword(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(storedHash))
            {
                return false;
            }

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(password, storedHash);

            return isPasswordValid;
        }

    }

}