using MediatR;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.PosSale.Queries.Models;
using Sportswear.Core.Features.PosSale.Queries.Response_DTO_;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;

namespace Sportswear.Core.Features.PosSale.Queries.Handlers
{
    public class PosSaleQueryHandler : ResponseHandler,
        IRequestHandler<GetPosSaleByIdQuery, Response<GetPosSaleByIdResponse>>,
        IRequestHandler<GetPosSalesListQuery, Response<List<GetPosSalesListResponse>>>
    {
        #region Fields
        private readonly IPosSaleService _posSaleService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructor
        public PosSaleQueryHandler(
            IPosSaleService posSaleService,
            IStringLocalizer<SharedResources> localizer) : base(localizer)
        {
            _posSaleService = posSaleService;
            _localizer = localizer;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<GetPosSaleByIdResponse>> Handle(
            GetPosSaleByIdQuery request, CancellationToken cancellationToken)
        {
            var posSale = await _posSaleService.GetByIdWithItemsAsync(request.Id);
            if (posSale == null)
                return NotFound<GetPosSaleByIdResponse>(
                    _localizer[SharedResourcesKeys.NotFound]);

            bool isArabic = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName.ToLower().Equals("ar");

            var response = new GetPosSaleByIdResponse
            {
                Id = posSale.Id,
                SaleNumber = posSale.SaleNumber,
                SaleDate = posSale.SaleDate,
                TotalAmount = posSale.TotalAmount,
                DiscountAmount = posSale.DiscountAmount,
                FinalAmount = posSale.FinalAmount,
                PaymentMethod = posSale.PaymentMethod.ToString(),
                Status = posSale.Status.ToString(),
                Notes = posSale.Notes,
                CreatedBy = posSale.CreatedBy,
                Items = posSale.Items.Select(i => new PosSaleItemResponse
                {
                    ProductVariantId = i.ProductVariantId,
                    ProductName = isArabic ? i.ProductVariant.Product.NameAr : i.ProductVariant.Product.NameEn,
                    AttributeValue = isArabic ? i.ProductVariant.AttributeValueAr : i.ProductVariant.AttributeValueEn,
                    Color = i.ProductVariant.ColorLabel,
                    SKU = i.ProductVariant.SKU,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    DiscountAmount = i.DiscountAmount,
                    TotalPrice = i.TotalPrice
                }).ToList()
            };

            return Success(response);
        }

        public async Task<Response<List<GetPosSalesListResponse>>> Handle(
            GetPosSalesListQuery request, CancellationToken cancellationToken)
        {
            var posSales = await _posSaleService.GetAllWithItemsAsync();

            var response = posSales.Select(s => new GetPosSalesListResponse
            {
                Id = s.Id,
                SaleNumber = s.SaleNumber,
                SaleDate = s.SaleDate,
                FinalAmount = s.FinalAmount,
                PaymentMethod = s.PaymentMethod.ToString(),
                Status = s.Status.ToString(),
                CreatedBy = s.CreatedBy,
                ItemsCount = s.Items.Count
            }).ToList();

            var result = Success(response);
            result.Meta = new { Count = response.Count };
            return result;
        }
        #endregion
    }
}
