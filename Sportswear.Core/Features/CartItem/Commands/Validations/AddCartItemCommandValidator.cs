using FluentValidation;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Features.CartItem.Commands.Models;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;

namespace Sportswear.Core.Features.CartItem.Commands.Validations
{
    public class AddCartItemCommandValidator : AbstractValidator<AddCartItemCommand>
    {
        #region Fields
        private readonly IProductVariantService _variantService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructor
        public AddCartItemCommandValidator(
            IProductVariantService variantService,
            IStringLocalizer<SharedResources> localizer)
        {
            _variantService = variantService;
            _localizer = localizer;

            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Validation Rules
        private void ApplyValidationsRules()
        {
            RuleFor(x => x.ProductVariantId)
               .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(x => x.Quantity)
               .GreaterThan(0).WithMessage(_localizer[SharedResourcesKeys.MustBeGreaterThanZero]);
        }

        private void ApplyCustomValidationsRules()
        {
            RuleFor(x => x.ProductVariantId)
                .MustAsync(async (id, _) => await _variantService.IsProductVariantExistsAsync(id))
                .WithMessage(_localizer[SharedResourcesKeys.VariantNotFound]);
        }
        #endregion
    }
}
