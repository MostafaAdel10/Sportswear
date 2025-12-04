using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Review.Queries.Models;
using Sportswear.Core.Features.Review.Queries.Response_DTO_;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;
using Sportswear.Service.AuthServices.Interfaces;

namespace Sportswear.Core.Features.Review.Queries.Handlers
{
    public class ReviewQueryHandler : ResponseHandler,
                IRequestHandler<GetReviewByIdQuery, Response<ReviewDto>>,
                IRequestHandler<GetReviewsByProductIdQuery, Response<List<ReviewDto>>>
    {
        #region Fields
        private readonly IReviewService _reviewService;
        private readonly IProductService _productService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public ReviewQueryHandler(IReviewService reviewService,
                                    IProductService productService,
                                    ICurrentUserService currentUserService,
                                    IMapper mapper,
                                    IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _reviewService = reviewService;
            _productService = productService;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _localizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<ReviewDto>> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
        {
            var review = await _reviewService.GetByIdAsync(request.Id);
            if (review == null)
            {
                return NotFound<ReviewDto>(_localizer[SharedResourcesKeys.NotFound]);
            }

            var reviewDto = _mapper.Map<ReviewDto>(review);

            return Success(reviewDto);
        }

        public async Task<Response<List<ReviewDto>>> Handle(GetReviewsByProductIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productService.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return NotFound<List<ReviewDto>>(_localizer[SharedResourcesKeys.ProductNotFound]);
            }

            var reviews = await _reviewService.GetReviewsByProductIdAsync(request.ProductId);

            var reviewDtos = _mapper.Map<List<ReviewDto>>(reviews);

            return Success(reviewDtos);
        }
        #endregion
    }
}
