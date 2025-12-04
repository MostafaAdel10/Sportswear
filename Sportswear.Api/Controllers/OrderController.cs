using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sportswear.Api.Base;
using Sportswear.Core.Features.Order.Commands.Models;
using Sportswear.Core.Features.Order.Queries.Models;
using Sportswear.DataAccess.AppMetaData;

namespace Sportswear.Api.Controllers
{
    public class OrderController : AppControllerBase
    {
        [Authorize(Roles = "Admin,User")]
        [HttpPost(Router.OrderRouting.Create)]
        public async Task<IActionResult> Create([FromBody] CreateOrderCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut(Router.OrderRouting.EditOrderStatus)]
        public async Task<IActionResult> ChangeOrderStatus([FromBody] ChangeOrderStatusCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut(Router.OrderRouting.EditPaymentStatus)]
        public async Task<IActionResult> ChangePaymentStatus([FromBody] ChangePaymentStatusCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet(Router.OrderRouting.GetById)]
        public async Task<IActionResult> GetOrderById([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetOrderByIdQuery(id));
            return NewResult(response);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet(Router.OrderRouting.MyOrders)]
        public async Task<IActionResult> GetOrderListForCurrentUser()
        {
            var response = await Mediator.Send(new GetOrderListForCurrentUserQuery());
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet(Router.OrderRouting.List)]
        public async Task<IActionResult> GetAllOrders()
        {
            var response = await Mediator.Send(new GetAllOrdersQuery());
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet(Router.OrderRouting.GetByUserId)]
        public async Task<IActionResult> GetOrdersByUser([FromRoute] int userId)
        {
            var response = await Mediator.Send(new GetOrdersByUserIdQuery(userId));
            return NewResult(response);
        }
    }
}
