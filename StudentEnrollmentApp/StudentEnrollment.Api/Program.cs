using Microsoft.EntityFrameworkCore;
using StudentEnrollment.Data;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration.GetConnectionString("StudentEnrollmentDbConnection");
builder.Services.AddDbContext<StudentEnrollmentDbContext>(options =>
{
    options.UseSqlServer(connection);
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.MapGet("/courses", async (StudentEnrollmentDbContext context) => {
    return await context.Courses.ToListAsync();
});

app.Run();