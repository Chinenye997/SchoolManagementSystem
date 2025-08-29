using SchoolManagementTask6.Domain.UserManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementTask6.Domain.DTOs
{
    public class UserRegistrationRequest
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!; 
        public UserRole Role { get; set; } // Enum
        public Gender Gender { get; set; }
    }
}
