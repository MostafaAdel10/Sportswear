using FluentValidation;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Features.Order.Commands.Models;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;
using Sportswear.Service.AuthServices.Interfaces;

namespace Sportswear.Core.Features.Order.Commands.Validations
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        #region Fields
        private readonly ICurrentUserService _currentUserService;
        private readonly ICartItemService _cartItemService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public CreateOrderCommandValidator(ICurrentUserService currentUserService,
                                           ICartItemService cartItemService,
                                           IStringLocalizer<SharedResources> localizer)
        {
            _currentUserService = currentUserService;
            _cartItemService = cartItemService;
            _localizer = localizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Actions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.ShippingMethodId)
               .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
               .GreaterThan(0).WithMessage(_localizer[SharedResourcesKeys.MustBeGreaterThanZero]);

            RuleFor(x => x.Shipment)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);

            When(x => x.Shipment != null, () =>
            {
                RuleFor(x => x.Shipment.FullName)
                    .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required])
                    .MaximumLength(200).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs200]);

                RuleFor(x => x.Shipment.City)
                    .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required])
                    .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs100]);

                RuleFor(x => x.Shipment.Country)
                    .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required])
                    .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs100]);

                RuleFor(x => x.Shipment.Region)
                    .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs100]);

                RuleFor(x => x.Shipment.StreetAddress)
                    .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required])
                    .MaximumLength(300).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs300]);

                RuleFor(x => x.Shipment.BuildingNumber)
                    .GreaterThan(0)
                    .When(x => x.Shipment.BuildingNumber.HasValue)
                    .WithMessage(_localizer[SharedResourcesKeys.MustBeGreaterThanZero]);

                RuleFor(x => x.Shipment.FloorNumber)
                    .GreaterThan(0)
                    .When(x => x.Shipment.FloorNumber.HasValue)
                    .WithMessage(_localizer[SharedResourcesKeys.MustBeGreaterThanZero]);

                RuleFor(x => x.Shipment.ApartmentNumber)
                    .GreaterThan(0)
                    .When(x => x.Shipment.ApartmentNumber.HasValue)
                    .WithMessage(_localizer[SharedResourcesKeys.MustBeGreaterThanZero]);

                RuleFor(x => x.Shipment.PhoneNumber)
                    .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required])
                    .MaximumLength(20).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs20])
                    .Matches(@"^[0-9+\-\s]+$")
                    .WithMessage(_localizer[SharedResourcesKeys.InvalidPhone]);
            });
        }

        public void ApplyCustomValidationsRules()
        {
        }
        #endregion
    }
}
