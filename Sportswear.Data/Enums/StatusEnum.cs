namespace Sportswear.DataAccess.Enums
{
    public enum PaymentMethod
    {
        Card,
        CashOnDelivery,
        PayPal
    }

    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed
    }

    public enum OrderStatus
    {
        Pending,
        Shipped,
        Paid,
        Completed,
        Cancelled
    }

    public enum ShippingStatus
    {
        Processing,
        Shipped,
        Delivered
    }

    public enum DiscountType
    {
        Global,      // Coupon / Voucher
        ProductSpecific
    }
    public enum DiscountStatusFilter
    {
        All,      // كل الـ Discounts
        Active,   // شغالة دلوقتي
        Expired,  // منتهية
        Upcoming  // لسه هتبدأ
    }
}
