using MediatR;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Order.Queries.Models;
using Sportswear.Core.Features.Order.Queries.Response_DTO_;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;
using Sportswear.Service.AuthServices.Interfaces;

namespace Sportswear.Core.Features.Order.Queries.Handlers
{
    public class OrderQueryHandler : ResponseHandler,
       IRequestHandler<GetOrderFullDetailsQuery, Response<OrderFullDetailsDto>>,
       IRequestHandler<GetOrderByIdQuery, Response<OrderDto>>,
       IRequestHandler<GetOrderListForCurrentUserQuery, Response<List<OrderDto>>>,
       IRequestHandler<GetAllOrdersQuery, Response<List<AdminOrderListDto>>>,
       IRequestHandler<GetOrdersByUserIdQuery, Response<List<AdminOrderListDto>>>
    {
        private readonly IOrderService _orderService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;

        public OrderQueryHandler(
            IOrderService orderService,
            ICurrentUserService currentUserService,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _orderService = orderService;
            _currentUserService = currentUserService;
            _stringLocalizer = stringLocalizer;
        }

        //Get Order Full Details by id Query
        public async Task<Response<OrderFullDetailsDto>> Handle(GetOrderFullDetailsQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetOrderWithDetailsAsync(request.OrderId);
            if (order == null)
                return NotFound<OrderFullDetailsDto>(
                    _stringLocalizer[SharedResourcesKeys.NotFound]);

            bool isArabic = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName.ToLower().Equals("ar");

            var dto = new OrderFullDetailsDto
            {
                Id = order.Id,
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString(),
                CreatedAt = order.CreatedAt,

                // User
                UserId = order.UserId,
                UserEmail = order.User?.Email ?? string.Empty,
                UserName = order.User?.UserName ?? string.Empty,

                // Payment
                PaymentStatus = order.Payment?.Status.ToString(),
                PaymentMethod = order.Payment?.Method.ToString(),
                PaidAt = order.Payment?.PaidAt,

                // Shipment
                ShipmentInfo = order.Shipment == null ? null : new ShipmentDto
                {
                    FullName = order.Shipment.FullName,
                    City = order.Shipment.City,
                    Country = order.Shipment.Country,
                    Region = order.Shipment.Region,
                    StreetAddress = order.Shipment.StreetAddress,
                    BuildingNumber = order.Shipment.BuildingNumber,
                    FloorNumber = order.Shipment.FloorNumber,
                    ApartmentNumber = order.Shipment.ApartmentNumber,
                    PhoneNumber = order.Shipment.PhoneNumber,
                    Notes = order.Shipment.Notes,
                    TrackingNumber = order.Shipment.TrackingNumber,
                    ShippingMethod = isArabic
                        ? order.Shipment.ShippingMethod?.NameAr ?? string.Empty
                        : order.Shipment.ShippingMethod?.NameEn ?? string.Empty,
                    ShipmentStatus = order.Shipment.Status.ToString()
                },

                // Items
                Items = order.OrderItems.Select(i => new OrderItemDto
                {
                    ProductVariantId = i.ProductVariantId,
                    SKU = i.ProductVariant?.SKU ?? string.Empty,
                    ProductName = isArabic
                        ? i.ProductVariant?.Product?.NameAr ?? string.Empty
                        : i.ProductVariant?.Product?.NameEn ?? string.Empty,
                    AttributeKey = isArabic
                        ? i.ProductVariant?.Product.AttributeKeyAr
                        : i.ProductVariant?.Product.AttributeKeyEn,
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity,
                    TotalPrice = i.UnitPrice * i.Quantity,
                    AttributeValue = isArabic
                        ? i.ProductVariant?.AttributeValueAr
                        : i.ProductVariant?.AttributeValueEn,
                    Unit = i.ProductVariant?.Unit,
                    ColorLabel = i.ProductVariant?.ColorLabel,
                    ColorHex = i.ProductVariant?.ColorHex
                }).ToList(),

                TotalQuantity = order.OrderItems.Sum(i => i.Quantity)
            };

            return Success(dto);
        }

        // 1️⃣ Get order by id
        public async Task<Response<OrderDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetOrderWithDetailsAsync(request.OrderId);
            if (order == null)
                return NotFound<OrderDto>(_stringLocalizer[SharedResourcesKeys.OrderNotFound]);

            var userId = _currentUserService.GetUserId();
            if (order.UserId != userId)
                return Unauthorized<OrderDto>(_stringLocalizer[SharedResourcesKeys.UnAuthorized]);

            bool isArabic = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName.ToLower().Equals("ar");

            return Success(MapToOrderDto(order, isArabic));
        }

        // 2️⃣ Get orders list for current user
        public async Task<Response<List<OrderDto>>> Handle(GetOrderListForCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            if (userId <= 0)
                return Unauthorized<List<OrderDto>>(_stringLocalizer[SharedResourcesKeys.UnAuthorized]);

            var orders = await _orderService.GetOrdersByUserAsync(userId);
            if (orders == null || !orders.Any())
                return Success(new List<OrderDto>(), _stringLocalizer[SharedResourcesKeys.TheOrderIsEmpty]);

            bool isArabic = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName.ToLower().Equals("ar");

            return Success(orders.Select(o => MapToOrderDto(o, isArabic)).ToList());
        }

        // 3️⃣ Get all orders (Admin)
        public async Task<Response<List<AdminOrderListDto>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderService.GetAllOrdersWithDetailsAsync();

            var dtoList = orders.Select(o => new AdminOrderListDto
            {
                OrderId = o.Id,
                UserId = o.UserId,
                UserEmail = o.User?.Email ?? string.Empty,
                TotalAmount = o.TotalAmount,
                OrderStatus = o.Status.ToString(),
                PaymentStatus = o.Payment?.Status.ToString() ?? "Unknown",
                CreatedAt = o.CreatedAt
            }).ToList();

            return Success(dtoList);
        }

        // 4️⃣ Get orders by user id (Admin)
        public async Task<Response<List<AdminOrderListDto>>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderService.GetOrdersByUserAsync(request.UserId);
            if (!orders.Any())
                return Success(new List<AdminOrderListDto>(), _stringLocalizer[SharedResourcesKeys.TheOrderIsEmpty]);

            var dtoList = orders.Select(o => new AdminOrderListDto
            {
                OrderId = o.Id,
                UserId = o.UserId,
                UserEmail = o.User?.Email ?? string.Empty,
                TotalAmount = o.TotalAmount,
                OrderStatus = o.Status.ToString(),
                PaymentStatus = o.Payment?.Status.ToString() ?? "Unknown",
                CreatedAt = o.CreatedAt
            }).ToList();

            return Success(dtoList);
        }

        // ✅ Private helper عشان منكررش الكود
        private OrderDto MapToOrderDto(DataAccess.Entities.Order order, bool isArabic)
        {
            return new OrderDto
            {
                Id = order.Id,
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString(),
                CreatedAt = order.CreatedAt,

                PaymentStatus = order.Payment?.Status.ToString(),
                PaymentMethod = order.Payment?.Method.ToString(),

                ShipmentInfo = order.Shipment == null ? null : new ShipmentDto
                {
                    FullName = order.Shipment.FullName,
                    City = order.Shipment.City,
                    Country = order.Shipment.Country,
                    Region = order.Shipment.Region,
                    StreetAddress = order.Shipment.StreetAddress,
                    BuildingNumber = order.Shipment.BuildingNumber,
                    FloorNumber = order.Shipment.FloorNumber,
                    ApartmentNumber = order.Shipment.ApartmentNumber,
                    PhoneNumber = order.Shipment.PhoneNumber,
                    Notes = order.Shipment.Notes,
                    TrackingNumber = order.Shipment.TrackingNumber,
                    ShippingMethod = isArabic
                        ? order.Shipment.ShippingMethod?.NameAr ?? string.Empty
                        : order.Shipment.ShippingMethod?.NameEn ?? string.Empty,
                    ShipmentStatus = order.Shipment.Status.ToString()
                },

                Items = order.OrderItems.Select(i => new OrderItemDto
                {
                    ProductVariantId = i.ProductVariantId,
                    SKU = i.ProductVariant?.SKU ?? string.Empty,
                    ProductName = isArabic
                        ? i.ProductVariant?.Product?.NameAr ?? string.Empty
                        : i.ProductVariant?.Product?.NameEn ?? string.Empty,
                    AttributeKey = isArabic
                        ? i.ProductVariant?.Product.AttributeKeyAr
                        : i.ProductVariant?.Product.AttributeKeyEn,
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity,
                    TotalPrice = i.UnitPrice * i.Quantity,
                    AttributeValue = isArabic
                        ? i.ProductVariant?.AttributeValueAr
                        : i.ProductVariant?.AttributeValueEn,
                    Unit = i.ProductVariant?.Unit,
                    ColorLabel = i.ProductVariant?.ColorLabel,
                    ColorHex = i.ProductVariant?.ColorHex
                }).ToList()
            };
        }
    }
}