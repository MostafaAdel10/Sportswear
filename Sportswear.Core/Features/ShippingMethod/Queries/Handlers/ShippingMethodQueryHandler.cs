using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.ShippingMethod.Queries.Models;
using Sportswear.Core.Features.ShippingMethod.Queries.Response_DTO_;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;

namespace Sportswear.Core.Features.ShippingMethod.Queries.Handlers
{
    public class ShippingMethodQueryHandler : ResponseHandler,
        IRequestHandler<GetShippingMethodsListQuery, Response<List<GetShippingMethodsListResponse>>>,
        IRequestHandler<GetShippingMethodByIdQuery, Response<GetShippingMethodByIdResponse>>
    {
        #region Fields
        private readonly IShippingMethodService _shippingMethodService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public ShippingMethodQueryHandler(IShippingMethodService shippingMethodService, IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _shippingMethodService = shippingMethodService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handel Functions
        public async Task<Response<List<GetShippingMethodsListResponse>>> Handle(GetShippingMethodsListQuery request, CancellationToken cancellationToken)
        {
            var shippingMethodsList = await _shippingMethodService.GetShippingMethodsListAsync();
            var shippingMethodsListMapper = _mapper.Map<List<GetShippingMethodsListResponse>>(shippingMethodsList);

            var result = Success(shippingMethodsListMapper);
            result.Meta = new { Count = shippingMethodsListMapper.Count() };
            return result;
        }

        public async Task<Response<GetShippingMethodByIdResponse>> Handle(GetShippingMethodByIdQuery request, CancellationToken cancellationToken)
        {
            var shippingMethod = await _shippingMethodService.GetByIdAsync(request.Id);

            if (shippingMethod is null)
                return NotFound<GetShippingMethodByIdResponse>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            var result = _mapper.Map<GetShippingMethodByIdResponse>(shippingMethod);

            return Success(result);
        }
        #endregion
    }
}
