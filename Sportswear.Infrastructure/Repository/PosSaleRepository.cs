using Microsoft.EntityFrameworkCore;
using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.Abstracts;
using Sportswear.Infrastructure.Context;
using Sportswear.Infrastructure.InfrastructureBases;

namespace Sportswear.Infrastructure.Repository
{
    public class PosSaleRepository : GenericRepositoryAsync<PosSale>, IPosSaleRepository
    {
        private readonly DbSet<PosSale> _posSales;

        public PosSaleRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _posSales = dbContext.Set<PosSale>();
        }

        public async Task<PosSale?> GetByIdWithItemsAsync(int id)
        {
            return await _posSales
                .Include(s => s.Items)
                    .ThenInclude(i => i.ProductVariant)
                        .ThenInclude(v => v.Product)
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
        }

        public async Task<List<PosSale>> GetAllWithItemsAsync()
        {
            return await _posSales
                .Where(s => !s.IsDeleted)
                .Include(s => s.Items)
                .OrderByDescending(s => s.SaleDate)
                .ToListAsync();
        }

        public async Task<string> GenerateSaleNumberAsync()
        {
            // بيجيب آخر رقم ويزود عليه
            var lastSale = await _posSales
                .OrderByDescending(s => s.Id)
                .FirstOrDefaultAsync();

            int nextNumber = (lastSale == null) ? 1 : lastSale.Id + 1;

            return $"POS-{nextNumber:D5}"; // POS-00001
        }

        public async Task<List<PosSale>> GetPosSalesForDashboardAsync(DateTime from)
        {
            return await _posSales
                .Include(s => s.Items)
                    .ThenInclude(i => i.ProductVariant)
                        .ThenInclude(v => v.Product)
                            .ThenInclude(p => p.Images)
                .Where(s => s.SaleDate >= from && !s.IsDeleted)
                .OrderByDescending(s => s.SaleDate)
                .ToListAsync();
        }
    }
}
