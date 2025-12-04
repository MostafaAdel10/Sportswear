using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sportswear.Api.Base;
using Sportswear.Core.Features.Product_Discount.Commands.Models;
using Sportswear.Core.Features.Product_Discount.Queries.Models;
using Sportswear.DataAccess.AppMetaData;

namespace Sportswear.Api.Controllers
{
    public class Product_DiscountController : AppControllerBase
    {
        [AllowAnonymous]
        [HttpGet(Router.Product_DiscountRouting.Paginated)]
        public async Task<IActionResult> GetProductsByDiscountIdPaginated([FromQuery] GetProductsByDiscountIdQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(Router.Product_DiscountRouting.Create)]
        public async Task<IActionResult> AddDiscountToProducts([FromBody] AddDiscountToProductsCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut(Router.Product_DiscountRouting.Edit)]
        public async Task<IActionResult> UpdateProductsForDiscount([FromBody] UpdateProductsForDiscountCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete(Router.Product_DiscountRouting.Delete)]
        public async Task<IActionResult> RemoveDiscountFromProducts([FromBody] RemoveDiscountFromProductsCommand command)
        {
            return NewResult(await Mediator.Send(command));
        }

    }
}
