using MediatR;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.DashboardOverview.Queries.Response_DTO_;

namespace Sportswear.Core.Features.DashboardOverview.Queries.Models
{
    public class GetDashboardOverviewQuery : IRequest<Response<DashboardOverviewResponse>>
    {
        public int Days { get; set; } = 30; // آخر 30 يوم default
    }
}
