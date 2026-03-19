using Sportswear.Infrastructure.Abstracts;
using Sportswear.Service.Abstract;

namespace Sportswear.Service.Implementations
{
    public class DiscountCleanupJob : IDiscountCleanupJob
    {
        #region Fields
        private readonly IDiscountRepository _discountRepository;
        private readonly IProduct_DiscountRepository _productDiscountRepository;
        #endregion

        #region Constructors
        public DiscountCleanupJob(
            IDiscountRepository discountRepository,
            IProduct_DiscountRepository productDiscountRepository)
        {
            _discountRepository = discountRepository;
            _productDiscountRepository = productDiscountRepository;
        }
        #endregion

        #region Handle Functions
        public async Task ExecuteAsync()
        {
            var now = DateTime.UtcNow;

            // 1️⃣ جيب كل الـ Discounts المنتهية
            var expiredDiscounts = await _discountRepository.GetExpiredDiscountsAsync(now);
            if (!expiredDiscounts.Any()) return;

            var expiredIds = expiredDiscounts.Select(d => d.Id).ToList();

            // 2️⃣ جيب الـ Product_Discount Relations
            var relations = await _productDiscountRepository.GetByDiscountIdsAsync(expiredIds);

            // 3️⃣ امسح الـ Relations الأول
            if (relations.Any())
                await _productDiscountRepository.DeleteRangeAsync(relations);

            // 4️⃣ امسح الـ Discounts
            await _discountRepository.DeleteRangeAsync(expiredDiscounts);
        }
        #endregion
    }
}
