using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementTask6.Domain.DTOs;
using SchoolManagementTask6.Domain.UserManager;
using SchoolManagementTask6.Persistence;

namespace SchoolManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class UserController : ControllerBase
    {
        private readonly SchoolManagementTask6DbContext _context;

        public UserController(SchoolManagementTask6DbContext context)
        {
            _context = context;
        }

        // CREATE User
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateUser([FromBody] UserRegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Prevent duplicate emails
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return BadRequest(new { message = "Email already exists" });
            }
            var user = new User
            {
                Email = request.Email,
                PasswordHash = request.PasswordHash, // hash password in real scenario
                FirstName = request.FirstName,
                LastName = request.LastName,
                Role = request.Role,
                Gender = request.Gender
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        // GET User by Id
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userData = new UserResponseDto
            {
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
            };

            return Ok(userData);
        }

        // GET All Users
        [HttpGet]
        public async Task<ActionResult<List<UserResponseDto>>> GetAllUsers()
        {
            var users = _context.Users.Select(user => new UserResponseDto 
            {
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                
            });

            var response = await users.ToListAsync();

            return Ok(response);
        }

        // UPDATE User
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserRegistrationRequest request)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            // Prevent duplicate emails for other users
            var isEmailExist = await _context.Users.AnyAsync(u => u.Email == request.Email && u.Id != id);
            if (isEmailExist)
            {
                return BadRequest(new { message = "Email already exists for another user" });
            }
            user.Email = request.Email;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Role = request.Role;
            user.Gender = request.Gender;
            user.PasswordHash = request.PasswordHash; // hash in real scenario

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE User
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = $"User with id {id} not found" });
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
