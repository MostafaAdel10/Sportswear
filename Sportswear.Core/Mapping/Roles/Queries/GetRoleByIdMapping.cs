using Sportswear.Core.Features.Authorization.Queries.Response_DTO_;
using Sportswear.DataAccess.Entities.Identity;

namespace Sportswear.Core.Mapping.Roles
{
    public partial class RoleProfile
    {
        public void GetRoleByIdMapping()
        {
            CreateMap<Role, GetRoleByIdResponse>();
        }
    }
}
