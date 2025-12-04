using Sportswear.Core.Features.User.Commands.Models;
using Sportswear.DataAccess.Entities.Identity;

namespace Sportswear.Core.Mapping.User
{
    public partial class UserProfile
    {
        public void UpdateUserMapping()
        {
            CreateMap<EditUserCommand, ApplicationUser>();
        }
    }
}
