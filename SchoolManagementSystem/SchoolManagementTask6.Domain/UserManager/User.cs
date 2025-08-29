
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementTask6.Domain.UserManager
{
    public class User
    {
        public int Id { get; set; }  // Primary Key
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!; // Store hashed password
        public UserRole Role { get; set; } // Enum: Admin, Student, Lecturer
        public UserStatus UserStatus { get; set; }
        public Gender Gender { get; set; }
    }
}
