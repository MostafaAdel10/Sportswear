using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sportswear.Api.Base;
using Sportswear.Core.Features.ProductVariant.Commands.Models;
using Sportswear.DataAccess.AppMetaData;

namespace Sportswear.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductVariantController : AppControllerBase
    {
        [HttpPost(Router.ProductVariantRouting.Create)]
        public async Task<IActionResult> Create([FromBody] CreateProductVariantCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut(Router.ProductVariantRouting.Edit)]
        public async Task<IActionResult> Edit([FromBody] EditProductVariantCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpDelete(Router.ProductVariantRouting.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return NewResult(await Mediator.Send(new DeleteProductVariantCommand(id)));
        }

    }
}
