using Sportswear.DataAccess.Entities.Identity;
using Sportswear.DataAccess.Results;
using System.IdentityModel.Tokens.Jwt;

namespace Sportswear.Service.Abstract
{
    public interface IAuthenticationService
    {
        public Task<JwtAuthResult> GetJWTToken(ApplicationUser user);
        public JwtSecurityToken ReadJWTToken(string accessToken);
        public Task<(string, DateTime?)> ValidateDetails(JwtSecurityToken jwtToken, string AccessToken, string RefreshToken);
        public Task<JwtAuthResult> GetRefreshToken(ApplicationUser user, JwtSecurityToken jwtToken, DateTime? expiryDate, string refreshToken);
        public Task<string> ValidateToken(string AccessToken);
        public Task<string> ConfirmEmail(int? userId, string? code);
        public Task<string> SendResetPasswordCode(string Email);
        public Task<string> ConfirmResetPassword(string Code, string Email);
        public Task<string> ResetPassword(string Email, string Password);
    }
}
