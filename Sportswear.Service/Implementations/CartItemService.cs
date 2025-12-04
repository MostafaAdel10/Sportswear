using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.Abstracts;
using Sportswear.Service.Abstract;

namespace Sportswear.Service.Implementations
{
    public class CartItemService : ICartItemService
    {
        #region Fields
        private readonly ICartItemRepository _cartItemRepository;
        #endregion

        #region Contractors
        public CartItemService(ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<bool> AddAsync(CartItem cartItem)
        {
            await _cartItemRepository.AddAsync(cartItem);
            return true;
        }

        public async Task<bool> DeleteAsync(CartItem cartItem)
        {
            var transaction = _cartItemRepository.BeginTransaction();

            try
            {
                await _cartItemRepository.DeleteAsync(cartItem);
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> EditAsync(CartItem cartItem)
        {
            await _cartItemRepository.UpdateAsync(cartItem);
            return true;
        }

        public async Task<CartItem?> GetCartItemByCartAndVariant(int cartId, int variantId)
        {
            return await _cartItemRepository.GetCartItemByCartAndVariant(cartId, variantId);
        }

        public async Task<CartItem?> GetByIdAsync(int cartItemId)
        {
            var brand = await _cartItemRepository.GetByIdAsync(cartItemId);
            return brand;
        }

        public async Task<List<CartItem>> GetCartItemsByUserIdAsync(int userId)
        {
            return await _cartItemRepository.GetCartItemsByUserIdAsync(userId);
        }

        public async Task<List<CartItem>> GetCartItemsListAsync()
        {
            return await _cartItemRepository.GetCartItemsListAsync();
        }

        public async Task<bool> IsProductRelatedWithCartItem(int productId)
        {
            return await _cartItemRepository.IsProductRelatedWithCartItem(productId);
        }

        public async Task<CartItem?> GetByIdWithIncludesAsync(int cartId)
        {
            return await _cartItemRepository.GetByIdWithIncludesAsync(cartId);
        }
        public async Task ClearCartAsync(int userId)
        {
            var cartItems = await GetCartItemsByUserIdAsync(userId);

            if (cartItems.Any())
            {
                await _cartItemRepository.DeleteRangeAsync(cartItems);
            }
        }

        public async Task<bool> CheckStockAvailabilityAsync(int userId)
        {
            var cartItems = await _cartItemRepository.GetCartItemsByUserIdAsync(userId);

            if (cartItems == null || !cartItems.Any())
                return false;

            foreach (var item in cartItems)
            {
                if (item.ProductVariant.StockQuantity < item.Quantity)
                    return false; // Stock not sufficient
            }

            return true; // All good
        }
        #endregion
    }
}
