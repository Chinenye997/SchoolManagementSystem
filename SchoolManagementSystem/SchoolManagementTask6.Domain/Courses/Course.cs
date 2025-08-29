
using SchoolManagementTask6.Domain.Departments;
using SchoolManagementTask6.Domain.Lecturers;

namespace SchoolManagementTask6.Domain.Courses
{
    public class Course
    {
        public int Id { get; set; }  // Primary Key
        public string CourseCode { get; set; } = default!;  
        public string Title { get; set; } = default!;
        public int Unit { get; set; } //  credit units

        public int DepartmentId { get; set; } // FK → Department
        public Department? Departments { get; set; }

        public int LecturerId { get; set; } // FK → Lecturer
        public Lecturer? Lecturers { get; set; }
    }
}
