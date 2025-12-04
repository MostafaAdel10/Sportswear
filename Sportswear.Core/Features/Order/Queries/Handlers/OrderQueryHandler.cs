using AutoMapper;
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
    IRequestHandler<GetOrderByIdQuery, Response<OrderDto>>,
    IRequestHandler<GetOrderListForCurrentUserQuery, Response<List<OrderDto>>>,
    IRequestHandler<GetAllOrdersQuery, Response<List<AdminOrderListDto>>>,
    IRequestHandler<GetOrdersByUserIdQuery, Response<List<AdminOrderListDto>>>
    {
        private readonly IOrderService _orderService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;

        public OrderQueryHandler(
            IOrderService orderService,
            ICurrentUserService currentUserService,
            IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _orderService = orderService;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }

        // 1️⃣ Get order by id
        public async Task<Response<OrderDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetOrderWithDetailsAsync(request.OrderId);

            if (order == null)
                return NotFound<OrderDto>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            // Optional check → user can access only their orders
            var userId = _currentUserService.GetUserId();
            if (order.UserId != userId)
                return Unauthorized<OrderDto>(_stringLocalizer[SharedResourcesKeys.UnAuthorized]);

            var dto = new OrderDto
            {
                Id = order.Id,
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString(),
                CreatedAt = order.CreatedAt
            };

            return Success(dto);
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

            var dtoList = orders.Select(order => new OrderDto
            {
                Id = order.Id,
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString(),
                CreatedAt = order.CreatedAt
            }).ToList();

            return Success(dtoList);
        }

        public async Task<Response<List<AdminOrderListDto>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderService.GetAllOrdersWithDetailsAsync(); // لازم نعمل الميثود دي في السيرفيس

            var dtoList = orders.Select(o => new AdminOrderListDto
            {
                OrderId = o.Id,
                UserId = o.UserId,
                UserEmail = o.User.Email,
                TotalAmount = o.TotalAmount,
                OrderStatus = o.Status.ToString(),
                PaymentStatus = o.Payment.Status.ToString(),
                CreatedAt = o.CreatedAt
            }).ToList();

            return Success(dtoList);
        }

        public async Task<Response<List<AdminOrderListDto>>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderService.GetOrdersByUserAsync(request.UserId);

            if (!orders.Any())
                return Success(new List<AdminOrderListDto>(), _stringLocalizer[SharedResourcesKeys.TheOrderIsEmpty]);

            var dtoList = orders.Select(o => new AdminOrderListDto
            {
                OrderId = o.Id,
                UserId = o.UserId,
                UserEmail = o.User.Email,
                TotalAmount = o.TotalAmount,
                OrderStatus = o.Status.ToString(),
                PaymentStatus = o.Payment.Status.ToString(),
                CreatedAt = o.CreatedAt
            }).ToList();

            return Success(dtoList);
        }
    }
}