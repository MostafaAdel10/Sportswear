using MediatR;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Emails.Commands.Models;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;

namespace Sportswear.Core.Features.Emails.Commands.Handlers
{
    public class EmailsCommandHandler : ResponseHandler,
        IRequestHandler<SendEmailCommand, Response<string>>
    {
        #region Fields
        private readonly IEmailsService _emailsService;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public EmailsCommandHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                    IEmailsService emailsService) : base(stringLocalizer)
        {
            _emailsService = emailsService;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<string>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _emailsService.SendEmailAsync(
                    request.Email,
                    request.Subject ?? "Message from Aboutrika store",
                    request.Message);

                return Success<string>(_stringLocalizer[SharedResourcesKeys.EmailSentSuccessfully]);
            }
            catch
            {
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.SendEmailFailed]);
            }
        }
        #endregion
    }
}
