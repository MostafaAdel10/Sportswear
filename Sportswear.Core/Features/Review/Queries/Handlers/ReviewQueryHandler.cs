using MediatR;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Review.Queries.Models;
using Sportswear.Core.Features.Review.Queries.Response_DTO_;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;

namespace Sportswear.Core.Features.Review.Queries.Handlers
{
    public class ReviewQueryHandler : ResponseHandler,
                IRequestHandler<GetReviewByIdQuery, Response<ReviewDto>>,
                IRequestHandler<GetReviewsByProductIdQuery, Response<List<ReviewDto>>>
    {
        #region Fields
        private readonly IReviewService _reviewService;
        private readonly IProductService _productService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public ReviewQueryHandler(IReviewService reviewService,
                                    IProductService productService,
                                    IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _reviewService = reviewService;
            _productService = productService;
            _localizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<ReviewDto>> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
        {
            var review = await _reviewService.GetByIdWithIncludesAsync(request.Id);
            if (review == null)
                return NotFound<ReviewDto>(_localizer[SharedResourcesKeys.NotFound]);

            bool isArabic = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName.ToLower().Equals("ar");

            var dto = new ReviewDto
            {
                Id = review.Id,
                ProductId = review.ProductId,
                ProductName = isArabic ? review.Product?.NameAr : review.Product?.NameEn,
                UserId = review.UserId,
                UserName = review.User?.UserName ?? string.Empty,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            };

            return Success(dto);
        }

        public async Task<Response<List<ReviewDto>>> Handle(GetReviewsByProductIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productService.GetByIdAsync(request.ProductId);
            if (product == null)
                return NotFound<List<ReviewDto>>(_localizer[SharedResourcesKeys.ProductNotFound]);

            bool isArabic = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName.ToLower().Equals("ar");

            var reviews = await _reviewService.GetReviewsByProductIdAsync(request.ProductId);

            var dtos = reviews.Select(r => new ReviewDto
            {
                Id = r.Id,
                ProductId = r.ProductId,
                ProductName = isArabic ? r.Product?.NameAr : r.Product?.NameEn,
                UserId = r.UserId,
                UserName = r.User?.UserName ?? string.Empty,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            }).ToList();

            return Success(dtos);
        }
        #endregion
    }
}
