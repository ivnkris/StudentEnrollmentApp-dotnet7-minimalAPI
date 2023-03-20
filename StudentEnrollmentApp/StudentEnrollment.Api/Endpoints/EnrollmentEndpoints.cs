using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using StudentEnrollment.Data;

namespace StudentEnrollment.Api.Endpoints;

public static class EnrollmentEndpoints
{
    public static void MapEnrollmentEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Enrollment").WithTags(nameof(Enrollment));

        group.MapGet("/", async (StudentEnrollmentDbContext db) =>
        {
            return await db.Enrollments.ToListAsync();
        })
        .WithName("GetAllEnrollments")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Enrollment>, NotFound>> (int id, StudentEnrollmentDbContext db) =>
        {
            return await db.Enrollments.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Enrollment model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
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

        group.MapPost("/", async (Enrollment enrollment, StudentEnrollmentDbContext db) =>
        {
            db.Enrollments.Add(enrollment);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Enrollment/{enrollment.Id}", enrollment);
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
