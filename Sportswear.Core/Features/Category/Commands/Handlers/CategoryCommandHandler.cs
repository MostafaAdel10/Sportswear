using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Sportswear.Core.Bases;
using Sportswear.Core.Features.Category.Commands.Models;
using Sportswear.Core.Resources;
using Sportswear.Service.Abstract;
using Sportswear.Service.AuthServices.Interfaces;

namespace Sportswear.Core.Features.Category.Commands.Handlers
{
    public class CategoryCommandHandler : ResponseHandler,
                        IRequestHandler<CreateCategoryCommand, Response<string>>,
                        IRequestHandler<EditCategoryCommand, Response<string>>,
                        IRequestHandler<DeleteCategoryCommand, Response<string>>
    {
        #region Fields
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public CategoryCommandHandler(ICategoryService categoryService,
            IProductService productService,
            ICurrentUserService currentUserService,
            IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _categoryService = categoryService;
            _productService = productService;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handel Functions
        public async Task<Response<string>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetUserAsync();
            if (currentUser == null || string.IsNullOrEmpty(currentUser.UserName))
                return Unauthorized<string>();

            var category = _mapper.Map<DataAccess.Entities.Category>(request);

            category.CreatedBy = currentUser.UserName;

            var isSuccess = await _categoryService.AddAsync(category);

            if (isSuccess)
                return Created("");
            else
                return BadRequest<string>();
        }

        public async Task<Response<string>> Handle(EditCategoryCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetUserAsync();
            if (currentUser == null || string.IsNullOrEmpty(currentUser.UserName))
                return Unauthorized<string>();

            // Check if category exists
            var existingCategory = await _categoryService.GetByIdAsync(request.Id);
            if (existingCategory == null)
                return NotFound<string>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            // Map new values to existing entity
            existingCategory = _mapper.Map(request, existingCategory);

            existingCategory.UpdatedAt = DateTime.UtcNow;
            existingCategory.UpdatedBy = currentUser.UserName;

            var isSuccess = await _categoryService.EditAsync(existingCategory);

            if (isSuccess)
                return Success<string>(_stringLocalizer[SharedResourcesKeys.Updated]);
            else
                return BadRequest<string>();
        }

        public async Task<Response<string>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserService.GetUserAsync();
            if (currentUser == null || string.IsNullOrEmpty(currentUser.UserName))
                return Unauthorized<string>();

            // Check if Category exists
            var existingCategory = await _categoryService.GetByIdAsync(request.Id);
            if (existingCategory == null)
                return NotFound<string>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            // Check if Category has related products
            var hasProducts = await _productService.IsAnyProductRelatedToCategoryAsync(request.Id);
            if (hasProducts)
                return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.RelatedProducts]);

            // Soft delete
            existingCategory.IsDeleted = true;
            existingCategory.UpdatedAt = DateTime.UtcNow;
            existingCategory.UpdatedBy = currentUser.UserName;

            var isSuccess = await _categoryService.EditAsync(existingCategory);

            return isSuccess
                ? Success<string>(_stringLocalizer[SharedResourcesKeys.Deleted])
                : BadRequest<string>();
        }
        #endregion
    }
}
