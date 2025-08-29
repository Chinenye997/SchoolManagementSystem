using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementTask6.Domain.DTOs
{
    public class CreateDepartmentRequest
    {
        public string Name { get; set; } = default!;
    }

    public class DepartmentResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
    }
}
