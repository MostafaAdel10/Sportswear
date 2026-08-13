using Sportswear.DataAccess.Entities;

namespace Sportswear.Service.Abstract
{
    public interface IBrandService
    {
        public Task<bool> IsBrandIdExist(int brandId);
        public Task<List<Brand>> GetBrandsListAsync();
        public Task<Brand> GetByIdAsync(int id);
        public Task<bool> AddAsync(Brand brand);
        public Task<bool> EditAsync(Brand brand);
        public Task<bool> DeleteAsync(Brand brand);
        public Task<string> GenerateUniqueSlugAsync(string nameEn, int? excludeId = null);
    }
}
