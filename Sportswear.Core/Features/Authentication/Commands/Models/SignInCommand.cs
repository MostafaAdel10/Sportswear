using Sportswear.Core.Bases;
using Sportswear.DataAccess.Results;
using MediatR;

namespace Sportswear.Core.Features.Authentication.Commands.Models
{
    public class SignInCommand : IRequest<Response<JwtAuthResult>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
