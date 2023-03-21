namespace StudentEnrollment.Api.DTOs.Student
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateofBirth { get; set; }
        public string IdNumber { get; set; }
        public string Picture { get; set; }
    }
    public class CreateStudentDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateofBirth { get; set; }
        public string IdNumber { get; set; }
        public string Picture { get; set; }
    }
}
