using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sportswear.Api.Base;
using Sportswear.Core.Features.Discount.Commands.Models;
using Sportswear.Core.Features.Discount.Queries.Models;
using Sportswear.DataAccess.AppMetaData;
using Sportswear.DataAccess.Enums;

namespace Sportswear.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DiscountController : AppControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet(Router.DiscountRouting.GetAll)]
        public async Task<IActionResult> GetAllDiscounts([FromQuery] DiscountStatusFilter status = DiscountStatusFilter.All)
        {
            return NewResult(await Mediator.Send(new GetAllDiscountsQuery { Status = status }));
        }

        [HttpGet(Router.DiscountRouting.List)]
        public async Task<IActionResult> GetActiveDiscountsList()
        {
            var response = await Mediator.Send(new GetActiveDiscountsQuery());
            return Ok(response);
        }

        [HttpGet(Router.DiscountRouting.GetById)]
        public async Task<IActionResult> GetActiveDiscountById([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetActiveDiscountByIdQuery(id));
            return NewResult(response);
        }

        [HttpGet(Router.DiscountRouting.GetByIdToEdit)]
        public async Task<IActionResult> GetActiveDiscountByIdToEdit([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetActiveDiscountByIdToEditQuery(id));
            return NewResult(response);
        }

        [HttpPost(Router.DiscountRouting.Create)]
        public async Task<IActionResult> Create([FromBody] CreateDiscountCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut(Router.DiscountRouting.Edit)]
        public async Task<IActionResult> Edit([FromBody] EditDiscountCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpDelete(Router.DiscountRouting.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return NewResult(await Mediator.Send(new DeleteDiscountCommand(id)));
        }

    }
}
