using FluentValidation;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Features.ProductAttributeTemplate.Commands.Models;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;

namespace Sportswear.Core.Features.ProductAttributeTemplate.Commands.Validations
{
    public class CreateAttributeTemplateRangeCommandValidator : AbstractValidator<CreateAttributeTemplateRangeCommand>
    {
        #region Fields
        private readonly ICategoryService _categoryService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public CreateAttributeTemplateRangeCommandValidator(
            ICategoryService categoryService,
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
            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage(_localizer[SharedResourcesKeys.Required]);

            RuleFor(x => x.Templates)
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty]);

            RuleForEach(x => x.Templates).ChildRules(template =>
            {
                template.RuleFor(t => t.KeyEn)
                    .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                    .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                    .MaximumLength(50).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs100])
                    .Matches(@"^[a-zA-Z\s]+$").WithMessage(_localizer[SharedResourcesKeys.EnglishOnly]);

                template.RuleFor(t => t.KeyAr)
                    .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                    .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                    .MaximumLength(50).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs100]);

                template.RuleFor(t => t.Type)
                    .IsInEnum().WithMessage(_localizer[SharedResourcesKeys.InvalidAttributeType]);
            });
        }

        public void ApplyCustomValidationsRules()
        {
            // تأكد إن الـ Category موجودة
            RuleFor(x => x.CategoryId)
                .MustAsync(async (categoryId, cancellationToken) =>
                    await _categoryService.IsCategoryIdExist(categoryId))
                .WithMessage(_localizer[SharedResourcesKeys.CategoryNotFound]);

            // تأكد مفيش duplicate في نفس الـ request
            RuleFor(x => x.Templates)
                .Must(templates =>
                {
                    var keys = templates.Select(t => t.KeyEn.ToLower()).ToList();
                    return keys.Count == keys.Distinct().Count();
                })
                .WithMessage(_localizer[SharedResourcesKeys.AttributeTemplateDuplicateInRequest]);
        }
        #endregion
    }
}
