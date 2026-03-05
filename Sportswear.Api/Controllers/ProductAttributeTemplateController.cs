using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sportswear.Api.Base;
using Sportswear.Core.Features.ProductAttributeTemplate.Commands.Models;
using Sportswear.Core.Features.ProductAttributeTemplate.Queries.Models;
using Sportswear.DataAccess.AppMetaData;

namespace Sportswear.Api.Controllers
{
    public class ProductAttributeTemplateController : AppControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet(Router.AttributeTemplateRouting.GetByCategoryId)]
        public async Task<IActionResult> GetByCategoryId([FromRoute] int categoryId)
        {
            var response = await Mediator.Send(new GetAttributeTemplatesByCategoryIdQuery(categoryId));
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(Router.AttributeTemplateRouting.Create)]
        public async Task<IActionResult> Create([FromBody] CreateAttributeTemplateRangeCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete(Router.AttributeTemplateRouting.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return NewResult(await Mediator.Send(new DeleteAttributeTemplateCommand(id)));
        }
    }
}
