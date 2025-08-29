using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementTask6.Domain.DTOs;
using SchoolManagementTask6.Domain.Students;
using SchoolManagementTask6.Persistence;

namespace SchoolManagementTask6.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, Lecturer")] // Only Admin can manage Students
    public class StudentController : ControllerBase
    {
        private readonly SchoolManagementTask6DbContext _context;

        public StudentController(SchoolManagementTask6DbContext context)
        {
            _context = context;
        }

        // CREATE Student
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<StudentResponse>> CreateStudent([FromBody] CreateStudentRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Prevent duplicate MatricNumber
            if (await _context.Students.AnyAsync(s => s.MatricNumber == request.MatricNumber))
                return BadRequest(new { message = "Matric number already exists" });

            var student = new Student
            {
                UserId = request.UserId,
                MatricNumber = request.MatricNumber,
                Status = Enum.Parse<AdmissionStatus>(request.AdmissionStatus, true),
                DepartmentId = request.DepartmentId
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            var response = await _context.Students
                .Include(s => s.User)
                .Include(s => s.Department)
                .Where(s => s.Id == student.Id)
                .Select(s => new StudentResponse
                {
                    Id = s.Id,
                    MatricNumber = s.MatricNumber,
                    AdmissionStatus = s.Status.ToString(),
                    DepartmentName = s.Department.Name,
                    CreatedDate = s.CreatedAt,
                    UpdatedDate = s.UpdatedAt,
                    FullName = s.User.FirstName + " " + s.User.LastName
                })
                .FirstOrDefaultAsync();

            return CreatedAtAction(nameof(GetStudentById), new { id = response.Id }, response);
        }

        // GET all Students
        [HttpGet]
        public async Task<ActionResult<List<StudentResponse>>> GetStudents()
        {
            var students = await _context.Students
                .Include(s => s.User)
                .Include(s => s.Department)
                .Select(s => new StudentResponse
                {
                    Id = s.Id,
                    MatricNumber = s.MatricNumber,
                    AdmissionStatus = s.Status.ToString(),
                    DepartmentName = s.Department.Name,
                    CreatedDate = s.CreatedAt,
                    UpdatedDate = s.UpdatedAt,
                    FullName = s.User.FirstName + " " + s.User.LastName
                })
                .ToListAsync();

            return Ok(students);
        }

        // GET Student by Id
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentResponse>> GetStudentById(int id)
        {
            var student = await _context.Students
                .Include(s => s.User)
                .Include(s => s.Department)
                .Where(s => s.Id == id)
                .Select(s => new StudentResponse
                {
                    Id = s.Id,
                    MatricNumber = s.MatricNumber,
                    AdmissionStatus = s.Status.ToString(),
                    DepartmentName = s.Department.Name,
                    CreatedDate = s.CreatedAt,
                    UpdatedDate = s.UpdatedAt,
                    FullName = s.User.FirstName + " " + s.User.LastName
                })
                .FirstOrDefaultAsync();

            if (student == null) return NotFound();

            return Ok(student);
        }

        // UPDATE Student
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] CreateStudentRequest request)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            // Prevent duplicate MatricNumber for other students
            if (await _context.Students.AnyAsync(s => s.MatricNumber == request.MatricNumber && s.Id != id))
                return BadRequest(new { message = "Matric number already exists for another student" });

            student.UserId = request.UserId;
            student.MatricNumber = request.MatricNumber;
            student.Status = Enum.Parse<AdmissionStatus>(request.AdmissionStatus, true);
            student.DepartmentId = request.DepartmentId;
            student.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE Student
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

