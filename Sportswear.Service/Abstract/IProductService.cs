using Sportswear.DataAccess.Entities;
using Sportswear.DataAccess.Enums;

namespace Sportswear.Service.Abstract
{
    public interface IProductService
    {
        public Task<List<Product>> GetProductsListWithIncludesAsync();
        public Task<Product> GetByIdWithIncludesAsync(int id);
        public Task<List<Product>> GetByIdsAsync(List<int> ids);
        public Task<Product> GetByIdAsync(int id);
        public Task<bool> AddAsync(Product product);
        public Task<bool> EditAsync(Product product);
        public Task<bool> DeleteAsync(Product product);
        public decimal? CalculateDiscountedPriceOnProduct(Product product);
        public decimal? CalculateDiscountedPriceOnProductVariants(Product product, decimal originalPrice);
        public IQueryable<Product> GetProductQueryableWithIncludes();
        public IQueryable<Product> FilterProductPaginatedQueryable(ProductOrderingEnum orderingEnum, string search);
        public Task<bool> IsCodeExistsAsync(string code);
        public Task<bool> IsCodeExistsExcludeSelfAsync(string code, int id);
        public Task<bool> IsAnyProductRelatedToBrandAsync(int brandId);
        public Task<bool> IsAnyProductRelatedToCategoryAsync(int categoryId);
        public Task<bool> IsAnyProductRelatedToDiscountAsync(int discountId);
    }
}
