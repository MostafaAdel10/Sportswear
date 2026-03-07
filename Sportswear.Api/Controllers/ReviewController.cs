using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sportswear.Api.Base;
using Sportswear.Core.Features.Review.Commands.Models;
using Sportswear.Core.Features.Review.Queries.Models;
using Sportswear.DataAccess.AppMetaData;

namespace Sportswear.Api.Controllers
{
    public class ReviewController : AppControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet(Router.ReviewRouting.GetReviewsByProductId)]
        public async Task<IActionResult> GetReviewsByProductId([FromRoute] int productId)
        {
            var response = await Mediator.Send(new GetReviewsByProductIdQuery(productId));
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet(Router.ReviewRouting.GetById)]
        public async Task<IActionResult> GetReviewById([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetReviewByIdQuery(id));
            return NewResult(response);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost(Router.ReviewRouting.Create)]
        public async Task<IActionResult> Create([FromBody] AddReviewCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPut(Router.ReviewRouting.Edit)]
        public async Task<IActionResult> Edit([FromBody] EditReviewCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpDelete(Router.ReviewRouting.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await Mediator.Send(new DeleteReviewCommand(id));
            return NewResult(response);
        }
    }
}
