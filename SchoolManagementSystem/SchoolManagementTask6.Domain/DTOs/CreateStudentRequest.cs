using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementTask6.Domain.DTOs
{
    // for creating student
    public class CreateStudentRequest
    {
        public int UserId { get; set; }
        public string MatricNumber { get; set; } = default!;
        public string AdmissionStatus { get; set; } = default!;
        public int DepartmentId { get; set; }
    }

    // for student responses
    public class StudentResponse
    {
        public int Id { get; set; }
        public string MatricNumber { get; set; } = default!;
        public string AdmissionStatus { get;set; } = default!;
        public string DepartmentName { get; set; } = default!;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string FullName { get; set; } = default!; //for user
    }
}
