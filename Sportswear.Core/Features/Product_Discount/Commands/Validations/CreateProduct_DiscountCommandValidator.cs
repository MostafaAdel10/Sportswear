using FluentValidation;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Features.Product_Discount.Commands.Models;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;

namespace Sportswear.Core.Features.Product_Discount.Commands.Validations
{
    public class CreateProduct_DiscountCommandValidator : AbstractValidator<AddDiscountToProductsCommand>
    {
        #region Fields
        private readonly IProductService _productService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public CreateProduct_DiscountCommandValidator(IProductService productService,
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

            RuleFor(x => x.ProductIds)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required])
                .Must(ids => ids.All(id => id > 0)).WithMessage(_localizer[SharedResourcesKeys.MustBeGreaterThanZero]);
        }

        public void ApplyCustomValidationsRules()
        {
        }
        #endregion
    }
}
