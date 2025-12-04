using Sportswear.DataAccess.Entities;
using Sportswear.DataAccess.Enums;

namespace Sportswear.Service.Abstract
{
    public interface IPaymentService
    {
        public Task<bool> AddAsync(Payment payment);
        public Task<bool> EditAsync(Payment payment);
        public bool CanChangePaymentStatus(Payment payment, PaymentStatus newStatus, OrderStatus orderStatus);

    }
}
