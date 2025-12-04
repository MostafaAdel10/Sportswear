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
        private readonly IProductService _productService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public UpdateProductVariantCommandValidator(IProductVariantService productVariantService, IProductService productService,
                                           IStringLocalizer<SharedResources> localizer)
        {
            _productVariantService = productVariantService;
            _productService = productService;
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

            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(x => x.Price)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .GreaterThanOrEqualTo(0).WithMessage(_localizer[SharedResourcesKeys.BadRequest]);

            RuleFor(x => x.StockQuantity)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .GreaterThanOrEqualTo(0).WithMessage(_localizer[SharedResourcesKeys.BadRequest]);

            RuleFor(x => x.Color)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(50).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs50]);

            RuleFor(x => x.Size)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(50).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs50]);
        }

        public void ApplyCustomValidationsRules()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .MustAsync(async (productId, cancellation) =>
                    await _productService.GetByIdAsync(productId) is not null)
                .WithMessage(_localizer[SharedResourcesKeys.ProductNotFound]);
        }
        #endregion
    }
}
