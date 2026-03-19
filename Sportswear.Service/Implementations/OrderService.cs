using Sportswear.DataAccess.Entities;
using Sportswear.DataAccess.Enums;
using Sportswear.Infrastructure.Abstracts;
using Sportswear.Service.Abstract;

namespace Sportswear.Service.Implementations
{
    public class OrderService : IOrderService
    {
        #region Fields 
        private readonly IOrderRepository _orderRepository;
        #endregion

        #region Contractors
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<int> AddAsync(Order order)
        {
            var savedOrder = await _orderRepository.AddAsync(order);
            return savedOrder.Id;
        }

        public async Task<List<Order>> GetAllOrdersWithDetailsAsync()
        {
            return await _orderRepository.GetAllOrdersWithDetailsAsync();
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task<Order?> GetOrderWithDetailsAsync(int orderId)
        {
            return await _orderRepository.GetOrderWithDetailsAsync(orderId);
        }

        public async Task<List<Order>> GetOrdersByUserAsync(int userId)
        {
            return await _orderRepository.GetOrdersByUserIdAsync(userId);
        }

        public async Task<bool> EditAsync(Order order)
        {
            await _orderRepository.UpdateAsync(order);
            return true;
        }

        public bool CanChangeOrderStatusAsync(Order order, OrderStatus newStatus)
        {
            // 1️⃣ حالات لا يمكن تغييرها
            if (order.Status == OrderStatus.Completed || order.Status == OrderStatus.Cancelled)
                return false;

            // 2️⃣ لا يمكن العودة إلى حالة أقل
            if ((int)newStatus < (int)order.Status)
                return false;

            // 3️⃣ لا يمكن الدفع  قبل شحن الطلب
            if (newStatus == OrderStatus.Paid && order.Status != OrderStatus.Shipped)
                return false;

            // 4️⃣ لا يمكن إكمال الطلب قبل الدفع
            if (newStatus == OrderStatus.Completed && order.Status != OrderStatus.Paid)
                return false;

            // 5️⃣ لا يمكن إلغاء الطلب بعد الدفع أو الشحن
            if (newStatus == OrderStatus.Cancelled &&
               (order.Status == OrderStatus.Paid || order.Status == OrderStatus.Shipped))
                return false;

            return true;
        }
        #endregion
    }
}
