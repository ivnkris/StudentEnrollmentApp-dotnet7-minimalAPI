using StudentEnrollment.Api.DTOs.Authentication;

namespace StudentEnrollment.Api.Services
{
    public interface IAuthManager
    {
        Task<AuthResponseDto> Login(LoginDto loginDto);
    }
}
