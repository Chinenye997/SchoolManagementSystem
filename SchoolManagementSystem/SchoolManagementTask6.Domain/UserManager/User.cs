
namespace SchoolManagementTask6.Domain.UserManager
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? PasswordHash { get; set; }
        public UserRole Role { get; set; }
        public Gender Gender { get; set; }
        public AdmissionStatus AdmissionStatus { get; set; } = AdmissionStatus.Pending;
    }
}
