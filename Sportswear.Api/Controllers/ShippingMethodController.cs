using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sportswear.Api.Base;
using Sportswear.Core.Features.ShippingMethod.Commands.Models;
using Sportswear.Core.Features.ShippingMethod.Queries.Models;
using Sportswear.DataAccess.AppMetaData;

namespace Sportswear.Api.Controllers
{
    public class ShippingMethodController : AppControllerBase
    {
        [Authorize(Roles = "Admin,User")]
        [HttpGet(Router.ShippingMethodRouting.List)]
        public async Task<IActionResult> GetShippingMethodsList()
        {
            var response = await Mediator.Send(new GetShippingMethodsListQuery());
            return Ok(response);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet(Router.ShippingMethodRouting.GetById)]
        public async Task<IActionResult> GetShippingMethodById([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetShippingMethodByIdQuery(id));
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(Router.ShippingMethodRouting.Create)]
        public async Task<IActionResult> Create([FromBody] CreateShippingMethodCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut(Router.ShippingMethodRouting.Edit)]
        public async Task<IActionResult> Edit([FromBody] EditShippingMethodCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete(Router.ShippingMethodRouting.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return NewResult(await Mediator.Send(new DeleteShippingMethodCommand(id)));
        }
    }
}
