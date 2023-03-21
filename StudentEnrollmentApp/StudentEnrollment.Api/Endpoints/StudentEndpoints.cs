using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using StudentEnrollment.Data;
using AutoMapper;
using StudentEnrollment.Api.DTOs.Student;
using StudentEnrollment.Api.DTOs.Course;

namespace StudentEnrollment.Api.Endpoints;

public static class StudentEndpoints
{
    public static void MapStudentEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Student").WithTags(nameof(Student));

        group.MapGet("/", async (StudentEnrollmentDbContext db, IMapper mapper) =>
        {
            var students = await db.Students.ToListAsync();
            return mapper.Map<List<StudentDto>>(students);
        })
        .WithName("GetAllStudents")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<StudentDto>, NotFound>> (int id, StudentEnrollmentDbContext db, IMapper mapper) =>
        {
            var student = await db.Students.AsNoTracking().FirstOrDefaultAsync(model => model.Id == id);

            if (student != null) return TypedResults.Ok(mapper.Map<StudentDto>(student));
            return TypedResults.NotFound();
        })
        .WithName("GetStudentById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, StudentDto studentDto, StudentEnrollmentDbContext db, IMapper mapper) =>
        {
            var foundModel = await db.Students.FindAsync(id);

            if (foundModel != null)
            {
                mapper.Map(studentDto, foundModel);
                await db.SaveChangesAsync();
                return TypedResults.Ok();
            }
            return TypedResults.NotFound();
        })
        .WithName("UpdateStudent")
        .WithOpenApi();

        group.MapPost("/", async (CreateStudentDto studentDto, StudentEnrollmentDbContext db, IMapper mapper) =>
        {
            var student = mapper.Map<Student>(studentDto);
            db.Students.Add(student);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Course/{student.Id}", student);
        })
        .WithName("CreateStudent")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, StudentEnrollmentDbContext db) =>
        {
            var affected = await db.Students
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteStudent")
        .WithOpenApi();
    }
}
