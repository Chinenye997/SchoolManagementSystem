using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementTask6.Domain.DTOs
{
    // DTO for creating a user
    public class CreateUserRequest
    {
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public string UserRole { get; set; } = default!;
    }

    //dto for returing user
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string UserRole { get; set; } = default!;
    }
}
