using Sportswear.DataAccess.Entities;

namespace Sportswear.Service.Abstract
{
    public interface ICartService
    {
        public Task<Cart?> GetByIdAsync(int id);
        public Task<bool> IsCartOwnedByUser(int cartId, int userId);
        public Task<Cart?> GetByUserIdAsync(int userId);
        public Task<bool> AddAsync(Cart cart);
        public Task<bool> EditAsync(Cart cart);
        public Task<bool> DeleteAsync(Cart cart);
    }
}
