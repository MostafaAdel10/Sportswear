using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Sportswear.Api.Base;
using Sportswear.Api.Helper;
using Sportswear.Core.Features.ProductImage.Commands.Models;
using Sportswear.DataAccess.AppMetaData;

namespace Sportswear.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductImageController : AppControllerBase
    {
        [HttpPost(Router.ProductImageRouting.CreateProductImages)]
        [EnableRateLimiting(RateLimitingPolicies.Upload)]
        public async Task<IActionResult> CreateProductImages([FromForm] AddProductImagesCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPost(Router.ProductImageRouting.CreateProductImage)]
        [EnableRateLimiting(RateLimitingPolicies.Upload)]
        public async Task<IActionResult> CreateProductImage([FromForm] AddProductImageCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut(Router.ProductImageRouting.Edit)]
        [EnableRateLimiting(RateLimitingPolicies.Upload)]
        public async Task<IActionResult> Edit([FromForm] EditProductImageCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpDelete(Router.ProductImageRouting.Delete)]
        public async Task<IActionResult> Delete([FromBody] DeleteProductImageCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
    }
}
