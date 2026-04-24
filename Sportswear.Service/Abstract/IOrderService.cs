using Sportswear.DataAccess.Entities;
using Sportswear.DataAccess.Enums;

namespace Sportswear.Service.Abstract
{
    public interface IOrderService
    {
        public Task<int> AddAsync(Order order);
        public Task<bool> EditAsync(Order order);
        public Task<List<Order>> GetAllOrdersWithDetailsAsync();
        public Task<Order> GetByIdAsync(int id);
        public Task<Order?> GetOrderWithDetailsAsync(int orderId);
        public Task<List<Order>> GetOrdersByUserAsync(int userId);
        public bool CanChangeOrderStatusAsync(Order order, OrderStatus newStatus);
        public Task<List<Order>> GetOrdersForDashboardAsync(DateTime from);
    }
}
