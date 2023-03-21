using AutoMapper;
using StudentEnrollment.Api.DTOs.Course;
using StudentEnrollment.Api.DTOs.Enrollment;
using StudentEnrollment.Api.DTOs.Student;
using StudentEnrollment.Data;

namespace StudentEnrollment.Api.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Course, CourseDto>().ReverseMap();
            CreateMap<Course, CreateCourseDto>().ReverseMap();
            CreateMap<Student, StudentDto>().ReverseMap();
            CreateMap<Student, CreateStudentDto>().ReverseMap();
            CreateMap<Enrollment, EnrollmentDto>().ReverseMap();
        }
    }
}
