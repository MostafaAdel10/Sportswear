using AutoMapper;

namespace Sportswear.Core.Mapping.Roles
{
    public partial class RoleProfile : Profile
    {
        public RoleProfile()
        {
            GetRolesListMapping();
            GetRoleByIdMapping();
        }
    }
}
