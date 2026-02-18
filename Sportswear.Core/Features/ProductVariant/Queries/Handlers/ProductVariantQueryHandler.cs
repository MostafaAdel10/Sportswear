using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.ProductVariant.Queries.Models;
using Sportswear.Core.Features.ProductVariant.Queries.Response_DTO_;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;

namespace Sportswear.Core.Features.ProductVariant.Queries.Handlers
{
    public class ProductVariantQueryHandler : ResponseHandler,
            IRequestHandler<GetProductVariantByIdToEditQuery, Response<GetProductVariantByIdToEditResponse>>
    {
        #region Fields
        private readonly IProductVariantService _productVariantService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public ProductVariantQueryHandler(IProductVariantService productVariantService, IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _productVariantService = productVariantService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handel Functions
        public async Task<Response<GetProductVariantByIdToEditResponse>> Handle(GetProductVariantByIdToEditQuery request, CancellationToken cancellationToken)
        {
            var productVariant = await _productVariantService.GetByIdAsync(request.Id);

            if (productVariant is null)
                return NotFound<GetProductVariantByIdToEditResponse>(_stringLocalizer[SharedResourcesKeys.VariantNotFound]);

            var result = _mapper.Map<GetProductVariantByIdToEditResponse>(productVariant);

            return Success(result);
        }
        #endregion
    }
}
