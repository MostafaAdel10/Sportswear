using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.InfrastructureBases;

namespace Sportswear.Infrastructure.Abstracts
{
    public interface ICartItemRepository : IGenericRepositoryAsync<CartItem>
    {
        public Task<CartItem?> GetCartItemByCartAndVariant(int cartId, int variantId);
        public Task<bool> IsProductRelatedWithCartItem(int productId);
        public Task<List<CartItem>> GetCartItemsByUserIdAsync(int userId);
        public Task<List<CartItem>> GetCartItemsListAsync();
        public Task<CartItem?> GetByIdWithIncludesAsync(int cartId);
    }
}
