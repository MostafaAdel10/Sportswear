using Sportswear.DataAccess.Entities;
using Sportswear.DataAccess.Enums;
using Sportswear.Infrastructure.Abstracts;
using Sportswear.Service.Abstract;
using System.Globalization;

namespace Sportswear.Service.Implementations
{
    public class ProductService : IProductService
    {
        #region Fields
        private readonly IProductRepository _productRepository;
        #endregion

        #region Contractors
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<Product?> GetProductWithIncludesFullDetailsAsync(int id)
        {
            return await _productRepository.GetProductWithIncludesFullDetailsAsync(id);
        }
        public async Task<List<Product>> GetProductsListWithIncludesAsync()
        {
            return await _productRepository.GetProductsListWithIncludesAsync();
        }
        public async Task<int> AddAsync(Product product)
        {
            var savedProduct = await _productRepository.AddAsync(product);
            return savedProduct.Id;
        }
        public async Task<Product> GetByIdWithIncludesAsync(int id)
        {
            var product = await _productRepository.GetByIdWithIncludesAsync(id);
            return product;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product;
        }
        public async Task<List<Product>> GetByIdsAsync(List<int> ids)
        {
            return await _productRepository.GetByIdsAsync(ids);
        }

        public async Task<bool> EditAsync(Product product)
        {
            await _productRepository.UpdateAsync(product);
            return true;
        }

        public async Task<bool> DeleteAsync(Product product)
        {
            var transaction = _productRepository.BeginTransaction();

            try
            {
                await _productRepository.DeleteAsync(product);
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public IQueryable<Product> GetProductQueryableWithIncludes()
        {
            return _productRepository.GetProductQueryableWithIncludes();
        }

        public IQueryable<Product> FilterProductPaginatedQueryable(ProductOrderingEnum orderingEnum, string? search)
        {
            var queryable = _productRepository.GetProductQueryableWithIncludes();

            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            bool isArabic = cultureInfo.TwoLetterISOLanguageName.ToLower().Equals("ar");

            if (!string.IsNullOrWhiteSpace(search))
            {
                if (isArabic)
                {
                    queryable = queryable.Where(b =>
                        b.Code.Contains(search) ||
                        b.NameAr.Contains(search) ||
                        b.DescriptionAr.Contains(search) ||
                        (b.ClubAr != null && b.ClubAr.Contains(search)) ||
                        (b.Brand.NameAr != null && b.Brand.NameAr.Contains(search)) ||
                        (b.Category.NameAr != null && b.Category.NameAr.Contains(search)));
                }
                else
                {
                    queryable = queryable.Where(b =>
                        b.Code.Contains(search) ||
                        b.NameEn.Contains(search) ||
                        b.DescriptionEn.Contains(search) ||
                        (b.ClubEn != null && b.ClubEn.Contains(search)) ||
                        (b.Brand.NameEn != null && b.Brand.NameEn.Contains(search)) ||
                        (b.Category.NameEn != null && b.Category.NameEn.Contains(search)));
                }
            }

            switch (orderingEnum)
            {
                case ProductOrderingEnum.Id:
                    queryable = queryable.OrderByDescending(b => b.Id);
                    break;
                case ProductOrderingEnum.Code:
                    queryable = queryable.OrderBy(b => b.Code);
                    break;
                case ProductOrderingEnum.Name:
                    queryable = isArabic
                        ? queryable.OrderBy(b => b.NameAr)
                        : queryable.OrderBy(b => b.NameEn);
                    break;
                case ProductOrderingEnum.Description:
                    queryable = isArabic
                        ? queryable.OrderBy(b => b.DescriptionAr)
                        : queryable.OrderBy(b => b.DescriptionEn);
                    break;
                case ProductOrderingEnum.Season:
                    queryable = queryable.OrderBy(b => b.Season);
                    break;
                case ProductOrderingEnum.Club:
                    queryable = isArabic
                        ? queryable.OrderBy(b => b.ClubAr)
                        : queryable.OrderBy(b => b.ClubEn);
                    break;
                case ProductOrderingEnum.BasePrice:
                    queryable = queryable.OrderBy(b => b.BasePrice);
                    break;
                case ProductOrderingEnum.BrandName:
                    queryable = isArabic
                        ? queryable.OrderBy(b => b.Brand.NameAr)
                        : queryable.OrderBy(b => b.Brand.NameEn);
                    break;
                case ProductOrderingEnum.CategoryName:
                    queryable = isArabic
                        ? queryable.OrderBy(b => b.Category.NameAr)
                        : queryable.OrderBy(b => b.Category.NameEn);
                    break;
            }

            return queryable;
        }

        public decimal? CalculateDiscountedPriceOnProduct(Product product)
        {
            return _productRepository.CalculateDiscountedPriceOnProduct(product);
        }

        public decimal? CalculateDiscountedPriceOnProductVariants(Product product, decimal originalPrice)
        {
            return _productRepository.CalculateDiscountedPriceOnProductVariants(product, originalPrice);
        }

        public async Task<bool> IsCodeExistsAsync(string code)
        {
            return await _productRepository.IsCodeExistsAsync(code);
        }

        public async Task<bool> IsAnyProductRelatedToBrandAsync(int brandId)
        {
            return await _productRepository.IsAnyProductRelatedToBrandAsync(brandId);
        }

        public async Task<bool> IsAnyProductRelatedToCategoryAsync(int categoryId)
        {
            return await _productRepository.IsAnyProductRelatedToCategoryAsync(categoryId);
        }

        public async Task<bool> IsAnyProductRelatedToDiscountAsync(int discountId)
        {
            return await _productRepository.IsAnyProductRelatedToDiscountAsync(discountId);
        }

        public async Task<bool> IsCodeExistsExcludeSelfAsync(string code, int id)
        {
            return await _productRepository.IsCodeExistsExcludeSelfAsync(code, id);
        }
        #endregion
    }
}
