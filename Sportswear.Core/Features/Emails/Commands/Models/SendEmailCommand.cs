using MediatR;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.Emails.Commands.Models
{
    public class SendEmailCommand : IRequest<Response<string>>
    {
        public string Email { get; set; }
        public string Message { get; set; }
        public string? Subject { get; set; }
    }
}
