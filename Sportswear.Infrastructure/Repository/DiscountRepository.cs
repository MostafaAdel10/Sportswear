using Microsoft.EntityFrameworkCore;
using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.Abstracts;
using Sportswear.Infrastructure.Context;
using Sportswear.Infrastructure.InfrastructureBases;

namespace Sportswear.Infrastructure.Repository
{
    public class DiscountRepository : GenericRepositoryAsync<Discount>, IDiscountRepository
    {
        #region Fields
        private readonly DbSet<Discount> _discounts;
        #endregion

        #region Contractors
        public DiscountRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _discounts = dbContext.Set<Discount>();
        }
        #endregion

        #region Handle Functions
        public async Task<List<Discount>> GetExpiredDiscountsAsync(DateTime now)
        {
            return await GetTableNoTracking()
                .Where(d => d.EndDate < now)
                .ToListAsync();
        }

        public async Task<List<Discount>> GetAllWithProductsCountAsync()
        {
            return await _discounts
                .Include(d => d.Product_Discounts)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> IsDiscountIdExist(int discountId)
        {
            return await GetTableNoTracking().AnyAsync(b => b.Id == discountId);
        }
        public async Task<List<Discount>> GetActiveDiscountsAsync()
        {
            var now = DateTime.UtcNow;
            return await _discounts
                .Where(d => !d.IsDeleted && d.StartDate <= now && d.EndDate >= now)
                .OrderByDescending(d => d.Id)
                .ToListAsync();
        }

        public async Task<Discount?> GetActiveDiscountByIdAsync(int discountId)
        {
            var now = DateTime.UtcNow;

            return await _discounts
                .Where(d =>
                    d.Id == discountId &&
                    !d.IsDeleted &&
                    d.StartDate <= now &&
                    d.EndDate >= now)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsCodeExistsAsync(string code)
        {
            return await GetTableNoTracking().AnyAsync(d => d.Code == code);
        }

        public async Task<bool> IsCodeExistsExcludeSelfAsync(string code, int id)
        {
            return await GetTableNoTracking().AnyAsync(d => d.Code.Equals(code) & !d.Id.Equals(id));
        }
        #endregion
    }
}
