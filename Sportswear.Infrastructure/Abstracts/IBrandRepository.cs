using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.InfrastructureBases;

namespace Sportswear.Infrastructure.Abstracts
{
    public interface IBrandRepository : IGenericRepositoryAsync<Brand>
    {
        public Task<bool> IsBrandIdExist(int brandId);
        public Task<List<Brand>> GetBrandsListAsync();
        public Task<string> GenerateUniqueSlugAsync(string nameEn, int? excludeId = null);
    }
}
