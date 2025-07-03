namespace MVCConsumer.DTO
{
    public class CourseAndInstructorDto
    {
        List<CourseDTO> Courses { get; set; }
        List<string> Instructors { get; set; } = new List<string>();
    }
}
