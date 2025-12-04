using MediatR;
using Sportswear.Core.Bases;

namespace Sportswear.Core.Features.User.Commands.Models
{
    public class EditUserCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string? PhoneNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
    }
}
