using SchoolManagementTask6.Domain.UserManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementTask6.Domain.DTOs
{
    public class LoginRequest
    {
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
    }

    public class LoginResponseDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public UserRole Role { get; set; }
    }
}
