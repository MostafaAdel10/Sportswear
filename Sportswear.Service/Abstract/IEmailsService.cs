namespace Sportswear.Service.Abstract
{
    public interface IEmailsService
    {
        public Task SendEmailAsync(string email, string subject, string body);
    }
}
