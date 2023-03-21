namespace StudentEnrollment.Api.DTOs.Enrollment
{
    public class EnrollmentDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }
    }

    public class CreateEnrollmentDto
    {
        public int CourseId { get; set; }
        public int StudentId { get; set; }
    }
}
