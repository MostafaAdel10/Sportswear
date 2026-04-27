using Microsoft.EntityFrameworkCore;
using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.Abstracts;
using Sportswear.Infrastructure.Context;
using Sportswear.Infrastructure.InfrastructureBases;

namespace Sportswear.Infrastructure.Repository
{
    public class ProductRepository : GenericRepositoryAsync<Product>, IProductRepository
    {
        #region Fields
        private readonly DbSet<Product> _products;
        #endregion

        #region Contractors
        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _products = dbContext.Set<Product>();
        }
        #endregion

        #region Handle Functions
        public async Task<Product?> GetProductWithIncludesFullDetailsAsync(int id)
        {
            return await _products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Reviews)
                .Include(p => p.Product_Discounts)
                    .ThenInclude(pd => pd.Discount)
                .Include(p => p.Variants)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

        public async Task<List<Product>> GetProductsListWithIncludesAsync()
        {
            return await _products.AsNoTracking()
                .Include(b => b.Brand)
                .Include(c => c.Category)
                .Include(i => i.Images)
                .Include(d => d.Product_Discounts)
                    .ThenInclude(pd => pd.Discount)
                .OrderByDescending(p => p.Id)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdWithIncludesAsync(int id)
        {
            return await _products.AsNoTracking()
                .Include(b => b.Brand)
                .Include(c => c.Category)
                .Include(i => i.Images)
                .Include(v => v.Variants)
                .Include(r => r.Reviews)
                .Include(d => d.Product_Discounts)
                    .ThenInclude(pd => pd.Discount)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

        public async Task<Product?> GetByCodeWithIncludesAsync(string code)
        {
            return await _products.AsNoTracking()
                .Include(b => b.Brand)
                .Include(c => c.Category)
                .Include(i => i.Images)
                .Include(v => v.Variants)
                .Include(r => r.Reviews)
                .Include(d => d.Product_Discounts)
                    .ThenInclude(pd => pd.Discount)
                .FirstOrDefaultAsync(p => p.Code == code && !p.IsDeleted);
        }

        public async Task<List<Product>> GetByIdsAsync(List<int> ids)
        {
            if (!ids.Any()) return new List<Product>();

            return await _products.AsNoTracking()
                .Include(d => d.Product_Discounts)
                    .ThenInclude(pd => pd.Discount)
                .Where(p => ids.Contains(p.Id) && !p.IsDeleted)
                .ToListAsync();
        }

        public IQueryable<Product> GetProductQueryableWithIncludes()
        {
            return _products.AsNoTracking()
                .Include(b => b.Brand)
                .Include(c => c.Category)
                .Include(i => i.Images)
                .Include(d => d.Product_Discounts)
                    .ThenInclude(pd => pd.Discount)
                .AsQueryable();
        }

        public async Task<bool> IsCodeExistsAsync(string code)
        {
            return await GetTableNoTracking().AnyAsync(p => p.Code == code);
        }

        public async Task<bool> IsAnyProductRelatedToBrandAsync(int brandId)
        {
            return await _products.AnyAsync(p => p.BrandId == brandId);
        }

        public async Task<bool> IsAnyProductRelatedToCategoryAsync(int categoryId)
        {
            return await _products.AnyAsync(p => p.CategoryId == categoryId);
        }

        public async Task<bool> IsAnyProductRelatedToDiscountAsync(int discountId)
        {
            return await _products.AnyAsync(p => p.Product_Discounts.Any(pd => pd.DiscountId == discountId));
        }

        public async Task<bool> IsCodeExistsExcludeSelfAsync(string code, int id)
        {
            return await GetTableNoTracking().AnyAsync(d => d.Code.Equals(code) & !d.Id.Equals(id));
        }

        public decimal? CalculateDiscountedPriceOnProduct(Product product)
        {
            if (product == null || product.Product_Discounts == null || !product.Product_Discounts.Any())
                return null;

            var now = DateTime.UtcNow;

            // جلب كل الخصومات النشطة
            var activeDiscounts = product.Product_Discounts
                .Select(pd => pd.Discount)
                .Where(d => !d.IsDeleted && d.StartDate <= now && d.EndDate >= now)
                .ToList();

            if (!activeDiscounts.Any())
                return null;

            var totalPercentage = activeDiscounts.Sum(d => d.Percentage);

            // نضمن ألا تتجاوز 100% 
            var effectivePercentage = Math.Min(totalPercentage, 100);

            var discountedPrice = product.BasePrice - (product.BasePrice * effectivePercentage / 100);

            return decimal.Round(discountedPrice, 2, MidpointRounding.AwayFromZero);
        }
        public decimal? CalculateDiscountedPriceOnProductVariants(Product product, decimal originalPrice)
        {
            if (product == null || product.Product_Discounts == null || !product.Product_Discounts.Any())
                return null;

            var now = DateTime.UtcNow;
            var activeDiscounts = product.Product_Discounts
                .Select(pd => pd.Discount)
                .Where(d => !d.IsDeleted && d.StartDate <= now && d.EndDate >= now)
                .ToList();

            if (!activeDiscounts.Any())
                return null;

            var totalPercentage = activeDiscounts.Sum(d => d.Percentage);
            var effectivePercentage = Math.Min(totalPercentage, 100m);  // استخدم decimal للدقة
            var discountedPrice = originalPrice - (originalPrice * effectivePercentage / 100m);
            return decimal.Round(discountedPrice, 2, MidpointRounding.AwayFromZero);
        }
        #endregion
    }
}
