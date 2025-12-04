using Microsoft.EntityFrameworkCore;
using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.Abstracts;
using Sportswear.Infrastructure.Context;
using Sportswear.Infrastructure.InfrastructureBases;

namespace Sportswear.Infrastructure.Repository
{
    public class Product_DiscountRepository : GenericRepositoryAsync<Product_Discount>, IProduct_DiscountRepository
    {
        #region Fields
        private readonly DbSet<Product_Discount> _product_Discounts;
        #endregion

        #region Contractors
        public Product_DiscountRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _product_Discounts = dbContext.Set<Product_Discount>();
        }
        #endregion

        #region Handle Functions
        public async Task<bool> ExistsAsync(int discountId, int productId)
        {
            return await _product_Discounts
                .AnyAsync(pd => pd.DiscountId == discountId && pd.ProductId == productId);
        }

        public async Task<List<Product_Discount>> GetByDiscountIdAsync(int discountId)
        {
            return await _product_Discounts
                .Where(pd => pd.DiscountId == discountId)
                .ToListAsync();
        }

        public async Task<List<Product_Discount>> GetByDiscountAndProductsAsync(int discountId, List<int> productIds)
        {
            if (!productIds.Any())
            {
                // إذا فارغ، جلب كل الروابط للخصم
                return await GetByDiscountIdAsync(discountId);
            }

            return await _product_Discounts
                .Where(pd => pd.DiscountId == discountId && productIds.Contains(pd.ProductId))
                .ToListAsync();
        }
        #endregion
    }
}
