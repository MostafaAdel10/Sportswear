using Sportswear.DataAccess.Entities;

namespace Sportswear.Service.Abstract
{
    public interface IProductVariantService
    {
        public Task<bool> AddRangeAsync(List<ProductVariant> productVariants);
        public Task<bool> EditAsync(ProductVariant productVariant);
        public Task<bool> EditStockOnlyAsync(ProductVariant productVariant);
        public Task<bool> DeleteAsync(ProductVariant productVariant);
        public Task<ProductVariant?> GetByIdAsync(int id);
        public Task<ProductVariant?> GetByIdWithIncludesAsync(int id);
        public Task<List<ProductVariant>> GetByProductIdAsync(int productId);
        public Task<HashSet<string>> GetVariantKeysAsync(int productId, int excludeId = 0);
        public Task<List<ProductVariant>> GetByIdsWithProductAsync(List<int> ids);
        public Task<bool> IsProductVariantExistsAsync(int productVariantId);
        public Task<bool> IsProductVariantExistsExcludeSelfAsync(int productVariantId, int id);
    }
}
