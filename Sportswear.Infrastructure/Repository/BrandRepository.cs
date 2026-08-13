using Microsoft.EntityFrameworkCore;
using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.Abstracts;
using Sportswear.Infrastructure.Context;
using Sportswear.Infrastructure.InfrastructureBases;

namespace Sportswear.Infrastructure.Repository
{
    public class BrandRepository : GenericRepositoryAsync<Brand>, IBrandRepository
    {
        #region Fields
        private readonly DbSet<Brand> _brands;
        #endregion

        #region Contractors
        public BrandRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _brands = dbContext.Set<Brand>();
        }
        #endregion

        #region Handle Functions
        public async Task<bool> IsBrandIdExist(int brandId)
        {
            return await GetTableNoTracking().AnyAsync(b => b.Id == brandId);
        }
        public async Task<List<Brand>> GetBrandsListAsync()
        {
            return await _brands
                .OrderByDescending(b => b.Id)
                .ToListAsync();
        }
        public async Task<string> GenerateUniqueSlugAsync(string nameEn, int? excludeId = null)
        {
            var baseSlug = nameEn.ToLowerInvariant();
            var slug = baseSlug;
            var counter = 1;

            while (await _dbContext.Brands.AnyAsync(b =>
                b.Slug == slug && !b.IsDeleted && (excludeId == null || b.Id != excludeId)))
            {
                slug = $"{baseSlug}-{counter}";
                counter++;
            }

            return slug;
        }
        #endregion
    }
}
