using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Sportswear.DataAccess.Entities.Identity;
using Sportswear.DataAccess.Helpers;
using Sportswear.Service.AuthServices.Interfaces;

namespace Sportswear.Service.AuthServices.Implementations
{
    public class CurrentUserService : ICurrentUserService
    {

        #region Fields
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        #endregion

        #region Constructors
        public CurrentUserService(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        #endregion

        #region Functions
        public int GetUserId()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext == null || httpContext.User == null || !httpContext.User.Identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException();
            }

            var claim = httpContext.User.Claims
                .SingleOrDefault(c => c.Type == nameof(UserClaimModel.Id));

            if (claim == null || string.IsNullOrWhiteSpace(claim.Value))
            {
                throw new UnauthorizedAccessException();
            }

            if (!int.TryParse(claim.Value, out int userId))
            {
                throw new UnauthorizedAccessException();
            }

            return userId;
        }

        public async Task<ApplicationUser> GetUserAsync()
        {
            var userId = GetUserId();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            { throw new UnauthorizedAccessException(); }
            return user;
        }

        public async Task<List<string>> GetCurrentUserRolesAsync()
        {
            var user = await GetUserAsync();
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }
        #endregion
    }
}
