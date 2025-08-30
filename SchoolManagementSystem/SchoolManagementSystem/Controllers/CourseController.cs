using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementTask6.Domain.Courses;
using SchoolManagementTask6.Domain.DTOs;
using SchoolManagementTask6.Persistence;

namespace SchoolManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin, Lecturer")]
    public class CourseController : ControllerBase
    {
        private readonly SchoolManagementTask6DbContext _context;

        public CourseController(SchoolManagementTask6DbContext context)
        {
            _context = context;
        }

        // Create courses
        [HttpPost]
        [Authorize(Roles = "Admin, Lecturer")]
        public async Task<ActionResult<CourseResponse>> CreateCourse([FromBody] CreateCourseRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var course = new Course
            {
                Title = request.Title,
                CourseCode = request.CourseCode,
                DepartmentId = request.DepartmentId,
                LecturerId = request.LecturerId
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            var dapartment = await _context.Departments.FindAsync(course.DepartmentId);
            var lecturer = await _context.Lecturers.FindAsync(course.LecturerId);

            var response = new CourseResponse
            {
                Id = course.Id,
                Title = course.Title,
                CourseCode = course.CourseCode,
                DepartmentName = dapartment!.Name,
                LecturerName = lecturer!.StaffNumber
            };

            return CreatedAtAction(nameof(GetCourseById), new { id = course.Id }, response);
        } 
            // Fetch By Id Operation
            [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourseById(int id)
        {
            var course = await _context.Courses
               .Include(c => c.Departments)
               .Include(c => c.Lecturers)
               .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
                return NotFound("Course not found");

            var response = new CourseResponse
            {
                Id = course.Id,
                Title = course.Title,
                CourseCode = course.CourseCode,
                DepartmentName = course.Departments?.Name,
                LecturerName = course.Lecturers?.StaffNumber
            };

            return Ok(response);
        }

        // Fetch All Operation
        [HttpGet]
        public async Task<ActionResult<List<Course>>> GetAllCousers()
        {
            var courses = await _context.Courses
                .Include(c => c.Departments)
                .Include(c => c.Lecturers)
                .ToListAsync();

            var response = new List<CourseResponse>();
            foreach (var course in courses)
            {
                response.Add(new CourseResponse
                {
                    Id = course.Id,
                    Title = course.Title,
                    CourseCode = course.CourseCode,
                    DepartmentName = course.Departments?.Name,
                    LecturerName = course.Lecturers?.StaffNumber
                });
            }

            return Ok(response);
        }

        // Update Operation
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Lecturer")]
        public async Task<ActionResult> UpdateCourse(int id, [FromBody] CreateCourseRequest request)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound(new { message = $"Course with id {id} not found" });
            }

            course.Title = request.Title;
            course.CourseCode = request.CourseCode;
            course.DepartmentId = request.DepartmentId;
            course.LecturerId = request.LecturerId;

            await _context.SaveChangesAsync();

            return Ok();
        }
        // Delete Operation
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound(new { message = $"Course with id {id} not found" });
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return Ok();

        }
    }
}
