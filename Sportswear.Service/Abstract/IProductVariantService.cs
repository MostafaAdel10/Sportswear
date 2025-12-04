using Sportswear.DataAccess.Entities;

namespace Sportswear.Service.Abstract
{
    public interface IProductVariantService
    {
        public Task<ProductVariant> GetByIdAsync(int id);
        public Task<ProductVariant> GetByIdWithIncludesAsync(int id);
        public Task<bool> AddAsync(ProductVariant productVariant);
        public Task<bool> EditAsync(ProductVariant productVariant);
        public Task<bool> DeleteAsync(ProductVariant productVariant);
        public Task<bool> IsProductVariantExistsAsync(int productVariantId);
        public Task<bool> IsProductVariantExistsExcludeSelfAsync(int productVariantId, int id);
    }
}
