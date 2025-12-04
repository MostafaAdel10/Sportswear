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
        Paid,
        Shipped,
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
}
