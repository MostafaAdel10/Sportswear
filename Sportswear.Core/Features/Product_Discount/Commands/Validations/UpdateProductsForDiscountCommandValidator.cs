using FluentValidation;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Features.Product_Discount.Commands.Models;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;

namespace Sportswear.Core.Features.Product_Discount.Commands.Validations
{
    public class UpdateProductsForDiscountCommandValidator : AbstractValidator<UpdateProductsForDiscountCommand>
    {
        #region Fields
        private readonly IProductService _productService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public UpdateProductsForDiscountCommandValidator(IProductService productService,
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
            RuleFor(x => x.DiscountId)
            .GreaterThan(0).WithMessage(_localizer[SharedResourcesKeys.MustBeGreaterThanZero]);

            RuleFor(x => x.NewProductIds)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                .Must(ids => ids == null || ids.All(id => id > 0)).WithMessage(_localizer[SharedResourcesKeys.MustBeGreaterThanZero]);
        }

        public void ApplyCustomValidationsRules()
        {
        }
        #endregion
    }
}
