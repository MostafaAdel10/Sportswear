using MediatR;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.ProductAttributeTemplate.Commands.Models;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;
using Sportswear.Service.AuthServices.Interfaces;

namespace Sportswear.Core.Features.ProductAttributeTemplate.Commands.Handlers
{
    public class ProductAttributeTemplateCommandHandler : ResponseHandler,
        IRequestHandler<CreateAttributeTemplateRangeCommand, Response<string>>,
        IRequestHandler<DeleteAttributeTemplateCommand, Response<string>>
    {
        #region Fields
        private readonly IProductAttributeTemplateService _templateService;
        private readonly ICategoryService _categoryService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public ProductAttributeTemplateCommandHandler(
            IProductAttributeTemplateService templateService,
            ICategoryService categoryService,
            ICurrentUserService currentUserService,
            IStringLocalizer<SharedResources> localizer) : base(localizer)
        {
            _templateService = templateService;
            _categoryService = categoryService;
            _currentUserService = currentUserService;
            _localizer = localizer;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<string>> Handle(CreateAttributeTemplateRangeCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetUserAsync();
            if (currentUser == null || string.IsNullOrEmpty(currentUser.UserName))
                return Unauthorized<string>();

            // تأكد إن الـ Category موجودة
            var category = await _categoryService.GetByIdAsync(request.CategoryId);
            if (category == null)
                return NotFound<string>(_localizer[SharedResourcesKeys.CategoryNotFound]);

            // جيب الـ templates الموجودة للـ Category
            var existingTemplates = await _templateService.GetByCategoryIdAsync(request.CategoryId);
            var existingKeys = existingTemplates
                .Select(t => t.KeyEn.ToLower())
                .ToHashSet();

            var requestKeys = new HashSet<string>();
            var templatesToAdd = new List<DataAccess.Entities.ProductAttributeTemplate>();

            foreach (var dto in request.Templates)
            {
                var key = dto.KeyEn.ToLower();

                // check duplicate في نفس الـ request
                if (!requestKeys.Add(key))
                    return BadRequest<string>(_localizer[SharedResourcesKeys.AttributeTemplateDuplicateInRequest]);

                // check duplicate مع الموجود في الـ DB
                if (existingKeys.Contains(key))
                    return BadRequest<string>(_localizer[SharedResourcesKeys.AttributeTemplateAlreadyExists]);

                templatesToAdd.Add(new DataAccess.Entities.ProductAttributeTemplate
                {
                    CategoryId = request.CategoryId,
                    KeyEn = dto.KeyEn,
                    KeyAr = dto.KeyAr,
                    Type = dto.Type,
                    CreatedBy = currentUser.UserName
                });
            }

            var isAdded = await _templateService.AddRangeAsync(templatesToAdd);

            return isAdded ? Created("") : BadRequest<string>();
        }

        public async Task<Response<string>> Handle(DeleteAttributeTemplateCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetUserAsync();
            if (currentUser == null || string.IsNullOrEmpty(currentUser.UserName))
                return Unauthorized<string>();

            var template = await _templateService.GetByIdAsync(request.Id);
            if (template == null)
                return NotFound<string>(_localizer[SharedResourcesKeys.AttributeTemplateNotFound]);

            // ✅ تأكد مفيش variants بتستخدم الـ template ده
            var hasVariants = await _templateService.HasVariantAttributesAsync(request.Id);
            if (hasVariants)
                return BadRequest<string>(_localizer[SharedResourcesKeys.CannotDeleteTemplateWithVariants]);

            var isDeleted = await _templateService.DeleteAsync(template);

            return isDeleted
                ? Success<string>(_localizer[SharedResourcesKeys.Deleted])
                : BadRequest<string>();
        }
        #endregion
    }
}
