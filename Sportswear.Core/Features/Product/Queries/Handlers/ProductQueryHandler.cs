using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Product.Queries.Models;
using Sportswear.Core.Features.Product.Queries.Response_DTO_;
using Sportswear.Core.Resources;
using Sportswear.Core.Wrappers;
using Sportswear.Service.Abstract;
using Sportswear.Service.Implementations;

namespace Sportswear.Core.Features.Product.Queries.Handlers
{
    public class ProductQueryHandler : ResponseHandler,
        IRequestHandler<GetProductFullDetailsQuery, Response<GetProductFullDetailsResponse>>,
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
        private readonly ICacheService _cacheService;
        #endregion

        #region Constructors
        public ProductQueryHandler(IProductService productService, IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer, ICacheService cacheService) : base(stringLocalizer)
        {
            _productService = productService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
            _cacheService = cacheService;
        }
        #endregion

        #region Handel Functions
        public async Task<Response<GetProductFullDetailsResponse>> Handle(GetProductFullDetailsQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = string.Format(CacheKeys.ProductFullDetails, request.Id);

            // 1. Check Cache
            var cached = _cacheService.Get<GetProductFullDetailsResponse>(cacheKey);
            if (cached != null)
                return Success(cached);

            // 2. Pocket of DB
            var product = await _productService.GetProductWithIncludesFullDetailsAsync(request.Id);
            if (product == null)
                return NotFound<GetProductFullDetailsResponse>(
                    _stringLocalizer[SharedResourcesKeys.ProductNotFound]);

            var now = DateTime.UtcNow;

            var minPriceAfterDiscount = _productService.CalculateDiscountedPriceOnProductVariants(product, product.MinPrice) ?? product.MinPrice;
            var maxPriceAfterDiscount = _productService.CalculateDiscountedPriceOnProductVariants(product, product.MaxPrice) ?? product.MaxPrice;

            var response = new GetProductFullDetailsResponse
            {
                Id = product.Id,
                Code = product.Code,
                NameEn = product.NameEn,
                NameAr = product.NameAr,
                DescriptionEn = product.DescriptionEn,
                DescriptionAr = product.DescriptionAr,
                Season = product.Season,
                ClubEn = product.ClubEn,
                ClubAr = product.ClubAr,

                // Attribute Key
                AttributeKeyEn = product.AttributeKeyEn,
                AttributeKeyAr = product.AttributeKeyAr,

                // Brand
                BrandId = product.BrandId,
                BrandNameEn = product.Brand?.NameEn ?? string.Empty,
                BrandNameAr = product.Brand?.NameAr ?? string.Empty,

                // Category
                CategoryId = product.CategoryId,
                CategoryNameEn = product.Category?.NameEn ?? string.Empty,
                CategoryNameAr = product.Category?.NameAr ?? string.Empty,

                // Pricing
                BasePrice = product.BasePrice,
                MinPrice = product.MinPrice,
                MaxPrice = product.MaxPrice,
                HasVariants = product.HasVariants,
                PriceAfterDiscount = _productService.CalculateDiscountedPriceOnProduct(product) ?? product.BasePrice,
                MinPriceAfterDiscount = minPriceAfterDiscount,
                MaxPriceAfterDiscount = maxPriceAfterDiscount,

                // Images
                Images = product.Images
                    .Where(i => !i.IsDeleted)
                    .Select(i => i.Url)
                    .ToList(),

                // Variants With Attributes
                Variants = product.Variants
                    .Where(v => !v.IsDeleted)
                    .Select(v => new FullProductVariantDto
                    {
                        Id = v.Id,
                        SKU = v.SKU,
                        Price = v.Price,
                        PriceAfterDiscount = _productService.CalculateDiscountedPriceOnProductVariants(product, v.Price) ?? v.Price,
                        StockQuantity = v.StockQuantity,
                        AttributeValueAr = v.AttributeValueAr,
                        AttributeValueEn = v.AttributeValueEn,
                        Unit = v.Unit,
                        ColorLabel = v.ColorLabel,
                        ColorHex = v.ColorHex
                    }).ToList(),

                // Active and Expired Discounts
                Discounts = product.Product_Discounts
                    .Where(pd => !pd.Discount.IsDeleted)
                    .Select(pd => new ProductDiscountDto
                    {
                        NameEn = pd.Discount.NameEn,
                        NameAr = pd.Discount.NameAr,
                        Percentage = pd.Discount.Percentage,
                        StartDate = pd.Discount.StartDate,
                        EndDate = pd.Discount.EndDate,
                        IsActive = pd.Discount.StartDate <= now && pd.Discount.EndDate >= now
                    }).ToList(),

                // Reviews
                Reviews = product.Reviews
                    .Where(r => !r.IsDeleted)
                    .Select(r => new ProductReviewDto
                    {
                        Id = r.Id,
                        UserName = r.CreatedBy ?? string.Empty,
                        Rating = r.Rating,
                        Comment = r.Comment,
                        CreatedAt = r.CreatedAt
                    }).ToList(),

                AverageRating = product.Reviews.Any(r => !r.IsDeleted)
                    ? Math.Round(product.Reviews.Where(r => !r.IsDeleted).Average(r => r.Rating), 1)
                    : 0,

                ReviewsCount = product.Reviews.Count(r => !r.IsDeleted)
            };

            // 3. Save in Cache for 5 minutes (discounts may change during this time)
            _cacheService.Set(cacheKey, response, TimeSpan.FromMinutes(5));

            return Success(response);
        }

        public async Task<Response<List<GetProductsListResponse>>> Handle(GetProductsListQuery request, CancellationToken cancellationToken)
        {
            // 1. Check Cache
            var cached = _cacheService.Get<List<GetProductsListResponse>>(CacheKeys.ProductsList);
            if (cached != null)
                return Success(cached);

            // 2. Pocket of DB
            var productsList = await _productService.GetProductsListWithIncludesAsync();
            var mapped = _mapper.Map<List<GetProductsListResponse>>(productsList);

            foreach (var productDto in mapped)
            {
                var product = productsList.First(p => p.Id == productDto.Id);
                productDto.MinPrice = product.MinPrice;
                productDto.MaxPrice = product.MaxPrice;
                productDto.HasVariants = product.HasVariants;

                productDto.PriceAfterDiscount = _productService.CalculateDiscountedPriceOnProduct(product) ?? product.BasePrice;
                productDto.MinPriceAfterDiscount = _productService.CalculateDiscountedPriceOnProductVariants(product, product.MinPrice) ?? product.MinPrice;
                productDto.MaxPriceAfterDiscount = _productService.CalculateDiscountedPriceOnProductVariants(product, product.MaxPrice) ?? product.MaxPrice;
            }

            // 3. Save in Cache for 10 minutes
            _cacheService.Set(CacheKeys.ProductsList, mapped, TimeSpan.FromMinutes(10));

            var result = Success(mapped);
            result.Meta = new { Count = mapped.Count() };
            return result;
        }

        public async Task<Response<GetProductByIdResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = string.Format(CacheKeys.ProductById, request.Id);

            // 1. Check Cache
            var cached = _cacheService.Get<GetProductByIdResponse>(cacheKey);
            if (cached != null)
                return Success(cached);

            // 2. Pocket of DB
            var product = await _productService.GetByIdWithIncludesAsync(request.Id);
            if (product is null)
                return NotFound<GetProductByIdResponse>(_stringLocalizer[SharedResourcesKeys.ProductNotFound]);

            var mapped = _mapper.Map<GetProductByIdResponse>(product);
            mapped.MinPrice = product.MinPrice;
            mapped.MaxPrice = product.MaxPrice;
            mapped.HasVariants = product.HasVariants;

            mapped.PriceAfterDiscount = _productService.CalculateDiscountedPriceOnProduct(product) ?? product.BasePrice;
            mapped.MinPriceAfterDiscount = _productService.CalculateDiscountedPriceOnProductVariants(product, product.MinPrice) ?? product.MinPrice;
            mapped.MaxPriceAfterDiscount = _productService.CalculateDiscountedPriceOnProductVariants(product, product.MaxPrice) ?? product.MaxPrice;

            // 3. Save in Cache for 10 minutes
            _cacheService.Set(cacheKey, mapped, TimeSpan.FromMinutes(10));

            return Success(mapped);
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
            var cacheKey = string.Format(CacheKeys.ProductWithVariants, request.Id);

            // 1. Check Cache
            var cached = _cacheService.Get<GetProductByIdWithVariantsResponse>(cacheKey);
            if (cached != null)
                return Success(cached);

            // 2. Pocket of DB
            var product = await _productService.GetByIdWithIncludesAsync(request.Id);
            if (product == null)
                return NotFound<GetProductByIdWithVariantsResponse>(_stringLocalizer[SharedResourcesKeys.ProductNotFound]);

            bool isArabic = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName.ToLower().Equals("ar");

            var minPriceAfterDiscount = _productService.CalculateDiscountedPriceOnProductVariants(product, product.MinPrice) ?? product.MinPrice;
            var maxPriceAfterDiscount = _productService.CalculateDiscountedPriceOnProductVariants(product, product.MaxPrice) ?? product.MaxPrice;

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

                MinPrice = product.MinPrice,
                MaxPrice = product.MaxPrice,
                HasVariants = product.HasVariants,

                AttributeKey = isArabic ? product.AttributeKeyAr : product.AttributeKeyEn,

                PriceAfterDiscount = _productService.CalculateDiscountedPriceOnProduct(product) ?? product.BasePrice,
                MinPriceAfterDiscount = minPriceAfterDiscount,
                MaxPriceAfterDiscount = maxPriceAfterDiscount,

                Images = product.Images.Select(i => i.Url).ToList(),
                Variants = product.Variants.Where(v => !v.IsDeleted).Select(v => new ProductVariantResponse
                {
                    Id = v.Id,
                    SKU = v.SKU,
                    Price = v.Price,
                    PriceAfterDiscount = _productService.CalculateDiscountedPriceOnProductVariants(product, v.Price) ?? v.Price,
                    StockQuantity = v.StockQuantity,
                    AttributeValueEn = v.AttributeValueEn,
                    AttributeValueAr = v.AttributeValueAr,
                    Unit = v.Unit,
                    ColorLabel = v.ColorLabel,
                    ColorHex = v.ColorHex
                }).ToList()
            };

            // 3. Save in Cache for 5 minutes
            _cacheService.Set(cacheKey, response, TimeSpan.FromMinutes(5));

            return Success(response);
        }

        public async Task<PaginatedResult<GetProductPaginatedListResponse>> Handle(GetProductPaginatedListQuery request, CancellationToken cancellationToken)
        {
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
                AttributeKey = isArabic ? p.AttributeKeyAr : p.AttributeKeyEn,
                BasePrice = p.BasePrice,
                MinPrice = p.MinPrice,
                MaxPrice = p.MaxPrice,
                HasVariants = p.HasVariants,
                Season = p.Season,
                Images = p.Images.Select(i => i.Url).ToList()
            }).ToPaginatedListAsync(request.PageNumber, request.PageSize);

            // ✅ بعد ما الداتا رجعت من الـ DB نحسب الـ discounts 
            var products = await _productService.GetByIdsAsync(result.Data.Select(x => x.Id).ToList());

            foreach (var item in result.Data)
            {
                var product = products.First(p => p.Id == item.Id);


                item.PriceAfterDiscount = _productService.CalculateDiscountedPriceOnProduct(product) ?? product.BasePrice;
                item.MinPriceAfterDiscount = _productService.CalculateDiscountedPriceOnProductVariants(product, item.MinPrice) ?? item.MinPrice;
                item.MaxPriceAfterDiscount = _productService.CalculateDiscountedPriceOnProductVariants(product, item.MaxPrice) ?? item.MaxPrice;
            }

            result.Meta = new { Count = result.Data.Count() };
            return result;
        }
        #endregion
    }
}
