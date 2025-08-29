using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementTask6.Domain.DTOs
{
    public class CreateLecturerRequest
    {
        public int UserId { get; set; }
        public int DepartmentId { get; set; }
    }

    public class LecturerResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; } = default!;
        public string DepartmentName { get; set; } = default!;
    } 
}
