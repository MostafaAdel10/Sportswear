using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.Abstracts;
using Sportswear.Service.Abstract;

namespace Sportswear.Service.Implementations
{
    public class CartService : ICartService
    {
        #region Fields 
        private readonly ICartRepository _cartRepository;
        #endregion

        #region Contractors
        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<bool> AddAsync(Cart cart)
        {
            await _cartRepository.AddAsync(cart);
            return true;
        }

        public async Task<bool> DeleteAsync(Cart cart)
        {
            var transaction = _cartRepository.BeginTransaction();

            try
            {
                await _cartRepository.DeleteAsync(cart);
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> EditAsync(Cart cart)
        {
            await _cartRepository.UpdateAsync(cart);
            return true;
        }

        public async Task<Cart?> GetByIdAsync(int id)
        {
            var cart = await _cartRepository.GetByIdAsync(id);
            return cart;
        }

        public async Task<Cart?> GetByUserIdAsync(int userId)
        {
            return await _cartRepository.GetByUserIdAsync(userId);
        }

        public async Task<bool> IsCartOwnedByUser(int cartId, int userId)
        {
            return await _cartRepository.IsCartOwnedByUser(cartId, userId);
        }
        #endregion
    }
}
