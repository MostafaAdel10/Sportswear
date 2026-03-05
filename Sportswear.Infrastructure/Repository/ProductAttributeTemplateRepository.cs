using Microsoft.EntityFrameworkCore;
using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.Abstracts;
using Sportswear.Infrastructure.Context;
using Sportswear.Infrastructure.InfrastructureBases;

namespace Sportswear.Infrastructure.Repository
{
    public class ProductAttributeTemplateRepository : GenericRepositoryAsync<ProductAttributeTemplate>, IProductAttributeTemplateRepository
    {
        #region Fields
        private readonly DbSet<ProductAttributeTemplate> _templates;
        #endregion

        #region Contractors
        public ProductAttributeTemplateRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _templates = dbContext.Set<ProductAttributeTemplate>();
        }
        #endregion

        #region Handle Functions
        public async Task<List<ProductAttributeTemplate>> GetByCategoryIdAsync(int categoryId)
        {
            return await _templates
                .Where(t => t.CategoryId == categoryId && !t.IsDeleted)
                .OrderBy(t => t.Id)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int categoryId, string keyEn)
        {
            return await GetTableNoTracking()
                .AnyAsync(t => t.CategoryId == categoryId &&
                               t.KeyEn.ToLower() == keyEn.ToLower() &&
                               !t.IsDeleted);
        }

        public async Task<bool> HasVariantAttributesAsync(int templateId)
        {
            return await GetTableNoTracking()
                .AnyAsync(t => t.Id == templateId &&
                               t.VariantAttributes.Any(v => !v.IsDeleted) &&
                               !t.IsDeleted);
        }
        public async Task<bool> CategoryHasVariantsAsync(int categoryId)
        {
            return await GetTableNoTracking()
                .AnyAsync(t => t.CategoryId == categoryId &&
                               t.VariantAttributes.Any(v => !v.IsDeleted));
        }
        #endregion
    }
}
