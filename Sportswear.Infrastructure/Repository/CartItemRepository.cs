using Microsoft.EntityFrameworkCore;
using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.Abstracts;
using Sportswear.Infrastructure.Context;
using Sportswear.Infrastructure.InfrastructureBases;

namespace Sportswear.Infrastructure.Repository
{
    public class CartItemRepository : GenericRepositoryAsync<CartItem>, ICartItemRepository
    {
        #region Fields
        private readonly DbSet<CartItem> _cartItem;
        #endregion

        #region Contractors
        public CartItemRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _cartItem = dbContext.Set<CartItem>();
        }
        #endregion

        #region Handle Functions
        public async Task<List<CartItem>> GetCartItemsByUserIdAsync(int userId)
        {
            return await _dbContext.CartItems
            .Where(c => c.Cart.UserId == userId)
            .Include(pv => pv.ProductVariant)
                .ThenInclude(p => p.Product)
            .ToListAsync();
        }
        public async Task<List<CartItem>> GetCartItemsListAsync()
        {
            return await _cartItem
                .Include(pv => pv.ProductVariant)
                    .ThenInclude(p => p.Product)
                .ToListAsync();
        }

        public async Task<CartItem?> GetCartItemByCartAndVariant(int cartId, int variantId)
        {
            return await _cartItem.Where(x => x.CartId == cartId && x.ProductVariantId == variantId).FirstOrDefaultAsync();
        }

        public async Task<bool> IsProductRelatedWithCartItem(int productId)
        {
            var book = await _cartItem.Where(b => b.ProductVariant.ProductId.Equals(productId)).FirstOrDefaultAsync();
            if (book == null) return false;
            return true;
        }

        public async Task<CartItem?> GetByIdWithIncludesAsync(int cartId)
        {
            return await _cartItem
                .Include(c => c.ProductVariant)
                        .ThenInclude(v => v.Product)
                            .ThenInclude(p => p.Images)
                .Include(c => c.ProductVariant)
                        .ThenInclude(v => v.Product)
                            .ThenInclude(p => p.Product_Discounts)
                                .ThenInclude(pd => pd.Discount)
                .FirstOrDefaultAsync(c => c.Id == cartId);
        }
        #endregion
    }
}
