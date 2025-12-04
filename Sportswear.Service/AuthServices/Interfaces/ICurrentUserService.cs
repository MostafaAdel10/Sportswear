using Sportswear.DataAccess.Entities.Identity;

namespace Sportswear.Service.AuthServices.Interfaces
{
    public interface ICurrentUserService
    {
        public Task<ApplicationUser> GetUserAsync();
        public int GetUserId();
        public Task<List<string>> GetCurrentUserRolesAsync();
    }
}
