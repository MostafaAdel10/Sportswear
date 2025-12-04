using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Product_Discount.Queries.Models;
using Sportswear.Core.Features.Product_Discount.Queries.Response_DTO_;
using Sportswear.Core.Resources;
using Sportswear.Core.Wrappers;
using Sportswear.Service.Abstract;

namespace Sportswear.Core.Features.Product_Discount.Queries.Handlers
{
    public class Product_DiscountQueryHandler : ResponseHandler,
        IRequestHandler<GetProductsByDiscountIdQuery, PaginatedResult<GetProductsByDiscountIdResponse>>
    {
        #region Fields
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public Product_DiscountQueryHandler(IProductService productService,
                                            IMapper mapper,
                                            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _productService = productService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handel Functions
        public async Task<PaginatedResult<GetProductsByDiscountIdResponse>> Handle(GetProductsByDiscountIdQuery request, CancellationToken cancellationToken)
        {
            //pagination
            var now = DateTime.UtcNow;
            var query = _productService.FilterProductPaginatedQueryable(request.Ordering, request.Search);

            // فلتر الخصم النشط فقط
            query = query.Where(p => p.Product_Discounts.Any(pd =>
                pd.DiscountId == request.DiscountId &&
                pd.Discount.StartDate <= now &&
                pd.Discount.EndDate >= now &&
                !pd.Discount.IsDeleted));

            bool isArabic = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName.ToLower().Equals("ar");

            var result = await query.Select(p => new GetProductsByDiscountIdResponse
            {
                Id = p.Id,
                Code = p.Code,
                Name = isArabic ? p.NameAr : p.NameEn,
                Description = isArabic ? p.DescriptionAr : p.DescriptionEn,
                Club = isArabic ? p.ClubAr : p.ClubEn,
                BrandName = isArabic ? p.Brand.NameAr : p.Brand.NameEn,
                CategoryName = isArabic ? p.Category.NameAr : p.Category.NameEn,
                BasePrice = p.BasePrice,
                PriceAfterDiscount = _productService.CalculateDiscountedPriceOnProduct(p) ?? p.BasePrice,
                Season = p.Season,
                Images = p.Images.Select(i => i.Url).ToList()
            }).ToPaginatedListAsync(request.PageNumber, request.PageSize);

            result.Meta = new { Count = result.Data.Count() };
            return result;
        }
        #endregion
    }
}
