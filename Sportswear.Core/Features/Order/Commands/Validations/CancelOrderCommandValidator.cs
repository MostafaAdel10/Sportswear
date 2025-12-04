using FluentValidation;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Features.Order.Commands.Models;
using Sportswear.Core.Resources;

namespace Sportswear.Core.Features.Order.Commands.Validations
{
    public class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
    {
        public CancelOrderCommandValidator(IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0)
                .WithMessage(localizer[SharedResourcesKeys.Required]);
        }
    }
}
