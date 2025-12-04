namespace Sportswear.Core.Features.User.Queries.Response_DTO_
{
    public class GetUserPaginationReponse
    {
        public string Email { get; set; }


        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
    }
}
