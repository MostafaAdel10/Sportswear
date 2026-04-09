using FluentValidation;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Features.ProductVariant.Commands.Models;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;

namespace Sportswear.Core.Features.ProductVariant.Commands.Validations
{
    public class CreateProductVariantRangeCommandValidator : AbstractValidator<CreateProductVariantRangeCommand>
    {
        #region Fields
        private readonly IProductService _productService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public CreateProductVariantRangeCommandValidator(
            IProductService productService,
            IStringLocalizer<SharedResources> localizer)
        {
            _productService = productService;
            _localizer = localizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(x => x.Variants)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty]);

            RuleForEach(x => x.Variants).ChildRules(variant =>
            {
                variant.RuleFor(v => v.Price)
                    .GreaterThanOrEqualTo(0).WithMessage(_localizer[SharedResourcesKeys.MustBeGreaterThanZero]);

                variant.RuleFor(v => v.StockQuantity)
                    .GreaterThanOrEqualTo(0).WithMessage(_localizer[SharedResourcesKeys.MustBeGreaterThanZero]);

                // ✅ AttributeValueEn اختياري بس لو موجود MaxLength
                variant.RuleFor(v => v.AttributeValueEn)
                    .MaximumLength(200).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs200])
                    .When(v => !string.IsNullOrEmpty(v.AttributeValueEn));

                variant.RuleFor(v => v.AttributeValueAr)
                    .MaximumLength(200).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs200])
                    .When(v => !string.IsNullOrEmpty(v.AttributeValueAr));

                // ✅ Unit اختياري
                variant.RuleFor(v => v.Unit)
                    .MaximumLength(50).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs50])
                    .When(v => !string.IsNullOrEmpty(v.Unit));

                // ✅ ColorLabel اختياري
                variant.RuleFor(v => v.ColorLabel)
                    .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs100])
                    .When(v => !string.IsNullOrEmpty(v.ColorLabel));

                // ✅ ColorHex اختياري بس لو موجود لازم يكون صح
                variant.RuleFor(v => v.ColorHex)
                    .Matches(@"^#([A-Fa-f0-9]{6})$")
                    .WithMessage(_localizer[SharedResourcesKeys.InvalidColorHex])
                    .When(v => !string.IsNullOrEmpty(v.ColorHex));

                // ✅ لو ColorHex موجود لازم ColorLabel يكون موجود
                variant.RuleFor(v => v.ColorLabel)
                    .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required])
                    .When(v => !string.IsNullOrEmpty(v.ColorHex));
            });
        }

        public void ApplyCustomValidationsRules()
        {
            RuleFor(x => x.ProductId)
                .MustAsync(async (productId, cancellationToken) =>
                {
                    var product = await _productService.GetByIdAsync(productId);
                    return product != null;
                })
                .WithMessage(_localizer[SharedResourcesKeys.ProductNotFound]);
        }
        #endregion
    }
}