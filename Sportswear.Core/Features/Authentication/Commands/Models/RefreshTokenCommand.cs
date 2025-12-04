using Sportswear.Core.Bases;
using Sportswear.DataAccess.Results;
using MediatR;

namespace Sportswear.Core.Features.Authentication.Commands.Models
{
    public class RefreshTokenCommand : IRequest<Response<JwtAuthResult>>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
