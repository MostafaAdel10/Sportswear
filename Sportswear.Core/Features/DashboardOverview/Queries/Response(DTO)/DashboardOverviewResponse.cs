namespace Sportswear.Core.Features.DashboardOverview.Queries.Response_DTO_
{
    public class DashboardOverviewResponse
    {
        // KPIs
        public KpiCardsDto KpiCards { get; set; }

        // Charts
        public List<RevenueChartDto> RevenueChart { get; set; }
        public List<OrderStatusDto> OrdersByStatus { get; set; }
        public List<TopProductDto> TopProducts { get; set; }
        public List<TopCityDto> TopCities { get; set; }
        public RatingsOverviewDto RatingsOverview { get; set; }

        // Recent Activity
        public List<RecentOrderDto> RecentOrders { get; set; }
        public List<RecentPosSaleDto> RecentPosSales { get; set; }
    }

    // ─── KPIs ───
    public class KpiCardsDto
    {
        public decimal TotalRevenue { get; set; }
        public decimal OnlineRevenue { get; set; }
        public decimal PosRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int TotalPosSales { get; set; }
        public decimal AverageOrderValue { get; set; }
        public int TotalProducts { get; set; }
        public int TotalCustomers { get; set; }
        public int LowStockProducts { get; set; } // Stock < 5
    }

    // ─── Revenue Chart ───
    public class RevenueChartDto
    {
        public string Date { get; set; }           // "2026-04-01"
        public decimal OnlineRevenue { get; set; }
        public decimal PosRevenue { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    // ─── Orders by Status ───
    public class OrderStatusDto
    {
        public string Status { get; set; }
        public int Count { get; set; }
        public decimal Percentage { get; set; }
    }

    // ─── Top Products ───
    public class TopProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string? ImageUrl { get; set; }
        public int TotalQuantitySold { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    // ─── Top Cities ───
    public class TopCityDto
    {
        public string City { get; set; }
        public int OrdersCount { get; set; }
        public decimal Percentage { get; set; }
    }

    // ─── Ratings ───
    public class RatingsOverviewDto
    {
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public List<RatingDistributionDto> Distribution { get; set; }
    }

    public class RatingDistributionDto
    {
        public int Stars { get; set; }      // 1, 2, 3, 4, 5
        public int Count { get; set; }
        public decimal Percentage { get; set; }
    }

    // ─── Recent Orders ───
    public class RecentOrderDto
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // ─── Recent POS Sales ───
    public class RecentPosSaleDto
    {
        public int Id { get; set; }
        public string SaleNumber { get; set; }
        public decimal FinalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime SaleDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
