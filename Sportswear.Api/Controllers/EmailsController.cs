using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sportswear.Api.Base;
using Sportswear.Core.Features.Emails.Commands.Models;
using Sportswear.DataAccess.AppMetaData;

namespace Sportswear.Api.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin,User")]
    public class EmailsController : AppControllerBase
    {
        [HttpPost(Router.EmailsRoute.SendEmail)]
        public async Task<IActionResult> SendEmail([FromQuery] SendEmailCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
    }
}
