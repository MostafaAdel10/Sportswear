using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Product.Queries.Models;
using Sportswear.Core.Features.Product.Queries.Response_DTO_;
using Sportswear.Core.Resources;
using Sportswear.Core.Wrappers;
using Sportswear.Service.Abstract;

namespace Sportswear.Core.Features.Product.Queries.Handlers
{
    public class ProductQueryHandler : ResponseHandler,
        IRequestHandler<GetProductsListQuery, Response<List<GetProductsListResponse>>>,
        IRequestHandler<GetProductByIdQuery, Response<GetProductByIdResponse>>,
        IRequestHandler<GetProductByIdToEditQuery, Response<GetProductByIdToEditResponse>>,
        IRequestHandler<GetProductByIdWithVariantsQuery, Response<GetProductByIdWithVariantsResponse>>,
        IRequestHandler<GetProductPaginatedListQuery, PaginatedResult<GetProductPaginatedListResponse>>
    {
        #region Fields
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public ProductQueryHandler(IProductService productService, IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _productService = productService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handel Functions
        public async Task<Response<List<GetProductsListResponse>>> Handle(GetProductsListQuery request, CancellationToken cancellationToken)
        {
            var prodctsList = await _productService.GetProductsListWithIncludesAsync();
            var prodctsListMapper = _mapper.Map<List<GetProductsListResponse>>(prodctsList);

            foreach (var productDto in prodctsListMapper)
            {
                var product = prodctsList.First(p => p.Id == productDto.Id);

                productDto.PriceAfterDiscount = _productService.CalculateDiscountedPriceOnProduct(product) ?? product.BasePrice;
            }

            var result = Success(prodctsListMapper);
            result.Meta = new { Count = prodctsListMapper.Count() };
            return result;
        }

        public async Task<Response<GetProductByIdResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productService.GetByIdWithIncludesAsync(request.Id);

            if (product is null)
                return NotFound<GetProductByIdResponse>(_stringLocalizer[SharedResourcesKeys.ProductNotFound]);

            var result = _mapper.Map<GetProductByIdResponse>(product);

            result.PriceAfterDiscount = _productService.CalculateDiscountedPriceOnProduct(product) ?? product.BasePrice;

            return Success(result);
        }

        public async Task<Response<GetProductByIdToEditResponse>> Handle(GetProductByIdToEditQuery request, CancellationToken cancellationToken)
        {
            var product = await _productService.GetByIdWithIncludesAsync(request.Id);

            if (product is null)
                return NotFound<GetProductByIdToEditResponse>(_stringLocalizer[SharedResourcesKeys.ProductNotFound]);

            var result = _mapper.Map<GetProductByIdToEditResponse>(product);

            return Success(result);
        }

        public async Task<Response<GetProductByIdWithVariantsResponse>> Handle(GetProductByIdWithVariantsQuery request, CancellationToken cancellationToken)
        {
            var product = await _productService.GetByIdWithIncludesAsync(request.Id);

            if (product == null)
                return NotFound<GetProductByIdWithVariantsResponse>(_stringLocalizer[SharedResourcesKeys.ProductNotFound]);

            bool isArabic = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName.ToLower().Equals("ar");

            var response = new GetProductByIdWithVariantsResponse
            {
                Id = product.Id,
                Code = product.Code,
                Name = isArabic ? product.NameAr : product.NameEn,
                Description = isArabic ? product.DescriptionAr : product.DescriptionEn,
                Season = product.Season,
                Club = isArabic ? product.ClubAr : product.ClubEn,
                BrandName = product.Brand?.NameEn ?? string.Empty,
                CategoryName = product.Category?.NameEn ?? string.Empty,
                BasePrice = product.BasePrice,
                PriceAfterDiscount = _productService.CalculateDiscountedPriceOnProduct(product) ?? product.BasePrice,
                Images = product.Images.Select(i => i.Url).ToList(),
                Variants = product.Variants
                    .Where(v => !v.IsDeleted)
                    .Select(v => new ProductVariantResponse
                    {
                        Id = v.Id,
                        Size = v.Size,
                        Color = v.Color,
                        Price = v.Price,
                        PriceAfterDiscount = _productService.CalculateDiscountedPriceOnProductVariants(product, v.Price) ?? v.Price,  // الخصم على سعر المتغير
                        StockQuantity = v.StockQuantity
                    }).ToList()
            };

            return Success(response);

        }

        public async Task<PaginatedResult<GetProductPaginatedListResponse>> Handle(GetProductPaginatedListQuery request, CancellationToken cancellationToken)
        {
            //pagination
            var query = _productService.FilterProductPaginatedQueryable(request.Ordering, request.Search);

            bool isArabic = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName.ToLower().Equals("ar");

            var result = await query.Select(p => new GetProductPaginatedListResponse
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
