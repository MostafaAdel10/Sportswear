using FluentValidation;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Features.Product.Commands.Models;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;

namespace Sportswear.Core.Features.Product.Commands.Validations
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        #region Fields
        private readonly IProductService _productService;
        private readonly IBrandService _brandService;
        private readonly ICategoryService _categoryService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public CreateProductCommandValidator(IProductService productService,
                                             IBrandService brandService,
                                             ICategoryService categoryService,
                                             IStringLocalizer<SharedResources> localizer)
        {
            _productService = productService;
            _brandService = brandService;
            _categoryService = categoryService;
            _localizer = localizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.Code)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs100]);

            RuleFor(x => x.NameEn)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(200).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs200]);

            RuleFor(x => x.NameAr)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .MaximumLength(200).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs200]);

            RuleFor(x => x.DescriptionEn)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty]);

            RuleFor(x => x.DescriptionAr)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty]);

            RuleFor(x => x.ClubEn)
                .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs100]);

            RuleFor(x => x.ClubAr)
                .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs100]);

            RuleFor(x => x.BasePrice)
                .GreaterThanOrEqualTo(0).WithMessage(_localizer[SharedResourcesKeys.MustBeGreaterThanZero]);

            RuleFor(x => x.BrandId)
                .GreaterThan(0).WithMessage(_localizer[SharedResourcesKeys.MustBeGreaterThanZero]);

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage(_localizer[SharedResourcesKeys.MustBeGreaterThanZero]);
        }

        public void ApplyCustomValidationsRules()
        {
            RuleFor(x => x.BrandId)
               .MustAsync(async (Key, CancellationToken) => await _brandService.IsBrandIdExist(Key))
               .WithMessage(_localizer[SharedResourcesKeys.IsNotExist]);
            RuleFor(x => x.CategoryId)
               .MustAsync(async (Key, CancellationToken) => await _categoryService.IsCategoryIdExist(Key))
               .WithMessage(_localizer[SharedResourcesKeys.IsNotExist]);
            RuleFor(x => x.Code)
               .MustAsync(async (Key, CancellationToken) => !await _productService.IsCodeExistsAsync(Key))
               .WithMessage(_localizer[SharedResourcesKeys.CodeAlreadyExists]);
        }
        #endregion
    }
}
