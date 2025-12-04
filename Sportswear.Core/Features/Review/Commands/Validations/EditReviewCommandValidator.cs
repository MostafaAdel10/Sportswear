using FluentValidation;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Features.Review.Commands.Models;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;

namespace Sportswear.Core.Features.Review.Commands.Validations
{
    public class EditReviewCommandValidator : AbstractValidator<EditReviewCommand>
    {
        #region Fields
        private readonly IReviewService _reviewService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public EditReviewCommandValidator(IReviewService reviewService, IStringLocalizer<SharedResources> localizer)
        {
            _reviewService = reviewService;
            _localizer = localizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(r => r.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage(_localizer[SharedResourcesKeys.RatingBetween1and5]);

            RuleFor(b => b.Comment)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty]);
        }

        public void ApplyCustomValidationsRules()
        {

        }
        #endregion
    }
}
