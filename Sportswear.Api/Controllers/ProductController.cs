using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Sportswear.Api.Base;
using Sportswear.Api.Helper;
using Sportswear.Core.Features.Product.Commands.Models;
using Sportswear.Core.Features.Product.Queries.Models;
using Sportswear.DataAccess.AppMetaData;

namespace Sportswear.Api.Controllers
{
    [EnableRateLimiting(RateLimitingPolicies.Api)]
    public class ProductController : AppControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet(Router.ProductRouting.GetFullDetails)]
        public async Task<IActionResult> GetFullDetails([FromRoute] int id)
        {
            return NewResult(await Mediator.Send(new GetProductFullDetailsQuery(id)));
        }

        [AllowAnonymous]
        [HttpGet(Router.ProductRouting.Paginated)]
        public async Task<IActionResult> GetProductsPaginated([FromQuery] GetProductPaginatedListQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet(Router.ProductRouting.List)]
        public async Task<IActionResult> GetProductsList()
        {
            var response = await Mediator.Send(new GetProductsListQuery());
            return NewResult(response);
        }

        [AllowAnonymous]
        [HttpGet(Router.ProductRouting.GetById)]
        public async Task<IActionResult> GetProductById([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetProductByIdQuery(id));
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet(Router.ProductRouting.GetByIdToEdit)]
        public async Task<IActionResult> GetProductByIdToEdit([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetProductByIdToEditQuery(id));
            return NewResult(response);
        }

        [AllowAnonymous]
        [HttpGet(Router.ProductRouting.GetByIdWithVariants)]
        public async Task<IActionResult> GetProductByIdWithVariants([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetProductByIdWithVariantsQuery(id));
            return NewResult(response);
        }

        [AllowAnonymous]
        [HttpGet(Router.ProductRouting.GetByCodeWithVariants)]
        [EnableRateLimiting(RateLimitingPolicies.Api)]
        public async Task<IActionResult> GetProductByCodeWithVariants([FromRoute] string code)
        {
            var response = await Mediator.Send(new GetProductByCodeWithVariantsQuery(code));
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(Router.ProductRouting.Create)]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut(Router.ProductRouting.Edit)]
        public async Task<IActionResult> Edit([FromBody] EditProductCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete(Router.ProductRouting.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return NewResult(await Mediator.Send(new DeleteProductCommand(id)));
        }
    }
}
