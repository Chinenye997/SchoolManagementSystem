
namespace SchoolManagementTask6.Domain.Courses
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; } = default;
        public int CreditHours { get; set; }
        public Department Department { get; set; }  // Enum usage
    }

    public enum Department
    {
        Science,
        Arts,
        Commerce
    }
}
