using FluentValidation;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Features.ProductVariant.Commands.Models;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;

namespace Sportswear.Core.Features.ProductVariant.Commands.Validations
{
    public class CreateProductVariantRangeCommandValidator
        : AbstractValidator<CreateProductVariantRangeCommand>
    {
        private readonly IProductService _productService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public CreateProductVariantRangeCommandValidator(
            IProductService productService,
            IStringLocalizer<SharedResources> localizer)
        {
            _productService = productService;
            _localizer = localizer;

            ApplyValidationRules();
            ApplyCustomValidationRules();
        }

        public void ApplyValidationRules()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(x => x.Variants)
                .NotEmpty()
                .WithMessage(_localizer[SharedResourcesKeys.NotEmpty]);

            RuleForEach(x => x.Variants).ChildRules(variant =>
            {
                variant.RuleFor(v => v.Price)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage(_localizer[SharedResourcesKeys.BadRequest]);

                variant.RuleFor(v => v.StockQuantity)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage(_localizer[SharedResourcesKeys.BadRequest]);

                variant.RuleFor(v => v.ColorName)
                    .NotEmpty()
                    .MaximumLength(50)
                    .WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs50]);

                variant.RuleFor(v => v.ColorHex)
                    .NotEmpty()
                    .MaximumLength(10)
                    .WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs10]);

                variant.RuleFor(v => v.Size)
                    .NotEmpty()
                    .MaximumLength(50)
                    .WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs50]);
            });
        }

        public void ApplyCustomValidationRules()
        {
            RuleFor(x => x.ProductId)
                .MustAsync(async (productId, cancellation) =>
                    await _productService.GetByIdAsync(productId) is not null)
                .WithMessage(_localizer[SharedResourcesKeys.ProductNotFound]);
        }
    }
}
