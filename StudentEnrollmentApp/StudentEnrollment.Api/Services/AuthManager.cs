using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using StudentEnrollment.Api.DTOs.Authentication;
using StudentEnrollment.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentEnrollment.Api.Services
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<SchoolUser> _userManager;
        private readonly IConfiguration _configuration;
        private SchoolUser? _user;

        public AuthManager(UserManager<SchoolUser> userManager, IConfiguration configuration)
        {
            this._userManager = userManager;
            this._configuration = configuration;
        }
        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {
            _user = await _userManager.FindByEmailAsync(loginDto.Username);
            if (_user == null) return default;

            bool isValidCredentials = await _userManager.CheckPasswordAsync(_user, loginDto.Password);
            if (!isValidCredentials) return default;

            var token = await GenerateTokenAsync();

            return new AuthResponseDto
            {
                Token = token,
                UserId = _user.Id
            };
        }

        private async Task<string> GenerateTokenAsync()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(_user);
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
            var userClaims = await _userManager.GetClaimsAsync(_user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, _user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, _user.Email),
                new Claim("userId", _user.Id),
            }.Union(userClaims).Union(roleClaims);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(Convert.ToInt32(_configuration["JwtSettings:DurationInHours"])),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
