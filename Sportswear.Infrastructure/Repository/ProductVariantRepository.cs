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
            return await _productVariants
                .Where(v => v.ProductId == productId && !v.IsDeleted)
                .Include(v => v.Attributes)
                    .ThenInclude(a => a.ProductAttributeTemplate)
                .ToListAsync();
        }

        public async Task<ProductVariant?> GetByIdWithIncludesAsync(int id)
        {
            return await _productVariants
                .Include(v => v.Product)
                .Include(v => v.Attributes)
                    .ThenInclude(a => a.ProductAttributeTemplate)
                .Include(v => v.OrderItems)
                .FirstOrDefaultAsync(v => v.Id == id && !v.IsDeleted);
        }

        public async Task<bool> IsProductVariantExistsAsync(int productVariantId)
        {
            return await GetTableNoTracking()
                .AnyAsync(v => v.Id == productVariantId && !v.IsDeleted);
        }

        public async Task<bool> IsProductVariantExistsExcludeSelfAsync(int productVariantId, int id)
        {
            return await GetTableNoTracking()
                .AnyAsync(v => v.Id == productVariantId && v.Id != id && !v.IsDeleted);
        }

        public async Task<HashSet<string>> GetVariantKeysAsync(int productId, int excludeId = 0)
        {
            var variants = await _productVariants
                .Where(v => v.ProductId == productId && !v.IsDeleted && v.Id != excludeId)
                .Include(v => v.Attributes)
                .ToListAsync();

            return variants
                .Select(v => string.Join("-", v.Attributes
                    .OrderBy(a => a.ProductAttributeTemplateId)
                    .Select(a => a.ValueEn.ToUpper())))
                .ToHashSet();
        }
        #endregion
    }
}
