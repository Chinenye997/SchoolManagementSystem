using SchoolManagementTask6.Domain.Courses;
using SchoolManagementTask6.Domain.Lecturers;
using SchoolManagementTask6.Domain.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementTask6.Domain.Departments
{
    public class Department
    {
        public int Id { get; set; }  // Primary Key
        public string Name { get; set; } = string.Empty;

        // Navigation properties
        public List<Student>? Students { get; set; }
        public List<Lecturer>? Lecturers { get; set; }
        public List<Course>? Courses { get; set; }
    }
}
