using FluentValidation;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Features.CartItem.Commands.Models;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;

namespace Sportswear.Core.Features.CartItem.Commands.Validations
{
    public class EditCartItemCommandValidator : AbstractValidator<EditCartItemCommand>
    {
        #region Fields
        private readonly ICartItemService _cartItemService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructor
        public EditCartItemCommandValidator(
            ICartItemService cartItemService,
            IStringLocalizer<SharedResources> localizer)
        {
            _cartItemService = cartItemService;
            _localizer = localizer;

            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Validation Rules
        private void ApplyValidationsRules()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage(_localizer[SharedResourcesKeys.MustBeGreaterThanZero]);
        }

        private void ApplyCustomValidationsRules()
        {
            RuleFor(x => x.Id)
                .MustAsync(async (id, _) => await _cartItemService.GetByIdAsync(id) != null)
                .WithMessage(_localizer[SharedResourcesKeys.CartItemNotFound]);
        }
        #endregion
    }
}
