using Sportswear.DataAccess.Entities;
using Sportswear.Infrastructure.Abstracts;
using Sportswear.Service.Abstract;

namespace Sportswear.Service.Implementations
{
    public class OrderItemService : IOrderItemService
    {
        #region Fields 
        private readonly IOrderItemRepository _orderItemRepository;
        #endregion

        #region Contractors
        public OrderItemService(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<bool> AddAsync(OrderItem orderItem)
        {
            await _orderItemRepository.AddAsync(orderItem);
            return true;
        }
        #endregion
    }
}
