using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sportswear.Api.Base;
using Sportswear.Core.Features.Category.Commands.Models;
using Sportswear.Core.Features.Category.Queries.Models;
using Sportswear.DataAccess.AppMetaData;

namespace Sportswear.Api.Controllers
{
    public class CategoryController : AppControllerBase
    {
        [AllowAnonymous]
        [HttpGet(Router.CategoryRouting.List)]
        public async Task<IActionResult> GetCategoriesList()
        {
            var response = await Mediator.Send(new GetCategoriesListQuery());
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet(Router.CategoryRouting.GetById)]
        public async Task<IActionResult> GetCategoryById([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetCategoryByIdQuery(id));
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet(Router.CategoryRouting.GetByIdToEdit)]
        public async Task<IActionResult> GetCategoryByIdToEdit([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetCategoryByIdToEditQuery(id));
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(Router.CategoryRouting.Create)]
        public async Task<IActionResult> Create([FromForm] CreateCategoryCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut(Router.CategoryRouting.Edit)]
        public async Task<IActionResult> Edit([FromForm] EditCategoryCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete(Router.CategoryRouting.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return NewResult(await Mediator.Send(new DeleteCategoryCommand(id)));
        }
    }
}
