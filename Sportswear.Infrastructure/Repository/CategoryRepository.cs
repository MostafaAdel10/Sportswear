using Microsoft.EntityFrameworkCore;
using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.Abstracts;
using Sportswear.Infrastructure.Context;
using Sportswear.Infrastructure.InfrastructureBases;

namespace Sportswear.Infrastructure.Repository
{
    public class CategoryRepository : GenericRepositoryAsync<Category>, ICategoryRepository
    {
        #region Fields
        private readonly DbSet<Category> _categorys;
        #endregion

        #region Contractors
        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _categorys = dbContext.Set<Category>();
        }
        #endregion

        #region Handle Functions
        public async Task<bool> IsCategoryIdExist(int categoryId)
        {
            return await GetTableNoTracking().AnyAsync(c => c.Id == categoryId);
        }

        public async Task<List<Category>> GetCategoriesListAsync()
        {
            return await _categorys
                .OrderByDescending(c => c.Id)
                .ToListAsync();
        }

        public async Task<string> GenerateUniqueSlugAsync(string nameEn, int? excludeId = null)
        {
            var baseSlug = nameEn.ToLowerInvariant();
            var slug = baseSlug;
            var counter = 1;

            while (await _dbContext.Categories.AnyAsync(c =>
                c.Slug == slug && !c.IsDeleted && (excludeId == null || c.Id != excludeId)))
            {
                slug = $"{baseSlug}-{counter}";
                counter++;
            }

            return slug;
        }
        #endregion
    }
}
