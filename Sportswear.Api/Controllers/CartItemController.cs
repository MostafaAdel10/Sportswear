using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sportswear.Api.Base;
using Sportswear.Core.Features.CartItem.Commands.Models;
using Sportswear.Core.Features.CartItem.Queries.Models;
using Sportswear.DataAccess.AppMetaData;

namespace Sportswear.Api.Controllers
{
    [ApiController]
    [Authorize(Roles = "User")]
    public class CartItemController : AppControllerBase
    {
        [HttpGet(Router.CartItemRouting.List)]
        public async Task<IActionResult> GetCartItemsList()
        {
            var response = await Mediator.Send(new GetCartItemsListQuery());
            return Ok(response);
        }

        [HttpGet(Router.CartItemRouting.GetCartSummary)]
        public async Task<IActionResult> GetCartSummary()
        {
            var response = await Mediator.Send(new GetCartSummaryQuery());
            return Ok(response);
        }

        [HttpGet(Router.CartItemRouting.GetById)]
        public async Task<IActionResult> GetCartItemById([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetCartItemByIdQuery(id));
            return NewResult(response);
        }

        [HttpPost(Router.CartItemRouting.Create)]
        public async Task<IActionResult> Create([FromBody] AddCartItemCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut(Router.CartItemRouting.Edit)]
        public async Task<IActionResult> Edit([FromBody] EditCartItemCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpDelete(Router.CartItemRouting.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return NewResult(await Mediator.Send(new DeleteCartItemCommand(id)));
        }
    }
}
