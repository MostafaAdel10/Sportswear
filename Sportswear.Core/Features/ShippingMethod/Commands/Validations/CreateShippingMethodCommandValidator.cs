using FluentValidation;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Features.ShippingMethod.Commands.Models;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;

namespace Sportswear.Core.Features.ShippingMethod.Commands.Validations
{
    public class CreateShippingMethodCommandValidator : AbstractValidator<CreateShippingMethodCommand>
    {
        #region Fields
        private readonly IShippingMethodService _shippingMethodService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public CreateShippingMethodCommandValidator(IShippingMethodService shippingMethodService,
                                           IStringLocalizer<SharedResources> localizer)
        {
            _shippingMethodService = shippingMethodService;
            _localizer = localizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.NameEn)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs100]);

            RuleFor(x => x.NameAr)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs100]);

            RuleFor(x => x.DescriptionEn)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(400).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs400]);

            RuleFor(x => x.DescriptionAr)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(400).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs400]);

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage(_localizer[SharedResourcesKeys.MustBeGreaterThanZero]);

            RuleFor(x => x.EstimatedDeliveryDays)
                .GreaterThanOrEqualTo(0).WithMessage(_localizer[SharedResourcesKeys.MustBeGreaterThanZero]);
        }

        public void ApplyCustomValidationsRules()
        {
        }
        #endregion
    }
}
