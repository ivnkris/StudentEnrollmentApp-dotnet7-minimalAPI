using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using StudentEnrollment.Data;
using AutoMapper;
using StudentEnrollment.Api.DTOs.Student;

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

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Student student, StudentEnrollmentDbContext db) =>
        {
            var affected = await db.Students
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.FirstName, student.FirstName)
                  .SetProperty(m => m.LastName, student.LastName)
                  .SetProperty(m => m.DateofBirth, student.DateofBirth)
                  .SetProperty(m => m.IdNumber, student.IdNumber)
                  .SetProperty(m => m.Picture, student.Picture)
                  .SetProperty(m => m.Id, student.Id)
                  .SetProperty(m => m.CreatedDate, student.CreatedDate)
                  .SetProperty(m => m.CreatedBy, student.CreatedBy)
                  .SetProperty(m => m.ModifiedDate, student.ModifiedDate)
                  .SetProperty(m => m.ModifiedBy, student.ModifiedBy)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateStudent")
        .WithOpenApi();

        group.MapPost("/", async (CreateStudentDto studentDto, StudentEnrollmentDbContext db, IMapper mapper) =>
        {
            db.Students.Add(mapper.Map<Student>(studentDto));
            await db.SaveChangesAsync();
            return TypedResults.Created($"{studentDto.FirstName} {studentDto.LastName}", studentDto);
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
