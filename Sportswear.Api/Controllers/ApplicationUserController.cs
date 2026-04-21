using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Sportswear.Api.Base;
using Sportswear.Api.Helper;
using Sportswear.Core.Features.User.Commands.Models;
using Sportswear.Core.Features.User.Queries.Models;
using Sportswear.DataAccess.AppMetaData;

namespace Sportswear.Api.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    public class ApplicationUserController : AppControllerBase
    {
        [AllowAnonymous]
        [HttpPost(Router.ApplicationUserRouting.Create)]
        [EnableRateLimiting(RateLimitingPolicies.Register)]
        public async Task<IActionResult> Create([FromBody] AddUserCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut(Router.ApplicationUserRouting.Edit)]
        [EnableRateLimiting(RateLimitingPolicies.Api)]
        public async Task<IActionResult> Edit([FromBody] EditUserCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpDelete(Router.ApplicationUserRouting.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return NewResult(await Mediator.Send(new DeleteUserCommand(id)));
        }

        [HttpPut(Router.ApplicationUserRouting.ChangePassword)]
        [EnableRateLimiting(RateLimitingPolicies.ResetPassword)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangeUserPasswordCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet(Router.ApplicationUserRouting.Paginated)]
        public async Task<IActionResult> Paginated([FromQuery] GetUserPaginationQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [HttpGet(Router.ApplicationUserRouting.GetByID)]
        public async Task<IActionResult> GetUserByID([FromRoute] int id)
        {
            return NewResult(await Mediator.Send(new GetUserByIdQuery(id)));
        }

    }
}
