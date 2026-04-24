using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.InfrastructureBases;

namespace Sportswear.Infrastructure.Abstracts
{
    public interface IOrderRepository : IGenericRepositoryAsync<Order>
    {
        public Task<List<Order>> GetAllOrdersWithDetailsAsync();
        public Task<Order?> GetOrderWithDetailsAsync(int orderId);
        public Task<List<Order>> GetOrdersByUserIdAsync(int userId);
        public Task<List<Order>> GetOrdersForDashboardAsync(DateTime from);
    }
}
