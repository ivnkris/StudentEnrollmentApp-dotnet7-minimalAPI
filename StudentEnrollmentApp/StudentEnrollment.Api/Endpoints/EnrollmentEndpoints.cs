using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using StudentEnrollment.Data;
using AutoMapper;
using StudentEnrollment.Api.DTOs.Enrollment;

namespace StudentEnrollment.Api.Endpoints;

public static class EnrollmentEndpoints
{
    public static void MapEnrollmentEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Enrollment").WithTags(nameof(Enrollment));

        group.MapGet("/", async (StudentEnrollmentDbContext db, IMapper mapper) =>
        {
            var enrollments = await db.Enrollments.ToListAsync();
            return mapper.Map<List<EnrollmentDto>>(enrollments);
        })
        .WithName("GetAllEnrollments")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<EnrollmentDto>, NotFound>> (int id, StudentEnrollmentDbContext db, IMapper mapper) =>
        {
            var enrollment = await db.Enrollments.AsNoTracking().FirstOrDefaultAsync(model => model.Id == id);
            if (enrollment != null) return TypedResults.Ok(mapper.Map<EnrollmentDto>(enrollment));
            return TypedResults.NotFound();
        })
        .WithName("GetEnrollmentById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Enrollment enrollment, StudentEnrollmentDbContext db) =>
        {
            var affected = await db.Enrollments
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.CourseId, enrollment.CourseId)
                  .SetProperty(m => m.StudentId, enrollment.StudentId)
                  .SetProperty(m => m.Id, enrollment.Id)
                  .SetProperty(m => m.CreatedDate, enrollment.CreatedDate)
                  .SetProperty(m => m.CreatedBy, enrollment.CreatedBy)
                  .SetProperty(m => m.ModifiedDate, enrollment.ModifiedDate)
                  .SetProperty(m => m.ModifiedBy, enrollment.ModifiedBy)
                );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateEnrollment")
        .WithOpenApi();

        group.MapPost("/", async (CreateEnrollmentDto enrollmentDto, StudentEnrollmentDbContext db, IMapper mapper) =>
        {
            db.Enrollments.Add(mapper.Map<Enrollment>(enrollmentDto));
            await db.SaveChangesAsync();
            return TypedResults.Created($"CourseId: {enrollmentDto.CourseId}, StudentId: {enrollmentDto.StudentId}", enrollmentDto);
        })
        .WithName("CreateEnrollment")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, StudentEnrollmentDbContext db) =>
        {
            var affected = await db.Enrollments
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteEnrollment")
        .WithOpenApi();
    }
}
