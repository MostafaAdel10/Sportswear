using Sportswear.DataAccess.Entities;
using Sportswear.DataAccess.Enums;
using Sportswear.Infrastructure.Abstracts;
using Sportswear.Service.Abstract;

namespace Sportswear.Service.Implementations
{
    public class PaymentService : IPaymentService
    {
        #region Fields 
        private readonly IPaymentRepository _paymentRepository;
        #endregion

        #region Contractors
        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<bool> AddAsync(Payment payment)
        {
            await _paymentRepository.AddAsync(payment);
            return true;
        }

        public async Task<bool> EditAsync(Payment payment)
        {
            await _paymentRepository.UpdateAsync(payment);
            return true;
        }

        public bool CanChangePaymentStatus(Payment payment, PaymentStatus newStatus, OrderStatus orderStatus)
        {
            // 1️⃣ Payment is null or no change requested
            if (payment == null)
                return false;

            // 2️⃣ Already Completed — cannot change
            if (payment.Status == PaymentStatus.Completed)
                return false;

            // 3️⃣ If new status is Pending — meaningless
            if (newStatus == PaymentStatus.Pending)
                return false;

            // 4️⃣ Cannot mark payment Completed if order is Cancelled
            if (newStatus == PaymentStatus.Completed && orderStatus == OrderStatus.Cancelled)
                return false;

            // 5️⃣ Cannot mark payment Completed if order not delivered yet (optional rule)
            // Uncomment if required:
            // if (newStatus == PaymentStatus.Completed && orderStatus != OrderStatus.Shipped)
            //     return false;

            return true;
        }

        #endregion
    }
}
