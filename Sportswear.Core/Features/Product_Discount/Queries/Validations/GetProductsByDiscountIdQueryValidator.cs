using FluentValidation;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Features.Product_Discount.Queries.Models;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;

namespace Sportswear.Core.Features.Product_Discount.Queries.Validations
{
    public class GetProductsByDiscountIdQueryValidator : AbstractValidator<GetProductsByDiscountIdQuery>
    {
        #region Fields
        private readonly IProductService _productService;
        private readonly IDiscountService _discountService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public GetProductsByDiscountIdQueryValidator(IProductService productService,
                                                    IDiscountService discountService,
                                                    IStringLocalizer<SharedResources> localizer)
        {
            _productService = productService;
            _discountService = discountService;
            _localizer = localizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.DiscountId)
            .GreaterThan(0).WithMessage(_localizer[SharedResourcesKeys.MustBeGreaterThanZero])
            .MustAsync(async (discountId, cancellation) =>
            {
                var discount = await _discountService.GetByIdAsync(discountId);
                return discount != null && !discount.IsDeleted;
            }).WithMessage(_localizer[SharedResourcesKeys.DiscountNotExist]);
        }

        public void ApplyCustomValidationsRules()
        {
        }
        #endregion
    }
}
