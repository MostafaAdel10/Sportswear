using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sportswear.Api.Base;
using Sportswear.Core.Features.ProductVariant.Commands.Models;
using Sportswear.Core.Features.ProductVariant.Queries.Models;
using Sportswear.DataAccess.AppMetaData;

namespace Sportswear.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductVariantController : AppControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet(Router.ProductVariantRouting.GetByIdToEdit)]
        public async Task<IActionResult> GetProductVariantByIdToEdit([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetProductVariantByIdToEditQuery(id));
            return NewResult(response);
        }

        [HttpPost(Router.ProductVariantRouting.CreateRange)]
        public async Task<IActionResult> CreateRange([FromBody] CreateProductVariantRangeCommand command)
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
