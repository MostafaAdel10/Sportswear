using MediatR;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Order.Commands.Models;
using Sportswear.Core.Resources;
using Sportswear.DataAccess.Entities;
using Sportswear.DataAccess.Enums;
using Sportswear.Service.Abstract;
using Sportswear.Service.AuthServices.Interfaces;

namespace Sportswear.Core.Features.Order.Commands.Handlers
{
    public class OrderCommandHandler : ResponseHandler,
                    IRequestHandler<CreateOrderCommand, Response<int>>,
                    IRequestHandler<ChangeOrderStatusCommand, Response<string>>,
                    IRequestHandler<ChangePaymentStatusCommand, Response<string>>,
                    IRequestHandler<CancelOrderCommand, Response<string>>
    {
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;
        private readonly IProductService _productService;
        private readonly IProductVariantService _productVariantService;
        private readonly ICartService _cartService;
        private readonly ICartItemService _cartItemService;
        private readonly IPaymentService _paymentService;
        private readonly IShipmentService _shipmentService;
        private readonly IShippingMethodService _shippingMethodService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;

        public OrderCommandHandler(
            IOrderService orderService,
            IOrderItemService orderItemService,
            IProductService productService,
            IProductVariantService productVariantService,
            ICartService cartService,
            ICartItemService cartItemService,
            IPaymentService paymentService,
            IShipmentService shipmentService,
            IShippingMethodService shippingMethodService,
            ICurrentUserService currentUserService,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _orderService = orderService;
            _orderItemService = orderItemService;
            _productService = productService;
            _productVariantService = productVariantService;
            _cartService = cartService;
            _cartItemService = cartItemService;
            _paymentService = paymentService;
            _shipmentService = shipmentService;
            _shippingMethodService = shippingMethodService;
            _currentUserService = currentUserService;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<Response<int>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetUserAsync();
            int userId = currentUser.Id;

            var checkStockAvailability = await _cartItemService.CheckStockAvailabilityAsync(userId);
            if (!checkStockAvailability)
                return BadRequest<int>(_stringLocalizer[SharedResourcesKeys.InsufficientStock]);

            // 1️⃣ Get cart items by user
            var cartItems = await _cartItemService.GetCartItemsByUserIdAsync(userId);
            if (cartItems == null || !cartItems.Any())
                return BadRequest<int>(_stringLocalizer[SharedResourcesKeys.TheShoppingCartIsEmpty]);

            // Get Shipping Method
            var shippingMethod = await _shippingMethodService.GetByIdAsync(request.ShippingMethodId);
            if (shippingMethod == null)
                return BadRequest<int>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            // 2️⃣ Calculate total
            decimal totalAmount = 0;

            foreach (var item in cartItems)
            {
                var product = item.ProductVariant.Product;
                var originalPrice = item.ProductVariant.Price;

                var discountedPrice = _productService.CalculateDiscountedPriceOnProductVariants(product, originalPrice)
                                     ?? originalPrice;

                totalAmount += discountedPrice * item.Quantity;
            }

            // 3️⃣ Create order
            var order = new DataAccess.Entities.Order
            {
                UserId = userId,
                TotalAmount = totalAmount,
                Status = OrderStatus.Pending,
                CreatedBy = currentUser.UserName
            };
            var orderId = await _orderService.AddAsync(order);
            if (orderId <= 0)
                return BadRequest<int>(_stringLocalizer[SharedResourcesKeys.BadRequest]);

            // 4️⃣ Add order items 
            foreach (var item in cartItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = orderId,
                    ProductVariantId = item.ProductVariantId,
                    UnitPrice = item.ProductVariant.Price,
                    Quantity = item.Quantity
                };
                await _orderItemService.AddAsync(orderItem);
            }

            // 5️⃣ Create Payment (Default: CashOnDelivery)
            var payment = new Payment
            {
                OrderId = orderId,
                Method = PaymentMethod.CashOnDelivery, // DEFAULT 🚀
                Status = PaymentStatus.Pending // Until delivered
            };
            await _paymentService.AddAsync(payment);

            // 6️⃣ Create Shipment
            var shipment = new Shipment
            {
                OrderId = orderId,
                FullName = request.Shipment.FullName,
                City = request.Shipment.City,
                Country = request.Shipment.Country,
                Region = request.Shipment.Region,
                StreetAddress = request.Shipment.StreetAddress,
                BuildingNumber = request.Shipment.BuildingNumber,
                FloorNumber = request.Shipment.FloorNumber,
                ApartmentNumber = request.Shipment.ApartmentNumber,
                PhoneNumber = request.Shipment.PhoneNumber,
                Notes = request.Shipment.Notes,
                ShippingMethodId = request.ShippingMethodId,
                Status = ShippingStatus.Processing
            };
            await _shipmentService.AddAsync(shipment);

            // 7️⃣ Clear Cart
            await _cartItemService.ClearCartAsync(userId);

            return Success(orderId, _stringLocalizer[SharedResourcesKeys.Created]);
        }

        public async Task<Response<string>> Handle(ChangeOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetByIdAsync(request.OrderId);
            if (order == null)
                return NotFound<string>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            // Validate transition
            if (!_orderService.CanChangeOrderStatusAsync(order, request.Status))
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.InvalidOrderStatusTransition]);

            order.Status = request.Status;

            await _orderService.EditAsync(order);

            return Success<string>(_stringLocalizer[SharedResourcesKeys.Updated]);
        }

        public async Task<Response<string>> Handle(ChangePaymentStatusCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetOrderWithDetailsAsync(request.OrderId);
            if (order == null)
                return NotFound<string>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            // Rule enforcement
            if (!_paymentService.CanChangePaymentStatus(order.Payment, request.Status, order.Status))
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.InvalidPaymentStatusTransition]);

            if (order.Payment == null)
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.NoPaymentFound]);

            order.Payment.Status = request.Status;

            // لو الدفع تم الآن
            if (request.Status == PaymentStatus.Completed)
                order.Payment.PaidAt = DateTime.UtcNow;

            await _paymentService.EditAsync(order.Payment);

            return Success<string>(_stringLocalizer[SharedResourcesKeys.Updated]);
        }

        public async Task<Response<string>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetOrderWithDetailsAsync(request.OrderId);

            if (order == null)
                return NotFound<string>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            // 🛑 لو الطلب خارج الشحن أو وصل، ممنوع الإلغاء
            if (order.Status == OrderStatus.Shipped || order.Status == OrderStatus.Completed)
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.OrderCannotBeCanceled]);

            // 1️⃣ تغيير حالة الطلب
            order.Status = OrderStatus.Cancelled;
            await _orderService.EditAsync(order);

            // 2️⃣ تغيير حالة الدفع لو موجود
            if (order.Payment != null)
            {
                order.Payment.Status = PaymentStatus.Failed;
                await _paymentService.EditAsync(order.Payment);
            }

            // 3️⃣ إعادة المنتجات للمخزون
            foreach (var item in order.OrderItems)
            {
                var productVariant = item.ProductVariant;
                productVariant.StockQuantity += item.Quantity;
                await _productVariantService.EditStockOnlyAsync(productVariant);
            }

            return Success<string>(_stringLocalizer[SharedResourcesKeys.CanceledOrder]);
        }
    }
}
