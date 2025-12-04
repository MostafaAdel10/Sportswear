using Sportswear.DataAccess.Entities.Identity;
using Sportswear.Infrastructure.InfrastructureBases;

namespace Sportswear.Infrastructure.Abstracts
{
    public interface IRefreshTokenRepository : IGenericRepositoryAsync<UserRefreshToken>
    {

    }
}
