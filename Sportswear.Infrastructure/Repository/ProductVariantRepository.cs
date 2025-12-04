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
        public async Task<ProductVariant?> GetByIdWithIncludesAsync(int id)
        {
            return await _productVariants
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
        #endregion
    }
}
