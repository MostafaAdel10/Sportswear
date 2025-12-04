using Sportswear.DataAccess.Entities.Identity;

namespace Sportswear.Service.Abstract
{
    public interface IApplicationUserService
    {
        public Task<string> AddUserAsync(ApplicationUser user, string password);
    }
}
