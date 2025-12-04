using Sportswear.DataAccess.Entities;

namespace Sportswear.Service.Abstract
{
    public interface IOrderItemService
    {
        public Task<bool> AddAsync(OrderItem orderItem);
    }
}
