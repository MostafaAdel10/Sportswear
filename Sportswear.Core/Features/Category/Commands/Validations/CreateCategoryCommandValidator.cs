using FluentValidation;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Features.Category.Commands.Models;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;

namespace Sportswear.Core.Features.Category.Commands.Validations
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        #region Fields
        private readonly ICategoryService _categoryService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public CreateCategoryCommandValidator(ICategoryService categoryService,
                                           IStringLocalizer<SharedResources> localizer)
        {
            _categoryService = categoryService;
            _localizer = localizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.NameEn)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(200).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs200]);

            RuleFor(x => x.NameAr)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(200).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs200]);

            RuleFor(x => x.Image)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.Required])
                .Must(img => img != null && img.Length > 0).WithMessage(_localizer[SharedResourcesKeys.InvalidImagesProvided]);
        }

        public void ApplyCustomValidationsRules()
        {
        }
        #endregion
    }
}
