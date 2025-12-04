using AutoMapper;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Authorization.Queries.Models;
using Sportswear.Core.Features.Authorization.Queries.Response_DTO_;
using Sportswear.Core.Resources;
using Sportswear.DataAccess.Entities.Identity;
using Sportswear.DataAccess.Results;
using Sportswear.Service.Abstract;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace Sportswear.Core.Features.Authorization.Queries.Handlers
{
    public class RoleQueryHandler : ResponseHandler,
        IRequestHandler<GetRolesListQuery, Response<List<GetRolesListResponse>>>,
        IRequestHandler<GetRoleByIdQuery, Response<GetRoleByIdResponse>>,
        IRequestHandler<ManageUserRolesQuery, Response<ManageUserRolesResult>>
    {
        #region Fields
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly UserManager<ApplicationUser> _userManager;
        #endregion

        #region Constructors
        public RoleQueryHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                IAuthorizationService authorizationService,
                                IMapper mapper,
                                UserManager<ApplicationUser> userManager) : base(stringLocalizer)
        {
            _authorizationService = authorizationService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
            _userManager = userManager;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<List<GetRolesListResponse>>> Handle(GetRolesListQuery request, CancellationToken cancellationToken)
        {
            var roles = await _authorizationService.GetRolesList();
            var result = _mapper.Map<List<GetRolesListResponse>>(roles);
            return Success(result);
        }

        public async Task<Response<GetRoleByIdResponse>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var role = await _authorizationService.GetRoleById(request.Id);
            if (role == null) return NotFound<GetRoleByIdResponse>(_stringLocalizer[SharedResourcesKeys.RoleNotExist]);
            var result = _mapper.Map<GetRoleByIdResponse>(role);
            return Success(result);
        }

        public async Task<Response<ManageUserRolesResult>> Handle(ManageUserRolesQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null) return NotFound<ManageUserRolesResult>(_stringLocalizer[SharedResourcesKeys.UserIsNotFound]);
            var result = await _authorizationService.ManageUserRolesData(user);
            return Success(result);
        }
        #endregion
    }
}
