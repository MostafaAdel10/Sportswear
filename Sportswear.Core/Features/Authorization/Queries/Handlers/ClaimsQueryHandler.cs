using Sportswear.Core.Bases;
using Sportswear.Core.Features.Authorization.Queries.Models;
using Sportswear.Core.Resources;
using Sportswear.DataAccess.Entities.Identity;
using Sportswear.DataAccess.Results;
using Sportswear.Service.Abstract;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace Sportswear.Core.Features.Authorization.Queries.Handlers
{
    public class ClaimsQueryHandler : ResponseHandler,
        IRequestHandler<ManageUserClaimsQuery, Response<ManageUserClaimsResult>>
    {
        #region Fields
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public ClaimsQueryHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                  IAuthorizationService authorizationService,
                                  UserManager<ApplicationUser> userManager) : base(stringLocalizer)
        {
            _authorizationService = authorizationService;
            _userManager = userManager;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<ManageUserClaimsResult>> Handle(ManageUserClaimsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null) return NotFound<ManageUserClaimsResult>(_stringLocalizer[SharedResourcesKeys.UserIsNotFound]);
            var result = await _authorizationService.ManageUserClaimData(user);
            return Success(result);
        }
        #endregion
    }
}
