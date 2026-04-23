using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Sportswear.Api.Base;
using Sportswear.Api.Helper;
using Sportswear.Core.Features.PosSale.Commands.Models;
using Sportswear.Core.Features.PosSale.Queries.Models;
using Sportswear.DataAccess.AppMetaData;

namespace Sportswear.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [EnableRateLimiting(RateLimitingPolicies.Api)]
    public class PosSaleController : AppControllerBase
    {
        [HttpGet(Router.PosSaleRouting.List)]
        public async Task<IActionResult> GetPosSalesList()
        {
            var response = await Mediator.Send(new GetPosSalesListQuery());
            return NewResult(response);
        }

        [HttpGet(Router.PosSaleRouting.GetById)]
        public async Task<IActionResult> GetPosSaleById([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetPosSaleByIdQuery(id));
            return NewResult(response);
        }

        [HttpPost(Router.PosSaleRouting.Create)]
        public async Task<IActionResult> Create([FromBody] CreatePosSaleCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpDelete(Router.PosSaleRouting.Cancel)]
        public async Task<IActionResult> Cancel([FromRoute] int id)
        {
            var response = await Mediator.Send(new CancelPosSaleCommand(id));
            return NewResult(response);
        }
    }
}
