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

    public enum PosPaymentMethod
    {
        Cash = 1,
        Card = 2,
        Mixed = 3
    }

    public enum PosSaleStatus
    {
        Completed = 1,
        Cancelled = 2
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
