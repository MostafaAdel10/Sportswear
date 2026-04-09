using FluentValidation;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Features.ProductVariant.Commands.Models;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;

namespace Sportswear.Core.Features.ProductVariant.Commands.Validations
{
    public class UpdateProductVariantCommandValidator : AbstractValidator<EditProductVariantCommand>
    {
        #region Fields
        private readonly IProductVariantService _productVariantService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public UpdateProductVariantCommandValidator(
            IProductVariantService productVariantService,
            IStringLocalizer<SharedResources> localizer)
        {
            _productVariantService = productVariantService;
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

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage(_localizer[SharedResourcesKeys.MustBeGreaterThanZero]);

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage(_localizer[SharedResourcesKeys.MustBeGreaterThanZero]);

            // ✅ AttributeValueEn اختياري
            RuleFor(x => x.AttributeValueEn)
                .MaximumLength(200).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs200])
                .When(x => !string.IsNullOrEmpty(x.AttributeValueEn));

            RuleFor(x => x.AttributeValueAr)
                .MaximumLength(200).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs200])
                .When(x => !string.IsNullOrEmpty(x.AttributeValueAr));

            // ✅ Unit اختياري
            RuleFor(x => x.Unit)
                .MaximumLength(50).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs50])
                .When(x => !string.IsNullOrEmpty(x.Unit));

            // ✅ ColorLabel اختياري
            RuleFor(x => x.ColorLabel)
                .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs100])
                .When(x => !string.IsNullOrEmpty(x.ColorLabel));

            // ✅ ColorHex اختياري بس لو موجود لازم يكون صح
            RuleFor(x => x.ColorHex)
                .Matches(@"^#([A-Fa-f0-9]{6})$")
                .WithMessage(_localizer[SharedResourcesKeys.InvalidColorHex])
                .When(x => !string.IsNullOrEmpty(x.ColorHex));

            // ✅ لو ColorHex موجود لازم ColorLabel يكون موجود
            RuleFor(x => x.ColorLabel)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required])
                .When(x => !string.IsNullOrEmpty(x.ColorHex));
        }

        public void ApplyCustomValidationsRules()
        {
            RuleFor(x => x.Id)
                .MustAsync(async (id, cancellationToken) =>
                    await _productVariantService.IsProductVariantExistsAsync(id))
                .WithMessage(_localizer[SharedResourcesKeys.VariantNotFound]);
        }
        #endregion
    }
}