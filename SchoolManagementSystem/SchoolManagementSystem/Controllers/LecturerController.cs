using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementTask6.Domain.DTOs;
using SchoolManagementTask6.Domain.Lecturers;
using SchoolManagementTask6.Persistence;

namespace SchoolManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, Lecturer")]
    public class LecturerController : ControllerBase
    {
        private readonly SchoolManagementTask6DbContext _context;

        public LecturerController(SchoolManagementTask6DbContext context)
        {
            _context = context;
        }

        // GET all Lecturers
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<LecturerResponse>>> GetLecturers()
        {
            var lecturers = await _context.Lecturers
                .Include(l => l.User)
                .Include(l => l.Department)
                .Select(l => new LecturerResponse
                {
                    Id = l.Id,
                    FullName = l.User.FirstName + " " + l.User.LastName,  
                    DepartmentName = l.Department.Name
                })
                .ToListAsync();

            return Ok(lecturers);
        }

        // GET Lecturer by Id
        [HttpGet("{id}")]
        public async Task<ActionResult<LecturerResponse>> GetLecturer(int id)
        {
            var lecturer = await _context.Lecturers
                .Include(l => l.User)
                .Include(l => l.Department)
                .Where(l => l.Id == id)
                .Select(l => new LecturerResponse
                {
                    Id = l.Id,
                    FullName = l.User.FirstName + " " + l.User.LastName,   
                    DepartmentName = l.Department.Name
                })
                .FirstOrDefaultAsync();

            if (lecturer == null)
            {
                return NotFound();
            }
            return Ok(lecturer);
        }

        // POST create Lecturer
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<LecturerResponse>> CreateLecturer(CreateLecturerRequest request)
        {
            var lecturer = new Lecturer
            {
                UserId = request.UserId,
                DepartmentId = request.DepartmentId
            };

            _context.Lecturers.Add(lecturer);
            await _context.SaveChangesAsync();

            var result = _context.Lecturers
                .Include(l => l.User)
                .Include(l => l.Department)
                .Where(l => l.Id == lecturer.Id)
                .Select(l => new LecturerResponse
                {
                    Id = l.Id,
                    FullName = l.User.FirstName + " " + l.User.LastName,   
                    DepartmentName = l.Department.Name
                });

            var response = await result.FirstOrDefaultAsync();

            return CreatedAtAction(nameof(GetLecturer), new { id = response.Id }, response);
        }

        // PUT update Lecturer
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateLecturer(int id, CreateLecturerRequest request)
        {
            var lecturer = await _context.Lecturers.FindAsync(id);
            if (lecturer == null)
            {
                return NotFound();
            }

            lecturer.UserId = request.UserId;
            lecturer.DepartmentId = request.DepartmentId;

            await _context.SaveChangesAsync();
            return Ok();
        }

        // DELETE Lecturer
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteLecturer(int id)
        {
            var lecturer = await _context.Lecturers.FindAsync(id);
            if (lecturer == null)
            {
                return NotFound();
            }
            _context.Lecturers.Remove(lecturer);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
   
}
