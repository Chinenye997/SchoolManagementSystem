using SchoolManagementTask6.Domain.Courses;
using SchoolManagementTask6.Domain.Departments;
using SchoolManagementTask6.Domain.UserManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementTask6.Domain.Students
{
    public class Student
    {
        public int Id { get; set; }  // Primary Key
        public string MatricNumber { get; set; } = string.Empty; // Unique Student ID
        public int UserId { get; set; } // FK → User
        public User? User { get; set; } // Navigation property
        public AdmissionStatus Status { get; set; } // Enum: Pending, Admitted, Rejected
        public int DepartmentId { get; set; } // FK → Department
        public Department? Department { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
