using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Sportswear.Api.Base;
using Sportswear.Api.Helper;
using Sportswear.Core.Features.Brand.Commands.Models;
using Sportswear.Core.Features.Brand.Queries.Models;
using Sportswear.DataAccess.AppMetaData;

namespace Sportswear.Api.Controllers
{
    [EnableRateLimiting(RateLimitingPolicies.Api)]
    public class BrandController : AppControllerBase
    {
        [AllowAnonymous]
        [HttpGet(Router.BrandRouting.List)]
        public async Task<IActionResult> GetBrandsList()
        {
            var response = await Mediator.Send(new GetBrandsListQuery());
            return NewResult(response);
        }

        [AllowAnonymous]
        [HttpGet(Router.BrandRouting.GetById)]
        public async Task<IActionResult> GetBrandById([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetBrandByIdQuery(id));
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet(Router.BrandRouting.GetByIdToEdit)]
        public async Task<IActionResult> GetBrandByIdToEdit([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetBrandByIdToEditQuery(id));
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(Router.BrandRouting.Create)]
        public async Task<IActionResult> Create([FromForm] CreateBrandCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut(Router.BrandRouting.Edit)]
        public async Task<IActionResult> Edit([FromForm] EditBrandCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete(Router.BrandRouting.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return NewResult(await Mediator.Send(new DeleteBrandCommand(id)));
        }

    }
}
