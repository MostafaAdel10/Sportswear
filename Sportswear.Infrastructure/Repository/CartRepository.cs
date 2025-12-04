using Microsoft.EntityFrameworkCore;
using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.Abstracts;
using Sportswear.Infrastructure.Context;
using Sportswear.Infrastructure.InfrastructureBases;

namespace Sportswear.Infrastructure.Repository
{
    public class CartRepository : GenericRepositoryAsync<Cart>, ICartRepository
    {
        #region Fields
        private readonly DbSet<Cart> _carts;
        #endregion

        #region Contractors
        public CartRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _carts = dbContext.Set<Cart>();
        }
        #endregion

        #region Handle Functions
        public async Task<Cart?> GetByUserIdAsync(int userId)
        {
            return await _carts
                .Include(c => c.Items)
                    .ThenInclude(i => i.ProductVariant)
                        .ThenInclude(v => v.Product)
                            .ThenInclude(p => p.Images)
                .Include(c => c.Items)
                    .ThenInclude(i => i.ProductVariant)
                        .ThenInclude(v => v.Product)
                            .ThenInclude(p => p.Product_Discounts)
                                .ThenInclude(pd => pd.Discount)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<bool> IsCartOwnedByUser(int cartId, int userId)
        {
            return await _carts.AnyAsync(x => x.Id == cartId && x.UserId == userId);
        }
        #endregion
    }
}
