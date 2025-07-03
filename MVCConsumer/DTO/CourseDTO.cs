using MVCConsumer.Models;

namespace MVCConsumer.DTO
{
    public class CourseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int InstID { get; set; }
        public string InstName { get; set; }

        public List<Instructor> Instructors { get; set; } = new();
    }
}
