using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Review.Commands.Models;
using Sportswear.Core.Features.Review.Queries.Response_DTO_;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;
using Sportswear.Service.AuthServices.Interfaces;

namespace Sportswear.Core.Features.Review.Commands.Handlers
{
    public class ReviewCommandHandler : ResponseHandler,
                IRequestHandler<AddReviewCommand, Response<ReviewDto>>,
                IRequestHandler<EditReviewCommand, Response<ReviewDto>>,
                IRequestHandler<DeleteReviewCommand, Response<string>>
    {
        #region Fields
        private readonly IReviewService _reviewService;
        private readonly IProductService _productService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public ReviewCommandHandler(IReviewService reviewService,
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
        public async Task<Response<ReviewDto>> Handle(AddReviewCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.GetUserId();

            var product = await _productService.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return NotFound<ReviewDto>(_localizer[SharedResourcesKeys.ProductNotFound]);
            }

            var review = _mapper.Map<DataAccess.Entities.Review>(request);
            review.UserId = currentUserId;

            var addedReview = await _reviewService.AddAsync(review);

            var reviewDto = _mapper.Map<ReviewDto>(review);

            return Created(reviewDto);
        }

        public async Task<Response<ReviewDto>> Handle(EditReviewCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.GetUserId();

            var review = await _reviewService.GetByIdAsync(request.Id);
            if (review == null)
            {
                return NotFound<ReviewDto>(_localizer[SharedResourcesKeys.NotFound]);
            }

            if (review.UserId != currentUserId)
            {
                return Unauthorized<ReviewDto>(_localizer[SharedResourcesKeys.YouAreNotAuthorizedToUpdateThisReview]);
            }

            _mapper.Map(request, review);

            var updatedReview = await _reviewService.EditAsync(review);

            var reviewDto = _mapper.Map<ReviewDto>(review);

            return updatedReview
            ? Success(reviewDto, _localizer[SharedResourcesKeys.Updated])
            : BadRequest<ReviewDto>(_localizer[SharedResourcesKeys.FailedToUpdate]);
        }

        public async Task<Response<string>> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.GetUserId();

            var review = await _reviewService.GetByIdAsync(request.Id);
            if (review == null)
            {
                return NotFound<string>(_localizer[SharedResourcesKeys.NotFound]);
            }

            if (review.UserId != currentUserId)
            {
                return Unauthorized<string>(_localizer[SharedResourcesKeys.YouAreNotAuthorizedToDeleteThisReview]);
            }

            var result = await _reviewService.DeleteAsync(review);

            return result
            ? Deleted<string>()
            : BadRequest<string>(_localizer[SharedResourcesKeys.FailedToDelete]);
        }

        #endregion
    }
}
