using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.InfrastructureBases;

namespace Sportswear.Infrastructure.Abstracts
{
    public interface IProductVariantRepository : IGenericRepositoryAsync<ProductVariant>
    {
        public Task<List<ProductVariant>> GetByProductIdAsync(int productId);
        public Task<ProductVariant?> GetByIdWithIncludesAsync(int id);
        public Task<bool> IsProductVariantExistsAsync(int productVariantId);
        public Task<bool> IsProductVariantExistsExcludeSelfAsync(int productVariantId, int id);
        public Task<bool> ExistsAsync(int productId, string colorName, string size, int excludeId);
    }
}
