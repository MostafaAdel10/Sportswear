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

            RuleFor(x => x.Attributes)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty]);

            RuleForEach(x => x.Attributes).ChildRules(attr =>
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
        }

        public void ApplyCustomValidationsRules()
        {
            // تأكد إن الـ Variant موجود
            RuleFor(x => x.Id)
                .MustAsync(async (id, cancellationToken) =>
                    await _productVariantService.IsProductVariantExistsAsync(id))
                .WithMessage(_localizer[SharedResourcesKeys.VariantNotFound]);
        }
        #endregion
    }
}
