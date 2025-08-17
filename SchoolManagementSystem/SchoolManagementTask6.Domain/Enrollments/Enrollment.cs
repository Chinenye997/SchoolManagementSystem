
namespace SchoolManagementTask6.Domain.Enrollments
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public EnrollmentStatus Status { get; set; }
    }

    public enum EnrollmentStatus
    {
        Pending,
        Active,
        Completed,
        Dropped
    }
}
