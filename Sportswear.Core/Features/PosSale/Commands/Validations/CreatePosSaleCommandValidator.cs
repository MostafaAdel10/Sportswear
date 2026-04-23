using FluentValidation;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Features.PosSale.Commands.Models;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;

namespace Sportswear.Core.Features.PosSale.Commands.Validations
{
    public class CreatePosSaleCommandValidator : AbstractValidator<CreatePosSaleCommand>
    {
        private readonly IProductVariantService _productVariantService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public CreatePosSaleCommandValidator(
            IProductVariantService productVariantService,
            IStringLocalizer<SharedResources> localizer)
        {
            _productVariantService = productVariantService;
            _localizer = localizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }

        private void ApplyValidationsRules()
        {
            RuleFor(x => x.Items)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty]);

            RuleFor(x => x.PaymentMethod)
                .IsInEnum().WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(x => x.ProductVariantId)
                    .GreaterThan(0).WithMessage(_localizer[SharedResourcesKeys.Required]);

                item.RuleFor(x => x.Quantity)
                    .GreaterThan(0).WithMessage(_localizer[SharedResourcesKeys.Required]);
            });
        }

        private void ApplyCustomValidationsRules()
        {
            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(x => x.ProductVariantId)
                    .MustAsync(async (id, _) =>
                        await _productVariantService.IsProductVariantExistsAsync(id))
                    .WithMessage(_localizer[SharedResourcesKeys.VariantNotFound]);
            });
        }
    }
}
