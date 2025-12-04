using Sportswear.Core.Bases;
using MediatR;

namespace Sportswear.Core.Features.Authentication.Commands.Models
{
    public class SendResetPasswordCommand : IRequest<Response<string>>
    {
        public string Email { get; set; }
    }
}
