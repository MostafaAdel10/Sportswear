using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Sportswear.Api.Base;
using Sportswear.Api.Helper;
using Sportswear.Core.Features.DashboardOverview.Queries.Models;
using Sportswear.DataAccess.AppMetaData;

namespace Sportswear.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [EnableRateLimiting(RateLimitingPolicies.Api)]
    public class DashboardOverviewController : AppControllerBase
    {
        [HttpGet(Router.DashboardOverviewRouting.Overview)]
        public async Task<IActionResult> GetOverview([FromQuery] GetDashboardOverviewQuery query)
        {
            var response = await Mediator.Send(query);
            return NewResult(response);
        }
    }
}
