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
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public ProductVariantQueryHandler(
            IProductVariantService productVariantService,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _productVariantService = productVariantService;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<GetProductVariantByIdToEditResponse>> Handle(
            GetProductVariantByIdToEditQuery request,
            CancellationToken cancellationToken)
        {
            var variant = await _productVariantService.GetByIdWithIncludesAsync(request.Id);
            if (variant is null)
                return NotFound<GetProductVariantByIdToEditResponse>(
                    _stringLocalizer[SharedResourcesKeys.VariantNotFound]);

            var result = new GetProductVariantByIdToEditResponse
            {
                Id = variant.Id,
                SKU = variant.SKU,
                Price = variant.Price,
                StockQuantity = variant.StockQuantity,
                Attributes = variant.Attributes.Select(a => new VariantAttributeToEditDto
                {
                    TemplateId = a.ProductAttributeTemplateId,
                    KeyEn = a.ProductAttributeTemplate.KeyEn,
                    KeyAr = a.ProductAttributeTemplate.KeyAr,
                    Type = a.ProductAttributeTemplate.Type.ToString(),
                    ValueEn = a.ValueEn,
                    ValueAr = a.ValueAr,
                    ColorHex = a.ColorHex
                }).ToList()
            };

            return Success(result);
        }
        #endregion
    }
}
