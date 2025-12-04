using MediatR;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Authorization.Commands.Models;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;

namespace Sportswear.Core.Features.Authorization.Commands.Handlers
{
    public class ClaimsCommandHandler : ResponseHandler,
        IRequestHandler<UpdateUserClaimsCommand, Response<string>>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IAuthorizationService _authorizationService;
        #endregion

        #region Constructors
        public ClaimsCommandHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                    IAuthorizationService authorizationService) : base(stringLocalizer)
        {
            _authorizationService = authorizationService;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<string>> Handle(UpdateUserClaimsCommand request, CancellationToken cancellationToken)
        {
            var result = await _authorizationService.UpdateUserClaims(request);
            switch (result)
            {
                case "UserIsNull": return NotFound<string>(_stringLocalizer[SharedResourcesKeys.UserIsNotFound]);
                case "FailedToRemoveOldClaims": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToRemoveOldClaims]);
                case "FailedToAddNewClaims": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToAddNewClaims]);
                case "FailedToUpdateClaims": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToUpdateClaims]);
            }
            return Success<string>(_stringLocalizer[SharedResourcesKeys.Success]);
        }
        #endregion
    }
}
