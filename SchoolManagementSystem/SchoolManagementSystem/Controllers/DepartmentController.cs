using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementTask6.Domain.Departments;
using SchoolManagementTask6.Domain.DTOs;
using SchoolManagementTask6.Persistence;

namespace SchoolManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly SchoolManagementTask6DbContext _context;

        public DepartmentController(SchoolManagementTask6DbContext context)
        {
            _context = context;
        }

        // CREATE Department
        [HttpPost]
        public async Task<ActionResult<DepartmentResponse>> CreateDepartment([FromBody] CreateDepartmentRequest request)
        {
            var department = new Department
            {
                Name = request.Name
            };

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return Ok(new DepartmentResponse
            {
                Id = department.Id,
                Name = department.Name
            });
        }

        // GET all Departments
        [HttpGet]
        public async Task<ActionResult<List<DepartmentResponse>>> GetDepartments()
        {
            var departments = await _context.Departments
                .Select(d => new DepartmentResponse
                {
                    Id = d.Id,
                    Name = d.Name
                })
                .ToListAsync();

            return Ok(departments);
        }

        // GET Department by Id
        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentResponse>> GetDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
                return NotFound();

            return Ok(new DepartmentResponse
            {
                Id = department.Id,
                Name = department.Name
            });
        }

        // UPDATE Department
        [HttpPut("{id}")]
        public async Task<ActionResult<DepartmentResponse>> UpdateDepartment(int id, [FromBody] CreateDepartmentRequest request)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
                return NotFound();

            department.Name = request.Name;
            await _context.SaveChangesAsync();

            return Ok(new DepartmentResponse
            {
                Id = department.Id,
                Name = department.Name
            });
        }

        // DELETE Department
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
                return NotFound();

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
