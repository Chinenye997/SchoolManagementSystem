using SchoolManagementTask6.Domain.Departments;
using SchoolManagementTask6.Domain.UserManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementTask6.Domain.Lecturers
{
    public class Lecturer
    {
        public int Id { get; set; }  // Primary Key
        public string StaffNumber { get; set; } = string.Empty; // Unique Staff ID
        public int UserId { get; set; } // FK → User
        public User? User { get; set; }
        public int DepartmentId { get; set; } // FK → Department
        public Department? Department { get; set; }
    }
}
