using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.InfrastructureBases;

namespace Sportswear.Infrastructure.Abstracts
{
    public interface ICategoryRepository : IGenericRepositoryAsync<Category>
    {
        public Task<bool> IsCategoryIdExist(int categoryId);
        public Task<List<Category>> GetCategoriesListAsync();
        public Task<string> GenerateUniqueSlugAsync(string nameEn, int? excludeId = null);
    }
}
