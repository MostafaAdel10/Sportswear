using MailKit.Net.Smtp;
using MimeKit;
using Serilog;
using Sportswear.DataAccess.Helpers;
using Sportswear.Service.Abstract;

namespace Sportswear.Service.Implementations
{
    public class EmailsService : IEmailsService
    {
        #region Fields
        private readonly EmailSettings _emailSettings;
        #endregion

        #region Constructors
        public EmailsService(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }
        #endregion

        #region Handle Functions
        public async Task SendEmailAsync(string email, string subject, string body)
        {
            try
            {
                using var client = new SmtpClient();

                await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, true);
                client.Authenticate(_emailSettings.FromEmail, _emailSettings.Password);

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = body,
                    TextBody = body
                };

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("ABOUTRIKA", _emailSettings.FromEmail));
                message.To.Add(new MailboxAddress(email, email));
                message.Subject = subject;
                message.Body = bodyBuilder.ToMessageBody();

                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                Log.Information("Email sent successfully to {Email}", email);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to send email to {Email}", email);
                throw; // ✅ رجّع الـ Exception عشان MassTransit يعمل retry
            }
        }
        #endregion
    }
}
