using FluentValidation;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Features.Order.Commands.Models;
using Sportswear.Core.Resources;

namespace Sportswear.Core.Features.Order.Commands.Validations
{
    public class ChangeOrderStatusCommandValidator : AbstractValidator<ChangeOrderStatusCommand>
    {
        public ChangeOrderStatusCommandValidator(IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0)
                .WithMessage(localizer[SharedResourcesKeys.Required]);

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage(localizer[SharedResourcesKeys.InvalidStatus]);
        }
    }
}
