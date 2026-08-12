using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.DashboardOverview.Queries.Models;
using Sportswear.Core.Features.DashboardOverview.Queries.Response_DTO_;
using Sportswear.Core.Resources;
using Sportswear.DataAccess.Enums;
using Sportswear.Infrastructure.Context;
using Sportswear.Service.Abstract;

namespace Sportswear.Core.Features.DashboardOverview.Queries.Handlers
{
    public class DashboardQueryHandler : ResponseHandler,
       IRequestHandler<GetDashboardOverviewQuery, Response<DashboardOverviewResponse>>
    {
        #region Fields
        private readonly IOrderService _orderService;
        private readonly IPosSaleService _posSaleService;
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructor
        public DashboardQueryHandler(
            IOrderService orderService,
            IPosSaleService posSaleService,
            ApplicationDbContext context,
            IStringLocalizer<SharedResources> localizer) : base(localizer)
        {
            _orderService = orderService;
            _posSaleService = posSaleService;
            _context = context;
            _localizer = localizer;
        }
        #endregion

        #region Handle
        public async Task<Response<DashboardOverviewResponse>> Handle(
            GetDashboardOverviewQuery request, CancellationToken cancellationToken)
        {
            var from = DateTime.UtcNow.AddDays(-request.Days);

            // ─── جيب البيانات ───
            var orders = await _orderService.GetOrdersForDashboardAsync(from);
            var posSales = await _posSaleService.GetPosSalesForDashboardAsync(from);

            // ─── KPIs ───
            var completedOrders = orders
                .Where(o => o.Status != OrderStatus.Cancelled).ToList();

            var completedPosSales = posSales
                .Where(s => s.Status == PosSaleStatus.Completed).ToList();

            var onlineRevenue = completedOrders.Sum(o => o.TotalAmount);
            var posRevenue = completedPosSales.Sum(s => s.FinalAmount);
            var totalRevenue = onlineRevenue + posRevenue;

            var totalOrdersCount = completedOrders.Count;
            var totalPosSalesCount = completedPosSales.Count;
            var totalTransactions = totalOrdersCount + totalPosSalesCount;

            var averageOrderValue = totalTransactions > 0
                ? totalRevenue / totalTransactions
                : 0;

            var totalProducts = await _context.Products
                .CountAsync(p => !p.IsDeleted, cancellationToken);

            var totalCustomers = await _context.Users
                .CountAsync(cancellationToken);

            var lowStockProducts = await _context.ProductVariants
                .CountAsync(v => !v.IsDeleted && v.StockQuantity < 5, cancellationToken);

            var kpiCards = new KpiCardsDto
            {
                TotalRevenue = totalRevenue,
                OnlineRevenue = onlineRevenue,
                PosRevenue = posRevenue,
                TotalOrders = totalOrdersCount,
                TotalPosSales = totalPosSalesCount,
                AverageOrderValue = Math.Round(averageOrderValue, 2),
                TotalProducts = totalProducts,
                TotalCustomers = totalCustomers,
                LowStockProducts = lowStockProducts
            };

            // ─── Revenue Chart (per day) ───
            var allDates = Enumerable.Range(0, request.Days)
                .Select(i => DateTime.UtcNow.AddDays(-i).Date)
                .OrderBy(d => d)
                .ToList();

            var revenueChart = allDates.Select(date =>
            {
                var dayOnlineRevenue = completedOrders
                    .Where(o => o.CreatedAt.Date == date)
                    .Sum(o => o.TotalAmount);

                var dayPosRevenue = completedPosSales
                    .Where(s => s.SaleDate.Date == date)
                    .Sum(s => s.FinalAmount);

                return new RevenueChartDto
                {
                    Date = date.ToString("yyyy-MM-dd"),
                    OnlineRevenue = dayOnlineRevenue,
                    PosRevenue = dayPosRevenue,
                    TotalRevenue = dayOnlineRevenue + dayPosRevenue
                };
            }).ToList();

            // ─── Orders by Status ───
            var totalOrdersAll = orders.Count;
            var ordersByStatus = orders
                .GroupBy(o => o.Status)
                .Select(g => new OrderStatusDto
                {
                    Status = g.Key.ToString(),
                    Count = g.Count(),
                    Percentage = totalOrdersAll > 0
                        ? Math.Round((decimal)g.Count() / totalOrdersAll * 100, 1)
                        : 0
                }).ToList();

            // ─── Top Products (Orders + POS) ───
            var productSales = new Dictionary<int, (string Name, string? Image, int Qty, decimal Revenue)>();

            // من الـ Orders
            foreach (var order in completedOrders)
            {
                foreach (var item in order.OrderItems)
                {
                    var productId = item.ProductVariant.ProductId;
                    var productName = item.ProductVariant.Product.NameEn;
                    var image = item.ProductVariant.Product.Images
                        .FirstOrDefault()?.Url;

                    if (productSales.ContainsKey(productId))
                    {
                        var existing = productSales[productId];
                        productSales[productId] = (
                            existing.Name,
                            existing.Image,
                            existing.Qty + item.Quantity,
                            existing.Revenue + (item.UnitPrice * item.Quantity)
                        );
                    }
                    else
                    {
                        productSales[productId] = (
                            productName,
                            image,
                            item.Quantity,
                            item.UnitPrice * item.Quantity
                        );
                    }
                }
            }

            // من الـ POS
            foreach (var sale in completedPosSales)
            {
                foreach (var item in sale.Items)
                {
                    var productId = item.ProductVariant.ProductId;
                    var productName = item.ProductVariant.Product.NameEn;
                    var image = item.ProductVariant.Product.Images
                        .FirstOrDefault()?.Url;

                    if (productSales.ContainsKey(productId))
                    {
                        var existing = productSales[productId];
                        productSales[productId] = (
                            existing.Name,
                            existing.Image,
                            existing.Qty + item.Quantity,
                            existing.Revenue + item.TotalPrice
                        );
                    }
                    else
                    {
                        productSales[productId] = (
                            productName,
                            image,
                            item.Quantity,
                            item.TotalPrice
                        );
                    }
                }
            }

            var topProducts = productSales
                .OrderByDescending(p => p.Value.Qty)
                .Take(10)
                .Select(p => new TopProductDto
                {
                    ProductId = p.Key,
                    ProductName = p.Value.Name,
                    ImageUrl = p.Value.Image,
                    TotalQuantitySold = p.Value.Qty,
                    TotalRevenue = Math.Round(p.Value.Revenue, 2)
                }).ToList();

            // ─── Top Cities ───
            var totalShipments = completedOrders
                .Where(o => o.Shipment != null).Count();

            var topCities = completedOrders
                .Where(o => o.Shipment != null)
                .GroupBy(o => o.Shipment!.City)
                .Select(g => new TopCityDto
                {
                    City = g.Key,
                    OrdersCount = g.Count(),
                    Percentage = totalShipments > 0
                        ? Math.Round((decimal)g.Count() / totalShipments * 100, 1)
                        : 0
                })
                .OrderByDescending(c => c.OrdersCount)
                .Take(5)
                .ToList();

            // ─── Ratings Overview ───
            var reviews = await _context.Reviews
                .Where(r => !r.IsDeleted)
                .ToListAsync(cancellationToken);

            var totalReviews = reviews.Count;
            var averageRating = totalReviews > 0
                ? Math.Round(reviews.Average(r => r.Rating), 1)
                : 0;

            var ratingDistribution = Enumerable.Range(1, 5)
                .Select(star => new RatingDistributionDto
                {
                    Stars = star,
                    Count = reviews.Count(r => r.Rating == star),
                    Percentage = totalReviews > 0
                        ? Math.Round(
                            (decimal)reviews.Count(r => r.Rating == star) / totalReviews * 100, 1)
                        : 0
                }).ToList();

            var ratingsOverview = new RatingsOverviewDto
            {
                AverageRating = averageRating,
                TotalReviews = totalReviews,
                Distribution = ratingDistribution
            };

            // ─── Recent Orders ───
            var recentOrders = orders
                .OrderByDescending(o => o.CreatedAt)
                .Take(5)
                .Select(o => new RecentOrderDto
                {
                    OrderId = o.Id,
                    CustomerName = o.User?.UserName ?? "Unknown",
                    TotalAmount = o.TotalAmount,
                    Status = o.Status.ToString(),
                    CreatedAt = o.CreatedAt
                }).ToList();

            // ─── Recent POS Sales ───
            var recentPosSales = posSales
                .OrderByDescending(s => s.SaleDate)
                .Take(5)
                .Select(s => new RecentPosSaleDto
                {
                    Id = s.Id,
                    SaleNumber = s.SaleNumber,
                    FinalAmount = s.FinalAmount,
                    PaymentMethod = s.PaymentMethod.ToString(),
                    SaleDate = s.SaleDate,
                    CreatedBy = s.CreatedBy
                }).ToList();

            // ─── Final Response ───
            var response = new DashboardOverviewResponse
            {
                KpiCards = kpiCards,
                RevenueChart = revenueChart,
                OrdersByStatus = ordersByStatus,
                TopProducts = topProducts,
                TopCities = topCities,
                RatingsOverview = ratingsOverview,
                RecentOrders = recentOrders,
                RecentPosSales = recentPosSales
            };

            return Success(response);
        }
        #endregion
    }
}
