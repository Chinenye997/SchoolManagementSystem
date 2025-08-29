using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementTask6.Domain.DTOs
{
    public class CreateCourseRequest
    {
        
        public string Title { get; set; } = default!;
        public string CourseCode { get; set; } = default!;
        public int DepartmentId { get; set; } = default!;
        public int LecturerId { get; set; } = default!;
    }

    public class CourseResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string CourseCode { get; set; } = default!;
        public string DepartmentName { get; set;} = default!;
        public string LecturerName { get;set;} = default!;
    }
}
