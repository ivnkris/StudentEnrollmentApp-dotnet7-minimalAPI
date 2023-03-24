using StudentEnrollment.Api.DTOs.Authentication;
using StudentEnrollment.Api.Services;

namespace StudentEnrollment.Api.Endpoints
{
    public static class AuthenticationEndpoints
    {
        public static void MapAuthenticationEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapPost("api/Login/", async (LoginDto loginDto, IAuthManager authManager) =>
            {
                var response = await authManager.Login(loginDto);

                if (response == null) return Results.Unauthorized();

                return Results.Ok(response);
            })
            .WithTags("Authentication")
            .WithName("Login")
            .Produces(StatusCodes.Status200OK);
        }
    }
}
