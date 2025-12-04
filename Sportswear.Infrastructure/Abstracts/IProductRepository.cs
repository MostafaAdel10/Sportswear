using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.InfrastructureBases;

namespace Sportswear.Infrastructure.Abstracts
{
    public interface IProductRepository : IGenericRepositoryAsync<Product>
    {
        public Task<List<Product>> GetProductsListWithIncludesAsync();
        public IQueryable<Product> GetProductQueryableWithIncludes();
        public Task<Product> GetByIdWithIncludesAsync(int id);
        public Task<List<Product>> GetByIdsAsync(List<int> ids);
        public Task<bool> IsCodeExistsAsync(string code);
        public Task<bool> IsCodeExistsExcludeSelfAsync(string code, int id);
        public Task<bool> IsAnyProductRelatedToBrandAsync(int brandId);
        public Task<bool> IsAnyProductRelatedToCategoryAsync(int categoryId);
        public Task<bool> IsAnyProductRelatedToDiscountAsync(int discountId);
        public decimal? CalculateDiscountedPriceOnProduct(Product product);
        public decimal? CalculateDiscountedPriceOnProductVariants(Product product, decimal originalPrice);
    }
}
