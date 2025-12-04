using MediatR;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.Authentication.Queries.Models
{
    public class AuthorizeUserQuery : IRequest<Response<string>>
    {
        public string AccessToken { get; set; }
    }
}
