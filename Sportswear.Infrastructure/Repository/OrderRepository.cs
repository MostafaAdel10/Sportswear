using Microsoft.EntityFrameworkCore;
using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.Abstracts;
using Sportswear.Infrastructure.Context;
using Sportswear.Infrastructure.InfrastructureBases;

namespace Sportswear.Infrastructure.Repository
{
    public class OrderRepository : GenericRepositoryAsync<Order>, IOrderRepository
    {
        #region Fields
        private readonly DbSet<Order> _orders;
        #endregion

        #region Contractors
        public OrderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _orders = dbContext.Set<Order>();
        }
        #endregion

        #region Handle Functions
        public async Task<List<Order>> GetAllOrdersWithDetailsAsync()
        {
            return await _orders
                .Include(o => o.User)
                .Include(o => o.Payment)
                .OrderByDescending(o => o.Id)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderWithDetailsAsync(int orderId)
        {
            return await _orders
               .Include(o => o.OrderItems)
                   .ThenInclude(oi => oi.ProductVariant)
                       .ThenInclude(pv => pv.Product)
               .Include(o => o.OrderItems)
                   .ThenInclude(oi => oi.ProductVariant)
                       .ThenInclude(pv => pv.Attributes)
                           .ThenInclude(a => a.ProductAttributeTemplate)
               .Include(o => o.Payment)
               .Include(o => o.User)
               .Include(o => o.Shipment)
                   .ThenInclude(s => s.ShippingMethod)
               .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductVariant)
                        .ThenInclude(pv => pv.Product)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductVariant)
                        .ThenInclude(pv => pv.Attributes)
                            .ThenInclude(a => a.ProductAttributeTemplate)
                .Include(o => o.Payment)
                .Include(o => o.User)
                .Include(o => o.Shipment)
                    .ThenInclude(s => s.ShippingMethod)
                .OrderByDescending(o => o.Id)
                .ToListAsync();
        }
        #endregion
    }
}
