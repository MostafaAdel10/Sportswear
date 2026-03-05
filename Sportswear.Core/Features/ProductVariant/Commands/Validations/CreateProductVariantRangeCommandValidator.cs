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
        private readonly IProductAttributeTemplateService _templateService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public CreateProductVariantRangeCommandValidator(
            IProductService productService,
            IProductAttributeTemplateService templateService,
            IStringLocalizer<SharedResources> localizer)
        {
            _productService = productService;
            _templateService = templateService;
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

                variant.RuleFor(v => v.Attributes)
                    .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                    .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty]);

                variant.RuleForEach(v => v.Attributes).ChildRules(attr =>
                {
                    attr.RuleFor(a => a.TemplateId)
                        .GreaterThan(0).WithMessage(_localizer[SharedResourcesKeys.Required]);

                    attr.RuleFor(a => a.ValueEn)
                        .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                        .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                        .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs100]);

                    attr.RuleFor(a => a.ValueAr)
                        .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                        .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                        .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs100]);

                    attr.RuleFor(a => a.ColorHex)
                        .Matches(@"^#([A-Fa-f0-9]{6})$")
                        .WithMessage(_localizer[SharedResourcesKeys.InvalidColorHex])
                        .When(a => !string.IsNullOrEmpty(a.ColorHex));
                });
            });
        }

        public void ApplyCustomValidationsRules()
        {
            // تأكد إن المنتج موجود
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
