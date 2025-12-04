using Sportswear.Core.Features.User.Queries.Response_DTO_;
using Sportswear.DataAccess.Entities.Identity;

namespace Sportswear.Core.Mapping.User
{
    public partial class UserProfile
    {
        public void GetUserByIdMapping()
        {
            CreateMap<ApplicationUser, GetUserByIdResponse>();
        }
    }
}
