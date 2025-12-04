using Microsoft.EntityFrameworkCore;
using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.Abstracts;
using Sportswear.Infrastructure.Context;
using Sportswear.Infrastructure.InfrastructureBases;

namespace Sportswear.Infrastructure.Repository
{
    public class OrderItemRepository : GenericRepositoryAsync<OrderItem>, IOrderItemRepository
    {
        #region Fields
        private readonly DbSet<OrderItem> _orderItems;
        #endregion

        #region Contractors
        public OrderItemRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _orderItems = dbContext.Set<OrderItem>();
        }
        #endregion

        #region Handle Functions
        #endregion
    }
}
