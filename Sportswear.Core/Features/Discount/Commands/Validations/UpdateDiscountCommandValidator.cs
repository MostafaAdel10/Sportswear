using FluentValidation;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Features.Discount.Commands.Models;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;

namespace Sportswear.Core.Features.Discount.Commands.Validations
{
    public class UpdateDiscountCommandValidator : AbstractValidator<EditDiscountCommand>
    {
        #region Fields
        private readonly IDiscountService _discountService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public UpdateDiscountCommandValidator(IDiscountService discountService,
                                           IStringLocalizer<SharedResources> localizer)
        {
            _discountService = discountService;
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

            RuleFor(x => x.Code)
            .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
            .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
            .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs100]);

            RuleFor(x => x.NameEn)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(200).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs200]);

            RuleFor(x => x.NameAr)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(200).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs200]);

            RuleFor(x => x.Percentage)
            .GreaterThan(0).WithMessage(_localizer[SharedResourcesKeys.MustBeGreaterThanZero])
            .LessThanOrEqualTo(100).WithMessage(_localizer[SharedResourcesKeys.MustBeLessThanOrEqual100]);

            RuleFor(x => x.StartDate)
                .LessThan(x => x.EndDate)
                .WithMessage(_localizer[SharedResourcesKeys.StartDateMustBeBeforeEndDate]);

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .WithMessage(_localizer[SharedResourcesKeys.EndDateMustBeAfterStartDate]);

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage(_localizer[SharedResourcesKeys.InvalidEnumValue]);

        }

        public void ApplyCustomValidationsRules()
        {
            RuleFor(x => x.Code)
               .MustAsync(async (Model, Key, CancellationToken) => !await _discountService.IsCodeExistsExcludeSelfAsync(Key, Model.Id))
               .WithMessage(_localizer[SharedResourcesKeys.CodeAlreadyExists]);
        }
        #endregion
    }
}
