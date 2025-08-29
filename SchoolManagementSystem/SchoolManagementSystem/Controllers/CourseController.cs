using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementTask6.Domain.Courses;
using SchoolManagementTask6.Persistence;

namespace SchoolManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        //private readonly SchoolManagementTask6DbContext _context;

        //public CourseController(SchoolManagementTask6DbContext context)
        //{
        //    _context = context;
        //}

        //// Create courses
        //[HttpPost]
        //public async Task<ActionResult> CreateCourse(Course course)
        //{

        //}

        //// Fetch By Id Operation
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Course>> GetCourseById(int id)
        //{

        //}

        //// Fetch All Operation
        //[HttpGet]
        //public async Task<ActionResult<List<Course>>> GetAllCousers()
        //{

        //}

        //// Update Operation
        //[HttpPut]
        //public async Task<ActionResult> UpdateCourse(int id, Course course)
        //{

        //}

        //// Delete Operation
        //[HttpDelete]
        //public async Task<ActionResult> DeleteCourse()
        //{

        //}
    }
}
