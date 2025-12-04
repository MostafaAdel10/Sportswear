using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.ShippingMethod.Commands.Models;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;
using Sportswear.Service.AuthServices.Interfaces;

namespace Sportswear.Core.Features.ShippingMethod.Commands.Handlers
{
    public class ShippingMethodCommandHandler : ResponseHandler,
                        IRequestHandler<CreateShippingMethodCommand, Response<string>>,
                        IRequestHandler<EditShippingMethodCommand, Response<string>>,
                        IRequestHandler<DeleteShippingMethodCommand, Response<string>>
    {
        #region Fields
        private readonly IShippingMethodService _shippingMethodService;
        private readonly IShipmentService _shipmentService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public ShippingMethodCommandHandler(IShippingMethodService shippingMethodService,
            ICurrentUserService currentUserService,
            IMapper mapper,
            IShipmentService shipmentService,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _shippingMethodService = shippingMethodService;
            _shipmentService = shipmentService;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handel Functions
        public async Task<Response<string>> Handle(CreateShippingMethodCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetUserAsync();
            if (currentUser == null || string.IsNullOrEmpty(currentUser.UserName))
                return Unauthorized<string>();

            var shippingMethod = _mapper.Map<DataAccess.Entities.ShippingMethod>(request);

            shippingMethod.CreatedBy = currentUser.UserName;

            var isSuccess = await _shippingMethodService.AddAsync(shippingMethod);

            if (isSuccess)
                return Created("");
            else
                return BadRequest<string>();
        }

        public async Task<Response<string>> Handle(EditShippingMethodCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetUserAsync();
            if (currentUser == null || string.IsNullOrEmpty(currentUser.UserName))
                return Unauthorized<string>();

            // Check if shippingMethod exists
            var existingShippingMethod = await _shippingMethodService.GetByIdAsync(request.Id);
            if (existingShippingMethod == null)
                return NotFound<string>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            // Map new values to existing entity
            existingShippingMethod = _mapper.Map(request, existingShippingMethod);

            existingShippingMethod.UpdatedBy = currentUser.UserName;
            existingShippingMethod.UpdatedAt = DateTime.UtcNow;

            var isSuccess = await _shippingMethodService.EditAsync(existingShippingMethod);

            if (isSuccess)
                return Success<string>(_stringLocalizer[SharedResourcesKeys.Updated]);
            else
                return BadRequest<string>();
        }

        public async Task<Response<string>> Handle(DeleteShippingMethodCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetUserAsync();
            if (currentUser == null || string.IsNullOrEmpty(currentUser.UserName))
                return Unauthorized<string>();

            // Check if ShippingMethod exists
            var existingShippingMethod = await _shippingMethodService.GetByIdAsync(request.Id);
            if (existingShippingMethod == null)
                return NotFound<string>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            // Check if shippingMethod has related Shipments
            var hasShipments = await _shipmentService.IsAnyShipmentRelatedToShippingMethodAsync(request.Id);
            if (hasShipments)
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.RelatedProducts]);

            // Soft delete
            existingShippingMethod.IsDeleted = true;
            existingShippingMethod.UpdatedAt = DateTime.UtcNow;
            existingShippingMethod.UpdatedBy = currentUser.UserName;

            var isSuccess = await _shippingMethodService.EditAsync(existingShippingMethod);

            return isSuccess
                ? Success<string>(_stringLocalizer[SharedResourcesKeys.Deleted])
                : BadRequest<string>();
        }
        #endregion
    }
}
