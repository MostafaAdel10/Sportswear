using FluentValidation;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Features.User.Commands.Models;
using Sportswear.Core.Resources;

namespace Sportswear.Core.Features.User.Commands.Validations
{
    public class EditUserValidator : AbstractValidator<EditUserCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public EditUserValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Handle Functions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.FirstName)
                 .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                 .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                 .MaximumLength(25).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs25]);
            RuleFor(x => x.LastName)
                 .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                 .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                 .MaximumLength(25).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs25]);


            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required])
                .MaximumLength(100).WithMessage(_localizer[SharedResourcesKeys.MaxLengthIs100]);

            RuleFor(x => x.Email)
                 .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                 .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);

        }

        public void ApplyCustomValidationsRules()
        {

        }

        #endregion
    }
}
