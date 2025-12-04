using FluentValidation;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Features.ProductImage.Commands.Models;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;

namespace Sportswear.Core.Features.ProductImage.Commands.Validations
{
    public class AddProductImagesCommandValidator : AbstractValidator<AddProductImagesCommand>
    {
        #region Fields
        private readonly IProductImageService _productImageService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public AddProductImagesCommandValidator(IProductImageService productImageService,
                                           IStringLocalizer<SharedResources> localizer)
        {
            _productImageService = productImageService;
            _localizer = localizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required])
                .GreaterThan(0).WithMessage(_localizer[SharedResourcesKeys.MustBeGreaterThanZero]);

            RuleFor(x => x.Images)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.AtLeastOneImageIsRequired])
                .Must(images => images.All(img => img != null && img.Length > 0)).WithMessage(_localizer[SharedResourcesKeys.InvalidImagesProvided]);
        }

        public void ApplyCustomValidationsRules()
        {
        }
        #endregion
    }
}
