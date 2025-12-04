using FluentValidation;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Features.Brand.Commands.Models;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;

namespace Sportswear.Core.Features.Brand.Commands.Validations
{
    public class UpdateBrandCommandValidator : AbstractValidator<EditBrandCommand>
    {
        #region Fields
        private readonly IBrandService _brandService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public UpdateBrandCommandValidator(IBrandService brandService,
                                           IStringLocalizer<SharedResources> localizer)
        {
            _brandService = brandService;
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

            RuleFor(x => x.NameEn)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(200).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs200]);

            RuleFor(x => x.NameAr)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(200).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs200]);
        }

        public void ApplyCustomValidationsRules()
        {
        }
        #endregion
    }
}
