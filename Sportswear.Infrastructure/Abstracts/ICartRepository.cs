using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.InfrastructureBases;

namespace Sportswear.Infrastructure.Abstracts
{
    public interface ICartRepository : IGenericRepositoryAsync<Cart>
    {
        public Task<Cart?> GetByUserIdAsync(int userId);
        public Task<bool> IsCartOwnedByUser(int cartId, int userId);
    }
}
