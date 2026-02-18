using Microsoft.EntityFrameworkCore;
using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.Abstracts;
using Sportswear.Infrastructure.Context;
using Sportswear.Infrastructure.InfrastructureBases;

namespace Sportswear.Infrastructure.Repository
{
    public class ProductVariantRepository : GenericRepositoryAsync<ProductVariant>, IProductVariantRepository
    {
        #region Fields
        private readonly DbSet<ProductVariant> _productVariants;
        #endregion

        #region Contractors
        public ProductVariantRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _productVariants = dbContext.Set<ProductVariant>();
        }
        #endregion

        #region Handle Functions 
        public async Task<List<ProductVariant>> GetByProductIdAsync(int productId)
        {
            return await GetTableNoTracking()
                .Where(x => x.ProductId == productId)
                .ToListAsync();
        }

        public async Task<ProductVariant?> GetByIdWithIncludesAsync(int id)
        {
            return await _productVariants
                .Include(pv => pv.Product)
                .Include(pv => pv.OrderItems)
                .FirstOrDefaultAsync(pv => pv.Id == id);
        }

        public async Task<bool> IsProductVariantExistsAsync(int productVariantId)
        {
            return await GetTableNoTracking().AnyAsync(p => p.Id == productVariantId);
        }

        public async Task<bool> IsProductVariantExistsExcludeSelfAsync(int productVariantId, int id)
        {
            return await GetTableNoTracking().AnyAsync(p => p.Id.Equals(productVariantId) & !p.Id.Equals(id));
        }

        public async Task<bool> ExistsAsync(int productId, string colorName, string size, int excludeId)
        {
            return await _productVariants.AnyAsync(x =>
                x.ProductId == productId &&
                x.ColorName == colorName &&
                x.Size == size &&
                x.Id != excludeId);
        }
        #endregion
    }
}
