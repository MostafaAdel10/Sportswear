using Sportswear.DataAccess.Entities;

namespace Sportswear.Service.Abstract
{
    public interface ICartItemService
    {
        public Task<CartItem?> GetByIdAsync(int cartItemId);
        public Task<CartItem?> GetCartItemByCartAndVariant(int cartId, int variantId);
        public Task<bool> IsProductRelatedWithCartItem(int productId);
        public Task<List<CartItem>> GetCartItemsByUserIdAsync(int userId);
        public Task<List<CartItem>> GetCartItemsListAsync();
        public Task<bool> AddAsync(CartItem cartItem);
        public Task<bool> EditAsync(CartItem cartItem);
        public Task<bool> DeleteAsync(CartItem cartItem);
        public Task ClearCartAsync(int userId);
        public Task<CartItem?> GetByIdWithIncludesAsync(int cartId);
        public Task<bool> CheckStockAvailabilityAsync(int userId);
    }
}
