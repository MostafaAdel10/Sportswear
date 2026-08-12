using MediatR;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Category.Commands.Models;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;
using Sportswear.Service.AuthServices.Interfaces;
using Sportswear.Service.Implementations;

namespace Sportswear.Core.Features.Category.Commands.Handlers
{
    public class CategoryCommandHandler : ResponseHandler,
                        IRequestHandler<CreateCategoryCommand, Response<int>>,
                        IRequestHandler<EditCategoryCommand, Response<string>>,
                        IRequestHandler<DeleteCategoryCommand, Response<string>>
    {
        #region Fields
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IFileService _fileService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly ICacheService _cacheService;
        #endregion

        #region Constructors
        public CategoryCommandHandler(ICategoryService categoryService,
            IFileService fileService,
            IProductService productService,
            ICurrentUserService currentUserService,
            IStringLocalizer<SharedResources> stringLocalizer,
            ICacheService cacheService) : base(stringLocalizer)
        {
            _categoryService = categoryService;
            _productService = productService;
            _fileService = fileService;
            _currentUserService = currentUserService;
            _stringLocalizer = stringLocalizer;
            _cacheService = cacheService;
        }
        #endregion

        #region Handel Functions
        public async Task<Response<int>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetUserAsync();
            if (currentUser == null || string.IsNullOrEmpty(currentUser.UserName))
                return Unauthorized<int>();

            if (request.Image == null)
                return BadRequest<int>(_stringLocalizer[SharedResourcesKeys.NoImagesProvided]);

            var url = await _fileService.UploadImageAsync(request.Image, "category-images");

            if (url == null)
                return BadRequest<int>(_stringLocalizer[SharedResourcesKeys.FailedToUploadImage]);

            var category = new DataAccess.Entities.Category
            {
                NameEn = request.NameEn,
                NameAr = request.NameAr,
                Slug = request.NameEn.ToLowerInvariant(),
                ImageUrl = url,
                CreatedBy = currentUser.UserName
            };

            var categoryId = await _categoryService.AddAsync(category);

            if (categoryId > 0)
            {
                // ✅ Clear the cache to update
                _cacheService.Remove(CacheKeys.Categories);
                return Success(categoryId, _stringLocalizer[SharedResourcesKeys.Created]);
            }

            return BadRequest<int>();
        }

        public async Task<Response<string>> Handle(EditCategoryCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetUserAsync();
            if (currentUser == null || string.IsNullOrEmpty(currentUser.UserName))
                return Unauthorized<string>();

            var existingCategory = await _categoryService.GetByIdAsync(request.Id);
            if (existingCategory == null)
                return NotFound<string>(_stringLocalizer[SharedResourcesKeys.CategoryNotFound]);

            // لو في صورة جديدة
            if (request.Image != null)
            {
                var newUrl = await _fileService.ReplaceImageAsync(
                    existingCategory.ImageUrl,
                    request.Image,
                    "category-images");

                existingCategory.ImageUrl = newUrl;
            }

            // تحديث باقي البيانات
            existingCategory.NameEn = request.NameEn;
            existingCategory.NameAr = request.NameAr;
            existingCategory.Slug = request.NameEn.ToLowerInvariant();
            existingCategory.UpdatedAt = DateTime.UtcNow;
            existingCategory.UpdatedBy = currentUser.UserName;

            var isSuccess = await _categoryService.EditAsync(existingCategory);

            if (isSuccess)
            {
                // ✅ Clear the cache
                _cacheService.Remove(CacheKeys.Categories);
                _cacheService.Remove(string.Format(CacheKeys.CategoryById, request.Id));
                return Success<string>(_stringLocalizer[SharedResourcesKeys.Updated]);
            }

            return BadRequest<string>();
        }


        public async Task<Response<string>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetUserAsync();
            if (currentUser == null || string.IsNullOrEmpty(currentUser.UserName))
                return Unauthorized<string>();

            var existingCategory = await _categoryService.GetByIdAsync(request.Id);
            if (existingCategory == null)
                return NotFound<string>(_stringLocalizer[SharedResourcesKeys.CategoryNotFound]);

            // ✅ تأكد مفيش products تابعة للـ Category
            var hasProducts = await _productService.IsAnyProductRelatedToCategoryAsync(request.Id);
            if (hasProducts)
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.RelatedProducts]);

            // حذف الصورة من السيرفر
            _fileService.DeleteImage(existingCategory.ImageUrl);

            // Soft delete
            existingCategory.IsDeleted = true;
            existingCategory.UpdatedAt = DateTime.UtcNow;
            existingCategory.UpdatedBy = currentUser.UserName;

            var isSuccess = await _categoryService.EditAsync(existingCategory);

            if (isSuccess)
            {
                // ✅ Clear the cache
                _cacheService.Remove(CacheKeys.Categories);
                _cacheService.Remove(string.Format(CacheKeys.CategoryById, request.Id));
                return Success<string>(_stringLocalizer[SharedResourcesKeys.Deleted]);
            }

            return BadRequest<string>();
        }
        #endregion
    }
}
